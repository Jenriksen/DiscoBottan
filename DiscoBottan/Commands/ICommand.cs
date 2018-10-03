using DSharpPlus.Entities;

namespace DiscoBottan.Commands {
    public interface ICommand {
        string RunCommand(DiscordUser author);
    }
}
