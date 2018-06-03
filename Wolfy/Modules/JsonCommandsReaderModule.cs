using DSharpPlus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wolfy.Commands.Workers;

namespace Wolfy.Modules
{
    public class JsonCommandsReaderModule : BaseModule
    {
        string json;
        public List<CommandWorker> workers = new List<CommandWorker>();
        protected override void Setup(DiscordClient client)
        {
            Client = client;
            json = File.ReadAllText("Data/commands_simple.json");
            JArray jArray = JArray.Parse(json);
            foreach (JToken tok in jArray)
            {
                string type = tok["type"].Value<string>();
                Type t = Type.GetType("Wolfy.Commands.Workers." + type);
                if (t != null)
                {
                    if (typeof(CommandWorker).IsAssignableFrom(t))
                    {
                        CommandWorker cw = (CommandWorker)Activator.CreateInstance(t);
                        cw.RegisterClient(client);
                        cw.LoadDataFromJson(tok);
                        workers.Add(cw);
                        client.DebugLogger.LogMessage(LogLevel.Debug, "Wolfy", $"Loaded command {cw}", DateTime.Now);
                    }
                    else
                    {
                        client.DebugLogger.LogMessage(LogLevel.Error, "Wolfy", $"Exception loading command workers: Type {t} does not derive from Wolfy.Commands.Workers.CommandWorker\r\n\r\nData: {tok}", DateTime.Now);
                    }
                }
                else
                {
                    client.DebugLogger.LogMessage(LogLevel.Error, "Wolfy", $"Exception loading command workers: Could not find type Wolfy.Commands.Workers.{type}\r\n\r\nData: {tok}", DateTime.Now);
                }
            }
            client.MessageCreated += e => Task.WhenAny(from cw in workers select cw.Process(e));
        }
    }
}
