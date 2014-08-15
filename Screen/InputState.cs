
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LunchHourGames.Screen
{
    // Helper for reading input from keyboard and gamepad. This class tracks both
    // the current and previous state of both input devices, and implements query
    // properties for high level input actions such as "move up through the menu"
    // or "pause the game".
    public class InputState
    {
        public KeyboardState CurrentKeyboardState;
        public GamePadState CurrentGamePadState;

        public KeyboardState LastKeyboardState;
        public GamePadState LastGamePadState;

        // Checks for a "menu up" input action (on either keyboard or gamepad).
        public bool MenuUp
        {
            get
            {
                return IsNewKeyPress(Keys.Up) ||
                       (CurrentGamePadState.DPad.Up == ButtonState.Pressed &&
                        LastGamePadState.DPad.Up == ButtonState.Released) ||
                       (CurrentGamePadState.ThumbSticks.Left.Y > 0 &&
                        LastGamePadState.ThumbSticks.Left.Y <= 0);
            }
        }

        public bool MenuDown
        {
            get
            {
                return IsNewKeyPress(Keys.Down) ||
                       (CurrentGamePadState.DPad.Down == ButtonState.Pressed &&
                        LastGamePadState.DPad.Down == ButtonState.Released) ||
                       (CurrentGamePadState.ThumbSticks.Left.Y < 0 &&
                        LastGamePadState.ThumbSticks.Left.Y >= 0);
            }
        }

        public bool MenuRight
        {
            get
            {
                return IsNewKeyPress(Keys.Right) ||
                       (CurrentGamePadState.DPad.Right == ButtonState.Pressed &&
                        LastGamePadState.DPad.Right == ButtonState.Released) ||
                       (CurrentGamePadState.ThumbSticks.Left.X > 0 &&
                        LastGamePadState.ThumbSticks.Left.X <= 0);
            }
        }

        public bool MenuLeft
        {
            get
            {
                return IsNewKeyPress(Keys.Left) ||
                       (CurrentGamePadState.DPad.Left == ButtonState.Pressed &&
                        LastGamePadState.DPad.Left == ButtonState.Released) ||
                       (CurrentGamePadState.ThumbSticks.Left.X < 0 &&
                        LastGamePadState.ThumbSticks.Left.X >= 0);
            }
        }

        public bool MenuSelect
        {
            get
            {
                return IsNewKeyPress(Keys.Space) ||
                       IsNewKeyPress(Keys.Enter) ||
                       (CurrentGamePadState.Buttons.A == ButtonState.Pressed &&
                        LastGamePadState.Buttons.A == ButtonState.Released) ||
                       (CurrentGamePadState.Buttons.Start == ButtonState.Pressed &&
                        LastGamePadState.Buttons.Start == ButtonState.Released);
            }
        }

        public bool MenuCancel
        {
            get
            {
                return IsNewKeyPress(Keys.Escape) ||
                       (CurrentGamePadState.Buttons.B == ButtonState.Pressed &&
                        LastGamePadState.Buttons.B == ButtonState.Released) ||
                       (CurrentGamePadState.Buttons.Back == ButtonState.Pressed &&
                        LastGamePadState.Buttons.Back == ButtonState.Released);
            }
        }

        public bool PauseGame
        {
            get
            {
                return IsNewKeyPress(Keys.Escape) ||
                       (CurrentGamePadState.Buttons.Back == ButtonState.Pressed &&
                        LastGamePadState.Buttons.Back == ButtonState.Released) ||
                       (CurrentGamePadState.Buttons.Start == ButtonState.Pressed &&
                        LastGamePadState.Buttons.Start == ButtonState.Released);
            }
        }

        // Reads the latest state of the keyboard and gamepad.
        public void Update()
        {
            LastKeyboardState = CurrentKeyboardState;
            LastGamePadState = CurrentGamePadState;

            CurrentKeyboardState = Keyboard.GetState();
            CurrentGamePadState = GamePad.GetState(PlayerIndex.One);
        }
        
        // Helper for checking if a key was newly pressed during this update.
        bool IsNewKeyPress(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key));
        }
    }
}