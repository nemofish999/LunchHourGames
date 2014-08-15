using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using New2DRPG.CoreComponents;

namespace New2DRPG
{
    class ViewCharacterScreen : GameScreen
    {
        ButtonMenu menu;
        Texture2D image;
        Texture2D buttonImage;
        SpriteFont spriteFont;
        Vector2 imagePosition;
        PlayerCharacter playerCharacter;

        public ViewCharacterScreen(Game game)
            : base(game)
        {
            LoadContent();

            Components.Add(new BackgroundComponent(game, image, false));

            imagePosition = new Vector2();
            imagePosition.X = (game.Window.ClientBounds.Width - image.Width) / 2;
            imagePosition.Y = (game.Window.ClientBounds.Height - image.Height) / 2;

            string[] items = { "OK" };
            menu = new ButtonMenu(game, spriteFont, buttonImage);
            menu.SetMenuItems(items);
            Components.Add(menu);
        }

        protected override void LoadContent()
        {
            image = Content.Load<Texture2D>(@"GUI\popupbackground");
            buttonImage = Content.Load<Texture2D>(@"GUI\buttonbackgroundshort");
            spriteFont = Content.Load<SpriteFont>(@"normal");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Vector2 position = new Vector2();
            Vector2 stringSize = new Vector2();
            string text;
            float maxLength = 0.0f;

            text = playerCharacter.Name;
            stringSize = spriteFont.MeasureString(text);

            position.Y = imagePosition.Y + 12;
            position.X = imagePosition.X + (image.Width - stringSize.X) / 2;

            DrawText(text, position);

            position.Y += 10;

            string[] abilityWords = Enum.GetNames(typeof(Abilities));
            CharacterAbilities abilities = playerCharacter.Abilities;
            int[] scores = new int[abilities.Length];

            for (int i = 0; i < scores.Length; i++)
            {
                scores[i] = abilities[i];
            }

            foreach (string s in abilityWords)
            {
                text = s + ": ";
                stringSize = spriteFont.MeasureString(text);
                if (stringSize.X > maxLength)
                    maxLength = stringSize.X;
            }

            for (int i = 0; i < abilities.Length; i++)
            {
                text = abilityWords[i] + ": ";
                
                position.Y += spriteFont.LineSpacing;
                position.X = imagePosition.X + 17;

                DrawText(text, position);

                position.X += maxLength;
                
                text = scores[i].ToString();
                DrawText(text, position);
            }


            string[] statNames = { 
                    "Class: ", 
                    "Level: ", 
                    "HP: ", 
                    "SP: ", 
                    "XP: ", 
                    "Gold: " };
            string[] statValues = {
                    playerCharacter.ClassName,
                    playerCharacter.PlayerLevel.ToString(),
                    playerCharacter.HitPointsCurrent.ToString() + "/" +
                        playerCharacter.HitPointsMax.ToString(),
                    playerCharacter.SpellPointsCurrent.ToString() + "/" +
                        playerCharacter.SpellPointsMax.ToString(),
                    playerCharacter.Experience.ToString(),
                    playerCharacter.Gold.ToString() };

            maxLength = 0.0f;

            foreach (string s in statNames)
            {
                stringSize = spriteFont.MeasureString(s);
                if (stringSize.X > maxLength)
                    maxLength = stringSize.X;
            }

            position.Y = imagePosition.Y + 20;

            for (int i = 0; i < statNames.Length; i++)
            {
                position.Y += spriteFont.LineSpacing;
                position.X = imagePosition.X + (image.Width / 2) + 8;

                text = statNames[i];
                DrawText(text, position);

                position.X += maxLength;

                text = statValues[i].ToString();
                DrawText(text, position);
            }
        }

        private void DrawText(string text, Vector2 position)
        {
            Vector2 shadow = new Vector2();
            shadow = position + Vector2.One;

            spriteBatch.DrawString(spriteFont, text, shadow, Color.Black);
            spriteBatch.DrawString(spriteFont, text, position, Color.White);
        }

        public override void Show()
        {
            base.Show();
            menu.Position = new Vector2((image.Width -
                              menu.Width) / 2 + imagePosition.X,
                              image.Height - menu.Height - 10 + imagePosition.Y);
        }

        public void SetPlayerCharacter(PlayerCharacter playerCharacter)
        {
            this.playerCharacter = playerCharacter;
        }
    }
}
