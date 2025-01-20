using Android.Graphics;
using Paint = Android.Graphics.Paint;
using Color = Android.Graphics.Color;

namespace ZegarMauiWidget.Platforms.Android
{
    public static class ClockView
    {
        public static Bitmap DrawClock()
        {
            int size = 400;
            float centerX = size / 2;
            float centerY = size / 2;

            Bitmap bitmap = Bitmap.CreateBitmap(size, size, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(bitmap);
            Paint paint = new Paint { AntiAlias = true };

            // Tarcza
            paint.Color = Color.White;
            canvas.DrawCircle(centerX, centerY, size / 2, paint);

            paint.Color = Color.Black;
            paint.SetStyle(Paint.Style.Stroke);
            paint.StrokeWidth = 5;
            canvas.DrawCircle(centerX, centerY, size / 2, paint);

            paint.SetStyle(Paint.Style.Fill);
            canvas.DrawCircle(centerX, centerY, 5, paint);

            // Liczby na zegarze
            DrawText(canvas, "12", 155, 70, 75, Color.DarkGray);
            DrawText(canvas, "3", 340, 220, 75, Color.DarkGray);
            DrawText(canvas, "6", 180, 370, 75, Color.DarkGray);
            DrawText(canvas, "9", 20, 220, 75, Color.DarkGray);

            // Wskazówki
            DateTime now = DateTime.Now;

            DrawHand(canvas, now.Hour % 12 * 30 + now.Minute * 0.5f, centerX, centerY, size / 3, Color.Black, 8);
            DrawHand(canvas, now.Minute * 6, centerX, centerY, size / 2.5f, Color.Black, 6);
            DrawHand(canvas, now.Second * 6, centerX, centerY, size / 2.2f, Color.Red, 3);

            // Data i godzina cyfrowa
            string data = now.ToString("dd-MM-yyyy");
            string godzina = now.ToString("HH:mm:ss");
            DrawText(canvas, data, 135, 270, 25, Color.Gray);
            DrawText(canvas, godzina, 130, 300, 35, Color.Gray);

            return bitmap;
        }

        public static void DrawHand(Canvas canvas, float angle, float cx, float cy, float length, Color color, float width)
        {
            Paint paint = new Paint { AntiAlias = true, Color = color, StrokeWidth = width, StrokeCap = Paint.Cap.Round };
            float radian = (float)(Math.PI / 180 * (angle - 90));
            float x = cx + (float)(Math.Cos(radian) * length);
            float y = cy + (float)(Math.Sin(radian) * length);

            canvas.DrawLine(cx, cy, x, y, paint);
        }

        private static void DrawText(Canvas canvas, string text, float x, float y, float size, Color color)
        {
            Paint paint = new Paint { AntiAlias = true, Color = color, TextSize = size, TextAlign = Paint.Align.Left };
            canvas.DrawText(text, x, y, paint);
        }
    }
}
