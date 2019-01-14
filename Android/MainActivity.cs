using Android.App;
using Android.OS;
using Engine;

namespace Android
{
    [Activity(Label = "Quasi", Theme = "@style/theme", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.Window.DecorView.SystemUiVisibility = Constants.STATUS_BAR_VISIBILITY;
            base.Window.DecorView.SystemUiVisibilityChange += (sender, e) => {
                if (Window.DecorView.SystemUiVisibility != Constants.STATUS_BAR_VISIBILITY)
                    Window.DecorView.SystemUiVisibility = Constants.STATUS_BAR_VISIBILITY;
            };
            base.OnCreate(savedInstanceState);

            // Create our OpenGL view, and display it
            AndroidAssetProvider.Context = this;
            Engine.Assets.AssetProvider = new AndroidAssetProvider();
            Manager.Instance = new Implementation.TestManager();

            View view = new View(this);
            view.SetOnTouchListener(TouchHandler.Instance);
            SetContentView(view);
        }

        protected override void OnDestroy()
        {
            Manager.Instance.Destroy();
            base.OnDestroy();
        }
    }
}