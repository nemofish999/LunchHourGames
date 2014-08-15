using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using LunchHourGames.Players;
using LunchHourGames.Obstacles;

namespace LunchHourGames.Combat
{
    public class GameEntities
    {
        private List<GameEntity> gameEntities = new List<GameEntity>();

        private List<Player> players = new List<Player>();
        private List<Obstacle> obstacles = new List<Obstacle>();

        public void addPlayer(Player player)
        {
            players.Add(player);
            gameEntities.Add(player);
        }

        public void removePlayer(Player player)
        {
            players.Remove(player);
            gameEntities.Remove(player);
        }

        public List<Player> MyPlayers
        {
            get { return this.players; }
        }

        public void addObstacle(Obstacle obstacle)
        {
            obstacles.Add(obstacle);
            gameEntities.Add(obstacle);
        }

        public void removeObstacle(Obstacle obstacle)
        {
            obstacles.Remove(obstacle);
            gameEntities.Remove(obstacle);
        }

        public List<Obstacle> MyObstacles
        {
            get { return this.obstacles; }
        }

        public void Add(GameEntity gameEntity)
        {
            gameEntities.Add(gameEntity);

            if (gameEntity.MyEntityType == GameEntity.EntityType.Player)
                addPlayer((Player)gameEntity);
            else if (gameEntity.MyEntityType == GameEntity.EntityType.Obstacle)
                addObstacle((Obstacle)gameEntity);
        }

        public void Remove(GameEntity gameEntity)
        {
            if (gameEntity.MyEntityType == GameEntity.EntityType.Player)
                removePlayer((Player)gameEntity);
            else if (gameEntity.MyEntityType == GameEntity.EntityType.Obstacle)
                removeObstacle((Obstacle)gameEntity);
        }

        public List<GameEntity> MyGameEntities
        {
            get { return this.gameEntities; }
        }

        public void clearAll()
        {
            players.Clear();
            obstacles.Clear();
        }

        public void setGraphicsMatrices(Matrix view, Matrix projection, Matrix world)
        {
            foreach (GameEntity gameEntity in gameEntities)
            {
                gameEntity.setGraphicsMatrices(view, projection, world);
            }
        }

        public Player moveToHumanControlledPlayer()
        {
            foreach (Player player in MyPlayers)
            {
                if (player.MyType == Player.Type.Human)
                    return player;
            }

            return null;
        }

        public List<Player> getPlayer(Player.Type type)
        {
            List<Player> typesOfPlayers = new List<Player>();

            foreach (Player player in MyPlayers)
            {
                if (player.MyType == type)
                    typesOfPlayers.Add(player);
            }

            return typesOfPlayers;
        }

        public Player getPlayer(int id)
        {
            foreach (Player player in MyPlayers)
            {
                if (player.ID == id)
                    return player;
            }

            return null;
        }

        public Player findPlayerAtPoint(Point point)
        {
            foreach (Player player in MyPlayers)
            {
                Rectangle playerRect = player.getCurrentRectangle();
                if (playerRect.Contains(point))
                    return player;
            }

            return null;
        }

        public GameEntity findGameEntityAtPoint(Point point)
        {
            foreach (GameEntity gameEntity in MyGameEntities)
            {
                Rectangle extents = gameEntity.getExtents();
                if (extents.Contains(point))
                    return gameEntity;
            }

            return null;
        }

        public bool areAnyPlayersMoving()
        {
            foreach (Player player in MyPlayers)
            {
                if (player.IsMoving)
                    return true;
            }

            return false;
        }
    }
}
