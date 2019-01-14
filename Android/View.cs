using System;
using Android.Content;
using OpenTK;
using OpenTK.Platform.Android;
using Engine;

namespace Android
{
    public class View : AndroidGameView
    {
        bool firstLoad = false;

        public View(Context context) : base(context)
        {
            OpenTK.Graphics.GraphicsContext.ShareContexts = true;
            ContextRenderingApi = OpenTK.Graphics.GLVersion.ES2;
            Window.Info = WindowInfo;
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            Window.Change(w, h);
            base.OnSizeChanged(w, h, oldw, oldh);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!firstLoad)
                Manager.Instance.Init();
            firstLoad = true;
            Run();
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

        protected override void Dispose(bool disposing)
        {
            firstLoad = false;
            base.Dispose(disposing);
        }
    }
}