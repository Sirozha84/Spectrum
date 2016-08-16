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
        public static byte[] ROM;
        public static int Strings = 312;//Количество строк 
        public enum Modes { Normal, Stop, Step, Frame };
        public static Modes Mode;
        public static int BreakPoint = 0; //13166 - Enter, 4178 - DEL, 4344 - "E"
        public static void Init()
        {
            //ROM = File.ReadAllBytes(@"c:\Users\sg\YandexDisk\test.rom");
            ROM = File.ReadAllBytes("48.rom");
            for (int i = 0; i < 16384; i++)
                Z80.RAM[i] = ROM[i];
            Z80.Reset();
            Mode = Modes.Normal;
        }
    }
}
