//////// ///////////////////////////////////////////////////////////////////////////////////
// LHGStage.cs
//
// This class maintains a 3D graphical stage for placing objects in a scene.
// The stage is broken into six panels that represnt simple rectangular grids.
// The art designer can place sprites on the stage by giving the stage location
// and a simple row, col coordinates instead of complex 3D coordinates.
//
// The main game play area is all performed in the center panel.
// The surrounding panels are used for art and other story elements.
// This design allows you to easily grow or shrink the game play area as the
// needed and surrounding panels will accomodate the change.
//
// The panels can be different grid sizes as needed.
//
//
//       --------------------------------------------------------------------------
//       |                     |                        |                         |
//       |       Left          |        Center          |        Right            |
//       |     Background      |      Background        |      Background         |
//       |                     |                        |                         |
//       |                     |                        |                         |
//       |                     |                        |                         |
//       --------------------------------------------------------------------------
//      /                      /                        \                         \
//     /       Left           /         Center           \         Right           \
//    /       Floor          /          Floor             \        Floor            \
//   /                      /       (Game Play Area)       \                         \
//  /                      /                                \                         \
//  ------------------------------------------------------------------------------------
//
//
//
// Copyright (C) 2011 Lunch Hour Games. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames.Sprite;
using LunchHourGames.Drawing;

namespace LunchHourGames.Stage
{
    public class LHGStage : DrawableGameComponent
    {
        private LunchHourGames lhg;

        public enum StageSection { Left, Center, Right }
        public enum StageLocation { Background, Floor }

        public StagePanel leftBackground;
        public StagePanel leftFloor;
        public StagePanel centerBackground;
        public StagePanel centerFloor;
        public StagePanel rightBackground;
        public StagePanel rightFloor;

        private int cellSize;
        private int backgroundHeight;
        private int floorHeight;
        private int leftWidth;
        private int centerWidth;
        private int rightWidth;

        public LHGStage(LunchHourGames lhg, int cellSize, int backgroundHeight, int floorHeight,
                        int leftWidth, int centerWidth, int rightWidth)
            :base(lhg)
        {
            this.lhg = lhg;
            this.cellSize = cellSize;
            this.backgroundHeight = backgroundHeight;
            this.floorHeight = floorHeight;
            this.leftWidth = leftWidth;
            this.centerWidth = centerWidth;
            this.rightWidth = rightWidth;

            createSection(StageSection.Left);
            createSection(StageSection.Center);
            createSection(StageSection.Right);
            calculatePanelExtents();
        }

        private void createSection(StageSection section)
        {
            switch (section)
            {
                case StageSection.Left:
                    leftBackground = new StagePanel(lhg, section, StageLocation.Background,cellSize, leftWidth, backgroundHeight);
                    leftFloor = new StagePanel(lhg, section, StageLocation.Floor, cellSize, leftWidth, floorHeight);
                    break;

                case StageSection.Center:
                    centerBackground = new StagePanel(lhg, section, StageLocation.Background, cellSize, centerWidth, backgroundHeight);
                    centerFloor = new StagePanel(lhg, section, StageLocation.Floor, cellSize, centerWidth, floorHeight);
                    break;

                case StageSection.Right:
                    rightBackground = new StagePanel(lhg, section, StageLocation.Background, cellSize, centerWidth, backgroundHeight);
                    rightFloor = new StagePanel(lhg, section, StageLocation.Floor, cellSize, centerWidth, floorHeight);
                    break;
            }
        }

        private void calculatePanelExtents()
        {
            // Here we are going to calculate where all the stage panels will go in 3D space.
            // The center will start at the origin (0,0,0) and the other panels will be around it

            // Center Background;
            Vector3 topLeft = new Vector3(0.0f, centerBackground.MyHeight, 0.0f);
            Vector3 topRight = new Vector3(centerBackground.MyWidth, centerBackground.MyHeight, 0.0f);
            Vector3 bottomLeft = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 bottomRight = new Vector3(centerBackground.MyWidth, 0.0f, 0.0f);
            centerBackground.MyExtents = new Rectangle3D(topLeft, topRight, bottomLeft, bottomRight);
            
            // Center Floor
            topLeft = new Vector3(0.0f, 0.0f, 0.0f);
            topRight = new Vector3(centerFloor.MyWidth, 0.0f, 0.0f);
            bottomLeft = new Vector3(0.0f, 0.0f, centerFloor.MyHeight);
            bottomRight = new Vector3(centerFloor.MyWidth, 0.0f, centerFloor.MyHeight);
            centerFloor.MyExtents = new Rectangle3D(topLeft, topRight, bottomLeft, bottomRight);

            // Left Background
            topLeft = new Vector3(0.0f - leftBackground.MyWidth, leftBackground.MyHeight, 0.0f);
            topRight = new Vector3(0.0f, leftBackground.MyHeight, 0.0f);
            bottomLeft = new Vector3(0.0f - leftBackground.MyWidth, 0.0f, 0.0f);
            bottomRight = new Vector3(0.0f, 0.0f, 0.0f);
            leftBackground.MyExtents = new Rectangle3D(topLeft, topRight, bottomLeft, bottomRight);

            // Left Floor
            topLeft = new Vector3(0.0f - leftFloor.MyWidth, 0.0f, 0.0f);
            topRight = new Vector3(0.0f, 0.0f, 0.0f);
            bottomLeft = new Vector3(0.0f - leftFloor.MyWidth, 0.0f, leftFloor.MyHeight);
            bottomRight = new Vector3(0.0f, 0.0f, leftFloor.MyHeight);
            leftFloor.MyExtents = new Rectangle3D(topLeft, topRight, bottomLeft, bottomRight);

            // Right Background
            topLeft = new Vector3(centerFloor.MyWidth, rightBackground.MyHeight, 0.0f);
            topRight = new Vector3(centerFloor.MyWidth + rightBackground.MyWidth, rightBackground.MyHeight, 0.0f);
            bottomLeft = new Vector3(centerFloor.MyWidth, 0.0f, 0.0f);
            bottomRight = new Vector3(centerFloor.MyWidth + rightBackground.MyWidth, 0.0f, 0.0f);
            rightBackground.MyExtents = new Rectangle3D(topLeft, topRight, bottomLeft, bottomRight);
      
            // Right Floor
            topLeft = new Vector3(centerFloor.MyWidth, 0.0f, 0.0f);
            topRight = new Vector3(centerFloor.MyWidth + rightFloor.MyWidth, 0.0f, 0.0f);
            bottomLeft = new Vector3(centerFloor.MyWidth, 0.0f, rightFloor.MyHeight);
            bottomRight = new Vector3(centerFloor.MyWidth + rightFloor.MyWidth, 0.0f, rightFloor.MyHeight);
            rightFloor.MyExtents = new Rectangle3D(topLeft, topRight, bottomLeft, bottomRight);
        }

