using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscoBottan.Commands {
    public class NoobCommand : ICommand {
        private string _player;

        public NoobCommand(string player) {
            _player = player;
        }

        public string RunCommand(DiscordUser author) {
            if (string.IsNullOrWhiteSpace(_player)) {
                return "Hvem er en noob?";
            }

            if (_player.ToLower() == "jeg" && author.Username.ToLower() == "siggy" && author.Discriminator == "9248") {
                return "Ja, det er du. :joy:";
            }
            if (_player.ToLower() == "siggy") {
                return "Ja, det er han. :smile:";
            } 

            return "Nej da.";
        }
    }
}