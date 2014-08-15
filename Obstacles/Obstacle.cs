using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LunchHourGames.Sprite;
using LunchHourGames.Combat;

namespace LunchHourGames.Obstacles
{
    public class Obstacle : GameEntity
    {
        private String referenceName;              
        private ObstacleSprite sprite;
        private CombatLocation combatLocation;
        private ObstacleProperties properties;
            
        public Obstacle(LunchHourGames lhg, String referenceName, String displayName, ObstacleSprite sprite)
            : base(lhg, EntityType.Obstacle, referenceName, displayName)
        {
            this.referenceName = referenceName;
            this.sprite = sprite;
        }

        public void removeFromCombatBoard()
        {
        }

        public CombatLocation Location
        {
            get { return this.combatLocation; }
            set 
            {
                this.combatLocation = value;
                // Put the character in the middle of the cell
                combatLocation.position = MyBoard.putGameEntityOnCell(this, combatLocation.i, combatLocation.j);

                sprite.updateLocation(value);
            }
        }

        public CombatBoard MyBoard
        {
            get { return this.combatLocation.board; }
        }

        public ObstacleProperties MyProperties
        {
            get { return this.properties; }
            set { this.properties = value; }
        }

        public ObstacleSprite MySprite
        {
            get { return this.sprite; }
            set { this.sprite = value; }
        }

        public string MyReferenceName
        {
            get { return this.referenceName; }
        }

        public string MyDisplayName
        {
            get { return this.displayName; }
        }

        public void setCombatPosition(CombatLocation combatLocation)
        {
            sprite.updateLocation(combatLocation); 
        }

        public override void setGraphicsMatrices(Matrix view, Matrix projection, Matrix world)
        {
            sprite.setGraphicsMatrices(view, projection, world);
        }

        public override Matrix MyView
        {
            set 
            {
                base.MyView = value;
                this.sprite.MyView = view;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            sprite.Draw(gameTime);
        }
    }
}
