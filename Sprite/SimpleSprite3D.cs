using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using LunchHourGames.Drawing;

namespace LunchHourGames.Sprite
{
    public class SimpleSprite3D : DrawableGameComponent
    {
        protected LunchHourGames lhg;

        protected BasicEffect quadEffect;
        protected VertexDeclaration vertexDeclaration;

        protected Vector3 origin;
        protected Vector3 upperLeft;
        protected Vector3 lowerLeft;
        protected Vector3 upperRight;
        protected Vector3 lowerRight;
        protected Vector3 normal;
        protected Vector3 up;
        protected Vector3 left;

        protected int width = 0;
        protected int height = 0;

        protected VertexPositionNormalTexture[] vertices;
        protected short[] indexes;
      
        Matrix view, projection, world;

        public SimpleSprite3D(LunchHourGames lhg)
            :base(lhg)
        {
            this.lhg = lhg;

            vertices = new VertexPositionNormalTexture[4];
            indexes = new short[6];

            quadEffect = new BasicEffect(lhg.GraphicsDevice);
            quadEffect.LightingEnabled = false;
            
            /*
            quadEffect.LightingEnabled = true; // turn on the lighting subsystem.
            quadEffect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0, 0); // a red light
            quadEffect.DirectionalLight0.Direction = new Vector3(865, 128, 470); 
            quadEffect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0);
            quadEffect.DirectionalLight0.Enabled = true;

            //quadEffect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
            //quadEffect.EmissiveColor = new Vector3(0.2f, 0.2f, 0.2f);
            */

            this.world = Matrix.Identity;
            quadEffect.World = Matrix.Identity;
        }

        public SimpleSprite3D(LunchHourGames lhg, Texture2D texture)
            :this(lhg)
        {
            MyTexture = texture;
            
            width = texture.Width;
            height = texture.Height;
        }

        public SimpleSprite3D(LunchHourGames lhg, Texture2D texture, Vector3 position)
            : this(lhg, texture)
        {
            MyPosition = position;           
        }

        public SimpleSprite3D(LunchHourGames lhg, Texture2D texture, Rectangle3D extents)
            : this(lhg, texture)
        {
            upperLeft = extents.topLeft;
            upperRight = extents.topRight;
            lowerLeft = extents.bottomLeft;
            lowerRight = extents.bottomRight;

            origin = upperLeft;
            normal = upperLeft;

            FillVertices();
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

        public Matrix MyWorld
        {
            get { return this.projection; }
            set
            {
                this.world = value;
                quadEffect.World = value;
            }
        }

        public void setGraphicsMatrices(Matrix view, Matrix projection, Matrix world)
        {
            MyView = view;
            MyProjection = projection;
            MyWorld = world;
        }

        public Vector3 MyPosition
        {
            set
            {
                origin = value;
                normal = value;

                upperLeft = normal;
                upperRight = new Vector3(normal.X + width, normal.Y, normal.Z);
                lowerLeft = new Vector3(normal.X, normal.Y - height, normal.Z);
                lowerRight = new Vector3(normal.X + width, normal.Y - height, normal.Z);

                FillVertices();
            }

            get { return this.normal; }
        }

        public Rectangle3D MyRectangle3D
        {
            get { return new Rectangle3D( upperLeft, upperRight,lowerLeft, lowerRight); }
        }

        public Texture2D MyTexture
        {
            get { return quadEffect.Texture; }
            set
            {
                if (quadEffect.Texture != value)
                {
                    quadEffect.Texture = value;
                    quadEffect.TextureEnabled = true;
                }
            }
        }

        protected void FillVertices()
        {
            // Fill in texture coordinates to display full texture
            // on quad
            Vector2 textureUpperLeft = new Vector2(0.0f, 0.0f);
            Vector2 textureUpperRight = new Vector2(1.0f, 0.0f);
            Vector2 textureLowerLeft = new Vector2(0.0f, 1.0f);
            Vector2 textureLowerRight = new Vector2(1.0f, 1.0f);

            // Provide a normal for each vertex
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal = normal;
            }

            // Set the position and texture coordinate for each
            // vertex
            vertices[0].Position = lowerLeft;
            vertices[0].TextureCoordinate = textureLowerLeft;
            vertices[1].Position = upperLeft;
            vertices[1].TextureCoordinate = textureUpperLeft;
            vertices[2].Position = lowerRight;
            vertices[2].TextureCoordinate = textureLowerRight;
            vertices[3].Position = upperRight;
            vertices[3].TextureCoordinate = textureUpperRight;

            // Set the index buffer for each vertex, using
            // clockwise winding
            indexes[0] = 0;
            indexes[1] = 1;
            indexes[2] = 2;
            indexes[3] = 2;
            indexes[4] = 1;
            indexes[5] = 3;

            /*
            vertexDeclaration = new VertexDeclaration(new VertexElement[]
                {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                    new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
                });
             * */
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // If we’re using the Reach graphics profile, We need to do add the line
            // The reason we need to do this is to change the texture address mode to Clamp 
            // to allow using texture sizes that are not powers of two.
            lhg.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

            foreach (EffectPass pass in quadEffect.CurrentTechnique.Passes)
            {
                pass.Apply();


                lhg.GraphicsDevice.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList,
                    vertices, 0, 4,
                    indexes, 0, 2);
            }

            base.Draw(gameTime);
        }
    }
}
