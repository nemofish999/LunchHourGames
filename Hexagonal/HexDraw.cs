using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using LunchHourGames.Drawing;

namespace LunchHourGames.Hexagonal
{
	public class HexDraw
	{
		private HexBoard board;
		private float boardPixelWidth;
		private float boardPixelHeight;
		private int boardXOffset;
		private int boardYOffset;
	
		public HexDraw(Hexagonal.HexBoard board)
		{
			this.Initialize(board, 0, 0);
		}

		public HexDraw(Hexagonal.HexBoard board, int xOffset, int yOffset)
		{
			this.Initialize(board, xOffset, yOffset);
		}

		public int BoardXOffset
		{
			get
			{
				return boardXOffset;
			}
			set
			{
				throw new System.NotImplementedException();
			}
		}

		public int BoardYOffset
		{
			get
			{
				return boardYOffset;
			}
			set
			{
				throw new System.NotImplementedException();
			}
		}
		
		private void Initialize(Hexagonal.HexBoard board, int xOffset, int yOffset)
		{
			this.board = board;
			this.boardXOffset = xOffset;
			this.boardYOffset = yOffset;
		}

		public void Draw( PrimitiveBatch primitiveBatch )
		{ 
			int width =  Convert.ToInt32(System.Math.Ceiling(board.PixelWidth));
			int height = Convert.ToInt32(System.Math.Ceiling(board.PixelHeight));
			// seems to be needed to avoid bottom and right from being chopped off
			width += 1;
			height += 1;

            // tell the primitive batch to start drawing lines
            primitiveBatch.Begin(PrimitiveType.LineList);

			//
			// Draw Hex Background 
			//
			for (int i = 0; i < board.Hexes.GetLength(0); i++)
			{
				for (int j = 0; j < board.Hexes.GetLength(1); j++)
				{
                    drawHex(primitiveBatch, board.Hexes[i, j]);
				}
			}

			//
			// Draw Active Hex, if present
			//
            List<Hex> activeCells = board.BoardState.getActiveCells();
            foreach ( Hex hex in activeCells )
                drawHex( primitiveBatch, hex);

            // and we're done.
            primitiveBatch.End();
		}

        private void drawHex(PrimitiveBatch primitiveBatch, Hex hex)
        {
            primitiveBatch.AddVertex(new Vector2(hex.Points[0].X, hex.Points[0].Y), hex.HexState.BackgroundColor);
            primitiveBatch.AddVertex(new Vector2(hex.Points[1].X, hex.Points[1].Y), hex.HexState.BackgroundColor);
            primitiveBatch.AddVertex(new Vector2(hex.Points[2].X, hex.Points[2].Y), hex.HexState.BackgroundColor);
            primitiveBatch.AddVertex(new Vector2(hex.Points[3].X, hex.Points[3].Y), hex.HexState.BackgroundColor);
            primitiveBatch.AddVertex(new Vector2(hex.Points[4].X, hex.Points[4].Y), hex.HexState.BackgroundColor);
            primitiveBatch.AddVertex(new Vector2(hex.Points[5].X, hex.Points[5].Y), hex.HexState.BackgroundColor);
        }


        public void Draw(LHGGrid grid)
        {
             // tell the grid to start drawing lines
            grid.Begin();

            //
            // Draw Hex Background 
            //
            for (int i = 0; i < board.Hexes.GetLength(0); i++)
            {
                for (int j = 0; j < board.Hexes.GetLength(1); j++)
                {
                    drawHex(grid, board.Hexes[i, j]);
                }
            }
        
            // and we're done.
            grid.End();
        }

        private void drawHex(LHGGrid grid, Hex hex )
        {
            grid.AddVertex(new Vector3(hex.Points[0].X, 0.0f, hex.Points[0].Y), hex.HexState.BackgroundColor);
            grid.AddVertex(new Vector3(hex.Points[1].X, 0.0f, hex.Points[1].Y), hex.HexState.BackgroundColor);
            grid.AddVertex(new Vector3(hex.Points[2].X, 0.0f, hex.Points[2].Y), hex.HexState.BackgroundColor);
            grid.AddVertex(new Vector3(hex.Points[3].X, 0.0f, hex.Points[3].Y), hex.HexState.BackgroundColor);
            grid.AddVertex(new Vector3(hex.Points[4].X, 0.0f, hex.Points[4].Y), hex.HexState.BackgroundColor);
            grid.AddVertex(new Vector3(hex.Points[5].X, 0.0f, hex.Points[5].Y), hex.HexState.BackgroundColor);
        }
 	}
}
