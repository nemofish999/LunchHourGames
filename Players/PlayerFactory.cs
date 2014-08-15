using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames.Sprite;
using LunchHourGames.Combat;
using LunchHourGames.Common;
using LunchHourGames.Screen;
using LunchHourGames.Inventory;

namespace LunchHourGames.Players
{
    public class PlayerFactory
    {
        private LunchHourGames lhg;
        private XmlDocument playersDocument;

        private LoadingProgress progress;

        private InventoryFactory inventoryFactory;

        public PlayerFactory(LunchHourGames lhg, LoadingProgress progress, InventoryFactory inventoryFactory)
        {
            this.lhg = lhg;
            this.progress = progress;
            this.inventoryFactory = inventoryFactory;
        }

        public LoadingProgress MyLoadingProgress
        {
            set { this.progress = value; }
        }

        public Player createMainPlayer(string referenceName, string name, PrimaryStatistics stats)
        {
            LHGCommon.printToConsole("Creating main player " + name);
            Player player = createPlayerFromTemplate(referenceName);
            if (player.MyType == Player.Type.Human)
            {
                Human human = (Human)player;
                human.MyName = name;
                human.MyStats = stats;
                human.IsComputerControlled = false;
                human.IsMainPlayer = true;
                human.IsAlive = true;
                return human;
            }

            return null;
        }
      
        public Player createPlayerForCombat(CombatBoard combatBoard, XmlNode playerNode)
        {
            string referenceName = playerNode.Attributes["referenceName"].Value;
            string name = playerNode.Attributes["name"].Value;
            progress.updateProgress("Creating player " + name + " for combat", "Loading", 0);

            Player player = createPlayerFromTemplate(referenceName);
            player.MyName = name;
            player.IsAlive = true;

            PrimaryStatistics stats = null;
            CombatLocation location = null;

            XmlNodeList childNodes = playerNode.ChildNodes;
            foreach (XmlNode childNode in childNodes)
            {
                switch (childNode.Name)
                {
                    case "stats":
                        progress.updateProgress("Loading " + name + " stats", "Loading", 0);
                        stats = loadStats(childNode);
                        break;

                    case "location":
                        progress.updateProgress("Loading " + name + " location", "Loading", 0);
                        location = loadCombatLocation(combatBoard, player, childNode);
                        break;
                }
            }
            
            player.Location = location;
            return player;
        }
    
        private CombatLocation loadCombatLocation(CombatBoard combatBoard, Player player, XmlNode locationNode)
        {
            CombatLocation location = null;

            try
            {
                int row = Convert.ToInt16(locationNode.Attributes["row"].Value);
                int col = Convert.ToInt16(locationNode.Attributes["col"].Value);
                AnimationKey direction = getDirectionFromString(locationNode.Attributes["facing"].Value);
                location = new CombatLocation(combatBoard, row, col, direction);
            }
            catch (Exception ex)
            {
            }

            return location;
        }       

        private AnimationKey getDirectionFromString(string facing)
        {
            AnimationKey direction = AnimationKey.North;

            switch (facing)
            {
                case "n":
                    direction = AnimationKey.North;
                    break;

                case "ne":
                    direction = AnimationKey.NorthEast;
                    break;

                case "e":
                    direction = AnimationKey.East;
                    break;

                case "se":
                    direction = AnimationKey.SouthEast;
                    break;

                case "s":
                    direction = AnimationKey.South;
                    break;

                case "sw":
                    direction = AnimationKey.SouthWest;
                    break;

                case "w":
                    direction = AnimationKey.West;
                    break;

                case "nw":
                    direction = AnimationKey.NorthWest;
                    break;
            }

            return direction;
        }
      
