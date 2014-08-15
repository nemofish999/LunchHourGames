using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames.Sprite;

namespace LunchHourGames.Players
{
    class NumberBox : StaticSprite
    {
        private String attributeName;
        private int value;
        private int min;
        private int max;
        private SpriteFont font;
        private Texture2D texture;     
       
        private Rectangle upButton;
        private Rectangle downButton;

        Color color = new Color(255, 0, 0);

        public NumberBox(LunchHourGames lhg, String attributeName, int value, Vector2 position, int min, int max, SpriteFont font, 
                         Texture2D background, Rectangle upButton, Rectangle downButton)
             : base(lhg, background, position, lhg.MySpriteBatch)
        {
            this.attributeName = attributeName;
            this.value = value;
            this.min = min;
            this.max = max;            
            this.font = font;            
            this.upButton = upButton;
            this.downButton = downButton;
        }

        public bool isPointInBox(Point point)
        {
            return this.Bounds.Contains(point);
        }

        public String MyAttributeName
        {
            get { return this.attributeName; }
        }

        public int determineMouseClick(Point point)
        {
            Point click = new Point(point.X - (int)position.X, point.Y - (int)position.Y);
            if (this.upButton.Contains(click))
            {
                this.value += 1;
                if ( this.value > this.max )
                {
                    this.value = this.max;
                    return 0;
                }

                return 1;
            }
            if (this.downButton.Contains(click))
            {
                this.value -= 1;
                if (this.value < this.min)
                {
                    this.value = this.min;
                    return 0;
                }

                return -1;
            }

            return 0;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            
            float scale = 3;

            String numberAsString = string.Format("{0}", value); 

            // Draw text, centered on the middle of each line, and centered on screen
            Vector2 origin = new Vector2(font.MeasureString(numberAsString).X / 2, font.LineSpacing * .5f);
            Vector2 textPosition = new Vector2(position.X + 35, position.Y + 35);
            lhg.MySpriteBatch.DrawString(font, numberAsString, textPosition, color, 0, origin, scale, SpriteEffects.None, 0);

        }
    }
}
