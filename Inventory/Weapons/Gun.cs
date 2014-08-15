using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using LunchHourGames.Players;
using LunchHourGames.Combat;
using LunchHourGames.Hexagonal;
using LunchHourGames.Combat.Attacks;

namespace LunchHourGames.Inventory.Weapons
{
    public class Gun : Weapon
    {
        private bool supportsRapidFiring;  // false means one shot at a time, true is rapid firing
        private SoundEffect soundEffect;  // sound of the gun firing     

        private float firingRate = 13.0f;  // How fast the gun can fire the bullet

        public Gun(LunchHourGames lhg, String referenceName, String displayName, int damage, 
                   bool supportsRapidFiring, int range, int areaOfEffect, int actionPointCost)
            : base(lhg, referenceName, displayName, AttackType.PointToPoint, Trajectory.Linear, true, range, damage, areaOfEffect, actionPointCost)
        {                      
            this.supportsRapidFiring = supportsRapidFiring;           
        }

        public override InventoryItem copy()
        {
            Gun gun = new Gun(lhg, referenceName, displayName, damage, supportsRapidFiring, range, areaOfEffect, actionPointCost);
            gun.Initialize();
            gun.MyAmmo = MyAmmo;
            return gun;
        }

        public override void Initialize()
        {
            base.Initialize();

            try
            {
                soundEffect = Game.Content.Load<SoundEffect>("Audio/gunshot");
            }
            catch
            {
            }            
        }

        public override CombatAttack getCombatAttack(CombatSystem combatSystem)
        {
            if (combatAttack == null)
                combatAttack = new LinearAttack(lhg, combatSystem, this);

            return combatAttack;
        }

        public override void fire()
        {
            this.isFiring = true;
            
            // play sound
            try
            {
                soundEffect.Play();
            }
            catch
            {
            }

            // Subtract one bullet from our clip
            MyAmmo.Count--;

            // Start the ammo moving/animating
            MyAmmo.IsMoving = true;
        }
    }
}
