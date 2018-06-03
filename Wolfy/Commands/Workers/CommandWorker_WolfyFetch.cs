using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;

namespace Wolfy.Commands.Workers
{
    public class CommandWorker_WolfyFetch : CommandWorker_ChanceOptions
    {
        protected override async Task SendRandomMessage(MessageCreateEventArgs e)
        {
            await Task.Delay(5000);
            await base.SendRandomMessage(e);
        }
    }
}
