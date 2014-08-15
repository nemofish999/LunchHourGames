using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames.Drawing;

namespace LunchHourGames.Hexagonal
{
	
	// Represents a 2D hexagon board
	public class HexBoard
	{
		private Hex[,] hexes;
		private int width;
		private int height;
		private int xOffset;
		private int yOffset;
		private int side;
		private float pixelWidth;
		private float pixelHeight;
		private HexOrientation orientation;
		private Color boardColor;
		private Hexagonal.BoardState boardState;
		
		public HexBoard(int width, int height, int side, Hexagonal.HexOrientation orientation, Color boardColor)
		{
			Initialize(width, height, side, orientation, 0, 0, boardColor);
		}

		/// <param name="width">Board width</param>
		/// <param name="height">Board height</param>
		/// <param name="side">Hexagon side length</param>
		/// <param name="orientation">Orientation of the hexagons</param>
		/// <param name="xOffset">X coordinate offset</param>
		/// <param name="yOffset">Y coordinate offset</param>
		public HexBoard(int width, int height, int side, Hexagonal.HexOrientation orientation, int xOffset, int yOffset, Color boardColor)
		{
			Initialize(width, height, side, orientation, xOffset, yOffset, boardColor);
		}

		public Hexagonal.Hex[,] Hexes
		{
			get
			{
				return hexes;
			}
			set
			{
			}
		}

		public float PixelWidth
		{
			get
			{
				return pixelWidth;
			}
			set
			{
			}
		}

		public float PixelHeight
		{
			get
			{
				return pixelHeight;
			}
			set
			{
			}
		}

		public int XOffset
		{
			get
			{
				return xOffset;
			}
			set
			{
			}
		}

		public int YOffset
		{
			get
			{
				return xOffset;
			}
			set
			{
			}
		}

		public int Width
		{
			get
			{
				return width;
			}
			set
			{
			}
		}

		public int Height
		{
			get
			{
				return height;
			}
			set
			{
			}
		}

		public Hexagonal.BoardState BoardState
		{
			get
			{
				return boardState;
			}
			set
			{
				throw new System.NotImplementedException();
			}
		}
		
