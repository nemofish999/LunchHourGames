using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LunchHourGames.Debugger;
using LunchHourGames.Combat;

namespace LunchHourGames.Players
{
    public class PlayerMemento : Memento
    {
        // Location of the player on the combat board
        public CombatLocation combatLocation;

        // S.U.R.V.I.V.A.L properties
        public PrimaryStatistics stats;
        public DerivedAttributes attributes;
        public Skills skills;
        //public Reactions reactions;

        public int roll;
        

        public PlayerMemento()
            : base(Type.Player)
        {
        }

    }
}
