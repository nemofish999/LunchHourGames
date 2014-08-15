using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LunchHourGames;
using LunchHourGames.Drawing;
using LunchHourGames.Players;
using LunchHourGames.Obstacles;
using LunchHourGames.Common;
using LunchHourGames.Screen;
using LunchHourGames.Stage;
using LunchHourGames.Inventory;

namespace LunchHourGames.Combat
{
    public class CombatFactory
    {
        private LunchHourGames lhg;

        private XmlDocument combatDocument;
        
        private PlayerFactory playerFactory;
        private ObstacleFactory obstacleFactory;
        private StageFactory stageFactory;
        private InventoryFactory inventoryFactory;

        private LoadingProgress progress;
        private string combatReferenceName;

        public CombatFactory(LunchHourGames lhg, LoadingProgress progress, PlayerFactory playerFactory, 
                             ObstacleFactory obstacleFactory, StageFactory stageFactory, InventoryFactory inventoryFactory)
        {
            this.lhg = lhg;
            this.progress = progress;
            this.playerFactory = playerFactory;
            this.obstacleFactory = obstacleFactory;
            this.stageFactory = stageFactory;
            this.inventoryFactory = inventoryFactory;
        }

        public void prepareCombatToLoad(string combatReferenceName)        
        {
            this.combatReferenceName = combatReferenceName;
        }

        public LoadingProgress MyLoadingProgress
        {
            set { this.progress = value; }
        }

        public void loadCombat()
        {
            this.progress.updateProgress("Start Loading Combat System", "", 0);
            if (combatDocument == null)
            {
                combatDocument = new XmlDocument();
                combatDocument.Load("Content/Xml/combat.xml");
            }

            lhg.MyCombatSystem = loadCombat(combatDocument, combatReferenceName);

            this.progress.updateProgress("Finished Loading Combat System", "", 100);
            this.progress.loadingComplete();
        }

        public CombatSystem loadCombat(XmlNode combatSystemNode, string combatReferenceName)
        {
            this.progress.updateProgress( "Loading Combat " + combatReferenceName, "Loading Combat System", 10);

            CombatSystem combatSystem = new CombatSystem(lhg);
 
            // Find level and combat
            string xpath = "/combatsystem/combat[@referenceName='" + combatReferenceName + "']";
            XmlNode combatNode = combatSystemNode.SelectSingleNode(xpath);
            if (combatNode != null)
            {   
                XmlNodeList combatChildren = combatNode.ChildNodes;
                foreach (XmlNode childNode in combatChildren)
                {
                    switch (childNode.Name)
                    {
                        case "board":
                            progress.updateProgress("Loading Board Assets", "Loading Board Assets", 20);
                            combatSystem.MyBoard = loadBoard(childNode);                                
                            break;

                        case "screen":
                            progress.updateProgress("Loading Screen Assets", "Loading Screen Assets", 30);
                            combatSystem.MyScreen = loadScreen(childNode);
                            break;

                        case "audio":
                            break;
                    }
                }
            }

            // Now that the combat board has all the components loaded.  Initialilze all the components so the board and screen know how to
            // work with each other.
            // Tell our children about each other and this system
            combatSystem.MyBoard.MyCombatScreen = combatSystem.MyScreen;
            combatSystem.MyBoard.MyCombatSystem = combatSystem;

            combatSystem.MyScreen.MyCombatBoard = combatSystem.MyBoard;
            combatSystem.MyScreen.MyCombatSystem = combatSystem;
            
            foreach (Player player in combatSystem.MyBoard.MyGameEntities.MyPlayers)
            {
                player.Location.screen = combatSystem.MyBoard.MyCombatScreen;
            }

            foreach (Obstacle obstacle in combatSystem.MyBoard.MyGameEntities.MyObstacles)
            {
                obstacle.Location.screen = combatSystem.MyBoard.MyCombatScreen;
            }
           
            progress.updateProgress("Finished Loading Combat System", "Loading", 100);

            return combatSystem;
        }
    
