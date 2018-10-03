using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;

namespace DiscoBottan
{
    public class SimpleHelpFormatter : IHelpFormatter
    {
        private StringBuilder MessageBuilder { get; }

        public SimpleHelpFormatter()
        {
            MessageBuilder = new StringBuilder();
        }

        public IHelpFormatter WithCommandName(string name)
        {
            MessageBuilder.Append("Command: ")
                .AppendLine(Formatter.Bold(name))
                .AppendLine();
            return this;
        }

        public IHelpFormatter WithDescription(string description)
        {
            MessageBuilder.Append("Description: ")
                .AppendLine(description)
                .AppendLine();
            return this;
        }

        public IHelpFormatter WithGroupExecutable()
        {
            MessageBuilder.AppendLine("This group is a standalone command.").AppendLine();
            return this;
        }

        public IHelpFormatter WithAliases(IEnumerable<string> aliases)
        {
            MessageBuilder.Append("Aliases: ")
                .AppendLine(string.Join(", ", aliases))
                .AppendLine();

            return this;
        }
        
        public IHelpFormatter WithArguments(IEnumerable<CommandArgument> arguments)
        {
            MessageBuilder.Append("Arguments: ")
                .AppendLine(string.Join(", ", arguments.Select(xarg => $"{xarg.Name} ({xarg.Type.ToUserFriendlyName()})")))
                .AppendLine();

            return this;
        }
        
        public IHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            MessageBuilder.Append("Subcommands: ")
                .AppendLine(string.Join(", ", subcommands.Select(xc => xc.Name)))
                .AppendLine();

            return this;
        }

        public CommandHelpMessage Build()
        {
            return new CommandHelpMessage(MessageBuilder.ToString().Replace("\r\n", "\n"));
        }
        
        
    }
}