using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames.Sprite;

namespace LunchHourGames.Players
{
    public class PlayerSpriteSheet : SpriteSheet
    {
        public enum Type
        {
            Unknown, Idle, Standing, Walking, Running, Attacking, BeenHit, Shooting, 
            Dying, Exploding, OnFire, Disappearing, UsingItem, FallingDown
        }

        public enum Mode
        {
            Unknown, Combat, Travel, Scavenge, Story
        }
       
        private Type type;
        private Mode mode;

        public PlayerSpriteSheet(LunchHourGames lhg, Type type, Mode mode, string textureName, Texture2D texture, List<Animation> animations)
            :base(lhg, textureName, texture, animations)
        {            
            this.type = type;
            this.mode = mode;
        }
       
        public static PlayerSpriteSheet.Mode getModeFromString(string modeAsString)
        {
            PlayerSpriteSheet.Mode mode = PlayerSpriteSheet.Mode.Unknown;

            switch (modeAsString)
            {
                case "combat":
                    mode = PlayerSpriteSheet.Mode.Combat;
                    break;

                case "travel":
                    mode = PlayerSpriteSheet.Mode.Travel;
                    break;

                case "scavenge":
                    mode = PlayerSpriteSheet.Mode.Scavenge;
                    break;

                case "story":
                    mode = PlayerSpriteSheet.Mode.Story;
                    break;
            }

            return mode;
        }

        public static PlayerSpriteSheet.Type getTypeFromString(string typeAsString)
        {
            PlayerSpriteSheet.Type type = PlayerSpriteSheet.Type.Unknown;

            switch ( typeAsString )
            {
                case "idle":
                    type = PlayerSpriteSheet.Type.Idle;
                   break;
                
                case "standing":
                   type = PlayerSpriteSheet.Type.Standing;
                    break;

                case "walking":
                    type = PlayerSpriteSheet.Type.Walking;
                    break;

                case "running":
                    type = PlayerSpriteSheet.Type.Running;
                    break;

                case "attacking":
                    type = PlayerSpriteSheet.Type.Attacking;
                    break;

                case "beenhit":
                    type = PlayerSpriteSheet.Type.BeenHit;
                    break;

                case "shooting":
                    type = PlayerSpriteSheet.Type.Shooting;
                    break;

                case "dying":
                    type = PlayerSpriteSheet.Type.Dying;
                    break;

                case "Exploding":
                    type = PlayerSpriteSheet.Type.Exploding;
                    break;

                case "onfire":
                    type = PlayerSpriteSheet.Type.OnFire;
                    break;

                case "disappearing":
                    type = PlayerSpriteSheet.Type.Disappearing;
                    break;

                case "usingitem":
                    type = PlayerSpriteSheet.Type.UsingItem;
                    break;

                case "fallingdown":
                    type = PlayerSpriteSheet.Type.FallingDown;
                    break;

                default:
                    type = PlayerSpriteSheet.Type.Unknown;
                    break;
            }

            return type;
        }

        public Type MyType
        {
            get { return this.type; }
        }

        public Mode MyMode
        {
            get { return this.mode; }
        }
    }
}
