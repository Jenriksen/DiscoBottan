using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscoBottan.Commands {
    class WhatTimeIsItCommand : ICommand {
        public string RunCommand(DiscordUser author) {
            return "Klokken er " + DateTime.Now.ToShortTimeString();
        }
    }
}
