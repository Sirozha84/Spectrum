using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Spectrum
{
    static class Spectrum
    {
        public static byte[] Memory = new byte[65536];
        public static byte[] ROM;
        public static int Strings = 312;//Количество строк 
        public enum Modes { Normal, Stop, Step, Frame };
        public static Modes Mode;
        public static void Init()
        {
            ROM = File.ReadAllBytes("48.rom");
            for (int i = 0; i < 16384; i++)
                Memory[i] = ROM[i];
        Mode = Modes.Normal;
    }
}
}
