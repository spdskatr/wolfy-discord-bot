using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using Newtonsoft.Json.Linq;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System.Text.RegularExpressions;

namespace Wolfy.Commands.Workers
{
    public class CommandWorker_SimpleInteractive : CommandWorker_Simple
    {
        public string waitFor;
        public string waitMode = "whole";
        public string waitReply;
        public override void LoadDataFromJson(JToken tok)
        {
            base.LoadDataFromJson(tok);
            if (tok["waitFor"] != null)
            {
                waitFor = tok["waitFor"].Value<string>();
            }
            if (tok["waitMode"] != null)
            {
                waitMode = tok["waitMode"].Value<string>();
            }
            if (tok["waitReply"] != null)
            {
                waitReply = tok["waitReply"].Value<string>();
            }
        }
        protected override async Task SendMessage(MessageCreateEventArgs e)
        {
            await base.SendMessage(e);
            Client.GetInteractivityModule().WaitForMessageAsync(TriggeredMessage, TimeSpan.FromMinutes(5)).ContinueWith(t => { if (t.Result == null) e.Message.RespondAsync(waitReply); });
        }
        public bool TriggeredMessage(DiscordMessage message)
        {
            bool triggered = false;
            switch (waitMode)
            {
                case "whole_case_sensitive":
                    triggered = message.Content == waitFor;
                    break;
                case "whole":
                    triggered = message.Content.ToLower() == waitFor.ToLower();
                    break;
                case "contains":
                    triggered = message.Content.ToLower().Contains(waitFor.ToLower());
                    break;
                case "regex_case_sensitive":
                    triggered = Regex.IsMatch(message.Content, waitFor);
                    break;
                case "regex":
                    triggered = Regex.IsMatch(message.Content, waitFor, RegexOptions.IgnoreCase);
                    break;
                default:
                    break;
            }
            return triggered;
        }
    }
}
