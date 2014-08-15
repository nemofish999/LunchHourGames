using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LunchHourGames.Inventory.Weapons;

namespace LunchHourGames.Combat.Attacks
{
    public class ParabolicAttack : CombatAttack
    {
        public ParabolicAttack(LunchHourGames lhg, CombatSystem combatSystem, Weapon weapon)
            : base(lhg, combatSystem, weapon)
        {
        }
    }
}
