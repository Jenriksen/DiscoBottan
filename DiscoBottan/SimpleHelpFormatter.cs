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
            this.MessageBuilder = new StringBuilder();
        }

        public IHelpFormatter WithCommandName(string name)
        {
            this.MessageBuilder.Append("Command: ")
                .AppendLine(Formatter.Bold(name))
                .AppendLine();
            return this;
        }

        public IHelpFormatter WithDescription(string Description)
        {
            this.MessageBuilder.Append("Description: ")
                .AppendLine(Description)
                .AppendLine();
            return this;
        }

        public IHelpFormatter WithGroupExecutable()
        {
            this.MessageBuilder.AppendLine("This group is a standalone command.").AppendLine();
            return this;
        }

        public IHelpFormatter WithAliases(IEnumerable<string> aliases)
        {
            this.MessageBuilder.Append("Aliases: ")
                .AppendLine(string.Join(", ", aliases))
                .AppendLine();

            return this;
        }
        
        public IHelpFormatter WithArguments(IEnumerable<CommandArgument> arguments)
        {
            this.MessageBuilder.Append("Arguments: ")
                .AppendLine(string.Join(", ", arguments.Select(xarg => $"{xarg.Name} ({xarg.Type.ToUserFriendlyName()})")))
                .AppendLine();

            return this;
        }
        
        public IHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            this.MessageBuilder.Append("Subcommands: ")
                .AppendLine(string.Join(", ", subcommands.Select(xc => xc.Name)))
                .AppendLine();

            return this;
        }

        public CommandHelpMessage Build()
        {
            return new CommandHelpMessage(this.MessageBuilder.ToString().Replace("\r\n", "\n"));
        }
        
        
    }
}