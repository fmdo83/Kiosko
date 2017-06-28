using Android.Content;

namespace KioskoMode
{
    public class OnScreenOffReceiver : BroadcastReceiver
    {
       public override void OnReceive(Context context, Intent intent)
        {
            if (Intent.ActionScreenOff != intent.Action) return;
            var ctx = (App)context.ApplicationContext;
            WakeUpDevice(ctx);
        }

        private static void WakeUpDevice(App context)
        {
            var wakeLock = context.GetWakeLock();
            if (wakeLock.IsHeld)
                wakeLock.Release();
            wakeLock.Acquire();
            wakeLock.Release();
        }

    }
}