using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harry.Image.Validate
{
    public class ValidateImageOptions
    {
        public string CodeChars { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ12346789";
        public int CodeLength { get; set; } = 5;

        public int Width { get; set; } = 160;

        public int Height { get; set; } = 40;


        public void CheckOptions()
        {
            if (string.IsNullOrWhiteSpace(CodeChars))
            {
                throw new ArgumentNullException(nameof(CodeChars));
            }
            if (CodeLength <= 0)
            {
                throw new Exception("CodeLength必须大于0");
            }
            if (Width <= 0)
            {
                throw new Exception("Width必须大于0");
            }
            if (Height <= 0)
            {
                throw new Exception("Height必须大于0");
            }
        }
    }
}
