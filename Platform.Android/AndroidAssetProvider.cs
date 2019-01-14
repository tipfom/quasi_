using System.IO;
using Android.Content;
using Android.Graphics;
using Android.Opengl;
using Core.Graphics;
using Engine;
using OpenTK.Graphics.ES20;

namespace Android
{
    class AndroidAssetProvider : Assets.IAssetProvider
    {
        public static Context Context;

        public void Dispose()
        {
        }

        public Stream GetStream(string path)
        {
            return Context.Assets.Open(System.IO.Path.Combine(path));
        }

        public int GetTexture(string path, InterpolationMode interpolation, out int width, out int height)
        {
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            using (BitmapFactory.Options options = new BitmapFactory.Options() { InScaled = false })
            using (Stream stream = GetStream(path))
            using (Bitmap bitmap = BitmapFactory.DecodeStream(stream, null, options)) {
                GLUtils.TexImage2D((int)TextureTarget.Texture2D, 0, bitmap, 0);
                width = bitmap.Width;
                height = bitmap.Height;
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)interpolation);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)interpolation);

            return id;
        }
    }
}