using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;


namespace DiscoBottan.Commands
{
    public class ExampleUngrouppedCommands
    {
        [Command("Ping")] // Method defining as a command
        [DSharpPlus.CommandsNext.Attributes.Description("Example ping command")]
        [Aliases("pong")]
        // alternative names
        public async Task Ping(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var emoji = DiscordEmoji.FromName(ctx.Client, ":wave");

            await ctx.RespondAsync($"{emoji} Pong! Ping: {ctx.Client.Ping}ms");
        }

        [Command("greet"), DSharpPlus.CommandsNext.Attributes.Description("Says hi to specified user."), Aliases("sayhi", "say_hi")]
        public async Task Greet(CommandContext ctx, [DSharpPlus.CommandsNext.Attributes.Description("The user to say hi to.")]
            DiscordMember member) // this command takes a member as an argument; you can pass one by username, nickname, id, or mention
        {
            // note the [Description] attribute on the argument.
            // this will appear when people invoke help for the
            // command.

            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            // let's make the message a bit more colourful
            var emoji = DiscordEmoji.FromName(ctx.Client, ":wave:");

            // and finally, let's respond and greet the user.
            await ctx.RespondAsync($"{emoji} Hello, {member.Mention}!");
        }


        [Command("sum"), DSharpPlus.CommandsNext.Attributes.Description("Sums all given numbers and returns said sum.")]
        public async Task SumOfNumbers(CommandContext ctx, [DSharpPlus.CommandsNext.Attributes.Description("Integers to sum.")] params int[] args)
        {
            // triggers typing indicator to show that we are working.
            await ctx.TriggerTypingAsync();

            // sum calculation
            var sum = args.Sum();

            // send it to end user.
            await ctx.RespondAsync($"The sum of these numbers is {sum.ToString("#,##0")}");
        }

        [Command("math"), DSharpPlus.CommandsNext.Attributes.Description("Does basic math.")]
        public async Task Math(CommandContext ctx, [DSharpPlus.CommandsNext.Attributes.Description("Operation to perform on the operands.")]
            MathOperation operation, [DSharpPlus.CommandsNext.Attributes.Description("First operand.")] double num1,
            [DSharpPlus.CommandsNext.Attributes.Description("Second operand.")] double num2)
        {
            var result = 0.0;

            switch (operation)
            {
                case MathOperation.Add:
                    result = num1 + num2;
                    break;
                case MathOperation.Substract:
                    result = num1 - num2;
                    break;
                case MathOperation.Multiply:
                    result = num1 * num2;
                    break;
                case MathOperation.Divide:
                    result = num1 / num2;
                    break;
                case MathOperation.Modulo:
                    result = num1 % num2;
                    break;
            }

            var emoji = DiscordEmoji.FromName(ctx.Client, ":1234:");
            await ctx.RespondAsync($"{emoji} The result is {result.ToString("#,##0.00")}");
        }
    }


    [Group("admin")] // marks a command group
    [DSharpPlus.CommandsNext.Attributes.Description("Administrative commands.")]
    [Hidden] //hides it from curious users
    [RequirePermissions(Permissions.ManageGuild)]
    // restricts usage to appropriate permission level.
    public class ExampleGrouppedCommands
    {
        [Command("sudo"), DSharpPlus.CommandsNext.Attributes.Description("Executes a command as another user."), Hidden, RequireOwner]
        public async Task Sudo(CommandContext ctx, [DSharpPlus.CommandsNext.Attributes.Description("Member to execute as.")] DiscordMember member,
            [RemainingText, DSharpPlus.CommandsNext.Attributes.Description("Command text to execute.")]
            string command)
        {
            await ctx.TriggerTypingAsync();

            // Get the command service needed for sudo purposes.
            var cmds = ctx.CommandsNext;

            // performs the sudo
            await cmds.SudoAsync(member, ctx.Channel, command);
        }

        [Command("nick"), DSharpPlus.CommandsNext.Attributes.Description("Gives someone a new nickname."), RequirePermissions(Permissions.ManageNicknames)]
        public async Task ChangeNickname(CommandContext ctx, [DSharpPlus.CommandsNext.Attributes.Description("Member to change the nickname for.")]
            DiscordMember member, [RemainingText, DSharpPlus.CommandsNext.Attributes.Description("The nivkname to give that user.")]
            string newNickname)
        {
            await ctx.TriggerTypingAsync();

            try
            {
                await member.ModifyAsync(newNickname, reason: $"Changed by {ctx.User.Username} ({ctx.User.Id}).");

                // simple response
                var emoji = DiscordEmoji.FromName(ctx.Client, ":+1:");

                await ctx.RespondAsync(emoji);
            }
            catch (Exception)
            {
                var emoji = DiscordEmoji.FromName(ctx.Client, ":-1");
                await ctx.RespondAsync(emoji);
                throw;
            }
        }
    }

    [Group("memes", CanInvokeWithoutSubcommand =
        true)] // this makes the class a group, but with a twist; the class now needs an ExecuteGroupAsync method
    [DSharpPlus.CommandsNext.Attributes.Description("Contains some memes. When invoked without subcommand, returns a random one.")]
    [Aliases("copypasta")]
    public class ExampleExecutableGroup
    {
        // commands in this group need to be executed as 
        // <prefix>memes [command] or <prefix>copypasta [command]

        // this is the group's command; unlike with other commands, 
        // any attributes on this one are ignored, but like other
        // commands, it can take arguments

        public async Task ExecuteGroupAsync(CommandContext ctx)
        {
            var rnd = new Random();
            var nxt = rnd.Next(0, 2);

            switch (nxt)
            {
                case 0:
                    await Pepe(ctx);
                    return;
                case 1:
                    await NavySeal(ctx);
                    return;
                case 2:
                    await Kekistani(ctx);
                    return;
            }
        }

        [Command("pepe"), Aliases("feelsbadman"), DSharpPlus.CommandsNext.Attributes.Description("Feels bad, man.")]
        public async Task Pepe(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            // wrap it into an embed
            var embed = new DiscordEmbedBuilder
            {
                Title = "Pepe",
                ImageUrl = "http://i.imgur.com/44SoSqS.jpg"
            };
            await ctx.RespondAsync(embed: embed);
        }

        [Command("navyseal"), Aliases("gorillawarfare"), DSharpPlus.CommandsNext.Attributes.Description("What the fuck did you just say to me?")]
        public async Task NavySeal(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync(
                "What the fuck did you just fucking say about me, you little bitch? I’ll have you know I graduated top of my class in the Navy Seals, and I’ve been involved in numerous secret raids on Al-Quaeda, and I have over 300 confirmed kills. I am trained in gorilla warfare and I’m the top sniper in the entire US armed forces. You are nothing to me but just another target. I will wipe you the fuck out with precision the likes of which has never been seen before on this Earth, mark my fucking words. You think you can get away with saying that shit to me over the Internet? Think again, fucker. As we speak I am contacting my secret network of spies across the USA and your IP is being traced right now so you better prepare for the storm, maggot. The storm that wipes out the pathetic little thing you call your life. You’re fucking dead, kid. I can be anywhere, anytime, and I can kill you in over seven hundred ways, and that’s just with my bare hands. Not only am I extensively trained in unarmed combat, but I have access to the entire arsenal of the United States Marine Corps and I will use it to its full extent to wipe your miserable ass off the face of the continent, you little shit. If only you could have known what unholy retribution your little “clever” comment was about to bring down upon you, maybe you would have held your fucking tongue. But you couldn’t, you didn’t, and now you’re paying the price, you goddamn idiot. I will shit fury all over you and you will drown in it. You’re fucking dead, kiddo.");
        }

        [Command("kekistani"), Aliases("kek", "normies"), DSharpPlus.CommandsNext.Attributes.Description("I'm a proud ethnic Kekistani.")]
        public async Task Kekistani(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync(
                "I'm a proud ethnic Kekistani. For centuries my people bled under Normie oppression. But no more. We have suffered enough under your Social Media Tyranny. It is time to strike back. I hereby declare a meme jihad on all Normies. Normies, GET OUT! RRRÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆ﻿");
        }

        // this is a subgroup; you can nest groups as much 
        // as you like
        [Group("mememan", CanInvokeWithoutSubcommand = true), Hidden]
        public class MemeMan
        {
            public async Task ExecuteGroupAsync(CommandContext ctx)
            {
                await ctx.TriggerTypingAsync();

                // wrap it into an embed
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Meme man",
                    ImageUrl = "http://i.imgur.com/tEmKtNt.png"
                };
                await ctx.RespondAsync(embed: embed);
            }

            [Command("ukip"), DSharpPlus.CommandsNext.Attributes.Description("The UKIP pledge.")]
            public async Task Ukip(CommandContext ctx)
            {
                await ctx.TriggerTypingAsync();

                // wrap it into an embed
                var embed = new DiscordEmbedBuilder
                {
                    Title = "UKIP pledge",
                    ImageUrl = "http://i.imgur.com/ql76fCQ.png"
                };
                await ctx.RespondAsync(embed: embed);
            }

            [Command("lineofsight"), DSharpPlus.CommandsNext.Attributes.Description("Line of sight.")]
            public async Task Los(CommandContext ctx)
            {
                await ctx.TriggerTypingAsync();

                // wrap it into an embed
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Line of sight",
                    ImageUrl = "http://i.imgur.com/ZuCUnEb.png"
                };
                await ctx.RespondAsync(embed: embed);
            }

            [Command("art"), DSharpPlus.CommandsNext.Attributes.Description("Art.")]
            public async Task Art(CommandContext ctx)
            {
                await ctx.TriggerTypingAsync();

                // wrap it into an embed
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Art",
                    ImageUrl = "http://i.imgur.com/VkmmmQd.png"
                };
                await ctx.RespondAsync(embed: embed);
            }