        private CombatBoard loadBoard(XmlNode boardNode)
        {
            CombatBoard board = null;

            if (boardNode != null)
            {   
                XmlNodeList boardChildren = boardNode.ChildNodes;
                foreach (XmlNode childNode in boardChildren)
                {
                    switch (childNode.Name)
                    {
                        case "dimensions":
                            int size = 0, height = 0, width = 0, xoffset = 0, yoffset = 0, penWidth = 0;
                            bool isPointy = false;
                            Color boardColor = Color.White;

                            loadDimensions( childNode, ref size, ref height, ref width, ref xoffset, ref yoffset, ref penWidth, ref isPointy, ref boardColor );
                            board = new CombatBoard(this.lhg, size, width, height, xoffset, yoffset, penWidth, isPointy, boardColor);
                            break;

                        case "colors":
                            if (board != null)
                            {
                                Color backgroundColor = Color.Black, selectedPlayerColor = Color.Black, moveRadiusColor = Color.Black,
                                      moveSelectColor = Color.Black, attackRadiusColor = Color.Black;

                                loadColors(childNode, ref backgroundColor, ref selectedPlayerColor, ref moveRadiusColor, ref moveSelectColor, ref attackRadiusColor);
                                board.setHexColors(backgroundColor, selectedPlayerColor, moveRadiusColor, moveSelectColor, attackRadiusColor);
                            }
                            break;

                        case "gameEntities":
                            if (board != null)
                            {
                                GameEntities gameEntities = loadGameEntities(childNode, board);
                                board.MyGameEntities = gameEntities;
                            }
                            break;
                    }
                }
            }

            return board;
        }

        private void loadDimensions(XmlNode dimensionNode, ref int size, ref int height, ref int width, ref int xoffset, 
                                    ref int yoffset, ref int penWidth, ref bool isPointy, ref Color boardColor )
        {
            try
            {
                size = Convert.ToInt16(dimensionNode.Attributes["size"].Value);
                height = Convert.ToInt16(dimensionNode.Attributes["height"].Value);
                width = Convert.ToInt16(dimensionNode.Attributes["width"].Value);
                xoffset = Convert.ToInt16(dimensionNode.Attributes["xOffset"].Value);
                yoffset = Convert.ToInt16(dimensionNode.Attributes["yOffset"].Value);
                penWidth = Convert.ToInt16(dimensionNode.Attributes["penWidth"].Value);
                loadColor(dimensionNode, ref boardColor);
                
                string orientation = dimensionNode.Attributes["orientation"].Value.ToLower();
                isPointy = true;
                if (orientation.Equals("flat"))
                    isPointy = false;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("CombatFactory.loadDimensions - " + ex.Message);
            }
        }

        private void loadColors(XmlNode colorsNode, ref Color backgroundColor, ref Color selectedPlayerColor, ref Color moveRadiusColor,
                                ref Color moveSelectColor, ref Color attackRadiusColor)
        {
            XmlNodeList colorsChildren = colorsNode.ChildNodes;
            foreach (XmlNode childNode in colorsChildren)
            {
                switch (childNode.Name)
                {
                    case "background":
                        loadColor(childNode, ref backgroundColor);
                        break;

                    case "selectedPlayer":
                        loadColor(childNode, ref selectedPlayerColor);
                        break;

                    case "moveRadius":
                        loadColor(childNode, ref moveRadiusColor);
                        break;

                    case "moveSelect":
                        loadColor(childNode, ref moveSelectColor);
                        break;

                    case "attackRadius":
                        loadColor(childNode, ref attackRadiusColor);
                        break;
                }
            }
        }

        private void loadColor(XmlNode colorNode, ref Color theColor)
        {
            try
            {
                int red = Convert.ToInt16(colorNode.Attributes["red"].Value);
                int green = Convert.ToInt16(colorNode.Attributes["green"].Value);
                int blue = Convert.ToInt16(colorNode.Attributes["blue"].Value);

                theColor = new Color(red, green, blue, 255);
            }
            catch (Exception ex)
            {
            }
        }     

