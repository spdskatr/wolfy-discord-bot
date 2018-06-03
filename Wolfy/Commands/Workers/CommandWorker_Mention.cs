using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Wolfy.Commands.Workers
{
    public class CommandWorker_Mention : CommandWorker_Simple
    {
        protected override Task SendMessage(MessageCreateEventArgs e)
        {
            return e.Message.RespondAsync(string.Format(response, e.Message.MentionedUsers.Select(u => u.Mention).ToArray()));
        }
    }
}
