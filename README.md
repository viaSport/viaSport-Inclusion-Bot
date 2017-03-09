Bots for Good – Building a Conversational UI for Inclusive Sport
---

Introduction
============

In the relatively new space of Conversations as a Platform, many
organizations have begun to incorporate chat bots into the structure of
their business as tools to drive sales and manage customer relations. In
February 2017, viaSport British Columbia, a Vancouver-based
not-for-profit with a mission to promote inclusive sport, partnered with
Microsoft to build a proof-of-concept conversational interface and
analytics dashboard for their newest inclusive sport initiative.

Technologies used in this project:

-   Bot Framework

-   Cognitive Services

    -   LUIS

    -   Bing Spell Check

    -   Bing Web Search

-   PowerBI Embedded

-   Azure SQL Database

-   UWP

The team was comprised of developers from the Microsoft community and
advised by representatives from viaSport:

-   Mark Schramm, MVP

-   Sage Franch, Technical Evangelist

-   Sergii Baidachnyi, Principal Technical Evangelist

-   Nastassia Rashid, Technical Evangelist

-   Yash Manivannan, Technical Evangelist Co-op

-   Elisabeth Walker-Young, Manager Inclusion & Sport for Life, Sport
    Development, viaSport

Summary
=======

Customer Profile
----------------

