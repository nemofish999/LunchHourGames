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
    public class StagePanel : DrawableGameComponent
    {      
        private LHGStage.StageSection section;
        private LHGStage.StageLocation location;

        private int cellSize;
        private int widthInCells;
        private int heightInCells;

        private Rectangle3D extents;

        private List<SimpleSprite3D> sprites = new List<SimpleSprite3D>();

        public StagePanel(LunchHourGames lhg, LHGStage.StageSection section, LHGStage.StageLocation location, 
            int cellSize, int widthInCells, int heightInCells)
            :base(lhg)
        {
            this.section = section;
            this.location = location;
            this.cellSize = cellSize;
            this.widthInCells = widthInCells;
            this.heightInCells = heightInCells;
            this.extents = Rectangle3D.Zero;
        }

        public void addSprite(SimpleSprite3D sprite)
        {
            sprites.Add(sprite);
        }

        public LHGStage.StageSection MySection
        {
            get { return this.section; }
        }

        public LHGStage.StageLocation MyLocation
        {
            get { return this.location; }
        }

        public int MyCellSize
        {
            get { return this.cellSize; }
        }

        public int MyWidth
        {
            get { return this.widthInCells * cellSize; }
        }

        public int MyHeight
        {
            get { return this.heightInCells * cellSize; }
        }

        public Rectangle3D MyExtents
        {
            get { return this.extents; }
            set { this.extents = value; }
        }

        public Matrix MyWorld
        {
            set
            {
                foreach (SimpleSprite3D sprite in sprites)
                    sprite.MyWorld = value;
            }
        }

        public Matrix MyProjection
        {
            set
            {
                foreach (SimpleSprite3D sprite in sprites)
                    sprite.MyProjection = value;
            }
        }

        public Matrix MyView
        {
            set
            {
                foreach (SimpleSprite3D sprite in sprites)
                    sprite.MyView = value;
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (SimpleSprite3D sprite in sprites)
                sprite.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (SimpleSprite3D sprite in sprites)
                sprite.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
