using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LunchHourGames.Hexagonal;
using LunchHourGames.Sprite;
using LunchHourGames.Drawing;
using LunchHourGames.Common;
using LunchHourGames.Players;
using LunchHourGames.Obstacles;
using LunchHourGames.Combat.Attacks;

namespace LunchHourGames.Combat
{
    // Maintains and handles the drawing of the hexagonal board used in the combat screens
    public class CombatBoard : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Our main game object.  used to reach global resources (fonts, sound system, etc)
        private LunchHourGames lhg;

        private CombatSystem combatSystem;
        private CombatScreen combatScreen;
        private CombatAttack combatAttack;

        // Grid object used to draw this hex board in true 3D space
        private LHGGrid grid;

        // Our Hex board.  Manages hexagonal logic.
        private Hexagonal.HexBoard hexBoard;

        // Handles the drawing of the hex to the grid
        private Hexagonal.HexDraw hexDraw;

        private int hexSize;
        private int hexBoardHeight;
        private int hexBoardWidth;
        private int xOffset;
        private int yOffset;
        private int penWidth;
        private Hexagonal.HexOrientation orientation = HexOrientation.Flat;

        private Color backgroundColor, selectedPlayerColor, moveRadiusColor,
                      moveSelectColor, attackRadiusColor;

        private List<Hex> selectedPlayerHexes = new List<Hex>();
        private List<Hex> moveRadiusHexes = new List<Hex>();
        private List<Hex> moveSelectHexes = new List<Hex>();
        private List<Hex> attackRadiusHexes = new List<Hex>();

        private int zoomInValue = 0;
        private int zoomOutValue = 0;

        private Rectangle3D extents;

        private List<Hex> activeHexes = new List<Hex>();

        private GameEntities gameEntities;

        private List<SimpleSprite3D> floorSprites = new List<SimpleSprite3D>();

        private HexSelection playerHexSelection;

        private bool gridNumbersVisible;

        public CombatBoard(LunchHourGames lhg, int hexSize, int width, int height,
                           int xOffset, int yOffset, int penWidth, bool isPointy, Color backgroundColor)
            : base(lhg)
        {
            this.lhg = lhg;
            this.gridNumbersVisible = false;

            this.grid = new LHGGrid(lhg);
            grid.LoadGraphicsContent(lhg.GraphicsDevice);
            
            initialize(lhg, hexSize, width, height, xOffset, yOffset, penWidth, isPointy, backgroundColor);       
        }

        public void initialize(LunchHourGames lhg, int hexSize, int width, int height,
                               int xOffset, int yOffset, int penWidth, bool isPointy, Color backgroundColor)
        {
            this.hexSize = hexSize;
            this.hexBoardWidth = width;
            this.hexBoardHeight = height;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.penWidth = penWidth;
            this.backgroundColor = backgroundColor;

            if (isPointy)
                this.orientation = HexOrientation.Pointy;
            else
                this.orientation = HexOrientation.Flat;          

            // Create the board.  Add all these 
            createHexagonalBoard();
            this.extents = get3DExtents();

            // Go ahead and draw the hex board to the grid
            this.hexDraw.Draw(grid);
        }

        public CombatSystem MyCombatSystem
        {
            get { return this.combatSystem; }
            set { this.combatSystem = value; }
        }

        public CombatScreen MyCombatScreen
        {
            get { return this.combatScreen; }
            set
            {
                this.combatScreen = value;

                // Since we are associating a screen to this board, lets pull the graphic matrices so we can draw our grid, players, and obstacles correctly.
                grid.setGraphicsMatrices(combatScreen.MyView, combatScreen.MyProjection, combatScreen.MyWorld);

                // Set the graphics matrices for all the game entities on the board
                gameEntities.setGraphicsMatrices(combatScreen.MyView, combatScreen.MyProjection, combatScreen.MyWorld);
            }
        }

        public GameEntities MyGameEntities
        {
            get { return this.gameEntities; }
            set { this.gameEntities = value; }
        }

        public void addGameEntity(GameEntity gameEntity)
        {
            gameEntities.Add(gameEntity);
        }

        public void removeGameEntity(GameEntity gameEntity)
        {
            gameEntities.Remove(gameEntity);
        }

