using System.Collections.Generic;
using Core;
using Engine;
using Engine.UI;
using Engine.UI.Layout;

namespace Implementation.UI {

    public class UIButton : UIItem {
        private const float DEFAULT_TEXT_SIZE = 0.1f;
        private const float EDGE_WIDTH_HEIGHT_RATIO = 3f / 20f;

        private Color _Color;
        private string _Text;
        private float charSize;
        private string[ ] lines;

        public UIButton(Layer owner, UILayout layout, string text) : this(owner, layout, text, DEFAULT_TEXT_SIZE, 0, Color.White) {
        }

        public UIButton(Layer owner, UILayout layout, string text, int depth, Color color) : this(owner, layout, text, DEFAULT_TEXT_SIZE, depth, color) {
        }

        public UIButton(Layer owner, UILayout layout, string text, float textsize, int depth, Color color) : base(owner, layout, depth, false) {
            _Text = text;
            lines = text.Split('\n');
            charSize = textsize;
            Color = color;
            Click += ( ) => IsDirty = true;
            Release += ( ) => IsDirty = true;
            Leave += ( ) => IsDirty = true;
        }

        public Color Color { get { return _Color; } set { _Color = value; IsDirty = true; ; } }
        public string Text { get { return _Text; } set { _Text = value; lines = _Text.Split('\n'); IsDirty = true; } }

        public override IEnumerable<UIRenderData> ConstructVertexData( ) {
            string textureDomain = "btn_" + (Clicked ? "p" : "i");
            float w = EDGE_WIDTH_HEIGHT_RATIO * Layout.Height;
            yield return new UIRenderData(UIRectangle.GetVerticies(Layout.X, Layout.Y, w, Layout.Height), textureDomain + "l", Color, Depth);
            yield return new UIRenderData(UIRectangle.GetVerticies(Layout.X + w, Layout.Y, Layout.Width - 2 * w, Layout.Height), textureDomain + "c", Color, Depth);
            yield return new UIRenderData(UIRectangle.GetVerticies(Layout.X + Layout .Width- w, Layout.Y, w, Layout.Height), textureDomain + "r", Color, Depth);

            Vector2 textPosition = new Vector2(Layout.X + Layout.Width* 0.5f, Layout.Y - (Layout.Height - lines.Length * charSize) * 0.5f);

            foreach (UIRenderData d in UILabel.GetVertexData(new string[ ] { Text }, UILabel.TextAlignment.Center, textPosition, charSize, Depth, Color.White)) {
                yield return d;
            }
        }
    }
}