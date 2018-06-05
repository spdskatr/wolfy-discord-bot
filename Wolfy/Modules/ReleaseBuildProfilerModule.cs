using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfy.Modules
{
    public class ReleaseBuildProfilerModule : BaseModule
    {
#if DEBUG
        protected override void Setup(DiscordClient client)
        {
        }
#else
        protected override void Setup(DiscordClient client)
        {
            Client = client;
            client.Heartbeated += Client_Heartbeated;
            client.Ready += Client_Ready;
        }

        private Task Client_Ready(DSharpPlus.EventArgs.ReadyEventArgs e)
        {
            e.Client.DebugLogger.LogMessage(LogLevel.Info, "Wolfy", "Ready!", DateTime.Now);
            return Task.CompletedTask;
        }

        private Task Client_Heartbeated(DSharpPlus.EventArgs.HeartbeatEventArgs e)
        {
            e.Client.DebugLogger.LogMessage(LogLevel.Info, "Wolfy", "Heartbeat - Ping " + e.Ping, DateTime.Now);
            return Task.CompletedTask;
        }
#endif
    }
}