		private void Initialize(int width, int height, int side, Hexagonal.HexOrientation orientation, int xOffset, int yOffset, Color boardColor)
		{
			this.width = width;
			this.height = height;
			this.xOffset = xOffset;
			this.yOffset = yOffset;
			this.side = side;
			this.orientation = orientation;
			hexes = new Hex[height, width]; //opposite of what we'd expect
			this.boardState = new BoardState();
            this.boardColor = boardColor;

			float h = Hexagonal.HexMath.CalculateH(side); // short side
			float r = Hexagonal.HexMath.CalculateR(side); // long side

			//
			// Calculate pixel info..remove?
			// because of staggering, need to add an extra r/h
			float hexWidth = 0;
			float hexHeight = 0;
			switch (orientation)
			{
				case HexOrientation.Flat:
					hexWidth = side + h;
					hexHeight = r + r;
					this.pixelWidth = (width * hexWidth) + h;
					this.pixelHeight = (height * hexHeight) + r;
					break;
				case HexOrientation.Pointy:
					hexWidth = r + r;
					hexHeight = side + h;
					this.pixelWidth = (width * hexWidth) + r;
					this.pixelHeight = (height * hexHeight) + h;
					break;
				default:
					break;
			}


			bool inTopRow = false;
			bool inBottomRow = false;
			bool inLeftColumn = false;
			bool inRightColumn = false;
			bool isTopLeft = false;
			bool isTopRight = false;
			bool isBotomLeft = false;
			bool isBottomRight = false;


			// i = y coordinate (rows), j = x coordinate (columns) of the hex tiles 2D plane
			for (int i = 0; i < height; i++)
			{                
				for (int j = 0; j < width; j++)
				{
					// Set position booleans
					
						if (i == 0)
						{
							inTopRow = true;
						}
						else
						{
							inTopRow = false;
						}

						if (i == height - 1)
						{
							inBottomRow = true;
						}
						else
						{
							inBottomRow = false;
						}

						if (j == 0)
						{
							inLeftColumn = true;
						}
						else
						{
							inLeftColumn = false;
						}

						if (j == width - 1)
						{
							inRightColumn = true;
						}
						else
						{
							inRightColumn = false;
						}

						if (inTopRow && inLeftColumn)
						{
							isTopLeft = true;
						}
						else
						{
							isTopLeft = false;
						}

						if (inTopRow && inRightColumn)
						{
							isTopRight = true;
						}
						else
						{
							isTopRight = false;
						}

						if (inBottomRow && inLeftColumn)
						{
							isBotomLeft = true;
						}
						else
						{
							isBotomLeft = false;
						}

						if (inBottomRow && inRightColumn)
						{
							isBottomRight = true;
						}
						else
						{
							isBottomRight = false;
						}
					
					//
					// Calculate Hex positions
					//
					if (isTopLeft)
					{
						//First hex
						switch (orientation)
						{ 
							case HexOrientation.Flat:
								hexes[0, 0] = new Hex(0 + h + xOffset, 0 + yOffset, 0, 0, side, orientation, boardColor);
								break;
							case HexOrientation.Pointy:
								hexes[0, 0] = new Hex(0 + r + xOffset, 0 + yOffset, 0, 0, side, orientation, boardColor);
								break;
							default:
								break;
						}
					}
					else
					{
						switch (orientation)
						{
							case HexOrientation.Flat:
								if (inLeftColumn)
								{
									// Calculate from hex above
									hexes[i, j] = new Hex(hexes[i - 1, j].Points[(int)Hexagonal.FlatVertice.BottomLeft], i, j, side, orientation, boardColor);
								}
								else
								{
									// Calculate from Hex to the left and need to stagger the columns
									if (j % 2 == 0)
									{
										// Calculate from Hex to left's Upper Right Vertice plus h and R offset 
										float x = hexes[i, j - 1].Points[(int)Hexagonal.FlatVertice.UpperRight].X;
										float y = hexes[i, j - 1].Points[(int)Hexagonal.FlatVertice.UpperRight].Y;
										x += h;
										y -= r;
										hexes[i, j] = new Hex(x, y, i, j, side, orientation, boardColor);
									}
									else
									{
										// Calculate from Hex to left's Middle Right Vertice
										hexes[i, j] = new Hex(hexes[i, j - 1].Points[(int)Hexagonal.FlatVertice.MiddleRight], i, j, side, orientation, boardColor);
									}
								}
								break;
							case HexOrientation.Pointy:
								if (inLeftColumn)
								{
									// Calculate from hex above and need to stagger the rows
									if (i % 2 == 0)
									{
										hexes[i, j] = new Hex(hexes[i - 1, j].Points[(int)Hexagonal.PointyVertice.BottomLeft], i, j, side, orientation, boardColor);
									}
									else
									{
										hexes[i, j] = new Hex(hexes[i - 1, j].Points[(int)Hexagonal.PointyVertice.BottomRight], i, j, side, orientation, boardColor);
									}

								}
                                else
                                {
                                    // Calculate from Hex to the left
                                    float x = hexes[i, j - 1].Points[(int)Hexagonal.PointyVertice.UpperRight].X;
                                    float y = hexes[i, j - 1].Points[(int)Hexagonal.PointyVertice.UpperRight].Y;
                                    x += r;
                                    y -= h;
                                    hexes[i, j] = new Hex(x, y, i, j, side, orientation, boardColor);
                                }
								break;
							default:
								break;
						}
					}
				}
			}	
		}

		public bool PointInBoardRectangle(Point point)
		{
			return PointInBoardRectangle(point.X,point.Y);
		}

		public bool PointInBoardRectangle(int x, int y)
		{
			//
			// Quick check to see if X,Y coordinate is even in the bounding rectangle of the board.
			// Can produce a false positive because of the staggerring effect of hexes around the edge
			// of the board, but can be used to rule out an x,y point.
			//
			int topLeftX = 0 + XOffset;
			int topLeftY = 0 + yOffset;
			float bottomRightX = topLeftX + pixelWidth;
			float bottomRightY = topLeftY + PixelHeight;


			if (x > topLeftX && x < bottomRightX && y > topLeftY && y < bottomRightY)
			{
				return true;
			}
			else 
			{
				return false;
			}

		}

        public Hex FindHexVector2(Vector2 vector)
        {
            return FindHexMouseClick(new Point((int)vector.X, (int)vector.Y));
        }

		public Hex FindHexMouseClick(Point point)
		{
			return FindHexMouseClick(point.X,point.Y);
		}

		public Hex FindHexMouseClick(int x, int y)
		{
			Hex target = null;

			if (PointInBoardRectangle(x, y))
			{
				for (int i = 0; i < hexes.GetLength(0); i++)
				{
					for (int j = 0; j < hexes.GetLength(1); j++)
					{
						if (HexMath.InsidePolygon(hexes[i, j].Points, 6, new HexPointF(x, y)))
						{
							target = hexes[i, j];
							break;
						}
					}

					if (target != null)
					{
						break;
					}
				}

			}
			
			return target;
			
		}

        public void resetAllHexPaths()
        {
            for (int i = 0; i < hexes.GetLength(0); i++)
            {
                for (int j = 0; j < hexes.GetLength(1); j++)
                {
                    Hex hex = hexes[i, j];
                    hex.Path.reset();
                }
            }
        }

