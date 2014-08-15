

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;

namespace LunchHourGames.Drawing
{
    public class LHGGrid : IDisposable
    {
        private LunchHourGames lhg;

        protected List<VertexPositionColor> vertices = new List<VertexPositionColor>();

        // hasBegun is flipped to true once Begin is called, and is used to make
        // sure users don't call End before Begin is called.
        protected bool hasBegun = false;

        protected Color gridColor;
        protected bool isDisposed;       

        // Rendering
        protected VertexBuffer vertexBuffer;
        protected int vertexCount;
        protected int primitiveCount;
        protected BasicEffect effect;
        protected GraphicsDevice device;

        public LHGGrid(LunchHourGames lhg)
        {
            this.lhg = lhg;            
            gridColor = Color.CadetBlue;
        }

        public void UnloadGraphicsContent()
        {
            if (this.vertexBuffer != null)
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

        public void LoadGraphicsContent(GraphicsDevice graphicsDevice)
        {
            device = graphicsDevice;
            effect = new BasicEffect(device);
            effect.VertexColorEnabled = true;
            effect.LightingEnabled = false;
            setGraphicsMatrices(Matrix.Identity, Matrix.Identity, Matrix.Identity);         
        }

        public Color MyColor
        {
            get { return gridColor; }
            set { gridColor = value; }
        }
      
        public Matrix ProjectionMatrix
        {
            get { return effect.Projection;  }
            set { effect.Projection = value; }
        }
      
        public Matrix WorldMatrix
        {
            get { return effect.World; }
            set { effect.World = value; }
        }
        
        public Matrix ViewMatrix
        {
            get { return effect.View; }
            set { effect.View = value; }
        }

        public void setGraphicsMatrices(Matrix view, Matrix projection, Matrix world)
        {
            effect.View = view;
            effect.Projection = projection;
            effect.World = world;
        }

        // Begin is called to tell the PrimitiveBatch what kind of primitives will be
        // drawn, and to prepare the graphics card to render those primitives.
        public void Begin()
        {
            if (hasBegun)
            {
                throw new InvalidOperationException
                    ("End must be called before Begin can be called again.");
            }

            vertices.Clear();

            // flip the error checking boolean. It's now ok to call AddVertex, Flush,
            // and End.
            hasBegun = true;
        }

        /*
         * 
         *        public void createSquareGrid()
        {
            int gridSize = 16;
            float gridScale = 32.0f;
            int gridSize1 = gridSize + 1;
            float length = (float)gridSize * gridScale;
            float halfLength = length * 0.5f;           

            for (int i = 0; i < gridSize1; ++i)
            {
                AddVertex(new Vector3(-halfLength, 0.0f, i * gridScale - halfLength), this.gridColor);
                AddVertex(new Vector3(halfLength, 0.0f, i * gridScale - halfLength), this.gridColor);
                AddVertex(new Vector3(i * gridScale - halfLength, 0.0f, -halfLength), this.gridColor);
                AddVertex(new Vector3(i * gridScale - halfLength, 0.0f, halfLength), this.gridColor);
            }
        }
         */

        public void AddVertex(Vector2 vertex, Color color)
        {
            AddVertex(new Vector3(vertex, 0), color);
        }

        // AddVertex is called to add another vertex to be rendered. To draw a point,
        // AddVertex must be called once. for lines, twice, and for triangles 3 times.
        // this function can only be called once begin has been called.
        // if there is not enough room in the vertices buffer, Flush is called
        // automatically.
        public void AddVertex(Vector3 vertex, Color color)
        {
            if (!hasBegun)
            {
                throw new InvalidOperationException
                    ("Begin must be called before AddVertex can be called.");
            }
           
            // once we know there's enough room, set the vertex in the buffer,
            // and increase position.
            vertices.Add(new VertexPositionColor(vertex, color));        
        }

        // End is called once all the primitives have been drawn using AddVertex.      
        public void End()
        {
            this.vertexCount = vertices.Count;
            this.primitiveCount = vertexCount / 2;

            this.vertexBuffer = new VertexBuffer(device, typeof(VertexPositionColor),
                                                 this.vertexCount,
                                                 BufferUsage.WriteOnly);
            this.vertexBuffer.SetData<VertexPositionColor>(vertices.ToArray());

            hasBegun = false;
        }

        ~LHGGrid()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    //if we're manually disposing,
                    //then managed content should be unloaded
                    UnloadGraphicsContent();
                }
                isDisposed = true;
            }
        }

        public void Draw()
        {
            device.SetVertexBuffer(this.vertexBuffer);   

            for (int i = 0; i < this.effect.CurrentTechnique.Passes.Count; ++i)
            {
                this.effect.CurrentTechnique.Passes[i].Apply();
                device.DrawPrimitives(PrimitiveType.LineList, 0, this.primitiveCount);
            }
        }
    }
}
