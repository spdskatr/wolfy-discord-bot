using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wolfy.Commands.Workers
{
    public class CommandWorker_ComplexInteractive : CommandWorker_Simple
    {
        class InteractiveNode
        {
#pragma warning disable CS0649
            public string trigger;
            public string mode = "whole";
            public string response;
#pragma warning restore CS0649
            public bool MatchesTrigger(string text)
            {
                return text.IsMatch(trigger, mode);
            }
        }
        public HashSet<ulong> membersWaitingForInteraction = new HashSet<ulong>();
        InteractiveNode[] nodes = new InteractiveNode[0];
        public override void LoadDataFromJson(JToken tok)
        {
            base.LoadDataFromJson(tok);
            if (tok["nodes"] != null)
            {
                nodes = JsonConvert.DeserializeObject<InteractiveNode[]>(tok["nodes"].ToString());
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
                if (membersWaitingForInteraction.Contains(e.Author.Id))
                {
                    InteractiveNode node = nodes.FirstOrDefault(n => n.MatchesTrigger(e.Message.Content));
                    if (node != null)
                    {
                        await e.Message.RespondAsync(node.response);
                        membersWaitingForInteraction.Remove(e.Author.Id);
                    }
                }
            }
            return result;
        }
    }
}
