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
            UIButton button = new UIButton(this, new UILayout(new UIMargin(.1f, .4f, 0.1f, 0.2f), UIMarginType.Absolute), "Test");
            UIButton label = new UIButton(this, new UILayout(new UIMargin(.1f, 0.8f, 0.5f, 0.2f), UIMarginType.Absolute), "HALLO MARTIN", 0.05f, 0, Color.Red);
			button.Click += () => { label.Text = "David ist dumm"; };
			base.Load();
        }


        public override void Draw()
        {
            base.Draw();
        }
    }
}
