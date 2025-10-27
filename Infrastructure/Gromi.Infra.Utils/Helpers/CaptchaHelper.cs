using SkiaSharp;

namespace Gromi.Infra.Utils.Helpers
{
    public static class CaptchaHelper
    {
        private static readonly char[] _chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToArray();

        // 固定背景和干扰线颜色
        private static readonly SKColor BackgroundColor = new(240, 240, 240); // 浅灰

        private static readonly SKColor LineColor = new(180, 180, 180);       // 中灰

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <returns></returns>
        public static (string Code, byte[] Image) GenerateCaptcha(int length = 6, int width = 130, int height = 48)
        {
            string code = GenerateCode(length);

            var rnd = new Random();

            using var surface = SKSurface.Create(new SKImageInfo(width, height));
            var canvas = surface.Canvas;

            canvas.Clear(BackgroundColor);
            DrawNoiseLines(canvas, width, height, 25, LineColor, rnd);
            DrawCodeChars(canvas, code, width, height, rnd);

            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return (code, data.ToArray());
        }

        /// <summary>
        /// 生成随机验证码字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string GenerateCode(int length)
        {
            var rnd = new Random();
            char[] buffer = new char[length];
            for (int i = 0; i < length; i++)
            {
                buffer[i] = _chars[rnd.Next(_chars.Length)];
            }
            return new string(buffer);
        }

        /// <summary>
        /// 绘制验证码字符
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="code"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="rnd"></param>
        private static void DrawCodeChars(SKCanvas canvas, string code, int width, int height, Random rnd)
        {
            int charWidth = width / code.Length;
            for (int i = 0; i < code.Length; i++)
            {
                float angle = rnd.Next(-25, 25);
                var color = GetRandomColor(rnd);
                float textSize = rnd.Next(22, 28);

                using var typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold);
                using var font = new SKFont(typeface, textSize);
                using var paint = new SKPaint
                {
                    Color = color,
                    IsAntialias = true
                };

                float x = i * charWidth + 10;
                float y = height / 2 + textSize / 3;

                canvas.Save();
                canvas.Translate(x, y);
                canvas.RotateDegrees(angle);
                canvas.DrawText(code[i].ToString(), 0, 0, font, paint);
                canvas.Restore();
            }
        }

        /// <summary>
        /// 绘制干扰线
        /// </summary>
        private static void DrawNoiseLines(SKCanvas canvas, int width, int height, int count, SKColor color, Random rnd)
        {
            using var paint = new SKPaint
            {
                Color = color,
                StrokeWidth = 1,
                IsAntialias = true
            };

            for (int i = 0; i < count; i++)
            {
                var p1 = new SKPoint(rnd.Next(width), rnd.Next(height));
                var p2 = new SKPoint(rnd.Next(width), rnd.Next(height));
                canvas.DrawLine(p1, p2, paint);
            }
        }

        /// <summary>
        /// 获取随机颜色
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        private static SKColor GetRandomColor(Random rnd)
        {
            SKColor[] palette = new[]
            {
                new SKColor(220, 50, 47),   // 红
                new SKColor(38, 139, 210),  // 蓝
                new SKColor(133, 153, 0),   // 绿
                new SKColor(181, 137, 0),   // 橙
                new SKColor(108, 113, 196), // 紫
            };
            return palette[rnd.Next(palette.Length)];
        }
    }
}