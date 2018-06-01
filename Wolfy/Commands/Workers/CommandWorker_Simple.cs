using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Newtonsoft.Json.Linq;
using Wolfy.Modules;
using System.Text.RegularExpressions;

namespace Wolfy.Commands.Workers
{
    public class CommandWorker_Simple : CommandWorker
    {
        public static Random random = new Random();
        public string trigger;
        public string response;
        public string mode = "whole";
        public double chance = 1.0;
        public long cooldown;

        public override void LoadDataFromJson(JToken tok)
        {
            if (tok["trigger"] != null)
            {
                trigger = tok["trigger"].Value<string>();
            }
            if (tok["response"] != null)
            {
                response = tok["response"].Value<string>();
            }
            if (tok["mode"] != null)
            {
                mode = tok["mode"].Value<string>();
            }
            if (tok["chance"] != null)
            {
                chance = tok["chance"].Value<double>();
            }
            if (tok["cooldown"] != null)
            {
                cooldown = tok["cooldown"].Value<long>();
            }
        }

        public override async Task Process(MessageCreateEventArgs e)
        {
            bool triggered = false;
            switch (mode)
            {
                case "whole_case_sensitive":
                    triggered = e.Message.Content == trigger;
                    break;
                case "whole":
                    triggered = e.Message.Content.ToLower() == trigger.ToLower();
                    break;
                case "contains":
                    triggered = e.Message.Content.ToLower().Contains(trigger.ToLower());
                    break;
                case "regex_case_sensitive":
                    triggered = Regex.IsMatch(e.Message.Content, trigger);
                    break;
                case "regex":
                    triggered = Regex.IsMatch(e.Message.Content, trigger, RegexOptions.IgnoreCase);
                    break;
                default:
                    break;
            }
            if (triggered)
            {
                if (cooldown <= 0 || Client.GetModule<CooldownManagerModule>().CanRunCommand(GetUniqueId(), cooldown))
                {
                    if (chance >= 1.0 || chance > random.NextDouble())
                    {
                        Client.GetModule<CooldownManagerModule>().CommandRun(GetUniqueId());
                        await SendMessage(e);
                    }
                }
            }
        }

        protected virtual Task SendMessage(MessageCreateEventArgs e)
        {
            return e.Message.RespondAsync(response);
        }

        public string GetUniqueId()
        {
            return "Simple_" + trigger;
        }

        public override string ToString()
        {
            return $"[ trigger: {trigger}, response: {response}, mode: {mode}, chance: {chance}, cooldown: {cooldown} ]";
        }
    }
}
