using Engine;
using Engine.Graphics;
using Engine.Graphics.Buffer;
using Engine.Graphics.Programs;
using Engine.Graphics.Renderer;
using Engine.UI.Layout;
using Implementation.UI;

namespace Implementation
{
    public class TestLayer : Layer
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Window.Background = new Core.Color(255, 100, 0, 255);
        }

        public override void Load()
        {
            UIButton button = new UIButton(this, new UILayout(new UIMargin(.3f, .4f), UIMarginType.Absolute, dock: UIPosition.Center), "Test");
            base.Load();
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