        private Player createPlayerFromTemplate(string referenceName)
        {
            Player player = null;

            if (playersDocument == null)
            {
                this.playersDocument = new XmlDocument();
                playersDocument.Load("Content/Xml/players.xml");
            }

            // Check to see if the player template is in our cache already.
            Player cachedPlayer = lhg.Assets.findPlayerTemplateInCache(referenceName);
            if (cachedPlayer != null)
            {
                progress.updateProgress("Loading player template " + referenceName + " from cache", "Loading", 0);
                return createQuickCopy(cachedPlayer);
            }
            else
            {
                progress.updateProgress("Loading player template " + referenceName + " from disk", "Loading", 0);
                string xpath = "/players/player[@referenceName='" + referenceName + "']";
                XmlNode playerNode = playersDocument.SelectSingleNode(xpath);
                if (playerNode != null)
                {
                    PrimaryStatistics stats = null;
                    
                    List<PlayerSpriteSheet> playerSpriteSheetList = null;
                    StaticSprite hudSprite = null;
                    InventoryStorage inventoryStorage = null;
                    Player.Type type = Player.getTypeFromString(playerNode.Attributes["type"].Value);
                    XmlNodeList playerChildren = playerNode.ChildNodes;
                    foreach (XmlNode childNode in playerChildren)
                    {
                        switch (childNode.Name)
                        {
                            case "stats":
                                stats = loadStats(childNode);
                                break;

                            case "hud":
                                hudSprite = loadCombatHUDSprite(childNode);
                                break;

                            case "animations":
                                playerSpriteSheetList = loadAnimations(childNode);
                                break;

                            case "inventory":
                                inventoryStorage = loadInventory(childNode);
                                break;
                        }
                    }

                    PlayerSprite playerSprite = new PlayerSprite(lhg, playerSpriteSheetList, PlayerSpriteSheet.Type.Standing);

                    // Create a player for our cache that won't change, so we can quickly create copies
                    if (type == Player.Type.Human)
                        cachedPlayer = new Human(lhg, referenceName, referenceName, playerSprite);
                    else if (type == Player.Type.Zombie)
                        cachedPlayer = new Zombie(lhg, referenceName, referenceName, playerSprite);

                    cachedPlayer.MyHUDSprite = hudSprite;
                    cachedPlayer.MyInventory = inventoryStorage;
                    lhg.Assets.addPlayerTemplate(cachedPlayer);

                    // Now, create a copy we will use as the real instance
                    player = createQuickCopy(cachedPlayer);
                }
            }

            return player;
        }

        private Player createQuickCopy(Player cachedPlayer)
        {
            PlayerSprite sprite = cachedPlayer.MySprite.copy();

            Player player = null;

            // create a new instance of this player
            if ( cachedPlayer.MyType == Player.Type.Human )
                player = new Human(lhg, cachedPlayer.MyTemplateName, cachedPlayer.MyName, sprite);
            else if ( cachedPlayer.MyType == Player.Type.Zombie )
                player = new Zombie(lhg, cachedPlayer.MyTemplateName, cachedPlayer.MyName, sprite);

            player.MyHUDSprite = cachedPlayer.MyHUDSprite;
            if (cachedPlayer.MyInventory != null )
                player.MyInventory = cachedPlayer.MyInventory.copy();
            return player;
        }

        public StaticSprite loadCombatHUDSprite(XmlNode hudNode)
        {
            string spriteName = hudNode.Attributes["texture"].Value;
            Texture2D texture = lhg.Content.Load<Texture2D>(spriteName);
            return new StaticSprite(lhg, texture, Vector2.Zero, lhg.MySpriteBatch);
        }

        public InventoryStorage loadInventory(XmlNode inventoryNode)
        {
            return inventoryFactory.loadInventoryStorage(inventoryNode);
        }

        private PrimaryStatistics loadStats(XmlNode statsNode)
        {
            try
            {
                int strength = Convert.ToInt16(statsNode.Attributes["strength"].Value);
                int utilization = Convert.ToInt16(statsNode.Attributes["utilization"].Value);
                int resourcefulness = Convert.ToInt16(statsNode.Attributes["resourcefulness"].Value);
                int vitality = Convert.ToInt16(statsNode.Attributes["vitality"].Value);
                int intelligence = Convert.ToInt16(statsNode.Attributes["intelligence"].Value);
                int vision = Convert.ToInt16(statsNode.Attributes["vision"].Value);
                int agility = Convert.ToInt16(statsNode.Attributes["agility"].Value);
                int luck = Convert.ToInt16(statsNode.Attributes["luck"].Value);

                return new PrimaryStatistics(strength, utilization, resourcefulness, vitality,
                                             intelligence, vision, agility, luck);
            }
            catch (Exception ex)
            {
            }
            
            return null;
        }