            [Command("seeameme"), DSharpPlus.CommandsNext.Attributes.Description("When you see a meme.")]
            public async Task SeeMeme(CommandContext ctx)
            {
                await ctx.TriggerTypingAsync();

                // wrap it into an embed
                var embed = new DiscordEmbedBuilder
                {
                    Title = "When you see a meme",
                    ImageUrl = "http://i.imgur.com/8GD0hbZ.jpg"
                };
                await ctx.RespondAsync(embed: embed);
            }

            [Command("thisis"), DSharpPlus.CommandsNext.Attributes.Description("This is meme man.")]
            public async Task ThisIs(CommandContext ctx)
            {
                await ctx.TriggerTypingAsync();

                // wrap it into an embed
                var embed = new DiscordEmbedBuilder
                {
                    Title = "This is meme man",
                    ImageUrl = "http://i.imgur.com/57vDOe6.png"
                };
                await ctx.RespondAsync(embed: embed);
            }

            [Command("deepdream"), DSharpPlus.CommandsNext.Attributes.Description("Deepdream'd meme man.")]
            public async Task DeepDream(CommandContext ctx)
            {
                await ctx.TriggerTypingAsync();

                // wrap it into an embed
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Deep dream",
                    ImageUrl = "http://i.imgur.com/U666J6x.png"
                };
                await ctx.RespondAsync(embed: embed);
            }

            [Command("sword"), DSharpPlus.CommandsNext.Attributes.Description("Meme with a sword?")]
            public async Task Sword(CommandContext ctx)
            {
                await ctx.TriggerTypingAsync();

                // wrap it into an embed
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Meme with a sword?",
                    ImageUrl = "http://i.imgur.com/T3FMXdu.png"
                };
                await ctx.RespondAsync(embed: embed);
            }

            [Command("christmas"), DSharpPlus.CommandsNext.Attributes.Description("Beneath the christmas spike...")]
            public async Task ChristmasSpike(CommandContext ctx)
            {
                await ctx.TriggerTypingAsync();

                // wrap it into an embed
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Christmas spike",
                    ImageUrl = "http://i.imgur.com/uXIqUS7.png"
                };
                await ctx.RespondAsync(embed: embed);
            }

        }
    }
}