        public CombatAttack MyCombatAttack
        {
            get { return this.combatAttack; }
            set { this.combatAttack = value; }
        }

        public int getBoardWidth()
        {
            return this.hexBoardWidth;
        }

        public int getBoardHeight()
        {
            return this.hexBoardHeight;
        }

        public LHGGrid MyGrid
        {
            get { return this.grid; }
        }

        public Player CurrentPlayer
        {
            get { return this.combatSystem.MyTurnBased.CurrentPlayer; }
        }

        public List<Player> getPlayer(Player.Type type)
        {
            return this.gameEntities.getPlayer(type);
        }

        public List<Human> getHumans()
        {
            List<Player> players = this.gameEntities.getPlayer(Player.Type.Human);

            List<Human> humans = new List<Human>();
            foreach (Player player in players)
            {
                humans.Add((Human)player);
            }

            return humans;
        }

        public List<Zombie> getZombies()
        {
            List<Player> players = this.gameEntities.getPlayer(Player.Type.Zombie);

            List<Zombie> zombies = new List<Zombie>();
            foreach (Player player in players)
            {
                zombies.Add((Zombie)player);
            }

            return zombies;
        }


        public void showGridNumbers(bool show)
        {
            this.gridNumbersVisible = show;
        }

        public void setHexColors(Color backgroundColor, Color selectedPlayerColor, Color moveRadiusColor,
                                 Color moveSelectColor, Color attackRadiusColor)
        {
            this.backgroundColor = backgroundColor;
            this.selectedPlayerColor = selectedPlayerColor;
            this.moveRadiusColor = moveRadiusColor;
            this.moveSelectColor = moveSelectColor;
            this.attackRadiusColor = attackRadiusColor;
        }

        private void createHexagonalBoard()
        {
            this.hexBoard = new HexBoard(hexBoardWidth, hexBoardHeight, hexSize, orientation, xOffset, yOffset, backgroundColor);
            this.hexBoard.BoardState.BackgroundColor = Color.Green;
            this.hexBoard.BoardState.GridPenWidth = penWidth;
            this.hexBoard.BoardState.ActiveHexBorderColor = Color.Red;
            this.hexBoard.BoardState.ActiveHexBorderWidth = penWidth;

            this.hexDraw = new HexDraw(hexBoard, xOffset, yOffset);
        }

        public void highlightCurrentPlayer(bool highlight)
        {
            if (playerHexSelection != null)
            {
                playerHexSelection.Dispose();
                playerHexSelection = null;
            }

            if (highlight)
            {
                this.playerHexSelection = new HexSelection(this.lhg, CurrentPlayer.getCurrentHex(), HexSelection.Type.Pulsate, true, 15, Color.Yellow);
                this.playerHexSelection.Pulsate = true;
            }
        }
  
        public override void Update(GameTime gameTime)
        {
            // Update all the game entities on the board
            foreach (GameEntity gameEntity in gameEntities.MyGameEntities)
                gameEntity.Update(gameTime);

            // Update player selection if applicable
            if (playerHexSelection != null)
                playerHexSelection.Update(gameTime);

            if (combatAttack != null)
                combatAttack.Update(gameTime);
            
            base.Update(gameTime);
        }

