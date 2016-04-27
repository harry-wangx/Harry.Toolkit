using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Harry.Image.Validate
{
    public class ValidateImageBuilder
    {
        List<IImageProvider> lstProviders = new List<IImageProvider>();
        ValidateImageOptions options = new ValidateImageOptions();
        static readonly Random r = new Random();

        /// <summary>
        /// 配置项
        /// </summary>
        public ValidateImageOptions Options
        {
            get { return options; }
        }

        /// <summary>
        /// 添加验证码生成器
        /// </summary>
        /// <param name="provider"></param>
        public ValidateImageBuilder AddProvider(IImageProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            lstProviders.Add(provider);
            return this;
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <returns></returns>
        public ValidateImageBuilder Configure(Action<ValidateImageOptions> fun)
        {
            fun(options);
            options.CheckOptions();
            return this;
        }

        /// <summary>
        /// 生成验证码对象
        /// </summary>
        /// <returns></returns>
        public ValidateImage Build()
        {
            if (lstProviders.Count <= 0)
            {
                throw new Exception("未找到绘图provider");
            }
            var provider = lstProviders[r.Next(lstProviders.Count)];
            return new ValidateImage(provider, this.Options);
        }


    }
}
