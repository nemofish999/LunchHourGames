using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames;
using LunchHourGames.Sprite;
using LunchHourGames.Combat;

namespace LunchHourGames.Obstacles
{
    public class ObstacleSprite : SimpleSprite3D
    {
        public ObstacleSprite(LunchHourGames lhg, Texture2D texture)
            :base(lhg, texture)
        {
        }

        public void updateLocation(CombatLocation combatLocation)
        {
            Vector3 position = combatLocation.position;
            position.Y += (this.height - 30);

            // Align the sprite in the center of the position
            float centerX = this.width / 2;
            position.X -= centerX;

            MyPosition = position;
        }

        public ObstacleSprite copy()
        {
            return new ObstacleSprite(lhg, MyTexture);
        }
    }
}
