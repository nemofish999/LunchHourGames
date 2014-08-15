using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames.Drawing;
using LunchHourGames.Common;

namespace LunchHourGames.Hexagonal
{
    public class HexSelection : LHGGrid
    {
        public enum Type { Pulsate, Gradient, Contrail }
 
        private LunchHourGames lhg;

        private Hex hex;
        private Type type;
        private bool moveOut;
        private int maxThickness;
        private int currentStart;
        private bool pulsate;
        private int rate;
        private bool goingOut;
        private int nextColor;
        private bool shouldFlash;

        private List<Color> gradientColors;

        public HexSelection(LunchHourGames lhg, Hex hex, Type type, bool moveOut, int maxThickness, Color color)
            : base(lhg)
        {
            this.lhg = lhg;
            this.hex = hex;
            this.moveOut = moveOut;
            this.maxThickness = maxThickness;
            this.MyColor = color;
            this.currentStart = 0;
            this.rate = 5;
            this.goingOut = true;
            this.nextColor = 0;
            this.shouldFlash = true;
            
            LoadGraphicsContent(lhg.GraphicsDevice);

            MakeThickHex(maxThickness, 0);

            gradientColors = LHGGraphicsHelper.getGradientColors(Color.LightYellow, Color.Yellow, 50);
        }

        public bool Pulsate
        {
            set { this.pulsate = false; }
        }

        public bool Flash
        {
            set { this.shouldFlash = value; }
        }

        public void MakeThickHex(int thickness, int startOffset)
        {
            Begin();

            Color currentColor = MyColor;

            float Y = 0.0f;
            float offset = startOffset;
            while (offset < thickness + startOffset)
            {
                if (hex.Orientation == HexOrientation.Flat)
                {
                    AddVertex(new Vector3(hex.Points[0].X, Y, hex.Points[0].Y - offset), currentColor);
                    AddVertex(new Vector3(hex.Points[1].X, Y, hex.Points[1].Y - offset), currentColor);
                    AddVertex(new Vector3(hex.Points[1].X, Y, hex.Points[1].Y - offset), currentColor);
                    AddVertex(new Vector3(hex.Points[2].X + offset, Y, hex.Points[2].Y), currentColor);
                    AddVertex(new Vector3(hex.Points[2].X + offset, Y, hex.Points[2].Y), currentColor);
                    AddVertex(new Vector3(hex.Points[3].X, Y, hex.Points[3].Y + offset), currentColor);
                    AddVertex(new Vector3(hex.Points[3].X, Y, hex.Points[3].Y + offset), currentColor);
                    AddVertex(new Vector3(hex.Points[4].X + offset, Y, hex.Points[4].Y + offset), currentColor);
                    AddVertex(new Vector3(hex.Points[4].X + offset, Y, hex.Points[4].Y + offset), currentColor);
                    AddVertex(new Vector3(hex.Points[5].X - offset, Y, hex.Points[5].Y), currentColor);
                    AddVertex(new Vector3(hex.Points[5].X - offset, Y, hex.Points[5].Y), currentColor);
                }
                else
                {
                    AddVertex(new Vector3(hex.Points[0].X, Y, hex.Points[0].Y - offset), currentColor);
                    AddVertex(new Vector3(hex.Points[1].X + offset, Y, hex.Points[1].Y - offset / 2), currentColor);
                    AddVertex(new Vector3(hex.Points[1].X + offset, Y, hex.Points[1].Y - offset / 2), currentColor);
                    AddVertex(new Vector3(hex.Points[2].X + offset, Y, hex.Points[2].Y + offset / 2), currentColor);
                    AddVertex(new Vector3(hex.Points[2].X + offset, Y, hex.Points[2].Y + offset / 2), currentColor);
                    AddVertex(new Vector3(hex.Points[3].X, Y, hex.Points[3].Y + offset), currentColor);
                    AddVertex(new Vector3(hex.Points[3].X, Y, hex.Points[3].Y + offset), currentColor);
                    AddVertex(new Vector3(hex.Points[4].X - offset, Y, hex.Points[4].Y + offset / 2), currentColor);
                    AddVertex(new Vector3(hex.Points[4].X - offset, Y, hex.Points[4].Y + offset / 2), currentColor);
                    AddVertex(new Vector3(hex.Points[5].X - offset, Y, hex.Points[5].Y - offset / 2), currentColor);
                    AddVertex(new Vector3(hex.Points[5].X - offset, Y, hex.Points[5].Y - offset / 2), currentColor);
                }

                offset += 0.25f;
            }

            End();
        }

        public void Update(GameTime gameTime)
        {
            if (pulsate)
            {
                if ( (gameTime.ElapsedGameTime.Milliseconds % 8) == 0 )
                {
                    Color[] rainbow = new Color[7] { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet };
                    MakeThickHex(maxThickness, currentStart);

                    if (goingOut)
                        currentStart = currentStart + rate;
                    else
                        currentStart = currentStart - rate;

                    if (shouldFlash)
                    {
                        MyColor = gradientColors[nextColor];
                        nextColor++;
                        if (nextColor >= gradientColors.Count)
                            nextColor = 0;
                    }

                    if (currentStart > 30)
                        goingOut = false;

                    if (currentStart <= 0)
                        goingOut = true;
                }
            }
         }
    }
}