        private List<PlayerSpriteSheet> loadAnimations(XmlNode animationsNode)
        {
            List<PlayerSpriteSheet> playerSpriteSheetList = new List<PlayerSpriteSheet>();
            XmlNodeList childNodes = animationsNode.ChildNodes;
            foreach (XmlNode animationNode in childNodes)
            {
                if (animationNode.Name.Equals("animation"))
                {
                    PlayerSpriteSheet playerSpriteSheet = loadAnimation(animationNode);
                    if (playerSpriteSheet != null)
                        playerSpriteSheetList.Add(playerSpriteSheet);
                }
            }

            return playerSpriteSheetList;
        }

        private PlayerSpriteSheet loadAnimation(XmlNode animationNode)
        {
            PlayerSpriteSheet playerSpriteSheet = null;

            try
            {
                PlayerSpriteSheet.Type type = PlayerSpriteSheet.getTypeFromString(animationNode.Attributes["type"].Value);
                PlayerSpriteSheet.Mode mode = PlayerSpriteSheet.getModeFromString(animationNode.Attributes["mode"].Value);
                string textureName = animationNode.Attributes["texture"].Value;
               
                // Check to see if the texture has already been loaded into our asset manager
                playerSpriteSheet = (PlayerSpriteSheet) lhg.Assets.findSpriteSheet(textureName);
                if (playerSpriteSheet == null)
                {
                    Texture2D texture = lhg.Content.Load<Texture2D>(textureName);
                    List<Animation> positions = loadPositions(animationNode);
                    playerSpriteSheet = new PlayerSpriteSheet(lhg, type, mode, textureName, texture, positions);
                    lhg.Assets.addSpriteSheet(playerSpriteSheet);
                    progress.updateProgress("Loading player sprite sheet " + textureName, "Loading", 0);
                }
                else
                {
                    progress.updateProgress("Using sprite sheet from cache " + textureName, "Loading", 0);
                }

                //int framesPerSecond = Convert.ToInt16(animationNode.Attributes["framesPerSecond"].Value);               
            }
            catch (Exception ex)
            {
            }

            return playerSpriteSheet;
        }

        private List<Animation> loadPositions(XmlNode animationNode)
        {
            List<Animation> positions = new List<Animation>();

            positions.Add( loadPositions(animationNode, "n") );
            positions.Add( loadPositions(animationNode, "ne") );
            positions.Add( loadPositions(animationNode, "e") );
            positions.Add( loadPositions(animationNode, "se") );
            positions.Add( loadPositions(animationNode, "s") );
            positions.Add( loadPositions(animationNode, "sw") );
            positions.Add( loadPositions(animationNode, "w") );
            positions.Add( loadPositions(animationNode, "nw") );

            return positions;
        }

        private Animation loadPositions(XmlNode animationNode, string facing)
        {
            Animation animation = null;
            AnimationKey direction = getDirectionFromString(facing);

            string xpath = "position[@facing='" + facing + "']";
            XmlNode positionNode = animationNode.SelectSingleNode(xpath);

            try
            {
                int frameCount  = Convert.ToInt16(positionNode.Attributes["frameCount"].Value);
                int frameWidth  = Convert.ToInt16(positionNode.Attributes["frameWidth"].Value);
                int frameHeight = Convert.ToInt16(positionNode.Attributes["frameHeight"].Value);
                int xOffset = Convert.ToInt16(positionNode.Attributes["xOffset"].Value);
                int yOffset = Convert.ToInt16(positionNode.Attributes["yOffset"].Value);

                animation = new Animation(direction, frameCount, frameWidth, frameHeight, xOffset, yOffset);
            }
            catch (Exception ex)
            {
            }

            return animation;
        }
    }
}
