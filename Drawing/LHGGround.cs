using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace LunchHourGames.Drawing
{
    public class LHGGround : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private LunchHourGames lhg;

        Texture2D texture;
        BasicEffect quadEffect;

        Quad quad;
        VertexDeclaration vertexDeclaration;
        Matrix view, projection;

        public LHGGround(LunchHourGames lhg)
            : base(lhg)
        {
            this.lhg = lhg;
        }             

        public override void Initialize()
        {
            quad = new Quad(Vector3.Zero, Vector3.Backward, Vector3.Up, 1, 1);
            view = Matrix.CreateLookAt(new Vector3(0, 0, 2), Vector3.Zero, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 4.0f / 3.0f, 1, 500);
          
            texture = lhg.Content.Load<Texture2D>("Backgrounds/Glass");
            quadEffect = new BasicEffect(lhg.GraphicsDevice);
            //quadEffect.EnableDefaultLighting();

            quadEffect.World = Matrix.Identity;
            quadEffect.View = view;
            quadEffect.Projection = projection;
            quadEffect.TextureEnabled = true;
            quadEffect.Texture = texture;

            vertexDeclaration = new VertexDeclaration(new VertexElement[]
            {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
            }
             );

            base.Initialize();
        }
        
        public Matrix MyView
        {
            get { return this.view; }
            set
            { 
                this.view = value;
                quadEffect.View = view;           
            }
        }

        public Matrix MyProjection
        {
            get { return this.projection; }
            set 
            { 
                this.projection = value;
                quadEffect.Projection = value;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (EffectPass pass in quadEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList,
                    quad.Vertices, 0, 4,
                    quad.Indexes, 0, 2);
            }

            base.Draw(gameTime);
        }
    }
}
