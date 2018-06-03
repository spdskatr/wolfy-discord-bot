using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using Newtonsoft.Json.Linq;

namespace Wolfy.Commands.Workers
{
    public abstract class CommandCondition
    {
        public abstract bool CanExecute(MessageCreateEventArgs e);
        public virtual void LoadDataFromJson(JToken tok)
        {
        }
    }
}
