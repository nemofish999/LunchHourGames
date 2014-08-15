using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LunchHourGames.Inventory;
using LunchHourGames.Inventory.Weapons;

namespace LunchHourGames.Players
{
    public class Human : Player
    {
        // S.U.R.V.I.V.A.L properties
        private PrimaryStatistics stats;
        private Skills skills;

        public Human(LunchHourGames lhg, String templateName, String name, PlayerSprite sprite)
            : base(lhg, templateName, name, Type.Human, false, sprite)
        {
            this.stats = new PrimaryStatistics();
            this.skills = new Skills();
        }

        public PrimaryStatistics MyStats
        {
            set { this.stats = value; }
            get { return this.stats; }
        }

        public Skills MySkills
        {
            set { this.skills = value; }
            get { return this.skills; }
        }

        public void changeStat(String stat, int value, int level, int roll)
        {
            switch (stat)
            {
                case "s":
                    stats.strength = value;
                    break;

                case "u":
                    stats.utilization = value;
                    break;

                case "r":
                    stats.resourcefulness = value;
                    break;

                case "vit":
                    stats.vitality = value;
                    break;

                case "i":
                    stats.intelligence = value;
                    break;

                case "vis":
                    stats.vision = value;
                    break;

                case "a":
                    stats.agility = value;
                    break;

                case "l":
                    stats.luck = value;
                    break;
            }

            recalcAllStats(level);
        }

        public override void recalcAllStats(int level)
        {
            MyAttributes.calculate(stats, level, roll);
            MySkills.calculate(stats);
        }

        public override bool canMainPlayerWalkTo()
        {
            return false;
        }

        public override bool canMainPlayerAttack(Weapon weapon)
        {
            return false;
        }
    }
}