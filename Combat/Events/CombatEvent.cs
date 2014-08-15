using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using LunchHourGames.Inventory;

namespace LunchHourGames.Combat
{
    public class CombatEvent
    {
        public enum Type
        {            
            Attack,
            Starting,
            Ending,
        }

        /*
        
        private Weapon weapon;

        public CombatEvent(CombatSystem combat, Weapon weapon)
            :base(combat.lhg)
        {
            this.weapon = weapon;
        }

        public override void Update(GameTime gameTime)
        {
            this.weapon.Update(gameTime);

           

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.weapon.Draw(gameTime);
        }

        public bool isEventComplete()
        {
            return !this.weapon.IsFiring;
        }
         * */
    }
}
