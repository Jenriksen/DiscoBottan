using DSharpPlus.Entities;
using System;

namespace DiscoBottan.Commands {
    public class LaughCommand : ICommand
    {
        private readonly Random random = new Random();
        public string RunCommand(DiscordUser author)
        {
            var laugh = (Laughs)random.Next(4);

            string response;
            switch (laugh) {
                case Laughs.ha:
                    response = haha();
                    break;
                case Laughs.he:
                    response = heh();
                    break;
                case Laughs.lol:
                    response = lol();
                    break;
                case Laughs.lul:
                    response = "lul";
                    break;
                default:
                    response = "Jeg er en kage";
                    break;
            }

            return response;
        }

        private string haha() {
            var numberOfHas = random.Next(2, 10);

            var haha = "";
            for (var i = 0; i < numberOfHas; i++) {
                haha += "ha";
            }

            if (random.Next(2) == 1) {
                haha = haha.ToUpper();
            }

            return haha;
        }

        private string heh() {
            return "heh";
        }

        private string lol() {
            if (random.Next(100) > 95) {
                var howFunny = random.Next(15);
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
            ha,
            he,
            lol,
            lul
        }
    }
}