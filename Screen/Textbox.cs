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


namespace LunchHourGames.Screen
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Textbox : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Texture2D textboxTexture;
        Texture2D cursor;

        SpriteFont spriteFont;
        SpriteBatch spriteBatch;
        ContentManager Content;

        string text;

        Keys[] keysToCheck = new Keys[] { 
            Keys.A, Keys.B, Keys.C, Keys.D, Keys.E,
            Keys.F, Keys.G, Keys.H, Keys.I, Keys.J,
            Keys.K, Keys.L, Keys.M, Keys.N, Keys.O,
            Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T,
            Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y,
            Keys.Z, Keys.Back, Keys.Space };

        Vector2 cursorPosition;
        Vector2 textPosition;
        Vector2 textboxPosition;

        TimeSpan blinkTime;
        bool blink;

        KeyboardState currentKeyboardState;
        KeyboardState lastKeyboardState;

        public Textbox(Game game, SpriteFont spriteFont)
            : base(game)
        {
            spriteBatch =
                (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            Content =
                (ContentManager)Game.Services.GetService(typeof(ContentManager));

            this.spriteFont = spriteFont;

            textboxTexture = Content.Load<Texture2D>("GUI/textbox");
            cursor = Content.Load<Texture2D>("GUI/cursor");

            textboxPosition = new Vector2();
            cursorPosition = new Vector2(
                textboxPosition.X + 5,
                textboxPosition.Y + 5);
            textPosition = new Vector2(
                textboxPosition.X + 5,
                textboxPosition.Y + 5);
            blink = false;
            text = "";
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Vector2 Position
        {
            get { return textboxPosition; }
            set 
            { 
                textboxPosition = value;
                SetTextPosition();
            }
        }

        private void SetTextPosition()
        {
            cursorPosition = new Vector2(
                textboxPosition.X + 5,
                textboxPosition.Y + 5);
            textPosition = new Vector2(
                textboxPosition.X + 5,
                textboxPosition.Y + 5);
        }

        public int Height
        {
            get { return textboxTexture.Height; }
        }

        public int Width
        {
            get { return textboxTexture.Width; }
        }
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            currentKeyboardState = Keyboard.GetState();
            blinkTime += gameTime.ElapsedGameTime;
            if (blinkTime > TimeSpan.FromMilliseconds(500))
            {
                blink = !blink;
                blinkTime -= TimeSpan.FromMilliseconds(500);
            }
            foreach (Keys key in keysToCheck)
            {
                if (CheckKey(key))
                {
                    AddKeyToText(key);
                    break;
                }
            }
            base.Update(gameTime);
            Vector2 textSize = spriteFont.MeasureString(text);
            cursorPosition.X = textPosition.X + textSize.X;
            lastKeyboardState = currentKeyboardState;
        }

        private void AddKeyToText(Keys key)
        {
            string newChar = "";

            if (text.Length >= 17 && key != Keys.Back)
                return;

            switch (key)
            {
                case Keys.A:
                    newChar += "a";
                    break;
                case Keys.B:
                    newChar += "b";
                    break;
                case Keys.C:
                    newChar += "c";
                    break;
                case Keys.D:
                    newChar += "d";
                    break;
                case Keys.E:
                    newChar += "e";
                    break;
                case Keys.F:
                    newChar += "f";
                    break;
                case Keys.G:
                    newChar += "g";
                    break;
                case Keys.H:
                    newChar += "h";
                    break;
                case Keys.I:
                    newChar += "i";
                    break;
                case Keys.J:
                    newChar += "j";
                    break;
                case Keys.K:
                    newChar += "k";
                    break;
                case Keys.L:
                    newChar += "l";
                    break;
                case Keys.M:
                    newChar += "m";
                    break;
                case Keys.N:
                    newChar += "n";
                    break;
                case Keys.O:
                    newChar += "o";
                    break;
                case Keys.P:
                    newChar += "p";
                    break;
                case Keys.Q:
                    newChar += "q";
                    break;
                case Keys.R:
                    newChar += "r";
                    break;
                case Keys.S:
                    newChar += "s";
                    break;
                case Keys.T:
                    newChar += "t";
                    break;
                case Keys.U:
                    newChar += "u";
                    break;
                case Keys.V:
                    newChar += "v";
                    break;
                case Keys.W:
                    newChar += "w";
                    break;
                case Keys.X:
                    newChar += "x";
                    break;
                case Keys.Y:
                    newChar += "y";
                    break;
                case Keys.Z:
                    newChar += "z";
                    break;
                case Keys.Space:
                    newChar += " ";
                    break;
                case Keys.Back:
                    if (text.Length != 0)
                        text = text.Remove(text.Length - 1);
                    return;
            }
            if (currentKeyboardState.IsKeyDown(Keys.RightShift) ||
                currentKeyboardState.IsKeyDown(Keys.LeftShift))
            {
                newChar = newChar.ToUpper();
            }
            text += newChar;
        }

        private bool CheckKey(Keys theKey)
        {
            return lastKeyboardState.IsKeyDown(theKey) && currentKeyboardState.IsKeyUp(theKey);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(textboxTexture, textboxPosition, Color.White);
            if (!blink)
                spriteBatch.Draw(cursor, cursorPosition, Color.White);
            spriteBatch.DrawString(spriteFont, text, textPosition, Color.Black);
            base.Draw(gameTime);
        }

        public void Show()
        {
            Enabled = true;
            Visible = true;
        }

        public void Hide()
        {
            Enabled = false;
            Visible = false;
        }
    }
}
