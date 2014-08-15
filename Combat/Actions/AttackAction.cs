using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using LunchHourGames.Combat.Attacks;
using LunchHourGames.Players;
using LunchHourGames.Hexagonal;
using LunchHourGames.Inventory;
using LunchHourGames.Inventory.Weapons;

namespace LunchHourGames.Combat.Actions
{
    public class AttackAction : CombatAction
    {
        private MouseState mouseNoState;
        private MouseState mouseStatePrevious;        // Last mouse pointer position and state

        public AttackAction(LunchHourGames lhg, CombatSystem combatSystem, Handler combatActionHandler)
            : base(lhg, combatSystem, combatActionHandler, ActionType.Attack)
        {
        }

        public override void Start()
        {
            if (allowUserInteraction())
            {
                // Current player is under user control, so highlight the cells the player can move.
                CombatLocation location = CurrentPlayer.Location;
                int radius = CurrentPlayer.MyAttributes.actionPoints;

                MyBoard.selectAttackCells(location.i, location.j, radius);
            }

            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.actionHasFinished)
                return;

            MouseState mouseStateCurrent = Mouse.GetState();

            if (MyBoard.MyCombatAttack != null)
            {
                // We have an attack running.  The update and draw methods for this attack will be handled by the board itself.
                // We just need to check to see if the attack sequence has finished.
                if (MyBoard.MyCombatAttack.HasCompleted)
                {
                    actionHasFinished = true;
                    MyBoard.MyCombatAttack = null;                    
                    combatActionHandler.handleActionComplete(actionType);
                }
            }
            else
            {
                if (!allowUserInteraction())
                {
                    // Current player is under computer control.
                    if (CurrentPlayer.MyType == Player.Type.Zombie)
                        handleZombieAttack();
                }
                else
                {
                    // Current player is under user control.  Wait for a mouse click
                    if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
                    {
                        Point mousePoint = new Point(mouseStateCurrent.X, mouseStateCurrent.Y);
                        handleHumanAttack(mousePoint);
                    }
                }
            }

            mouseStatePrevious = mouseStateCurrent;
        } 

        private void handleZombieAttack()
        {
            Zombie zombie = (Zombie)CurrentPlayer;

            // Get the human to attack
            Human human = zombie.getHumanToAttack();
            if (human != null)
            {
                ZombieAttack zombieAttack = zombie.getCombatAttack(this.combatSystem);
                zombieAttack.TargetGameEntity = human;
                zombieAttack.AttackerGameEntity = zombie;
                MyBoard.MyCombatAttack = zombieAttack;
                
                actionIsTriggered = true;
            }

            if (actionIsTriggered)
            {
                actionHasFinished = false;

                // Initialize or reinitialize
                MyBoard.MyCombatAttack.Initialize();

                // Subtract the weapon range from the player's action points
                CurrentPlayer.subtractActionPointsBy(1);
            }
        }

        private void handleHumanAttack(Point mousePoint)
        {
            Hex targetHex = getTargetHex(mousePoint);
            if (targetHex != null)
            {
                GameEntity gameEntity = targetHex.MyGameEntity;
                Weapon weapon = CurrentPlayer.MyWeapon;
                MyBoard.MyCombatAttack = weapon.getCombatAttack(this.combatSystem);
                if (gameEntity == null)
                {
                    // Nothing there to attack.  Check to see if the weapon has spread damage.
                    if (weapon.MyAttackType == Weapon.AttackType.Spread)
                    {
                        MyBoard.MyCombatAttack.TargetHex = targetHex;
                        actionIsTriggered = true;
                    }
                }
                else
                {
                    // Something is there to attack.  Make sure it can be attacked.
                    if (gameEntity.canMainPlayerAttack(CurrentPlayer.MyWeapon))
                    {
                        MyBoard.MyCombatAttack.TargetGameEntity = gameEntity;
                        actionIsTriggered = true;
                    }
                }

                if (actionIsTriggered)
                {
                    actionHasFinished = false;

                    // Initialize or reinitialize
                    MyBoard.MyCombatAttack.Initialize();

                    // Face the player toward the direction they are attacking
                    CurrentPlayer.face(targetHex);

                    // Subtract the weapon range from the player's action points
                    CurrentPlayer.subtractActionPointsBy(weapon.ActionPointCost);
                }
            }
        }

        private Hex getTargetHex(Point point)
        {
            Hex targetHex = null;

            // Generally the target will be a game entity instead of a blank hex.
            // Check to see if the point hits a game entity first
            GameEntity targetGameEntity = MyBoard.findGameEntityAtPoint(point);
            if (targetGameEntity != null)
            {
                // The point hits a game entity. 
                // First determine if this game entity can be attacked.
                if (targetGameEntity.MyEntityType == GameEntity.EntityType.Player && ((Player)targetGameEntity).IsActive)
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
    }
}
