using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LunchHourGames.Players;

namespace LunchHourGames.Hexagonal
{
    // This algorithm was adapted from an article published on http://www.policyalmanac.org/games/aStarTutorial.htm.
    // All code is original and the algorithm modified for a hexagonal grid
    class PathFinder
    {
        private LunchHourGames lhg;

        List<Hex> closedList = new List<Hex>();
        List<Hex> openList = new List<Hex>();
        List<Hex> path = new List<Hex>();

        HexBoard board;

        public static Random random = new Random();

        public PathFinder(LunchHourGames lhg, HexBoard board)
        {
            this.lhg = lhg;
            this.board = board;            
        }

        public void reset()
        {
            board.resetAllHexPaths();
            openList.Clear();
            closedList.Clear();
            path.Clear();
        }

        public List<Hex> Path
        {
            get { return path; }
        }

        public bool findPath(Hex start, Hex end)
        {
            // If start and end are the same there is no path!
            if (start == end)
                return false;

            // Make sure we have valid hexes
            if ( start == null || end == null )
                return false;

            // Clear the board and all paths
            reset();

            // Add starting hex to the open list
            addToOpenList(start);

            // Try to find the path until the target hex (end) is found
            while (closedList.Contains(end) != true)
            {
                bool setFirst = true;
                int lowestF = 0;
                Hex lowestHex = null;

                // Find the lowest F in openlist
                foreach (Hex selectHex in openList)
                {
                    if (setFirst)
                    {
                        lowestF = selectHex.Path.F;
                        setFirst = false;
                    }

                    if (selectHex.Path.F <= lowestF)
                    {
                        lowestF = selectHex.Path.F;
                        lowestHex = selectHex;
                    }
                }

                if (setFirst)
                {
                    // Open list is empty
                    // No possible route can be found
                    return false;
                }

                // Now add the lowest F to the closed list and remove it from the open list
                moveToClosedList(lowestHex);

                // Add the neighbor hexes to the open list
                List<Hex> neighbors = board.getNeighboringCells(lowestHex, 1);
                foreach (Hex nodeHex in neighbors)
                {
                    if (!isAccessible(nodeHex, start))
                    {
                        addToClosedList(nodeHex);
                    }
                    else
                    {
                        if (!closedList.Contains(nodeHex))
                        {
                            // Not in the closed list, so check if its in the open list meaning we already
                            // have processed it's path parameters
                            if (openList.Contains(nodeHex))
                            {
                                // This node is already in our open list.  Check to see if this is a better move
                                if (nodeHex.Path.G < lowestHex.Path.G)
                                {
                                    lowestHex.Path.Parent = nodeHex;
                                    lowestHex.Path.G = nodeHex.Path.G + 10;
                                    lowestHex.Path.F = lowestHex.Path.G + lowestHex.Path.H;
                                }
                            }
                            else
                            {
                                // Not in closed list or open list, so add to open list.
                                addToOpenList(nodeHex);

                                // Set parent
                                nodeHex.Path.Parent = lowestHex;

                                // Update H, G, F
                                int H = board.getHexDistance(nodeHex, end) * 10;
                                int G = lowestHex.Path.G + 10;
                                int F = H + G;
                                nodeHex.Path.update(H, G, F);
                            }
                        }
                    }
                }
            }

            // Get Path
            Hex hex = end;
            do
            {
                path.Add(hex);
                hex = hex.Path.Parent;
            } 
            while (hex != null);

            path.Reverse();

            return ( path.Count() > 0 );
        }

        private bool isAccessible(Hex nodeHex, Hex startHex)
        {
            if (nodeHex == startHex)
                return true;  // We are looking at ourselves, so we're not blocked

            if ( nodeHex.HexState.Blocked )
            {
                // This Hex is blocked by something.  Find out what
                switch ( nodeHex.MyGameEntity.MyEntityType )
                {
                    case GameEntity.EntityType.Player:
                        Player player = (Player)nodeHex.MyGameEntity;
                        if (!player.IsActive)
                            return true;

                        if (player.MyType == Player.Type.Human)
                            return true;
                        else
                            return false;

                    case GameEntity.EntityType.Obstacle:
                    default:
                        return false;
                }
            }

            return true;
        }
      
