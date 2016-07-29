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
                case 5:                                                         //DEC B
                    DEC(ref B);
                    return 4;
                case 6:                                                         //LD B, n
                    B = Spectrum.Memory[PC++];
                    return 7;
                case 8:                                                         //EX AF, AF'
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
                case 9:                                                         //ADD HL, BC
                    ADDHL(B, C);
                    return 11;
                case 11:                                                        //DEC BC
                    DEC(ref B, ref C);
                    return 6;
                case 13:                                                        //DEC C
                    DEC(ref C);
                    return 4; //наверное...
                case 14:                                                        //LD C, n
                    C = Spectrum.Memory[PC++];
                    return 7;
                case 15:                                                        //RRCA - вращение аккумулятора вправо с переносом бита
                    fC = (A & 1) != 0;
                    A /= 2;
                    if (fC) A += 128;
                    return 4;
                case 16:                                                        //DJNZ n
                    B--;
                    if (B != 0) { JR(); return 13; }
                    else { PC++; return 8; }
                case 17:                                                        //LD DE, nn
                    E = Spectrum.Memory[PC++];
                    D = Spectrum.Memory[PC++];
                    return 10;
                case 18:                                                        //LD (DE), A
                    Spectrum.Memory[D * 256 + E] = A;
                    return 7;
                case 19:                                                        //INC DE
                    INC(ref D, ref E);
                    return 6;
                case 20:                                                        //INC D
                    INC(ref D);
                    return 4;
                case 22:                                                        //LD D, n
                    D = Spectrum.Memory[PC++];
                    return 7;
                case 24:                                                        //JR s
                    JR();
                    return 4; //А сколько на самом деле хз, в книге не написано
                case 25:                                                        //ADD HL, DE
                    ADDHL(D, E);
                    return 11;
                case 26:                                                        //LD A, (DE)
                    A = Spectrum.Memory[D * 256 + E];
                    return 7;
                case 27:                                                        //DEC DE
                    DEC(ref D, ref E);
                    return 6;
                case 31:                                                        //RRA - вращение аккумулятора вправо
                    fC = (A & 1) != 0;
                    A /= 2;
                    //if (fC) A += 128; //Наверное так...
                    return 4;
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
                case 36:                                                        //INC H
                    INC(ref H);
                    return 4;
                case 37:                                                        //DEC H
                    DEC(ref H);
                    return 4;
                case 38:                                                        //LD H, n
                    H = Spectrum.Memory[PC++];
                    return 7;
                case 40:                                                        //JR Z, n
                    if (fZ) { JR(); return 12; }
                    else { PC++; return 7; }
                case 41:                                                        //ADD HL, HL
                    ADDHL(H, L);
                    return 11;
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
                case 58:                                                        //LD A, (nn)
                    A = Spectrum.Memory[Spectrum.Memory[PC++] + Spectrum.Memory[PC++] * 256];
                    return 13;
                case 60:                                                        //INC A
                    INC(ref A);
                    return 4;
                case 61:                                                        //DEC A
                    DEC(ref A);
                    return 4;
                case 62:                                                        //LD A, n
                    A = Spectrum.Memory[PC++];
                    return 7;
                case 63:                                                        //CCF
                    fC ^= true;
                    return 4;
                case 68:                                                        //LD B, H
                    B = H;
                    return 4;
                case 71:                                                        //LD B, A
                    B = A;
                    return 4;
                case 77:                                                        //LD C, L
                    C = L;
                    return 4;
                case 78:                                                        //LD C, (HL)
                    C = Spectrum.Memory[H * 256 + L];
                    return 7;
                case 79:                                                        //LD C, A
                    C = A;
                    return 4;
                case 84:                                                        //LD D, H
                    D = H;
                    return 4;
                case 86:                                                        //LD D, (HL)
                    D = Spectrum.Memory[H * 256 + L];
                    return 7;
                case 87:                                                        //LD D, A
                    D = A;
                    return 4;
                case 93:                                                        //LD E, L
                    E = L;
                    return 4;
                case 94:                                                        //LD E, (HL)
                    E = Spectrum.Memory[H * 256 + L];
                    return 7;
                case 95:                                                        //LD E, A
                    E = A;
                    return 4;
                case 97:                                                        //LD H, C
                    H = C;
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
                case 104:                                                       //LD L, B
                    L = B;
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
                case 124:                                                       //LD A, H
                    A = H;
                    return 4;
                case 126:                                                       //LD A, (HL)
                    A = Spectrum.Memory[H * 256 + L];
                    return 7;
                case 135:                                                       //ADD A, A
                    ADD(ref A, A);
                    return 4;
                case 144:                                                       //SUB B
                    SUB(B);
                    return 4;
                case 145:                                                       //SUB C
                    SUB(C);
                    return 4;
                case 159:                                                       //SBC A, A
                    SBCA(A);
                    return 4;
                case 160:                                                       //AND B
                    AND(B);
                    return 4;
                case 162:                                                       //AND D
                    AND(D);
                    return 4;
                case 167:                                                       //AND A
                    AND(A);
                    return 4;
                case 169:                                                       //XOR C
                    XOR(C);
                    return 4;
                case 171:                                                       //XOR E
                    XOR(E);
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
                case 192:                                                       //RET NZ
                    if (!fZ) { RET(); return 5; }
                    else return 11;
                case 193:                                                       //POP BC
                    C = Spectrum.Memory[SP++];
                    B = Spectrum.Memory[SP++];
                    return 10;
                case 194:                                                       //JP NZ, nn
                    if (!fZ) PC = (ushort)(Spectrum.Memory[PC] + Spectrum.Memory[PC + 1] * 256);
                    return 10;
                case 195:                                                       //JP nn
                    PC = (ushort)(Spectrum.Memory[PC] + Spectrum.Memory[PC + 1] * 256);
                    return 10;
                case 197:                                                       //PUSH BC
                    Spectrum.Memory[--SP] = B;
                    Spectrum.Memory[--SP] = C;
                    return 11;
                case 198:                                                       //AAD A, n
                    ADD(ref A, Spectrum.Memory[PC++]);
                    return 7;
                case 200:                                                       //RET Z
                    if (fZ) { RET(); return 5; }
                    else return 11;
                case 201:                                                       //RET
                    RET();
                    return 10;
                #region case 203 (Префикс CD)
                case 203:                                                       //(Префикс CD)
                    R++;
                    if (R > 127) R = 0;
                    switch (Spectrum.Memory[PC++])
                    {
                        case 126:                                               //BIT 7, (HL)
                            BIT(Spectrum.Memory[H * 256 + L], 7);
                            return 12;
                        case 134:                                               //RES 0, (HL)
                            RES(ref Spectrum.Memory[H * 256 + L], 0);
                            return 15;
                        case 174:                                               //RES 5, (HL)
                            RES(ref Spectrum.Memory[H * 256 + L], 5);
                            return 15;
                        case 198:                                               //SET 0, (HL)
                            SET(ref Spectrum.Memory[H * 256 + L], 0);
                            return 15;
                    }
                    break;
                #endregion
                case 204:                                                       //CALL Z, nn
                    if (fZ)
                    {
                        Spectrum.Memory[--SP] = (byte)((PC + 2) / 256);
                        Spectrum.Memory[--SP] = (byte)((PC + 2) % 256);
                        PC = (ushort)(Spectrum.Memory[PC] + Spectrum.Memory[PC + 1] * 256);
                        return 10;
                    }
                    else
                    {
                        PC += 2;
                        return 17;
                    }
                case 205:                                                       //CALL nn
                    Spectrum.Memory[--SP] = (byte)((PC + 2) / 256);
                    Spectrum.Memory[--SP] = (byte)((PC + 2) % 256);
                    PC = (ushort)(Spectrum.Memory[PC] + Spectrum.Memory[PC + 1] * 256);
                    return 17;
                case 208:                                                       //RET NC
                    if (!fC) { RET(); return 5; }
                    else return 11;
                case 209:                                                       //POP DE
                    E = Spectrum.Memory[SP++];
                    D = Spectrum.Memory[SP++];
                    return 10;
                case 210:                                                       //JP NC, nn
                    if (!fC) PC = (ushort)(Spectrum.Memory[PC] + Spectrum.Memory[PC + 1] * 256);
                    return 10;
                case 211:                                                       //OUT (n), A
                    OUT(Spectrum.Memory[PC++], A);
                    return 11;
                case 213:                                                       //PUSH DE
                    Spectrum.Memory[--SP] = D;
                    Spectrum.Memory[--SP] = E;
                    return 11;
                case 214:                                                       //SUB n
                    SUB(Spectrum.Memory[PC++]);
                    return 7;
                case 215:                                                       //RST 10
                    Spectrum.Memory[--SP] = (byte)((PC) / 256);
                    Spectrum.Memory[--SP] = (byte)((PC) % 256);
                    PC = 16;
                    return 10;
                case 216:                                                       //RET C
                    if (fC) { RET(); return 5; }
                    else return 11;
                case 217:                                                       //EXX
                    EXX();
                    return 4;
                case 121:                                                       //LD A, C
                    A = C;
                    return 4;
                case 225:                                                       //POP HL
                    L = Spectrum.Memory[SP++];
                    H = Spectrum.Memory[SP++];
                    return 10;
                case 227:                                                       //EX (SP), HL
                    t = Spectrum.Memory[SP]; Spectrum.Memory[SP] = L; L = t;
                    t = Spectrum.Memory[SP + 1]; Spectrum.Memory[SP + 1] = H; H = t;
                    return 19;
                case 229:                                                       //PUSH HL
                    Spectrum.Memory[--SP] = H;
                    Spectrum.Memory[--SP] = L;
                    return 11;
                case 230:                                                       //AND n
                    AND(Spectrum.Memory[PC++]);
                    return 7;
                case 233:                                                       //JP (HL)
                    PC = (ushort)(H * 256 + L);
                    return 4;
                case 235:                                                       //EX DE, HL
                    t = D; D = H; H = t;
                    t = E; E = L; L = t;
                    return 4;
                #region case 237 (Префикс ED)
                case 237:                                                       //Префикс ED
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
                        case 75:                                                //LD BC, (nn)
                            tmp = (ushort)(Spectrum.Memory[PC++] + Spectrum.Memory[PC++] * 256);
                            C = Spectrum.Memory[tmp++];
                            B = Spectrum.Memory[tmp];
                            return 20;
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
                        case 91:                                                //LD DE, (nn)
                            tmp = (ushort)(Spectrum.Memory[PC++] + Spectrum.Memory[PC++] * 256);
                            E = Spectrum.Memory[tmp++];
                            D = Spectrum.Memory[tmp];
                            return 20;
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
                case 241:                                                       //POP AF
                    GetFlags(Spectrum.Memory[SP++]);
                    A = Spectrum.Memory[SP++];
                    return 10;
                case 243:                                                       //DI
                    Prer = false;
                    return 40;
                case 245:                                                       //PUSH AF
                    Spectrum.Memory[--SP] = A;
                    Spectrum.Memory[--SP] = F();
                    return 11;
                case 246:                                                       //OR n
                    OR(Spectrum.Memory[PC++]);
                    return 7;
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
                            DEC(ref Spectrum.Memory[IplusS(IY)]);
                            return 23;
                        case 54:                                                //LD (IY+S), N
                            Spectrum.Memory[IplusS(IY)] = Spectrum.Memory[PC++];
                            return 19;
                        case 70:                                                //LD B, (IY+S)
                            B = Spectrum.Memory[IplusS(IY)];
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
                                    BIT(Spectrum.Memory[IplusS4(IY)], 0);
                                    return 23;
                                case 78:                                        //BIT 1, (IY+S)
                                    BIT(Spectrum.Memory[IplusS4(IY)], 1);
                                    return 23;
                                case 102:                                       //BIT 4, (IY+S)
                                    BIT(Spectrum.Memory[IplusS4(IY)], 4);
                                    return 23;
                                case 118:                                       //BIT 6, (IY+S)
                                    BIT(Spectrum.Memory[IplusS4(IY)], 6);
                                    return 23;
                                case 134:                                       //RES 0, (IY+S)
                                    RES(ref Spectrum.Memory[IplusS4(IY)], 0);
                                    return 23;
                                case 142:                                       //RES 1, (IY+S)
                                    RES(ref Spectrum.Memory[IplusS4(IY)], 1);
                                    return 23;
                                case 166:                                       //RES 4, (IY+S)
                                    RES(ref Spectrum.Memory[IplusS4(IY)], 4);
                                    return 23;
                                case 174:                                       //RES 5, (IY+S)
                                    RES(ref Spectrum.Memory[IplusS4(IY)], 5);
                                    return 23;
                                case 198:                                       //SET 0, (IY+S)
                                    SET(ref Spectrum.Memory[IplusS4(IY)], 0);
                                    return 23;
                                case 206:                                       //SET 1, (IY+S)
                                    SET(ref Spectrum.Memory[IplusS4(IY)], 1);
                                    return 23;
                                case 230:                                       //SET 4, (IY+S)
                                    SET(ref Spectrum.Memory[IplusS4(IY)], 4); return 23;
                            }
                            break;
                    }
                    break;
                #endregion
                case 254:                                                       //CP n
                    CP(Spectrum.Memory[PC++]);
                    return 7;
            }
            PC = pc;
            return 1;
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
            byte to = Spectrum.Memory[PC];
            PC = (ushort)(PC + to + 1);
            if (to > 127) PC -= 256;
        }
        //RET
        static void RET()
        {
            //PC = (ushort)(Spectrum.Memory[SP] + Spectrum.Memory[SP + 1] * 256);
            //SP += 2;
            PC = (ushort)(Spectrum.Memory[SP++] + Spectrum.Memory[SP++] * 256);
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
