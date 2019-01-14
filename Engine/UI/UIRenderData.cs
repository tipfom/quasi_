using Core;

namespace Engine.UI
{
    public struct UIRenderData
    {
        public int Depth;
        public float[] Verticies;
        public string Texture;
        public Color Color;
        
        public UIRenderData(float[] verticies, string texture, Color color, int depth)
        {
            Verticies = verticies;
            Texture = texture;
            Color = color;
            Depth = depth;
        }
    }
}
