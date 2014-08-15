using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LunchHourGames.Screen
{
    class LogoScreen : GameScreen
    {
        Texture2D logo;
        TimeSpan myTimeSpan;
        Vector2 position;
        bool topReached = false;
        Color tintColor = Color.White;

        public LogoScreen(LunchHourGames lhg)
            : base(lhg, Type.Logo)
        {
            LoadContent();
            position = new Vector2(0, Game.Window.ClientBounds.Height);

            TransitionPosition = 1;
            TransitionOnTime = TimeSpan.Zero;
            TransitionOffTime = new TimeSpan(0, 0, 2);  // Allow 2 seconds for fade
        }

        protected override void LoadContent()
        {
            logo = lhg.Content.Load<Texture2D>("Backgrounds/lhglogo1024");
            base.LoadContent();
        }
      
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (playState != PlayState.Finished)
            {
                myTimeSpan += gameTime.ElapsedGameTime;
                if (myTimeSpan > TimeSpan.FromMilliseconds(25) && !topReached)
                {
                    position.Y -= 25.0f;
                    myTimeSpan -= TimeSpan.FromMilliseconds(25);
                    if (position.Y < 0.0f)
                    {
                        position.Y = 0.0f;
                        topReached = true;
                    }
                }

                if (myTimeSpan > TimeSpan.FromSeconds(2) & topReached)
                {
                    playState = PlayState.Finished;

                    // Tell the game system we are done, so it can decide what to do next
                    lhg.MyGameFlow.setGameScreenComplete(this);
                }
            }
      
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            lhg.GraphicsDevice.Clear(Color.White);

            SpriteBatch spriteBatch = lhg.MySpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(logo, position, tintColor);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
