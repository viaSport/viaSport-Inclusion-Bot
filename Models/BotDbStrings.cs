using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.Bot.Connector;

namespace viaSportResourceBot.Models
{
    public static class BotDbStrings
    {
        private static BotDataClassesDataContext context;
        static BotDbStrings()
        {
            context = new BotDataClassesDataContext();
        }

        public static string MakeItAcceptable(string message)
        {
            var prefMessages = from a in context.LanguageCorrections select a;

            StringBuilder sb = new StringBuilder(message);
            sb.Append(' ');
            sb.Insert(0, ' ');

            foreach (var item in prefMessages)
            {
                sb.Replace(String.Format($" {item.OutdatedTerm} "), String.Format($" {item.PreferredTerm} "));
                sb.Replace(String.Format($" {item.OutdatedTerm}."), String.Format($" {item.PreferredTerm}."));
                sb.Replace(String.Format($" {item.OutdatedTerm}!"), String.Format($" {item.PreferredTerm}!"));
                sb.Replace(String.Format($" {item.OutdatedTerm},"), String.Format($" {item.PreferredTerm},"));
                sb.Replace(String.Format($" {item.OutdatedTerm}?"), String.Format($" {item.PreferredTerm}?"));
            }
            sb.Remove(0, 1);
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}