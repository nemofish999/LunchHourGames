using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LunchHourGames.Sprite
{
    public class AnimatedSprite : SimpleSprite
    {
        protected List<Animation> animations = new List<Animation>();
        protected AnimationKey currentAnimation;
        protected bool isAnimating;
        protected float speed = 3.0f;
        protected Color backgroundColor = Color.White;

        protected SpriteBatch spriteBatch;

        protected String referenceName;
        protected String cellName;

        public AnimatedSprite(LunchHourGames lhg, Texture2D texture, List<Animation> animations, SpriteBatch spriteBatch)
            : base(lhg, texture)
        {
            this.spriteBatch = spriteBatch;
            this.animations = animations;
            currentAnimation = AnimationKey.North;
            isAnimating = false;
            width = animations[(int)currentAnimation].FrameWidth;
            height = animations[(int)currentAnimation].FrameHeight;
            center = new Vector2(width / 2, height / 2);
        }

        public float Speed
        {
            get { return speed; }
            set
            {
                speed = MathHelper.Clamp(value, 0.1f, 10f);
            }
        }

        public Color BackgroundColor
        {
            get { return this.backgroundColor;  }
            set { this.backgroundColor = value; }
        }

        public bool IsAnimating
        {
            get { return isAnimating; }
            set { isAnimating = value; }
        }

        public AnimationKey CurrentAnimation
        {
            get { return currentAnimation; }
            set { currentAnimation = value; }
        }

        public Rectangle CurrentRectangle
        {
            get 
            {
                if (cellName == null)
                    return animations[(int)currentAnimation].CurrentFrameRect;
                else
                {
                    foreach ( Animation animation in animations)
                    {
                        if (animation.CellName.Equals(cellName))
                            return animation.CurrentFrameRect;
                    }

                    return Rectangle.Empty;
                }
            }
        }

        public String ReferenceName
        {
            get { return this.referenceName; }
            set { this.referenceName = value; }
        }

        public String CellName
        {
            get { return this.cellName; }
            set { this.cellName = value; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (isAnimating)
                animations[(int)currentAnimation].Update(gameTime);
        }

        public SpriteBatch mySpriteBatch
        {
            get
            {
                return spriteBatch;
            }
            set
            {
                spriteBatch = value;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Vector2 origin = Vector2.Zero;// new Vector2(CurrentRectangle.Center.X, CurrentRectangle.Bottom);
            

            spriteBatch.Draw(
                  texture,
                  Position,
                  CurrentRectangle,
                  this.backgroundColor, 0.0f, origin, scale, SpriteEffects.None, 0);
        }
    }
}
