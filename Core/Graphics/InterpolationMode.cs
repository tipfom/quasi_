#if __ANDROID__ || __WINDOWS__
using OpenTK.Graphics.ES20;
#endif

namespace Core.Graphics {
    public enum InterpolationMode {
        PixelPerfect
#if __ANDROID__ || __WINDOWS__
            = (int)All.Nearest
#endif
            ,
        Linear
#if __ANDROID__ || __WINDOWS__
            = (int)All.Linear
#endif 
            ,
    }
}
