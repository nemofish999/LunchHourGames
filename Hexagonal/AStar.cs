using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LunchHourGames.Hexagonal
{
    public class AStar
    {
        // A* path finding algorithm
        private int pathScoreG;  // the movement cost to move from the starting point A to a given hex on the grid.
        private int pathScoreH;  // the estimated movement cose to move from that given square on the grid to the final destination.
        private int pathScoreF;  // The F score 

        private Hex parent;
        private Hex previous;
        private Hex next;

        public AStar()
        {
            this.parent = null;
            this.previous = null;
            this.next = null;
        }

        public int G
        {
            get
            {
                return pathScoreG;
            }
            set
            {
                pathScoreG = value;
            }
        }

        public int H
        {
            get
            {
                return pathScoreH;
            }
            set
            {
                pathScoreH = value;
            }
        }

        public int F
        {
            get
            {
                return pathScoreF;
            }
            set
            {
                pathScoreF = value;
            }
        }

        public Hex Next
        {
            get
            {
                return next;
            }
            set
            {
                next = value;
            }
        }

        public Hex Previous
        {
            get
            {
                return previous;
            }
            set
            {
                previous = value;
            }
        }

        public Hex Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        public void reset()
        {
            next = null;
            previous = null;
            parent = null;
            pathScoreF = 0;
            pathScoreH = 0;
            pathScoreG = 0;
        }

        public void update(int h, int g, int f)
        {
            this.pathScoreH = h;
            this.pathScoreG = g;
            this.pathScoreF = f;
        }
    }
}
