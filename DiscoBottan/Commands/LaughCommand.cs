using DSharpPlus.Entities;
using System;

namespace DiscoBottan.Commands {
    public class LaughCommand : ICommand
    {
        private readonly Random _random = new Random();
        public string RunCommand(DiscordUser author)
        {
            var laugh = (Laughs)_random.Next(4);

            string response;
            switch (laugh) {
                case Laughs.Ha:
                    response = Haha();
                    break;
                case Laughs.He:
                    response = Heh();
                    break;
                case Laughs.Lol:
                    response = Lol();
                    break;
                case Laughs.Lul:
                    response = "lul";
                    break;
                default:
                    response = "Jeg er en kage";
                    break;
            }

            return response;
        }

        private string Haha() {
            var numberOfHas = _random.Next(2, 10);

            var haha = "";
            for (var i = 0; i < numberOfHas; i++) {
                haha += "ha";
            }

            if (_random.Next(2) == 1) {
                haha = haha.ToUpper();
            }

            return haha;
        }

        private string Heh() {
            return "heh";
        }

        private string Lol() {
            if (_random.Next(100) > 95) {
                var howFunny = _random.Next(15);
                string laugh = "l";
                for (var i = 0; i < howFunny; i++) {
                    laugh += "o";
                }
                return laugh + "l";
            } else {
                return "lol";
            }
        }
        private enum Laughs {
            Ha,
            He,
            Lol,
            Lul
        }
    }
}