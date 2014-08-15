using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using  Microsoft.Xna.Framework;

namespace LunchHourGames.Common
{
    public class LHGMath
    {
        public static Vector2 PointOnCircle(float radius, float angleInDegrees, Vector2 origin)
        {
            // Convert from degrees to radians via multiplication by PI/180        
            float x = (float)(radius * Math.Cos(angleInDegrees * Math.PI / 180F)) + origin.X;
            float y = (float)(radius * Math.Sin(angleInDegrees * Math.PI / 180F)) + origin.Y;

            return new Vector2(x, y);
        }
    }
}
