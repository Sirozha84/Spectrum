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
        static byte IM;
        /// <summary>
        /// Сброс
        /// </summary>
        public static void Reset()
        {
            Spectrum.Init();
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
            ushort tmp;
            R++;
            if (R > 127) R = 0;
            switch (Spectrum.Memory[PC++])
            {
                case 0:                                                         //NOP
                    return 4;
                case 1:                                                         //LD BC, nn
                    C = Spectrum.Memory[PC++]; //Проверенно! Работает!
                    B = Spectrum.Memory[PC++];
                    return 10;
                case 4:                                                         //INC B
                    INC(ref B);
                    return 4;
                case 14:                                                        //LD C, n
                    C = Spectrum.Memory[PC++];
                    return 7;
                case 16:                                                        //DJNZ n
                    B--;
                    if (B != 0) { JR(); return 13; }
                    else { PC++; return 8; }
                case 17:                                                        //LD DE, nn
                    E = Spectrum.Memory[PC++];
                    D = Spectrum.Memory[PC++];
                    return 10;
                case 22:                                                        //LD D, n
                    D = Spectrum.Memory[PC++];
                    return 7;
                case 24:                                                        //JR s
                    JR();
                    return 4; //А сколько на самом деле хз, в книге не написано
                case 25:                                                        //ADD HL, DE
                    ADDHL(D, E);
                    return 11;
                case 27:                                                        //DEC DE
                    DEC(ref D, ref E);
                    return 6;
                case 32:                                                        //JR NZ, s
                    if (!fZ) { JR(); return 12; }
                    else { PC++; return 7; }
                case 33:                                                        //LD HL, nn
                    L = Spectrum.Memory[PC++];
                    H = Spectrum.Memory[PC++];
                    return 10;
                case 34:                                                        //LD (nn), HL
                    tmp = (ushort)(Spectrum.Memory[PC++] + Spectrum.Memory[PC++] * 256); //Проверенно! Работает!
                    Spectrum.Memory[tmp++] = L;
                    Spectrum.Memory[tmp] = H;
                    return 20;
                case 35:                                                        //INC HL
                    INC(ref H, ref L);
                    return 6;
                case 38:                                                        //LD H, n
                    H = Spectrum.Memory[PC++];
                    return 7;
                case 40:                                                        //JR Z, n
                    if (fZ) { JR(); return 12; }
                    else { PC++; return 7; }
                case 42:                                                        //LD HL, (nn)
                    tmp = (ushort)(Spectrum.Memory[PC++] + Spectrum.Memory[PC++] * 256); //Проверенно! Работает!
                    L = Spectrum.Memory[tmp++];
                    H = Spectrum.Memory[tmp];
                    return 16;
                case 43:                                                        //DEC HL
                    DEC(ref H, ref L);
                    return 6;
                case 48:                                                        //JR NC, n
                    if (!fC) { JR(); return 12; }
                    else { PC++; return 7; }
                case 50:                                                        //LD (nn), A
                    tmp = (ushort)(Spectrum.Memory[PC++] + Spectrum.Memory[PC++] * 256);
                    Spectrum.Memory[tmp] = A;
                    return 13;
                case 53:                                                        //DEC (HL)
                    DEC(ref Spectrum.Memory[H * 256 + L]);
                    return 11;
                case 54:                                                        //LD (HL), n
                    Spectrum.Memory[H * 256 + L] = Spectrum.Memory[PC++];
                    return 10;
                case 55:                                                        //SCF
                    fC = true;
                    return 4;
                case 56:                                                        //JR C, n
                    if (fC) { JR(); return 12; }
                    else { PC++; return 7; }
                case 62:                                                        //LD A, n
                    A = Spectrum.Memory[PC++];
                    return 7;
                case 63:                                                        //CCF
                    fC ^= true;
                    return 4;
                case 71:                                                        //LD B, A
                    B = A;
                    return 4;
                case 78:                                                        //LD C, (HL)
                    C = Spectrum.Memory[H * 256 + L];
                    return 7;
                case 86:                                                        //LD D, (HL)
                    D = Spectrum.Memory[H * 256 + L];
                    return 7;
                case 94:                                                        //LD E, (HL)
                    E = Spectrum.Memory[H * 256 + L];
                    return 7;
                case 95:                                                        //LD E, A
                    E = A;
                    return 4;
                case 98:                                                        //LD H, D
                    H = D;
                    return 4;
                case 99:                                                        //LD H, E
                    H = E;
                    return 4;
                case 103:                                                       //LD H, A
                    H = A;
                    return 4;
                case 106:                                                       //LD L, D
                    L = D;
                    return 4;
                case 107:                                                       //LD L, E
                    L = E;
                    return 4;
                case 111:                                                       //LD L, A
                    L = A;
                    return 4;
                case 114:                                                       //LD (HL), D
                    Spectrum.Memory[H * 256 + L] = D;
                    return 7;
                case 115:                                                       //LD (HL), E
                    Spectrum.Memory[H * 256 + L] = E;
                    return 7;
                case 119:                                                       //LD (HL), A
                    Spectrum.Memory[H * 256 + L] = A;
                    return 7;
                case 120:                                                       //LD A, B
                    A = B;
                    return 4;
                case 122:                                                       //LD A, D
                    A = D;
                    return 4;
                case 126:                                                       //LD A, (HL)
                    A = Spectrum.Memory[H * 256 + L];
                    return 7;
                case 135:                                                       //ADD A, A
                    ADD(ref A, A);
                    return 4;
                case 145:                                                       //SUB C
                    SUB(C);
                    return 4;
                case 167:                                                       //AND A
                    AND(A);
                    return 4;
                case 174:                                                       //XOR (HL)
                    XOR(Spectrum.Memory[H * 256 + L]);
                    return 7;
                case 175:                                                       //XOR A
                    XOR(A);
                    return 4;
                case 179:                                                       //OR E
                    OR(E);
                    return 4;
                case 185:                                                       //CP C
                    CP(C);
                    return 4;
                case 188:                                                       //CP H
                    CP(H);
                    return 4;
                case 195:                                                       //JP nn
                    PC = (ushort)(Spectrum.Memory[PC] + Spectrum.Memory[PC + 1] * 256);
                    return 10;
                case 198:                                                       //AAD A, n
                    ADD(ref A, Spectrum.Memory[PC++]);
                    return 7;
                case 200:                                                       //RET Z
                    if (fZ) { RET(); return 5; }
                    else return 11;
                case 201:                                                       //RET
                    RET();
                    return 10;
                case 205:                                                       //CALL;
                    Spectrum.Memory[--SP] = (byte)((PC+2) / 256);
                    Spectrum.Memory[--SP] = (byte)((PC+2) % 256);
                    PC = (ushort)(Spectrum.Memory[PC] + Spectrum.Memory[PC + 1] * 256);
                    return 17;
                case 208:                                                       //RET NC
                    if (!fC) { RET(); return 5; }
                    else return 11;
                case 211:                                                       //OUT (n), A
                    OUT(Spectrum.Memory[PC++], A);
                    return 11;
                case 217:                                                       //EXX
                    EXX();
                    return 4;
                case 230:                                                       //AND n
                    AND(Spectrum.Memory[PC++]);
                    return 7;
                case 233:                                                       //JP (HL)
                    PC = (ushort)(H * 256 + L);
                    return 4;
                case 235:                                                       //EX DE, HL
                    byte t = D; D = H; H = t;
                    t = E; E = L; L = t;
                    return 4;
                #region case 237 (Префикс ED)
                case 237:                                                       //ED
                    R++;
                    if (R > 127) R = 0;
                    switch (Spectrum.Memory[PC++])
                    {
                        case 67:                                                //LD (nn), BC
                            tmp = (ushort)(Spectrum.Memory[PC++] + Spectrum.Memory[PC++] * 256);
                            Spectrum.Memory[tmp++] = C;
                            Spectrum.Memory[tmp] = B;
                            return 20;
                        case 71:                                                //LD I, A
                            I = A;
                            return 9;
                        case 82:                                                //SBC HL, DE
                            SBCHL(D, E);
                            return 15;
                        case 83:                                                //LD (nn), DE
                            tmp = (ushort)(Spectrum.Memory[PC++] + Spectrum.Memory[PC++] * 256);
                            Spectrum.Memory[tmp++] = E;
                            Spectrum.Memory[tmp] = D;
                            return 20;
                        case 86:                                                //IM 1
                            IM = 1;
                            return 8;
                        case 176:                                               //LDIR - Копирование "снизу" (нормальное)
                            Spectrum.Memory[D * 256 + E] = Spectrum.Memory[H * 256 + L];
                            fN = false;
                            fH = false;
                            C--; if (C == 255) B--;
                            E++; if (E == 0) D++;
                            L++; if (L == 0) H++;
                            if (B != 0 | C != 0)
                            {
                                fP = true;
                                PC -= 2;
                                return 21; //Если копировали (если копирование не закончено)
                            }
                            else
                            {
                                fP = false;
                                return 16; //Если скопировали (последнее копирование тоже учитывается)
                            }
                        case 184:                                               //LDDR - Копирование "сверху"
                            Spectrum.Memory[D * 256 + E] = Spectrum.Memory[H * 256 + L];
                            fN = false;
                            fH = false;
                            C--; if (C == 255) B--;
                            E--; if (E == 255) D--;
                            L--; if (L == 255) H--;
                            if (B != 0 | C != 0)
                            {
                                fP = true;
                                PC -= 2;
                                return 21; //Если копировали (если копирование не закончено)
                            }
                            else
                            {
                                fP = false;
                                return 16; //Если скопировали (последнее копирование тоже учитывается)
                            }
                    }
                    break;
                #endregion
                case 243:                                                       //DI
                    Prer = false;
                    return 40;
                case 249:                                                       //LD SP, HL
                    SP = (ushort)(H * 256 + L);
                    return 6;
                case 251:                                                       //EI
                    Prer = true;
                    return 4;
                #region case 253 (Префикс IY)
                case 253:
                    R++;
                    if (R > 127) R = 0;
                    ushort IReg = (Spectrum.Memory[PC - 1] == 221) ? IX : IY;
                    switch (Spectrum.Memory[PC++])
                    {
                        case 33:                                                //LD IY, nn
                            IY = (ushort)(Spectrum.Memory[PC++] + Spectrum.Memory[PC++] * 256);
                            return 14;
                        case 53:                                                //DEC (IY+S)
                            Spectrum.Memory[IplusS(IY)]--;
                            //z
                            //p
                            //s
                            return 23;
                        case 54:                                                //LD (IY+S), N
                            Spectrum.Memory[IplusS(IY)] = Spectrum.Memory[PC++];
                            return 19;
                        case 110:                                               //LD L, (IY+S)
                            L = Spectrum.Memory[IplusS(IY)];
                            return 19;
                        case 113:                                               //LD (IY+S), C
                            Spectrum.Memory[IplusS(IY)] = C;
                            return 19;
                        case 117:                                               //LD (IY+S), L
                            Spectrum.Memory[IplusS(IY)] = L;
                            return 19;
                        case 134:                                               //ADD A, (IY+S)
                            ADD(ref A, Spectrum.Memory[IplusS(IY)]);
                            return 19;
                        case 203:
                            PC++;
                            switch (Spectrum.Memory[PC])
                            {
                                case 70:                                        //BIT 0, (IY+S)
                                    fZ = (Spectrum.Memory[IplusS4(IY)] & 1) == 0;
                                    return 23;
                                case 78:                                        //BIT 1, (IY+S)
                                    fZ = (Spectrum.Memory[IplusS4(IY)] & 2) == 0;
                                    return 23;
                                case 134:                                       //RES 0, (IY+S)
                                    Spectrum.Memory[IplusS4(IY)] &= 254; //11111110
                                    return 23;
                                case 142:                                       //RES 1, (IY+S)
                                    Spectrum.Memory[IplusS4(IY)] &= 253; //11111101
                                    return 23;
                                case 166:                                       //RES 4, (IY+S)
                                    Spectrum.Memory[IplusS4(IY)] &= 239; //11101111
                                    return 23;
                                case 174:                                       //RES 5, (IY+S)
                                    Spectrum.Memory[IplusS4(IY)] &= 223; //11011111
                                    return 23;
                                case 198:                                       //SET 0, (IY+S)
                                    Spectrum.Memory[IplusS4(IY)] |= 1; //00000001
                                    return 23;
                                case 206:                                       //SET 1, (IY+S)
                                    Spectrum.Memory[IplusS4(IY)] |= 2; //00000010
                                    return 23;
                                case 230:                                       //SET 4, (IY+S)
                                    Spectrum.Memory[IplusS4(IY)] |= 16; //00010000
                                    return 23;
                            }
                            break;
                    }
                    break;
                    #endregion
            }
            PC = pc;
            return 1;
        }
        //AND
        static void AND(byte Reg)
        {
            A &= Reg;
            fC = false; //Наверное.... надо будет проверить
            //
        }
        //OR
        static void OR(byte Reg)
        {
            A |= Reg;
            fC = false; //Наверное.... надо будет проверить
            //
        }
        //XOR
        static void XOR(byte Reg)
        {
            A ^= Reg;
            fC = false;
            fZ = A == 0;
            fP = A % 2 == 0;
        }
        //INC
        static void INC(ref byte Reg)
        {
            Reg++;
            if (Reg == 0)
            {
                fC = true; //Наверное
            }
        }
        static void INC(ref byte r1, ref byte r2)
        {
            r2++;
            if (r2 == 0)
            {
                r1++;
                fH = true; //Наверное
            }
            if (r1 == 0)
            {
                fC = true; //Наверное
            }
        }
        //DEC
        static void DEC(ref byte b)
        {
            b--;
            fC = b == 255; //Наверное...
            fZ = b == 0;   //Наверное...
        }
        static void DEC(ref byte r1, ref byte r2)
        {
            r2--;
            if (r2 == 255)
                r1--;
            fC = r1 == 255; //Наверное...
            fZ = (r1 == 0 & r2 == 0);   //Наверное...
        }
        //ADD
        static void ADD(ref byte r1, byte r2)
        {
            byte t = r1;
            r1 += r2;
            //czps
            fC = t > r1;
            fZ = r1 == 0;
        }
        static void ADDHL(byte r1, byte r2)
        {
            fH = L + r2 > 255;
            L = (byte)(L + r2);
            byte c = fH ? (byte)1 : (byte)0;
            fC = H + r1 + c > 255;
            H = (byte)(H + r1 + c);
            fZ = H == 0 & L == 0;
        }
        //SUB
        static void SUB(byte Reg)
        {
            A -= Reg;
            fC = A > Reg;
            fZ = A == 0;
            //
            //
        }
        //ADC

        //SBC
        static void SBCHL(byte r1, byte r2)
        {
            byte c = fC ? (byte)1 : (byte)0;
            fH = L - r2 - c < 0;
            L = (byte)(L - r2 - c);
            c = fH ? (byte)1 : (byte)0;
            fC = H - r1 - c < 0;
            H = (byte)(H - r1 - c);
            //fN = true;
        }
        //CP
        static void CP(byte Reg)
        {
            ushort a = (ushort)(A - Reg);
            fS = (a & 128) != 0;
            fN = true;
            fZ = a == 0;
            fC = (a & 256) != 0;
            fH = ((A & 15) - (Reg & 15) & 16) != 0; //Не разбирался, тупо списал
            fP = ((A ^ Reg) & (A ^ a) & 128) != 0; //Не разбирался, тупо списал
        }
        //JR
        static void JR()
        {
            byte to = Spectrum.Memory[PC];
            PC = (ushort)(PC + to + 1);
            if (to > 127) PC -= 256;
        }
        //RET
        static void RET()
        {
            PC = (ushort)(Spectrum.Memory[SP] + Spectrum.Memory[SP + 1] * 256);
            SP += 2;
        }

        //IN

        //OUT
        static void OUT(byte Port, byte Reg)
        {
            if (Port == 254) Screen.Border = Reg % 8;
            //
        }
        //EXX
        static void EXX()
        {
            //bc de hl - меняются на альтернативные
            byte temp;
            temp = B; B = Ba; Ba = temp;
            temp = C; C = Ca; Ca = temp;
            temp = D; D = Da; Da = temp;
            temp = E; E = Ea; Ea = temp;
            temp = H; H = Ha; Ha = temp;
            temp = L; L = La; La = temp;
        }
        //IX+S / IY+S
        static ushort IplusS(ushort IReg)
        {
            byte t = Spectrum.Memory[PC++];
            ushort tmp = (ushort)(IY + t);
            if (t > 127) tmp -= 256;
            return tmp;
        }
        static ushort IplusS4(ushort IReg) //Тоже самое, но для тупых 4-х байтных команд где сначала S, потом код команды
        {
            byte t = Spectrum.Memory[PC++ - 1];
            ushort tmp = (ushort)(IY + t);
            if (t > 127) tmp -= 256;
            return tmp;
        }
    }
}
