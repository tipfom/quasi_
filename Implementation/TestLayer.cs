﻿using Core;
using Engine;
using Engine.UI.Layout;
using Implementation.UI;

namespace Implementation
{
    public class TestLayer : Layer
    {
        Engine.Graphics.TileMap tm;

        protected override void OnAttached()
        {
            base.OnAttached();
            Window.Background = new Color(255, 100, 0, 255);
        }

        public override void Load()
        { 
            UIButton button = new UIButton(this, new UILayout(new Vector2(0.6f, 0.6f), new Vector2(0.3f,0.3f), dock: UIPosition.TopLeft), "Test");
            UIButton label = new UIButton(this, new UILayout(new Vector2(1f, 1f), new Vector2(0.1f, 0.1f), dock: UIPosition.BottomRight, anchor: UIPosition.TopLeft, relative:button) , "HALLO MARTIN", 0.1f, 0, Color.Red);
            tm = new Engine.Graphics.TileMap(Assets.AssetProvider.GetStream("maps/green-1.map"));
            button.Click += () => { label.Text = "David ist dumm"; };

            base.Load();
        }

        public override void Update(DeltaTime dt)
        {
            tm.Update(dt);
            base.Update(dt);
        }

        public override void Draw()
        {
            tm.Draw();
            base.Draw();
        }
    }
}
