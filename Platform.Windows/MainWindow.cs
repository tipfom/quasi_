using System;
using OpenTK;
using OpenTK.Graphics;
using Engine;
using OpenTK.Input;
using Engine.UI;

namespace Windows
{
    public class MainWindow : GameWindow
    {
        public MainWindow()
            : base (1280, 720, GraphicsMode.Default, "Quasi", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, GraphicsContextFlags.Default)
        {

        }

        protected override void OnResize(EventArgs e)
        {
            Window.Change(this.Width, this.Height);
            base.OnResize(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            Manager.Instance.Init();
            base.OnLoad(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Manager.Instance.Update();
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Manager.Instance.Draw();
            SwapBuffers();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        { 
            base.OnMouseEnter(e);

        }
    }
}