        // Draws the entire combat board:  floor, grid, player, and obstacles.  Note the order in the way they are draw.  This is to show proper Z-order.
        public override void Draw(GameTime gameTime)
        {   
            // Update the grid's view.  The projection and world matrices won't change
            grid.ViewMatrix = combatScreen.MyView;
            grid.Draw();

            // Display grid numbers
            if (gridNumbersVisible)
                drawGridNumbers();

            // Draw player selection if applicable
            if (playerHexSelection != null)
            {
                playerHexSelection.setGraphicsMatrices(combatScreen.MyView, combatScreen.MyProjection, combatScreen.MyWorld);
                playerHexSelection.Draw();
            }
            
            // Draw the game entities on the board. 
            // We have to draw it this way so we get correct z-ordering.
            // When drawing a 3D scene, it is important to depth sort the graphics, so things that are close to the camera will be drawn
            // over the top of things from further away. We do not want those distant mountains to be drawn over the top of the building
            // that is right in front of us! There are three depth sorting techniques in widespread use today:
            //
            //     Depth buffering (aka. z-buffering)
            //     Painter’s algorithm
            //     Backface culling
            //
            // Unfortunately, all have limitations. To get good results, most games rely on a combination of all three.
            // 
            // Depth buffering won't work in our case because it can only be used with opaque objects.  That is objects that are completely solid.
            // Since our sprites have transparency (alpha blending) we can't use this mode.  Almost all the websites I looked up said just set the
            // depth buffer stencil and things would draw correctly, but if you have transparent textures the XNA rendering engine ignores the
            // transparency and just fills it with the default color.
            //
            // Backface culling only works with curved objects or more solid objects.  Since our sprites are 2D quads that exist in 3D they don't 
            // have any substance.
            //
            // The only other option is to make our algoritm paint these objects in the order from back to front.  Since lower hex cells are in the 
            // back we just draw them in row, column order to get the correct depth (z-order).
            //
            // See http://blogs.msdn.com/b/shawnhar/archive/2009/02/18/depth-sorting-alpha-blended-objects.aspx  for details about this stuff
            //
            // TO DO:  Make a sorting algorithm when the game entities are first put on the board or are moved so we don't have to go through
            //         ever hex cell.

            for (int i = 0; i < this.hexBoardHeight; i++)
            {
                for (int j = 0; j < this.hexBoardWidth; j++)
                {
                    Hex hex = getHex(i, j);
                    GameEntity inactiveEntity = hex.InactiveEntity;
                    if (inactiveEntity != null)
                    {
                        inactiveEntity.MyView = combatScreen.MyView;
                        inactiveEntity.Draw(gameTime);
                    }

                    GameEntity gameEntity = hex.MyGameEntity;
                    if (gameEntity != null)
                    {
                        gameEntity.MyView = combatScreen.MyView;
                        gameEntity.Draw(gameTime);
                    }
                }
            }

            /*
            foreach (GameEntity gameEntity in gameEntities.MyGameEntities)
            {
                gameEntity.setViewMatrix(combatScreen.MyView);
                gameEntity.Draw(gameTime);
            }
            * */

            if (combatAttack != null)
            {
                combatAttack.MyView = combatScreen.MyView;
                combatAttack.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public void DrawBoard()
        {
            this.hexDraw.Draw(grid);  // Draws the hex to the grid only.  The grid needs to still be drawn (handled above)
        }

        private void drawGridNumbers()
        {
            //the SpriteBatch added below to draw the current technique name
            //is changing some needed render states, so they are reset here.
            //lhg.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            SpriteBatch spriteBatch = lhg.MySpriteBatch;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
 
            int width = getBoardWidth();
            int height = getBoardHeight();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Vector3 center3 = getCellCenter(i, j);
                    Vector2 center2 = combatScreen.convertWorldToScreenVector2(center3);
                    center2.X -= 8;
                    center2.Y -= 8;
                    String coord = i.ToString() + "," + j.ToString();
                    spriteBatch.DrawString(lhg.SmallFont, coord, center2, Color.PowderBlue);
                }
            }

            spriteBatch.End();
        }

        public bool selectCell(Vector3 vector, Color selectColor)
        {
            Hex hex = findHexAt(new Vector2(vector.X, vector.Z));
            if (hex != null)
            {
                LHGCommon.printToConsole("Selected Hex", hex);
                hex.HexState.BackgroundColor = selectColor;
                return true;
            }

            return false;
        }

        public void selectCell(int i, int j, Color selectColor)
        {
            Hex hex = this.hexBoard.getHex(i, j);
            if (hex != null)
            {
                hex.HexState.BackgroundColor = selectColor;
            }
        }


        public void selectMoveCells(int i, int j, int radius)
        {
            selectCells(i, j, radius, moveSelectColor);
        }

        public void selectAttackCells(int i, int j, int radius)
        {
            selectCells(i, j, radius, attackRadiusColor);
        }

        public void selectCells(int i, int j, int radius, Color selectColor)
        {
            Hex hex = hexBoard.getHex(i, j);
            selectCells(hex, radius, selectColor);
        }

