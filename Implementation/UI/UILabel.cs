using System.Collections.Generic;
using System.Linq;
using Core;
using Engine;
using Engine.Graphics.Renderer;
using Engine.UI;
using Engine.UI.Layout;

namespace Implementation.UI
{
    public class UILabel : UIItem
    {
        public enum TextAlignment : byte
        {
            Left = 0,
            Center = 1,
            Right = 2
        }

        public const int CHAR_WIDTH_PIXEL = 7;
        public const int CHAR_HEIGHT_PIXEL = 9;
        public const int CHAR_SPACING_PIXEL = 1;
        public const float CHAR_SPACING_MULTIPLIER = 21f / 20f;


        private static Dictionary<char, float> charScales = new Dictionary<char, float>() { [' '] = 1f };

        static UILabel()
        {
            foreach (string entry in UIRenderer.Texture.Sprites()) {
                char entryCharacter;
                if (char.TryParse(entry, out entryCharacter)) {
                    float[] verticies = UIRenderer.Texture[entry];
                    float scale = ((verticies[4] - verticies[0]) * UIRenderer.Texture.Width) / ((verticies[3] - verticies[1]) * UIRenderer.Texture.Height);
                    charScales.Add(entryCharacter, scale);
                }
            }
        }

        private string _Text;
        public string Text {
            get { return _Text; }
            set { _Text = value; lines = Text.Split('\n'); Layout.Size = MeasureText(value, charSize); } //size setting will call requupdate
        }
        private string[] lines;

        private TextAlignment _Alignment;
        public TextAlignment Alignment { get { return _Alignment; } set { _Alignment = value; IsDirty = true; } }

        private Color _Color;
        public Color Color {
            get { return _Color; }
            set { _Color = value; IsDirty = true; }
        }

        readonly float charSize;

        public UILabel(Layer owner, UILayout layout, float size, string text, TextAlignment alignment = TextAlignment.Left) : this(owner, layout, UIDepths.MIDDLE, size, text, alignment)
        {

        }

        public UILabel(Layer owner, UILayout layout, int depth, float size, string text, TextAlignment alignment = TextAlignment.Left) : this(owner, layout, depth, size, Color.White, text, alignment)
        {

        }

        public UILabel(Layer owner, UILayout layout, int depth, float size, Color color, string text, TextAlignment alignment = TextAlignment.Left) : base(owner, layout, depth)
        {
            charSize = size;
            Text = text;
            Color = color;
            Alignment = alignment;
            Layout.Update();
        }

        public override IEnumerable<UIRenderData> ConstructVertexData()
        {
            switch (Alignment) {
            case TextAlignment.Left:
                return GetVertexData(lines, Alignment, Layout.GlobalPosition, this.charSize, Depth, this.Color);
            case TextAlignment.Right:
                return GetVertexData(lines, Alignment, Layout.GlobalPosition + new Vector2(Layout.Size.X, 0), this.charSize, Depth, this.Color);
            case TextAlignment.Center:
                return GetVertexData(lines, Alignment, Layout.GlobalPosition + new Vector2(Layout.Size.X * 0.5f, 0), this.charSize, Depth, this.Color);
            default:
                return null;
            }
        }

        public Vector2 MeasureText()
        {
            return MeasureText(Text, this.charSize);
        }

        public static IEnumerable<UIRenderData> GetVertexData(string[] lines, TextAlignment alignment, Vector2 position, float charSize, int depth, Color color)
        {
            Vector2 currentPosition = position;
            Vector2[] sizes;

            float charSizeSpacing = charSize * CHAR_SPACING_MULTIPLIER;

            switch (alignment) {
            case TextAlignment.Left:
                foreach (string line in lines) {
                    foreach (char character in line.ToUpper()) {
                        if (character == ' ') {
                            currentPosition.X += charSize;
                        } else {
                            float characterWidth = charScales[character] * charSize;
                            yield return new UIRenderData(
                                new float[] {
                                        currentPosition.X, currentPosition.Y,
                                        currentPosition.X, currentPosition.Y - charSize,
                                        currentPosition.X + characterWidth, currentPosition.Y - charSize,
                                        currentPosition.X + characterWidth, currentPosition.Y },
                                character.ToString(),
                                color,
                                depth);
                            currentPosition.X += characterWidth * CHAR_SPACING_MULTIPLIER;
                        }
                    }
                    currentPosition.X = position.X;
                    currentPosition.Y -= charSizeSpacing;
                }
                break;
            case TextAlignment.Right:
                for (int i = 0; i < lines.Length; i++) {
                    currentPosition.X = position.X;
                    foreach (char character in lines[i].ToUpper().Reverse()) {
                        if (character == ' ') {
                            currentPosition.X -= charSize;
                        } else {
                            float characterWidth = charScales[character] * charSize;
                            yield return new UIRenderData(
                                new float[] {
                                        currentPosition.X - characterWidth, currentPosition.Y,
                                        currentPosition.X - characterWidth, currentPosition.Y - charSize,
                                        currentPosition.X, currentPosition.Y - charSize,
                                        currentPosition.X, currentPosition.Y },
                                character.ToString(),
                                color,
                                depth);
                            currentPosition.X -= characterWidth * CHAR_SPACING_MULTIPLIER;
                        }
                    }
                    currentPosition.Y -= charSizeSpacing;
                }
                break;
            case TextAlignment.Center:
                sizes = MeasureLines(lines, charSize);

                for (int i = 0; i < lines.Length; i++) {
                    currentPosition.X = position.X - sizes[i].X * 0.5f;
                    foreach (char character in lines[i].ToUpper()) {
                        if (character == ' ') {
                            currentPosition.X += charSize;
                        } else {
                            float characterWidth = charScales[character] * charSize;
                            yield return new UIRenderData(
                                new float[] {
                                        currentPosition.X, currentPosition.Y,
                                        currentPosition.X, currentPosition.Y - charSize,
                                        currentPosition.X + characterWidth, currentPosition.Y - charSize,
                                        currentPosition.X + characterWidth, currentPosition.Y },
                                character.ToString(),
                                color,
                                depth);
                            currentPosition.X += characterWidth * CHAR_SPACING_MULTIPLIER;
                        }
                    }
                    currentPosition.Y -= charSizeSpacing;
                }
                break;
            }
        }

        public static Vector2[] MeasureLines(string[] lines, float charSize)
        {
            Vector2[] lineSizes = new Vector2[lines.Length];
            for (int i = 0; i < lineSizes.Length; i++) {
                lineSizes[i].Y = charSize;
                foreach (char character in lines[i].ToUpper()) {
                    lineSizes[i].X += charScales[character] * charSize;
                }
            }
            return lineSizes;
        }

        public static Vector2 MeasureText(string text, float charSize)
        {
            float maxWidth = 0;
            float currentWidth = 0;
            int height = 1;
            charSize *= CHAR_SPACING_MULTIPLIER;
            foreach (char character in text.ToUpper()) {
                if (character == '\n') {
                    height++;
                    if (currentWidth > maxWidth)
                        maxWidth = currentWidth;
                    currentWidth = 0;
                } else {
                    currentWidth += charScales[character] * charSize;
                }
            }
            if (currentWidth > maxWidth)
                maxWidth = currentWidth;

            return new Vector2(maxWidth, charSize * height);
        }
    }
}