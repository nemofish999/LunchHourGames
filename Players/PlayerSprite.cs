///////////////////////////////////////////////////////////////////////////////
// PlayerAnimation.cs
//
// Wrapper to hold one animation sequence for a player
//
// Copyright (C) Lunch Hour Games. All rights reserved.
//



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames;
using LunchHourGames.Sprite;
using LunchHourGames.Combat;

using LunchHourGames.Hexagonal;

namespace LunchHourGames.Players
{
    public class PlayerSprite : AnimatedSprite3D
    {
        private List<PlayerSpriteSheet> spriteSheetList;
        private PlayerSpriteSheet.Type currentType;

        public PlayerSprite(LunchHourGames lhg, List<PlayerSpriteSheet> spriteSheetList, PlayerSpriteSheet.Type startingType) 
            :base(lhg)
        {
            this.spriteSheetList = spriteSheetList;
            changeSpriteSheet(startingType, AnimationKey.South);
        }

        public void updateLocation(CombatLocation combatLocation)
        {
            Vector3 position = combatLocation.position;
            position.Y += ( this.height - 30);

            // Align the sprite in the center of the position
            float centerX = this.width / 2;
            position.X -= centerX;
       
            MyPosition = position;
            Direction = combatLocation.direction;
        }

        public PlayerSpriteSheet getSpriteSheetByType(PlayerSpriteSheet.Type type)
        {
            foreach (PlayerSpriteSheet playerSpriteSheet in spriteSheetList)
            {
                if (playerSpriteSheet.MyType.Equals(type))
                    return playerSpriteSheet;
            }

            return null;
        }

        public void changeSpriteSheet(PlayerSpriteSheet.Type type, AnimationKey animationKey)
        {
            this.currentType = type;
            this.MySpriteSheet = getSpriteSheetByType(type);
            this.MyTexture = MySpriteSheet.getTexture(animationKey, 0);  
        }
    
        public PlayerSprite copy()
        {
            return new PlayerSprite(this.lhg, this.spriteSheetList, this.currentType);
        }
    }                
}
