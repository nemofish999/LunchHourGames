using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames.Sprite;
using LunchHourGames.Combat;
using LunchHourGames.Players;
using LunchHourGames.Screen;

namespace LunchHourGames.Obstacles
{
    public class ObstacleFactory
    {
        private LunchHourGames lhg;
        private XmlDocument obstacleDocument;

        private LoadingProgress progress;
        
        public ObstacleFactory(LunchHourGames lhg, LoadingProgress progress)
        {
            this.lhg = lhg;
            this.progress = progress;
        }
   
        public LoadingProgress MyLoadingProgress
        {
            set { this.progress = value; }
        }

        public Obstacle createObstacleForCombat(CombatBoard board, XmlNode obstacleNode)
        {
            string referenceName = obstacleNode.Attributes["referenceName"].Value;           

            Obstacle obstacle = createObstacleFromTemplate(referenceName);

            XmlNodeList children = obstacleNode.ChildNodes;
            foreach (XmlNode childNode in children)
            {
                if (childNode.Name.Equals("location"))
                {
                    obstacle.Location = loadCombatLocation(board, childNode, obstacle.MySprite);
                }
            }

            return obstacle;
        }

        private CombatLocation loadCombatLocation(CombatBoard board, XmlNode locationNode, ObstacleSprite sprite)
        {
            CombatLocation location = null;

            try
            {
                int row = Convert.ToInt16(locationNode.Attributes["row"].Value);
                int col = Convert.ToInt16(locationNode.Attributes["col"].Value);
                location = new CombatLocation(board, row, col);
            }
            catch (Exception ex)
            {
            }

            return location;
        }       

        // Loads the obstacle from obstacle.xml which is the main definition file for all obstacles
        public Obstacle createObstacleFromTemplate(string referenceName)
        {
            Obstacle obstacle = null;

            if (obstacleDocument == null)
            {
                this.obstacleDocument = new XmlDocument();
                obstacleDocument.Load("Content/Xml/obstacles.xml");
            }

            // Check to see if the obstacle template is in our cache already.
            Obstacle cachedObstacle = lhg.Assets.findObstacleTemplateInCache(referenceName);
            if (cachedObstacle != null)
            {
                progress.updateProgress("Loading obstacle template " + referenceName + " from cache", "Loading", 0);
                obstacle = createQuickCopy(cachedObstacle);
            }
            else
            {
                // We have to load this from our xml template
                string xpath = "/obstacles/obstacleGroup/obstacle[@referenceName='" + referenceName + "']";
                XmlNode obstacleNode = obstacleDocument.SelectSingleNode(xpath);
                if (obstacleNode != null)
                {
                    XmlNode groupNode = obstacleNode.ParentNode;
                    List<Obstacle> obstacles = loadObstacleGroup(groupNode);
                    lhg.Assets.addObstacleTemplates(obstacles);

                    cachedObstacle = lhg.Assets.findObstacleTemplateInCache(referenceName);
                    if (cachedObstacle != null)
                    {
                        progress.updateProgress("Loading obstacle template " + referenceName + " from cache", "Loading", 0);
                        obstacle = createQuickCopy(cachedObstacle);
                    }
                }
            }

            return obstacle;
        }

        private Obstacle createQuickCopy(Obstacle cachedObstacle)
        {
            ObstacleSprite sprite = cachedObstacle.MySprite.copy();

            // create a new instance of this player
            Obstacle obstacle = new Obstacle(this.lhg, cachedObstacle.MyReferenceName, cachedObstacle.MyDisplayName, sprite);
            
             return obstacle;
        }

        private List<Obstacle> loadObstacleGroup(XmlNode groupNode)
        {
            List<Obstacle> obstacles = new List<Obstacle>();

            // Load the texture from the content stream
            string texturePath = groupNode.Attributes["texture"].Value;
            Texture2D texture = lhg.Content.Load<Texture2D>(texturePath);
            
            // Load the frames for each obstacle and create the sprite sheet
            // Each obstacle makes up a subtexture of the group sprite sheet
            List<SimpleFrame> frames = new List<SimpleFrame>();

            string xpath = "obstacle/frame";
            XmlNodeList frameNodes = groupNode.SelectNodes(xpath);
            if (frameNodes != null)
            {
                frames = loadFrames(frameNodes);
                StaticSpriteSheet spriteSheet = new StaticSpriteSheet(lhg, texturePath, texture, frames);
                lhg.Assets.addStaticSpriteSheet(spriteSheet);

                // Now that we have the sprite sheet, load each obstacle

                // Load the properties that will be for each obstacle

                ObstacleProperties properties = null;

                XmlNodeList groupChildren = groupNode.ChildNodes;
                foreach (XmlNode childNode in groupChildren)
                {
                    switch (childNode.Name)
                    {
                        case "properties":
                            properties = loadObstacleProperties(childNode);
                            break;

                        case "obstacle":
                            Obstacle obstacle = loadObstacle(childNode, spriteSheet);
                            obstacle.MyProperties = properties;
                            obstacles.Add(obstacle);
                            break;
                    }
                }
            }

            return obstacles;
        }

        private List<SimpleFrame> loadFrames(XmlNodeList frameNodes)
        {
            List<SimpleFrame> frames = new List<SimpleFrame>();

            foreach (XmlNode frameNode in frameNodes)
            {
                SimpleFrame frame = loadFrame(frameNode);
                frames.Add(frame);
            }

            return frames;
        }

        private SimpleFrame loadFrame(XmlNode frameNode)
        {
            try
            {
                int frameWidth = Convert.ToInt16(frameNode.Attributes["frameWidth"].Value);
                int frameHeight = Convert.ToInt16(frameNode.Attributes["frameHeight"].Value);
                int xOffset = Convert.ToInt16(frameNode.Attributes["xOffset"].Value);
                int yOffset = Convert.ToInt16(frameNode.Attributes["yOffset"].Value);

                XmlNode obstacleNode = frameNode.ParentNode;
                string referenceName = obstacleNode.Attributes["referenceName"].Value;
                SimpleFrame frame = new SimpleFrame(referenceName, xOffset, yOffset, frameWidth, frameHeight);
                return frame;
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        private ObstacleProperties loadObstacleProperties(XmlNode propertiesNode)
        {
            return null;
        }

        private Obstacle loadObstacle(XmlNode obstacleNode, StaticSpriteSheet spriteSheet)
        {
            string referenceName = obstacleNode.Attributes["referenceName"].Value;
            string displayName = obstacleNode.Attributes["displayName"].Value;
            Texture2D texture = spriteSheet.getTexture(referenceName);

            ObstacleSprite sprite = new ObstacleSprite(lhg, texture);
            Obstacle obstacle = new Obstacle(lhg, referenceName, displayName, sprite);
            return obstacle;
        }
    }
}