[viaSport British Columbia](http://www.viasport.ca/) was created in 2011
as a legacy of the 2010 Olympic and Paralympic Winter Games. As
an independent not-for-profit organization, viaSport is tasked by the
provincial government to be the lead agency responsible for promoting
and developing amateur sport in British Columbia. viaSport's purpose is
to champion positive changes so that more British Columbians thrive via
sport and physical activity by uniting leaders across the sector to:

-   think provincially to increase participation and performance

-   foster excellence in order for all athletes to develop and improve
    their performance

-   activate locally to enrich and energize communities

-   leverage investment to maximize funding and revenue

Problem Statement
-----------------

Currently, finding and navigating resources around sport and physical
activity and disability is a challenge for everyone, be they active or
potential participants, parents/guardians/caregivers, sport
administrators, sport club owners, Provincial sport organizations,
physiotherapists, recreational therapists, coaches or officials. This
challenge also results in the duplication in resource development,
participants falling through cracks and a lost opportunity to empower
British Columbians with a disability the option to be healthy and active
and/or pursue high performance sport.

viaSport’s inclusion policy is an umbrella policy for many groups often
left behind the traditional sport system. There is an opportunity to
scale this tool/technology around increased awareness, training and
participation, for girls and women, those who identify as LGBTQI2S, as
well as for aboriginal, cultural, socioeconomic and seniors
participation in *physical* activity and sport. There is an opportunity
to work with National partners in increasing the scope to national level
and crossing sectors with the education, health and medical sectors.

In the initial form of this project, the three main questions viaSport
aims to address for its audience are:

-   How to coach specific sports for people with specific disabilities

-   Where people with specific disabilities can participate in specific
    sports

-   What resources exist related to learning specific sports, geared
    towards people with specific disabilities.

Because of the complex and customized nature of these questions,
viaSport needs a rich and targeted solution that will allow them to
serve their audience.

Customer Testimonial
--------------------

“To see this chat bot come to fruition and evolve over the last few days
was awe-inspiring. \[The development team’s\] attention to detail, high
standards and quality control were noted.

I feel quite fortunate to have connected to Microsoft through the
Microsoft Ascend+ Project. The viaSport Inclusion ChatBot is going to be
innovative for our sector. There is a strong possibility of positively
impacting the lives of a lot of people with and without disabilities in
becoming more active and more inclusive through physical activity and
sport. You will have played a big role in that.”

-   Elisabeth Walker-Young, Manager Inclusion & Sport for Life, viaSport
    British Columbia

Project Objectives
------------------

The objective of the project was to build an intelligent chat bot to
simplify the search for resources, allowing coaches, sport leaders, and
participants in all sports to provide appropriate programming and
services to athletes with a disability. By simplifying the navigation of
resources and identifying gaps, viaSport will see more inclusive
programs come to fruition, more coaches become trained in working with
those with a disability. Sport and physical activity for those with a
disability will become a daily conversation in the sport sector.
Combined, these will be the catalyst for more people with a disability
participating locally, regionally, provincially, nationally and at all
levels whether it be a playground, a school yard, a local sport team or
increased participation on one of Canadian teams (Canadian Paralympic
Team, Canadian Special Olympics Team, Canadian Deaflympic Team or even
the Olympic Team).

1.  Implement LUIS for context-based conversation to collect missing
    information in the original query

    -   Streamline bot communication and information gathering using
        LUIS

2.  Implement rules and learning to identify unaccepted, offensive, or
    outdated terms used by user and always respond with correct
    terminology. Example: guest says “Hearing Impaired,” reply with
    correct terminology, “Hard of Hearing”

    -   Varying response to relieve repetitiveness

    -   Stretch: implement Cognitive Services sentiment API to gauge
        user response

3.  Implement Power BI for analytics

    -   Generate reports on intent completion.  Success/Failure
        percentages

    -   App Insights to provide statistics on usage

In addition to the above core goals, additional stretch goals were also
set:

1.  Adding value to Bot conversations 

    -   Accomplished:

        -   Implement a default handler for new intents

        -   Improve content of cards to add more value 

        -   Collect user feedback 

        -   Implement depluralization to simplify database 

        -   Bing SpellCheck API 

    -   Future:

        -   Resource ranking to deliver best resources first

        -   Sentiment API to gauge user response - add "do you find
            these resources helpful?" 

  

1.  Implementing voice capabilities (partly accomplished)

    -   Bing Speech API 

    -   Create UWP bot client

>  

1.  Improving the administrator app 

    -   Accomplished:

        -   Secure database – Make sure that the current database (and
            any necessary enhancements) are secure. 

    -   Future:

        -   Add ability to retrain LUIS model outside of the portal 

        -   Possible migration to Xamarin or Azure Mobile Services
            integration 

 

Execution 
==========

Tools and Technologies Used
---------------------------

-   Bot Framework (C\#)

-   Cognitive Services

    -   LUIS

    -   Bing Spell Check

    -   Bing Web Search

-   PowerBI

-   Azure SQL Database

-   UWP

The implementation of these technologies is detailed in the following
sections.

Architecture Overview
---------------------

Prior to the workshop, Mark Schramm had been working with viaSport to
set up the backbone for this project. Since viaSport has no in-house
technical team, everything that was built was built from the ground up.

The core functionality of the project is the bot, built in C\# using Bot
Framework and deployed to an Azure Web App. This is the staging site for
the bot during its private preview phase, after which it will be
accessed as a web chat directly through the viaSport website.

Three Cognitive Services APIs were used to enhance bot functionality:
LUIS, Bing Web Search, and Bing Spell Check. LUIS was used to guide the
dialog by detecting user intents, Bing Web Search was used to provide
web results in the event that none of the results in the database match
the user’s query, and Bing Spell Check was used to eliminate human error
by spell checking user input before storing or performing operations on
it.

The bot is built on top of an Azure SQL database, populated with
resources curated by viaSport.

Two UWP apps were also built to support this project, one as an
administrator and one as an interface. The custom speech interface app
was built as a test for potential future expansion of this bot for users
who require voice interaction instead of written text. The
administrative app was built outside the scope of the five days assigned
to this project, and as such will be mentioned in this report but the
implementation will not be discussed here. The administrative app
enables the non-technical viaSport team to modify the database without
needing to hire a developer or have direct access to the tables. This is
instrumental to the success of this project because viaSport is
continually generating new resources to support its userbase, and
database update capabilities are critical to this process.

To better inform viaSport’s resource curation going forward, Power BI
Embedded was used to generate reports and a dashboard for viaSport to
access rich insights into the bot’s performance.

The following diagram illustrates the project architecture:

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/viaSport_architecture_diagram_main.PNG)

Bot Framework SDK
-----------------

The core bot functionality was built leveraging the Bot Framework SDK in
C\#. The bot leverages the Knowledge Base design pattern, where in this
case the knowledge base is the database curated by viaSport. The speech
component of the bot (detailed in a later section of this report)
leverages the Backchannel pattern through the Direct Line connection.

The dialog is handled through LUIS intents, and its implementation is
described in detail in the following sections.

Developing this bot was an opportunity for the development team to
explore bot design practices and discover some key learnings about user
experience in conversational interfaces. Here are some key insights
gleaned from this experience:

#### **Learning:** Simple Interactions Are Complex

One of the key advantages of using a bot is simplifying the user
experience and providing the user with a one-stop-shop for the task or
tasks they want to accomplish. The development team quickly discovered
that building a bot that appears simple to the user is a complex
process. In order to be able to hold an effective and natural
conversation, the bot’s dialog must be rich enough to recognize state,
keep track of the user’s intention, recognize the many ways a user might
phrase a query with the same intention, and proactively handle user
error. Failing to do so will create a bot with a very dry user
experience, eliminating the benefit of creating a bot.

Some of the ways the viaSport Inclusion Bot does the above is by:

-   Proactively spell checking user input to avoid human error slowing
    down the bot with extra database calls

-   Implementing intelligence through Cognitive Services APIs (detailed
    in the Cognitive Services section) to recognize entities from user
    queries and understand the user’s intent

-   Manually feeding spell-checked queries back into LUIS and looping
    back to the root of the dialog, in order to avoid genuine queries
    with misspelled words being deemed unrecognizable – this not only
    creates a better user experience, but also impacts the analytics by
    ensuring that the correct intent is captured.

#### **Learning**: Features vs. Functionality

During the development process, a new stretch goal of adding an in-bot
location control was entertained, but ultimately scrapped in favor of
functionality over features. In the planning stages of this bot, Allan
Bonifacio from the Bing and Cortana Ecosystem group cautioned that the
development team should always favor functionality over adding features.
A bot with a great user experience should seem simple and intuitive to
the user, and sometimes superfluous features can get in the way and
overcomplicate the bot experience unnecessarily.

In the project’s current state, most of the resources in the database do
not have location information. Some exploration of the bot location
control integration with the Bing Maps API was done during the project,
but eventually scrapped because it was a better use of time to
strengthen the core bot experience than to add this complex feature that
would have only impacted a small portion of the results provided to
users.

#### **Learning**: Asynchronous Programming and Bot Dialog Prompts

When developing the dialog for the bot, the development team noticed
that the way Prompts are structured, they must not be followed by any
other code before the end of their method, otherwise the threading will
fall out of sync. This came to light when working with the Confirm
prompts asking the user to confirm a spell-corrected phrase:

    PromptDialog.Confirm(
    
    context,
    
    AfterSpellCheckAsync,
    
    (\$"Did you mean {suggestions}?"),
    
    "Didn't get that!",
    
    promptStyle: PromptStyle.None);

This piece of code was initially placed in the middle of a method that
would also parse through the spell-corrected results, but because the
method that follows the prompt cannot be awaited, the routine would
finish before the user confirmed and the entire dialog would be
misaligned. So these two functions were separated into separate methods,
and the threading problem was solved.

Cognitive Services
------------------

To build a better bot, three Cognitive Services APIs were implemented:
LUIS, Bing Web Search, and Spell Check. LUIS is used to guide the
conversation through recognizing intents, Bing Web Search is used when
there are no results found in the database for the user’s query, and
Bing Spell Check is used to reduce margin for human error by correcting
the spelling of user input before querying the database or storing
entities.

The below diagram displays the flow for a scenario in which all three
APIs are called. In this scenario, the user input is first sent to LUIS,
the original query is sent to Bing Spell Check before entities are
stored, the database is queried for results relevant to the user’s query
and, when no results are found in the database, the Bing Web Search API
is called with a query string based on the entities found in the user’s
spell checked query. Those results are then delivered to the user. The
sections that follow detail the implementation of the three APIs.

![Cognitive Services implementation in viaSport Inclusion Bot](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/viaSport_cognitive_services_flow_diagram.PNG)

### LUIS

The core bot dialog is handled through six LUIS intents: None, Hello,
Help, HowToCoach, FindProgram, and FindResource. These intents map to
three key entities: Sport, Disability, and Subject (who the athlete is,
i.e. children, adults). Below is a breakdown of what each of these
intents does and how it is handled in the bot code.

#### None 

The None intent handles the case in which no other intent has been
recognized by LUIS. In this case, the bot

    \[LuisIntent("")\]
    
    public async Task None(IDialogContext context, LuisResult result)
    
    {
    
    this.query = result.Query;
    
    await context.LoadAsync(new CancellationToken());
    
    PostTelemetryCustomEvent("none", 0, false);
    
    string message = \$"Sorry I did not understand you: ";
    
    if (result.Query.ToLower() == "version")
    
    {
    
    message = "viaSport Disability Resource Bot, version: "
    
    +
    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
    
    await context.PostAsync(BotDbStrings.MakeItAcceptable(message));
    
    context.Wait(MessageReceived);
    
    }
    
    else if (result != null && result.Query != null)
    
    {
    
    await ConfirmNoneIntent(context, result);
    
    }
    
    }

Within the None intent, another routine entitled ConfirmNoneIntent is
called. The purpose of this routine is to double check that the user’s
input did not match any of the five core intents – it first performs a
spell check on the input, then uses a Confirm prompt to send the
spell-corrected input back to the user and ask if that is what they had
intended to input. If the user confirms that the new query is correct,
the bot then feeds that back in to LUIS to determine if it matches any
of the intents.

In the example below, the user asks, “How do I cooach hockey?” This
should match the HowToCoach intent, but because “coach” is spelled
incorrectly, LUIS recognizes it instead as a None intent. The bot then
performs a spell check, asks the user if they meant to say, “How do I
coach hockey?” and, when the user confirms, feeds this new query into
LUIS, which recognizes it correctly as a HowToCoach intent.

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/bot-snapshot-confirmnoneintent.PNG)

