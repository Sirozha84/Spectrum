using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrum
{
    static class Z80
    {
        public static byte A, B, C, D, E, H, L, I, R;
        public static byte Aa, Ba, Ca, Da, Ea, Ha, La;
        public static UInt16 PC, SP, IX, IY;
        static bool fS, fZ, fY, fH, fX, fP, fN, fC;
        static bool fSa, fZa, fYa, fHa, fXa, fPa, fNa, fCa;
        static bool Prer;
        /// <summary>
        /// Сброс
        /// </summary>
        public static void Reset()
        {
            A = 0; B = 0; C = 0; D = 0; E = 0; H = 0; L = 0;
            Aa = 0; Ba = 0; Ca = 0; Da = 0; Ea = 0; Ha = 0; La = 0;
            PC = 0; SP = 0; IX = 0; IY = 0; I = 0; R = 0;
            fS = false; fZ = false; fY = false; fH = false;
            fX = false; fP = false; fN = false; fC = false;
            fSa = false; fZa = false; fYa = false; fHa = false;
            fXa = false; fPa = false; fNa = false; fCa = false;
            Prer = false;
        }

        /// <summary>
        /// Получение флагового регистра
        /// </summary>
        /// <returns></returns>
        public static byte F()
        {
            byte reg = 0;
            if (fS) reg += 128;
            if (fZ) reg += 64;
            if (fY) reg += 32;
            if (fH) reg += 16;
            if (fX) reg += 8;
            if (fP) reg += 4;
            if (fN) reg += 2;
            if (fC) reg += 1;
            return reg;
        }
        public static byte Fa()
        {
            byte reg = 0;
            if (fSa) reg += 128;
            if (fZa) reg += 64;
            if (fYa) reg += 32;
            if (fHa) reg += 16;
            if (fXa) reg += 8;
            if (fPa) reg += 4;
            if (fNa) reg += 2;
            if (fCa) reg += 1;
            return reg;
        }

        public static int Run()
        {
            ushort pc = PC;
            R++;
            if (R > 127) R = 0;
            switch (Spectrum.Memory[PC++])
            {
                case 0:                                                         //NOP
                    return 4;
                case 17:                                                        //LD DE, nn
                    D = Spectrum.Memory[PC++];
                    E = Spectrum.Memory[PC++];
                    return 10;
                case 62:                                                        //LD A, n
                    A = Spectrum.Memory[PC++];
                    return 7;
                case 71:                                                        //LD B, A
                    B = A;
                    return 4;
                case 175:                                                       //XOR A
                    XOR(A);
                    return 4;
                case 195:                                                       //JP nn
                    PC = (ushort)(Spectrum.Memory[PC] + Spectrum.Memory[PC + 1] * 256);
                    return 10;
                case 211:                                                       //OUT (n), A
                    OUT(Spectrum.Memory[PC++], A);
                    return 11;
                case 243:                                                       //DI
                    Prer = false;
                    return 40;
                #region case 237 (Префикс ED)
                case 237:                                                       //ED
                    switch (Spectrum.Memory[PC++])
                    {
                        case 71:                                                //LD I, A
                            I = A;
                            return 9;
                    }
                    break;
                #endregion
            }
            PC = pc;
            return 1;
        }

        static void XOR(byte Reg)
        {
            A ^= Reg;
            fC = false;
        }

        static void OUT(byte Port, byte Reg)
        {
            //
        }
    }
}