        public void selectCells(Point point, int radius, Color selectColor)
        {
            Hex hex = hexBoard.FindHexMouseClick(point);
            selectCells(hex, radius, selectColor);
        }

        public void selectCells(Hex hex, int radius, Color selectColor)
        {            
            if (hex != null)
            {
                activeHexes = hexBoard.getNeighboringCells(hex, radius);
                colorHexes(activeHexes, selectColor);
            }

            // Redraw the hex board
            this.hexDraw.Draw(grid);
        }

        public Hex getHexNextTo(CombatLocation location)
        {
            return getHexNextTo(location.getHex(), location.direction);
        }

        public Hex getHexNextTo(Hex center, AnimationKey direction)
        {
            HexDirection hexDirection;

            switch (direction)
            {
                default:
                case AnimationKey.North:
                    hexDirection = HexDirection.North;
                    break;

                case AnimationKey.NorthEast:
                    hexDirection = HexDirection.Northeast;
                    break;

                case AnimationKey.East:
                    hexDirection = HexDirection.East;
                    break;

                case AnimationKey.SouthEast:
                    hexDirection = HexDirection.Southeast;
                    break;

                case AnimationKey.South:
                    hexDirection = HexDirection.South;
                    break;

                case AnimationKey.SouthWest:
                    hexDirection = HexDirection.Southwest;
                    break;

                case AnimationKey.West:
                    hexDirection = HexDirection.West;
                    break;

                case AnimationKey.NorthWest:
                    hexDirection = HexDirection.Northwest;
                    break;
            }

            return getHexNextTo(center, hexDirection);
        }

        public Hex getHexNextTo(Hex center, HexDirection direction)
        {
            return this.hexBoard.getHexNextTo(center, direction);
        }

        public Hex findActiveHex(Point point)
        {
            Hex hex = findHexAt(point);
            if (hex != null && activeHexes.Contains(hex))
                return hex;

            return null;
        }

        public bool isActiveHex(Hex hex)
        {
            if (hex != null && activeHexes.Contains(hex))
                return true;

            return false;
        }

        public List<Hex> getHexesInRadius(Hex hex, int radius)
        {
            return null;
        }

        public List<Hex> getHexesInTriangle(Hex hex, int numRows)
        {
            return null;
        }

        public void resetCells()
        {
            if (activeHexes.Count > 0)
            {
                colorHexes(activeHexes, backgroundColor);
                activeHexes.Clear();

                // Redraw the hex board
                this.hexDraw.Draw(grid);
            }
        }

        /*
        public List<Hex> highlightPath(int i, int j, Point point)
        {

            Hex startHex = hexBoard.getHex(i, j);
            Hex endHex = hexBoard.FindHexMouseClick(point.X, point.Y);
            if (endHex != null)
            {
                // Only show a path on a hex that the user can actually move to
                if (hexBoard.BoardState.isCellSelected(endHex))
                {
                    colorHighlightedHexes(this.moveRadiusColor);
                    highlightedHexes.Clear();
                    highlightedHexes = findPath(startHex, endHex);
                    highlightedHexes.Reverse();
                    colorHighlightedHexes(this.moveSelectColor);
                }
            }          

            return null; // this.highlightedHexes;
        }
         */


        public void highlightPath(List<Hex> path)
        {
            colorHexes(path, Color.White);

            // Redraw the hex board
            this.hexDraw.Draw(grid);
        }

        public void unhighlightPath(List<Hex> path)
        {
            colorHexes(path, Color.Green);

            // Redraw the hex board
            this.hexDraw.Draw(grid);
        }

        public void colorHexes(List<Hex> hexList, Color color)
        {
            foreach (Hex hex in hexList)
            {
                hex.HexState.BackgroundColor = color;
            }
        }

        public Hex findHexAt(Vector2 vector2)
        {
            Vector3 vector3 = combatScreen.convertScreenToWorld((int)vector2.X, (int)vector2.Y);
            return hexBoard.FindHexVector2(new Vector2(vector3.X, vector3.Z));
        }

        public Hex findHexAt(Vector3 vector3)
        {
            return hexBoard.FindHexVector2(new Vector2(vector3.X, vector3.Z));
        }

