using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LunchHourGames.Screen;

namespace LunchHourGames.Players
{
    class RatingScreen : GameScreen, ButtonMenuEvent
    {
        private BackgroundPanel background;
        MouseState mouseStatePrevious;

        private PrimaryStatistics stats;

        private List<NumberBox> numberBoxes;
        private NumberBox strengthNumberBox;
        private NumberBox utilizationNumberBox;
        private NumberBox resourcefulnessNumberBox;
        private NumberBox vitalityNumberBox;
        private NumberBox intelligenceNumberBox;
        private NumberBox visionNumberBox;
        private NumberBox agilityNumberBox;
        private NumberBox luckNumberBox;

        private ButtonMenu buttonMenu;
        private bool isItemSelected = false; 

        public RatingScreen(LunchHourGames lhg)
            : base(lhg, Type.PlayerRating)
        {
            this.stats = new PrimaryStatistics();
            this.numberBoxes = new List<NumberBox>();

            mouseStatePrevious = Mouse.GetState();
            LoadContent();

            TransitionPosition = 0;
            TransitionOnTime = TimeSpan.Zero;
            TransitionOffTime = new TimeSpan(0, 0, 2);  // Allow 2 seconds for fade
        }

        public void userSelectedItem(ButtonMenuItem menuItem)
        {
            if (!isItemSelected)
            {
                isItemSelected = true;
                lhg.MyGameFlow.setGameScreenComplete(this);
            }

        }

        public PrimaryStatistics MyStats
        {
            get { return this.stats; }
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Texture2D texture = Content.Load<Texture2D>("Backgrounds/ratingscreen");
            background = new BackgroundPanel(lhg, texture, true);

            addStartButton();

/*
S - STR - Strength
U - UTI - Utilization
R - RES - Resourcefulness
V - VIT - Vitality
I - INT - Intelligence
V - VIS - Vision
A - AGI - Agility
L - LCK - Luck
 * 
 * 
*/
            float horizontalSpacing = 500.0f;

            strengthNumberBox = createNumberBox(new Vector2(horizontalSpacing, 60.0f), stats.strength, "STR");
            numberBoxes.Add(strengthNumberBox);

            utilizationNumberBox = createNumberBox(new Vector2(horizontalSpacing, 145.0f), stats.utilization, "UTI");
            numberBoxes.Add(utilizationNumberBox);

            resourcefulnessNumberBox = createNumberBox(new Vector2(horizontalSpacing, 230.0f), stats.resourcefulness, "RES");
            numberBoxes.Add(resourcefulnessNumberBox);

          
            vitalityNumberBox = createNumberBox(new Vector2(horizontalSpacing, 315.0f), stats.vitality, "VIT");
            numberBoxes.Add(vitalityNumberBox);

            intelligenceNumberBox = createNumberBox(new Vector2(horizontalSpacing, 400.0f), stats.intelligence, "INT");
            numberBoxes.Add(intelligenceNumberBox);

            visionNumberBox = createNumberBox(new Vector2(horizontalSpacing, 485.0f), stats.vision, "VIS");
            numberBoxes.Add(visionNumberBox);

            agilityNumberBox = createNumberBox(new Vector2(horizontalSpacing, 570.0f), stats.agility, "AGI");
            numberBoxes.Add(agilityNumberBox);

            luckNumberBox = createNumberBox(new Vector2(horizontalSpacing, 655.0f), stats.luck, "LCK");
            numberBoxes.Add(luckNumberBox);
        }

        private void addStartButton()
        {
            Texture2D buttonImage = Content.Load<Texture2D>("GUI/start_button");
            buttonMenu = new ButtonMenu(lhg, lhg.NormalFont, buttonImage, 5, this);

            string[] items = { "Start" };

            buttonMenu.SetMenuItems(items);
            buttonMenu.Position = new Vector2(700, 500);
            buttonMenu.SelectedIndex = 0;
        }

        private NumberBox createNumberBox(Vector2 position, int value, String attributeName)
        {
            Texture2D numberBoxBackground = Content.Load<Texture2D>("GUI/plusminus");
            NumberBox numberBox = new NumberBox(lhg, attributeName, value, position, 0, 46, lhg.NormalFont, numberBoxBackground,
                new Rectangle(77, 0, 35, 35), new Rectangle(77, 51, 35, 35));

            return numberBox;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            MouseState mouseStateCurrent = Mouse.GetState();

            if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)
            {
                Point mousePoint = new Point(mouseStateCurrent.X, mouseStateCurrent.Y);
               
                // Determine if the user clicked on a number box button (+ or -)
                NumberBox numberBox = findNumberBoxByMousePoint(mousePoint);
                if (numberBox != null)
                {
                    int increaseBy = numberBox.determineMouseClick(mousePoint);
                    switch (numberBox.MyAttributeName)
                    {
                        case "STR":
                            stats.strength += increaseBy;
                            break;

                        case "UTI":
                            stats.utilization += increaseBy;
                            break;

                        case "RES":
                            stats.resourcefulness += increaseBy;
                            break;

                        case "VIT":
                            stats.vitality += increaseBy;
                            break;

                        case "INT":
                            stats.intelligence += increaseBy;
                            break;

                        case "VIS":
                            stats.vision += increaseBy;
                            break;

                        case "AGI":
                            stats.agility += increaseBy;
                            break;

                        case "LCK":
                            stats.luck += increaseBy;
                            break;
                    }
                }
            }

            mouseStatePrevious = mouseStateCurrent;

            foreach (NumberBox numberBox in numberBoxes)
            {
                numberBox.Update(gameTime);
            }

            buttonMenu.Update(gameTime);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private NumberBox findNumberBoxByMousePoint(Point point)
        {
            foreach (NumberBox numberBox in numberBoxes)
            {
                if (numberBox.isPointInBox(point))
                    return numberBox;
            }

            return null;
        }

        public override void Draw(GameTime gameTime)
        {           
            spriteBatch.Begin();
            background.Draw(gameTime);

            foreach (NumberBox numberBox in numberBoxes)
            {
                numberBox.Draw(gameTime);
            }

            buttonMenu.Draw(gameTime);
               
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
