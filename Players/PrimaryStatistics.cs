using System;

namespace LunchHourGames.Players
{
    // Directly inspired by the S.P.E.C.I.A.L. system in Fallout, the S.U.R.V.I.V.A.L. system 
    // specifies 8 statistics that directly affect players’ passive and active skills and abilities. 
    // Each primary statistic is measured by points from 1 to 10, with a maximum starting total of 46 points. 
    public class PrimaryStatistics
    {
        public int strength; // Represents your character’s physical strength.  This will determine how 
                             // effective he will be in close hand to hand combat, and how much he can
                             // carry back during a scavenge run.

        public int utilization;  // Represents your character’s ability to use/repair/modify your things.
                                 // This will determine how effective he will be in repairing your broken
                                 // down car or modify your gun to be more powerful.

        public int resourcefulness;  // Represents your character’s ability to find or combine things 
                                     // while scavenging.
                                     // This will determine how many things he can find while scavenging.

        public int vitality;  // Represents your character’s life force.  This will determine how many hit
                              // points he can have, and how hardy he is to resist a zombie infection if bitten.

        public int intelligence;  // Represents your character’s mental faculties.  This will determine how 
                                  // effective he is at applying first aid, and how good he is at picking up
                                  // intelligent dialogue.

        public int vision;  // Represents your character’s physical and abstract perception.  This will determine
                            // how accurate his shots are, and his ability to sense impeding danger.

        public int agility;  // Represents your character’s reflexive ability.   This will determine if an 
                             // attack can be evaded, and how many action points he can use per turn.

        public int luck;     // Represents your character’s chance at a favorable outcome.  This will determine
                             // a critical hit, and will give him a cushion to all dice rolls.

        public PrimaryStatistics()
        {
            strength = 5;
            utilization = 5;
            resourcefulness = 5;
            vitality = 5;
            intelligence = 5;
            vision = 5;
            agility = 5;
            luck = 5;
        }

        public static PrimaryStatistics PlayerDefaults
        {
            get
            {
                return new PrimaryStatistics(5, 5, 5, 5, 5, 5, 5, 5);
            }
        }

        public PrimaryStatistics( int strength, int utilization, int resourcefulness, int vitality,
                                  int intelligence, int vision, int agility, int luck)
        {
            this.strength = strength;
            this.utilization = utilization;
            this.resourcefulness = resourcefulness;
            this.vitality = vitality;
            this.intelligence = intelligence;
            this.vision = vision;
            this.agility = agility;
            this.luck = luck;
        }
    }
}
