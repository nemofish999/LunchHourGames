using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LunchHourGames.Console
{
    class DefaultInterpreter : ConsoleInterpreter
    {
        public void parse(LHGConsole console, String[] arguments)
        {
        }

        public void showWelcome(LHGConsole console, String message)
        {
            console.WriteLine("# Welcome to the Lunch Hour Games Console");
            console.WriteLine("# Trail of the Dead version 0.0.0.5 (prototype)");
            console.WriteLine("#");
            console.WriteLine("# Type 'help' to see the commands available.");
            console.WriteLine("# Use up and down arrows to see command history");
            console.WriteLine("#");
            console.WriteLine("# " + message);
            console.WriteLine("");
        }
    }
}
