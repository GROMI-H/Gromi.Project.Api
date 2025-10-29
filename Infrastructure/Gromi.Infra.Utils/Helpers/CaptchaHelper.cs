using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Gromi.Infra.Utils.Helpers
{
    public static class CaptchaHelper
    {
        private static readonly char[] _chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToArray();

        private static readonly Color BackgroundColor = Color.FromRgb(240, 240, 240); // 浅灰
        private static readonly Color LineColor = Color.FromRgb(180, 180, 180);       // 中灰

        private static readonly Color[] Palette = new[] {
            Color.FromRgb(220, 50, 47),   // 红
            Color.FromRgb(38, 139, 210),  // 蓝
            Color.FromRgb(133, 153, 0),   // 绿
            Color.FromRgb(181, 137, 0),   // 橙
            Color.FromRgb(108, 113, 196)  // 紫
        };

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        public static (string Code, byte[] Image) GenerateCaptcha(int length = 6, int width = 130, int height = 48)
        {
            string code = GenerateCode(length);
            var rnd = new Random();

            using var image = new Image<Rgba32>(width, height, BackgroundColor);

            DrawNoiseLines(image, width, height, 25, LineColor, rnd);
            DrawCodeChars(image, code, width, height, rnd);

            using var ms = new MemoryStream();
            image.SaveAsPng(ms);
            return (code, ms.ToArray());
        }

        /// <summary>
        /// 生成随机验证码字符串
        /// </summary>
        private static string GenerateCode(int length)
        {
            var rnd = new Random();
            char[] buffer = new char[length];
            for (int i = 0; i < length; i++)
                buffer[i] = _chars[rnd.Next(_chars.Length)];
            return new string(buffer);
        }

        /// <summary>
        /// 绘制验证码字符
        /// </summary>
        private static void DrawCodeChars(Image<Rgba32> image, string code, int width, int height, Random rnd)
        {
            int totalWidth = 0;
            var fontCollection = new FontCollection();
            Font font;
            try
            {
                font = SystemFonts.CreateFont("Arial", 26, FontStyle.Bold);
            }
            catch
            {
                // 回退方案：使用系统默认字体
                font = SystemFonts.CreateFont("DejaVu Sans", 26, FontStyle.Bold);
            }

            // 计算所有字符的总宽度
            var charWidths = new float[code.Length];
            for (int i = 0; i < code.Length; i++)
            {
                var text = code[i].ToString();
                var textSize = TextMeasurer.MeasureSize(text, new TextOptions(font));
                charWidths[i] = textSize.Width;
                totalWidth += (int)textSize.Width;
            }

            // 根据总宽度计算起始的 X 坐标，确保水平居中
            int startX = (width - totalWidth) / 2;

            image.Mutate(ctx =>
            {
                for (int i = 0; i < code.Length; i++)
                {
                    float angle = rnd.Next(-25, 25);
                    var color = Palette[rnd.Next(Palette.Length)];
                    float textSize = rnd.Next(22, 28);

                    var drawFont = new Font(font, textSize);
                    var text = code[i].ToString();

                    // 每个字符的 X 坐标
                    var x = startX + (int)(charWidths.Take(i).Sum()) + (charWidths[i] - textSize) / 2 + 10;
                    var y = height / 2f - textSize / 2f;

                    // 旋转绘制
                    ctx.DrawText(
                        new DrawingOptions
                        {
                            GraphicsOptions = new GraphicsOptions
                            {
                                Antialias = true,
                                AntialiasSubpixelDepth = 4
                            },
                            Transform = Matrix3x2Extensions.CreateRotationDegrees(angle, new PointF(x, y))
                        },
                        text,
                        drawFont,
                        color,
                        new PointF(x, y)
                    );
                }
            });
        }

        /// <summary>
        /// 绘制干扰线
        /// </summary>
        private static void DrawNoiseLines(Image<Rgba32> image, int width, int height, int count, Color color, Random rnd)
        {
            image.Mutate(ctx =>
            {
                for (int i = 0; i < count; i++)
                {
                    var p1 = new PointF(rnd.Next(width), rnd.Next(height));
                    var p2 = new PointF(rnd.Next(width), rnd.Next(height));
                    ctx.DrawLine(color, 1, new[] { p1, p2 });
                }
            });
        }
    }
}