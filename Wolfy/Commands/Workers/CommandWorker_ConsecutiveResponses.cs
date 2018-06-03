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
    public class CommandWorker_ConsecutiveResponses : CommandWorker_Simple
    {
        public int timeout;
        public string[] responses = new string[0];
        public override void LoadDataFromJson(JToken tok)
        {
            base.LoadDataFromJson(tok);
            if (tok["responses"] != null)
            {
                responses = JsonConvert.DeserializeObject<string[]>(tok["responses"].ToString());
            }
            if (tok["timeout"] != null)
            {
                timeout = tok["timeout"].Value<int>();
            }
        }
        public override async Task<bool> Process(MessageCreateEventArgs e)
        {
            bool result = await base.Process(e);
            if (result)
            {
                for (int i = 0; i < responses.Length; i++)
                {
                    await Task.Delay(1000);
                    await e.Message.RespondAsync(responses[i]);
                }
            }
            return result;
        }
    }
}
