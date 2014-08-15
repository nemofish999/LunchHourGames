using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace LunchHourGames.Sprite
{
    public class AnimatedSprite3D : SimpleSprite3D
    {
        protected SpriteSheet spriteSheet;
        protected AnimationKey direction;

        protected bool isAnimating;

        protected String referenceName;
        protected String cellName;

        protected int framesPerSecond;
        protected TimeSpan frameLength;
        protected TimeSpan frameTimer;
        protected int currentFrame;
        protected int previousFrame = -1;
        protected int frameCount;

        public AnimatedSprite3D(LunchHourGames lhg)
            : base(lhg)
        {
        }

        public AnimatedSprite3D(LunchHourGames lhg, SpriteSheet spriteSheet)
            :base(lhg)
        {
            this.spriteSheet = spriteSheet;
            direction = AnimationKey.South;
            isAnimating = false;
            FramesPerSecond = 5;

            MyTexture = this.spriteSheet.getTexture(direction, 0);
        }

        public bool IsAnimating
        {
            get { return isAnimating; }
            set { isAnimating = value; }
        }

        public int FramesPerSecond
        {
            get { return framesPerSecond; }
            set
            {
                if (value < 1)
                    framesPerSecond = 1;
                else if (value > 60)
                    framesPerSecond = 60;
                else
                    framesPerSecond = value;

                frameLength = TimeSpan.FromSeconds(1 / (double)framesPerSecond);
            }
        }

        public int CurrentFrame
        {
            get { return currentFrame; }
            set
            {
                currentFrame = (int)MathHelper.Clamp(value, 0, spriteSheet.FrameCount - 1);
            }
        }

        public int FrameCount
        {
            get { return this.frameCount; }
        }

        public SpriteSheet MySpriteSheet
        {
            set 
            {
                this.spriteSheet = value;
                this.frameCount = this.spriteSheet.FrameCount;
                this.currentFrame = 0;
                this.FramesPerSecond = this.spriteSheet.FramesPerSecond;
                this.width = this.spriteSheet.FrameWidth;
                this.height = this.spriteSheet.FrameHeight;
            }

            get { return this.spriteSheet; }
        }

        public AnimationKey Direction
        {
            get { return direction; }
            set { direction = value; }
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
            {
                frameTimer += gameTime.ElapsedGameTime;

                if (frameTimer >= frameLength)
                {
                    frameTimer = TimeSpan.Zero;
                    currentFrame = (currentFrame + 1) % frameCount;
                }               
            }

            if (previousFrame != currentFrame)
                updateFrame();
        }

        public void updateFrame()
        {
            MyTexture = this.spriteSheet.getTexture(direction, currentFrame);
            previousFrame = currentFrame;
        }
    }   
}
