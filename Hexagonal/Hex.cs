using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LunchHourGames.Hexagonal
{
	public class Hex
	{
		private HexPointF[] points;
		private float side;
		private float h;
		private float r;
		private HexOrientation orientation;
		private float x;
		private float y;
        private int i;
        private int j;
		private HexState hexState;

        private AStar astar;
       
        private GameEntity gameEntity = null;  // Represents the entity that is on this hex
        private GameEntity inactiveEntity = null;
	
		/// <param name="side">length of one side of the hexagon</param>
        public Hex(int x, int y, int i, int j, int side, Hexagonal.HexOrientation orientation, Color hexColor)
		{
			Initialize(Hexagonal.HexMath.ConvertToFloat(x), Hexagonal.HexMath.ConvertToFloat(y), i, j, Hexagonal.HexMath.ConvertToFloat(side),orientation, hexColor);
		}

		public Hex(float x, float y, int i, int j, float side, Hexagonal.HexOrientation orientation, Color hexColor)
		{
			Initialize(x, y, i, j, side, orientation, hexColor);
		}

		public Hex(HexPointF point, int i, int j, float side, HexOrientation orientation, Color hexColor)
		{
			Initialize(point.X, point.Y, i, j, side, orientation, hexColor);
		}

		public Hex()
		{ }

		/// <summary>
		/// Sets internal fields and calls CalculateVertices()
		/// </summary>
        private void Initialize(float x, float y, int i, int j, float side, Hexagonal.HexOrientation orientation, Color hexColor)
		{
			this.x = x;
			this.y = y;
            this.i = i;
            this.j = j;
			this.side = side;
			this.orientation = orientation;
			this.hexState = new HexState();
            this.hexState.BackgroundColor = hexColor;
            this.astar = new AStar();
           
			CalculateVertices();
		}

		/// <summary>
		/// Calculates the vertices of the hex based on orientation. Assumes that points[0] contains a value.
		/// </summary>
		private void CalculateVertices()
		{
			//  
			//  h = short length (outside)
			//  r = long length (outside)
			//  side = length of a side of the hexagon, all 6 are equal length
			//
			//  h = sin (30 degrees) x side
			//  r = cos (30 degrees) x side
			//
			//		 h
			//	     ---
			//   ----     |r
			//  /    \    |          
			// /      \   |
			// \      /
			//  \____/
			//
			// Flat orientation (scale is off)
			//
	        //     /\
			//    /  \
			//   /    \
			//   |    |
			//   |    |
			//   |    |
			//   \    /
			//    \  /
			//     \/
			// Pointy orientation (scale is off)
         
			h = HexMath.CalculateH(side);
			r = HexMath.CalculateR(side);

			switch (orientation)
			{ 
				case Hexagonal.HexOrientation.Flat:
					// x,y coordinates are top left point
					points = new HexPointF[6];
					points[0] = new HexPointF(x, y);
					points[1] = new HexPointF(x + side, y);
					points[2] = new HexPointF(x + side + h, y + r);
					points[3] = new HexPointF(x + side, y + r + r);
					points[4] = new HexPointF(x, y + r + r);
					points[5] = new HexPointF(x - h, y + r );
					break;
				case Hexagonal.HexOrientation.Pointy:
					//x,y coordinates are top center point
					points = new HexPointF[6];
					points[0] = new HexPointF(x, y);
					points[1] = new HexPointF(x + r, y + h);
					points[2] = new HexPointF(x + r, y + side + h);
					points[3] = new HexPointF(x, y + side + h + h);
					points[4] = new HexPointF(x - r, y + side + h);
					points[5] = new HexPointF(x - r, y + h);
					break;
				default:
					throw new Exception("No HexOrientation defined for Hex object.");
			
			}
		}

		public HexOrientation Orientation
		{
			get
			{
				return orientation;
			}
			set
			{
				orientation = value;
			}
		}

		public HexPointF[] Points
		{
			get
			{
				return points;
			}
			set
			{
			}
		}

        public float Side
		{
			get
			{
				return side;
			}
			set
			{
			}
		}

		public float H
		{
			get
			{
				return h;
			}
			set
			{
			}
		}

		public float R
		{
			get
			{
				return r;
			}
			set
			{
			}
		}

        public int I
        {
            get
            {
                return i;
            }
            set
            {
                i = value;
            }
        }

        public int J
        {
            get
            {
                return j;
            }
            set
            {
                j = value;
            }
        }

		public HexState HexState
		{
			get
			{
				return hexState;
			}
			set
			{
				throw new System.NotImplementedException();
			}
		}

        public AStar Path
        {
            get { return astar; }
        }

        public HexPointF getCenter()
        {
            float x;
            float y;
            switch (orientation)
            {
                case Hexagonal.HexOrientation.Flat:
                    x = points[0].X + ((points[1].X - points[0].X) / 2);
                    y = points[0].Y + ((points[4].Y - points[0].Y) / 2);
                    return new HexPointF(x, y);

                case Hexagonal.HexOrientation.Pointy:
                    y = points[0].Y + ((points[3].Y - points[0].Y) / 2);
                    x = points[0].X; //points[5].X + ((points[1].X - points[5].X) / 2);
                    return new HexPointF(x, y);
            }

            return new HexPointF(0,0);
        }

        public Vector2 getCenterAsVector()
        {
            HexPointF hexPointF = getCenter();
            return new Vector2(hexPointF.X, hexPointF.Y);
        }

        public GameEntity MyGameEntity
        {
            get { return this.gameEntity; }
            set
            { 
                this.gameEntity = value;
                if (value == null)
                    hexState.Blocked = false;
                else
                    hexState.Blocked = true;
            }
        }

        public GameEntity InactiveEntity
        {
            get { return this.inactiveEntity;  }
            set { this.inactiveEntity = value; }
        }
	}
}
