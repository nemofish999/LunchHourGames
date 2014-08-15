using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using LunchHourGames.Players;
using LunchHourGames.Inventory;
using LunchHourGames.Inventory.Weapons;
using LunchHourGames.Hexagonal;
using LunchHourGames.Sprite;

namespace LunchHourGames.Combat.Attacks
{
    public abstract class CombatAttack : Microsoft.Xna.Framework.DrawableGameComponent, AnimationCallback
    {
        protected CombatSystem combatSystem;
        protected Weapon weapon;
        protected bool hasCompleted = false;
        protected bool startedAnimationSequence = false;
        protected GameEntity targetGameEntity;
        protected Hex targetHex;

        protected GameEntity attackerGameEntity;

        protected Matrix world, view, projection;

        public CombatAttack(LunchHourGames lhg, CombatSystem combatSystem, Weapon weapon)
            :base(lhg)
        {
            this.combatSystem = combatSystem;
            this.weapon = weapon;
        }

        public GameEntity TargetGameEntity
        {
            get { return this.targetGameEntity;  }
            set { this.targetGameEntity = value; }
        }

        public Hex TargetHex
        {
            get { return this.targetHex; }
            set { this.targetHex = value; }
        }

        public GameEntity AttackerGameEntity
        {
            get { return this.attackerGameEntity;  }
            set { this.attackerGameEntity = value; }
        }

        public Weapon MyWeapon
        {
            set { this.weapon = value; }
            get { return this.weapon;  }
        }

        public bool HasCompleted
        {
            get { return this.hasCompleted; }
        }

        public bool StartedAnimationSequence
        {
            get { return this.startedAnimationSequence;  }
            set { this.startedAnimationSequence = value; }
        }

        public CombatBoard MyBoard
        {
            get { return combatSystem.MyBoard; }
        }

        protected Player CurrentPlayer
        {
            get { return this.combatSystem.MyTurnBased.CurrentPlayer; }
        }

        public virtual Matrix MyView
        {
            set
            { 
                this.view = value;
                if ( this.weapon != null )
                    this.weapon.MyAmmo.MyView = value;
            }

            get { return view; }
        }

        public virtual Matrix MyProjection
        {
            set
            { 
                this.projection = value;
                if (this.weapon != null )
                    this.weapon.MyAmmo.MyProjection = value;            
            }

            get { return this.projection; }
        }

        public virtual Matrix MyWorld
        {
            set 
            { 
                this.world = value;
                this.weapon.MyAmmo.MyWorld = value;
            }

            get { return this.world; }
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime)
        {
        }

        public virtual void animationBegin(GameEntity gameEntity, AnimationType type)
        {
        }

        public virtual void animationEnd(GameEntity gameEntity, AnimationType type)
        {
        }
    }
}
