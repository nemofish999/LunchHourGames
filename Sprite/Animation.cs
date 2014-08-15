using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LunchHourGames.Sprite
{
    public class Animation : ICloneable
    {
        private String cellName;
        private AnimationKey direction;
        private Rectangle[] frames;
        private int framesPerSecond;
        private TimeSpan frameLength;
        private TimeSpan frameTimer;
        private int currentFrame;
        private int frameWidth;
        private int frameHeight;
        private int frameCount;

        public Animation(AnimationKey direction, int frameCount, int frameWidth, int frameHeight, int xOffset, int yOffset)
        {
            this.direction = direction;
            this.frameCount = frameCount;
            frames = new Rectangle[frameCount];
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;

            for (int i = 0; i < frameCount; i++)
            {
                frames[i] = new Rectangle(
                        xOffset + (frameWidth * i),
                        yOffset,
                        frameWidth,
                        frameHeight);
            }
            FramesPerSecond = 5;
            Reset();
        }

        private Animation()
        {
            FramesPerSecond = 5;
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

        public Rectangle CurrentFrameRect
        {
            get { return frames[currentFrame]; }
        }

        public int CurrentFrame
        {
            get { return currentFrame; }
            set 
            { 
                currentFrame = (int)MathHelper.Clamp(value, 0, frames.Length - 1); 
            }
        }

        public int FrameWidth
        {
            get { return frameWidth; }
        }

        public int FrameHeight
        {
            get { return frameHeight; }
        }

        public String CellName
        {
            get { return this.cellName; }
            set { this.cellName = value; }
        }

        public void Update(GameTime gameTime)
        {
            frameTimer += gameTime.ElapsedGameTime;

            if (frameTimer >= frameLength)
            {
                frameTimer = TimeSpan.Zero;
                currentFrame = (currentFrame + 1) % frames.Length;
            }
        }

        public void Reset()
        {
            currentFrame = 0;
            frameTimer = TimeSpan.Zero;
        }

        public object Clone()
        {
            Animation animation = new Animation();
            animation.frames = this.frames;
            animation.frameWidth = this.frameWidth;
            animation.frameHeight = this.frameHeight;
            animation.Reset();
            return animation;
        }

        public AnimationKey Direction
        {
            get { return this.direction; }
        }

        public string DirectionName
        {
            get 
            {
                switch (this.direction)
                {
                    case AnimationKey.North:
                        return "North";
                    case AnimationKey.NorthEast:
                        return "NorthEast";
                    case AnimationKey.East:
                        return "East";
                    case AnimationKey.SouthEast:
                        return "SouthEast";
                    case AnimationKey.South:
                        return "South";
                    case AnimationKey.SouthWest:
                        return "SouthWest";
                    case AnimationKey.West:
                        return "West";
                    case AnimationKey.NorthWest:
                        return "NorthWest";
                }

                return "";
            }
        }

        public int FrameCount
        {
            get { return this.frameCount; }
        }

        public Rectangle[] Frames
        {
            get { return this.frames; }
        }
    }
}
