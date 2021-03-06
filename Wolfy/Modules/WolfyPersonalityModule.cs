﻿using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfy.Modules
{
    // 10/10 name tho
    public class WolfyPersonalityModule : BaseModule
    {
        bool weekend = false;
        protected override void Setup(DiscordClient client)
        {
            Client = client;
            client.Heartbeated += Client_Heartbeated;
            client.Ready += Client_Ready;
        }

        async Task Client_Ready(ReadyEventArgs e)
        {
            DiscordChannel channel = await Client.GetChannelAsync(214523379766525963);
            if (channel != null)
            {
#if DEBUG
#else
                await channel.SendMessageAsync("I\'m back! <:awoo:254007902510120961>");
#endif
            }
        }

        async Task Client_Heartbeated(HeartbeatEventArgs e)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday && DateTime.Now.Hour > 9)
            {
                DiscordChannel channel = await Client.GetChannelAsync(214523379766525963);
                if (channel != null && !weekend)
                {
#if DEBUG
#else
                    await channel.SendMessageAsync("Yay it\'s finally the weekend! Hope everyone is having an amazing Saturday so far!\r\nhttp://i.imgur.com/VKDm9Pj.png");
#endif
                    weekend = true;
                }
            }
        }
    }
}
