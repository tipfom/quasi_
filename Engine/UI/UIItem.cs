using System;
using System.Collections.Generic;
using Core;
using Engine.Graphics.Renderer;
using Engine.UI.Layout;

namespace Engine.UI
{
    public abstract class UIItem : IDisposable
    {
        public delegate void HandleItemClick();
        public event HandleItemClick Click;
        public event HandleItemClick Release;
        public event HandleItemClick Leave;
        public bool Clicked { get { return (clickCount > 0); } }
        private bool multiClick;
        private int clickCount;

        public readonly UILayout Layout;

        private bool _Visible = true;
        public bool Visible { get { return Layer.IsAttached && _Visible; } set { if (_Visible != value) IsDirty = true; _Visible = value; } }

        private int _Depth;
        public int Depth { get { return _Depth; } set { _Depth = value; IsDirty = true; } }

        protected bool IsDirty;

        public readonly Layer Layer;

        public UIItem(Layer owner, UILayout layout, int depth, bool multiclick = false)
        {
            Layer = owner;

            this.multiClick = multiclick;
            this._Depth = depth;
            this.Layout = layout;
            UIRenderer.Add(owner, this);

            Layout.Init(this);
        }

        public static void HandleGlobalAction(UIActionType actionType, UIAction action)
        {
            for (int i = 0; i < UIRenderer.Current.Count; i++) {
                UIItem item = UIRenderer.Current[i];
                if (item.Collides(action.RelativePosition) && item.HandleAction(actionType, action)) {
                    break;
                }
            }
        }

        public virtual bool HandleAction(UIActionType actiontype, UIAction action)
        {
            switch (actiontype) {
            case UIActionType.Begin:
            case UIActionType.Enter:
                if (!Clicked || multiClick) {
                    clickCount++;
                    Click?.Invoke();
                }
                break;
            case UIActionType.End:
                if (Clicked) {
                    clickCount--;
                    if (!Clicked)
                        Release?.Invoke();
                }
                break;
            case UIActionType.Leave:
                if (Clicked) {
                    clickCount--;
                    if (!Clicked)
                        Leave?.Invoke();
                }
                break;
            }
            return true;
        }

        public bool Collides(Vector2 touchPosition)
        {
            return Layout.IsInside(touchPosition);
        }

        public virtual void Update(DeltaTime dt)
        {
            if (IsDirty || Layout.IsDirty) {
                if (Layout.IsDirty) {
                    Layout.Update();
                }
                UIRenderer.Update(this);
                IsDirty = false;
            }
        }

        public virtual void Dispose()
        {
            UIRenderer.Remove(this);
        }

        public abstract IEnumerable<UIRenderData> ConstructVertexData();
    }
}