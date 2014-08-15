using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LunchHourGames.Console
{
    public interface ConsoleInterpreter
    {
        void parse(LHGConsole console, String[] arguments);
        void showWelcome(LHGConsole console, String message);
    }
}
