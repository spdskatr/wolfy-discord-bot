using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using Newtonsoft.Json.Linq;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Text.RegularExpressions;

namespace Wolfy.Commands.Workers
{
    public class CommandWorker_SimpleInteractive : CommandWorker_Simple
    {
        public HashSet<ulong> membersWaitingForInteraction = new HashSet<ulong>();
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
        
        public override async Task<bool> Process(MessageCreateEventArgs e)
        {
            bool result = await base.Process(e);
            if (result)
            {
                membersWaitingForInteraction.Add(e.Author.Id);
            }
            else
            {
                if (membersWaitingForInteraction.Contains(e.Author.Id) && e.Message.Content.IsMatch(waitFor, waitMode))
                {
                    await e.Message.RespondAsync(waitReply);
                    membersWaitingForInteraction.Remove(e.Author.Id);
                }
            }
            return result;
        }
    }
}
