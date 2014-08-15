using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames.Common;

namespace LunchHourGames.Sprite
{
    public class StaticSpriteSheet
    {
        protected LunchHourGames lhg;
        protected string textureName;
        protected Texture2D texture;
        protected List<Texture2D> textures = new List<Texture2D>();

        public StaticSpriteSheet(LunchHourGames lhg, string textureName, Texture2D texture, List<SimpleFrame> frames)
        {
            this.lhg = lhg;
            this.textureName = textureName;
            this.texture = texture;
            this.texture.Name = textureName;

            //Get the pixel data from the original texture:
            Color[] textureData = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(textureData);

            foreach (SimpleFrame frame in frames)
            {
                extractSubTextures(textureData, texture.Width, texture.Height, frame);
            }
        }

        public string MyName
        {
            get { return textureName; }
        }

        public Texture2D getTexture(string frameReferenceName)
        {
            foreach (Texture2D texture in textures)
            {
                if (texture.Name.CompareTo(frameReferenceName) == 0)
                    return texture;
            }
                
            return null;
        }

        private void extractSubTextures(Color[] textureData, int textureWidth, int textureHeight, SimpleFrame frame)
        {
            Texture2D cellTexture = LHGGraphicsHelper.extractSubTexture(lhg.GraphicsDevice, textureData, textureWidth, textureHeight, frame.MyExtents);
            cellTexture.Name = frame.MyReferenceName;
            textures.Add(cellTexture);
        }
    }
}
