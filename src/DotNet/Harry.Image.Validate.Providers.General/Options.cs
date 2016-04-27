using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harry.Image.Validate.Providers.General
{
    public class Options
    {
        public Options()
        {
            this.FontWarp = Level.Medium;
            this.BackgroundNoise = Level.High;
            this.LineNoise = Level.High;
        }

        public Level FontWarp
        {
            get;
            set;
        }
        public Level BackgroundNoise
        {
            get;
            set;
        }
        public Level LineNoise
        {
            get;
            set;
        }
    }
}
