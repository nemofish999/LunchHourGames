using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames.Players;
using LunchHourGames.Sprite;

namespace LunchHourGames.Combat
{
    class CombatHUDPanel : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private LunchHourGames lhg;
        private Player player;

        private SpriteFont font;
        private SpriteBatch spriteBatch;

        private String playerName = "";
        private String actionPoints = "AP: ";
        private String hitPoints = "HP: ";

        private StaticSprite playerSprite;
        private Vector2 position;

        private int xOffset, yOffset;

        private int hudXSize, hudYSize;
        int LinesDisplayed = 5;

        //private ProgressBar progressBar;

        private string copyrightInfo = "Prototype only.  Do not distribute.  Copyright 2011 Lunch Hour Games";

        public CombatHUDPanel(LunchHourGames lhg, Player player)
            :base(lhg)
        {
            this.lhg = lhg;
            this.spriteBatch = lhg.MySpriteBatch;
            this.player = player;
            this.playerSprite = player.MyHUDSprite;
            this.playerName = player.MyName;
            this.font = lhg.SmallFont;
        }

        public Player MyPlayer
        {
            get { return this.player; }
        }

        public Vector2 Position
        {
            get { return this.position;  }
            set 
            { 
                this.position = value;
                if (playerSprite != null)
                {
                    playerSprite.Position = position;

                    xOffset = (int)position.X + playerSprite.Width;
                    yOffset = (int)position.Y;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            actionPoints = string.Format("AP: {0}", player.MyAttributes.actionPoints);
            hitPoints = string.Format("HP: {0}", player.MyAttributes.hitPoints);
            if ( playerSprite != null )
                playerSprite.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.DrawString(font, copyrightInfo, new Vector2(300, 3), Color.Yellow);

            if (playerSprite != null)
                playerSprite.Draw(gameTime);

            int j = 0;
            spriteBatch.DrawString(font, playerName, new Vector2(xOffset + 5, yOffset + font.LineSpacing * (j)), Color.Yellow);
            j++;
            spriteBatch.DrawString(font, actionPoints, new Vector2(xOffset + 5, yOffset + font.LineSpacing * (j)), Color.Yellow);
            j++;
            spriteBatch.DrawString(font, hitPoints, new Vector2(xOffset + 5, yOffset + font.LineSpacing * (j)), Color.Yellow);
            j++;

            base.Draw(gameTime);
        }

    /*
        progressBar = new ProgressBar(lhg, 20, 10, 30, 10, ProgressBar.Orientation.HORIZONTAL_LR);
        progressBar.BackgroundColor = Color.Red;
        progressBar.FillColor = Color.Blue;
        progressBar.BorderColorInner = Color.Red;
        progressBar.BorderColorOuter = Color.Red;
        progressBar.BorderThicknessInner = 3;
        progressBar.BorderThicknessOuter = 3;            
      */ 


        /*
         * 
            Player player = combatSystem.CurrentPlayer;
            playerName = string.Format("{0}", player.Name );
         */




        //set the offsets 
        //int hudXOffset = 80;
        //int hudYOffset = 10;

        //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);           
        //background.SetData<Color>(new Color[1] { new Color(0, 0, 0, 128) });
        //spriteBatch.Draw(background, new Rectangle(hudXOffset, hudYOffset, hudXSize, hudYSize), Color.White);

        //sprite.Draw(gameTime);

        /*
        spriteBatch.DrawString(font, weaponName, new Vector2(hudXOffset + 5, hudYOffset + font.LineSpacing * (j)), Color.Yellow);
        j++;
        spriteBatch.DrawString(font, ammoLeft, new Vector2(hudXOffset + 5, hudYOffset + font.LineSpacing * (j)), Color.Yellow);
        j++;
        */


        //spriteBatch.End();

        //spriteBatch.Begin();
        //progressBar.Draw(spriteBatch);
        //spriteBatch.End();


    }
}
