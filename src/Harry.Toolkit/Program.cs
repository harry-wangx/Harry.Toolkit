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
            Console.WriteLine(long.MaxValue);
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(Common.IdHelper.CreateIdWithChar(dt));
            }
            Console.ReadLine();
        }
    }
}