        /*
         * First Algorithm
         * 
        public List<Hex> getPath( Hex start, Hex end )
        {           
            if (start == end)
            {
                // We have found the end.  Crawl the parent nodes to get the path
                List<Hex> path = new List<Hex>();
                Hex hex = end;
                do
                {
                    path.Add(hex);
                    hex = hex.Parent;
                } while (hex != null);
                
                return path;
            }

            addToClosedList(start);

            // Find the neighbors of start and calculate their F, G, and H scores.
            List<Hex> neighbors = board.getNeighboringCells(start, 1);
            foreach (Hex hex in neighbors)
            {
                if (hex.HexState.Blocked)
                    addToClosedList(hex);  // This hex is blocked, so add it to our closed list
                else if (!closedList.Contains(hex))
                {
                    // If the hex isn't in our closed list, then calculate the F, H, and G scores.
                    int H = board.getHexDistance(hex, end) * 10;
                    int G = 0;

                    switch (hex.HexState.Direction)
                    {
                        case HexDirection.North:
                        case HexDirection.South:
                        case HexDirection.East:
                        case HexDirection.West:
                            G = 10;
                            break;

                        case HexDirection.Northeast:
                        case HexDirection.Southeast:
                        case HexDirection.Northwest:
                        case HexDirection.Southwest:
                            G = 5;
                            break;
                    }

                   
                    if (openList.Contains(hex))
                    {
                        // This Hex is already on the open list from before.  Check to see if using that hex
                        // is better than using our current hex.
                        if (G < hex.HexState.G)
                        {
                            // This path is a better path that the one we are currently on.
                            Hex previousParent = start.Parent;
                            hex.Parent = previousParent;
                            start.Parent = null;
                        }
                    }                  
                    //else
                    {
                        hex.HexState.H = H;
                        hex.HexState.G = G;
                        hex.HexState.F = hex.HexState.G + hex.HexState.H;
                        addToOpenList(hex);
                    }
                }
            }

            // Now, go through the open list and find the hex with the lowest F score
            int lowestF = -1;
            Hex lowestHex = null;
            foreach (Hex hex in openList)
            {
                if (lowestF == -1)
                {
                    lowestF = hex.HexState.F;
                    lowestHex = hex;
                }
                else if (lowestF > hex.HexState.F)
                {
                    lowestF = hex.HexState.F;
                    lowestHex = hex;
                }
            }

            if (lowestHex == null)
            {
                // We have found the end.  Crawl the parent nodes to get the path
                List<Hex> path = new List<Hex>();
                Hex hex = start;
                do
                {
                    path.Add(hex);
                    hex = hex.Parent;
                } while (hex != null);

                return path;
            }

           openList.Remove(lowestHex);
           addToClosedList(lowestHex);           
           lowestHex.Parent = start;
           return getPath(lowestHex, end);           
        }
        */
        /*

        public List<Hex> getPath(Hex start, Hex end)
        {
            List<Hex> path = new List<Hex>();
            if (findPath(start, end))
            {
                Hex hex = start;
                while (hex != null)
                {
                    path.Add(hex);
                    hex = hex.Next;
                }
            }

            return path;
        }
        private bool findPath(Hex start, Hex end)
        {
            if (start == end)
                return true;

            addToClosedList(start);

            // Find the neighbors of start and calculate their F, G, and H scores.
            List<Hex> neighbors = board.getNeighboringCells(start, 1);
            foreach (Hex hex in neighbors)
            {
                if (hex.HexState.Blocked)
                    addToClosedList(hex);  // This hex is blocked, so add it to our closed list
                else if (!closedList.Contains(hex))
                {
                    // If the hex isn't in our closed list, then calculate the F, H, and G scores.
                    int H = board.getHexDistance(hex, end) * 10;
                    int G = 0;

                    switch (hex.HexState.Direction)
                    {
                        case HexDirection.North:
                        case HexDirection.South:
                        case HexDirection.East:
                        case HexDirection.West:
                            G = 10;
                            break;

                        case HexDirection.Northeast:
                        case HexDirection.Southeast:
                        case HexDirection.Northwest:
                        case HexDirection.Southwest:
                            G = 5;
                            break;
                    }

                    if (openList.Contains(hex))
                    {
                        // This Hex is already on the open list from before.  Check to see if using that hex
                        // is better than using our current hex.
                        if (G < hex.HexState.G)
                        {
                            // This path is a better path that the one we are currently on.
                           
                        }
                    }
                    else
                    {
                        hex.HexState.H = H;
                        hex.HexState.G = G;
                        hex.HexState.F = hex.HexState.G + hex.HexState.H;
                        addToOpenList(hex);
                    }
                }
            }

            // Now, go through the open list and find the hex with the lowest F score
            int lowestF = -1;
            List<Hex> lowestHexes = new List<Hex>();
            foreach (Hex hex in openList)
            {
                if (lowestF == -1)
                {
                    // First time through
                    lowestF = hex.HexState.F;
                    lowestHexes.Add(hex);
                }
                else if (lowestF > hex.HexState.F)
                {
                    // We found a F that is lower.  Clear the list
                    lowestF = hex.HexState.F;
                    lowestHexes.Clear();
                    lowestHexes.Add(hex);
                }
                else if (lowestF == hex.HexState.F)
                {
                    // We have more than paths we can take
                    lowestHexes.Add(hex);
                }
            }

            Hex lowestHex = null;
            if (lowestHexes.Count() == 0)
            {
                // We didn't find a path using the starting hex.               
                return false;
            }
            else if (lowestHexes.Count() == 1)
            {
                lowestHex = lowestHexes[0];
            }
            else
            {
                // We have more than one path to take.  Pick one at random
                int indexOfPathToTake = random.Next(0, lowestHexes.Count() - 1);
                lowestHex = lowestHexes[indexOfPathToTake];
            }

            openList.Remove(lowestHex);
            addToClosedList(lowestHex);
            lowestHex.Previous = start;
            start.Next = lowestHex;
            return findPath(lowestHex, end);
        }
        */

        private bool addToClosedList(Hex hex)
        {
            if (hex != null && !closedList.Contains(hex))
            {
                closedList.Add(hex);
                return true;
            }

            return false;
        }

        private bool addToOpenList(Hex hex)
        {
            if (hex != null && !openList.Contains(hex))
            {
                openList.Add(hex);
                return true;
            }

            return false;
        }

        private void moveToClosedList(Hex hex)
        {
            openList.Remove(hex);
            addToClosedList(hex);
        }
    }
}
