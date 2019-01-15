using Core;
using Core.Graphics;
using Engine.Graphics.Buffer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Engine.Graphics.Programs.MatrixProgram;

namespace Engine.Graphics
{
    public class TileMap : Core.TileMap
    {
        const int DRAW_WIDTH = 30;

        private BufferBatch mainBuffer, foregroundBuffer;
        private Texture2D texture;
        private Matrix matrix = new Matrix(new Vector2(Window.Ratio, 1));
        private float[][][] layerBuffer;
        private float xFocusCenterMinClamp;
        private float xFocusCenterMaxClamp;
        private float yFocusCenterMinClamp;
        private float yFocusCenterMaxClamp;
        private float xNextTileOffset;
        private float yNextTileOffset;
        private int xViewPortClamp;
        private float xEmitterMatrixOffset;
        private float yEmitterMatrixOffset;
        private int textureBufferL1baseOffset;
        private int textureBufferLength;

        private Vector2 focusCenter;
        private Vector2 drawThreshold;
        private Vector2 updateTile = new Vector2(0, 0);

        private CachedGPUBuffer mainTextureBuffer;
        private CachedGPUBuffer foregroundTextureBuffer;

        public Size DrawSize { get; private set; }
        public float VertexSize { get; private set; }

        public TileMap(Stream input) : base(input, null)
        {
            Window_Changed();
            InitTextureCoords();
            texture = Assets.GetTexture("maps/tiles/" + Texture);

            Window.Changed += Window_Changed;
        }

        private void Window_Changed()
        {
            matrix.UpdateProjection(Window.ProjectionSize);
            matrix.CalculateMVP();

            if (2 * Window.Ratio / DRAW_WIDTH != VertexSize) {
                VertexSize = 2 * Window.Ratio / DRAW_WIDTH;
                DrawSize = new Size(DRAW_WIDTH + 2, Mathi.Ceil(DRAW_WIDTH / Window.Ratio + 1));
                drawThreshold = new Vector2(DrawSize.Width / 2f, DrawSize.Height / 2f);

                mainBuffer?.Dispose();
                mainTextureBuffer = new CachedGPUBuffer(2, DrawSize.Area * 2, BufferPrimitiveType.Quad);
                mainBuffer = new BufferBatch(new IndexBuffer(DrawSize.Area * 2), new GPUBuffer(2, DrawSize.Area * 2, BufferPrimitiveType.Quad, GenerateMainVerticies()), mainTextureBuffer);

                foregroundBuffer?.Dispose();
                foregroundTextureBuffer = new CachedGPUBuffer(2, DrawSize.Area, BufferPrimitiveType.Quad);
                foregroundBuffer = new BufferBatch(new IndexBuffer(DrawSize.Area), new GPUBuffer(2, DrawSize.Area, BufferPrimitiveType.Quad, GenerateForegroundVerticies()), foregroundTextureBuffer);

                UpdateTextureBuffer();
            }
        }

        private float[] GenerateMainVerticies()
        {
            int tileCount = DrawSize.Area;

            float ystart = -(DrawSize.Height / 2f * VertexSize);
            float yOffsetRaw = (VertexSize - Math.Abs(ystart + 1));
            float yOffsetTile = yOffsetRaw / VertexSize - 1;

            xFocusCenterMinClamp = DrawSize.Width / 2f - 1;
            xFocusCenterMaxClamp = Width - DrawSize.Width / 2f + 1;
            yFocusCenterMinClamp = DrawSize.Height / 2f + yOffsetTile;
            yFocusCenterMaxClamp = Height - DrawSize.Height / 2f - yOffsetTile;
            xNextTileOffset = DrawSize.Width / 2f;
            yNextTileOffset = DrawSize.Height / 2f + yOffsetTile;
            xViewPortClamp = Width - DrawSize.Width;
            xEmitterMatrixOffset = DrawSize.Width / 2f;
            yEmitterMatrixOffset = DrawSize.Height / 2f;
            textureBufferL1baseOffset = DrawSize.Area * 8;
            textureBufferLength = DrawSize.Width * 8;

            float[] verticies = new float[tileCount * 2 * 2 * 4];
            for (int i = 0; i < 2; i++) { // PR tile and overlay vertex
                for (int y = 0; y < DrawSize.Height; y++) {
                    for (int x = 0; x < DrawSize.Width; x++) {
                        verticies[x * 8 + y * DrawSize.Width * 8 + i * tileCount * 8 + 0] = -Window.Ratio - VertexSize + (x * VertexSize);
                        verticies[x * 8 + y * DrawSize.Width * 8 + i * tileCount * 8 + 1] = -1f + (y * VertexSize);
                        verticies[x * 8 + y * DrawSize.Width * 8 + i * tileCount * 8 + 2] = -Window.Ratio - VertexSize + (x * VertexSize);
                        verticies[x * 8 + y * DrawSize.Width * 8 + i * tileCount * 8 + 3] = -1f + ((y + 1) * VertexSize);
                        verticies[x * 8 + y * DrawSize.Width * 8 + i * tileCount * 8 + 4] = -Window.Ratio - VertexSize + (x * VertexSize) + VertexSize;
                        verticies[x * 8 + y * DrawSize.Width * 8 + i * tileCount * 8 + 5] = -1f + ((y + 1) * VertexSize);
                        verticies[x * 8 + y * DrawSize.Width * 8 + i * tileCount * 8 + 6] = -Window.Ratio - VertexSize + (x * VertexSize) + VertexSize;
                        verticies[x * 8 + y * DrawSize.Width * 8 + i * tileCount * 8 + 7] = -1f + (y * VertexSize);
                    }
                }
            }
            return verticies;
        }

