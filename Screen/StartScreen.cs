using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LunchHourGames.Screen
{
    class StartScreen : GameScreen, ButtonMenuEvent
    {
        private BackgroundPanel background;
        private ButtonMenu buttonMenu;
        private bool isItemSelected = false; 

        public StartScreen(LunchHourGames lhg)
            : base(lhg, Type.Start)
        {
            LoadContent();
            TransitionPosition = 0;
            TransitionOnTime = TimeSpan.Zero;
            TransitionOffTime = new TimeSpan(0, 0, 2);  // Allow 2 seconds for fade
        }

        public void userSelectedItem(ButtonMenuItem menuItem)
        {
            if (!isItemSelected)
            {
                switch (menuItem.name)
                {
                    case "NEW GAME":
                        // Tell the game flow class we want to start a new game
                        lhg.MyGameFlow.startNewGame();
                        break;

                    case "CONTINUE GAME":
                        lhg.MyGameFlow.continueGame();
                        break;

                    case "OPTIONS":
                        lhg.MyGameFlow.showOptions();
                        break;

                    case "QUIT":
                        lhg.MyGameFlow.quitGame();
                        break;
                }

                isItemSelected = true;
            }
        }

        protected override void LoadContent()
        {
            Texture2D backgroundImage = Content.Load<Texture2D>("Backgrounds/zombiestart");
            background = new BackgroundPanel(lhg, backgroundImage, true);

            Texture2D buttonImage = Content.Load<Texture2D>("GUI/buttonbackground");
            buttonMenu = new ButtonMenu(lhg, lhg.NormalFont, buttonImage, 5, this);

            string[] items = { "NEW GAME", 
                               "CONTINUE GAME", 
                               "OPTIONS", 
                               "QUIT" };

            buttonMenu.SetMenuItems(items);
            buttonMenu.Position = new Vector2(350, 500);
            buttonMenu.SelectedIndex = 0;
            base.LoadContent();
        }

        public override void Show()
        {
            buttonMenu.Position = new Vector2((Game.Window.ClientBounds.Width - buttonMenu.Width) / 2, 450);
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            buttonMenu.Update(gameTime);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = lhg.MySpriteBatch;
            spriteBatch.Begin();
            background.Draw(gameTime);
            buttonMenu.Draw(gameTime);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
