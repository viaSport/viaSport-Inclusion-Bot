using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using viaSportResourceBot.Models;

namespace viaSportResourceBot.Dialogs
{
    using System.Configuration;

    [LuisModel("LUIS APP ID", "SUBSCRIPTION KEY")]
    [Serializable]
    public class ViaSportDialog : LuisDialog<object>
    {
        private bool _doWebSearch = false;
        private const string _language = "en-us";
        private const int _maxReferences = 4;
        private const double AcceptableScore = .25;
        private bool respondToLuis = false;
        private string intent;
        private double intentScore;
        private string query;
        private bool sportSkip = false;
        private bool disabilitySkip = false;
        private bool subjectSkip = false;
        private Dictionary<string, EntityRecommendation> _entities = new Dictionary<string, EntityRecommendation>();
        private string temporaryEntity;
        private string newMessage;
        private string temporaryEntityType;

        // Needed for paging
        private int pageNumber = 0;
        private int numberOfAvailableReferences = 0;

        [NonSerialized()]
        private TelemetryClient telemetry = new TelemetryClient();

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            this.query = result.Query;
            await context.LoadAsync(new CancellationToken());
            PostTelemetryCustomEvent("none", 0, false);
            string message = $"Sorry I did not understand you: ";
            if (result.Query.ToLower() == "version")
            {
                message = "viaSport Disability Resource Bot, version: "
                          + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                await context.PostAsync(BotDbStrings.MakeItAcceptable(message));
                context.Wait(MessageReceived);
            }
            else if (result != null && result.Query != null)
            {
                await ConfirmNoneIntent(context, result);
            }
        }

        [LuisIntent("viasport.intent.hello")]
        public async Task Hello(IDialogContext context, LuisResult result)
        {
            PostTelemetryCustomEvent("hello", 0, false);
            BotDB botDB = new BotDB();
            var message = botDB.GetString(null, "viasport.intent.hello.greeting");
            BotDbAnalytics.UpdateAnalyticDatabase(result.Intents[0].Intent, (double)result.Intents[0].Score);
            await context.PostAsync(BotDbStrings.MakeItAcceptable(message));
            context.Wait(MessageReceived);
        }