        public bool isBlocked(int i, int j)
        {
            return hexes[i, j].HexState.Blocked;
        }

        public void setBlocked(int i, int j, bool isBlocked)
        {
            hexes[i, j].HexState.Blocked = isBlocked;
        }

        public Hex getHex( int i, int j )
        {
            if ( ( i > -1 && i < hexes.GetLength( 0 ) ) &&
                 ( j > -1 && j < hexes.GetLength( 1 ) ) )
            {
                return hexes[i, j];
            }

            return null;
        }

        public List<Hex> getNeighboringCells(Hex center, int radius)
        {
            List<Hex> neighbors = new List<Hex>();

            if (center != null)
            {
                int i = center.I;
                int j = center.J;

                if (orientation == HexOrientation.Flat)
                {
                    Hex north = getHex(i - 1, j);
                    Hex south = getHex(i + 1, j);
                    Hex northeast = null;
                    Hex southeast = null;
                    Hex northwest = null;
                    Hex southwest = null;

                    if (j % 2 == 0)
                    {
                        northeast = getHex(i - 1, j - 1);
                        southeast = getHex(i, j + 1);
                        northwest = getHex(i - 1, j - 1);
                        southwest = getHex(i, j - 1);
                    }
                    else
                    {
                        northeast = getHex(i, j + 1);
                        southeast = getHex(i + 1, j + 1);
                        northwest = getHex(i, j - 1);
                        southwest = getHex(i + 1, j - 1);
                    }

                    addHexToList(north, neighbors, HexDirection.North);
                    addHexToList(south, neighbors, HexDirection.South);
                    addHexToList(northeast, neighbors, HexDirection.Northeast);
                    addHexToList(southeast, neighbors, HexDirection.Southeast);
                    addHexToList(northwest, neighbors, HexDirection.Northwest);
                    addHexToList(southwest, neighbors, HexDirection.Southwest);

                    if (radius > 1)
                    {
                        // Get the neighbors of our neighbors
                        List<Hex> northNeighbors = getNeighboringCells(north, radius - 1);
                        mergeHexLists(neighbors, northNeighbors);

                        List<Hex> southNeighbors = getNeighboringCells(south, radius - 1);
                        mergeHexLists(neighbors, southNeighbors);

                        List<Hex> northeastNeighbors = getNeighboringCells(northeast, radius - 1);
                        mergeHexLists(neighbors, northeastNeighbors);

                        List<Hex> southeastNeighbors = getNeighboringCells(southeast, radius - 1);
                        mergeHexLists(neighbors, southeastNeighbors);

                        List<Hex> northwestNeighbors = getNeighboringCells(northwest, radius - 1);
                        mergeHexLists(neighbors, northwestNeighbors);

                        List<Hex> southwestNeighbors = getNeighboringCells(southwest, radius - 1);
                        mergeHexLists(neighbors, southwestNeighbors);
                    }
                }
                else if (orientation == HexOrientation.Pointy)
                {
                    Hex east = getHex(i, j + 1);
                    Hex west = getHex(i, j - 1);
                    Hex northeast = null;
                    Hex southeast = null;
                    Hex northwest = null;
                    Hex southwest = null;

                    if (i % 2 == 0)
                    {
                        northeast = getHex(i - 1, j);
                        southeast = getHex(i + 1, j);
                        northwest = getHex(i - 1, j - 1);
                        southwest = getHex(i + 1, j + 1);
                    }
                    else
                    {
                        northeast = getHex(i - 1, j + 1);
                        southeast = getHex(i + 1, j + 1);
                        northwest = getHex(i - 1, j);
                        southwest = getHex(i + 1, j);
                    }
                    
                    addHexToList(east, neighbors, HexDirection.East);
                    addHexToList(west, neighbors, HexDirection.West);
                    addHexToList(northeast, neighbors, HexDirection.Northeast);
                    addHexToList(southeast, neighbors, HexDirection.Southeast);
                    addHexToList(northwest, neighbors, HexDirection.Northwest);
                    addHexToList(southwest, neighbors, HexDirection.Southwest);

                    if (radius > 1)
                    {
                        // Get the neighbors of our neighbors
                        List<Hex> eastNeighbors = getNeighboringCells(east, radius - 1);
                        mergeHexLists(neighbors, eastNeighbors);

                        List<Hex> westNeighbors = getNeighboringCells(west, radius - 1);
                        mergeHexLists(neighbors, westNeighbors);

                        List<Hex> northeastNeighbors = getNeighboringCells(northeast, radius - 1);
                        mergeHexLists(neighbors, northeastNeighbors);

                        List<Hex> southeastNeighbors = getNeighboringCells(southeast, radius - 1);
                        mergeHexLists(neighbors, southeastNeighbors);

                        List<Hex> northwestNeighbors = getNeighboringCells(northwest, radius - 1);
                        mergeHexLists(neighbors, northwestNeighbors);

                        List<Hex> southwestNeighbors = getNeighboringCells(southwest, radius - 1);
                        mergeHexLists(neighbors, southwestNeighbors);
                    }
                }
            }

            return neighbors;
        }

