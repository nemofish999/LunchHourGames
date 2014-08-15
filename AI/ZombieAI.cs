using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LunchHourGames.AI
{
    public class ZombieAI
    {
        public ZombieAI(LunchHourGames lhg)
        {
        }

        /*


        public void handleZombieTurn(Player zombie)
        {
            // locate the nearest human
            List<Player> humans = getPlayer(Player.Type.Human);
            if (humans.Count() > 0)
            {
                Player human = humans[0];

                CombatLocation zombieLocation = zombie.Location;
                HexBoard hexBoard = zombie.Location.board.MyHexBoard;
                Hex startHex = hexBoard.getHex(zombieLocation.i, zombieLocation.j);
                if (startHex != null)
                {
                    CombatLocation humanLocation = human.Location;
                    Hex endHex = hexBoard.getHex(humanLocation.i, humanLocation.j);
                    if (endHex != null)
                    {
                        // The zombies can only move the number of action points they have
                        if (movePlayer(zombie, startHex, endHex, zombie.MyAttributes.actionPoints))
                        {
                            if (allowZombiesToMoveSimulatiously)
                            {
                                needToMoveZombie = true;
                                initChooseEndTurn();
                            }
                            else
                            {
                                changeCombatStateAndAction(State.PerformingAction, Action.Move);
                            }
                        }
                        else
                        {
                            // Can't move player.  End Turn
                            initChooseEndTurn();
                        }
                    }
                }
            }
        }
        */


    }
}
