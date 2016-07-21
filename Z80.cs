using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrum
{
    static class Z80
    {
        public static byte A, AA, F, FA, B, BA, C, CA, D, DA, E, EA, H, HA, L, LA, I, R;
        public static int SP, IX, IY, PC;
        /// <summary>
        /// Сброс
        /// </summary>
        public static void Reset()
        {
            A = 0; AA = 0; F = 0; FA = 0; B = 0; BA = 0; C = 0; CA = 0;
            D = 0; DA = 0; E = 0; EA = 0; H = 0; HA = 0; L = 0; LA = 0;
            I = 0; R = 0; SP = 0; IX = 0; IY = 0; PC = 0;
        }

        public static int Run()
        {
            PC++;
            if (PC > 65535) PC = 0;
            return 1;
        }
    }
}
