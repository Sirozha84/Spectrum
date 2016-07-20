using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrum
{
    static class Spectrum
    {
        public static byte[] Memory = new byte[65536];
        public static int Strings = 312;//Количество строк
        public static void Init()
        {
            Random RND = new Random();
            for (int i = 0; i < 65535; i++)
                Memory[i] = (byte)RND.Next(255);
        }
    }
}
