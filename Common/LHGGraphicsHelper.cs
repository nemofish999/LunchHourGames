using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LunchHourGames.Common
{
    public class LHGGraphicsHelper
    {
        public static Texture2D extractSubTexture(GraphicsDevice device, Color[] textureData, int textureWidth, int textureHeight, Rectangle frameExtents)
        {
            Texture2D cellTexture = new Texture2D(device, frameExtents.Width, frameExtents.Height);

            try
            {
                int dataPerTexture = textureWidth * textureHeight;                
                int dataPerCell = frameExtents.Width * frameExtents.Height;
                Color[] cellData = new Color[dataPerCell];

                // Fill the cell data with colors from the texture data
                for (int y = frameExtents.Top; y < frameExtents.Bottom; y++)
                {
                    for (int x = frameExtents.Left; x < frameExtents.Right; x++)
                    {
                        int cellIndex = x - frameExtents.Left + ((y - frameExtents.Top) * frameExtents.Width);
                        if ((cellIndex < dataPerCell) && (cellIndex >= 0))
                        {
                            int textureIndex = x + (y * textureWidth);

                            if (textureIndex >= dataPerTexture)
                                cellData[cellIndex] = Color.Transparent;
                            else
                            {
                                Color color = textureData[textureIndex];
                                cellData[cellIndex] = color;
                            }
                        }
                    }
                }

                // Fill the cell with the extracted data
                cellTexture.SetData<Color>(cellData);
            }
            catch (Exception ex)
            {                
            }

            return cellTexture;
        }

        public static List<Color> getGradientColors(Color beginColor, Color endColor, int numSteps)
        {
            List<Color> gradientColors = new List<Color>();
            
            for ( int i = 0; i <= numSteps; i++)
            {
                int r = interpolate(beginColor.R, endColor.R, i, numSteps);
                int g = interpolate(beginColor.G, endColor.G, i, numSteps);
                int b = interpolate(beginColor.B, endColor.B, i, numSteps);

                gradientColors.Add( new Color(r, g, b) );
            }

            return gradientColors;
        }
    
        private static int interpolate(int begin, int end, int step, int max)
        {
            return begin + (int)((end - begin) * step / max);
        }

        public static Texture2D getGradientTexture(GraphicsDevice device, int width, int height, Color beginColor, Color endColor)
        {
            Texture2D texture = new Texture2D(device, width, height);
            List<Color> gradientColors = getGradientColors(beginColor, endColor, height);
            int numColors = gradientColors.Count();

            Color[] bgc = new Color[height * width];
            for (int i = 0; i < bgc.Length; i++)
            {
                int textureColor = (int)(i / width);
                bgc[i] = gradientColors[textureColor];
            }
            
            texture.SetData(bgc);

            return texture;
        }
    }
}
