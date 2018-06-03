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
        public string trigger;
        public string response;
        public string mode = "whole";
        public double chance = 1.0;
        public long cooldown;
        public List<CommandCondition> conditions = new List<CommandCondition>();

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
            if (tok["conditions"] != null)
            {
                foreach (JToken cond in tok["conditions"])
                {
                    Type t = Type.GetType("Wolfy.Commands.Workers." + cond["type"]);
                    if (t != null)
                    {
                        if (typeof(CommandCondition).IsAssignableFrom(t))
                        {
                            CommandCondition cw = (CommandCondition)Activator.CreateInstance(t);
                            cw.LoadDataFromJson(cond);
                            conditions.Add(cw);
                        }
                        else
                        {
                            Client.DebugLogger.LogMessage(LogLevel.Error, "Wolfy", $"Exception loading command conditions for worker {this}: Type {t} does not derive from Wolfy.Commands.Workers.CommandCondition\r\n\r\nData: {tok}", DateTime.Now);
                        }
                    }
                    else
                    {
                        Client.DebugLogger.LogMessage(LogLevel.Error, "Wolfy", $"Exception loading command conditions for worker {this}: Could not find type Wolfy.Commands.Workers.{t}\r\n\r\nData: {cond}", DateTime.Now);
                    }
                }
            }
        }

        public override async Task<bool> Process(MessageCreateEventArgs e)
        {
            if (e.Message.Content.IsMatch(trigger, mode))
            {
                if (cooldown <= 0 || Client.GetModule<CooldownManagerModule>().CanRunCommand(GetUniqueId(), cooldown))
                {
                    if (chance >= 1.0 || chance > Rand.Instance.NextDouble())
                    {
                        if (conditions.All(c => c.CanExecute(e)))
                        {
                            Client.GetModule<CooldownManagerModule>().CommandRun(GetUniqueId());
                            await SendMessage(e);
                            return true;
                        }
                    }
                }
            }
            return false;
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
            return $"[ {string.Join(", ", Describe().OrderBy(s => s))} ]";
        }

        public virtual IEnumerable<string> Describe()
        {
            yield return $"trigger: {trigger}";
            yield return $"response: {response}";
            yield return $"mode: {mode}";
            yield return $"chance: {chance}";
            yield return $"cooldown: {cooldown}";
            yield return $"conditions: [{string.Join(", ", conditions.OrderBy(c => c.ToString()))}]";
        }
    }
}
