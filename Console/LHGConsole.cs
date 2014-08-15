using System;
using System.IO;
using System.Text;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace LunchHourGames.Console
{
    public class LHGConsole : DrawableGameComponent
    {
        enum ConsoleState
        {
            Closed,
            Closing,
            Open,
            Opening
        }
       
        private LunchHourGames lhg;
        private ConsoleInterpreter interpreter;

        public ConsoleInterpreter defaultInterpreter;

        double AnimationTime = 0.2;
        int LinesDisplayed = 15;
        double CursorBlinkTime = 0.3;
        string NewLine = "\n";
      
        private GraphicsDevice device;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Texture2D background;
        private bool isConsoleDark;

        string InputBuffer, OutputBuffer;
        ConsoleHistory history;
        int lineWidth, cursorPos, cursorOffset, consoleXSize, consoleYSize;
        double firstInterval, repeatInterval;
        Dictionary<Keys, double> keyTimes;

        ConsoleState State;
        double StateStartTime;
        KeyboardState LastKeyState, CurrentKeyState;

        public LHGConsole(LunchHourGames lhg, SpriteFont font)
            : base(lhg)
        {
            this.lhg = lhg;
            device = lhg.GraphicsDevice;
            spriteBatch = new SpriteBatch(device);
            this.font = font;
            background = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            background.SetData<Color>(new Color[1] { new Color(0, 0, 0, 125) });
            isConsoleDark = false;

            InputBuffer = "";
            history = new ConsoleHistory();

            consoleXSize = Game.Window.ClientBounds.Right - Game.Window.ClientBounds.Left - 20;
            consoleYSize = font.LineSpacing * LinesDisplayed + 20;
            lineWidth = (int)(consoleXSize / font.MeasureString("a").X) - 2; //calculate number of letters that fit on a line, using "a" as example character

            State = ConsoleState.Closed;
            StateStartTime = 0;
            LastKeyState = this.CurrentKeyState = Keyboard.GetState();
            firstInterval = 500f;
            repeatInterval = 50f;

            //used for repeating keystrokes
            keyTimes = new Dictionary<Keys, double>();
            for (int i = 0; i < Enum.GetValues(typeof(Keys)).Length; i++)
            {
                Keys key = (Keys)Enum.GetValues(typeof(Keys)).GetValue(i);
                keyTimes[key] = 0f;
            }

            this.defaultInterpreter = new DefaultInterpreter();
        }

        public void setupInterpreter( ConsoleInterpreter interpreter )
        {
            this.interpreter = interpreter;
            clearBuffer();
            clearHistory();
    
            this.interpreter.showWelcome(this, "");
        }

        public string Chomp(string str)
        {
            if (str.Length > 0 && str.Substring(str.Length - 1, 1) == "\n")
            {
                return str.Substring(0, str.Length - 1);
            }
            return str;
        }

        //check if the key has just been pressed
        private bool IsKeyPressed(Keys key)
        {
            return CurrentKeyState.IsKeyDown(key) && !LastKeyState.IsKeyDown(key);
        }

        public bool ConsoleDark
        {
            set { this.isConsoleDark = value; }
            get { return this.isConsoleDark; }
        }

        //check if a key is pressed, and repeat it at the default repeat rate
        private bool KeyPressWithRepeat(Keys key, double elapsedTime)
        {
            if (CurrentKeyState.IsKeyDown(key))
            {
                if (IsKeyPressed(key)) return true; //if the key has just been pressed, it automatically counts
                keyTimes[key] -= elapsedTime; //count down to next repeat
                double keyTime = keyTimes[key]; //get the time left
                if (keyTimes[key] <= 0) //if the time has run out, repeat the letter
                {
                    keyTimes[key] = repeatInterval; //reset the timer to the repeat interval
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //if the key is not pressed, reset it's time to the first interval, which is usually longer
            else
            {
                keyTimes[key] = firstInterval;
                return false;
            }
        }

        private string GetStringFromKeyState(double elapsedTime)
        {
            bool shiftPressed = CurrentKeyState.IsKeyDown(Keys.LeftShift) || CurrentKeyState.IsKeyDown(Keys.RightShift);
            bool altPressed = CurrentKeyState.IsKeyDown(Keys.LeftAlt) || CurrentKeyState.IsKeyDown(Keys.RightAlt);

            foreach (KeyBinding binding in KeyboardHelper.LHGBindings)
                if (KeyPressWithRepeat(binding.Key, elapsedTime))
                {
                    if (!shiftPressed && !altPressed)
                        return binding.UnmodifiedString;
                    else if (shiftPressed && !altPressed)
                        return binding.ShiftString;
                    else if (!shiftPressed && altPressed)
                        return binding.AltString;
                    else if (shiftPressed && altPressed)
                        return binding.ShiftAltString;
                }

            return "";
        }
    
        private List<string> WrapLine(string line, int columns)
        {
            List<string> wraplines = new List<string>();
            if (line.Length > 0)
            {
                wraplines.Add("");
                int lineNum = 0;

                for (int i = 0; i < line.Length; i++)
                {
                    string ch = line.Substring(i, 1);

                    if (ch == "\n" || wraplines[lineNum].Length > columns)
                    {
                        wraplines.Add("");
                        lineNum++;
                    }
                    else
                    {
                        wraplines[lineNum] += ch;
                    }
                }
            }

            return wraplines;
        }

        private List<string> WrapLines(string[] lines, int columns)
        {
            List<string> wraplines = new List<string>();
            foreach (string line in lines)
            {
                wraplines.AddRange(WrapLine(line, columns));
            }
            return wraplines;
        }

        public void Write(string str)
        {
            OutputBuffer += str;
        }

        public void WriteLine(string str)
        {
            Write(str+NewLine);
        }
      
        public void clearBuffer()
        {
            OutputBuffer = "";
        }

        public void clearHistory()
        {
            history.Clear();
        }

        public void Prompt(string str)
        {
            Write(str);
            string[] lines = WrapLine(OutputBuffer, lineWidth).ToArray();
            cursorOffset = lines[lines.Length-1].Length;
        }

        public override void Update(GameTime gameTime)
        {
            double now = gameTime.TotalGameTime.TotalSeconds;
            double elapsedTime = gameTime.ElapsedGameTime.TotalMilliseconds; //time since last update call

            //get keyboard state
            LastKeyState = CurrentKeyState;
            CurrentKeyState = Keyboard.GetState();

            if (State == ConsoleState.Closing)
            {
                if (now - StateStartTime > AnimationTime)
                {
                    State = ConsoleState.Closed;
                    StateStartTime = now;
                }

                return;
            }

            if (State == ConsoleState.Opening)
            {
                if (now - StateStartTime > AnimationTime)
                {
                    State = ConsoleState.Open;
                    StateStartTime = now;
                }

                return;
            }

            if (State == ConsoleState.Closed)
            {
                if (IsKeyPressed(Keys.OemTilde)) //this opens the console
                {
                    State = ConsoleState.Opening;
                    StateStartTime = now;
                    this.Visible = true;
                }
                else
                {
                    return;
                }
            }

            if (State == ConsoleState.Open)
            {
                if (IsKeyPressed(Keys.OemTilde))
                {
                    System.Console.WriteLine("Console open ~ was pressed");

                    State = ConsoleState.Closing;
                    StateStartTime = now;
                    return;
                }

                //execute current line with the interpreter
                if (IsKeyPressed(Keys.Enter))
                {
                    if (InputBuffer.Length > 0)
                    {
                        history.Add(InputBuffer); //add command to history
                    }
                    WriteLine(InputBuffer);
                    
                    // Parse the input.  Send to the current interpreter
                    String line = InputBuffer;
                    line = line.Trim();
                    if (line.Length > 0)
                    {
                        line = line.ToLower();
                        String[] arguments = line.Split(' ');
                        this.interpreter.parse(this, arguments);
                    }

                    InputBuffer = "";
                    cursorPos = 0;
                }
                //erase previous letter when backspace is pressed
                if (KeyPressWithRepeat(Keys.Back, elapsedTime))
                {
                    if (cursorPos > 0)
                    {
                        InputBuffer = InputBuffer.Remove(cursorPos-1, 1);
                        cursorPos--;
                    }
                }
                //delete next letter when delete is pressed
                if (KeyPressWithRepeat(Keys.Delete, elapsedTime))
                {
                    if (InputBuffer.Length != 0)
                        InputBuffer = InputBuffer.Remove(cursorPos, 1);
                }
                //cycle backwards through the command history
                if (KeyPressWithRepeat(Keys.Up, elapsedTime))
                {
                    InputBuffer = history.Previous();
                    cursorPos = InputBuffer.Length;
                }
                //cycle forwards through the command history
                if (KeyPressWithRepeat(Keys.Down, elapsedTime))
                {
                    InputBuffer = history.Next();
                    cursorPos = InputBuffer.Length;
                }
                //move the cursor to the right
                if (KeyPressWithRepeat(Keys.Right, elapsedTime) && cursorPos != InputBuffer.Length)
                {
                    cursorPos++;
                }
                //move the cursor left
                if (KeyPressWithRepeat(Keys.Left, elapsedTime) && cursorPos > 0)
                {
                    cursorPos--;
                }
                //move the cursor to the beginning of the line
                if (IsKeyPressed(Keys.Home))
                {
                    cursorPos = 0;
                }
                //move the cursor to the end of the line
                if (IsKeyPressed(Keys.End))
                {
                    cursorPos = InputBuffer.Length;
                }
                //get a letter from input
                string nextChar = GetStringFromKeyState(elapsedTime);

                //only add it if it isn't null
                if (nextChar != "")
                {
                    //if the cursor is at the end of the line, add the letter to the end
                    if (InputBuffer.Length == cursorPos)
                    {
                        if (nextChar.Equals(", "))
                            nextChar = ",";

                        InputBuffer += nextChar;
                    }
                    //otherwise insert it where the cursor is
                    else
                    {
                        InputBuffer = InputBuffer.Insert(cursorPos, nextChar);
                    }
                    cursorPos += nextChar.Length;
                }
            }
            
        }

        public List<string> Render(string output)
        {
            List<string> lines = WrapLine(output, lineWidth);
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i].Replace("\t", "    ");
            }
            lines.Reverse();
            return lines;
        }

        public string DrawCursor(double now)
        {
            int spaces = (InputBuffer.Length > 0 && cursorPos > 0) ? 
                Render(InputBuffer.Substring(0, cursorPos))[0].Length + cursorOffset :
                cursorOffset;
            return new String(' ', spaces) + (((int)(now / CursorBlinkTime) % 2 == 0) ? "_" : "");
        }

        public override void Draw(GameTime gameTime)
        {
            //don't draw the console if it's closed
            if (State == ConsoleState.Closed)
                return;

            double now = gameTime.TotalGameTime.TotalSeconds;

            //get console dimensions
            consoleXSize = this.Game.Window.ClientBounds.Right - this.Game.Window.ClientBounds.Left - 20;
            consoleYSize = this.font.LineSpacing * LinesDisplayed + 20;

            //set the offsets 
            int consoleXOffset = 10;
            int consoleYOffset = 10;

            //run the opening animation
            if (State == ConsoleState.Opening)
            {
                int startPosition = 0 - consoleYOffset - consoleYSize;
                int endPosition = consoleYOffset;
                consoleYOffset = (int)MathHelper.Lerp(startPosition, endPosition, (float)(now - StateStartTime) / (float)AnimationTime);
            }
            //run the closing animation
            else if (State == ConsoleState.Closing)
            {
                int startPosition = consoleYOffset;
                int endPosition = 0 - consoleYOffset - consoleYSize;
                consoleYOffset = (int)MathHelper.Lerp(startPosition, endPosition, (float)(now - StateStartTime) / (float)AnimationTime);
            }
            //calculate the number of letters that fit on a line
            this.lineWidth = (int)(consoleXSize / font.MeasureString("a").X) - 2; //remeasure lineWidth, incase the screen size changes

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //spriteBatch.Begin(0, BlendState.Opaque);
            byte tint = 125;
            if (isConsoleDark)
                tint = 255;

            background.SetData<Color>(new Color[1] { new Color(0, 0, 0, tint) });
            spriteBatch.Draw(background, new Rectangle(consoleXOffset, consoleYOffset, consoleXSize, consoleYSize), Color.White);

            string cursorString = DrawCursor(now);

            spriteBatch.DrawString(font, cursorString, new Vector2(consoleXOffset + 10, consoleYOffset + consoleYSize - 10 - font.LineSpacing), Color.White);

            int j = 0;
            List<string> lines = Render(OutputBuffer + InputBuffer); //show them in the proper order, because we're drawing from the bottom
            foreach (string str in lines)
            {
                //draw each line at an offset determined by the line height and line count
                j++;
                spriteBatch.DrawString(font, str, new Vector2(consoleXOffset + 10, consoleYOffset + consoleYSize - 10 - font.LineSpacing * (j)), Color.White);
            }

            spriteBatch.End();
            
            //reset depth buffer to normal status, so as not to mess up 3d code            
            lhg.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
