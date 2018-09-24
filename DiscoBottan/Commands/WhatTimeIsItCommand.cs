using System;
using System.Collections.Generic;
using System.Text;

namespace DiscoBottan.Commands {
    class WhatTimeIsItCommand : ICommand {
        public string RunCommand() {
            return "Klokken er " + DateTime.Now.ToShortTimeString();
        }
    }
}
