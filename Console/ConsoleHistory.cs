using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LunchHourGames.Console
{
    public class ConsoleHistory
    {
        private List<string> history;
        private int index = 0;

        public string Current
        {
            get 
            { 
                if (index < history.Count) 
                { 
                    return history[index]; 
                } 
                else 
                { 
                    return ""; 
                } 
            }
        }

        public ConsoleHistory()
        {
            history = new List<string>();
        }

        public void Add(string str)
        {
            history.Add(str);
            index = history.Count;
        }

        public string Previous()
        {
            if (index > 0)
            {
                index--;
            }
            return Current;
        }

        public string Next()
        {
            if (index < history.Count - 1)
            {
                index++;
            }
            return Current;
        }

        public void Clear()
        {
            history.Clear();
        }
    }
}
