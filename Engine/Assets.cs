using System;
using System.Collections.Generic;
using System.IO;
using Core;
using Core.Graphics;
using Newtonsoft.Json;
using OpenTK.Graphics.ES20;

namespace Engine
{
    public static class Assets
    {
        public interface IAssetProvider : IDisposable
        {
            Stream GetStream(string path);
            int GetTexture(string path, InterpolationMode interpolation, out int width, out int height);
        }

        public static IAssetProvider AssetProvider;

        public static void Destroy()
        {
            foreach (int shader in loadedVertexShader.Values)
                GL.DeleteShader(shader);
            loadedVertexShader.Clear();
            foreach (int shader in loadedFragmentShader.Values)
                GL.DeleteShader(shader);
            loadedFragmentShader.Clear();

            AssetProvider.Dispose();
        }

        public static string GetText(params string[] path)
        {
            using (StreamReader reader = new StreamReader(AssetProvider.GetStream(Path.Combine(path)))) {
                return reader.ReadToEnd();
            }
        }

        private static Dictionary<string, int> loadedVertexShader = new Dictionary<string, int>();
        public static int GetVertexShader(string name)
        {
            if (loadedVertexShader.ContainsKey(name))
                return loadedVertexShader[name];
            int shader = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(shader, GetText("shader", "vertex", name + ".txt"));
            GL.CompileShader(shader);

#if DEBUG
            string log = GL.GetShaderInfoLog(shader);
            Debug.Print(typeof(Assets), $"vertexshader {shader} loaded");
            if (!string.IsNullOrWhiteSpace(log))
                Debug.Print(typeof(Assets), "log: " + log);
            Debug.CheckGL(typeof(Assets));
#endif

            return shader;
        }

        private static Dictionary<string, int> loadedFragmentShader = new Dictionary<string, int>();
        public static int GetFragmentShader(string name)
        {
            if (loadedFragmentShader.ContainsKey(name))
                return loadedFragmentShader[name];
            int shader = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(shader, GetText("shader", "fragment", name + ".txt"));
            GL.CompileShader(shader);

#if DEBUG
            string log = GL.GetShaderInfoLog(shader);
            Debug.Print(typeof(Assets), $"fragmentshader {shader} loaded");
            if (!string.IsNullOrWhiteSpace(log))
                Debug.Print(typeof(Assets), "log: " + log);
            Debug.CheckGL(typeof(Assets));
#endif

            return shader;
        }

        public static Texture2D GetTexture(params string[] path)
        {
            return GetTexture(InterpolationMode.PixelPerfect, path);
        }

        static Dictionary<int, Texture2D> textureCache = new Dictionary<int, Texture2D>();
        public static Texture2D GetTexture(InterpolationMode interpolation, params string[] path)
        {
            int pathhash = path.GetHashCode();
            if (!textureCache.ContainsKey(pathhash) || textureCache[pathhash].Disposed) {
                int width, height, id = AssetProvider.GetTexture("textures/" + Path.Combine(path) + ".png", interpolation, out width, out height);

                if (!textureCache.ContainsKey(pathhash)) {
                    textureCache.Add(pathhash, new Texture2D(id, new Size(width, height), path[path.Length - 1]));
                } else {
                    textureCache[pathhash] = new Texture2D(id, new Size(width, height), path[path.Length - 1]);
                }
#if DEBUG
                if (id == 0 || Debug.CheckGL(typeof(Assets)))
                    Debug.Print(typeof(Assets), "failed loading image " + path[path.Length - 1]);
                else
                    Debug.Print(typeof(Assets), "loaded image " + path[path.Length - 1]);
#endif
            }
            return textureCache[pathhash];
        }



        static Dictionary<int, Spritebatch2D> spriteCache = new Dictionary<int, Spritebatch2D>();
        public static Spritebatch2D GetSprite(string name)
        {
            int namehash = name.GetHashCode();
            if (!spriteCache.ContainsKey(namehash)) {
                Texture2D texture = Assets.GetTexture(name);
                Dictionary<string, int[]> spriteContent = new Dictionary<string, int[]>();
                JsonConvert.PopulateObject(GetText("textures", name + ".json"), spriteContent);
                spriteCache.Add(namehash, new Spritebatch2D(spriteContent, texture));
            }
            return spriteCache[namehash];
        }
    }
}
