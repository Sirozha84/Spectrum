using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrum
{
    static class Assembler
    {
        public static string GetCommand(ref int adr)
        {
            string cm = "*****";
            int codes = 1;
            switch (Spectrum.Memory[adr])
            {
                case 0: cm = "NOP"; break;
                case 1: cm = "LD BC, " + asm16(adr); codes = 3; break;
                case 17: cm = "LD DE, " + asm16(adr); codes = 3; break;
                case 175: cm = "XOR A"; break;
                case 243: cm = "DI"; break;
            }
            adr += codes;
            return cm;
        }

        static string asm16(int adr)
        {
            return (Spectrum.Memory[adr + 1] + Spectrum.Memory[adr + 2] * 256).ToString();
        }
    }
}