        [LuisIntent("viasport.intent.help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            PostTelemetryCustomEvent("hello", 0, false);
            BotDB botDB = new BotDB();
            var message = string.Format(botDB.GetString(null, "viasport.intent.help.greeting"), "\n", "\n", "\n");
            BotDbAnalytics.UpdateAnalyticDatabase(result.Intents[0].Intent, (double)result.Intents[0].Score);
            await context.PostAsync(BotDbStrings.MakeItAcceptable(message));
            context.Wait(MessageReceived);
        }


        [LuisIntent("viasport.intent.howtocoach")]
        public async Task HowToCoach(IDialogContext context, LuisResult result)
        {
            if (result.Intents[0].Score < AcceptableScore)
            {
                await this.None(context, result);
            }
            else
            {
                await LoadAllEntitiesAsync(context, result);
                query = result.Query;
                intent = result.Intents[0].Intent;
                intentScore = (double)result.Intents[0].Score;
                PostTelemetryCustomEvent("howtocoach", 0, false);
                respondToLuis = true;

                if (respondToLuis)
                {
                    await CheckResultsAsync(context);
                }
            }
        }
      
        [LuisIntent("viasport.intent.findresource")]
        public async Task FindResource(IDialogContext context, LuisResult result)
        {
            if (result.Intents[0].Score < AcceptableScore)
            {
                await this.None(context, result);
            }
            else
            {
                await LoadAllEntitiesAsync(context, result);
                query = result.Query;
                intent = "viasport.intent.findresource";
                intentScore = (double)result.Intents[0].Score;
                PostTelemetryCustomEvent("findresource", 0, false);
                var message = context.MakeMessage();
                respondToLuis = true;

                if (respondToLuis)
                {
                    await CheckResultsAsync(context);
                }
            }
        }

        [LuisIntent("viasport.intent.findprogram")]
        public async Task FindProgram(IDialogContext context, LuisResult result)
        {
            if (result.Intents[0].Score < AcceptableScore)
            {
                await this.None(context, result);
            }
            else
            {
                await LoadAllEntitiesAsync(context, result);
                query = result.Query;
                intent = "viasport.intent.findprogram";
                intentScore = (double)result.Intents[0].Score;
                PostTelemetryCustomEvent("findprogram", 0, false);
                respondToLuis = true;

                if (respondToLuis)
                {
                    await CheckResultsAsync(context);
                }
            }
        }

        private async Task ConfirmNoneIntent(IDialogContext context, LuisResult result)
        {
            var checkedPhrase = await SpellCheck(result.Query);

            var suggestions = string.Empty;
            this.newMessage = this.query;
            foreach (var phrase in checkedPhrase)
            {
                var oldWord = phrase.token.Replace("Wrong Word : ", string.Empty);
                this.newMessage = this.newMessage.Replace(oldWord, phrase.suggestion);
            }

            if (this.newMessage == this.query)
            {
                BotDbAnalytics.UpdateAnalyticDatabase();
                BotDB botDB = new BotDB();
                var message = string.Format(botDB.GetString(null, "none"), result.Query);
                await context.PostAsync(BotDbStrings.MakeItAcceptable(message));
                context.Wait(this.MessageReceived);
            }
            else
            {
                PromptDialog.Confirm(
                    context,
                    this.OnSpellCheckIntent,
                    $"Did you mean '{this.newMessage}'?",
                    "Didn't get that!",
                    promptStyle: PromptStyle.None);
            }
        }

        private async Task OnSpellCheckIntent(IDialogContext context, IAwaitable<bool> result)
        {
            var accepted = (await result);
            if (accepted)
            {
                var uri = this.services[0].BuildUri(new LuisRequest(this.newMessage));
                var newResult = await this.services[0].QueryAsync(uri, new CancellationToken());
                switch (newResult.Intents[0].Intent.ToLower())
                {
                    case "none":
                        await this.None(context, newResult);
                        break;
                    case "hello":
                        await this.Hello(context, newResult);
                        break;
                    case "viasport.intent.howtocoach":
                        await this.HowToCoach(context, newResult);
                        break;
                    case "viasport.intent.findresource":
                        await this.FindResource(context, newResult);
                        break;
                    case "viasport.intent.findprogram":
                        await this.FindProgram(context, newResult);
                        break;
                }
            }
            else
            {
                var message = "Ok, can you tell me what you meant?";
                await context.PostAsync(BotDbStrings.MakeItAcceptable(message));
                context.Wait(this.MessageReceived);
            }
        }

        private async Task CheckResultsAsync(IDialogContext context)
        {
            // We've already established we won't find anything in the DB so don't bother checking...
            if (_doWebSearch)
            {
                // We've already established we won't find anything in the DB so don't bother checking...
                if ((string.IsNullOrEmpty(_entities["SportName"].Entity) && !sportSkip)
                    || (string.IsNullOrEmpty(_entities["DisabilityType"].Entity) && !disabilitySkip)
                    || (string.IsNullOrEmpty(_entities["Subject"].Entity) && !subjectSkip))
                {
                    // We have more info we can collect! Let's do that! 
                    await CollectMoreInfo(context);
                }
                else
                {
                    // Have all the info they are going to give and nothing in the ViaSportDB
                    await OnWebSearch(context);
                }
            }
            else
            {
                // See if we have an appropriate response with the information we have!
                BotDB botDB = new BotDB();
                var references = botDB.QueryReferences(
                        intent,
                        _entities["SportName"].Entity,
                        _entities["DisabilityType"].Entity,
                        _entities["Subject"].Entity);
                if (references.Count == 0)
                {
                    // Didn't find anything adding more information to the query won't help
                    await context.PostAsync("I couldn't find anything in our records but I can search the web for you. Let's collect a bit more information.");
                    _doWebSearch = true;
                    await CheckResultsAsync(context);
                }
                else if (references.Count > 0 && references.Count <= _maxReferences)
                {
                    // Had the right number of results! Just return!
                    await SendResultsToPersonAsync(context, references);
                }
                // If we are here we had too many results! Let's see if we can narrow it down
                else if ((string.IsNullOrEmpty(_entities["SportName"].Entity) && !sportSkip)
                        || (string.IsNullOrEmpty(_entities["DisabilityType"].Entity) && !disabilitySkip)
                        || (string.IsNullOrEmpty(_entities["Subject"].Entity) && !subjectSkip))
                {
                    // We have more info we can collect! Let's do that!
                    await this.CollectMoreInfo(context, references.Count);

                }
                // If we are here we had too many results and they don't want to narrow it down so we should page!
                else if (references.Count > _maxReferences)
                {
                    numberOfAvailableReferences = references.Count;
                    // Had the right number of results! Just return!
                    await SendResultsToPersonAsync(context, references);
                }
            }
        }

        private async Task SendResultsToPersonAsync(IDialogContext context, List<Reference> references)
        {
            if (!_doWebSearch)
            {
                BotDbAnalytics.UpdateAnalyticDatabase(this.intent, this.intentScore, _entities, references);
            }
            await PostResultNoticeAsync(context, references.Count(), query);
            var message = BuildReferenceResponse(context, references);
            await PostCardResults(context, message);
        }

        private async Task PostCardResults(IDialogContext context, IMessageActivity message)
        {
            if (numberOfAvailableReferences > _maxReferences && ((pageNumber+1)*_maxReferences) < numberOfAvailableReferences )
            {
                // Could show more!
                await context.PostAsync(message);
                PostTelemetryCustomEvent("Found Results", message.Attachments.Count);
                // Give the option to page
                PromptDialog.Confirm(
                        context,
                        this.OnPageResults,
                        $"Show more?",
                        "Didn't get that!",
                        promptStyle: PromptStyle.None);

            }
            else if (message.Attachments.Count > 0)
            {
                await context.PostAsync(message);
                await context.PostAsync("We hope that you found what you were looking for. If not, please let us know below.");
                PostTelemetryCustomEvent("Found Results", message.Attachments.Count);
                await OnContactViaSport(context);
                ResetInfo();
                context.Done("Thanks");
                await context.PostAsync("Is there anything else you would like to know?");

            }
            else
            {
                // No Results! Send them to ViaSport and restart!
                await context.PostAsync("Sorry looks like I couldn't find anything for you!");
                await OnContactViaSport(context);
                ResetInfo();
                context.Done("Thanks");
                await context.PostAsync("Is there anything else you would like to know?");
            }
            
        }

        private async Task OnPageResults(IDialogContext context, IAwaitable<bool> result)
        {
            var accepted = (await result);
            if (accepted)
            {
                pageNumber = pageNumber + 1;
                var skip = pageNumber * _maxReferences;
                BotDB botDB = new BotDB();
                var references = botDB.QueryReferences(
                        intent,
                        _entities["SportName"].Entity,
                        _entities["DisabilityType"].Entity,
                        _entities["Subject"].Entity,
                        skip,
                        _maxReferences);
                // Get next page of results
                var message = BuildReferenceResponse(context, references);
                await PostCardResults(context, message);

            }
            else
            {
                await context.PostAsync("We hope that you found what you were looking for. If not, please let us know below.");
                await OnContactViaSport(context);
                ResetInfo();
                context.Done("Thanks");
                await context.PostAsync("Is there anything else you would like to know?");
            }
            
        }


        private async Task OnWebSearch(IDialogContext context)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            // TODO Move to config file
            var key = ConfigurationManager.AppSettings["API KEY HERE"];
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

            var intent = "coaching";
            if (intent.Contains("resource"))
            {
                intent = "information on";
            }
            else if (intent.Contains("program"))
            { 
                intent = "programs for";
            }

            var sport = _entities["SportName"].Entity ?? "sports";
            var subject = _entities["Subject"].Entity ?? "people";
            var disability = _entities["DisabilityType"].Entity ?? "with disabilites";

            // TODO: Add location searching later when we have more ifnromation
            //var location = "Canada";//_entities["Location"].Entity ??"Canada";

            var localQuery = $"{intent} {sport} {subject} {disability}"; //{location}";

            queryString["q"] = localQuery;
            queryString["count"] = _maxReferences.ToString();
            queryString["offset"] = "0";
            queryString["mkt"] = _language;
            queryString["safesearch"] = "strict";

            PostTelemetryCustomEvent("No Results", 0, true, localQuery);

            var uri = "https://api.cognitive.microsoft.com/bing/v5.0/search?" + queryString;

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync();
                var searchResults = JsonConvert.DeserializeObject<SearchResponse>(jsonString.Result);

                var references = new List<Reference>();
                foreach (var i in searchResults.WebPages?.Value)
                {
                    var reference = new Reference()
                    {
                        Title = i.Name,
                        Subtitle = i.DisplayUrl,
                        CardText = i.Snippet,
                        ReferenceUri = i.Url,
                        //CardImageUri = http://www.viasport.ca/sites/default/files/BCSport_logo_PlayIcon_0.png,
                    };
                    references.Add(reference);
                }
                await SendResultsToPersonAsync(context, references);
            }
            else
            {
                ResetInfo();
                await context.PostAsync("Sorry looks like we are having some issues with our websearch!");
                await OnContactViaSport(context);
            }
        }

        private async Task OnContactViaSport(IDialogContext context)
        {
            var responseCard = new ThumbnailCard()
            {
                Title = "Contact viaSport BC",
            };

            responseCard.Buttons = new List<CardAction>
            {
                new CardAction(ActionTypes.OpenUrl, "Contact Us",
                    value: "http://www.viasport.ca/contact-us")
            };

            var att = responseCard.ToAttachment();
            var message = context.MakeMessage();
            message.AttachmentLayout = "list";
            message.Attachments.Add(att);

            await context.PostAsync(message);
        }

 
        private void PostTelemetryCustomEvent(string eventName, int foundCount = 0, bool showQueryInfo = true, string webQuery = null)
        {
            // CHanges for source control
            if (this.telemetry == null)
            {
                this.telemetry = new TelemetryClient(TelemetryConfiguration.Active);
            }

            var telemetryParams = new Dictionary<string, string>();
            if (showQueryInfo)
            {
                telemetryParams.Add("Query", query);
                telemetryParams.Add("Intent", intent);
                telemetryParams.Add("NumFound", foundCount.ToString());
            }
            if (this._entities != null)
            {
                foreach (var entity in this._entities)
                {
                    try
                    {
                        telemetryParams.Add(entity.Key, entity.Value.Entity);
                    }
                    catch (Exception)
                    {
                        // Don't use that entity
                    }
                }
            }

            if (!string.IsNullOrEmpty(webQuery))
            {
                telemetryParams.Add("WebQuery", webQuery);
            }

            this.telemetry.TrackEvent(eventName, telemetryParams);
        }

        private async Task PostResultNoticeAsync(IDialogContext context, int count, string query)
        {
            string message;
            BotDB botDB = new BotDB();
            switch (count)
            {
                case 0:
                    message = botDB.GetString(null, intent + ".0");
                    if (message.Contains("{0}")) message = string.Format(message, _entities["SportName"].Entity);
                    break;
                case 1:
                    message = botDB.GetString(null, intent + ".1");
                    if (message.Contains("{0}")) message = string.Format(message, _entities["SportName"].Entity);
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                    message = string.Format(botDB.GetString(null, intent + ".many"), count.ToString());
                    break;
                default:
                    message = string.Format(botDB.GetString(null, intent + ".toomany"), count.ToString(), _maxReferences);
                    break;
            }
            await context.PostAsync(BotDbStrings.MakeItAcceptable(message));
        }

        private IMessageActivity BuildReferenceResponse(IDialogContext context, List<Reference> references)
        {
            var message = context.MakeMessage();
            message.AttachmentLayout = "carousel";
            foreach (var reference in references)
            {
                message.Attachments.Add(
                    CreateCard(
                        reference,
                        intent,
                        _entities["SportName"].Entity,
                        _entities["DisabilityType"].Entity,
                        _entities["Subject"].Entity));
                if (message.Attachments.Count == _maxReferences) break;
            }
            return message;
        }

        private Attachment CreateCard(
            Reference reference,
            string intent,
            string activity,
            string disability,
            string subject)
        {
            var responseCard = new ThumbnailCard() { Title = reference.Title, Subtitle = reference.Subtitle };
            BotDB botDB = new BotDB();
            if (!string.IsNullOrEmpty(reference.CardText))
            {
                responseCard.Text = reference.CardText;
            }
            else
            {
                if (!string.IsNullOrEmpty(activity))
                {
                    responseCard.Text = string.Format(
                        botDB.GetString(null, intent + ".activity"),
                        activity,
                        reference.Title);
                }
                else
                {
                    responseCard.Text = string.Format(botDB.GetString(null, intent), reference.Title);
                }
            }
            if (reference.CardImageUri != null)
            {
                responseCard.Images = new List<CardImage> { new CardImage(reference.CardImageUri) };
            }
            responseCard.Buttons = new List<CardAction>
            {
                new CardAction(
                    ActionTypes.OpenUrl,
                    "Go to " + reference.Title,
                    value: reference.ReferenceUri)
            };

            return responseCard.ToAttachment();
        }

        private async Task LoadAllEntitiesAsync(IDialogContext context, LuisResult result)
        {
            _entities = new Dictionary<string, EntityRecommendation>
            {
                {"SportName", await GetEntityAsync(context, result, "SportName")},
                {"DisabilityType", await GetEntityAsync(context, result, "DisabilityType")},
                {"Subject", await GetEntityAsync(context, result, "Subject")},
                {"buildin.geography.city", await GetEntityAsync(context, result, "builtin.geography.city")}
            };
        }

        private async Task<EntityRecommendation> GetEntityAsync(IDialogContext context, LuisResult result, string name)
        {
            EntityRecommendation returnVal;

            try
            {
                returnVal = result.Entities.First(s => s.Type == name);
                //once you've pulled out the entity, spell check it before returning
                var originalString = returnVal.Entity;
                var spellingResult = await SpellCheck(originalString);
                var spellCheckedReturnVal = CorrectEntitySpelling(returnVal.Entity, spellingResult);
                if (spellCheckedReturnVal != string.Empty)
                {
                    returnVal.Entity = spellCheckedReturnVal;
                }
                else
                {
                    returnVal.Entity = originalString;
                }
            }
            catch (Exception)
            {
                returnVal = new EntityRecommendation();
            }
            
            
            return returnVal;
        }

        private async Task CollectMoreInfo(IDialogContext context)
        {
            await CollectMoreInfo(context, 0);
        }

        private async Task CollectMoreInfo(IDialogContext context, int referenceCount)
        {
            var botDB = new BotDB();
            StringBuilder message = new StringBuilder("");
            if (referenceCount > 0)
            {
                message.Append(string.Format(botDB.GetString(null, intent + ".needmoreinfo"), referenceCount, _maxReferences));
                message.Append("  ");
            }
            if (string.IsNullOrEmpty(_entities["SportName"].Entity) && !sportSkip)
            {
                message.Append(botDB.GetString(null, intent + ".needSport") + "  Don't know the sport? Just type 'skip'");
                respondToLuis = false;
                PromptDialog.Text(
                    context,
                    OnSelectedSport,
                    message.ToString(),
                    null,
                    3);
            }
            else if (string.IsNullOrEmpty(_entities["DisabilityType"].Entity) && !disabilitySkip)
            {
                message.Append(botDB.GetString(null, intent + ".needDisability") + "  Don't know the disability? Just type 'skip'");
                respondToLuis = false;
                PromptDialog.Text(
                    context,
                    OnSelectedDisability,
                    message.ToString(),
                    null,
                    3);
            }
            else if (string.IsNullOrEmpty(_entities["Subject"].Entity) && !subjectSkip)
            {
                message.Append(botDB.GetString(null, intent + ".needSubject") + "  Don't know who this is for? Just type 'skip'");
                respondToLuis = false;
                PromptDialog.Text(
                    context,
                    OnSelectedSubject,
                    message.ToString(),
                    null,
                    3);
            }
        }

        private string CorrectEntitySpelling(string originalEntityInput, IEnumerable<Models.SpellCheckCall.SpellCheck> checkedPhrase)
        {
            ObservableCollection<Models.SpellCheckCall.SpellCol> SearchResults = new ObservableCollection<Models.SpellCheckCall.SpellCol>();

            var suggestions = string.Empty;
            for (int i = 0; i < checkedPhrase.Count(); i++)
            {
                Models.SpellCheckCall.SpellCheck suggestedCorrection = checkedPhrase.ElementAt(i);
                suggestions += suggestedCorrection.suggestion;
                SearchResults.Add(new Models.SpellCheckCall.SpellCol
                {
                    spellcol = suggestedCorrection
                });
            }

            if (suggestions == string.Empty)
            {
                return suggestions;
            }
            else
            {
                foreach (var phrase in checkedPhrase)
                {
                    var oldWord = phrase.token.Replace("Wrong Word : ", string.Empty);
                    originalEntityInput = originalEntityInput.Replace(oldWord, phrase.suggestion);
                }
                return originalEntityInput;
            }
        }

        private async Task OnSelectedSport(IDialogContext context, IAwaitable<object> result)
        {
            var received = await result;
            BotDB botDB = new BotDB();
            var sport = received.ToString();
            if (sport != "skip" && sport != "no")
            {
                // Check spelling
                var checkedPhrase = await SpellCheck(sport);
                var suggestions = CorrectEntitySpelling(sport, checkedPhrase);

                if (suggestions == string.Empty || suggestions == null)
                {
                    // If there are no suggestions, text is error-free
                    // Handle next step
                    await StoreEntityAndRetrieveResults(context, sport, "SportName");
                }
                else
                {
                    temporaryEntity = suggestions;
                    temporaryEntityType = "SportName";
                    PromptDialog.Confirm(
                          context,
                          AfterSpellCheckAsync,
                          ($"Did you mean {suggestions}?"),
                          "Didn't get that!",
                          promptStyle: PromptStyle.None);
                }
            }
            else
            {
                sportSkip = true;
                respondToLuis = true;
                await CheckResultsAsync(context);
            }

        }

        private void ResetInfo()
        {
            _entities = new Dictionary<string, EntityRecommendation>();

            query = "";
            intent = "";
            intentScore = 0;
            disabilitySkip = false;
            sportSkip = false;
            subjectSkip = false;
            _doWebSearch = false;
            pageNumber = 0;
            numberOfAvailableReferences = 0;
    }

        private async Task StoreEntityAndRetrieveResults(IDialogContext context, string valueToStore, string entityType)
        {
            var NotInDB = false;
            BotDB botDB = new BotDB();
            _entities[entityType] = new EntityRecommendation { Entity = valueToStore, Score = 1, Type = entityType };
            if (entityType == "SportName")
            {
                // Check to make sure we know about this sport otherwise warn the user!
                if (!botDB.CheckSportList(valueToStore))
                    NotInDB = true;
            }
            else if (entityType == "DisabilityType")
            {
                // Overwrite the disability with the acceptable term!
                valueToStore = BotDbStrings.MakeItAcceptable(valueToStore);
                _entities[entityType] = new EntityRecommendation { Entity = valueToStore, Score = 1, Type = entityType };
                // Check to make sure we know about this disability otherwise warn the user!
                if (!botDB.CheckDisabilitiesList(valueToStore))
                {
                    NotInDB = true;
                }
            }
            else
            {
                // Check to make sure we know about this subject otherwise warn the user!
                if (!botDB.CheckSubjectList(valueToStore))
                {
                    NotInDB = true;
                }
            }
            if (NotInDB)
            {
                // Warn the user it wasn't in the database but we'll web search
                _doWebSearch = true;
                // TODO: Replace with variable text!
                await context.PostAsync($"I currently don't have anything in the viaSport database regarding {valueToStore}. But don't worry I search the web and we are always adding to our database!");
            }
            if (!string.IsNullOrEmpty(_entities[entityType].Entity))
            {
                respondToLuis = true;
                await CheckResultsAsync(context);
            }
            else
            {
                respondToLuis = true;
                context.Wait(MessageReceived);
            }
        }

        private async Task OnSelectedDisability(IDialogContext context, IAwaitable<object> result)
        {
            BotDB botDB = new BotDB();
            var received = await result;
            var disabilityType = received.ToString();
            if (disabilityType != "skip" && disabilityType != "no")
            {
                // Check spelling
                var checkedPhrase = await SpellCheck(disabilityType);

                var suggestions = CorrectEntitySpelling(disabilityType, checkedPhrase);

                if (suggestions == "" || suggestions == null)
                {
                    // If there are no suggestions, text is error-free
                    // Handle next step
                    await StoreEntityAndRetrieveResults(context, disabilityType, "DisabilityType");
                }
                else
                {
                    temporaryEntity = suggestions;
                    temporaryEntityType = "DisabilityType";
                    PromptDialog.Confirm(
                          context,
                          AfterSpellCheckAsync,
                          ($"Did you mean {suggestions}?"),
                          "Didn't get that!",
                          promptStyle: PromptStyle.None);
                }
            }
            else
            {
                disabilitySkip = true;
                respondToLuis = true;
                await CheckResultsAsync(context);
            }
        }

        private async Task OnSelectedSubject(IDialogContext context, IAwaitable<object> result)
        {
            BotDB botDB = new BotDB();
            var subject = (await result).ToString();
            if (subject != "skip" && subject != "no")
            {
                // Check spelling
                var checkedPhrase = await SpellCheck(subject);
                var suggestions = CorrectEntitySpelling(subject, checkedPhrase);

                if (suggestions == "" || suggestions == null)
                {
                    // If there are no suggestions, text is error-free
                    // Handle next step
                    await StoreEntityAndRetrieveResults(context, subject, "Subject");
                }
                else
                {
                    temporaryEntity = suggestions;
                    temporaryEntityType = "Subject";
                    PromptDialog.Confirm(
                          context,
                          AfterSpellCheckAsync,
                          ($"Did you mean {suggestions}?"),
                          "Didn't get that!",
                          promptStyle: PromptStyle.None);
                }

            }
            else
            {
                subjectSkip = true;
                respondToLuis = true;
                await CheckResultsAsync(context);
            }
        }

        private async Task<IEnumerable<Models.SpellCheckCall.SpellCheck>> SpellCheck(string received)
        {
            List<Models.SpellCheckCall.SpellCheck> spellCheckRequest = new List<Models.SpellCheckCall.SpellCheck>();
            var client = new HttpClient();
            var key = ConfigurationManager.AppSettings["API KEY HERE"];
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
            string text = received;
            string mode = "proof";
            string mkt = "en-us";
            var spellEndPoint = "https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/?";
            try
            {
                var result =
                    await client.GetAsync(string.Format("{0}text={1}&mode={2}&mkt={3}", spellEndPoint, text, mode, mkt));
                result.EnsureSuccessStatusCode();
                var json = await result.Content.ReadAsStringAsync();
                dynamic data = JObject.Parse(json);
                for (int i = 0; i < data.flaggedTokens.Count; i++)
                {
                    spellCheckRequest.Add(
                        new Models.SpellCheckCall.SpellCheck
                        {
                            offset = "Offset : " + data.flaggedTokens[i].offset,
                            token = "Wrong Word : " + data.flaggedTokens[i].token,
                            suggestion = data.flaggedTokens[i].suggestions[0].suggestion
                        });
                }
                return spellCheckRequest;
            }
            catch (Exception ex)
            {
                return spellCheckRequest;
            }
        }

        public async Task AfterSpellCheckAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                await context.PostAsync(BotDbStrings.MakeItAcceptable("Okay, thank you."));
                await StoreEntityAndRetrieveResults(context, temporaryEntity, temporaryEntityType);
            }
            else
            {
                await context.PostAsync(BotDbStrings.MakeItAcceptable("Ok let's try that again!"));
                await CollectMoreInfo(context);
            }
        }

        public ViaSportDialog()
        {

        }

        public ViaSportDialog(ILuisService service)
            : base(service)
        {
        }


    }
}