Below is the code for the routine that handles this case.

    private async Task ConfirmNoneIntent(IDialogContext context, LuisResult
    result)
    
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
    
    var message = string.Format(botDB.GetString(null, "none"),
    result.Query);
    
    await context.PostAsync(BotDbStrings.MakeItAcceptable(message));
    
    context.Wait(this.MessageReceived);
    
    }
    
    else
    
    {
    
    PromptDialog.Confirm(
    
    context,
    
    this.OnSpellCheckIntent,
    
    \$"Did you mean '{this.newMessage}'?",
    
    "Didn't get that!",
    
    promptStyle: PromptStyle.None);
    
    }
    
    }

#### Hello

The Hello intent is triggered when the user greets the bot. The bot is
trained to understand generic and colloquial phrases including, “hello,”
“hi,” “what’s up,” etc. It responds to the user with a greeting from the
database, like the one shown in the example below:

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/bot-snapshot-hellointent.PNG)

Below is the code that handles the Hello intent.

    \[LuisIntent("viasport.intent.hello")\]
    
    public async Task Hello(IDialogContext context, LuisResult result)
    
    {
    
    PostTelemetryCustomEvent("hello", 0, false);
    
    BotDB botDB = new BotDB();
    
    var message = botDB.GetString(null, "viasport.intent.hello.greeting");
    
    BotDbAnalytics.UpdateAnalyticDatabase(result.Intents\[0\].Intent,
    (double)result.Intents\[0\].Score);
    
    await context.PostAsync(BotDbStrings.MakeItAcceptable(message));
    
    context.Wait(MessageReceived);
    
    }

#### 

#### Help

The Help intent is triggered when the user asks for help or
clarification. The bot responds by offering suggestions on what the user
can ask for, then returns to the root of the dialog where it awaits the
next input, which will then be fed through LUIS.

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/bot-snapshot-helpintent.PNG)

    \[LuisIntent("viasport.intent.help")\]
    
    public async Task Help(IDialogContext context, LuisResult result)
    
    {
    
    PostTelemetryCustomEvent("hello", 0, false);
    
    BotDB botDB = new BotDB();
    
    var message = string.Format(botDB.GetString(null,
    "viasport.intent.help.greeting"), "\\n", "\\n", "\\n");
    
    BotDbAnalytics.UpdateAnalyticDatabase(result.Intents\[0\].Intent,
    (double)result.Intents\[0\].Score);
    
    await context.PostAsync(BotDbStrings.MakeItAcceptable(message));
    
    context.Wait(MessageReceived);
    
    }

#### HowToCoach, FindProgram, and FindResource

The HowToCoach, FindProgram, and FindResource intents are almost
identical, so this section addresses all three of them. HowToCoach is
triggered when the user inputs a query that indicates they are searching
for resources to help them coach or teach a sport to a person or persons
with disabilities. FindProgram is triggered when the user asks the bot
to find a program where they can learn or play a sport. FindResource is
triggered when the user’s query indicates that they are looking for a
specific resource that will help them with a specific disability or
sport. The following three figures show some sample utterances that
trigger each intent, captured from the LUIS portal.

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/LUIS_howtocoach_utterances.PNG)

*Figure: Sample utterances for HowToCoach*

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/LUIS_findprogram_utterances.PNG)

*Figure: Sample utterances for FindProgram*

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/LUIS_findresource_utterances.PNG)
*Figure: Sample utterances for FindResource*

In the bot code, each of these three intents has a handler and
accompanying routines that are identical in purpose and structure,
except for the database components they use and which entities and
responses they employ. This section will explore how the flow is
handled, through the code for the HowToCoach intent. The same flow
applies through the corresponding Tasks for FindResource and FindProgram
as well.

When LUIS recognizes the user’s input as matching one of the trained
utterances, the HowToCoach Task is triggered. This Task first checks if
LUIS’s confidence score for the intent is at least the acceptable score
as set by the team (currently set to .25). If it isn’t, then the intent
is not a close enough match, so the bot passes it into the None handler
instead. If it is above the threshold, then the user’s query is passed
into LoadAllEntitiesAsync.

    \[LuisIntent("viasport.intent.howtocoach")\]
    
    public async Task HowToCoach(IDialogContext context, LuisResult result)
    
    {
    
    if (result.Intents\[0\].Score &lt; AcceptableScore)
    
    {
    
    await this.None(context, result);
    
    }
    
    else
    
    {
    
    await LoadAllEntitiesAsync(context, result);
    
    query = result.Query;
    
    intent = result.Intents\[0\].Intent;
    
    intentScore = (double)result.Intents\[0\].Score;
    
    PostTelemetryCustomEvent("howtocoach", 0, false);
    
    respondToLuis = true;
    
    if (respondToLuis)
    
    {
    
    await CheckResultsAsync(context);
    
    }
    
    }
    
    }

    LoadAllEntitiesAsync parses the query and loads any recognized entities
    by calling GetEntityAsync.
    
    private async Task LoadAllEntitiesAsync(IDialogContext context,
    LuisResult result)
    
    {
    
    \_entities = new Dictionary&lt;string, EntityRecommendation&gt;
    
    {
    
    {"SportName", await GetEntityAsync(context, result, "SportName")},
    
    {"DisabilityType", await GetEntityAsync(context, result,
    "DisabilityType")},
    
    {"Subject", await GetEntityAsync(context, result, "Subject")},
    
    {"buildin.geography.city", await GetEntityAsync(context, result,
    "builtin.geography.city")}
    
    };
    
    }

LoadAllEntitiesAsync calls GetEntityAsync (signature below), which
performs a spell check on the entity and returns the spell-corrected
term. This eliminates the chance of any misspelled information being
carried through the dialog, and lowers the human error in the bot
conversation. Doing this spell check prevents the bot from ever querying
the database for a misspelled entity, thus lowering the risk for error.

    private async Task&lt;EntityRecommendation&gt;
    GetEntityAsync(IDialogContext context, LuisResult result, string name)

Once all the entities are loaded, PostTelemetryCustomEvent is called,
then the query is passed into CheckResultsAsync. CheckResultsAsync takes
one of two paths, depending on whether \_doWebSearch has been set to
true (this only happens when no resources have been found in the
database). If \_doWebSearch is true, the bot will perform a web search
using the Bing Web Search API and print the top results to the user (see
the next subsection for the implementation details for this step).
Otherwise, a database check is performed to see if there are any results
that match the stored entities. If there are none, \_doWebSearch is set
to true and CheckResultsAsync is called recursively, this time
performing a web search. If there are between 1 and the \_maxReferences
value of results (currently set to 4), then the bot sends the results to
the user in the form of cards, using SendResultsToPersonAsync.

