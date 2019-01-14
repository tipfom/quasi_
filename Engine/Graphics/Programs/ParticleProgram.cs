﻿using System;
using Engine.Graphics.Buffer;
using Engine.Graphics.Handle;
using OpenTK.Graphics.ES20;

namespace Engine.Graphics.Programs
{
    public class ParticleProgram : Program {
        public static ParticleProgram Program;

        public static void Init ( ) {
            Program = new ParticleProgram( );
        }

        public static void Destroy ( ) {
            Program.Dispose( );
        }

        private AttributeHandle sizeHandle;
        private AttributeHandle colorHandle;
        private UniformMatrixHandle matrixHandle;

        public ParticleProgram () : base(Assets.GetVertexShader("particle"), Assets.GetFragmentShader("particle")) {
            sizeHandle = new AttributeHandle(glProgram, "a_size");
            colorHandle = new AttributeHandle(glProgram, "a_color");
            matrixHandle = new UniformMatrixHandle(glProgram, "u_mvpmatrix");
        }

        public override void Begin ( ) {
            base.Begin( );
            sizeHandle.Enable( );
            colorHandle.Enable( );
        }

        public override void End ( ) {
            sizeHandle.Disable( );
            colorHandle.Disable( );
            base.End( );
        }

        public void Draw (BufferBatch batch, bool alphablending = true) {
            Draw(batch.VertexBuffer, batch.SizeBuffer, batch.ColorBuffer, batch.SizeBuffer.Length, alphablending);
        }

        public void Draw (BufferBatch batch, int count, bool alphablending = true) {
            Draw(batch.VertexBuffer, batch.SizeBuffer, batch.ColorBuffer, count, alphablending);
        }

        public void Draw (IAttributeBuffer vertexbuffer, IAttributeBuffer sizebuffer, IAttributeBuffer colorbuffer, int count, bool alphablending = true) {
            Apply(vertexbuffer, alphablending);
            sizebuffer.Bind(sizeHandle);
            colorbuffer.Bind(colorHandle);
            // matrixHandle.Set(Emitter.Matrix.MVP);

            GL.DrawArrays(BeginMode.Points, 0, count);
        }

        public class BufferBatch : IDisposable {
            public IAttributeBuffer VertexBuffer;
            public IAttributeBuffer SizeBuffer;
            public IAttributeBuffer ColorBuffer;

            public BufferBatch(IAttributeBuffer vertexbuffer, IAttributeBuffer sizebuffer, IAttributeBuffer colorbuffer) {
                VertexBuffer = vertexbuffer;
                SizeBuffer = sizebuffer;
                ColorBuffer = colorbuffer;
            }


            public void Dispose ( ) {
                ColorBuffer.Dispose( );
                VertexBuffer.Dispose( );
            }
        }
    }
}