        public Hex getHexNextTo(Hex center, HexDirection direction)
        {
            int i = center.I;
            int j = center.J;

            Hex nextHex = null;

            if (orientation == HexOrientation.Flat)
            {
                switch (direction)
                {
                    case HexDirection.North:
                        nextHex = getHex(i - 1, j);
                        break;

                    case HexDirection.Northeast:
                        if (j % 2 == 0)
                            nextHex = getHex(i - 1, j - 1);
                        else
                            nextHex = getHex(i, j + 1);
                        break;

                    case HexDirection.East:
                        break;

                    case HexDirection.Southeast:
                        if (j % 2 == 0)
                            nextHex = getHex(i, j + 1);
                        else
                            nextHex = getHex(i + 1, j + 1);
                        break;

                    case HexDirection.South:
                        Hex south = getHex(i + 1, j);
                        break;

                    case HexDirection.Southwest:
                        if (j % 2 == 0)
                            nextHex = getHex(i, j - 1);
                        else
                            nextHex = getHex(i + 1, j - 1);
                        break;

                    case HexDirection.West:
                        break;

                    case HexDirection.Northwest:
                        if (j % 2 == 0)
                            nextHex = getHex(i - 1, j - 1);
                        else
                            nextHex = getHex(i, j - 1);
                        break;
                }
            }
            else if (orientation == HexOrientation.Pointy)
            {
                switch (direction)
                {
                    case HexDirection.North:
                        break;

                    case HexDirection.Northeast:
                        if (i % 2 == 0)
                            nextHex = getHex(i - 1, j);
                        else
                            nextHex = getHex(i - 1, j + 1);
                        break;

                    case HexDirection.East:
                        Hex east = getHex(i, j + 1);
                        break;

                    case HexDirection.Southeast:
                        if (i % 2 == 0)
                            nextHex = getHex(i + 1, j);
                        else
                            nextHex = getHex(i + 1, j + 1);
                        break;

                    case HexDirection.South:
                        break;

                    case HexDirection.Southwest:
                        if (i % 2 == 0)
                            nextHex = getHex(i + 1, j + 1);
                        else
                            nextHex = getHex(i + 1, j);
                        break;

                    case HexDirection.West:
                        Hex west = getHex(i, j - 1);
                        break;

                    case HexDirection.Northwest:
                        if (i % 2 == 0)
                            nextHex = getHex(i - 1, j - 1);
                        else
                            nextHex = getHex(i - 1, j);
                        break;
                }
            }

            return nextHex;
        }

        public List<Hex> getForwardCells(Hex center, HexDirection facingDirection, int length)
        {
            List<Hex> cells = new List<Hex>();

            return cells;
        }

        private bool addHexToList(Hex hex, List<Hex> list, HexDirection direction)
        {
            if (addHexToList(hex, list))
            {
                hex.HexState.Direction = direction;
                return true;
            }

            return false;
        }

        private bool addHexToList(Hex hex, List<Hex> list)
        {
            if (hex != null)
            {
                if (!list.Contains(hex))
                {
                    list.Add(hex);
                    return true;
                }
            }

            return false;
        }

        private void mergeHexLists(List<Hex> master, List<Hex> servant)
        {
            foreach (Hex hex in servant)
            {
                addHexToList(hex, master);
            }
        }

        public int getHexDistance(Hex start, Hex stop)
        {
            int i1 = start.I;
            int j1 = start.J;

            int i2 = stop.I;
            int j2 = stop.J;

            int i = 0;
            int j = 0;
            if (i1 > i2)
                i = i1 - i2;
            else
                i = i2 - i1;

            if (j1 > j2)
                j = j1 - j2;
            else
                j = j2 - j1;

            int sum = i + j - 1;
            return sum;
        }

        public Point getCellCenter(int i, int j)
        {
            Hex hex = getHex(i, j);
            if (hex != null)
            {
                HexPointF hexPointF = hex.getCenter();
                return new Point(Convert.ToInt32(System.Math.Ceiling(hexPointF.X)),
                                 Convert.ToInt32(System.Math.Ceiling(hexPointF.Y)));
            }

            return new Point(0, 0);
        }
	}
}
