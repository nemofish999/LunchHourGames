using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LunchHourGames.Drawing
{
    class DrawCircle : DrawableGameComponent
    {
        private GraphicsDevice device;
        private VertexBuffer vertexBuffer;
        private BasicEffect effect;
        public Matrix worldMatrix;
        private int vertexCount;

        public DrawCircle(LunchHourGames lhg, int vertexCount)
            : base(lhg)
        {
            device = lhg.GraphicsDevice;
            this.vertexCount = vertexCount;
            worldMatrix = Matrix.Identity;
        }

        public VertexBuffer CreateCircle()
        {
            /*
            VertexPositionColor[] vertices = new VertexPositionColor[vertexCount];
            Vector3 radius = new Vector3(4, 0, 0);
            Vector3 axis = new Vector3(0, 0, -1);
            Vector3 position;
            for (int i = 0; i < vertexCount; i++)
            {
                position = Vector3.Transform(radius,Matrix.CreateFromAxisAngle(axis, MathHelper.ToRadians(360f/(vertexCount-1)*i)));
                vertices[i] = new VertexPositionColor(position, Color.Black);
            }
            vertexBuffer = new VertexBuffer(device, vertexCount * VertexPositionColor.SizeInBytes, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vertices);

             * */
            return null;
        }

        protected override void LoadContent()
        {
            /*
            // Create the effect that will be used to draw the axis
            effect = new BasicEffect(device, null);

            // Calculate the effect aspect ratio, projection, and view matrix
            float aspectRatio = (float)device.Viewport.Width / device.Viewport.Height;
            effect.View = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 10.0f), Vector3.Zero,
            Vector3.Up);
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 100.0f);
            effect.LightingEnabled = false;

            // Create the 3-D axis
            CreateCircle();
             */
        }

        protected override void UnloadContent()
        {
            if (vertexBuffer != null)
            {
                vertexBuffer.Dispose();
                vertexBuffer = null;
            }

            if (effect != null)
            {
                effect.Dispose();
                effect = null;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            /*
            // Create a vertex declaration to be used when drawing the vertices
            device.VertexDeclaration = new VertexDeclaration(device, VertexPositionColor.VertexElements);
            // Set the vertex source
            device.Vertices[0].SetSource(vertexBuffer, 0, VertexPositionColor.SizeInBytes);
            effect.World = worldMatrix;

            // Draw the 3-D axis
            effect.Begin();
            foreach (EffectPass CurrentPass in effect.CurrentTechnique.Passes)
            {
                CurrentPass.Begin();
                //We are drawing 22 vertices, grouped in 11 lines
                device.DrawPrimitives(PrimitiveType.LineStrip, 0, vertexCount-1);
                CurrentPass.End();
            }
            effect.End();
             */
        }
    }
}