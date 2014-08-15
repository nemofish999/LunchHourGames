using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames.Sprite;

namespace LunchHourGames.Drawing
{
    public class LHGCursor : StaticSprite
    {
        public enum Type
        {
            Normal,
            Left,
            Right,
            Up,
            Down
        }

        public LHGCursor(LunchHourGames lhg, Texture2D texture, int width, int height, SpriteBatch spriteBatch)
            : base(lhg, texture, Vector2.Zero, spriteBatch)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Draw(texture, Bounds, Color.White);
        }
    }
}
