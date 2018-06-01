using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfy.Modules
{
    public class CooldownManagerModule : BaseModule
    {
        Dictionary<string, DateTime> cooldownBuckets = new Dictionary<string, DateTime>();
        protected override void Setup(DiscordClient client)
        {
            Client = client;
        }
        public bool CanRunCommand(string key, long ms)
        {
            if (cooldownBuckets.ContainsKey(key))
            {
                return (DateTime.Now - cooldownBuckets[key]).TotalMilliseconds > ms;
            }
            return true;
        }
        public void CommandRun(string key)
        {
            cooldownBuckets[key] = DateTime.Now;
        }
        public void Clear()
        {
            cooldownBuckets.Clear();
        }
    }
}
