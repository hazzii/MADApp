using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Petrolhead;

namespace DrawerApp.Fragments
{
    public class Fragment2 : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            var webView = FindViewById<WebView>(Resource.Id.webView);
            WebSettings settings = webView.Settings;
            settings.JavaScriptEnabled = true;
            webView.StartWebChromeClient(new WebChromeClient());
            webView.LoadUrl("https://www.youtube.com/embed/6XMuUVw7TOM");
        }

        public static Fragment2 NewInstance()
        {
            var frag2 = new Fragment2 { Arguments = new Bundle() };
            return frag2;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.fragment2, null);
        }
    }
}