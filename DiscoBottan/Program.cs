using System;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus;

namespace DiscoBottan
{
    class Program
    {
        private static DiscordClient _discord;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            var token = File.ReadAllText("../../../../.token");
            _discord = new DiscordClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot
            });

            _discord.MessageCreated += async e =>
            {
                var response = MessageParser.ParseIt(e.Message.Content).RunCommand(e.Author);

                if (!string.IsNullOrWhiteSpace(response)) {
                    await e.Message.RespondAsync(response);
                }
            };

            await _discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
