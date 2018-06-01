using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using DSharpPlus;

namespace Wolfy.Commands.Workers
{
    public abstract class CommandWorker
    {
        DiscordClient client;

        public DiscordClient Client => client;

        public void RegisterClient(DiscordClient client)
        {
            this.client = client;
        }
        public abstract void LoadDataFromJson(JToken tok);
        public abstract Task Process(MessageCreateEventArgs e);
    }
}
