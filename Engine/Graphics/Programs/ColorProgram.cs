using System;
using Core.Graphics;
using Engine.Graphics.Buffer;
using Engine.Graphics.Handle;
using OpenTK.Graphics.ES20;

namespace Engine.Graphics.Programs {
    public class ColorProgram : TextureProgram {
        public static ColorProgram Program;

        public static void Init ( ) {
            Program = new ColorProgram( );
        }

        public static void Destroy ( ) {
            Program.Dispose( );
        }

        private UniformMatrixHandle mvpMatrixHandle;
        private AttributeHandle colorHandle;

        public ColorProgram ( ) : base(Assets.GetVertexShader("color"), Assets.GetFragmentShader("color")) {
            mvpMatrixHandle = new UniformMatrixHandle(glProgram, "u_mvpmatrix");
            colorHandle = new AttributeHandle(glProgram, "a_color");
        }

        public override void Begin ( ) {
            Debug.CheckGL(this);
            colorHandle.Enable( );
            Debug.CheckGL(this);
            base.Begin( );
        }

        public override void End ( ) {
            colorHandle.Disable( );
            base.End( );
        }

        public void Draw (BufferBatch buffer, Texture2D texture, Matrix matrix, bool alphaBlending = true) {
            Draw(buffer, texture, matrix, buffer.IndexBuffer.Length, 0, alphaBlending);
        }

        public void Draw (BufferBatch buffer, Texture2D texture, Matrix matrix, int count, int offset, bool alphaBlending = true) {
            Draw(buffer.IndexBuffer, buffer.VertexBuffer, buffer.TextureBuffer, buffer.ColorBuffer, texture, matrix, count, offset, alphaBlending);
        }

        public void Draw (IndexBuffer indexbuffer, IAttributeBuffer vertexbuffer, IAttributeBuffer texturebuffer, IAttributeBuffer colorbuffer, Texture2D texture, Matrix matrix, int count, int offset, bool alphablending = true) {
            Debug.CheckGL(this);
            Apply(texture.ID, indexbuffer, vertexbuffer, texturebuffer, alphablending);
            Debug.CheckGL(this);
            colorbuffer.Bind(colorHandle);
            Debug.CheckGL(this);
            mvpMatrixHandle.Set(matrix.MVP);
            Debug.CheckGL(this);

            GL.DrawElements(BeginMode.Triangles, count, DrawElementsType.UnsignedShort, new IntPtr(offset));
            Debug.CheckGL(this);
        }

        public class BufferBatch : IDisposable {
            public IndexBuffer IndexBuffer;
            public IAttributeBuffer VertexBuffer;
            public IAttributeBuffer ColorBuffer;
            public IAttributeBuffer TextureBuffer; 

            public BufferBatch (IndexBuffer indexbuffer, IAttributeBuffer vertexbuffer, IAttributeBuffer colorbuffer, IAttributeBuffer texturebuffer) {
                IndexBuffer = indexbuffer;
                VertexBuffer = vertexbuffer;
                ColorBuffer = colorbuffer;
                TextureBuffer = texturebuffer;
            }


            public void Dispose ( ) {
                IndexBuffer.Dispose( );
                VertexBuffer.Dispose( );
                TextureBuffer.Dispose( );
                ColorBuffer.Dispose( );
            }
        }
    }
}
