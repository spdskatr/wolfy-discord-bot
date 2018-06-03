using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Wolfy.Commands.Workers
{
    public class CommandWorker_WolfyDoAFlip : CommandWorker_Simple
    {
        protected override async Task SendMessage(MessageCreateEventArgs e)
        {
            DiscordMessage mess = await e.Message.RespondAsync("Watch this!");
            await Task.Delay(1000);
            await mess.ModifyAsync("<:awoo:254007902510120961>");
            await Task.Delay(1000);
            await mess.ModifyAsync("<:goingupawoo:258576454114213888>");
            await Task.Delay(1000);
            await mess.ModifyAsync("<:upsideawoo:258576454101499904>");
            await Task.Delay(1000);
            await mess.ModifyAsync("<:sideawoo:258576454663536650>");
            await Task.Delay(1000);
            await mess.ModifyAsync("<:awoo:254007902510120961>");
            await Task.Delay(1000);
            await mess.ModifyAsync("Tada!~");
        }
    }
}
