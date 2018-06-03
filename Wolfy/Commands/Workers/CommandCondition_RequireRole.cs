using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using Newtonsoft.Json.Linq;

namespace Wolfy.Commands.Workers
{
    public class CommandCondition_RequireRole : CommandCondition
    {
        public ulong id;
        public override bool CanExecute(MessageCreateEventArgs e)
        {
            return e.Guild.GetMemberAsync(e.Author.Id).ContinueWith(t => t.Result.Roles.Any(r => r.Id == id)).GetAwaiter().GetResult();
        }
        public override void LoadDataFromJson(JToken tok)
        {
            if (tok["id"] != null)
            {
                id = tok["id"].Value<ulong>();
            }
        }
        public override string ToString()
        {
            return "Require role " + id;
        }
    }
}
