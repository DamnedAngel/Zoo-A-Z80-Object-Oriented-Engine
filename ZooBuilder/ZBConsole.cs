using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooBuilder
{
    class ZBConsole
    {
        public static bool Verbose { get; set; }

        public static void Debug(string format, params object[] arg)
        {
            if (Verbose)
            {
                Console.WriteLine(format, arg);
            }
        }

        public static void Print(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }
    }
}
