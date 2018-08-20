using System;
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
            _discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "NDc3OTI0NTA4NzIwOTU1NDAz.DlDN1Q.-yAj4VY12STJDypGhm30-AunpIs",
                TokenType = TokenType.Bot
            });

            _discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("ping"))
                    await e.Message.RespondAsync("pong!");
            };

            await _discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
