using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LunchHourGames.Screen;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LunchHourGames.Players
{
    class PlayerSetup : LoadingProgress
    {
        private LunchHourGames lhg;
        private PlayerScreen playerScreen;
        private RatingScreen ratingScreen;

        public PlayerSetup(LunchHourGames lhg)
        {
            this.lhg = lhg;
            this.playerScreen = new PlayerScreen(lhg);
            this.ratingScreen = new RatingScreen(lhg);
        }

        public GameScreen getCurrentScreen()
        {
            return ratingScreen;
        }

        public void Update(GameTime gameTime)
        {
            this.ratingScreen.Update(gameTime);
        }    

        public Player getPlayer()
        {
            //PlayerFactory playerFactory = new PlayerFactory(this.lhg, this);
            //Player player = playerFactory.createMainPlayer("arno", "Travis", this.ratingScreen.MyStats);
            //player.IsComputerControlled = false;
            //return player;
            return null;
        }

        public void updateProgress(string developerMessage, string displayMessage, int percentComplete)
        {
        }

        public void loadingComplete()
        {
        }
    }
}
