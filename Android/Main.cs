using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace Android
{
    [Activity(Label = "Quasi", Theme = "@style/theme", MainLauncher = true)]
    public class Main : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
        }
    }
}