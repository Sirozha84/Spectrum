using System.IO;

namespace Spectrum
{
    static class Spectrum
    {
        public static byte[] ROM;
        public static int Strings = 312;//Количество строк 
        public enum Modes { Normal, Stop, Step, Frame };
        public static Modes Mode;
        public static int View = 30020;
        public static int BreakPoint = -1;
        public static void Init()
        {
            //ROM = File.ReadAllBytes(@"c:\Users\sg\YandexDisk\test.rom");
            ROM = File.ReadAllBytes(@"..\..\..\..\..\Files\48.rom");
            for (int i = 0; i < 16384; i++)
                Z80.RAM[i] = ROM[i];
            Z80.Reset();
            Mode = Modes.Normal;

            //return;
            //BreakPoint = 11916;
            //BreakPoint = 11924;
            //BreakPoint = 11962;
            //View = 23722;
            //LoadSNA(@"..\..\..\..\..\Files\TEST.asm.sna");  BreakPoint = 30000;
            LoadSNA(@"..\..\..\..\..\Files\TEST.sna");      BreakPoint = 32768;
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
            Z80.Fa = Bytes[7];
            Z80.Aa = Bytes[8];
            Z80.L = Bytes[9];
            Z80.H = Bytes[10];
            Z80.E = Bytes[11];
            Z80.D = Bytes[12];
            Z80.C = Bytes[13];
            Z80.B = Bytes[14];
            Z80.IY = (ushort)(Bytes[15] + Bytes[16] * 256);
            Z80.IX = (ushort)(Bytes[17] + Bytes[18] * 256);
            //Bytes[19] - Interrupt (bit 2 contains IFF2, 1=EI/0=DI)
            Z80.R = Bytes[20];
            Z80.F = Bytes[21];
            Z80.A = Bytes[22];
            Z80.SP = (ushort)(Bytes[23] + Bytes[24] * 256);
            Z80.IM = Bytes[25];
            Screen.Border = Bytes[26];
            for (int i = 0; i < 49152; i++) Z80.RAM[16384 + i] = Bytes[27 + i];
            Z80.RET(true);
        }
    }
}
