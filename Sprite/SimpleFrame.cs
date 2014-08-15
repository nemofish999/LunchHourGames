using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace LunchHourGames.Sprite
{
    public class SimpleFrame
    {
        private String referenceName;
        private int frameWidth;
        private int frameHeight;

        private Rectangle extents;

        public SimpleFrame(String referenceName, int xOffset, int yOffset, int frameWidth, int frameHeight )
        {
            this.referenceName = referenceName;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;

            extents = new Rectangle( xOffset, yOffset, frameWidth, frameHeight);
        }

        public String MyReferenceName
        {
            get { return this.referenceName; }
        }

        public int MyWidth
        {
            get { return this.frameWidth; }
        }

        public int MyHeight
        {
            get { return this.frameHeight; }
        }

        public Rectangle MyExtents
        {
            get { return this.extents; }
        }
    }
}
