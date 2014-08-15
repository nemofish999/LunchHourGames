using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LunchHourGames.Hexagonal
{
	public class BoardState
	{
		private Color backgroundColor;
		private Color gridColor;
		private int gridPenWidth;
		private Color activeHexBorderColor;
		private int activeHexBorderWidth;

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

		public Color GridColor
		{
			get
			{
				return gridColor;
			}
			set
			{
				gridColor = value;
			}
		}

		public int GridPenWidth
		{
			get
			{
				return gridPenWidth;
			}
			set
			{
				gridPenWidth = value;
			}
		}

        public void resetActiveCells()
        {
         
        }

        public bool isCellSelected(Hex hex)
        {
            return false;
        }

        public List<Hex> getActiveCells()
        {
            return null;
        }

		public Color ActiveHexBorderColor
		{
			get
			{
				return activeHexBorderColor;
			}
			set
			{
				activeHexBorderColor = value;
			}
		}

        public int ActiveHexBorderWidth
        {
            get
            {
                return activeHexBorderWidth;
            }
            set
            {
                activeHexBorderWidth = value;
            }
        }

		public BoardState()
		{
			backgroundColor = Color.White;
			gridColor = Color.Black;
			gridPenWidth = 1;
			activeHexBorderColor = Color.Blue;
			activeHexBorderWidth = 1;
		}
	}
}
