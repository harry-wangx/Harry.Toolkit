using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace Harry.Image.Validate.Providers.General
{
    public class GeneralProvider : IImageProvider
    {
        Random rand = new Random();

        private static readonly string[] RandomFontFamily = new string[]
{
            "arial",
            "arial black",
            "comic sans ms",
            "courier new",
            "estrangelo edessa",
            "franklin gothic medium",
            "georgia",
            "lucida console",
            "lucida sans unicode",
            "mangal",
            "microsoft sans serif",
            "palatino linotype",
            "sylfaen",
            "tahoma",
            "times new roman",
            "trebuchet ms",
            "verdana"
};
        private static readonly Color[] RandomColor = new Color[]
    {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Black,
            Color.Purple,
            Color.Orange
    };


        public GeneralProvider()
        {
            this.Options = new Options();
        }

        public GeneralProvider(Options generalOptions)
        {
            this.Options = generalOptions;
        }

        public Options Options { get; set; }


        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>

        public Bitmap RenderImage(ValidateImageOptions option, string code)
        {
            Bitmap bitmap = new Bitmap(option.Width, option.Height, PixelFormat.Format24bppRgb);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.White);
                int num = 0;
                double num2 = (double)(option.Width / option.CodeLength);
                for (int i = 0; i < code.Length; i++)
                {
                    char c = code[i];
                    using (Font font = this.GetFont(option))
                    {
                        using (Brush brush = new SolidBrush(this.GetRandomColor()))
                        {
                            Rectangle rectangle = new Rectangle(Convert.ToInt32((double)num * num2), 0, Convert.ToInt32(num2), option.Height);
                            GraphicsPath graphicsPath = TextPath(c.ToString(), font, rectangle);
                            WarpText(option,graphicsPath, rectangle);
                            graphics.FillPath(brush, graphicsPath);
                            num++;
                        }
                    }
                }
                Rectangle rect = new Rectangle(new Point(0, 0), bitmap.Size);
                this.AddNoise(graphics, rect);
                this.AddLine(option,graphics, rect);
            }
            return bitmap;
        }

        /// <summary>
        /// 获取随机字体
        /// </summary>
        /// <returns></returns>
        private string GetRandomFontFamily()
        {
            return GeneralProvider.RandomFontFamily[this.rand.Next(0, GeneralProvider.RandomFontFamily.Length)];
        }

        /// <summary>
        /// 获取随机Point
        /// </summary>
        /// <returns></returns>
        private PointF RandomPoint(int xmin, int xmax, int ymin, int ymax)
        {
            return new PointF(this.rand.Next(xmin, xmax), this.rand.Next(ymin, ymax));
        }

        /// <summary>
        /// 获取随机Point
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        private PointF RandomPoint(Rectangle rect)
        {
            return this.RandomPoint(rect.Left, rect.Width, rect.Top, rect.Bottom);
        }

        /// <summary>
        /// 获取随机颜色
        /// </summary>
        /// <returns></returns>
        private Color GetRandomColor()
        {
            return GeneralProvider.RandomColor[this.rand.Next(0, GeneralProvider.RandomColor.Length)];
        }

        private static GraphicsPath TextPath(string s, Font f, Rectangle r)
        {
            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near
            };
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddString(s, f.FontFamily, (int)f.Style, f.Size, r, format);
            return graphicsPath;
        }

        /// <summary>
        /// 获取随机字体
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        private Font GetFont(ValidateImageOptions option)
        {
            string randomFontFamily = this.GetRandomFontFamily();
            float emSize;
            switch (this.Options.FontWarp)
            {
                case Level.Low:
                    emSize = Convert.ToInt32(option.Height * 0.8);
                    break;
                case Level.Medium:
                    emSize = Convert.ToInt32(option.Height * 0.85);
                    break;
                case Level.High:
                    emSize = Convert.ToInt32(option.Height * 0.9);
                    break;
                case Level.Extreme:
                    emSize = Convert.ToInt32(option.Height * 0.95);
                    break;
                default:
                    emSize = Convert.ToInt32(option.Height * 0.7);
                    break;
            }
            return new Font(randomFontFamily, emSize, FontStyle.Bold);
        }

        /// <summary>
        /// 扭曲图形
        /// </summary>
        /// <param name="option"></param>
        /// <param name="textPath"></param>
        /// <param name="rect"></param>
        private void WarpText(ValidateImageOptions option, GraphicsPath textPath, Rectangle rect)
        {
            float num;
            float num2;
            switch (this.Options.FontWarp)
            {
                case Level.Low:
                    num = 6f;
                    num2 = 1f;
                    break;
                case Level.Medium:
                    num = 5f;
                    num2 = 1.3f;
                    break;
                case Level.High:
                    num = 4.5f;
                    num2 = 1.4f;
                    break;
                case Level.Extreme:
                    num = 4f;
                    num2 = 1.5f;
                    break;
                default:
                    return;
            }
            RectangleF srcRect = new RectangleF(Convert.ToSingle(rect.Left), 0f, Convert.ToSingle(rect.Width), (float)rect.Height);
            int num3 = Convert.ToInt32((float)rect.Height / num);
            int num4 = Convert.ToInt32((float)rect.Width / num);
            int num5 = rect.Left - Convert.ToInt32((float)num4 * num2);
            int num6 = rect.Top - Convert.ToInt32((float)num3 * num2);
            int num7 = rect.Left + rect.Width + Convert.ToInt32((float)num4 * num2);
            int num8 = rect.Top + rect.Height + Convert.ToInt32((float)num3 * num2);
            if (num5 < 0)
            {
                num5 = 0;
            }
            if (num6 < 0)
            {
                num6 = 0;
            }
            if (num7 > option.Width)
            {
                num7 = option.Width;
            }
            if (num8 > option.Height)
            {
                num8 = option.Height;
            }
            PointF pointF = this.RandomPoint(num5, num5 + num4, num6, num6 + num3);
            PointF pointF2 = this.RandomPoint(num7 - num4, num7, num6, num6 + num3);
            PointF pointF3 = this.RandomPoint(num5, num5 + num4, num8 - num3, num8);
            PointF pointF4 = this.RandomPoint(num7 - num4, num7, num8 - num3, num8);
            PointF[] destPoints = new PointF[]
        {
                pointF,
                pointF2,
                pointF3,
                pointF4
        };
            Matrix matrix = new Matrix();
            matrix.Translate(0f, 0f);
            textPath.Warp(destPoints, srcRect, matrix, WarpMode.Perspective, 0f);
        }

        /// <summary>
        /// 添加噪点
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        private void AddNoise(Graphics g, Rectangle rect)
        {
            int noiseArea;//单个杂点所占的面积
            int num2;//单排或单列最大噪点数
            switch (this.Options.BackgroundNoise)
            {
                case Level.Low:
                    noiseArea = 30;
                    num2 = 40;
                    break;
                case Level.Medium:
                    noiseArea = 18;
                    num2 = 40;
                    break;
                case Level.High:
                    noiseArea = 16;
                    num2 = 39;
                    break;
                case Level.Extreme:
                    noiseArea = 12;
                    num2 = 38;
                    break;
                default:
                    return;
            }

            SolidBrush solidBrush = new SolidBrush(this.GetRandomColor());
            int maxValue = Convert.ToInt32(Math.Max(rect.Width, rect.Height) / num2);
            for (int i = 0; i <= Convert.ToInt32(rect.Width * rect.Height / noiseArea); i++)
            {
                g.FillEllipse(solidBrush, this.rand.Next(rect.Width), this.rand.Next(rect.Height), this.rand.Next(maxValue), this.rand.Next(maxValue));
            }
            solidBrush.Dispose();
        }

        /// <summary>
        /// 添加杂线
        /// </summary>
        /// <param name="option"></param>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        private void AddLine(ValidateImageOptions option, Graphics g, Rectangle rect)
        {
            int pointCount;//端点数
            int lineCount;//线条数
            float width;
            switch (this.Options.LineNoise)
            {
                case Level.Low:
                    {
                        pointCount = 4;
                        width = Convert.ToSingle(option.Height / 31.25);
                        lineCount = 1;
                        break;
                    }
                case Level.Medium:
                    {
                        pointCount = 5;
                        width = Convert.ToSingle(option.Height / 27.7777);
                        lineCount = 1;
                        break;
                    }
                case Level.High:
                    {
                        pointCount = 3;
                        width = Convert.ToSingle(option.Height / 25);
                        lineCount = 2;
                        break;
                    }
                case Level.Extreme:
                    {
                        pointCount = 3;
                        width = Convert.ToSingle((double)option.Height / 22.7272);
                        lineCount = 3;
                        break;
                    }
                default:
                    return;
            }

            PointF[] array = new PointF[pointCount + 1];
            using (Pen pen = new Pen(this.GetRandomColor(), width))
            {

                for (int i = 1; i <= lineCount; i++)
                {
                    for (int j = 0; j <= pointCount; j++)
                    {
                        array[j] = this.RandomPoint(rect);
                    }
                    g.DrawCurve(pen, array, 1.75f);
                }
            }
        }
    }
}
