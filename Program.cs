using System;

namespace LunchHourGames
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (LunchHourGames game = new LunchHourGames())
            {
                game.Run();
            }
        }
    }
}