If there are more than \_maxReferences results, and one or more of the
entities are missing, then the bot will enter OnCollectMoreInfo, which
prompts the user to enter the missing information. Entities are always
requested in priority order: sport first, then, disability, and finally,
subject (i.e. child, adult, or senior). If one of these entities is
already present, the bot will not prompt the user for that entity.

If there are more than \_maxReferences results, and all entities are
filled in or have been skipped, the bot will share the top
\_maxReferences with the user, again using SendResultsToPersonAsync to
post the results in card form. Now, if the user enters “more,” the bot
will continue to call SendResultsToPersonAsync until all the results
have been delivered to the user. The diagram for this flow is
illustrated in the diagram below:

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/viaSport_howtocoach_intent_dialog_flow.PNG)

### Bing Web Search API

The Bing Web Search API is used in the bot to search the web for results
related to the user’s query if no relevant results were found in the
viaSport database. This feature was not in the original goals, but after
discussion with viaSport, the team determined that the ultimate purpose
of the bot is to be as helpful as possible to the user, so rather than
having them leave the viaSport site if they don’t find the information
they are looking for, the bot can continue to assist them in their web
search.

The Bing Web Search API is implemented in the code in the OnWebSearch
Task. This Task builds a query from the entities that have been
collected from the user, and structures it as follows:

    query = \$"{intent} {sport} {subject} {disability}";

For example, if the user’s query was, “How do I coach volleyball to a
student with spinal cord bifuda?” the query sent to Bing Web Search will
be “coaching volleyball student spinal cord bifuda.” If the user’s query
was, “I want to find a program where my daughter can ski. She is in a
wheelchair,” the query sent to Bing Web Search will be, “programs for
ski daughter wheelchair.” The intents are converted to natural phrases
using a simple check, as shown below:

    var intent = "coaching";
    
    if (intent.Contains("resource"))
    
    {
    
    intent = "information on";
    
    }
    
    else if (intent.Contains("program"))
    
    {
    
    intent = "programs for";
    
    }

Once the query string is built it is posted to the API, the results are
deserialized and sent to the user as cards using PostResultsToUserAsync.
The full OnWebSearch Task is shown below:

    private async Task OnWebSearch(IDialogContext context)
    
    {
    
    var client = new HttpClient();
    
    var queryString = HttpUtility.ParseQueryString(string.Empty);
    
    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "&lt;BING
    WEB SEARCH API KEY HERE&gt;");
    
    var intent = "coaching";
    
    if (intent.Contains("resource"))
    
    {
    
    intent = "information on";
    
    }
    
    else if (intent.Contains("program"))
    
    {
    
    intent = "programs for";
    
    }
    
    var sport = \_entities\["SportName"\].Entity ?? "sports";
    
    var subject = \_entities\["Subject"\].Entity ?? "people";
    
    var disability = \_entities\["DisabilityType"\].Entity ?? "with
    disabilites";
    
    query = \$"{intent} {sport} {subject} {disability}";
    
    queryString\["q"\] = query;
    
    queryString\["count"\] = \_maxReferences.ToString();
    
    queryString\["offset"\] = "0";
    
    queryString\["mkt"\] = \_language;
    
    queryString\["safesearch"\] = "strict";
    
    var uri = "https://api.cognitive.microsoft.com/bing/v5.0/search?" +
    queryString;
    
    var response = await client.GetAsync(uri);
    
    if (response.IsSuccessStatusCode)
    
    {
    
    var jsonString = response.Content.ReadAsStringAsync();
    
    var searchResults =
    JsonConvert.DeserializeObject&lt;SearchResponse&gt;(jsonString.Result);
    
    var references = new List&lt;Reference&gt;();
    
    foreach (var i in searchResults.WebPages?.Value)
    
    {
    
    var reference = new Reference()
    
    {
    
    Title = i.Name,
    
    Subtitle = i.DisplayUrl,
    
    CardText = i.Snippet,
    
    ReferenceUri = i.Url,
    
    };
    
    references.Add(reference);
    
    }
    
    await SendResultsToPersonAsync(context, references);
    
    }
    
    else
    
    {
    
    await context.PostAsync("Sorry looks like we are having some issues with
    our websearch!");
    
    await OnContactViaSport(context);
    
    }
    
    }

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/bot-snapshot-websearch.PNG)

After the web search (or if the web search fails), the bot invites the
user to contact viaSport to provide feedback on the experience. The
positioning of this interaction was informed by Elisabeth from viaSport,
who agreed that this would be the most appropriate point in the
conversation to invite the user to provide feedback.

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/bot-snapshot-contact.PNG)

### Spell Check API

The Bing Spell Check API, which sits under the Language category of
Cognitive Services APIs, is used in the viaSport bot to spell check
components input by the user. The SpellCheck routine (header below)
performs a spell check by building a string to send to the Spell Check
API and returning a response which includes the rich data about the
query, including the index of any misspelled word in the string, the
misspelled word, and the suggested correction.

    private async
    Task&lt;IEnumerable&lt;Models.SpellCheckCall.SpellCheck&gt;&gt;
    SpellCheck(string received)

When dealing with a specific entity, CorrectEntitySpelling is called to
pull out the appropriate corrected term from the object returned by
SpellCheck. This parses through the structured return from SpellCheck
and replaces the misspelled words in the original string with the
corrected terms.

    private string CorrectEntitySpelling(string originalEntityInput,
    IEnumerable&lt;Models.SpellCheckCall.SpellCheck&gt; checkedPhrase)
    
    {
    
    ObservableCollection&lt;Models.SpellCheckCall.SpellCol&gt; SearchResults
    = new ObservableCollection&lt;Models.SpellCheckCall.SpellCol&gt;();
    
    var suggestions = string.Empty;
    
    for (int i = 0; i &lt; checkedPhrase.Count(); i++)
    
    {
    
    Models.SpellCheckCall.SpellCheck suggestedCorrection =
    checkedPhrase.ElementAt(i);
    
    suggestions += suggestedCorrection.suggestion;
    
    SearchResults.Add(new Models.SpellCheckCall.SpellCol
    
    {
    
    spellcol = suggestedCorrection
    
    });
    
    }
    
    if (suggestions == string.Empty)
    
    {
    
    return originalEntityInput;
    
    }
    
    else
    
    {
    
    foreach (var phrase in checkedPhrase)
    
    {
    
    var oldWord = phrase.token.Replace("Wrong Word : ", string.Empty);
    
    originalEntityInput = originalEntityInput.Replace(oldWord,
    phrase.suggestion);
    
    }
    
    return originalEntityInput;
    
    }
    
    }

