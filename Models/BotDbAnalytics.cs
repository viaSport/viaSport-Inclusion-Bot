using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace viaSportResourceBot.Models
{
    using Microsoft.Bot.Builder.Luis.Models;
    using System.Data.Linq;

    public static class BotDbAnalytics
    {
        private static BotDataClassesDataContext context;
        private static int currentSessionId;

        static BotDbAnalytics()
        {
            context = new BotDataClassesDataContext();
        }

        public static void AddSession(string channelId, string conversationId, DateTime date, string locale)
        {
            var result =
                (from a in context.Sessions
                 where a.ConversationId.Equals(conversationId)
                 select a).FirstOrDefault();
            if (result != null)
            {
                currentSessionId = result.ID;
            }
            else
            {


                Session s = new Session
                {
                    ConversationId = conversationId,
                    ChannelId = channelId,
                    Locale = locale,
                    UtcDateTime = date
                };
                context.Sessions.InsertOnSubmit(s);
                context.SubmitChanges();
                currentSessionId = s.ID;

            }
        }

        private static void AddEmptyQuery()
        {
            Query q = new Query
                          {
                              SessionId = currentSessionId, UtcDateTime = DateTime.UtcNow,
                              Intents=new EntitySet<Intent>()
                              { new Intent() { IntentName = "None"} }
                          };
            context.Queries.InsertOnSubmit(q);
            context.SubmitChanges();
        }

        private static void AddIntentQuery(string intentName,float intentScore)
        {
            Query q = new Query
            {
                SessionId = currentSessionId,
                UtcDateTime = DateTime.UtcNow,
                Intents = new EntitySet<Intent>()
                              { new Intent() { IntentName = intentName, IntentScore = intentScore} }
            };
            context.Queries.InsertOnSubmit(q);
            context.SubmitChanges();
        }

        private static void AddIntentEntitiesQuery(string intentName, float intentScore, Dictionary<string, EntityRecommendation> entities)
        {
            Query q = new Query
            {
                SessionId = currentSessionId,
                UtcDateTime = DateTime.UtcNow,
                Entities = new EntitySet<Entity>(),
                Intents = new EntitySet<Intent>()
                              { new Intent() { IntentName = intentName, IntentScore = intentScore} }
            };
            foreach (var e in entities)
            {
                if (e.Value.Score != null)
                    q.Entities.Add(new Entity() {EntityValue = e.Value.Entity, EntityType = e.Value.Type, EntityScore = (float)e.Value.Score});
            }
            context.Queries.InsertOnSubmit(q);
            context.SubmitChanges();
        }

        private static void AddIntentEntitiesReferencesQuery(string intentName, float intentScore, Dictionary<string, EntityRecommendation> entities,
            List<Reference> referenceIds)
        {
            Query q = new Query
            {
                SessionId = currentSessionId,
                UtcDateTime = DateTime.UtcNow,
                Entities = new EntitySet<Entity>(),
                ReferenceProvideds = new EntitySet<ReferenceProvided>(),
                Intents = new EntitySet<Intent>()
                              { new Intent() { IntentName = intentName, IntentScore = intentScore} }
            };
            foreach (var e in entities)
            {
                if (e.Value.Score!=null)
                    q.Entities.Add(new Entity() { EntityValue = e.Value.Entity, EntityType = e.Value.Type, EntityScore = (float)e.Value.Score });
            }
            foreach (var rid in referenceIds)
            {
                q.ReferenceProvideds.Add(new ReferenceProvided() {ReferenceId = rid.Id});
            }
            context.Queries.InsertOnSubmit(q);
            context.SubmitChanges();
        }

        public static void UpdateAnalyticDatabase(
            string intentName=null,
            double intentScore=0,
            Dictionary<string, EntityRecommendation> entities=null,
            List<Reference> referenceIds=null)
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
                AddIntentEntitiesReferencesQuery(intentName, (float)intentScore, entities, referenceIds);
            }
        }


    }


}