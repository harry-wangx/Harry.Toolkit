using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Harry.Image.Validate
{
    public class ValidateImage
    {
        static readonly Random r = new Random();
        IImageProvider provider;
        ValidateImageOptions options = null;
        internal ValidateImage(IImageProvider provider, ValidateImageOptions options)
        {
            this.provider = provider;
            this.options = options;
            this.Code = generateRandomText();
        }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// 生成图片
        /// </summary>
        /// <returns></returns>
        public Bitmap RenderImage()
        {
            return provider.RenderImage(options, this.Code);
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private string generateRandomText()
        {
            StringBuilder stringBuilder = new StringBuilder(options.CodeLength);
            for (int i = 0; i < options.CodeLength; i++)
            {
                stringBuilder.Append(options.CodeChars[r.Next(options.CodeChars.Length)]);
            }
            return stringBuilder.ToString();
        }
    }
}
