using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using LunchHourGames.Inventory.Weapons;
using LunchHourGames.Sprite;
using LunchHourGames.Combat.Attacks;

namespace LunchHourGames
{
    public abstract class GameEntity : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public enum EntityType
        {
            Player,    // Human, Zombie, or Animal
            Obstacle,  // Tree, wall, barrel
            Inventory, // Gun, Bullets, Food, Water, MedPack, Gas, Oil, Hammer, Flashlight, Matches, etc...
            Vehicle,   // Car, Van, Truck, Tank            
        }

        protected EntityType entityType;

        protected LunchHourGames lhg;

        protected String referenceName;
        protected String displayName;

        protected Matrix world, view, projection;

        public GameEntity(LunchHourGames lhg, EntityType entityType, String referenceName, String displayName)
            : base(lhg)
        {
            this.entityType = entityType;
            this.lhg = lhg;
            this.referenceName = referenceName;
            this.displayName = displayName;
        }

        public String MyReferenceName
        {
            get { return this.referenceName; }
        }

        public String MyDisplayName
        {
            get { return this.displayName; }
        }

        public EntityType MyEntityType
        {
            get { return this.entityType; }
        }

        public string printType()
        {
            switch (this.entityType)
            {
                case EntityType.Player:
                    return "Player";
                case EntityType.Obstacle:
                    return "Obstacle";
                case EntityType.Inventory:
                    return "Inventory";
                case EntityType.Vehicle:
                    return "Vehicle";
            }

            return "Unkown";
        }

        public virtual void setGraphicsMatrices(Matrix view, Matrix projection, Matrix world)
        {
            this.view = view;
            this.projection = projection;
            this.world = world;
        }

        public virtual Matrix MyView
        {
            set { this.view = value; }
            get { return view;       }
        }

        public virtual Matrix MyProjection
        {
            set { this.projection = value; }
            get { return this.projection;  }
        }

        public virtual Matrix MyWorld
        {
            set { this.world = value; }
            get { return this.world;  }
        }

        public virtual Rectangle getExtents()
        {
            return Rectangle.Empty;
        }
       
        public virtual bool canMainPlayerWalkTo()
        {
            return false;
        }

        public virtual bool canMainPlayerAttack(Weapon weapon)
        {
            return false;
        }

        public virtual void walk(List<Vector3> path, AnimationCallback callback)
        {
        }

        public virtual void attack(AttackType attackType, AnimationCallback callback)
        {
        }

        public virtual void hit(Ammo ammo, AnimationCallback callback)
        {
        }

        public virtual void die(AnimationCallback callback)
        {
        }
    }
}
