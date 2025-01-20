using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;

namespace ZegarMauiWidget.Platforms.Android
{
    [BroadcastReceiver(Label = "Clock Widget", Exported = true)]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE"})]
    [MetaData("android.appwidget.provider", Resource="@xml/appwidget_provider")]
    public class ClockWidget : AppWidgetProvider
    {
        private const int UPDATE_INTERVAL = 1000;

        public override void OnUpdate(Context? context, AppWidgetManager? appWidgetManager, int[]? appWidgetIds)
        {
            foreach (int appWidgetId in appWidgetIds)
            {
                RemoteViews views = new RemoteViews(context.PackageName, Resource.Layout.widget_layout);
                Bitmap clockBitmap = ClockView.DrawClock();
                views.SetImageViewBitmap(Resource.Id.clockImage, clockBitmap);

                appWidgetManager.UpdateAppWidget(appWidgetId, views);
            }

            if (IsAlarmEnabled(context))
                SetAlarm(context);
        }

        public override void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(context, intent);

            Console.WriteLine("Jestem w receive");

            if (intent.Action == AppWidgetManager.ActionAppwidgetUpdate)
            {
                AppWidgetManager appWidgetManager = AppWidgetManager.GetInstance(context);
                ComponentName thisWidget = new ComponentName(context, Java.Lang.Class.FromType(typeof(ClockWidget)).Name);
                int[] appWidgetIds = appWidgetManager.GetAppWidgetIds(thisWidget);

                OnUpdate(context, appWidgetManager, appWidgetIds);
            }
        }

        public override void OnEnabled(Context context)
        {
            SetAlarmEnabled(context, true);
        }

        public override void OnDisabled(Context context)
        {
            SetAlarmEnabled(context, false);
        }

        private static void SetAlarm(Context context)
        {
            AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
            Intent intent = new Intent(context, typeof(ClockWidget));
            intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
            alarmManager.SetExactAndAllowWhileIdle(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + UPDATE_INTERVAL, pendingIntent);
        }

        private static void SetAlarmEnabled(Context context, bool enabled)
        {
            var prefs = context.GetSharedPreferences("ClockWidgetPrefs", FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.PutBoolean("alarmEnabled", enabled);
            editor.Apply();
        }

        private static bool IsAlarmEnabled(Context context)
        {
            var prefs = context.GetSharedPreferences("ClockWidgetPrefs", FileCreationMode.Private);
            return prefs.GetBoolean("alarmEnabled", true); // Domyślnie true
        }
    }
}