        public Hex findHexAt(Point point)
        {
            Vector3 vector = combatScreen.convertScreenToWorld(point.X, point.Y);
            return hexBoard.FindHexVector2(new Vector2(vector.X, vector.Z));
        }

        public int movePlayer(Player player, int i, int j, AnimationCallback callback)
        {
            CombatLocation endLocation = new CombatLocation(player.Location.board, i, j, player.Location.direction);
            return movePlayer(player, endLocation, callback);
        }

        public int movePlayer(Player player, CombatLocation endLocation, AnimationCallback callback)
        {
            int numHexesMoved = 0;

            CombatLocation startLocation = player.Location;
            HexBoard hexBoard = startLocation.board.MyHexBoard;
            Hex startHex = hexBoard.getHex(startLocation.i, startLocation.j);
            if (startHex != null)
            {
                Hex endHex = hexBoard.getHex(endLocation.i, endLocation.j);
                if (endHex != null)
                {
                    // Get the hexes that are along the way to the path the user clicked on.
                    List<Hex> hexPath = startLocation.board.findPath(startHex, endHex);
                    movePlayer(player, hexPath, callback);
                    numHexesMoved = hexPath.Count;
                }
            }

            return numHexesMoved;
        }

        public int movePlayer(Player player, CombatLocation endLocation, int numCellsToMove, AnimationCallback callback)
        {
            CombatLocation startLocation = player.Location;
            Hex startHex = hexBoard.getHex(startLocation.i, startLocation.j);
            if (startHex != null)
            {
                Hex endHex = hexBoard.getHex(endLocation.i, endLocation.j);
                if (endHex != null)
                {
                    // Only move the player the number of action points they have
                    return movePlayer(player, startHex, endHex, numCellsToMove, callback);
                }
            }

            return 0;
        }

        public int movePlayer(Player player, List<Hex> hexPath, AnimationCallback callback)
        {
            int numHexesMoved = 0;

            if (hexPath.Count > 0)
            {
                // Get the center point of each hex
                List<HexPointF> centerPoints = new List<HexPointF>();
                foreach (Hex hex in hexPath)
                {
                    HexPointF centerPoint = hex.getCenter();
                    centerPoints.Add(centerPoint);
                }

                List<Vector3> vectorPath = new List<Vector3>();

                HexPointF[] centerArray = centerPoints.ToArray();
                int maxPoints = centerArray.Length;
                HexPointF point1, point2;
                for (int index = 0; index <= maxPoints - 2; index++)
                {
                    point1 = centerArray[index];
                    point2 = centerArray[index + 1];
                    vectorPath.AddRange(findPointsAlongLine3D(point1, point2, player.WalkingSpeed));
                }

                player.walk(vectorPath, callback);
            }

            return hexPath.Count;
        }

        public int movePlayer(Player player, Point mousePoint, AnimationCallback callback)
        {
            Hex endHex = player.MyBoard.findHexAt(mousePoint);
            if (endHex != null)
            {
                // Make sure the hex that the user clicked on is an allowable move

                //if (hexBoard.BoardState.isCellSelected(endHex))
                {
                    CombatLocation endLocation = new CombatLocation(player.Location.board, endHex.I, endHex.J, player.Location.direction);
                    return movePlayer(player, endLocation, callback);
                }
            }

            return 0;
        }

        public int movePlayer(Player player, Hex startHex, Hex endHex, int moveCount, AnimationCallback callback )
        {
            // Get the hexes that are along the way to the path the user clicked on.
            if (moveCount > 0)
            {
                List<Hex> hexPath = player.MyBoard.findPath(startHex, endHex);
                if (hexPath.Count() >= moveCount)
                {
                    hexPath.RemoveRange(moveCount, hexPath.Count - moveCount);
                }

                return movePlayer(player, hexPath, callback);
            }

            return 0;
        }

        public List<Hex> getHexPath(Player player, Point point)
        {
            List<Hex> hexPath = null;

            CombatLocation startLocation = player.Location;
            HexBoard hexBoard = startLocation.board.MyHexBoard;
            Hex startHex = hexBoard.getHex(startLocation.i, startLocation.j);
            Hex endHex = hexBoard.FindHexMouseClick(point);
            if (startHex != null && endHex != null)
            {
                hexPath = startLocation.board.findPath(startHex, endHex);
            }

            return hexPath;
        }

