using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LunchHourGames.Debugger
{
    public class Memento
    {
        public enum Type
        { 
            Player, 
            Obstacle 
        }

        private Type type;

        public Memento(Type type)
        {
            this.type = type;
        }
    }
}
