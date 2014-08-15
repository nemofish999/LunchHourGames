using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using New2DRPG.CoreComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace New2DRPG
{
    class CreditScreen : GameScreen
    {
        Texture2D background;
        Texture2D credits;
        Texture2D buttonImage;
        SpriteFont spriteFont;
        TimeSpan myTimeSpan;
        Vector2 position;
        byte alphaValue = 0;
        Color tintColor = Color.White;
        ButtonMenu buttonMenu;

        public CreditScreen(Game game)
            : base(game)
        {
            LoadContent();
            Components.Add(new BackgroundComponent(game, background, true));
            string[] items = { "RETURN TO MENU" };

            buttonMenu = new ButtonMenu(game, spriteFont, buttonImage);
            buttonMenu.SetMenuItems(items);
            Components.Add(buttonMenu);

            position = new Vector2(0, 0);
        }

        protected override void LoadContent()
        {
            background = Content.Load<Texture2D>(@"Backgrounds\creditsbackground");
            credits = Content.Load<Texture2D>(@"Backgrounds\credits");
            buttonImage = Content.Load<Texture2D>(@"GUI\buttonbackground");
            spriteFont = Content.Load<SpriteFont>("normal");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            myTimeSpan += gameTime.ElapsedGameTime;
            if (myTimeSpan > TimeSpan.FromMilliseconds(15))
            {
                if (alphaValue < 254)
                    alphaValue++;
                tintColor.A = alphaValue;
                myTimeSpan -= TimeSpan.FromMilliseconds(15);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Draw(credits, position, tintColor);
        }

        public override void Show()
        {
            buttonMenu.Position = new Vector2((Game.Window.ClientBounds.Width -
                                       buttonMenu.Width) / 2, 700);
            alphaValue = 0;
            base.Show();
        }
    }
}

