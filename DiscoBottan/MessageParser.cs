using Sprache;
using System;
using System.Collections.Generic;
using System.Text;
using DiscoBottan.Commands;

namespace DiscoBottan {
    public static class MessageParser {

        private static Parser<ICommand> timeIsIt = from word in Parse.String("er")
                                                   from space in Parse.WhiteSpace.Many()
                                                   from time in Parse.String("klokken")
                                                   from questionMark in Parse.Char('?').Optional()
                                                   select new WhatTimeIsItCommand();

        private static Parser<ICommand> whatParser = from what in Parse.String("hvad").Or(Parse.String("Hvad"))
                                                     from initialspace in Parse.WhiteSpace.Many()
                                                     from middle in timeIsIt
                                                     select middle;

        private static Parser<ICommand> siggyParser = from word in Parse.String("Er").Or(Parse.String("er"))
                                                      from who in Parse.AnyChar.Until(Parse.String(" en noob")).Text()
                                                      from question in Parse.Char('?').Optional()
                                                      select new NoobCommand(who.Trim());

        private static Parser<ICommand> garbage = from garbage in Parse.AnyChar.Many()
                                                  select new GarbageCommand();

        private static Parser<ICommand> laugh = from laugh in Parse.Regex("(h(e|a|i)|l(o|u|aw)l)")
                                                select new LaughCommand();

        private static Parser<ICommand> Parser =
            whatParser
            .Or(siggyParser)
            .Or(laugh)
            .Or(garbage);

        public static ICommand ParseIt(string message) {
            return Parser.Parse(message);
        }
    }
}
