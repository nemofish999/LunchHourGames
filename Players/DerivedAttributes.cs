using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LunchHourGames.Common;

namespace LunchHourGames.Players
{
    // Your derived attributes represent various attributes of your character 
    // that scale with your level.  The higher the corresponding primary statistic,
    // the higher your character’s base is at level 1.
    public class DerivedAttributes
    {
        public int actionPoints;  // (AP) These represent how many given actions your character
                                  // can perform on a given turn.
    
        public int criticalChance; // Represents the chance your character has to score a critical hit.
                                   // If a critical hit is triggered, this needs to be shown to the user. 
        
        public int hitPoints;  // (HP) This represents the amount of health your character has.  
                               // If a character’s health reaches zero, they are knocked out.
                               // If you do not give first aid to a non-primary character after three turns,
                               // they die.  If the primary character gets knocked out, it is game over.

        public int resistance; // Represents your character’s ability to resist attacks, and even infection. 
 
        public int accuracy;  // Represents your character’s ability to shoot weapons or throw projectiles
                              // accurately.  Each attack has a chance to miss, but the higher your accuracy,
                              // the less of a chance you have. 

        public int initiative;  // Represents your character’s ability to quickly attack when presented 
                                // with a hostile situation.  At the beginning of each turn, each character
                                // rolls for initiative just like in D n’ D.

        public void calculate(PrimaryStatistics stats, int level, int roll)
        {
            this.actionPoints = LHGCommon.getValueInRange(Math.Floor(stats.agility / 3.0) + Math.Floor(level / 3.0), 4, 27);           
            this.criticalChance = LHGCommon.getValueInRange(stats.luck, 1, 10);
            this.hitPoints = LHGCommon.getValueInRange(20 + stats.vitality + (level * 3), 21, 120);
            this.resistance = LHGCommon.getValueInRange(5 + stats.vitality + (level / 2), 6, 30);
            this.accuracy = LHGCommon.getValueInRange(95 + stats.vision / 2, 95, 100);
            this.initiative = LHGCommon.getValueInRange(roll + stats.agility, 13, 22);
        }

        public void calculate(int level, int roll)
        {
            this.actionPoints = 2;
            this.initiative = LHGCommon.getValueInRange(roll, 13, 22);
        }
    }
}
