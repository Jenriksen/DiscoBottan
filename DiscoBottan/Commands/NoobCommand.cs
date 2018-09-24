using System;
using System.Collections.Generic;
using System.Text;

namespace DiscoBottan.Commands {
    public class NoobCommand : ICommand {
        private string _player;
        public NoobCommand(string player) {
            _player = player;
        }
        public string RunCommand() {
            if (_player == null) {
                return "Hvem er en noob?";
            }

            if (_player.ToLower() == "siggy") {
                return "Ja, det er han.";
            } else {
                return "Nej da.";
            }
        }
    }
}
