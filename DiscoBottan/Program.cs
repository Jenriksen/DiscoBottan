using System;
using System.IO;
using System.Threading.Tasks;
using DiscoBottan.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;

namespace DiscoBottan
{
    public class Program
    {
        public DiscordClient Client;
        public CommandsNextModule Commands { get; set; }

        static void Main()
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
            Client.Ready += debug.Client_Ready;
            Client.ClientErrored += debug.Client_ClientError;

            var ccfg = new CommandsNextConfiguration
            {
                // Use string prefix from config.json
                //StringPrefix = cfgjson.CommandPrefix,
                
                // enables responding in direct messages
                EnableDms = true,
                EnableMentionPrefix = true
            };
            
            // hooking up commands
            Commands = Client.UseCommandsNext(ccfg);
            
            // Event logging of commands
            Commands.CommandExecuted += Commands_CommandExecuted;
            Commands.CommandErrored += Commands_CommandErrored;
            
            // let's add a converter for a custom type and a name
            var mathopcvt = new MathOperationConverter();
            CommandsNextUtilities.RegisterConverter(mathopcvt);
            CommandsNextUtilities.RegisterUserFriendlyTypeName<MathOperation>("operation");
            
            // registrering commands
            Commands.RegisterCommands<ExampleUngrouppedCommands>();
            Commands.RegisterCommands<ExampleGrouppedCommands>();
            Commands.RegisterCommands<ExampleExecutableGroup>();
            
            // custom formatting help
            Commands.SetHelpFormatter<SimpleHelpFormatter>();
                
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

        private Task Commands_CommandExecuted(CommandExecutionEventArgs e)
        {
            // Logging the command used and the user.
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Info, "DiscoBottan", $"{e.Context.User.Username} successfully executed '{e.Command.QualifiedName}'", DateTime.Now);
            return Task.CompletedTask;
        }

        private async Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            // Let's log the error details
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Error, "DiscoBottan", $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}", DateTime.Now);
            
            
            // Let's check if the error is a result of lack of required permissions
            if (e.Exception is ChecksFailedException ex)
            {
                // yes, the user lacks required persmissions, let them know
                var emoji = DiscordEmoji.FromName(e.Context.Client, ":no_entry:");

                var embed = new DiscordEmbedBuilder
                {
                    Title = "Access denied",
                    Description = $"{emoji} You do not have the permissions required to execute this command.",
                    Color = new DiscordColor(0xFF000) // red
                };
                await e.Context.RespondAsync("", embed: embed);
            }
        }
    }
}
