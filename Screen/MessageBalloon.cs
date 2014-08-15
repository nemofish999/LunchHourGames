using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LunchHourGames.Screen
{
    public class MessageBalloon : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public enum Effect
        {
            Pulsate,
            Fade,
            SideScroll
        }

        private LunchHourGames lhg;
        private String message;
        private int maxTimeToShow;
        private Effect effect;
        private SpriteFont font;
        private Vector2 position;

        private TimeSpan startTime;

        public MessageBalloon(LunchHourGames lhg, String message, SpriteFont font, Effect effect, int maxTimeToShow)
            :base(lhg)
        {
            this.lhg = lhg;         
            this.message = message;
            this.font = font;
            this.effect = effect;
            this.maxTimeToShow = maxTimeToShow;
            this.position = new Vector2(1024 / 2, 350);
        }

        public TimeSpan MyStartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; }
        }

        public override void Draw(GameTime gameTime)
        {           
            if ( effect == Effect.Pulsate )
            {
                // Time and Math.Sin are used to determine pulse of selected text
                double time = gameTime.TotalGameTime.TotalSeconds;

                float pulsate = (float)Math.Sin(time * 8) + 1;

                Color color = new Color(255, 200, 30);    // Color of selected text
                float scale = 3 + pulsate * 0.55f;        // Size of pulse indicated by float value here

                // Draw text, centered on the middle of each line, and centered on screen
                Vector2 origin = new Vector2(font.MeasureString(message).X / 2, font.LineSpacing * .5f);

                lhg.MySpriteBatch.DrawString(font, message, position, color, 0, origin, scale, SpriteEffects.None, 0);

                //lhg.MySpriteBatch.DrawString(font, message, position, color);

                //position.Y += font.LineSpacing + 20;
            }           
        }
    }
}
