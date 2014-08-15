using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using LunchHourGames.Sprite;

namespace LunchHourGames.Screen
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class BackgroundPanel : SimpleSprite
    {
        public BackgroundPanel(LunchHourGames lhg, Texture2D texture, bool fill)
            :base(lhg, texture)
        {
            if (fill)
            {
                sourceRectangle = new Rectangle(0,
                            0,
                            Game.Window.ClientBounds.Width,
                            Game.Window.ClientBounds.Height);
            }
            else
            {
                sourceRectangle = new Rectangle(
                    (Game.Window.ClientBounds.Width - texture.Width) / 2,
                    (Game.Window.ClientBounds.Height - texture.Height) / 2,
                    texture.Width,
                    texture.Height);
            }
 
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            lhg.MySpriteBatch.Draw(
               texture,
               Position,
               sourceRectangle,
               Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);

            base.Draw(gameTime);
        }

    }
}
