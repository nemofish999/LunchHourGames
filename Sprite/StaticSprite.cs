using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LunchHourGames.Sprite
{
    public class StaticSprite : SimpleSprite
    {
        protected SpriteBatch spriteBatch;

        public StaticSprite(LunchHourGames lhg, Texture2D texture, Vector2 position, SpriteBatch spriteBatch) 
            :base(lhg, texture, position)
            
        {
            this.spriteBatch = spriteBatch;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Draw(texture, Bounds, Color.White);
        }
    }
}
