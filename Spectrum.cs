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
        public static int View = 24656;
        public static int BreakPoint = -34674;
        public static void Init()
        {
            //ROM = File.ReadAllBytes(@"c:\Users\sg\YandexDisk\test.rom");
            ROM = File.ReadAllBytes("48.rom");
            for (int i = 0; i < 16384; i++)
                Z80.RAM[i] = ROM[i];
            Z80.Reset();
            Mode = Modes.Normal;

            //LoadSNA(@"..\..\..\..\..\..\zexall.sna");
            //LoadSNA(@"..\..\..\..\..\..\Dizzy 6.SNA");
            //LoadSNA(@"..\..\..\..\..\..\Panama Joe.sna");
            //BreakPoint = Z80.PC;
        }

        public static void LoadSNA(string FileName)
        {
            byte[] Bytes = File.ReadAllBytes(FileName);
            Z80.I = Bytes[0];
            Z80.La = Bytes[1];
            Z80.Ha = Bytes[2];
            Z80.Ea = Bytes[3];
            Z80.Da = Bytes[4];
            Z80.Ca = Bytes[5];
            Z80.Ba = Bytes[6];
            Z80.fSa = (Bytes[7] & 128) != 0;
            Z80.fZa = (Bytes[7] & 64) != 0;
            Z80.f5a = (Bytes[7] & 32) != 0;
            Z80.fHa = (Bytes[7] & 16) != 0;
            Z80.f3a = (Bytes[7] & 8) != 0;
            Z80.fVa = (Bytes[7] & 4) != 0;
            Z80.fNa = (Bytes[7] & 2) != 0;
            Z80.fCa = (Bytes[7] & 1) != 0;
            Z80.Aa = Bytes[8];
            Z80.L = Bytes[9];
            Z80.H = Bytes[10];
            Z80.E = Bytes[11];
            Z80.D = Bytes[12];
            Z80.C = Bytes[13];
            Z80.B = Bytes[14];
            Z80.IY = (ushort)(Bytes[15] + Bytes[16] * 256);
            Z80.IX = (ushort)(Bytes[17] + Bytes[18] * 256);
            //Bytes[19] - последний посланный байт в порт IFF2
            Z80.R = Bytes[20];
            Z80.fS = (Bytes[21] & 128) != 0;
            Z80.fZ = (Bytes[21] & 64) != 0;
            Z80.f5 = (Bytes[21] & 32) != 0;
            Z80.fH = (Bytes[21] & 16) != 0;
            Z80.f3 = (Bytes[21] & 8) != 0;
            Z80.fV = (Bytes[21] & 4) != 0;
            Z80.fN = (Bytes[21] & 2) != 0;
            Z80.fC = (Bytes[21] & 1) != 0;
            Z80.A = Bytes[22];
            Z80.SP = (ushort)(Bytes[23] + Bytes[24] * 256);
            Z80.IM = Bytes[25];
            Screen.Border = Bytes[26];
            for (int i = 0; i < 49152; i++) Z80.RAM[16384 + i] = Bytes[27 + i];
            Z80.RET();
        }
    }
}
