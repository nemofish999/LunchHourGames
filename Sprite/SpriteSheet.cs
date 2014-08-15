using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames.Common;

namespace LunchHourGames.Sprite
{
    // This is a shared resource.  Do NOT put specific information about the player or obstacle in this class.
    public class SpriteSheet
    {
        protected LunchHourGames lhg;
        protected string textureName;
        protected Texture2D texture;
        protected Texture2D[,] cells;

        protected int frameCount;
        protected int frameWidth;
        protected int frameHeight;
        protected int framesPerSecond;
        
        public SpriteSheet(LunchHourGames lhg, string textureName, Texture2D texture, List<Animation> animations)
        {
            this.lhg = lhg;
            this.textureName = textureName;
            this.texture = texture;
            this.texture.Name = textureName;

            Animation firstAnimation = animations.First();
            frameCount = firstAnimation.FrameCount;
            frameWidth = firstAnimation.FrameWidth;
            frameHeight = firstAnimation.FrameHeight;
            framesPerSecond = frameCount;
            cells = new Texture2D[8,frameCount];

            //Get the pixel data from the original texture:
            Color[] textureData = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(textureData);

            foreach (Animation animation in animations)
            {
                extractTextures(textureData, texture.Width, texture.Height, animation);
            }
        }

        public string MyName
        {
            get { return this.textureName; }
        }

        public int FrameCount
        {
            get { return this.frameCount; }
        }

        public int FramesPerSecond
        {
            get { return this.framesPerSecond; }
            set { this.framesPerSecond = value; }
        }

        public int FrameWidth
        {
            get { return this.frameWidth; }
        }

        public int FrameHeight
        {
            get { return this.frameHeight; }
        }

        public Texture2D getTexture(AnimationKey direction, int frameIndex)
        {
            return cells[(int)direction, frameIndex];
        }

        private void extractTextures(Color[] textureData, int textureWidth, int textureHeight, Animation animation)
        {
            int frameIndex = 0;
            string error;

            try
            {
                int dataPerTexture = textureWidth * textureHeight;

                Rectangle[] frames = animation.Frames;
                int cellNum = 0;
                foreach (Rectangle frame in frames)
                {
                    Texture2D cellTexture = LHGGraphicsHelper.extractSubTexture(lhg.GraphicsDevice, textureData, textureWidth, textureHeight, frame);

                    cellTexture.Name = textureName + "[" + animation.DirectionName + ", " + Convert.ToString(cellNum) + "]";

                    // Add the texture to the array
                    this.cells[(int)animation.Direction, frameIndex] = cellTexture;
                    frameIndex++;
                    cellNum++;
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
        }
    }
}
