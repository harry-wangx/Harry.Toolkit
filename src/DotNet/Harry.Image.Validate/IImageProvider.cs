using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Harry.Image.Validate
{
    public interface IImageProvider
    {
        Bitmap RenderImage(ValidateImageOptions option,string code);
    }
}