        private GameEntities loadGameEntities(XmlNode gameEntitiesNode, CombatBoard board)
        {
            GameEntities gameEntities = new GameEntities();

            XmlNodeList childNodes = gameEntitiesNode.ChildNodes;
            foreach (XmlNode childNode in childNodes)
            {
                switch ( childNode.Name )
                {
                    case "heroes":
                        break;

                    case "enemies":
                        progress.updateProgress("Loading Enemies", "Loading Enemies", 40);
                        loadEnemies(childNode, gameEntities, board);
                        break;

                    case "obstacles":
                        progress.updateProgress("Loading Obstacles", "Loading Obstacles", 50);
                        loadObstacles(childNode, gameEntities, board);
                        break;
                }
            }

            return gameEntities;
        }

        private void loadEnemies(XmlNode enemiesNode, GameEntities gameEntities, CombatBoard board)
        {
            XmlNodeList playerNodes = enemiesNode.ChildNodes;
            foreach (XmlNode playerNode in playerNodes)
            {
                if (playerNode.Name.Equals("player"))
                {
                    Player player = playerFactory.createPlayerForCombat(board, playerNode);
                    if (player != null)
                    {
                        gameEntities.addPlayer(player);
                    }
                }
            }
        }

        private void loadObstacles(XmlNode obstaclesNode, GameEntities gameEntities, CombatBoard board)
        {
            XmlNodeList children = obstaclesNode.ChildNodes;
            foreach (XmlNode childNode in children)
            {
                Obstacle obstacle = obstacleFactory.createObstacleForCombat(board, childNode);
                if (obstacle != null)
                {
                    gameEntities.addObstacle(obstacle);
                }
            }
        }

        private Vector3 readVector3(XmlNode node)
        {
            float x = (float) Convert.ToDouble(node.Attributes["x"].Value);
            float y = (float) Convert.ToDouble(node.Attributes["y"].Value);
            float z = (float) Convert.ToDouble(node.Attributes["z"].Value);

            return new Vector3(x, y, z);
        }

        private CombatScreen loadScreen(XmlNode backgroundNode)
        {
            LHGCamera camera = null;
            LHGStage stage = null;

            XmlNodeList childNodes = backgroundNode.ChildNodes;
            foreach (XmlNode childNode in childNodes)
            {
                switch (childNode.Name)
                {
                    case "camera":
                        camera = loadCamera(childNode);
                        break;

                    case "stage":
                        stage = stageFactory.loadStage(childNode);
                        break;
                }
            }

            CombatScreen screen = new CombatScreen(this.lhg, camera);
            screen.Initialize();
            screen.MyStage = stage;

            return screen;
        }

        private LHGCamera loadCamera(XmlNode cameraNode)
        {
            LHGCamera camera = new LHGCamera(LHGCameraMode.RollConstrained);
            camera.OrbitRight(MathHelper.Pi);

            XmlNodeList cameraChildren = cameraNode.ChildNodes;
            foreach (XmlNode childNode in cameraChildren)
            {
                try
                {
                    switch (childNode.Name)
                    {
                        case "target":
                            camera.Target = readVector3(childNode);
                            break;

                        case "position":
                            camera.Position = readVector3(childNode);
                            break;

                        case "distance":
                            camera.Distance = (float)Convert.ToDouble(childNode.Attributes["value"].Value);
                            break;

                        case "angle":
                            float angle = (float)Convert.ToDouble(childNode.Attributes["value"].Value);
                            camera.OrbitUp(angle);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("CombatFactory.loadCamera - " + ex.Message);
                }
            }

            /*
             *   LHGCamera camera = new LHGCamera(LHGCameraMode.RollConstrained);
                   
            camera.OrbitRight(MathHelper.Pi);
            camera.OrbitUp(.32f);
            camera.Target = new Vector3(1505f, 0.0f, 470.0f);
            camera.Position = new Vector3(1506f, 844f, 2073f);

            camera.Distance = 2000.0f;

             */

            return camera;
        }
       }
}
