using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using LunchHourGames.Inventory.Weapons;
using LunchHourGames.Sprite;
using LunchHourGames.Players;

namespace LunchHourGames.Combat.Attacks
{
    // Handles attacks that move in a linear path.  This includes calculating the points along the path, moving the ammo sprite,
    // handling sound, explosions, and player or target reactions.  That is,this class handles the ENTIRE attack event sequence.
    // At the end of this attack, players may be removed from the board.
    public class LinearAttack : CombatAttack
    {
        private bool targetHasBeenHit = false;
        private bool beenHitAnimationIsPlaying = false;
        private bool targetIsDying = false;
        private bool dyingAnimationIsPlaying = false;

        public LinearAttack(LunchHourGames lhg, CombatSystem combatSystem, Weapon weapon)
            : base(lhg, combatSystem, weapon)
        {           
        }

        public override void Initialize()
        {
            targetHasBeenHit = false;
            beenHitAnimationIsPlaying = false;
            targetIsDying = false;
            dyingAnimationIsPlaying = false;
            hasCompleted = false;

            if (this.targetGameEntity != null)
            {
                // Plot a straight path from the current player to this game entity
                MyWeapon.MyAmmo.MyPath = MyBoard.findPointsAlongLine3D(CurrentPlayer, targetGameEntity, 10f);
                MyWeapon.MyAmmo.setGraphicsMatrices(combatSystem.MyScreen.MyView, combatSystem.MyScreen.MyProjection, combatSystem.MyScreen.MyWorld);
                MyWeapon.fire();                
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (targetHasBeenHit == false)
            {
                // From our point of view the target hasn't been hit yet, 
                // so check the ammo to see if it has, so we can update our flags.
                Ammo ammo = MyWeapon.MyAmmo;
                ammo.Update(gameTime);
                if (ammo.isTargetHit())
                {
                    targetHasBeenHit = true;
                    if (this.targetGameEntity != null)
                    {
                        // Let the game entity know it was hit by this ammo
                        targetGameEntity.hit(ammo, this);
                        
                    }
                }
            }
            else
            {
                // The target has been hit and now we wait for the animation to complete

            }
        }

        public override void Draw(GameTime gameTime)
        {
            if ( !MyWeapon.MyAmmo.isTargetHit() )
                MyWeapon.MyAmmo.Draw(gameTime);
        }

        public override void animationBegin(GameEntity gameEntity, AnimationType type)
        {
        }

        public override void animationEnd(GameEntity gameEntity, AnimationType type)
        {
            if (type == AnimationType.BeenHit)
            {
                // The animation for the target being hit has ended.  We need to decide what to do next.
                if (TargetGameEntity.MyEntityType == GameEntity.EntityType.Player)
                {
                    Player targetPlayer = (Player) TargetGameEntity;
                    if (targetPlayer.MyAttributes.hitPoints <= 0)
                    {
                        // The target player has no more hit points, so the player will die
                        targetPlayer.die(this);
                    }
                    else
                    {
                        hasCompleted = true;
                    }
                }
            }
            else if (type == AnimationType.Dying)
            {
                // Player is dead.  We need to remove the player from the board
                
                hasCompleted = true;
            }
        }
    }
}
