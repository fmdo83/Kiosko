using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using Android.Content;
using Android.Runtime;
using Android.Preferences;
using System.Collections.Generic;

namespace KioskoMode
{
    [Activity(Label = "KioskoMode", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private const string PrefKioskMode = "pref_kiosk_mode";

        public class CustomViewGroup : ViewGroup
        {
            public CustomViewGroup(Context context) : base(context)
            {
            }
            public override bool OnTouchEvent(MotionEvent ev)
            {
                return true;
            }
            protected override void OnLayout(bool changed, int l, int t, int r, int b)
            {
                // throw new NotImplementedException();
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            this.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
            this.Window.AddFlags(WindowManagerFlags.DismissKeyguard);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            var sp = PreferenceManager.GetDefaultSharedPreferences(this);
            var edit = sp.Edit();
            edit.PutBoolean(PrefKioskMode, true);
            edit.Commit();

            WindowManagerLayoutParams localLayoutParams = new WindowManagerLayoutParams();

            localLayoutParams.Type = WindowManagerTypes.SystemError;
            localLayoutParams.Gravity = GravityFlags.Top;
            localLayoutParams.Flags = WindowManagerFlags.NotFocusable |
                WindowManagerFlags.NotTouchModal | WindowManagerFlags.LayoutInScreen;
            localLayoutParams.Width = WindowManagerLayoutParams.MatchParent;
            localLayoutParams.Height = (int)(50 * Resources.DisplayMetrics.ScaledDensity);
            localLayoutParams.Format = Format.Transparent;

            IWindowManager manager = ApplicationContext.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            CustomViewGroup view = new CustomViewGroup(this);
            manager.AddView(view, localLayoutParams);
        }

        public override void OnBackPressed(){}

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (!hasFocus)
            {
                var closeDialog = new Intent(Intent.ActionCloseSystemDialogs);
                SendBroadcast(closeDialog);
            }
        }

        //Optional: Disable buttons (i.e. volume buttons)
        private readonly IList<Keycode> _blockedKeys = new[] { Keycode.VolumeDown, Keycode.VolumeUp };
        public override bool DispatchKeyEvent(KeyEvent e)
        {
            if (_blockedKeys.Contains(e.KeyCode))
                return true;
            return base.DispatchKeyEvent(e);
        }


    }
}