        public List<Hex> findPath(Hex start, Hex end)
        {
            PathFinder pathfinder = new PathFinder(lhg, this.hexBoard);
            bool validPath = pathfinder.findPath(start, end);
            return pathfinder.Path;
        }

        public Point[] PointsAlongLine(Point start, Point end, int spacing)
        {
            int xDifference = end.X - start.X;
            int yDifference = end.Y - start.Y;
            int absoluteXdifference = Math.Abs(start.X - end.X);
            int absoluteYdifference = Math.Abs(start.Y - end.Y);

            int lineLength = (int)Math.Sqrt((Math.Pow(absoluteXdifference, 2) + Math.Pow(absoluteYdifference, 2))); //pythagoras
            int steps = lineLength / spacing;
            int xStep = xDifference / steps;
            int yStep = yDifference / steps;

            Point[] result = new Point[steps];

            for (int i = 0; i < steps; i++)
            {
                int x = start.X + (xStep * i);
                int y = start.Y + (yStep * i);
                result[i] = new Point(x, y);
            }

            return result;
        }

        public List<Vector2> findPointsAlongLine2D(GameEntity startEntity, GameEntity endEntity, float spacing)
        {
            Hex startHex = getHex(startEntity);
            if (startHex != null)
            {
                Hex endHex = getHex(endEntity);
                if (endHex != null)
                {
                    HexPointF startPointF = startHex.getCenter();
                    HexPointF endPointF = endHex.getCenter();
                    return findPointsAlongLine2D(startPointF, endPointF, spacing);
                }
            }

            return null;
        }

        public List<Vector2> findPointsAlongLine2D(HexPointF start, HexPointF end, float spacing)
        {
            List<Vector2> points = new List<Vector2>();

            float xDifference = end.X - start.X;
            float yDifference = end.Y - start.Y;
            float absoluteXdifference = Math.Abs(start.X - end.X);
            float absoluteYdifference = Math.Abs(start.Y - end.Y);

            float lineLength = (float)Math.Sqrt((Math.Pow(absoluteXdifference, 2) + Math.Pow(absoluteYdifference, 2))); //pythagoras
            float steps = lineLength / spacing;
            float xStep = xDifference / steps;
            float yStep = yDifference / steps;

            for (int i = 0; i < steps; i++)
            {
                float x = start.X + (xStep * i);
                float y = start.Y + (yStep * i);
                points.Add( new Vector2(x, y) );
            }

            return points;
        }

        public Hex getHex(GameEntity gameEntity)
        {
            if ( gameEntity.MyEntityType == GameEntity.EntityType.Player )
            {
                Player player = (Player)gameEntity;
                return player.Location.getHex();
            }
            else if ( gameEntity.MyEntityType == GameEntity.EntityType.Obstacle )
            {
                Obstacle obstacle = (Obstacle)gameEntity;
                return obstacle.Location.getHex();
            }

            return null;
        }

        public List<Vector3> findPointsAlongLine3D(GameEntity startEntity, GameEntity endEntity, float spacing)
        {
            Hex startHex = getHex(startEntity);
            if (startHex != null)
            {
                Hex endHex = getHex(endEntity);
                if (endHex != null)
                {
                    HexPointF startPointF = startHex.getCenter();
                    HexPointF endPointF = endHex.getCenter();
                    return findPointsAlongLine3D(startPointF, endPointF, spacing);
                }
            }

            return null;
        }

        public List<Vector3> findPointsAlongLine3D(HexPointF start, HexPointF end, float spacing)
        {
            List<Vector3> points = new List<Vector3>();

            float xDifference = end.X - start.X;
            float yDifference = end.Y - start.Y;
            float absoluteXdifference = Math.Abs(start.X - end.X);
            float absoluteYdifference = Math.Abs(start.Y - end.Y);

            float lineLength = (float)Math.Sqrt((Math.Pow(absoluteXdifference, 2) + Math.Pow(absoluteYdifference, 2))); //pythagoras
            float steps = lineLength / spacing;
            float xStep = xDifference / steps;
            float yStep = yDifference / steps;

            for (int i = 0; i < steps; i++)
            {
                float x = start.X + (xStep * i);
                float y = start.Y + (yStep * i);
                points.Add(new Vector3(x, 0, y));
            }

            return points;
        }

