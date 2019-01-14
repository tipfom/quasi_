using Engine.Graphics.Programs;
using Engine.Graphics.Renderer;
using OpenTK.Graphics.ES20;

namespace Engine
{
    public class Manager
    {
        public static Manager Instance;

        public virtual void Init()
        {
            ColorProgram.Init();
            MatrixProgram.Init();
            FBOProgram.Init();
            ParticleProgram.Init();
            GaussianBlurProgram.Init();
            DarkenProgram.Init();

            UIRenderer.Init();
            UIRenderer.Texture = Assets.GetSprite("interface");
        }

        public virtual void Update()
        {
            Time.Update();
            Layer.Attached.Update(Time.FrameTime);

#if DEBUG
            Time.UpdateFinished();
#endif 
        }

        public virtual void Draw()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            Layer.Attached.Draw();

#if DEBUG
            Time.DrawFinished();
#endif 

        }

        public virtual void Destroy()
        {
            Assets.Destroy();

            ColorProgram.Destroy();
            MatrixProgram.Destroy();
            FBOProgram.Destroy();
            ParticleProgram.Destroy();
            GaussianBlurProgram.Destroy();
            DarkenProgram.Destroy();
        }
    }
}
