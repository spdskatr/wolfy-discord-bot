using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;

namespace Wolfy.Commands.Workers
{
    public class CommandCondition_OnlyInDms : CommandCondition
    {
        public override bool CanExecute(MessageCreateEventArgs e)
        {
            return e.Channel is DiscordDmChannel;
        }
    }
}
