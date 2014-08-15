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
using System.Collections.Specialized;


namespace LunchHourGames.Screen
{
    public class ButtonMenuItem
    {
        public String name;
        public Rectangle extents;
    }

    public interface ButtonMenuEvent
    {
        void userSelectedItem(ButtonMenuItem menuItem);
    }
 
    public class ButtonMenu : DrawableGameComponent
    {
    
        SpriteFont spriteFont;
        SpriteBatch spriteBatch;
        Texture2D buttonImage;

        Color normalColor = Color.Black;
        Color hiliteColor = Color.White;

        Vector2 position = new Vector2();
        int selectedIndex = 0;
        int possibleIndex = -1;
        private int spacer; 
        int width, height;

        private MouseState mouseStateCurrent;
        private MouseState mouseStatePrevious;
        private MouseState mouseNoState;

        private List<ButtonMenuItem> menuItems = new List<ButtonMenuItem>();

        private ButtonMenuEvent eventHandler;

        public ButtonMenu(LunchHourGames lhg, SpriteFont spriteFont, Texture2D buttonImage, int spacer, ButtonMenuEvent eventHandler)
            : base(lhg)
        {
            this.spriteBatch = lhg.MySpriteBatch;
            this.spriteFont = spriteFont;
            this.buttonImage = buttonImage;
            this.spacer = spacer;
            this.eventHandler = eventHandler;
            this.width = buttonImage.Width;           
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public int Spacer
        {
            get { return this.spacer; }
            set { this.spacer = value; }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = (int)MathHelper.Clamp(
                        value,
                        0,
                        menuItems.Count - 1);
            }
        }

        public void resetMenu()
        {
            mouseStateCurrent = mouseNoState;
            mouseStatePrevious = mouseNoState;
            selectedIndex = 0;
            possibleIndex = -1;
        }

        public ButtonMenuItem getButtonMenuItem(int index)
        {
            int size = menuItems.Count;
            if ( size > 0 && (index >=0 && index < size) )
                return menuItems[index];

            return null;
        }
        
        public Color NormalColor
        {
            get { return normalColor; }
            set { normalColor = value; }
        }

        public Color HiliteColor
        {
            get { return hiliteColor; }
            set { hiliteColor = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set 
            { 
                position = value;
                int x = (int)position.X;
                int y = (int)position.Y;

                if ( x < 0 )
                    x = 0;
                
                if (y < 0)
                    y = 0;

                foreach (ButtonMenuItem menuItem in menuItems)
                {
                    menuItem.extents = new Rectangle(x, y, buttonImage.Width, buttonImage.Height);

                    y += buttonImage.Height;
                    y += this.spacer;
                }
            }
        }

        public void SetMenuItems(string[] items)
        {
            menuItems.Clear();

            height = 0;
            foreach (string item in items)
            {
                ButtonMenuItem menuItem = new ButtonMenuItem();
                menuItem.name = item;
                height += spacer;
                height += buttonImage.Height;
                menuItems.Add(menuItem);
            }
        }
        
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            mouseStateCurrent = Mouse.GetState();
            handleMouseMove(mouseStateCurrent.X, mouseStateCurrent.Y);

            if (mouseStateCurrent.LeftButton == ButtonState.Pressed)
            {
                int index = handleMouseClick(mouseStateCurrent.X, mouseStateCurrent.Y);
                if (index > -1)
                    possibleIndex = index;
            }

            if (mouseStateCurrent.LeftButton == ButtonState.Released)
            {
                int index = handleMouseClick(mouseStateCurrent.X, mouseStateCurrent.Y);
                if (index > -1 && possibleIndex == index)
                {
                    // We released the mouse button over the same item we clicked on, so that means we
                    // are selecting that item.
                    SelectedIndex = index;
                        
                    // tell our parent that the user selected an item
                    eventHandler.userSelectedItem(getButtonMenuItem(SelectedIndex));
                }
            }

            /*
            if (mouseStateCurrent.LeftButton == ButtonState.Pressed &&
                mouseStatePrevious.LeftButton == ButtonState.Released)
            {
                int index = handleMouseClick(mouseStateCurrent.X, mouseStateCurrent.Y);
                if (index > -1)
                {
                    SelectedIndex = index;

                    // tell our parent that the user selected an item
                    eventHandler.userSelectedItem(getButtonMenuItem(SelectedIndex));
                }
            }
             */

            mouseStatePrevious = mouseStateCurrent;

            if (LunchHourGames.CheckKey(Keys.Down))
            {
                selectedIndex++;

                if (selectedIndex == menuItems.Count)
                    selectedIndex = 0;
            }

            if (LunchHourGames.CheckKey(Keys.Up))
            {
                selectedIndex--;
                if (selectedIndex == -1)
                {
                    selectedIndex = menuItems.Count - 1;
                }
            }
            
            base.Update(gameTime);
        }

        public int isPointOnMenu(int x, int y)
        {
            int index = 0;
            foreach ( ButtonMenuItem menuItem in menuItems )
            {
                if (menuItem.extents.Contains(x, y))
                {
                    return index;
                }

                index++;               
            }


            return -1;
        }

        public void handleMouseMove(int x, int y)
        {
            int index = isPointOnMenu(x, y);
            if (index > -1)
                SelectedIndex = index;
        }

        public int handleMouseClick(int x, int y)
        {
            return isPointOnMenu(x, y);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                Vector2 textPosition = Position;
                Color myColor;
                int i = 0;
                foreach ( ButtonMenuItem menuItem in menuItems )
                {
                    if (i == SelectedIndex)
                        myColor = HiliteColor;
                    else
                        myColor = NormalColor;

                    Rectangle buttonRectangle = menuItem.extents;
                    spriteBatch.Draw(buttonImage, buttonRectangle, Color.White);

                    textPosition = new Vector2(
                        buttonRectangle.X + (buttonImage.Width / 2),
                        buttonRectangle.Y + (buttonImage.Height / 2));

                    Vector2 textSize = spriteFont.MeasureString(menuItem.name);
                    textPosition.X -= textSize.X / 2;
                    textPosition.Y -= spriteFont.LineSpacing / 2;

                    spriteBatch.DrawString(spriteFont,
                        menuItem.name,
                        textPosition,
                        myColor);

                    i++;
                }


                base.Draw(gameTime);
            }
        }
    }
}