These routines are called proactively anytime a new element input by the
user is going to be used, searched for, or stored in the database. Spell
checking user input makes the bot more efficient and promotes better
user experiences. Performing a spell check before accessing the database
prevents wasted calls to the database and prevents misspelled entities
from being carried through the dialog. If the bot enters the None
intent, instead of immediately telling the user it doesn’t understand
what they meant, the bot will provide a suggestion and prompt the user
to confirm if the corrected query is indeed what they had intended to
enter.

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/bot-snapshot-spellcheck.PNG)

Database
--------

Azure SQL database over the traditional SQL Server. viaSport does not
have an in-house SQL developer, so Azure SQL was chosen because it
presented an easy but robust database management system that would make
the database easier to maintain in the long term. The decision to use
Azure SQL was also informed by its support for Live (Direct Source) and
Imported (data pre-uploaded to pbix) reports.

The features that informed this decision include:

-   Working with SQLCMD or the SQL Server Management Studio

-   It requires no physical administration

-   High availability architecture

-   Scalable service plans and seamless integration with Microsoft
    technologies.

The administrative app provides CRUD (Create, Read, Update, Delete)
access to the database, allowing the viaSport team to modify its
contents as they see fit.

#### Table Structure
![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/viaSport_database_table_reports.jpg.png)

#### **Learning**: Singularizing Entities to Reduce Database Redundancy

In order to improve the database, the team was able to remove redundancy
by converting all entities to their singular version before querying or
storing in the database. For example, “basketballs” becomes
“basketball,” and “children” becomes “child.” This reduces the number of
entries required in the database by half.

Initially, it was thought that an API would be the best way to
singularize the entities, but it is possible to use the
PluralizationService class in the System.Data.Entity.Design namespace to
perform this action. The custom Singularize method is shown below:

    public string Singularize(string text)
    
    {
    
    var CI = new CultureInfo("en-US");
    
    var depluralizer = PluralizationService.CreateService(CI);
    
    var returnText = text;
    
    //var singularText = Singularize(text);
    
    if (depluralizer.IsPlural(text))
    
    {
    
    //convert message to singular form
    
    returnText = depluralizer.Singularize(text);
    
    }
    
    //a few hardcoded examples we will likely encounter, the Pluralizer
    doesn't recognize these as plurals
    
    if (text == "people")
    
    {
    
    text = "person";
    
    }
    
    else
    
    {
    
    //do nothing because text is already the desired result
    
    }
    
    return returnText.ToLower();
    
    }
    
Implementation of Outdated Term Correction
------------------------------------------

One of the goals of this project was to implement a function which would
automatically replace offensive or outdated terms to preferred ones.
When building for inclusivity, it is important to put into place a
certain level of content filtering and language filtering in order to
ensure that the bot is politically correct and does not say anything
that will harm the users. Specific to the target audience of the
viaSport Inclusion Bot, users may use outdated or offensive terms to
describe disabilities. Instead of using these offensive user-submitted
terms, viaSport wanted the bot to recognize an outdated term and replace
it with the preferred term for future communication with the user. In
order to implement this, a database table was created for these term
pairs, and a MakeItAcceptable method was created to handle word
replacements within the bot dialog.

The viaSport team created a table that had two columns “Outdated Terms”
and “Preferred Terms” which contained the words which are often
considered offensive and their replacements with people- first terms.
For example, “disabled athletes,” is outdated and should be replaced
with its preferred person-first equivalent, “athletes with
disabilities.”

A new class called “MakeItAcceptable” was created to handle the word
replacements within the bot. A call is made to this class made every
time there is user input. As soon as the user inputs the outdated term,
it will automatically search the database for the preferred term. The
syntax used to perform this function is:


    await context.PostAsync(BotDbStrings.MakeItAcceptable());

Power BI Embedded Implementation
--------------------------------

As a not-for-profit organization, viaSport works with many external
partners and sponsors, many of whom will be asked to provide resources
that viaSport can include in the knowledgebase for this bot. When
pitching the viaSport Inclusion Bot to these partners and sponsors,
having the ability to share insights into bot performance will be
critical to partner adoption of this project.

To support this, the third goal of the project was to build reports that
demonstrate the bot’s success in the community, through performance and
interaction statistics. It is also important for the viaSport team to
have rich information about what users are searching for, which queries
are successfully answered (where the resources are found in the
database), and, perhaps most importantly, which queries are not
successfully answered, so viaSport can invest in those areas and work
with partners to fill in gaps and provide resources for all potential
users.

The main technology implemented to accomplish these goals was Power BI
Embedded. The following actions were taken to implement this technology:

-   Provision a Power BI Embedded workspace collection in Azure

-   Create a client application in order to upload/update all reports in
    the collection

-   Create a data storage to store all information related to the
    reports

-   Integrate telemetry functionality to the bot

-   Design and publish live and imported reports

-   Display reports on the web

The following subsections detail the above actions.

### Provisioning 

PowerBI Embedded has few management features in the Azure portal, in
fact the only relevant action to take was to create a workspace
collection. At this step, a workspace collection name, subscription
name, resource group, and location were provided.

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/viasport_powerbi_embedded_provisioning_1.png)

The next step was to get access keys and review existing workspaces.

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/viasport_powerbi_embedded_provisioning_2.png)

**Learning**: Because PowerBI Embedded works better with workspaces than
collections, two different workspaces to host the reports. The first one
(internalReports) contains live reports that can be analyzed and
controlled by the viaSport team. The second collection (externalReports)
contains static imported reports that can be shared publicly on the
viaSport website.

### Client Application

In addition to the reports, a client application was also created to
ensure that viaSport would be able to update all reports without the
support of the development team. The application uses several parameters
from the App.config file that allow the application to be tuned:

-   **AzureWorkspaceCollectionKey** – contains the workspace collection
    key;

-   **sqlLogin** – a login to the Azure SQL database where data can be
    retrieved for the reports;

-   **sqlPassword** – a password to the Azure SQL database where data
    can be retrieved for the reports;

-   **AzureWorkspaceCollectionName** – contains the workspace collection
    name;

-   **PowerBIApiUri** – a standard URI (<https://api.powerbi.com>), but
    potentially can be changed by Microsoft;

-   **internalCollectionName** – a name for the internal collection;

-   **externalCollectionName** – a name for the external collection;

Before implementing any Power BI Embedded related code, it is important
to add several NuGet packages (Microsoft.PowerBI.Core and
Microsoft.PowerBI.API):

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/viaSport_nuget_configuration.png)

**Learning**: If you code does not compile due to a problem with
Newtonsoft.Json, the library simply needs to be updated to the latest
version.

Using the references above it is possible to implement some methods to
work with the workspace collection and workspaces inside. Here are some
of these methods:

    static async Task&lt;PowerBIClient&gt; CreateClient()
    
    {
    
    // Create a token credentials with "AppKey" type
    
    var credentials = new TokenCredentials(accessKey, "AppKey");
    
    // Instantiate your Power BI client passing in the required credentials
    
    var client = new PowerBIClient(credentials);
    
    // Override the api endpoint base URL. Default value is
    https://api.powerbi.com
    
    client.BaseUri = new Uri(apiEndpointUri);
    
    return client;
    
    }

