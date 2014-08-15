using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LunchHourGames.Players;

namespace LunchHourGames.GamePlay
{
    public class Party
    {
        private LunchHourGames lhg;

        private List<Human> humans = new List<Human>();
        private Vehicle vehicle;

        public Party(LunchHourGames lhg)
        {
            this.lhg = lhg;
        }

        public void addHuman(Human human)
        {
            humans.Add(human);
        }

        public Human getMainPlayer()
        {
            foreach (Human human in humans)
            {
                if (human.IsMainPlayer)
                    return human;
            }

            return null;
        }

        public Human getHumanByReferenceName(string referenceName)
        {
            foreach (Human human in humans)
            {
                if (human.MyReferenceName.CompareTo(referenceName) == 0)
                    return human;
            }

            return null;
        }

    }
}
