using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrum
{
    static class Z80
    {
        public static byte[] RAM = new byte[65536];
        public static byte A, B, C, D, E, H, L, I, R;
        public static byte Aa, Ba, Ca, Da, Ea, Ha, La;
        public static UInt16 PC, SP, IX, IY;
        static bool fS, fZ, fY, fH, fX, fP, fN, fC;
        static bool fSa, fZa, fYa, fHa, fXa, fPa, fNa, fCa;
        static byte IM;
        public static bool Interrupt;
        public static byte[] IN = new byte[256];
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
            Interrupt = false;

            IN[254] = 255;
        }

        /// <summary>
        /// Получение флагового регистра из флагов
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
        /// <summary>
        /// Получение флагов из регистра
        /// </summary>
        /// <param name="f"></param>
        static void GetFlags(byte f)
        {
            fS = (f & 128) != 0;
            fZ = (f & 64) != 0;
            fY = (f & 32) != 0;
            fH = (f & 16) != 0;
            fX = (f & 8) != 0;
            fP = (f & 4) != 0;
            fN = (f & 2) != 0;
            fC = (f & 1) != 0;
        }

        static byte t;
        static bool tb;

        public static int Run()
        {
            ushort pc = PC;
            ushort tmp;
            R++;
            if (R > 127) R = 0;
            switch (RAM[PC++])
            {
                case 0: return 4;                                               //NOP
                case 1: C = RAM[PC++]; B = RAM[PC++]; return 10;                //LD BC,nn
                case 4: INC(ref B); return 4;                                   //INC B
                case 5: DEC(ref B); return 4;                                   //DEC B
                case 6: B = RAM[PC++]; return 7;                                //LD B,n
                case 8:                                                         //EX AF,AF'
                    t = A; A = Aa; Aa = t;
                    tb = fS; fS = fSa; fSa = tb;
                    tb = fZ; fZ = fZa; fSa = tb;
                    tb = fY; fY = fYa; fSa = tb;
                    tb = fH; fH = fHa; fSa = tb;
                    tb = fX; fX = fXa; fSa = tb;
                    tb = fP; fP = fPa; fSa = tb;
                    tb = fN; fN = fNa; fSa = tb;
                    tb = fC; fC = fCa; fSa = tb;
                    return 4;
                case 9: ADDHL(B, C); return 11;                                 //ADD HL,BC
                case 11: DEC(ref B, ref C); return 6;                           //DEC BC
                case 13: DEC(ref C); return 4;                                  //DEC C
                case 14: C = RAM[PC++]; return 7;                               //LD C,n
                case 15:                                                        //RRCA - вращение аккумулятора вправо с переносом бита
                    fC = (A & 1) != 0;
                    A /= 2;
                    if (fC) A += 128;
                    return 4;
                case 16:                                                        //DJNZ n
                    B--;
                    if (B != 0) { JR(); return 13; }
                    else { PC++; return 8; }
                case 17: E = RAM[PC++]; D = RAM[PC++]; return 10;               //LD DE,nn
                case 18: RAM[D * 256 + E] = A; return 7;                        //LD (DE),A
                case 19: INC(ref D, ref E); return 6;                           //INC DE
                case 20: INC(ref D); return 4;                                  //INC D
                case 22: D = RAM[PC++]; return 7;                               //LD D,n
                case 24: JR(); return 4;                                        //JR s
                case 25: ADDHL(D, E); return 11;                                //ADD HL,DE
                case 26: A = RAM[D * 256 + E]; return 7;                        //LD A,(DE)
                case 27: DEC(ref D, ref E); return 6;                           //DEC DE
                case 31:                                                        //RRA - вращение аккумулятора вправо
                    fC = (A & 1) != 0;
                    A /= 2;
                    //if (fC) A += 128; //Наверное так...
                    return 4;
                case 32:                                                        //JR NZ, s
                    if (!fZ) { JR(); return 12; }
                    else { PC++; return 7; }
                case 33: L = RAM[PC++]; H = RAM[PC++]; return 10;               //LD HL, nn
                case 34:                                                        //LD (nn), HL
                    tmp = (ushort)(RAM[PC++] + RAM[PC++] * 256);
                    RAM[tmp++] = L;
                    RAM[tmp] = H;
                    return 20;
                case 35: INC(ref H, ref L); return 6;                           //INC HL
                case 36: INC(ref H); return 4;                                  //INC H
                case 37: DEC(ref H); return 4;                                  //DEC H
                case 38: H = RAM[PC++]; return 7;                               //LD H,n
                case 40:                                                        //JR Z,n
                    if (fZ) { JR(); return 12; }
                    else { PC++; return 7; }
                case 41: ADDHL(H, L); return 11;                                //ADD HL,HL
                case 42:                                                        //LD HL,(nn)
                    tmp = (ushort)(RAM[PC++] + RAM[PC++] * 256); //Проверенно! Работает!
                    L = RAM[tmp++];
                    H = RAM[tmp];
                    return 16;
                case 43: DEC(ref H, ref L); return 6;                           //DEC HL
                case 45: DEC(ref L); return 4;                                  //DEC L
                case 46: L = RAM[PC++]; return 7;                               //LD L,n
                case 47: A ^= 255; return 4;                                    //CPL
                case 48:                                                        //JR NC,n
                    if (!fC) { JR(); return 12; }
                    else { PC++; return 7; }
                case 50:                                                        //LD (nn),A
                    tmp = (ushort)(RAM[PC++] + RAM[PC++] * 256);
                    RAM[tmp] = A;
                    return 13;
                case 53: DEC(ref RAM[H * 256 + L]); return 11;                  //DEC (HL)
                case 54: RAM[H * 256 + L] = RAM[PC++]; return 10;               //LD (HL),n
                case 55: fC = true; return 4;                                   //SCF
                case 56:                                                        //JR C,n
                    if (fC) { JR(); return 12; }
                    else { PC++; return 7; }
                case 58: A = RAM[RAM[PC++] + RAM[PC++] * 256]; return 13;       //LD A,(nn)
                case 60: INC(ref A); return 4;                                  //INC A
                case 61: DEC(ref A); return 4;                                  //DEC A
                case 62: A = RAM[PC++]; return 7;                               //LD A,n
                case 63: fC ^= true; return 4;                                  //CCF
                case 66: B = D; return 4;                                       //LD B,D
                case 68: B = H; return 4;                                       //LD B,H
                case 71: B = A; return 4;                                       //LD B,A
                case 77: C = L; return 4;                                       //LD C,L
                case 78: C = RAM[H * 256 + L]; return 7;                        //LD C,(HL)
                case 79: C = A; return 4;                                       //LD C,A
                case 83: D = E; return 4;                                       //LD D,E
                case 84: D = H; return 4;                                       //LD D,H
                case 86: D = RAM[H * 256 + L]; return 7;                        //LD D,(HL)
                case 87: D = A; return 4;                                       //LD D,A
                case 93: E = L; return 4;                                       //LD E,L
                case 94: E = RAM[H * 256 + L]; return 7;                        //LD E,(HL)
                case 95: E = A; return 4;                                       //LD E,A
                case 97: H = C; return 4;                                       //LD H,C
                case 98: H = D; return 4;                                       //LD H,D
                case 99: H = E; return 4;                                       //LD H,E
                case 103: H = A; return 4;                                      //LD H,A
                case 104: L = B; return 4;                                      //LD L,B
                case 106: L = D; return 4;                                      //LD L,D
                case 107: L = E; return 4;                                      //LD L,E
                case 111: L = A; return 4;                                      //LD L,A
                case 114: RAM[H * 256 + L] = D; return 7;                       //LD (HL),D
                case 115: RAM[H * 256 + L] = E; return 7;                       //LD (HL),E
                case 119: RAM[H * 256 + L] = A; return 7;                       //LD (HL),A
                case 120: A = B; return 4;                                      //LD A,B
                case 122: A = D; return 4;                                      //LD A,D
                case 123: A = E; return 4;                                      //LD A,E
                case 124: A = H; return 4;                                      //LD A,H
                case 125: A = L; return 4;                                      //LD A,L
                case 126: A = RAM[H * 256 + L]; return 7;                       //LD A,(HL)
                case 135: ADD(ref A, A); return 4;                              //ADD A,A
                case 144: SUB(B); return 4;                                     //SUB B
                case 145: SUB(C); return 4;                                     //SUB C
                case 159: SBCA(A); return 4;                                    //SBC A,A
                case 160: AND(B); return 4;                                     //AND B
                case 162: AND(D); return 4;                                     //AND D
                case 167: AND(A); return 4;                                     //AND A
                case 169: XOR(C); return 4;                                     //XOR C
                case 171: XOR(E); return 4;                                     //XOR E
                case 174: XOR(RAM[H * 256 + L]); return 7;                      //XOR (HL)
                case 175: XOR(A); return 4;                                     //XOR A
                case 179: OR(E); return 4;                                      //OR E
                case 181: OR(L); return 4;                                      //OR L
                case 185: CP(C); return 4;                                      //CP C
                case 188: CP(H); return 4;                                      //CP H
                case 189: CP(L); return 4;                                      //CP L
                case 192:                                                       //RET NZ
                    if (!fZ) { RET(); return 5; }
                    else return 11;
                case 193: C = RAM[SP++]; B = RAM[SP++]; return 10;              //POP BC
                case 194:                                                       //JP NZ, nn
                    if (!fZ) PC = (ushort)(RAM[PC] + RAM[PC + 1] * 256);
                    return 10;
                case 195:                                                       //JP nn
                    PC = (ushort)(RAM[PC] + RAM[PC + 1] * 256);
                    return 10;
                case 196:                                                       //CALL NZ, nn
                    if (!fZ)
                    {
                        RAM[--SP] = (byte)((PC + 2) / 256);
                        RAM[--SP] = (byte)((PC + 2) % 256);
                        PC = (ushort)(RAM[PC] + RAM[PC + 1] * 256);
                        return 10;
                    }
                    else
                    {
                        PC += 2;
                        return 17;
                    }
                case 197: RAM[--SP] = B; RAM[--SP] = C; return 11;              //PUSH BC
                case 198: ADD(ref A, RAM[PC++]); return 7;                      //AAD A, n
                case 200:                                                       //RET Z
                    if (fZ) { RET(); return 5; }
                    else return 11;
                case 201: RET(); return 10;                                     //RET
                #region case 203 (Префикс CD)
                case 203:                                                       //(Префикс CD)
                    R++;
                    if (R > 127) R = 0;
                    switch (RAM[PC++])
                    {
                        case 0:                                                 //RLC B - вращение влево без учёта флага C
                            fC = (B & 128) == 128;
                            B &= 127; //откусываем левый бит, что бы он не перенёсся
                            B *= 2;
                            if (fC) B++;
                            return 8;
                        case 60:                                                //SRL H - вращение вправо
                            fC = (H & 1) == 1;
                            H /= 2;
                            H &= 127;
                            fZ = H == 0;
                            return 8;
                        case 126: BIT(RAM[H * 256 + L], 7); return 12;          //BIT 7,(HL)
                        case 134: RES(ref RAM[H * 256 + L], 0); return 15;      //RES 0,(HL)
                        case 174: RES(ref RAM[H * 256 + L], 5); return 15;      //RES 5,(HL)
                        case 198: SET(ref RAM[H * 256 + L], 0); return 15;      //SET 0,(HL)
                        case 238: SET(ref RAM[H * 256 + L], 5); return 15;      //SET 5,(HL)
                    }
                    break;
                #endregion
                case 204:                                                       //CALL Z, nn
                    if (fZ)
                    {
                        RAM[--SP] = (byte)((PC + 2) / 256);
                        RAM[--SP] = (byte)((PC + 2) % 256);
                        PC = (ushort)(RAM[PC] + RAM[PC + 1] * 256);
                        return 10;
                    }
                    else
                    {
                        PC += 2;
                        return 17;
                    }
                case 205:                                                       //CALL nn
                    RAM[--SP] = (byte)((PC + 2) / 256);
                    RAM[--SP] = (byte)((PC + 2) % 256);
                    PC = (ushort)(RAM[PC] + RAM[PC + 1] * 256);
                    return 17;
                case 208:                                                       //RET NC
                    if (!fC) { RET(); return 5; }
                    else return 11;
                case 209:                                                       //POP DE
                    E = RAM[SP++];
                    D = RAM[SP++];
                    return 10;
                case 210:                                                       //JP NC,nn
                    if (!fC) PC = (ushort)(RAM[PC] + RAM[PC + 1] * 256);
                    return 10;
                case 211:                                                       //OUT (n),A
                    OUT(RAM[PC++], A);
                    return 11;
                case 213: RAM[--SP] = D; RAM[--SP] = E; return 11;              //PUSH DE
                case 214: SUB(RAM[PC++]); return 7;                             //SUB n
                case 215:                                                       //RST 10
                    RAM[--SP] = (byte)((PC) / 256);
                    RAM[--SP] = (byte)((PC) % 256);
                    PC = 16;
                    return 11;
                case 216:                                                       //RET C
                    if (fC) { RET(); return 5; }
                    else return 11;
                case 217: EXX(); return 4;                                      //EXX
                case 121: A = C; return 4;                                      //LD A,C
                case 225: L = RAM[SP++]; H = RAM[SP++]; return 10;              //POP HL
                case 227:                                                       //EX (SP),HL
                    t = RAM[SP]; RAM[SP] = L; L = t;
                    t = RAM[SP + 1]; RAM[SP + 1] = H; H = t;
                    return 19;
                case 229: RAM[--SP] = H; RAM[--SP] = L; return 11;              //PUSH HL
                case 230: AND(RAM[PC++]); return 7;                             //AND n
                case 233: PC = (ushort)(H * 256 + L); return 4;                 //JP (HL)
                case 235: t = D; D = H; H = t; t = E; E = L; L = t; return 4;   //EX DE,HL
                #region case 237 (Префикс ED)
                case 237:                                                       //Префикс ED
                    R++;
                    if (R > 127) R = 0;
                    switch (RAM[PC++])
                    {
                        case 67:                                                //LD (nn),BC
                            tmp = (ushort)(RAM[PC++] + RAM[PC++] * 256);
                            RAM[tmp++] = C;
                            RAM[tmp] = B;
                            return 20;
                        case 71: I = A; return 9;                               //LD I,A
                        case 75:                                                //LD BC,(nn)
                            tmp = (ushort)(RAM[PC++] + RAM[PC++] * 256);
                            C = RAM[tmp++];
                            B = RAM[tmp];
                            return 20;
                        case 82: SBCHL(D, E); return 15;                        //SBC HL,DE
                        case 83:                                                //LD (nn),DE
                            tmp = (ushort)(RAM[PC++] + RAM[PC++] * 256);
                            RAM[tmp++] = E;
                            RAM[tmp] = D;
                            return 20;
                        case 86: IM = 1; return 8;                              //IM 1
                        case 91:                                                //LD DE,(nn)
                            tmp = (ushort)(RAM[PC++] + RAM[PC++] * 256);
                            E = RAM[tmp++];
                            D = RAM[tmp];
                            return 20;
                        case 115:                                               //LD (nn),SP
                            tmp = (ushort)(RAM[PC++] + RAM[PC++] * 256);
                            RAM[tmp++] = (byte)(SP % 256);
                            RAM[tmp] = (byte)(SP / 256);
                            return 20;
                        case 120: A = IN[C]; return 12;                         //IN A,(C)
                        case 176:                                               //LDIR - Копирование "снизу" (нормальное)
                            RAM[D * 256 + E] = RAM[H * 256 + L];
                            fN = false;
                            fH = false;
                            C--; if (C == 255) B--;
                            E++; if (E == 0) D++;
                            L++; if (L == 0) H++;
                            if (B != 0 | C != 0) { fP = true; PC -= 2; return 21; }
                            else { fP = false; return 16; }
                        case 184:                                               //LDDR - Копирование "сверху"
                            RAM[D * 256 + E] = RAM[H * 256 + L];
                            fN = false;
                            fH = false;
                            C--; if (C == 255) B--;
                            E--; if (E == 255) D--;
                            L--; if (L == 255) H--;
                            if (B != 0 | C != 0) { fP = true; PC -= 2; return 21; }
                            else { fP = false; return 16; }
                    }
                    break;
                #endregion
                case 241: GetFlags(RAM[SP++]); A = RAM[SP++]; return 10;        //POP AF
                case 243: Interrupt = false; return 40;                         //DI
                case 245: RAM[--SP] = A; RAM[--SP] = F(); return 11;            //PUSH AF
                case 246: OR(RAM[PC++]); return 7;                              //OR n
                case 249: SP = (ushort)(H * 256 + L); return 6;                 //LD SP, HL
                case 251: Interrupt = true; return 4;                           //EI
                #region case 253 (Префикс IY)
                case 253:
                    R++;
                    if (R > 127) R = 0;
                    ushort IReg = (RAM[PC - 1] == 221) ? IX : IY;
                    switch (RAM[PC++])
                    {
                        case 33:                                                //LD IY,nn
                            IY = (ushort)(RAM[PC++] + RAM[PC++] * 256);
                            return 14;
                        case 53: DEC(ref RAM[IplusS(IY)]); return 23;           //DEC (IY+S)
                        case 54: RAM[IplusS(IY)] = RAM[PC++]; return 19;        //LD (IY+S),N
                        case 70: B = RAM[IplusS(IY)]; return 19;                //LD B,(IY+S)
                        case 110: L = RAM[IplusS(IY)]; return 19;               //LD L,(IY+S)
                        case 113: RAM[IplusS(IY)] = C; return 19;               //LD (IY+S),C
                        case 117: RAM[IplusS(IY)] = L; return 19;               //LD (IY+S),L
                        case 134: ADD(ref A, RAM[IplusS(IY)]); return 19;       //ADD A,(IY+S)
                        case 203:
                            PC++;
                            switch (RAM[PC])
                            {
                                case 70: BIT(RAM[IplusS4(IY)], 0); return 23;   //BIT 0,(IY+S)
                                case 78: BIT(RAM[IplusS4(IY)], 1); return 23;   //BIT 1,(IY+S)
                                case 94: BIT(RAM[IplusS4(IY)], 3); return 23;   //BIT 3,(IY+S)
                                case 102: BIT(RAM[IplusS4(IY)], 4); return 23;  //BIT 4,(IY+S)
                                case 110: BIT(RAM[IplusS4(IY)], 5); return 23;  //BIT 5,(IY+S)
                                case 118: BIT(RAM[IplusS4(IY)], 6); return 23;  //BIT 6,(IY+S)
                                case 134: RES(ref RAM[IplusS4(IY)], 0); return 23;  //RES 0,(IY+S)
                                case 142: RES(ref RAM[IplusS4(IY)], 1); return 23;  //RES 1,(IY+S)
                                case 166: RES(ref RAM[IplusS4(IY)], 4); return 23;  //RES 4,(IY+S)
                                case 174: RES(ref RAM[IplusS4(IY)], 5); return 23;  //RES 5,(IY+S)
                                case 198: SET(ref RAM[IplusS4(IY)], 0); return 23;  //SET 0,(IY+S)
                                case 206: SET(ref RAM[IplusS4(IY)], 1); return 23;  //SET 1,(IY+S)
                                case 230: SET(ref RAM[IplusS4(IY)], 4); return 23;  //SET 4,(IY+S)
                                case 238: SET(ref RAM[IplusS4(IY)], 5); return 23;  //SET 5,(IY+S)
                            }
                            break;
                    }
                    break;
                #endregion
                case 254: CP(RAM[PC++]); return 7;                              //CP n
                case 255:                                                       //RST 38
                    RAM[--SP] = (byte)((PC) / 256);
                    RAM[--SP] = (byte)((PC) % 256);
                    PC = 56;
                    return 11;
            }
            PC = pc;
            return 1;
        }
        public static int RunRST38()
        {
            if (Interrupt)
            {
                RAM[--SP] = (byte)((PC) / 256);
                RAM[--SP] = (byte)((PC) % 256);
                PC = 56;
                return 11;
            }
            else return 0;
        }
        //AND
        static void AND(byte Reg)
        {
            A &= Reg;
            fC = false; //Наверное.... надо будет проверить
            fZ = A == 0; //Наверное.... надо будет проверить
        }
        //OR
        static void OR(byte Reg)
        {
            A |= Reg;
            fC = false; //Наверное.... надо будет проверить
            fZ = A == 0; //Наверное.... надо будет проверить
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
            fZ = Reg == 0;
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
            fC = A < Reg;
            A -= Reg;
            fZ = A == 0;
            //
            //
        }
        //ADC

        //SBC
        static void SBCA(byte Reg)
        {
            if (fC) A--;
            A -= Reg;
            fC = A > Reg;
            fZ = A == 0;
            //
            //
        }
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
            byte to = RAM[PC];
            PC = (ushort)(PC + to + 1);
            if (to > 127) PC -= 256;
        }
        //RET
        static void RET()
        {
            //PC = (ushort)(Spectrum.Memory[SP] + Spectrum.Memory[SP + 1] * 256);
            //SP += 2;
            PC = (ushort)(RAM[SP++] + RAM[SP++] * 256);
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
            byte t = RAM[PC++];
            ushort tmp = (ushort)(IY + t);
            if (t > 127) tmp -= 256;
            return tmp;
        }
        static ushort IplusS4(ushort IReg) //Тоже самое, но для тупых 4-х байтных команд где сначала S, потом код команды
        {
            byte t = RAM[PC++ - 1];
            ushort tmp = (ushort)(IY + t);
            if (t > 127) tmp -= 256;
            return tmp;
        }
        //SET
        static void SET(ref byte Byte, byte bit)
        {
            if (bit == 0) Byte |= 1;
            if (bit == 1) Byte |= 2;
            if (bit == 2) Byte |= 4;
            if (bit == 3) Byte |= 8;
            if (bit == 4) Byte |= 16;
            if (bit == 5) Byte |= 32;
            if (bit == 6) Byte |= 64;
            if (bit == 7) Byte |= 128;
        }
        //RES
        static void RES(ref byte Byte, byte bit)
        {
            if (bit == 0) Byte &= 254; //11111110
            if (bit == 1) Byte &= 253; //11111101
            if (bit == 2) Byte &= 251; //11111011
            if (bit == 3) Byte &= 247; //11110111
            if (bit == 4) Byte &= 239; //11101111
            if (bit == 5) Byte &= 223; //11011111
            if (bit == 6) Byte &= 191; //10111111
            if (bit == 7) Byte &= 127; //01111111
        }
        //BIT
        static void BIT(byte Byte, byte bit)
        {
            if (bit == 0) fZ = (Byte & 1) == 0;
            if (bit == 1) fZ = (Byte & 2) == 0;
            if (bit == 2) fZ = (Byte & 4) == 0;
            if (bit == 3) fZ = (Byte & 8) == 0;
            if (bit == 4) fZ = (Byte & 16) == 0;
            if (bit == 5) fZ = (Byte & 32) == 0;
            if (bit == 6) fZ = (Byte & 64) == 0;
            if (bit == 7) fZ = (Byte & 128) == 0;
        }
    }
}
