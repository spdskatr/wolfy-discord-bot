using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using Wolfy.Commands;

namespace Wolfy
{
    public class Program
    {
        public DiscordClient client;
        public CommandsNextModule commandsNext;
        public InteractivityModule interactivity;

        public Program()
        {
            string token = File.ReadAllText("Data/auth.txt");
            client = new DiscordClient(new DiscordConfiguration()
            {
                UseInternalLogHandler = true,
#if DEBUG
                LogLevel = LogLevel.Debug,
#else
                LogLevel = LogLevel.Info,
#endif
                TokenType = TokenType.Bot,
                Token = token
            });
            commandsNext = client.UseCommandsNext(new CommandsNextConfiguration()
            {
                EnableDms = false,
                EnableMentionPrefix = false,
                EnableDefaultHelp = false,
                StringPrefix = "!"
            });
            client.ClientErrored += Client_ClientErrored;
            AddAllModules();
        }

        private Task Client_ClientErrored(DSharpPlus.EventArgs.ClientErrorEventArgs e)
        {
            e.Client.DebugLogger.LogMessage(LogLevel.Error, "DSharpPlus", "Unhandled exception - message: " + e.Exception, DateTime.Now);
            return Task.CompletedTask;
        }

        void AddAllModules()
        {
            Stopwatch stopwatch = new Stopwatch();
            client.DebugLogger.LogMessage(LogLevel.Info, "Wolfy", "Loading modules...", DateTime.Now);
            stopwatch.Start();
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (typeof(BaseModule).IsAssignableFrom(t))
                {
                    try
                    {
                        BaseModule mod = (BaseModule)Activator.CreateInstance(t);
                        client.AddModule(mod);
                        client.DebugLogger.LogMessage(LogLevel.Debug, "Wolfy", $"Loaded module {t.FullName}", DateTime.Now);
                    }
                    catch (Exception e)
                    {
                        client.DebugLogger.LogMessage(LogLevel.Error, "Wolfy", $"Unhandled exception while trying to import module {t.FullName}: {e}", DateTime.Now);
                    }
                }
                else if (t.IsPublic && t.GetCustomAttribute<CommandModuleAttribute>() != null)
                {
                    try
                    {
                        commandsNext.RegisterCommands(t);
                        client.DebugLogger.LogMessage(LogLevel.Debug, "Wolfy", $"Loaded commands {t.FullName}", DateTime.Now);
                    }
                    catch (Exception e)
                    {
                        client.DebugLogger.LogMessage(LogLevel.Error, "Wolfy", $"Unhandled exception while trying to import commands {t.FullName}: {e}", DateTime.Now);
                    }
                }
            }
            stopwatch.Stop();
            client.DebugLogger.LogMessage(LogLevel.Info, "Wolfy", $"Finished loading modules in {stopwatch.ElapsedMilliseconds}ms", DateTime.Now);
        }

        public async Task StartAsync()
        {
            await client.ConnectAsync();
            await Task.Delay(-1);
        }

        static void Main(string[] args)
        {
            Program prog = new Program();
            prog.StartAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
