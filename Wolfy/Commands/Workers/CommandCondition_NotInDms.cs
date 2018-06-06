using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Wolfy.Commands.Workers
{
    public class CommandCondition_NotInDms : CommandCondition
    {
        public override bool CanExecute(MessageCreateEventArgs e)
        {
            return !(e.Channel is DiscordDmChannel);
        }
    }
}
