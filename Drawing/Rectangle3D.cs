using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;

namespace LunchHourGames.Drawing
{
    public class Rectangle3D
    {
        public Vector3 topLeft;
        public Vector3 topRight;
        public Vector3 bottomLeft;
        public Vector3 bottomRight;

        public Rectangle3D(Vector3 topLeft, Vector3 topRight, Vector3 bottomLeft, Vector3 bottomRight)
        {
            this.topLeft = topLeft;
            this.topRight = topRight;
            this.bottomLeft = bottomLeft;
            this.bottomRight = bottomRight;
        }

        public static Rectangle3D Zero
        {
            get { return new Rectangle3D(Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero); }
        }
    }
}
