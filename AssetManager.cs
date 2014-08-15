using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using LunchHourGames.Sprite;
using LunchHourGames.Players;
using LunchHourGames.Obstacles;

namespace LunchHourGames
{
    public class AssetManager
    {
        private LunchHourGames lhg;
        private List<SpriteSheet> spriteSheets = new List<SpriteSheet>();
        private List<StaticSpriteSheet> staticSpriteSheets = new List<StaticSpriteSheet>();
        private List<Texture2D> textures = new List<Texture2D>();

        private List<Player> playerTemplateCache = new List<Player>();
        private List<Obstacle> obstacleTemplateCache = new List<Obstacle>();

        public AssetManager(LunchHourGames lhg)
        {
            this.lhg = lhg;
        }

        public void addSpriteSheet(SpriteSheet spriteSheet)
        {
            this.spriteSheets.Add(spriteSheet);
        }

        public SpriteSheet findSpriteSheet(String textureName)
        {
            foreach (SpriteSheet spriteSheet in spriteSheets)
            {
                if (textureName.CompareTo(spriteSheet.MyName) == 0)
                    return spriteSheet;
            }

            return null;           
        }

        public void addStaticSpriteSheet(StaticSpriteSheet staticSpriteSheet)
        {
            this.staticSpriteSheets.Add(staticSpriteSheet);
        }

        public StaticSpriteSheet findStaticSpriteSheet(String textureName)
        {
            foreach (StaticSpriteSheet staticSpriteSheet in staticSpriteSheets)
            {
                if (textureName.CompareTo(staticSpriteSheet.MyName) == 0)
                    return staticSpriteSheet;
            }

            return null;
        }

        public void addPlayerTemplate(Player playerTemplate)
        {
            this.playerTemplateCache.Add(playerTemplate);
        }

        public Player findPlayerTemplateInCache(string templateName)
        {
            foreach (Player player in playerTemplateCache)
            {
                if (player.MyTemplateName.Equals(templateName))
                    return player;
            }

            return null;
        }

        public void addObstacleTemplate(Obstacle obstacleTemplate)
        {
            this.obstacleTemplateCache.Add(obstacleTemplate);
        }

        public void addObstacleTemplates(List<Obstacle> obstacleTemplates)
        {
            this.obstacleTemplateCache.AddRange(obstacleTemplates);
        }

        public Obstacle findObstacleTemplateInCache(string templateName)
        {
            foreach (Obstacle obstacle in obstacleTemplateCache)
            {
                if (obstacle.MyReferenceName.Equals(templateName))
                    return obstacle;
            }

            return null;
        }

        public Texture2D loadTexture(String textureName)
        {
            Texture2D texture = lhg.Assets.findTexture(textureName);
            if (texture == null)
            {
                texture = lhg.Content.Load<Texture2D>(textureName);
                texture.Name = textureName;
                lhg.Assets.addTexture(texture);
            }

            return texture;
        }
        public Texture2D findTexture(String textureName)
        {
            foreach (Texture2D texture in textures)
            {
                if (textureName.CompareTo(texture.Name) == 0)
                    return texture;
            }

            return null;
        }

        public void addTexture(Texture2D texture)
        {
            this.textures.Add(texture);
        }
    }
}
