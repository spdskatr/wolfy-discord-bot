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
    public class CommandWorker_ChanceOptions : CommandWorker_Simple
    {
        class ChanceOption
        {
#pragma warning disable CS0649
            public int weight;
            public string response;
#pragma warning restore CS0649
        }
        ChanceOption[] chances = new ChanceOption[0];
        public override void LoadDataFromJson(JToken tok)
        {
            base.LoadDataFromJson(tok);
            if (tok["chances"] != null)
            {
                chances = JsonConvert.DeserializeObject<ChanceOption[]>(tok["chances"].ToString());
            }
        }
        protected override async Task SendMessage(MessageCreateEventArgs e)
        {
            await base.SendMessage(e);
            await SendRandomMessage(e);
        }
        protected virtual async Task SendRandomMessage(MessageCreateEventArgs e)
        {
            if (chances.Length > 0)
            {
                int totalWeight = chances.Sum(c => c.weight);
                int result = Rand.Instance.Next(totalWeight);
                ChanceOption selected = null;
                int cumulative = 0;
                for (int i = 0; i < chances.Length; i++)
                {
                    selected = chances[i];
                    cumulative += selected.weight;
                    if (result < cumulative)
                    {
                        break;
                    }
                }
                if (selected != null)
                {
                    await e.Message.RespondAsync(selected.response);
                }
            }
        }
    }
}
