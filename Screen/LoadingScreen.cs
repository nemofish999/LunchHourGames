
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LunchHourGames.Screen
{
    public interface LoadingProgress
    {
        void updateProgress(string developerMessage, string displayMessage, int percentComplete);
        void loadingComplete();
    }

    // The loading screen loads the approprate game system. Normally one screen will transition off at the same time as
    // the next screen is transitioning on, but for larger transitions that can take a longer time to load their data, 
    // we want the menu system to be entirely gone before we start loading the game. This is done as follows:
    // 
    // - Tell all the existing screens to transition off.
    // - Activate a loading screen, which will transition on at the same time.
    // - The loading screen watches the state of the previous screens.
    // - When it sees they have finished transitioning off, it activates the real
    //   next screen, which may take a long time to load its data. The loading
    //   screen will be the only thing displayed while this load is taking place.
    public class LoadingScreen : GameScreen, LoadingProgress
    {
        private BackgroundPanel background;

        private string message = "Loading...";

        public LoadingScreen(LunchHourGames lhg)
            :base(lhg, Type.Loading)
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            LoadContent();
        }

        protected override void LoadContent()
        {
            Texture2D texture = lhg.Content.Load<Texture2D>("Backgrounds/black");
            background = new BackgroundPanel(lhg, texture, true);            
        }
      
        public override void Draw(GameTime gameTime)
        {
            // Center the text in the viewport.
            Viewport viewport = lhg.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = lhg.SmallFont.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;
            Color color = new Color(255, 255, 255, TransitionAlpha);

            // Draw the text.
            lhg.MySpriteBatch.Begin();
            background.Draw(gameTime);
            lhg.MySpriteBatch.DrawString(lhg.SmallFont, message, textPosition, color);
            lhg.MySpriteBatch.End();
        }

        public void updateProgress(string developerMessage, string displayMessage, int percentComplete)
        {
            message += " \n" + developerMessage;
        }

        public void loadingComplete()
        {
        }
    }
}
