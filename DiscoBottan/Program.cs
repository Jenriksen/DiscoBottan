using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DiscoBottan
{
    public class Program
    {
        public DiscordClient Client;

        static void Main(string[] args)
        {
            var prog = new Program();
            prog.RunBotAsync().GetAwaiter().GetResult();
        }

        public async Task RunBotAsync()
        {
            var token = File.ReadAllText("../../../../.token");
            Client = new DiscordClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            });
           

            Debug debug = new Debug();
            this.Client.Ready += debug.Client_Ready;
            this.Client.ClientErrored += debug.Client_ClientError;

            Client.MessageCreated += async e =>
            {
                var response = MessageParser.ParseIt(e.Message.Content).RunCommand(e.Author);

                if (!string.IsNullOrWhiteSpace(response)) {
                    await e.Message.RespondAsync(response);
                }
            };

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        public struct ConfigJson
        {
            [JsonProperty("token")]
            public string Token { get; private set; }

            [JsonProperty("prefix")]
            public string CommandPrefix { get; private set; }
        }
        
        // Debug logging handled here:
        private Task Client_Ready(ReadyEventArgs e)
        {
            // Lets log the fact that this event occured.
            e.Client.DebugLogger.LogMessage(LogLevel.Info, "DiscoBottan", "Client is ready to process events.", DateTime.Now);
            return Task.CompletedTask;
        }

        private Task Client_ClientError(ClientErrorEventArgs e)
        {
            e.Client.DebugLogger.LogMessage(LogLevel.Error, "DiscoBottan", $"Exception occured: {e.Exception.GetType()}: {e.Exception.Message}", DateTime.Now);
            return Task.CompletedTask;
        }
        
    }
}
