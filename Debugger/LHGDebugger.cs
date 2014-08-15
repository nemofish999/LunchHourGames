using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LunchHourGames.Players;

namespace LunchHourGames.Debugger
{
    public class LHGDebugger
    {
        private LunchHourGames lhg;

        private List<DebugLog> logs = new List<DebugLog>();

        public LHGDebugger(LunchHourGames lhg)
        {
            this.lhg = lhg;
        }

        //public void addEvent(Player player, DebugEvent debugEvent, string message, bool addMemento)
        //{
        //}

        public void addMessage(string message)
        {
        }



    }
}
