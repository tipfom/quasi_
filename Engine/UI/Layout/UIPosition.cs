using System;

namespace Engine.UI.Layout {
    [Flags]
    public enum UIPosition : byte {
        Center = 1 << 0,
        Left = 1 << 1,
        Right = 1 << 2,
        Top = 1 << 3,
        Bottom = 1 << 4,

        TopLeft = Top|Left,
        TopRight = Top|Right,
        BottomLeft = Bottom|Left,
        BottomRight = Bottom|Right,
    }
}
