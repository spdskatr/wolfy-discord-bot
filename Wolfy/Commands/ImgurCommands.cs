using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Wolfy.Commands
{
    [CommandModule]
    public class ImgurCommands
    {
#pragma warning disable CS0649
        class ImgurImage
        {
            public string link;
        }
        class ImgurResponse
        {
            public bool success;
            public int status;
            public ImgurImage[] data;
        }
#pragma warning restore CS0649
        [Command("awoo")]
        public async Task Awoo(CommandContext ctx)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", Program.instance.auth[1]);
                    string result = await client.GetStringAsync("https://api.imgur.com/3/album/leJob/images");
                    ImgurResponse res = JsonConvert.DeserializeObject<ImgurResponse>(result);
                    if (res.success)
                    {
                        await ctx.RespondAsync(res.data[Rand.Instance.Next(res.data.Length)].link);
                    }
                    else
                    {
                        await ctx.RespondAsync("Huh? Imgur isn't working properly! ;~;");
                    }
                }
                catch (Exception)
                {
                    await ctx.RespondAsync("Huh? Imgur isn't working properly! ;~;");
                }
            }
        }
        [Command("winnie")]
        public async Task Winnie(CommandContext ctx)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", Program.instance.auth[1]);
                    string result = await client.GetStringAsync("https://api.imgur.com/3/album/VyRD4/images");
                    ImgurResponse res = JsonConvert.DeserializeObject<ImgurResponse>(result);
                    if (res.success)
                    {
                        await ctx.RespondAsync(res.data[Rand.Instance.Next(res.data.Length)].link);
                    }
                    else
                    {
                        await ctx.RespondAsync("Huh? Imgur isn't working properly! ;~;");
                    }
                }
                catch (Exception)
                {
                    await ctx.RespondAsync("Huh? Imgur isn't working properly! ;~;");
                }
            }
        }
    }
}
