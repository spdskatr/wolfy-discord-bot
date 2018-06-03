using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using Wolfy.Modules;

namespace Wolfy.Commands.Workers
{
    public class CommandWorker_ResetAllCooldowns : CommandWorker_Simple
    {
        public override async Task<bool> Process(MessageCreateEventArgs e)
        {
            bool result = await base.Process(e);
            if (result)
            {
                Client.GetModule<CooldownManagerModule>().Clear();
                await e.Message.RespondWithFileAsync("Data/kitty.png");
            }
            return result;
        }
    }
}
