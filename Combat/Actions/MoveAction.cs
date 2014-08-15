using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using LunchHourGames.Players;
using LunchHourGames.Hexagonal;
using LunchHourGames.Sprite;

namespace LunchHourGames.Combat.Actions
{
    public class MoveAction : CombatAction, AnimationCallback
    {
        private MouseState mouseNoState;
        private MouseState mouseStatePrevious;        // Last mouse pointer position and state

        public MoveAction(LunchHourGames lhg, CombatSystem combatSystem, Handler combatActionHandler)
            :base(lhg, combatSystem, combatActionHandler, ActionType.Move)
        {
        }

        public override void Start()
        {
            if (allowUserInteraction())
            {
                // Current player is under user control, so highlight the cells the player can move.
                CombatLocation location = CurrentPlayer.Location;
                int radius = CurrentPlayer.MyAttributes.actionPoints;

                MyBoard.selectMoveCells(location.i, location.j, radius);
            }

            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            if ( this.actionHasFinished )
                return;
            
            MouseState mouseStateCurrent = Mouse.GetState();
            if (actionIsTriggered)
            {
                if (!CurrentPlayer.IsMoving)
                {
                    // Player has stopped moving, so this action is complete.
                    combatActionHandler.handleActionComplete(actionType);
                    this.actionHasFinished = true;
                }
                else
                {
                    // Player is moving, so there isn't anything we need to do
                    return;
                }
            }
            else
            {
                if (!allowUserInteraction())
                {
                    // Current player is under computer control.
                    if (CurrentPlayer.MyType == Player.Type.Zombie)
                    {
                        Zombie zombie = (Zombie)CurrentPlayer;
                        int numHexesMoved = zombie.handleCombatChooseMove(this);
                        zombie.subtractActionPointsBy(numHexesMoved);
                        actionIsTriggered = true;
                    }
                }
                else
                {
                    // Current player is under user control.  Wait for a mouse click
                    // Show the path the user may want to take
                    // CombatLocation location = currentPlayer.Location;
                    //List<Hex> hexPath = this.board.highlightPath(location.i, location.j, mousePoint);

                    if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                    {
                        Point mousePoint = new Point(mouseStateCurrent.X, mouseStateCurrent.Y);
                        Hex targetHex = getTargetHex(mousePoint);
                        if (targetHex != null )
                        {
                            GameEntity gameEntity = targetHex.MyGameEntity;
                            if (gameEntity != null)
                                actionIsTriggered = true;
                            else
                                actionIsTriggered = true;

                            if (actionIsTriggered)
                            {
                                int numHexesMoved = MyBoard.movePlayer(CurrentPlayer, mousePoint, this);

                                // Subtract the number spaces moved from the player's action points
                                CurrentPlayer.subtractActionPointsBy(numHexesMoved);
                            }
                        }
                    }
                }
            }

            mouseStatePrevious = mouseStateCurrent;
        }

        private Hex getTargetHex(Point point)
        {
            Hex targetHex = null;

            // Check to see if the point hits a game entity first
            GameEntity targetGameEntity = MyBoard.findGameEntityAtPoint(point);
            if (targetGameEntity != null)
            {
                // The point hits a game entity. 
                // First determine if the player can walk over the game entity.
                // If the player has died, we will allow the player to walk over the player
                if (targetGameEntity.MyEntityType == GameEntity.EntityType.Player && !((Player)targetGameEntity).IsActive)
                {
                    // Find out what hex it is own and return that as the target
                    targetHex = MyBoard.getHex(targetGameEntity);
                    if (!MyBoard.isActiveHex(targetHex))
                        targetHex = null;

                }
            }
            else
            {
                // The point doesn't hit a game entity, so find the hex the user clicked on
                if (isPointOnBoard(point))
                    targetHex = findActiveHex(point);
            }

            return targetHex;
        }


        public override void Stop()
        {
            MyBoard.resetCells();
            mouseStatePrevious = mouseNoState;

            base.Stop();
        }


        public List<Vector2> getPointsPath(List<Hex> hexPath)
        {
            /*
            // Get the center point of each hex
            List<HexPointF> centerPoints = new List<HexPointF>();
            foreach (Hex hex in hexPath)
            {
                HexPointF centerPoint = hex.getCenter();
                centerPoints.Add(centerPoint);
            }
            */

            List<Vector2> vectorPath = new List<Vector2>();

            /*
            HexPointF[] centerArray = centerPoints.ToArray();
            int maxPoints = centerArray.Length;
            HexPointF point1, point2;
            for (int index = 0; index <= maxPoints - 2; index++)
            {
                point1 = centerArray[index];
                point2 = centerArray[index + 1];
                vectorPath.AddRange(board.findPointsAlongLine(point1, point2, 0.5f));
            }
            */

            return vectorPath;
        }

        public List<Vector2> getPointsPath(Hex a, Hex b, float spacing)
        {
            //List <Vector2> vectorPath =  this.board.findPointsAlongLine(a.getCenter(), b.getCenter(), spacing);
            //return vectorPath;

            return null;
        }

        public List<Vector2> getPointsPath(Player player, Point point)
        {
            /*
            List<Hex> hexPath = getHexPath(player, point);
            if (hexPath != null)
            {
                return getPointsPath(hexPath);
            }
             */ 

            return null;
        }

        public void animationBegin(GameEntity gameEntity, AnimationType type)
        {
        }

        public void animationEnd(GameEntity gameEntity, AnimationType type)
        {
        }
    }
}
