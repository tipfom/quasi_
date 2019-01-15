using System;
using OpenTK;
using OpenTK.Graphics;
using Engine;
using OpenTK.Input;
using Engine.UI;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.IO;

namespace Windows
{
    public class MainWindow : GameWindow
    {
        public MainWindow()
            : base (1280, 720, GraphicsMode.Default, "Quasi", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, GraphicsContextFlags.Default)
        {
            Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("Platform.Windows.icon.ico"));
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
			UIItem.HandleGlobalAction(UIActionType.Begin, new UIAction(0, new Core.Vector2(e.X, e.Y)));
		}
		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			UIItem.HandleGlobalAction(UIActionType.End, new UIAction(0, new Core.Vector2(e.X, e.Y)));
		}

		protected override void OnMouseMove(MouseMoveEventArgs e)
		{
			UIItem.HandleGlobalAction(UIActionType.Move, new UIAction(0, new Core.Vector2(e.X, e.Y)));
		}

		//Gets mouse position form Mouse.GetCursorState()
		protected override void OnMouseEnter(EventArgs e)
		{
			MouseState ms = Mouse.GetCursorState();
			//Get relative positon to screen
			UIItem.HandleGlobalAction(UIActionType.Enter, new UIAction(0, new Core.Vector2(ms.X - X - 8, Y - 31)));
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			MouseState ms = Mouse.GetCursorState();
			//Get relative positon to screen
			UIItem.HandleGlobalAction(UIActionType.Enter, new UIAction(0, new Core.Vector2(ms.X - X - 8, ms.Y - Y - 31)));
		}
	}
}
