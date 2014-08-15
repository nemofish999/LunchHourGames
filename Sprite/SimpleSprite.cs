using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using LunchHourGames;

namespace LunchHourGames.Sprite
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class SimpleSprite : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected LunchHourGames lhg;
        protected Texture2D texture;

        protected Rectangle sourceRectangle;

        protected Vector2 position;
        protected Vector2 velocity;
        protected Vector2 center;

        protected float scale;
        protected float rotation;

        protected int width;
        protected int height;

        public SimpleSprite(LunchHourGames lhg, Texture2D texture)  
            : base(lhg)
        {
            this.lhg = lhg;
            this.texture = texture;
            width = texture.Width;
            height = texture.Height;
            position = Vector2.Zero;
            velocity = Vector2.Zero;
            center = new Vector2(texture.Width / 2, texture.Height / 2);

            scale = 1.0f;
            rotation = 0.0f;

            sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public SimpleSprite(LunchHourGames lhg, Texture2D texture, Rectangle sourceRectangle)
            : base(lhg)
        {
            this.lhg = lhg;
            this.texture = texture;
            this.sourceRectangle = sourceRectangle;

            position = Vector2.Zero;
            velocity = Vector2.Zero;
            center = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);
            width = sourceRectangle.Width;
            height = sourceRectangle.Height;
            scale = 1.0f;
            rotation = 0.0f;
        }

        public SimpleSprite(LunchHourGames lhg, Texture2D texture, Vector2 position)
            : base(lhg)
        {
            this.lhg = lhg;
            this.texture = texture;
            width = texture.Width;
            height = texture.Height;
            this.position = position;
            velocity = Vector2.Zero;
            center = new Vector2(texture.Width / 2, texture.Height / 2);

            scale = 1.0f;
            rotation = 0.0f;

            sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle( (int)position.X, (int)position.Y, width, height);
            }
        }

        public virtual Texture2D Texture
        {
            get { return texture; }
        }

        public virtual Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public virtual Vector2 Velocity
        {
            get { return velocity; }
        }

        public virtual Vector2 Center
        {
            get { return center; }
        }

        public Vector2 Origin
        {
            get { return position + center; }
        }

        public virtual float Scale
        {
            set { this.scale = value; }
            get { return scale; }
        }

        public virtual float Rotation
        {
            get { return rotation; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public virtual void Show()
        {
            Enabled = true;
            Visible = true;
        }

        public virtual void Hide()
        {
            Enabled = true;
            Visible = true;
        }
    }
}