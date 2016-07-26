using Harry.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harry.Toolkit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DateTime dt = new DateTime(1970, 1, 1);
            Console.WriteLine(Utils.IsEmail("gamelong@qq.com"));
            Console.WriteLine(Utils.IsEmail("game@long@qq.com"));
            Console.ReadLine();
        }
    }
}
