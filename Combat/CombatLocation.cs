using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using LunchHourGames.Sprite;
using LunchHourGames.Players;
using LunchHourGames.Hexagonal;
using LunchHourGames.Screen;
using LunchHourGames.Drawing;

namespace LunchHourGames.Combat
{
    public class CombatLocation
    {
        public CombatBoard board;
        public int i, j;
        public AnimationKey direction = AnimationKey.South;
        public Vector3 position  = Vector3.Zero;

        public CombatScreen screen;  // The Combat screen that is holding our board

        public CombatLocation(CombatBoard board, int i, int j)
        {
            this.board = board;
            this.i = i;
            this.j = j;
            this.screen = board.MyCombatScreen;
        }

        public CombatLocation(CombatBoard board, int i, int j, AnimationKey direction)
            :this(board, i, j)
        {
            this.direction = direction;
        }

        public CombatLocation(CombatBoard board, int i, int j, AnimationKey direction, Vector3 position)
            :this(board, i, j, direction)
        {
            this.position = position;
        }

        public Hex getHex()
        {
            return board.getHex(i, j);
        }
    }
}
