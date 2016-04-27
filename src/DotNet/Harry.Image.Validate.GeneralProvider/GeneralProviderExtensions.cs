using Harry.Image.Validate.Providers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harry.Image.Validate
{
    public static class GeneralProviderExtensions
    {
        public static ValidateImageBuilder UseGeneralProvider(this ValidateImageBuilder validateImage)
        {
            validateImage.AddProvider(new GeneralProvider());
            return validateImage;
        }

        public static ValidateImageBuilder UseGeneralProvider(this ValidateImageBuilder validateImage,Action<Options> func)
        {
            Options options = new Options();
            func(options);
            validateImage.AddProvider(new GeneralProvider(options));
            return validateImage;
        }
    }
}
