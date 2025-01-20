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

            SetAlarm(context);
        }

        public override void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(context, intent);

            if (intent.Action == AppWidgetManager.ActionAppwidgetUpdate)
            {
                AppWidgetManager appWidgetManager = AppWidgetManager.GetInstance(context);
                ComponentName thisWidget = new ComponentName(context, Java.Lang.Class.FromType(typeof(ClockWidget)).Name);
                int[] appWidgetIds = appWidgetManager.GetAppWidgetIds(thisWidget);

                OnUpdate(context, appWidgetManager, appWidgetIds);
            }
        }

        private static void SetAlarm(Context context)
        {
            AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
            Intent intent = new Intent(context, typeof(ClockWidget));
            intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
            alarmManager.SetExactAndAllowWhileIdle(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + UPDATE_INTERVAL, pendingIntent);
        }
    }
}