        private float[] GenerateForegroundVerticies()
        {
            int tileCount = DrawSize.Area;

            float[] verticies = new float[tileCount * 1 * 2 * 4];
            for (int y = 0; y < DrawSize.Height; y++) {
                for (int x = 0; x < DrawSize.Width; x++) {
                    verticies[x * 8 + y * DrawSize.Width * 8 + 0] = -Window.Ratio - VertexSize + (x * VertexSize);
                    verticies[x * 8 + y * DrawSize.Width * 8 + 1] = -1f + (y * VertexSize);
                    verticies[x * 8 + y * DrawSize.Width * 8 + 2] = -Window.Ratio - VertexSize + (x * VertexSize);
                    verticies[x * 8 + y * DrawSize.Width * 8 + 3] = -1f + ((y + 1) * VertexSize);
                    verticies[x * 8 + y * DrawSize.Width * 8 + 4] = -Window.Ratio - VertexSize + (x * VertexSize) + VertexSize;
                    verticies[x * 8 + y * DrawSize.Width * 8 + 5] = -1f + ((y + 1) * VertexSize);
                    verticies[x * 8 + y * DrawSize.Width * 8 + 6] = -Window.Ratio - VertexSize + (x * VertexSize) + VertexSize;
                    verticies[x * 8 + y * DrawSize.Width * 8 + 7] = -1f + (y * VertexSize);
                }
            }
            return verticies;
        }

        private void InitTextureCoords()
        {
            // buffer tile coords
            layerBuffer = new float[3][][];
            for (int layer = 0; layer < 3; layer++) {
                layerBuffer[layer] = new float[(int)base.Size.Height][];
                for (int y = 0; y < this.Size.Height; y++) {
                    layerBuffer[layer][y] = new float[(int)this.Size.Width * 8];
                }
            }

            for (int layer = 0; layer < 3; layer++) {
                for (int y = 0; y < base.Size.Height; y++) {
                    for (int x = 0; x < base.Size.Width; x++) {
                        for (int c = 0; c < 8; c++) {
                            layerBuffer[layer][y][x * 8 + c] = this.GetTile(x, y, layer).Texture[c];
                        }
                    }
                }
            }
        }

        private void UpdateTextureBuffer()
        {
            int sourceIndex = (int)updateTile.X * 8;

            for (int ly = 0; ly < DrawSize.Height; ly++) {
                int baseDestinationIndex = ly * textureBufferLength;
                if (ly + (int)updateTile.Y < Height) {
                    int layerBufferIndex = ly + (int)updateTile.Y;
                    Array.Copy(layerBuffer[0][layerBufferIndex], sourceIndex, mainTextureBuffer.Cache, baseDestinationIndex, textureBufferLength);
                    Array.Copy(layerBuffer[1][layerBufferIndex], sourceIndex, mainTextureBuffer.Cache, baseDestinationIndex + textureBufferL1baseOffset, textureBufferLength);
                    Array.Copy(layerBuffer[2][layerBufferIndex], sourceIndex, foregroundTextureBuffer.Cache, baseDestinationIndex, textureBufferLength);
                } else {
                    Array.Clear(mainTextureBuffer.Cache, baseDestinationIndex, textureBufferLength);
                    Array.Clear(mainTextureBuffer.Cache, baseDestinationIndex + textureBufferL1baseOffset, textureBufferLength);
                    Array.Clear(foregroundTextureBuffer.Cache, baseDestinationIndex, textureBufferLength);
                }
            }
            mainTextureBuffer.Apply();
            foregroundTextureBuffer.Apply();
        }

        public new void Draw()
        {
            Program.Begin();
            Program.Draw(mainBuffer, texture, matrix, true);
            Program.Draw(foregroundBuffer, texture, matrix, true);
            Program.End();
        }

        public new void Update(DeltaTime dt)
        {
        }

        public bool HasCollider(int x, int y)
        {
            return GetTile(x, y).HasFlag(TileAttribute.Collision);
        }
    }
}
