using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using LunchHourGames.Sprite;
using LunchHourGames.Drawing;
using LunchHourGames.Players;
using LunchHourGames.Inventory;

namespace LunchHourGames.Combat
{
    public class CombatHUD : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private LunchHourGames lhg;
        private CombatScreen combatScreen;

        private Vector2 position;

        private List<CombatHUDPanel> humanPanels = new List<CombatHUDPanel>();
        private List<CombatHUDPanel> zombiePanels = new List<CombatHUDPanel>();
          
        public CombatHUD(LunchHourGames lhg, CombatScreen combatScreen)
            :base(lhg)
        {
            this.lhg = lhg;
            this.combatScreen = combatScreen;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        public void startLevel(int level)
        {
            int xPos = 20;
            int yPos = 20;

            CombatBoard board = this.combatScreen.MyCombatBoard;
            List<Human> humans = board.getHumans();
            foreach (Human human in humans)
            {
                CombatHUDPanel humanPanel = new CombatHUDPanel(lhg, human);
                humanPanel.Position = new Vector2(xPos, yPos);
                yPos += 75;
                humanPanels.Add(humanPanel);
            }

            xPos = 200;
            yPos = 20;

            List<Zombie> zombies = board.getZombies();
            foreach (Zombie zombie in zombies)
            {
                CombatHUDPanel zombiePanel = new CombatHUDPanel(lhg, zombie);
                zombiePanel.Position = new Vector2(xPos, yPos);
                zombiePanels.Add(zombiePanel);
            }
        }

        public Player CurrentPlayer
        {
            get { return this.combatScreen.MyCombatBoard.CurrentPlayer; }
        }

        private CombatHUDPanel getZombiePanel(Zombie zombie)
        {
            foreach (CombatHUDPanel zombiePanel in zombiePanels)
            {
                if (zombiePanel.MyPlayer == zombie)
                    return zombiePanel;
            }

            return null;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (CombatHUDPanel humanPanel in humanPanels)
                humanPanel.Update(gameTime);

            Player player = CurrentPlayer;
            if (player.MyType == Player.Type.Zombie)
            {
                CombatHUDPanel zombiePanel = getZombiePanel((Zombie)player);
                zombiePanel.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public Vector2 Position
        {
            get { return this.position;  }
            set { this.position = value; }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (CombatHUDPanel humanPanel in humanPanels)
                humanPanel.Draw(gameTime);

            Player player = CurrentPlayer;
            if (player.MyType == Player.Type.Zombie)
            {
                CombatHUDPanel zombiePanel = getZombiePanel((Zombie)player);
                zombiePanel.Draw(gameTime);
            }
        }
    }
}