using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames.Combat;
using LunchHourGames.Hexagonal;
using LunchHourGames.Players;

namespace LunchHourGames.Inventory
{
    public class Projectile 
    {
        private Texture2D texture;
        private byte opacity = 255;

        private List<Vector2> pointsPath;
        private Vector2 position;

        private SpriteBatch spriteBatch;

        private Player playerShooting;
        private CombatSystem combatSystem;

        /*
        public Projectile(LunchHourGames lhg, String name, int range, int damage, 
                      int areaOfEffect, int actionPointCost)
            : base(lhg, name, range, damage, areaOfEffect, actionPointCost)
        {
            this.spriteBatch = lhg.MySpriteBatch;
            this.combatSystem = lhg.MyCombatSystem;
        }

        public override void Initialize()
        {
            base.Initialize();

           this.texture = Game.Content.Load<Texture2D>("Sprites/Weapons/bullet");
        }

        public void setPath(CombatBoard combatBoard, List<Vector2> pointsPath, Player playerShooting)
        {
            this.isFiring = true;
            this.pointsPath = pointsPath;
            this.playerShooting = playerShooting;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.pointsPath.Count() == 0)
            {
                // We missed the target!
                this.isFiring = false;
                //combatSystem.handleProjectileMiss(playerShooting, this);
                lhg.showMessageBalloon(gameTime, "Missed!");
            }
            else
            {
                // Bullet is still moving along the path
                position = pointsPath[0];
                pointsPath.RemoveAt(0);

                // Perform collision detection
                Hex hex = playerShooting.Location.board.findHexAt(position);
                if (hex != null && hex.MyGameEntity != null)
                {
                    // We have hit something
                    // RULE:  Combat system nees to know that a object was hit so it can decide what to do
                    Player playerHit = (Player)hex.MyGameEntity;
                    if (playerHit != this.playerShooting)
                    {
                        //combatSystem.handleProjectileHit(playerShooting, playerHit, this);
                    }
                }
            }
        }

 */
                        


        /*


                        // stop the bullet animation, since we hit something.
                        this.isFiring = false;
                        this.pointsPath.Clear();

                        if (hex.MyGameEntity.MyEntityType == EntityType.Player)
                        {
                            // Reduce this player's hit points
                            playerHit.MyAttributes.hitPoints -= this.damage;

                            // Show animation that they were hit

                            // if hit points is less than zero they will die

                            // show dying animation

                            lhg.showMessageBalloon(gameTime, "Good Shot!");

                            lhg.removePlayer(playerHit);
      
         */
        /*
        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);
            //spriteBatch.Draw( texture, position, Color.White);
        }
         */
    }
}
