using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames.Sprite;

namespace LunchHourGames.Inventory.Weapons
{
    public class Ammo : InventoryItem
    {
        public enum AmmoType
        {
            Bullet, Shell, Arrow, Gas, Rocket
        }

        protected AmmoType ammoType;
        protected Weapon weapon;
        protected int count;
        protected bool isMoving;
        protected bool hitTarget;
        protected List<Vector3> movePath;

        protected Texture2D texture;
        protected byte opacity = 255;
        protected Vector3 position;
        protected SimpleSprite3D sprite;

        public Ammo(LunchHourGames lhg, String referenceName, String displayName, Weapon weapon)
            :base(lhg, InventoryType.Ammo, referenceName, displayName)
        {
            this.weapon = weapon;
            this.isMoving = false;
            this.hitTarget = false;
        }

        public List<Vector3> MyPath
        {
            get { return this.movePath; }
            set 
            { 
                movePath = value;
                hitTarget = false;
            }
        }

        public bool IsMoving
        {
            get { return isMoving; }
            set { isMoving = value; }
        }

        public int Count
        {
            get { return this.count; }
            set { this.count = value; }
        }

        public int HitPoints
        {
            get { return this.weapon.HitPoints; }
        }

        public override void setGraphicsMatrices(Matrix view, Matrix projection, Matrix world)
        {
            MyView = view;
            MyProjection = projection;
            MyWorld = world;
        }

        public override Matrix MyView
        {
            set
            { 
                this.view = value;
                this.sprite.MyView = value;
            }

            get { return view; }
        }

        public override Matrix MyProjection
        {
            set
            { 
                this.projection = value;
                this.sprite.MyProjection = value;
            }

            get { return this.projection; }
        }

        public override Matrix MyWorld
        {
            set 
            { 
                this.world = value;
                this.sprite.MyWorld = value;
            }

            get { return this.world; }
        }

        public virtual bool canMainPlayerWalkTo()
        {
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.movePath == null)
            {
                this.isMoving = false;
            }
            else
            {
                int pathCount = this.movePath.Count();
                if (pathCount == 0)
                {
                    this.hitTarget = true;
                    this.movePath = null;
                }
                else
                {
                    position = this.movePath[0];
                    this.movePath.RemoveAt(0);
                }
            }

            Vector3 currentPosition = position;
            currentPosition.Y += texture.Height + 10;

            this.sprite.MyPosition = currentPosition;
            this.sprite.Update(gameTime);
        }

        public bool isTargetHit()
        {
            return this.hitTarget;
        }

        public override void Draw(GameTime gameTime)
        {
            this.sprite.Draw(gameTime);
        }
    }
}
