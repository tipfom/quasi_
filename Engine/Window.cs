using Core;
using Engine.Graphics;
using OpenTK.Graphics.ES20;
using OpenTK.Platform;

namespace Engine {
    public static class Window {
        public delegate void HandleScreenChanged ( );
        public static event HandleScreenChanged Changed;

        private static Color _Background;
        public static Color Background { get { return _Background; } set { _Background = value; SetBackgroundColor( ); } }
        public static Vector2 ProjectionSize { get { return new Vector2(Ratio, 1); } }
        public static Size Size = new Size(1280, 720);
        public static float Ratio;
        public static IWindowInfo Info;

        public static void Change (int width, int height) {
            GL.Viewport(0, 0, width, height);
            Size = new Size(width, height);
            Ratio = width / (float)height;

            Matrix.Default.UpdateProjection(ProjectionSize);
            Matrix.Default.CalculateMVP( );

            Changed?.Invoke( );
        }

        public static void SetBackgroundColor ( ) {
            GL.ClearColor(_Background.R, _Background.G, _Background.B, _Background.A);
        }
    }
}