This method may be used to create a client reference to the Power BI
Embedded service.

The next couple methods make it possible to create a new workspace if
needed and get a list of all available workspaces inside the collection:

    static async Task&lt;Workspace&gt; CreateWorkspace(string
    workspaceCollectionName, string workspaceName)
    
    {
    
    using (var client = await CreateClient())
    
    {
    
    return await
    client.Workspaces.PostWorkspaceAsync(workspaceCollectionName, new
    CreateWorkspaceRequest(workspaceName));
    
    }
    
    }
    
    static async Task&lt;IEnumerable&lt;Workspace&gt;&gt;
    GetWorkspaces(string workspaceCollectionName)
    
    {
    
    using (var client = await CreateClient())
    
    {
    
    var response = await
    client.Workspaces.GetWorkspacesByCollectionNameAsync(workspaceCollectionName);
    
    return response.Value;
    
    }
    
    }

The methods above are used to check if internal and external workspaces
exist. If they do not, they should be created:

    var workspaces = await GetWorkspaces(workspaceCollectionName);
    
    var internalWorkspace = (from a in workspaces where
    a.DisplayName.Equals(workspaceNameInternal) select a).FirstOrDefault();
    
    if (internalWorkspace==null)
    
    {
    
    internalWorkspace = await CreateWorkspace(workspaceCollectionName,
    workspaceNameInternal);
    
    Console.WriteLine("Workspace for internal reports is created");
    
    }
    
    else
    
    {
    
    Console.WriteLine("Workspace for internal reports is found");
    
    }

In order to upload new reports to the collection, the existing one must
first be deleted.

**Learning**: It was not clear how to delete existing reports, but the
team learned that if you delete a dataset that is associated with a
report, the report will be deleted as well. In general, any reports
cannot exist without a dataset.

To delete existing datasets in a workspace it is possible to use the
following method:

    static async Task DeleteAllDatasets(string workspaceCollectionName,
    string workspaceId)
    
    {
    
    using (var client = await CreateClient())
    
    {
    
    ODataResponseListDataset response = await
    client.Datasets.GetDatasetsAsync(workspaceCollectionName, workspaceId);
    
    if (response.Value.Any())
    
    {
    
    foreach (Dataset d in response.Value)
    
    {
    
    await client.Datasets.DeleteDatasetByIdAsync(workspaceCollectionName,
    workspaceId,d.Id);
    
    Console.WriteLine("{0}: {1}", d.Name, d.Id);
    
    }
    
    }
    
    else
    
    {
    
    Console.WriteLine("No Datasets found in this workspace");
    
    }
    
    }
    
    }

The Power BI Embedded API makes it possible to work with datasets by ID.
The next step is to upload reports and update connection strings:

    foreach (var file in files)
    
    {
    
    Console.WriteLine(\$"Importing {file}");
    
    string dataSetTempName = String.Format(\$"dataSetName
    {Guid.NewGuid().ToString()}");
    
    var import = await ImportPbix(workspaceCollectionName,
    internalWorkspace.WorkspaceId, dataSetTempName, file);
    
    Console.WriteLine(\$"Updating connection string for {file}");
    
    var dataSetID = (from a in import.Datasets where
    a.Name.Equals(dataSetTempName) select a.Id).FirstOrDefault();
    
    await UpdateConnection(workspaceCollectionName,
    internalWorkspace.WorkspaceId, dataSetID, sqlUserName, sqlPassword);
    
    }

In the code above, it is clear that the ImportPbix method accepts a
dataset name, but not ID. The ID itself should be generated by Azure
once a new report is added. This is why it is important to use a return
value to update existing connection strings (see below).

**Learning**: The report can contain imported data and information about
the connection string, but the login and password should be eliminated
to guarantee security. This is why it is important to provide a login
and a password to the data source once a new report is uploaded.

**Learning**: The dataset name can be any string, so if the goal is to
create a “universal” web application that will show all available
reports, it is better to use a custom name from a configuration file
that generates report names automatically. To accomplish this, a
configuration file was added that is passed to the application as a
command line parameter. To simplify the format for the partner, this was
formatted as a list of strings, ReportDescription.txt:

    external
    
    C:\\Users\\sbaydach\\Source\\Repos\\Power BI
    Analytics\\ProvisionPowerBIWorkspaces\\ProvisionPowerBIWorkspaces\\Reports\\External\\basicReport.pbix
    
    viaSport Basic Report
    
    internal
    
    C:\\Users\\sbaydach\\Source\\Repos\\Power BI
    Analytics\\ProvisionPowerBIWorkspaces\\ProvisionPowerBIWorkspaces\\Reports\\Internal\\basicReportsLive.pbix


    viaSport Basic LIVE Report

The first string is a type of report (external or internal). The second
string is a path to existing report. Finally, the last string is a
report (dataset) name.

Below are two methods that upload a new report and update a connection
string:

    static async Task&lt;Import&gt; ImportPbix(string
    workspaceCollectionName, string workspaceId, string datasetName, string
    filePath)
    
    {
    
    using (var fileStream = File.OpenRead(filePath.Trim('"')))
    
    {
    
    using (var client = await CreateClient())
    
    {
    
    // Set request timeout to support uploading large PBIX files
    
    client.HttpClient.Timeout = TimeSpan.FromMinutes(60);
    
    client.HttpClient.DefaultRequestHeaders.Add("ActivityId",
    Guid.NewGuid().ToString());
    
    // Import PBIX file from the file stream
    
    var import = await
    client.Imports.PostImportWithFileAsync(workspaceCollectionName,
    workspaceId, fileStream, datasetName);
    
    // Example of polling the import to check when the import has succeeded.
    
    while (import.ImportState != "Succeeded" && import.ImportState !=
    "Failed")
    
    {
    
    import = await
    client.Imports.GetImportByIdAsync(workspaceCollectionName, workspaceId,
    import.Id);
    
    Console.WriteLine("Checking import state... {0}", import.ImportState);
    
    Thread.Sleep(1000);
    
    }
    
    return import;
    
    }
    
    }
    
    }
    
    static async Task UpdateConnection(string workspaceCollectionName,
    string workspaceId, string datasetId, string login, string password)
    
    {
    
    using (var client = await CreateClient())
    
    {
    
    var datasources = await
    client.Datasets.GetGatewayDatasourcesAsync(workspaceCollectionName,
    workspaceId, datasetId);
    
    // Reset your connection credentials
    
    var delta = new GatewayDatasource
    
    {
    
    CredentialType = "Basic",
    
    BasicCredentials = new BasicCredentials
    
    {
    
    Username = login,
    
    Password = password
    
    }
    
    };
    
    if (datasources.Value.Count != 1)
    
    {
    
    Console.Write("Expected one datasource, updating the first");
    
    }
    
    // Update the datasource with the specified credentials
    
    await client.Gateways.PatchDatasourceAsync(workspaceCollectionName,
    workspaceId, datasources.Value\[0\].GatewayId,
    datasources.Value\[0\].Id, delta);
    
    }
    
    }

The final application can be executed from the Command Prompt windows
with just one parameter: a configuration file with information about the
reports.

### Data Storage in Relation to PowerBI

As mentioned in the Database section of this report, the decision to use
Azure SQL was also informed by its support for Live (Direct Source) and
Imported (data pre-uploaded to pbix) reports.

The following diagram shows the tables used in the analytics reports:

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/viaSport_database_table_reports.jpg.png)