        /*
        public Vector2 getCellCenter(Hex hex)
        {
            HexPointF hexPointF = hex.getCenter();

            if ( primitiveBatch != null )
                return new Vector2(hexPointF.X, hexPointF.Y);  // 2D coordinates map without a problem

            // Else we have 3D coordinates and we have to map them to 2D
            Vector3 centerVector3 = new Vector3(hexPointF.X, 0.0f, hexPointF.Y);
            view = camera.ViewMatrix;
            grid.ViewMatrix = camera.ViewMatrix;
            Point centerPoint = convertWorldToScreenPoint(centerVector3);
            return new Vector2(centerPoint.X, centerPoint.Y);
        }
        */

        public Vector2 getHexCenter(int i, int j)
        {
            Vector2 position = Vector2.Zero;

            Hex hex = getHex(i, j);
            if (hex != null)
            {
                HexPointF hexPointF = hex.getCenter();
                position = new Vector2(hexPointF.X, hexPointF.Y);
            }

            return position;
        }

        public Vector3 getCellCenter(Hex hex)
        {
            HexPointF hexPointF = hex.getCenter();
            return new Vector3(hexPointF.X, 0.0f, hexPointF.Y);
        }

        public Vector3 getCellCenter(int i, int j)
        {
            Hex hex = this.hexBoard.getHex(i, j);
            if (hex != null)
                return getCellCenter(hex);

            return Vector3.Zero;
        }

        public Hex getHex(int i, int j)
        {
            return this.hexBoard.getHex(i, j);
        }

        public int getCellSize()
        {
            return this.hexSize;
        }

        public HexBoard MyHexBoard
        {
            get { return this.hexBoard; }
        }

        public float Scale
        {
            get
            {
                float cameraDistance = combatScreen.MyCamera.Distance;
                float scale = 2000.0f / cameraDistance;
                return scale;
            }
        }

        public Vector3 putGameEntityOnCell(GameEntity gameEntity, int i, int j)
        {
            Hex hex = this.hexBoard.getHex(i, j);
            if (hex != null)
            {
                hex.MyGameEntity = gameEntity;
                return getCellCenter(hex);
            }

            return Vector3.Zero;
        }

   
        public bool IsPointOnBoard(Point point)
        {
            Hex hex = findHexAt(point);
            return (hex != null);
        }

        public bool canScrollBoardLeft()
        {
            return true;

            Point point = combatScreen.convertWorldToScreenPoint(extents.bottomLeft);
            if (point.X > 0)
                return false;

            return true;
        }

        public bool canScrollBoardRight()
        {
            return true;

            Point point = combatScreen.convertWorldToScreenPoint(extents.bottomRight);
            if (point.X < Game.Window.ClientBounds.Width)
                return false;

            return true;
        }

        public bool canScrollBoardUp()
        {
            return true;

            Point point = combatScreen.convertWorldToScreenPoint(extents.topRight);
            if (point.Y > (Game.Window.ClientBounds.Height / 2))
                return false;

            return true;
        }

        public bool canScrollBoardDown()
        {
            Point point = combatScreen.convertWorldToScreenPoint(extents.bottomRight);
            if (point.Y < Game.Window.ClientBounds.Height)
                return false;

            return true;
        }

        public Rectangle3D getHexExtents(Hex hex)
        {
            Vector3 topLeft;
            Vector3 topRight;
            Vector3 bottomLeft;
            Vector3 bottomRight;
            
            HexPointF[] points = hex.Points;
            if (orientation == HexOrientation.Flat)
            {
                topLeft = new Vector3(points[5].X, 0f, points[0].Y);
                topRight = new Vector3(points[2].X, 0f, points[1].Y);
                bottomLeft = new Vector3(points[5].X, 0f, points[4].Y);
                bottomRight = new Vector3(points[2].X, 0f, points[3].Y);
            }
            else
            {
                topLeft = new Vector3(points[5].X, 0f, points[0].Y);
                topRight = new Vector3(points[1].X, 0f, points[0].Y);
                bottomLeft = new Vector3(points[4].X, 0f, points[3].Y);
                bottomRight = new Vector3(points[2].X, 0f, points[3].Y);
            }

            return new Rectangle3D(topLeft, topRight, bottomLeft, bottomRight);
        }

