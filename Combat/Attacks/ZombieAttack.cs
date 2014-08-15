using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using LunchHourGames.Players;
using LunchHourGames.Sprite;

namespace LunchHourGames.Combat.Attacks
{
    public class ZombieAttack : CombatAttack
    {
        private Zombie zombie;

        public ZombieAttack(LunchHourGames lhg, CombatSystem combatSystem)
            :base(lhg, combatSystem, null)
        {
        }

        public override void Initialize()
        {
            zombie = (Zombie) AttackerGameEntity;
            zombie.attack(AttackType.Grab, this);
        }

        public override void Update(GameTime gameTime)
        {


        }

        public override void Draw(GameTime gameTime)
        {
        }

        public override void animationBegin(GameEntity gameEntity, AnimationType type)
        {
        }

        public override void animationEnd(GameEntity gameEntity, AnimationType type)
        {
        }
    }
}
