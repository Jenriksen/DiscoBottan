using DSharpPlus.Entities;
using System;

namespace DiscoBottan.Commands {
    class WhatTimeIsItCommand : ICommand {
        public string RunCommand(DiscordUser author) {
            return "Klokken er " + DateTime.Now.ToShortTimeString();
        }
    }
}
