using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Graphics;
using Android.Widget;

namespace ZegarMauiWidget.Platforms.Android
{
    [BroadcastReceiver(Label = "Clock Widget", Exported = true)]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE"})]
    [MetaData("android.appwidget.provider", Resource="@xml/appwidget_provider")]
    public class ClockWidget : AppWidgetProvider
    {
        public override void OnUpdate(Context? context, AppWidgetManager? appWidgetManager, int[]? appWidgetIds)
        {
            foreach (int appWidgetId in appWidgetIds)
            {
                RemoteViews views = new RemoteViews(context.PackageName, Resource.Layout.widget_layout);
                Bitmap clockBitmap = ClockView.DrawClock();
                views.SetImageViewBitmap(Resource.Id.clockImage, clockBitmap);

                appWidgetManager.UpdateAppWidget(appWidgetId, views);
            }
        }
    }
}
