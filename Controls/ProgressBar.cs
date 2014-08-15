using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LunchHourGames.Drawing
{
    public class ProgressBar : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected float minimum = 0.0f;
        protected float maximum = 100.0f;
        protected float progress = 0;

        protected Rectangle borderOuterRect;
        protected Rectangle borderInnerRect;
        protected Rectangle backgroundRect;
        protected Rectangle fillRect;

        protected Color borderColorOuter;
        protected Int32 borderThicknessOuter;

        protected Color borderColorInner;
        protected Int32 borderThicknessInner;

        protected Color fillColor;
        protected Color backgroundColor;

        protected Color[] outerData;
        protected Color[] innerData;
        protected Color[] fillData;
        protected Color[] backgroundData;
        protected Texture2D outerTexture;
        protected Texture2D innerTexture;
        protected Texture2D backgroundTexture;
        protected Texture2D fillTexture;

        protected Orientation orientation;

        public float Minimum
        {
            get
            {
                return minimum;
            }
            set
            {
                minimum = value;
                // causes progress to update, and rectangles to update
                Value = progress;
            }
        }
     
        public float Maximum
        {
            get
            {
                return maximum;
            }
            set
            {
                maximum = value;
                // causes progress to update, and rectangles to update
                Value = progress;
            }
        }

        public float Value
        {
            get
            {
                return progress;
            }
            set
            {
                progress = value;
                if (progress < minimum)
                    progress = minimum;
                else if (progress > maximum)
                    progress = maximum;
                UpdateRectangles();
            }
        }

        public Color BorderColorOuter
        {
            get
            {
                return borderColorOuter;
            }
            set
            {
                if (borderColorOuter != value)
                {
                    borderColorOuter = value;
                    outerData[0] = borderColorOuter;
                    outerTexture = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                    outerTexture.SetData(outerData);
                }
            }
        }
     
        public Int32 BorderThicknessOuter
        {
            get
            {
                return borderThicknessOuter;
            }
            set
            {
                borderThicknessOuter = value;
            }
        }
   
        public Color BorderColorInner
        {
            get
            {
                return borderColorInner;
            }
            set
            {
                if (borderColorInner != value)
                {
                    borderColorInner = value;
                    innerData[0] = borderColorInner;
                    innerTexture = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                    innerTexture.SetData(innerData);
                }
            }
        }

           public Int32 BorderThicknessInner
        {
            get
            {
                return borderThicknessInner;
            }
            set
            {
                borderThicknessInner = value;
            }
        }

        public Color FillColor
        {
            get
            {
                return fillColor;
            }
            set
            {
                if (fillColor != value)
                {
                    fillColor = value;
                    fillData[0] = fillColor;
                    fillTexture.Dispose();
                    fillTexture = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                    fillTexture.SetData(fillData);
                }
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                if (backgroundColor != value)
                {
                    backgroundColor = value;
                    backgroundData[0] = backgroundColor;
                    backgroundTexture = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                    backgroundTexture.SetData(backgroundData);
                }
            }
        }

        public enum Orientation
        {
            HORIZONTAL_LR, // default, horizontal orientation, left to right fill
            HORIZONTAL_RL, // horizontal orientation, right to left fill
            VERTICAL_TB, // vertical orientation, top to bottom fill
            VERTICAL_BT, // vertical orientation, bottom to top fill
        }

        /// <summary>
        /// Gets the orientation of this progress bar.  Set at creation time.
        /// </summary>
        public Orientation MyOrientation
        {
            get
            {
                return orientation;
            }
        }
      
        public ProgressBar(Game game, Rectangle rect)
            : base(game)
        {
            borderOuterRect = rect;
            orientation = Orientation.HORIZONTAL_LR;

            Initialize();
        }

        public ProgressBar(Game game, Rectangle rect, Orientation orientation)
            : base(game)
        {
            borderOuterRect = rect;
            this.orientation = orientation;

            Initialize();
        }
   
        public ProgressBar(Game game, Int32 x, Int32 y, Int32 width, Int32 height)
            : base(game)
        {
            borderOuterRect = new Rectangle(x, y, width, height);
            orientation = Orientation.HORIZONTAL_LR;

            Initialize();
        }

        public ProgressBar(Game game, Int32 x, Int32 y, Int32 width, Int32 height, Orientation orientation)
            : base(game)
        {
            borderOuterRect = new Rectangle(x, y, width, height);
            this.orientation = orientation;

            Initialize();
        }
 
        public override void Initialize()
        {
            // create some textures.  These will actually be overwritten when colors are set below.
            outerTexture = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            innerTexture = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            backgroundTexture = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            fillTexture = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);

            // initialize data arrays for building textures
            outerData = new Color[1];
            innerData = new Color[1];
            fillData = new Color[1];
            backgroundData = new Color[1];

            // initialize colors
            borderColorOuter = Color.Gray;
            borderColorInner = Color.Black;
            fillColor = Color.DarkBlue;
            backgroundColor = Color.White;

            // set border thickness
            borderThicknessInner = 2;
            borderThicknessOuter = 3;

            // calculate the rectangles for displaying the progress bar
            UpdateRectangles();

            base.Initialize();
        }
        
        protected void UpdateRectangles()
        {
            // figure out inner border
            borderInnerRect = borderOuterRect;
            borderInnerRect.Inflate(borderThicknessOuter * -1, borderThicknessOuter * -1);

            // figure out background rectangle
            backgroundRect = borderInnerRect;
            backgroundRect.Inflate(borderThicknessInner * -1, borderThicknessInner * -1);

            // figure out fill rectangle based on progress.
            fillRect = backgroundRect;
            float percentProgress = (progress - minimum) / (maximum - minimum);
            // calculate fill properly according to orientation
            switch (orientation)
            {
                case Orientation.HORIZONTAL_LR:
                    fillRect.Width = (int)((float)fillRect.Width * percentProgress); break;
                case Orientation.HORIZONTAL_RL:
                    // right to left means short the fill rect as usual, but it must justified to the right
                    fillRect.Width = (int)((float)fillRect.Width * percentProgress); 
                    fillRect.X = backgroundRect.Right - fillRect.Width;
                    break;
                case Orientation.VERTICAL_BT:
                    //justify the fill to the bottom
                    fillRect.Height = (int)((float)fillRect.Height * percentProgress);
                    fillRect.Y = backgroundRect.Bottom - fillRect.Height;
                    break;
                case Orientation.VERTICAL_TB:
                    fillRect.Height = (int)((float)fillRect.Height * percentProgress); break;
                default:// default is HORIZONTAL_LR
                    fillRect.Width = (int)((float)fillRect.Width * percentProgress); break;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {         
            // draw the outer border
            spriteBatch.Draw(outerTexture, borderOuterRect, Color.White);
            // draw the inner border
            spriteBatch.Draw(innerTexture, borderInnerRect, Color.White);
            // draw the background color
            spriteBatch.Draw(backgroundTexture, backgroundRect, Color.White);
            // draw the progress
            spriteBatch.Draw(fillTexture, fillRect, Color.White);
        }       
    }
}