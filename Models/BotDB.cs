using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;

namespace viaSportResourceBot.Models
{
    public class BotDB
    {
        private BotDataClassesDataContext context;
        public BotDB()
        {
            context = new BotDataClassesDataContext();
        }

        public string GetString(string locale, string name)
        {
            if (string.IsNullOrEmpty(locale))
                locale = "en-US";

            var result = context.AnswerStrings
                .Where(c => c.AnswerType == name)
                .OrderBy(c => Guid.NewGuid());

            var list = result.ToList();

            var r = list.OrderBy(c => Guid.NewGuid()).First();

            if (r != null && r.AnswerText != null)
                return r.AnswerText;
            else
                return "";
        }

       public bool CheckSportList(string name)
        {
            var result = from a in context.Activities
                         where a.Name == Singularize(name)
                         select a;
            return result.Any();
        }

        public bool CheckDisabilitiesList(string name)
        {
            var result = from a in context.Disabilities
                         where a.Name == Singularize(name)
                         select a;
            var synonymResults = from a in context.DisabilitySynonyms
                         where a.Synonym == Singularize(name)
                         select a;

            return result.Any() || synonymResults.Any() ;
        }

        public bool CheckSubjectList(string name)
        {
            var result = from a in context.Subjects
                         where a.Name == Singularize(name)
                         select a;

            var synonymResults = from a in context.SubjectSynonyms
                                 where a.Synonym == Singularize(name)
                                 select a;

            return result.Any() || synonymResults.Any();
        }

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
            //a few hardcoded examples we will likely encounter, the Pluralizer doesn't recognize these as plurals
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

        public List<Reference> QueryReferences(string intent, string activity, string disability, string subject, int skip = 0, int take = 1000)
        {
            if (!string.IsNullOrEmpty(subject))
            {
                subject = (from a in context.SubjectSynonyms
                              where a.Synonym == Singularize(subject)
                              select a.Subject).FirstOrDefault()?.Name ?? subject;
            }

            if (!string.IsNullOrEmpty(disability))
            {
                disability = (from a in context.DisabilitySynonyms
                              where a.Synonym == Singularize(disability)
                              select a.Disability).FirstOrDefault()?.Name ?? disability;
            }

            if (!string.IsNullOrEmpty(disability) && !string.IsNullOrEmpty(activity) && !string.IsNullOrEmpty(subject))
            {
                var result = (from r in context.References
                             join ra in context.ReferenceActivities.DefaultIfEmpty() on r.Id equals ra.ReferenceId
                             join a in context.Activities.DefaultIfEmpty() on ra.ActivityId equals a.Id
                             join da in context.ReferenceDisabilities.DefaultIfEmpty() on r.Id equals da.ReferenceId
                             join d in context.Disabilities.DefaultIfEmpty() on da.DisabilityId equals d.Id
                             join sa in context.ReferenceSubjects.DefaultIfEmpty() on r.Id equals sa.ReferenceId
                             join s in context.Subjects.DefaultIfEmpty() on sa.SubjectId equals s.Id
                             where r.Intent == intent &&
                                     a.Name == Singularize(activity) &&
                                     d.Name == Singularize(disability) &&
                                     s.Name == Singularize(subject)
                             select r).Skip(skip).Take(take);
                return result.ToList();
            }
            if (!string.IsNullOrEmpty(disability) && !string.IsNullOrEmpty(subject))
            {
                var result = (from r in context.References
                             join da in context.ReferenceDisabilities.DefaultIfEmpty() on r.Id equals da.ReferenceId
                             join d in context.Disabilities.DefaultIfEmpty() on da.DisabilityId equals d.Id
                             join sa in context.ReferenceSubjects.DefaultIfEmpty() on r.Id equals sa.ReferenceId
                             join s in context.Subjects.DefaultIfEmpty() on sa.SubjectId equals s.Id
                             where r.Intent == intent &&
                                     d.Name == Singularize(disability) &&
                                     s.Name == Singularize(subject)
                             select r).Skip(skip).Take(take);
                return result.ToList();
            }
            if (!string.IsNullOrEmpty(activity) && !string.IsNullOrEmpty(subject))
            {
                var result = (from r in context.References
                             join ra in context.ReferenceActivities.DefaultIfEmpty() on r.Id equals ra.ReferenceId
                             join a in context.Activities.DefaultIfEmpty() on ra.ActivityId equals a.Id
                             join sa in context.ReferenceSubjects.DefaultIfEmpty() on r.Id equals sa.ReferenceId
                             join s in context.Subjects.DefaultIfEmpty() on sa.SubjectId equals s.Id
                             where r.Intent == intent &&
                                     a.Name == Singularize(activity) &&
                                     s.Name == Singularize(subject)
                             select r).Skip(skip).Take(take);
                return result.ToList();
            }
            if (!string.IsNullOrEmpty(disability) && !string.IsNullOrEmpty(activity))
            {
                var result = (from r in context.References
                             join ra in context.ReferenceActivities.DefaultIfEmpty() on r.Id equals ra.ReferenceId
                             join a in context.Activities.DefaultIfEmpty() on ra.ActivityId equals a.Id
                             join da in context.ReferenceDisabilities.DefaultIfEmpty() on r.Id equals da.ReferenceId
                             join d in context.Disabilities.DefaultIfEmpty() on da.DisabilityId equals d.Id
                             where r.Intent == intent &&
                                     a.Name == Singularize(activity) &&
                                     d.Name == Singularize(disability)
                             select r).Skip(skip).Take(take);
                return result.ToList();
            }
            if (!string.IsNullOrEmpty(disability))
            {
                var result = (from r in context.References
                             join da in context.ReferenceDisabilities.DefaultIfEmpty() on r.Id equals da.ReferenceId
                             join d in context.Disabilities.DefaultIfEmpty() on da.DisabilityId equals d.Id
                             where r.Intent == intent &&
                                     d.Name == Singularize(disability)
                             select r).Skip(skip).Take(take);
                return result.ToList();
            }

            if (!string.IsNullOrEmpty(activity))
            {
                var result = (from r in context.References
                             join ra in context.ReferenceActivities.DefaultIfEmpty() on r.Id equals ra.ReferenceId
                             join a in context.Activities.DefaultIfEmpty() on ra.ActivityId equals a.Id
                             where r.Intent == intent &&
                                     a.Name == Singularize(activity)
                             select r).Skip(skip).Take(take);
                return result.ToList();
            }
            if (!string.IsNullOrEmpty(subject))
            {
                var result = (from r in context.References
                             join sa in context.ReferenceSubjects.DefaultIfEmpty() on r.Id equals sa.ReferenceId
                             join s in context.Subjects.DefaultIfEmpty() on sa.SubjectId equals s.Id
                             where r.Intent == intent &&
                                     s.Name == Singularize(subject)
                              select r).Skip(skip).Take(take);
                return result.ToList();
            }
            if (string.IsNullOrEmpty(activity) && string.IsNullOrEmpty(disability) && string.IsNullOrEmpty(subject))
            {
                var result = (from r in context.References
                            where r.Intent == intent
                            select r).Skip(skip).Take(take);
                return result.ToList();
            }

            return new List<Reference>();
        }


    }
}