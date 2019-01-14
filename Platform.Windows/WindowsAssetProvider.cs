using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Graphics;
using Engine;
using OpenTK.Graphics.ES20;
using PixelFormat = OpenTK.Graphics.ES20.PixelFormat;

namespace Windows
{
    class WindowsAssetProvider : Assets.IAssetProvider
    {
        public void Dispose()
        {
        }

        public Stream GetStream(string path)
        {
            return File.Open("Assets/" + path, FileMode.Open);
        }

        public int GetTexture(string path, InterpolationMode interpolation, out int width, out int height)
        {
            Bitmap bitmap = new Bitmap("Assets/" + path);

            int id;
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.GenTextures(1, out id);
            GL.BindTexture(TextureTarget.Texture2D, id);

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)interpolation);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)interpolation);

            width = bitmap.Width;
            height = bitmap.Height;
            return id;
        }
    }
}