Here is the list of all tables:

-   **Session** – a table that contains information about each new
    session including channel, conversation id and start date and time.
    This table exposes information about the number of users using the
    bot and some additional information about them (like channel type);

-   **Query** – this table stores all queries from users. Due to privacy
    concerns, the text of the messages is not stored, but information
    about query time is stored in order to create some related objects
    like Entity, ReferencesProvided and Intent. This information will
    allow the viaSport team to understand how many intents and entities
    were recognized (an indicator of the quality of the LUIS model) and
    number of provided references (to determine whether the user was
    helped);

-   **Entity** – entities that are provided by LUIS;

-   **Intent** – intents that are provided by LUIS;

-   **References** – this table contains references and is not related
    to reporting directly, but is used to populate the
    ReferencesProvided table;

-   **ReferencesProvided** – contains links to references that were
    provided in a session.

**Learning:** All tables contain an identity key **ID**. It was not wise
to use this name because Power BI Desktop uses keys to predict all
possible relations in the database. As a result, the team spent
considerable time deleting incorrectly-formed relations based on ID
fields (just for imported reports).

**Learning:** One potential approach is to store EntityName and
IntentName fields as unique values, however, implementing LUIS training
functionality in the code would simplify these tables. Instead, in the
viaSport bot, EntityName and IntentName are just basic strings and
grouping is required to select number of unique intents/entities.

### 

### Telemetry

Implementing telemetry is not challenging task. Because C\# and Entity
Framework were used, it was as simple as updating the entity framework
model and implementing a method that save telemetry data to the database
based on parameters:

    public static void UpdateAnalyticDatabase(
    
    string intentName=null,
    
    double intentScore=0,
    
    Dictionary&lt;string, EntityRecommendation&gt; entities=null,
    
    List&lt;Reference&gt; referenceIds=null)
    
    {
    
    if (intentName == null)
    
    {
    
    AddEmptyQuery();
    
    }
    
    else if (entities == null)
    
    {
    
    AddIntentQuery(intentName,(float)intentScore);
    
    }
    
    else if (referenceIds == null)
    
    {
    
    AddIntentEntitiesQuery(intentName, (float)intentScore,entities);
    
    }
    
    else
    
    {
    
    AddIntentEntitiesReferencesQuery(intentName, (float)intentScore,
    entities, referenceIds);
    
    }
    
    }

There are four different method calls inside that update the database
based on type of query (availability of the parameters).

Below is the code of the most complex method from the list:

    private static void AddIntentEntitiesReferencesQuery(string intentName,
    float intentScore, Dictionary&lt;string, EntityRecommendation&gt;
    entities,
    
    List&lt;Reference&gt; referenceIds)
    
    {
    
    Query q = new Query
    
    {
    
    SessionId = currentSessionId,
    
    UtcDateTime = DateTime.UtcNow,
    
    Entities = new EntitySet&lt;Entity&gt;(),
    
    ReferenceProvideds = new EntitySet&lt;ReferenceProvided&gt;(),
    
    Intents = new EntitySet&lt;Intent&gt;()
    
    { new Intent() { IntentName = intentName, IntentScore = intentScore} }
    
    };
    
    foreach (var e in entities)
    
    {
    
    if (e.Value.Score!=null)
    
    q.Entities.Add(new Entity() { EntityValue = e.Value.Entity, EntityType =
    e.Value.Type, EntityScore = (float)e.Value.Score });
    
    }
    
    foreach (var rid in referenceIds)
    
    {
    
    q.ReferenceProvideds.Add(new ReferenceProvided() {ReferenceId =
    rid.Id});
    
    }
    
    context.Queries.InsertOnSubmit(q);
    
    context.SubmitChanges();
    
    }

The most important challenge there is just finding the right place to call
this method from the bot.

### Design and Publishing

Power BI Desktop is a free tool that makes it possible to design Direct
Query (Live) and Imported reports:

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/viasport_powerbi_desktop.png)

The Power BI Desktop tool can be downloaded and installed using this
link: <https://powerbi.microsoft.com/en-us/desktop/>

**Learning:** There is no way to switch between Import and Direct Query
(or opposite way) during the design process. If it becomes necessary to
switch partway through development, a new report must be designed from
scratch.

**Learning:** Power BI Embedded doesn’t allow direct updates of the data
from code/Azure in Import mode. It means that in order to update an
imported report in Azure, it is necessary to use Power BI Desktop to
update all data and republish the report using the console application,
which is more challenging. Note that this is not the case for the O365
version of Power BI.

Once Power BI Desktop is connected to the database and all needed tables
are selected, it is possible to start building reports using existing
visuals.

For the project, five different reports were built. Here are three of
them:

1.  General Report that contains information about visitors and the most
    popular references

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/viaSport_report_general.png)

2.  By Query Report that shows different kinds of queries (by subject,
    by sport name, by disability type)

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/viaSport_report_byquery.png)

3.  Performance Report that shows the most popular intents and number of
    successful and unsuccessful queries. Note that the report below is
    connected to the development database, which was used for testing,
    so the data displayed below reflects a greater number of
    unsuccessful queries than successful ones. This data will be
    eliminated from the reports when the bot goes live, so data
    presented to viaSport will reflect real user queries.

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/viaSport_report_performance.png)

The reports were designed for both live and import mode. Once in
production, viaSport will be able to select which reports to publish
externally.

**Learning**: As described in the previous section, it is important to
check relations in Import mode because some of them could be predicted
incorrectly. This can be done using the Relationships tab

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/viasport_powerbi_relationships_tab.png)

**Learning**: It is possible to create new columns based on expressions
for both types of reports, but expressions will be different.
DirectQuery Mode has some limitations that are related to measurement
fields and datatime field requires additional expression to be used. For
example:

    UtcDate =
    DATE(YEAR(Query\[UtcDateTime\]),MONTH(Query\[UtcDateTime\]),DAY(Query\[UtcDateTime\]))

In the Import mode, a general datetime can be used and it will be
possible to select needed components. As a result, DirectQuery reports
have a different interface regarding filters by datetime.

### Displaying Reports

