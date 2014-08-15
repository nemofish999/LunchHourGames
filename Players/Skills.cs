using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LunchHourGames.Common;

namespace LunchHourGames.Players
{
    // Skills represent the actual things your character can do, and excels at.  
    // They represent a dynamic change to the character that user’s customize as they progress.
    // You will only adjust the skills for your primary character.  All non-primary characters
    // will automatically adjust their own skills automatically.  Skill points are awarded at each 
    // level depending on your character’s Intelligence statistic.  The skill points you are given 
    // each level are 10 + Intelligence.  Each skill has a minimum of 6, and a maximum of 100.

    public class Skills
    {
        public int meleeWeapons;  // Represents how good your character is with weapons like baseball bats,
        // crow bars, shovels, and sledge hammers.  They can be a great way to
        // conserve ammo.

        public int guns;  // Represents how good your character is with conventional guns like pistols, 
        // rifles, and shotguns.  It’s important to have at least character excel at guns.

        public int explosives;  // Represents how effective your character is at explosives like grenades,
        // Molotov cocktails, and rockets.

        public int vehicularRepair; // Represents how good your character is at fixing ailing cars.

        public int weaponMods;  // Represents how good your character is at modifying your weapons 
        // to be more deadly.  Modifications can be performed outside of combat
        // as long as you have the necessary materials, and the minimum Weapon
        // Mods skill level.

        public int firstAid;    // Represents how good your character is at applying first aid.  
        // Any character can use first aid items, but if a character with a high
        // first aid skill applies a first aid item to another character,
        // it will heal more HP.

        public int scavenge;    // Represents how good your character is at scavenging items.

        public int speech;      // Represents how good your character is at communicating with other
                                // characters.  Some characters can only be convinced to join you if 
                                // your speech skill level is high enough.

        public void calculate(PrimaryStatistics stats)
        {
            this.meleeWeapons = LHGCommon.getValueInRange( 6 + (stats.strength * 2), 6, 100 );
            this.guns = LHGCommon.getValueInRange( 6 + (stats.agility * 2), 6, 100 );
            this.explosives = LHGCommon.getValueInRange( 6 + stats.agility + stats.vision, 6, 100 );
            this.vehicularRepair = LHGCommon.getValueInRange( 6 + (stats.utilization * 2), 6, 100 );
            this.weaponMods = LHGCommon.getValueInRange( 6 + stats.utilization + stats.resourcefulness, 6, 100 );
            this.firstAid = LHGCommon.getValueInRange( 6 + (stats.intelligence * 2), 6, 100 );
            this.scavenge = LHGCommon.getValueInRange( 6 + (stats.resourcefulness * 2), 6, 100 );
            this.speech = LHGCommon.getValueInRange(6 + stats.intelligence + stats.vision, 6, 100);
        }
    }
}