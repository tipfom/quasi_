using System;
using Core;
using Engine.Graphics.Renderer;

namespace Engine
{
    public class Layer : IDisposable
    {
        private static Layer _Attached;
        public static Layer Attached {
            get { return _Attached; }
            set {
                UIRenderer.Prepare(value);
                if (_Attached != null)
                    _Attached.IsAttached = false;
                value.IsAttached = true;
                value.OnAttached();
                _Attached = value;
            }
        }

        static Layer()
        {
        }

        public bool IsAttached { get; private set; }

        public virtual void Dispose()
        {
            UIRenderer.Delete(this);
        }

        public virtual void Draw()
        {
            UIRenderer.Draw();
        }

        public virtual void Load()
        {
        }

        public virtual void Update(DeltaTime dt)
        {
            UIRenderer.Update(dt);
        }

        protected virtual void OnAttached()
        {
        }
    }
}