        public Rectangle3D get3DExtents()
        {
            int width = hexBoard.Width;
            int height = hexBoard.Height;

            Hex topLeftHex = hexBoard.Hexes[0, 0];
            Hex topRightHex = hexBoard.Hexes[0, width - 1];
            Hex bottomLeftHex = hexBoard.Hexes[height - 1, 0];
            Hex bottomRightHex = hexBoard.Hexes[height - 1, width - 1];

            //topLeftHex.HexState.BackgroundColor = Color.Turquoise;
            //topRightHex.HexState.BackgroundColor = Color.HotPink;
            //bottomLeftHex.HexState.BackgroundColor = Color.Purple;
            //bottomRightHex.HexState.BackgroundColor = Color.White;

            Vector3 topLeft;
            Vector3 topRight;
            Vector3 bottomLeft;
            Vector3 bottomRight;

            if (orientation == HexOrientation.Flat)
            {
                topLeft = new Vector3(topLeftHex.Points[5].X, 0, topLeftHex.Points[0].Y);
                topRight = new Vector3(topRightHex.Points[2].X, 0, topRightHex.Points[1].Y);
                bottomLeft = new Vector3(bottomLeftHex.Points[5].X, 0, bottomLeftHex.Points[4].Y);
                bottomRight = new Vector3(bottomRightHex.Points[2].X, 0, bottomRightHex.Points[3].Y);

                return new Rectangle3D(topLeft, topRight, bottomLeft, bottomRight);
            }
            else
            {
                topLeft = new Vector3(topLeftHex.Points[5].X, 0, topLeftHex.Points[0].Y);
                topRight = new Vector3(topRightHex.Points[1].X, 0, topRightHex.Points[0].Y);
                bottomLeft = new Vector3(bottomLeftHex.Points[4].X, 0, bottomLeftHex.Points[4].Y);
                bottomRight = new Vector3(bottomRightHex.Points[2].X, 0, bottomRightHex.Points[3].Y);

                return new Rectangle3D(topLeft, topRight, bottomLeft, bottomRight);
            }
        }

        public Vector2 getTopLeft()
        {
            Hex topLeftHex = hexBoard.Hexes[0, 0];

            Vector3 topLeft = new Vector3(topLeftHex.Points[5].X, 0, topLeftHex.Points[0].Y);
            return combatScreen.convertWorldToScreenVector2(topLeft);
        }

        public Vector2 getBottomRight()
        {
            int width = hexBoard.Width;
            int height = hexBoard.Height;

            Hex bottomRightHex = hexBoard.Hexes[height - 1, width - 1];

            Vector3 bottomRight = new Vector3(bottomRightHex.Points[2].X, 0, bottomRightHex.Points[3].Y);
            return combatScreen.convertWorldToScreenVector2(bottomRight);
        }

        public Vector2 getBottomLeft()
        {
            int width = hexBoard.Width;
            int height = hexBoard.Height;

            Hex bottomLeftHex = hexBoard.Hexes[height - 1, 0];

            Vector3 bottomLeft = new Vector3(bottomLeftHex.Points[5].X, 0, bottomLeftHex.Points[4].Y);
            return combatScreen.convertWorldToScreenVector2(bottomLeft);
        }

        public Vector2 getCenter()
        {
            int width = hexBoard.Width / 2;
            int height = hexBoard.Height;

            Hex centerHex = hexBoard.Hexes[0, width];

            Vector3 center = new Vector3(centerHex.Points[5].X, 0, centerHex.Points[4].Y);
            return combatScreen.convertWorldToScreenVector2(center);

        }

        public GameEntity findGameEntityAtPoint(Point point)
        {
            if (IsPointOnBoard(point))
            {
                return this.gameEntities.findGameEntityAtPoint(point);
            }

            return null;
        }
    }
}