        private Rectangle3D getTextureExtents(StagePanel panel, Texture2D texture, int i, int j, int zOrder)
        {
            Rectangle3D panelExtents = panel.MyExtents;

            float xOffset = panelExtents.topLeft.X + (i * panel.MyCellSize);
            float yOffset = 0.0f;
            float zOffset = 0.0f;

            Vector3 bottomLeft;
            Vector3 bottomRight;

            if (panel.MyLocation == StageLocation.Background)
            {    
                yOffset = panelExtents.topLeft.Y - (j * panel.MyCellSize);
                zOffset = 0.0f - zOrder;
                bottomLeft = new Vector3(xOffset, yOffset - texture.Height, zOffset);
                bottomRight = new Vector3(xOffset + texture.Width, yOffset - texture.Height, zOffset);
            }
            else
            {
                yOffset = 0.0f;
                zOffset = panelExtents.topRight.Z + (j * panel.MyCellSize);
                bottomLeft = new Vector3(xOffset, yOffset, zOffset + texture.Height);
                bottomRight = new Vector3(xOffset + texture.Width, yOffset, zOffset + texture.Height);
            }

            Vector3 topLeft = new Vector3(xOffset, yOffset, zOffset);
            Vector3 topRight = new Vector3(xOffset + texture.Width, yOffset, zOffset);

            return new Rectangle3D(topLeft, topRight, bottomLeft, bottomRight);
        }

        private StagePanel getPanel(StageSection section, StageLocation location)
        {
            switch (section)
            {
                case StageSection.Left:
                    if ( location == StageLocation.Background )
                        return leftBackground;
                    else
                        return leftFloor;

                case StageSection.Center:
                    if ( location == StageLocation.Background )
                        return centerBackground;
                    else
                        return centerFloor;

                case StageSection.Right:
                    if ( location == StageLocation.Background )
                        return rightBackground;
                    else
                        return rightFloor;
            }

            return null;
        }

        public void addBackground(StageSection section, string textureName, int i, int j, int zOrder)
        {
            StagePanel panel = getPanel(section, StageLocation.Background);
            Texture2D texture = lhg.Assets.loadTexture(textureName);
            Rectangle3D textureExtents = getTextureExtents(panel, texture, i, j, zOrder);
            SimpleSprite3D sprite = new SimpleSprite3D(lhg, texture, textureExtents);
            panel.addSprite(sprite);
        }

        public void addFloor(StageSection section, string textureName, int i, int j)
        {
            StagePanel panel = getPanel(section, StageLocation.Floor);
            Texture2D texture = lhg.Assets.loadTexture(textureName);
            Rectangle3D textureExtents = getTextureExtents(panel, texture, i, j, 0);
            SimpleSprite3D sprite = new SimpleSprite3D(lhg, texture, textureExtents);
            panel.addSprite(sprite);
        }

        public Matrix MyWorld
        {
            set
            {
                leftBackground.MyWorld = value;
                leftFloor.MyWorld = value;
                centerBackground.MyWorld = value;
                centerFloor.MyWorld = value;
                rightBackground.MyWorld = value;
                rightFloor.MyWorld = value;
            }
        }

        public Matrix MyProjection
        {
            set
            {
                leftBackground.MyProjection = value;
                leftFloor.MyProjection = value;
                centerBackground.MyProjection = value;
                centerFloor.MyProjection = value;
                rightBackground.MyProjection = value;
                rightFloor.MyProjection = value;
            }
        }

        public Matrix MyView
        {
            set
            {
                leftBackground.MyView = value;
                leftFloor.MyView = value;
                centerBackground.MyView = value;
                centerFloor.MyView = value;
                rightBackground.MyView = value;
                rightFloor.MyView = value;
            }
        }

        public override void Update(GameTime gameTime)
        {
            leftBackground.Update(gameTime);
            leftFloor.Update(gameTime);
            centerBackground.Update(gameTime);
            centerFloor.Update(gameTime);
            rightBackground.Update(gameTime);
            rightFloor.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            leftBackground.Draw(gameTime);
            rightBackground.Draw(gameTime);
            centerBackground.Draw(gameTime);

            leftFloor.Draw(gameTime);
            rightFloor.Draw(gameTime);
            centerFloor.Draw(gameTime);      
       
            base.Draw(gameTime);
        }
    }
}
