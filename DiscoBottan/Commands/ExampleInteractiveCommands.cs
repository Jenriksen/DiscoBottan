using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;

namespace DiscoBottan.Commands
{
    public class ExampleInteractiveCommands
    {
        [Command("poll"), Description("Run a poll with reactions.")]
        public async Task Poll(CommandContext ctx, [Description("How long should the poll last.")]
            TimeSpan duration, [Description("What options should people have.")]
            params DiscordEmoji[] options)
        {
            // Loads interactivity module and sets poll_options
            var interactivity = ctx.Client.GetInteractivityModule();
            var poll_options = options.Select(xe => xe.ToString());
            
            // presentation of poll
            var embed = new DiscordEmbedBuilder
            {
                Title = "Poll time!",
                Description = string.Join(" ", poll_options)
            };

            var msg = await ctx.RespondAsync(embed: embed);
            
            // add reactions as options
            for (var i = 0; i < options.Length; i++)
            {
                await msg.CreateReactionAsync(options[i]);
            }
            
            // collect and filter responses
            var poll_result = await interactivity.CollectReactionsAsync(msg, duration);
            var results = poll_result.Reactions.Where(xkvp => options.Contains(xkvp.Key))
                .Select(xkvp => $"{xkvp.Key}: {xkvp.Value}");
            
            // post results
            await ctx.RespondAsync(string.Join("\n", results));
        }

        [Command("waitforcode"), Description("Waits for a response containing a generated code.")]
        public async Task WaitForCode(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivityModule();
            
            // Generate a code
            var codebytes = new byte[8];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(codebytes);
            var code = BitConverter.ToString(codebytes).ToLower().Replace("-", "");
            
            // announcing code
            await ctx.RespondAsync($"The first one to type the following code gets a rewards: `{code}`");
            
            // Wait for anyone who types it
            var msg = await interactivity.WaitForMessageAsync(xm => xm.Content.Contains(code),
                TimeSpan.FromSeconds(60));
            if (msg != null)
            {
                // announcing winner
                await ctx.RespondAsync($"And the winner is: {msg.Message.Author.Mention}");
            }
            else
            {
                await ctx.RespondAsync("Nobody? Really?");
            }
        }

        [Command("waitforreact"), Description("Waits for a reaction.")]
        public async Task WaitForReaction(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivityModule();

            var emoji = DiscordEmoji.FromName(ctx.Client, ":point_up:");
            
            // wait for a reaction
            var em = await interactivity.WaitForReactionAsync(xe => xe == emoji, ctx.User, TimeSpan.FromSeconds(60));
            if (em != null)
            {
                // quote
                var embed = new DiscordEmbedBuilder
                {
                    Color = em.Message.Author is DiscordMember m ? m.Color : new DiscordColor(0xFF00FF),
                    Description = em.Message.Content,
                    Author = new DiscordEmbedBuilder.EmbedAuthor
                    {
                        Name = em.Message.Author is DiscordMember mx ? mx.DisplayName : em.Message.Author.Username,
                        IconUrl = em.Message.Author.AvatarUrl
                    }
                };
                await ctx.RespondAsync(embed: embed);
            }
            else
            {
                await ctx.RespondAsync("Seriously?");
            }
        }
    }
}