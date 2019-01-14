using Core;
using Engine;
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
            UIButton label = new UIButton(this, new UILayout(new UIMargin(0.1f,0.5f,0.1f,0.5f), UIMarginType.Relative, dock:UIPosition.Right|UIPosition.Bottom, anchor:UIPosition.Left|UIPosition.Top, relative:button) , "HALLO MARTIN", 0.01f, 0, Color.Red);
            base.Load();
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
