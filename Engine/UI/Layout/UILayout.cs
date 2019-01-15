using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.UI.Layout
{
    public class UILayout
    {
        private static Vector2 REFERENCE_POSITION;
        private static Vector2 REFERENCE_SIZE;

        static UILayout()
        {
            Window.Changed += () => {
                REFERENCE_POSITION = new Vector2(-Window.Ratio, 1);
                REFERENCE_SIZE = new Vector2(2 * Window.Ratio, 2);
            };
            REFERENCE_POSITION = new Vector2(-Window.Ratio, 1);
            REFERENCE_SIZE = new Vector2(2 * Window.Ratio, 2);
        }

        public Vector2 GlobalPosition;

        private UIPosition _Anchor;
        public UIPosition Anchor { get { return _Anchor; } set { _Anchor = value; IsDirty = true; } }

        private UIPosition _Dock;
        public UIPosition Dock { get { return _Dock; } set { _Dock = value; IsDirty = true; } }

        private Vector2 _AnchorOffset;
        public Vector2 AnchorOffset { get { return _AnchorOffset; } set { _AnchorOffset = value; IsDirty = true; } }

        private Vector2 _Size;
        public Vector2 Size { get { return _Size; } set { _Size = value; IsDirty = true; } }

        public UIItem _Relative;
        public UIItem Relative { get { return _Relative; } set { _Relative = value; IsDirty = true; } }

        public bool IsDirty = false;

        public float[] Verticies = new float[8];

        private UIItem item;

        public UILayout(Vector2 size, Vector2 anchorOffset, UIPosition anchor = UIPosition.TopLeft, UIPosition dock = UIPosition.TopLeft, UIItem relative = null)
        {
            _Anchor = anchor;
            _Dock = dock;
            _Size = size;
            _AnchorOffset = anchorOffset;
            _Relative = relative;
        }

        public void Init(UIItem sender)
        {
            item = sender;
            Update();
        }

        public void Update()
        {
            IsDirty = false;
            Vector2 dockPosition = _Relative?.Layout.GlobalPosition ?? REFERENCE_POSITION;
            Vector2 dockSize = _Relative?.Layout.Size ?? REFERENCE_SIZE;

            //dockPosition.X -= dockSize.X / 2f;    
            if ((_Dock & UIPosition.Right) == UIPosition.Right) {
                dockPosition.X += dockSize.X;
            } else if ((_Dock & UIPosition.Left) != UIPosition.Left) {
                dockPosition.X += dockSize.X / 2f;
            }
            if ((_Dock & UIPosition.Bottom) == UIPosition.Bottom) {
                dockPosition.Y -= dockSize.Y;
            } else if ((_Dock & UIPosition.Top) != UIPosition.Top) {
                dockPosition.Y -= dockSize.Y / 2;

            }

            if ((_Anchor & UIPosition.Right) == UIPosition.Right) {
                dockPosition.X -= Size.X  + _AnchorOffset.X;
            } else if ((_Anchor & UIPosition.Left) == UIPosition.Left) {
                dockPosition.X += _AnchorOffset.X;
            } else {
                dockPosition.X -= Size.X / 2f + _AnchorOffset.X;
            }
            if ((_Anchor & UIPosition.Bottom) == UIPosition.Bottom) {
                dockPosition.Y += Size.Y + _AnchorOffset.Y;
            } else if ((_Anchor & UIPosition.Top) == UIPosition.Top) {
                dockPosition.Y -= _AnchorOffset.Y;
            } else {
                dockPosition.Y += Size.Y / 2f + _AnchorOffset.Y;
            }

            GlobalPosition = dockPosition;
        }

        private void UpdateVerticies()
        {
            float left = GlobalPosition.X, right = GlobalPosition.X + Size.X, top = GlobalPosition.X, bottom = top - Size.Y;

            Verticies[0] = left;
            Verticies[1] = top;
            Verticies[2] = left;
            Verticies[3] = bottom;
            Verticies[4] = right;
            Verticies[5] = bottom;
            Verticies[6] = right;
            Verticies[7] = top;
        }

        public bool IsInside(Vector2 point)
        {
            return !(
                point.X < GlobalPosition.X ||
                point.Y > GlobalPosition.Y ||
                point.X > GlobalPosition.X + Size.X ||
                point.Y < GlobalPosition.Y - Size.Y
                );
        }
    }
}
