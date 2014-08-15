using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames.Players;
using LunchHourGames.Combat;
using LunchHourGames.Combat.Attacks;

namespace LunchHourGames.Inventory.Weapons
{
    public abstract class Weapon : InventoryItem
    {
        public enum AttackType
        {
            PointToPoint,
            AreaOfEffect,
            Spread
        }

        public enum Trajectory
        {
            Linear,
            Parabolic,
            Circular
        }

        protected CombatAttack combatAttack;

        protected AttackType attackType;
        protected Trajectory trajectory;
        protected int range;
        protected int damage;
        protected int areaOfEffect;
        protected int actionPointCost;

        protected bool usesAmmo;
        protected Ammo ammo;

        protected bool isFiring;       

        public Weapon(LunchHourGames lhg, String referenceName, String displayName, 
                      AttackType attackType, Trajectory trajectory, bool usesAmmo,
                      int range, int damage, int areaOfEffect, int actionPointCost)
            : base(lhg, InventoryType.Weapon, referenceName, displayName)
        {
            this.name = name;
            this.attackType = attackType;
            this.trajectory = trajectory;
            this.usesAmmo = usesAmmo;
            this.range = range;
            this.damage = damage;
            this.areaOfEffect = areaOfEffect;
            this.actionPointCost = actionPointCost;
            this.isFiring = false;
        }      

        public int ActionPointCost
        {
            get { return this.actionPointCost; }
        }

        public virtual bool fire(CombatSystem combat, Player sourcePlayer, Point target)
        {
            return false;  // weapon did not fire
        }

        public bool IsFiring
        {
            get { return this.isFiring; }           
        }

        public String Name
        {
            get { return this.name; }
        }

        public int HitPoints
        {
            get { return this.damage; }
        }

        public AttackType MyAttackType
        {
            get { return this.attackType; }
        }

        public Trajectory MyTrajectory
        {
            get { return this.trajectory; }
        }

        public virtual CombatAttack getCombatAttack(CombatSystem combatSystem)
        {
            return null;
        }

        public Ammo MyAmmo
        {
            get { return this.ammo; }
            set { this.ammo = value; }
        }

        public int AmmoLeft
        {
            get { return this.ammo.Count; }
        }

        public virtual void fire()
        {
        }

        public void reload(int count)
        {
            
        }
    }
}
