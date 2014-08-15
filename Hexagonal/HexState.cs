using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LunchHourGames.Hexagonal
{
	public class HexState
	{
		private Color backgroundColor;
        private bool isBlocked;
        private HexDirection direction;

		public Color BackgroundColor
		{
			get
			{
                return backgroundColor;
			}
			set
			{
				backgroundColor = value;
			}
		}

        public bool Blocked
        {
            get
            {
                return isBlocked;
            }
            set
            {
                isBlocked = value;
            }
        }

        public HexDirection Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
            }
        }

      	public HexState()
		{
            this.backgroundColor = Color.DarkGreen;
            this.isBlocked = false;
		}
	}
}