The Power BI Embedded SDK supports several APIs including ASP.NET MVC,
JavaScript and ASP.NET Web Forms. To publish existing reports for this
project, the team decided to use the following existing open source
ASP.NET MVC template:
<https://github.com/Azure-Samples/power-bi-embedded-integrate-report-into-web-app/tree/master/EmbedSample>

Authentication was added and the template was adapted to work with two
workspaces at the same time:

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/viaSport_live_reports_dashboard.png)

Once the bot is in production reports will be embedded into production
portal.

Speech Recognition Experiments
------------------------------

Since the target audience for the viaSport Inclusion Bot includes people
with disabilities, it is important to consider different input forms and
accessible design when designing the bot. Though out of scope of the
original project goals, the team decided to investigate the possibility
of introducing speech interactions as a means of communicating with the
bot. At the beginning of this experiment, the team tested the bot with
the Windows 10 screen reader, which did not pick up the text in the web
chat window. Tested in multiple browsers - Edge, Internet Explorer, and
Google Chrome – this lack of functionality is a serious accessibility
issue. The team has provided this feedback to the Bot Framework team.

For the scope of this project, the bot was published as a web chat only,
but for this purpose of experimenting with speech, the team created a
UWP client that, if extended to full functionality in future, can be
installed directly from the viaSport website to support users who
require voice interactions.

There wasn’t enough time left in the project to build a fully functional
client, but there were significant learnings that we recognized
developing Windows 10 prototype.

**Learning**: In order to make Speech work with LUIS, Direct Line 3.0
must be activated through the LUIS portal. Adding the DirectLine package
using NuGet brings to light some conflicts – the latest version (3.0)
requires the newer version of .NET Core for UWP, but this version does
not work with Visual Studio 2015, which is the version of Visual Studio
that was used to develop this bot. The following figure shows the
configuration that worked in VS 2015:

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/viaSport_nuget_configuration_2.png)

**Learning**: It is possible to implement Speech-to-Text using the UWP
API, but doing so with the Windows 10 Anniversary edition or newer will
require the user to accept a special policy. The policy cannot be
accepted from the application and the setting is hidden deeply in the
Windows Settings – a potential problem in usability if the purpose of
this application is accessibility.

![](https://github.com/viaSport/viaSport-Inclusion-Bot/blob/master/images/viaSport_user_permission_speech.png)

**Learning**: When implementing a custom bot interface in a UWP app, it
is not possible to use ListView and other collection-enabled user
controls due to different kind of information from the bot (text, cards,
etc.). Therefore it is better to use a StackPanel with ScrollViewer and
build custom user controls per message type.

**Learning**: There is very little documentation currently available
that demonstrate how to implement a custom bot interface. Here are the
steps as identified by the development team:

To initialize a conversation you can use the following block:

    async Task InitializeBotConversation()
    
    {
    
    client = new DirectLineClient("key");
    
    conversation = await client.Conversations.StartConversationAsync();
    
    await StartListening();
    
    await ReadBotMessagesAsync(client, conversation.ConversationId);
    
    }

The following event handler reads all messages from the bot and display
them one by one. Pay attention that the collection with messages
contains all messages (users and bots) including old messages:

    async Task ReadBotMessagesAsync(DirectLineClient client, string
    conversationId)
    
    {
    
    string watermark = null;
    
    while (true)
    
    {
    
    var messages = await
    client.Conversations.GetActivitiesAsync(conversationId);
    
    watermark = messages?.Watermark;
    
    var messagesFromBotText = from x in messages.Activities
    
    select x;
    
    foreach (var message in messagesFromBotText)
    
    {
    
    await
    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
    
    () =&gt;
    
    {
    
    var res = (from a in MessageIDs where a == message.Id select a).Count();
    
    if (res==0)
    
    {
    
    MessageIDs.Add(message.Id);
    
    if (message.From.Name.Contains("viaSport"))
    
    {
    
    if ((message.Text != null) && (message.Text.Length &gt; 0))
    
    {
    
    chatWindow.Children.Add(new BotMessageText() { Text = message.Text });
    
    TalkSomething(message.Text);
    
    }
    
    else if (message.Attachments.Count&gt;0)
    
    {
    
    }
    
    }
    
    else
    
    {
    
    chatWindow.Children.Add(new UserMessageText() { Text = \$"Sergii:
    {message.Text}" });
    
    }
    
    }
    
    });
    
    }
    
    await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);
    
    }
    
    }

Finally, the following code shows how to send messages to a user:

    async Task sendMessageToBot(string text)
    
    {
    
    if (conversation!=null)
    
    {
    
    IMessageActivity message = Activity.CreateMessageActivity();
    
    message.From = new ChannelAccount() { Id = "sbaydach@microsoft.com",
    Name = "Sergii" }; ;
    
    message.Text = text;
    
    message.Locale = "en-Us";
    
    await
    client.Conversations.PostActivityAsync(conversation.ConversationId,
    (Activity)message);
    
    }
    
    }

Conclusion
==========

In the weeks following the viaSport Inclusion Bot development project,
viaSport will deploy the bot to a private preview with selected partners
to test the bot, retrieve early analytics, and, most importantly,
determine areas of weakness that must be strengthened before the bot is
released to the public. Mark Schramm of Fusionware will continue to lead
ongoing development of this bot, and members of the development team
have volunteered to support the bot in the future.

As the project sees success and becomes well-used, there may be
opportunity to explore expansion of the bot to different channels and
communication methods. Building out the speech app would make the bot
more accessible to users who require voice-based interactions and the
use of screen readers, which is key to inclusion and accessibility.
Further to that, publishing the bot to further channels (i.e. Skype,
Messenger, Kik, etc.) would allow users to interact with the bot in
their own preferred chat platform. It would also open the doors to
implementing proactive notifications, whereby the bot could remember
when a user’s query did not match any resources in the database and,
when a new resource is added, could send a notification to all the users
who had previously requested such a resource, notifying them that a new
resource has been added that might be relevant to them.

The viaSport Inclusion Bot has the potential to positively impact the
atmosphere for inclusive sport in British Columbia by providing viaSport
with a channel to deliver their carefully curated resources to their
users in a unique and targeted manner. By proactively prompting the user
for information and guiding the conversation, the bot aids users to find
resources that are relevant to their precise needs. Cognitive Services
APIs add intelligence to the bot, making the user experience more
streamlined and proactive. Analytics dashboards equip viaSport with
reports that will enable them to improve their resources based on user
interactions, as well as leverage specific insights in conversations
with partners and sponsors to prove their impact on the community.

In the quest to promote inclusivity, a key step is making existing
resources available to those who need them most. This bot and the
accompanying administrative app provide a fully manageable and
intelligent solution for viaSport to access their audience in a new and
impactful way, to play a role in changing the conversation by subtly
phasing out the use of outdated terminology, and to act as the leader
for inclusive sport in British Columbia and Canada.
