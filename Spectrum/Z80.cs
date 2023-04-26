using System;

namespace Spectrum
{
    static class Z80
    {
        //Маски для проверки флагов
        const byte m3 = 8;          //3-ий флаг
        const byte m5 = 32;         //5-ый флаг
        const byte mS = 128;        //Знак
        const byte mH = 16;         //Полуперенос
        const int mHH = 4096;       //Полуперенос 16bit
        const byte mF = 15;
        const int mFF = 255;
        const int mFFF = 4095;
        const int mFFFF = 65535;

        public static byte[] RAM = new byte[65536];
        public static bool[] Be = new bool[65536];
        public static byte A, B, C, D, E, H, L, I, R;
        public static byte Aa, Fa, Ba, Ca, Da, Ea, Ha, La;
        public static UInt16 PC, SP, IX, IY;
        public static bool fS, fZ, f5, fH, f3, fV, fN, fC;
        public static byte IM;
        public static bool intIFF2;
        public static byte[] IN = new byte[65536];
        public static byte F
        {
            get
            {
                byte reg = 0;
                if (fS) reg += 128;
                if (fZ) reg += 64;
                if (f5) reg += 32;
                if (fH) reg += 16;
                if (f3) reg += 8;
                if (fV) reg += 4;
                if (fN) reg += 2;
                if (fC) reg += 1;
                return reg;
            }
            set
            {
                fS = (value & 128) != 0;
                fZ = (value & 64) != 0;
                f5 = (value & 32) != 0;
                fH = (value & 16) != 0;
                f3 = (value & 8) != 0;
                fV = (value & 4) != 0;
                fN = (value & 2) != 0;
                fC = (value & 1) != 0;
            }
        }
        static ushort BC { get { return (ushort)(B * 256 + C); } set { B = (byte)(value / 256); C = (byte)value; } }
        static ushort DE { get { return (ushort)(D * 256 + E); } set { D = (byte)(value / 256); E = (byte)value; } }
        static ushort HL { get { return (ushort)(H * 256 + L); } set { H = (byte)(value / 256); L = (byte)value; } }
        /// <summary>
        /// Сброс
        /// </summary>
        public static void Reset()
        {
            A = 0; F = 0; B = 0; C = 0; D = 0; E = 0; H = 0; L = 0;
            Aa = 0; Fa = 0; Ba = 0; Ca = 0; Da = 0; Ea = 0; Ha = 0; La = 0;
            PC = 0; SP = 0; IX = 0; IY = 0; I = 0; R = 0;
            IN[65278] = 255;
            IN[65022] = 255;
            IN[64510] = 255;
            IN[63486] = 255;
            IN[61438] = 255;
            IN[57342] = 255;
            IN[49150] = 255;
            IN[32766] = 255;
        }

        static byte t;

        public static int Run()
        {
            Be[PC] = true;
            ushort pc = PC;
            ushort tmp;
            R++;
            if (R > 127) R = 0;
            switch (RAM[PC++])
            {
                case 0: return 4;                                               //NOP
                case 1: C = RAM[PC++]; B = RAM[PC++]; return 10;                //LD BC,nn
                case 2: RAM[BC] = A; return 7;                                  //LD (BC),A
                case 3: INC(ref B, ref C); return 6;                            //INC BC
                case 4: INC(ref B); return 4;                                   //INC B
                case 5: DEC(ref B); return 4;                                   //DEC B
                case 6: B = RAM[PC++]; return 7;                                //LD B,n
                case 7: RLCA(); return 4;                                       //RLCA
                case 8: EXAF(); return 4;                                       //EX AF,AF'
                case 9: HL = ADD(HL, BC); return 11;                            //ADD HL,BC
                case 10: A = RAM[BC]; return 7;                                 //LD A,(BC);
                case 11: DEC(ref B, ref C); return 6;                           //DEC BC
                case 12: INC(ref C); return 4;                                  //INC C
                case 13: DEC(ref C); return 4;                                  //DEC C
                case 14: C = RAM[PC++]; return 7;                               //LD C,n
                case 15: RRCA(); return 4;                                      //RRCA
                case 16: B--; return JR(B != 0) + 1;                            //DJNZ s
                case 17: E = RAM[PC++]; D = RAM[PC++]; return 10;               //LD DE,nn
                case 18: RAM[DE] = A; return 7;                                 //LD (DE),A
                case 19: INC(ref D, ref E); return 6;                           //INC DE
                case 20: INC(ref D); return 4;                                  //INC D
                case 21: DEC(ref D); return 4;                                  //DEC D
                case 22: D = RAM[PC++]; return 7;                               //LD D,n
                case 23: RLA(); return 4;                                       //RLA
                case 24: JR(true); return 4;                                    //JR s
                case 25: HL = ADD(HL, DE); return 11;                           //ADD HL,DE
                case 26: A = RAM[DE]; return 7;                                 //LD A,(DE)
                case 27: DEC(ref D, ref E); return 6;                           //DEC DE
                case 28: INC(ref E); return 4;                                  //INC E
                case 29: DEC(ref E); return 4;                                  //DEC E
                case 30: E = RAM[PC++]; return 7;                               //LD E,n
                case 31: RRA(); return 4;                                       //RRA
                case 32: return JR(!fZ);                                        //JR NZ,s
                case 33: L = RAM[PC++]; H = RAM[PC++]; return 10;               //LD HL,nn
                case 34: POKE(HL); return 20;                                   //LD (nn),HL
                case 35: INC(ref H, ref L); return 6;                           //INC HL
                case 36: INC(ref H); return 4;                                  //INC H
                case 37: DEC(ref H); return 4;                                  //DEC H
                case 38: H = RAM[PC++]; return 7;                               //LD H,n
                case 39: DAA(); return 4;                                       //DAA
                case 40: return JR(fZ);                                         //JR Z,s
                case 41: HL = ADD(HL, HL); return 11;                           //ADD HL,HL
                case 42: HL = PEEK(); return 16;                                //LD HL,(nn)
                case 43: DEC(ref H, ref L); return 6;                           //DEC HL
                case 44: INC(ref L); return 4;                                  //INC L
                case 45: DEC(ref L); return 4;                                  //DEC L
                case 46: L = RAM[PC++]; return 7;                               //LD L,n
                case 47: CPL(); return 4;                                       //CPL
                case 48: return JR(!fC);                                        //JR NC,n
                case 49: SP = (ushort)(RAM[PC++] + RAM[PC++] * 256); return 10; //LD SP,nn
                case 50: RAM[RAM[PC++] + RAM[PC++] * 256] = A; return 13;       //LD (nn),A
                case 51: SP++; return 6;                                        //INC SP
                case 52: INC(ref RAM[HL]); return 11;                           //INC (HL)
                case 53: DEC(ref RAM[HL]); return 11;                           //DEC (HL)
                case 54: RAM[HL] = RAM[PC++]; return 10;                        //LD (HL),n
                case 55: SCF(); return 4;                                       //SCF
                case 56: return JR(fC);                                         //JR C,n
                case 57: HL = ADD(HL, SP); return 11;                           //ADD HL,SP
                case 58: A = RAM[RAM[PC++] + RAM[PC++] * 256]; return 13;       //LD A,(nn)
                case 59: SP--; return 6;                                        //DEC SP
                case 60: INC(ref A); return 4;                                  //INC A
                case 61: DEC(ref A); return 4;                                  //DEC A
                case 62: A = RAM[PC++]; return 7;                               //LD A,n
                case 63: CCF(); return 4;                                       //CCF
                case 64: return 4;                                              //LD B,B
                case 65: B = C; return 4;                                       //LD B,C
                case 66: B = D; return 4;                                       //LD B,D
                case 67: B = E; return 4;                                       //LD B,E
                case 68: B = H; return 4;                                       //LD B,H
                case 69: B = L; return 4;                                       //LD B,L
                case 70: B = RAM[HL]; return 7;                                 //LD B,(HL)
                case 71: B = A; return 4;                                       //LD B,A
                case 72: C = B; return 4;                                       //LD C,B
                case 73: return 4;                                              //LD C,C
                case 74: C = D; return 4;                                       //LD C,D
                case 75: C = E; return 4;                                       //LD C,E
                case 76: C = H; return 4;                                       //LD C,H
                case 77: C = L; return 4;                                       //LD C,L
                case 78: C = RAM[HL]; return 7;                                 //LD C,(HL)
                case 79: C = A; return 4;                                       //LD C,A
                case 80: D = B; return 4;                                       //LD D,B
                case 81: D = C; return 4;                                       //LD D,C
                case 82: return 4;                                              //LD D,D
                case 83: D = E; return 4;                                       //LD D,E
                case 84: D = H; return 4;                                       //LD D,H
                case 85: D = L; return 4;                                       //LD D,H
                case 86: D = RAM[HL]; return 7;                                 //LD D,(HL)
                case 87: D = A; return 4;                                       //LD D,A
                case 88: E = B; return 4;                                       //LD E,B
                case 89: E = C; return 4;                                       //LD E,C
                case 90: E = D; return 4;                                       //LD E,D
                case 91: return 4;                                              //LD E,E
                case 92: E = H; return 4;                                       //LD E,H
                case 93: E = L; return 4;                                       //LD E,L
                case 94: E = RAM[HL]; return 7;                                 //LD E,(HL)
                case 95: E = A; return 4;                                       //LD E,A
                case 96: H = B; return 4;                                       //LD H,B
                case 97: H = C; return 4;                                       //LD H,C
                case 98: H = D; return 4;                                       //LD H,D
                case 99: H = E; return 4;                                       //LD H,E
                case 100: return 4;                                             //LD H,H
                case 101: H = L; return 4;                                      //LD H,L
                case 102: H = RAM[HL]; return 7;                                //LD H,(HL)
                case 103: H = A; return 4;                                      //LD H,A
                case 104: L = B; return 4;                                      //LD L,B
                case 105: L = C; return 4;                                      //LD L,C
                case 106: L = D; return 4;                                      //LD L,D
                case 107: L = E; return 4;                                      //LD L,E
                case 108: L = H; return 4;                                      //LD L,H
                case 109: return 4;                                             //LD L,L
                case 110: L = RAM[HL]; return 7;                                //LD L,(HL)
                case 111: L = A; return 4;                                      //LD L,A
                case 112: RAM[HL] = B; return 7;                                //LD (HL),B
                case 113: RAM[HL] = C; return 7;                                //LD (HL),C
                case 114: RAM[HL] = D; return 7;                                //LD (HL),D
                case 115: RAM[HL] = E; return 7;                                //LD (HL),E
                case 116: RAM[HL] = H; return 7;                                //LD (HL),H
                case 117: RAM[HL] = L; return 7;                                //LD (HL),L
                case 118: fC = true; PC--; return 4;                            //HALT
                case 119: RAM[HL] = A; return 7;                                //LD (HL),A
                case 120: A = B; return 4;                                      //LD A,B
                case 121: A = C; return 4;                                      //LD A,C
                case 122: A = D; return 4;                                      //LD A,D
                case 123: A = E; return 4;                                      //LD A,E
                case 124: A = H; return 4;                                      //LD A,H
                case 125: A = L; return 4;                                      //LD A,L
                case 126: A = RAM[HL]; return 7;                                //LD A,(HL)
                case 127: return 4;                                             //LD A,A
                case 128: ADD(B); return 4;                                     //ADD A,B
                case 129: ADD(C); return 4;                                     //ADD A,C
                case 130: ADD(D); return 4;                                     //ADD A,D
                case 131: ADD(E); return 4;                                     //ADD A,E
                case 132: ADD(H); return 4;                                     //ADD A,H
                case 133: ADD(L); return 4;                                     //ADD A,L
                case 134: ADD(RAM[HL]); return 7;                               //ADD A,(HL)
                case 135: ADD(A); return 4;                                     //ADD A,A
                case 136: ADC(B); return 4;                                     //ADC A,B
                case 137: ADC(C); return 4;                                     //ADC A,C
                case 138: ADC(D); return 4;                                     //ADC A,D
                case 139: ADC(E); return 4;                                     //ADC A,E
                case 140: ADC(H); return 4;                                     //ADC A,H
                case 141: ADC(L); return 4;                                     //ADC A,L
                case 142: ADC(RAM[HL]); return 7;                               //ADC A,(HL)
                case 143: ADC(A); return 4;                                     //ADC A,A
                case 144: SUB(B); return 4;                                     //SUB B
                case 145: SUB(C); return 4;                                     //SUB C
                case 146: SUB(D); return 4;                                     //SUB D
                case 147: SUB(E); return 4;                                     //SUB E
                case 148: SUB(H); return 4;                                     //SUB H
                case 149: SUB(L); return 4;                                     //SUB L
                case 150: SUB(RAM[HL]); return 7;                               //SUB (HL)
                case 151: SUB(A); return 4;                                     //SUB A
                case 152: SBC(B); return 4;                                     //SBC A,B
                case 153: SBC(C); return 4;                                     //SBC A,C
                case 154: SBC(D); return 4;                                     //SBC A,D
                case 155: SBC(E); return 4;                                     //SBC A,E
                case 156: SBC(H); return 4;                                     //SBC A,H
                case 157: SBC(L); return 4;                                     //SBC A,L
                case 158: SBC(RAM[HL]); return 7;                               //SBC A,(HL)
                case 159: SBC(A); return 4;                                     //SBC A,A
                case 160: AND(B); return 4;                                     //AND B
                case 161: AND(C); return 4;                                     //AND C
                case 162: AND(D); return 4;                                     //AND D
                case 163: AND(E); return 4;                                     //AND E
                case 164: AND(H); return 4;                                     //AND H
                case 165: AND(L); return 4;                                     //AND L
                case 166: AND(RAM[HL]); return 7;                               //AND (HL)
                case 167: AND(A); return 4;                                     //AND A
                case 168: XOR(B); return 4;                                     //XOR B
                case 169: XOR(C); return 4;                                     //XOR C
                case 170: XOR(D); return 4;                                     //XOR D
                case 171: XOR(E); return 4;                                     //XOR E
                case 172: XOR(H); return 4;                                     //XOR H
                case 173: XOR(L); return 4;                                     //XOR L
                case 174: XOR(RAM[HL]); return 7;                               //XOR (HL)
                case 175: XOR(A); return 4;                                     //XOR A
                case 176: OR(B); return 4;                                      //OR B
                case 177: OR(C); return 4;                                      //OR C
                case 178: OR(D); return 4;                                      //OR D
                case 179: OR(E); return 4;                                      //OR E
                case 180: OR(H); return 4;                                      //OR H
                case 181: OR(L); return 4;                                      //OR L
                case 182: OR(RAM[HL]); return 4;                                //OR (HL)
                case 183: OR(A); return 4;                                      //OR A
                case 184: CP(B); return 4;                                      //CP B
                case 185: CP(C); return 4;                                      //CP C
                case 186: CP(D); return 4;                                      //CP D
                case 187: CP(E); return 4;                                      //CP E
                case 188: CP(H); return 4;                                      //CP H
                case 189: CP(L); return 4;                                      //CP L
                case 190: CP(RAM[HL]); return 7;                                //CP (HL)
                case 191: CP(A); return 4;                                      //CP A
                case 192: return RET(!fZ);                                      //RET NZ
                case 193: return POP(ref B, ref C);                             //POP BC
                case 194: return JP(!fZ);                                       //JP NZ, nn
                case 195: return JP(true);                                      //JP nn
                case 196: return CALL(!fZ);                                     //CALL NZ, nn
                case 197: return PUSH(B, C);                                    //PUSH BC
                case 198: ADD(RAM[PC++]); return 7;                             //ADD A,n
                case 199: return RST(0);                                        //RST 0
                case 200: return RET(fZ);                                       //RET Z
                case 201: RET(true); return 10;                                 //RET
                case 202: return JP(fZ);                                        //JP Z,nn
                #region case 203 (Префикс CB)
                case 203:                                                       //-------------------- Префикс CB
                    R++;
                    if (R > 127) R = 0;
                    switch (RAM[PC++])
                    {
                        case 0: RLC(ref B); return 8;                           //RLC B
                        case 1: RLC(ref C); return 8;                           //RLC C
                        case 2: RLC(ref D); return 8;                           //RLC D
                        case 3: RLC(ref E); return 8;                           //RLC E
                        case 4: RLC(ref H); return 8;                           //RLC H
                        case 5: RLC(ref L); return 8;                           //RLC L
                        case 6: RLC(ref RAM[HL]); return 8;                     //RLC (HL)
                        case 7: RLC(ref A); return 8;                           //RLC A
                        case 8: RRC(ref B); return 8;                           //RRC B
                        case 9: RRC(ref C); return 8;                           //RRC C
                        case 10: RRC(ref D); return 8;                          //RRC D
                        case 11: RRC(ref E); return 8;                          //RRC E
                        case 12: RRC(ref H); return 8;                          //RRC H
                        case 13: RRC(ref L); return 8;                          //RRC L
                        case 14: RRC(ref RAM[HL]); return 8;                    //RRC (HL)
                        case 15: RRC(ref A); return 8;                          //RRC A
                        case 16: RL(ref B); return 8;                           //RL B
                        case 17: RL(ref C); return 8;                           //RL C
                        case 18: RL(ref D); return 8;                           //RL D
                        case 19: RL(ref E); return 8;                           //RL E
                        case 20: RL(ref H); return 8;                           //RL H
                        case 21: RL(ref L); return 8;                           //RL L
                        case 22: RL(ref RAM[HL]); return 15;                    //RL (HL)
                        case 23: RL(ref A); return 8;                           //RL A
                        case 24: RR(ref B); return 8;                           //RR B
                        case 25: RR(ref C); return 8;                           //RR C
                        case 26: RR(ref D); return 8;                           //RR D
                        case 27: RR(ref E); return 8;                           //RR E
                        case 28: RR(ref H); return 8;                           //RR H
                        case 29: RR(ref L); return 8;                           //RR L
                        case 30: RR(ref RAM[HL]); return 8;                     //RR (HL)
                        case 31: RR(ref A); return 8;                           //RR A
                        case 32: SLA(ref B); return 8;                          //SLA B
                        case 33: SLA(ref C); return 8;                          //SLA C
                        case 34: SLA(ref D); return 8;                          //SLA D
                        case 35: SLA(ref E); return 8;                          //SLA E
                        case 36: SLA(ref H); return 8;                          //SLA H
                        case 37: SLA(ref L); return 8;                          //SLA L
                        case 38: SLA(ref RAM[HL]); return 8;                    //SLA (HL)
                        case 39: SLA(ref A); return 8;                          //SLA A
                        case 40: SRA(ref B); return 8;                          //SRA B
                        case 41: SRA(ref C); return 8;                          //SRA C
                        case 42: SRA(ref D); return 8;                          //SRA D
                        case 43: SRA(ref E); return 8;                          //SRA E
                        case 44: SRA(ref H); return 8;                          //SRA H
                        case 45: SRA(ref L); return 8;                          //SRA L
                        case 46: SRA(ref RAM[HL]); return 8;                    //SRA (HL)
                        case 47: SRA(ref A); return 8;                          //SRA A
                        //48-55 - наверное SLS
                        case 56: SRL(ref B); return 8;                          //SRL B
                        case 57: SRL(ref C); return 8;                          //SRL C
                        case 58: SRL(ref D); return 8;                          //SRL D
                        case 59: SRL(ref E); return 8;                          //SRL E
                        case 60: SRL(ref H); return 8;                          //SRL H
                        case 61: SRL(ref L); return 8;                          //SRL L
                        case 62: SRL(ref RAM[HL]); return 8;                    //SRL (HL)
                        case 63: SRL(ref A); return 8;                          //SRL A
                        case 64: BIT(B, 1); return 8;                           //BIT 0,B
                        case 65: BIT(C, 1); return 8;                           //BIT 0,C
                        case 66: BIT(D, 1); return 8;                           //BIT 0,D
                        case 67: BIT(E, 1); return 8;                           //BIT 0,E
                        case 68: BIT(H, 1); return 8;                           //BIT 0,H
                        case 69: BIT(L, 1); return 8;                           //BIT 0,L
                        case 70: BIT(RAM[HL], 1); return 8;                     //BIT 0,(HL)
                        case 71: BIT(A, 1); return 8;                           //BIT 0,A
                        case 72: BIT(B, 2); return 8;                           //BIT 1,B
                        case 73: BIT(C, 2); return 8;                           //BIT 1,C
                        case 74: BIT(D, 2); return 8;                           //BIT 1,D
                        case 75: BIT(E, 2); return 8;                           //BIT 1,E
                        case 76: BIT(H, 2); return 8;                           //BIT 1,H
                        case 77: BIT(L, 2); return 8;                           //BIT 1,L
                        case 78: BIT(RAM[HL], 2); return 8;                     //BIT 1,(HL)
                        case 79: BIT(A, 2); return 8;                           //BIT 1,A
                        case 80: BIT(B, 4); return 8;                           //BIT 2,B
                        case 81: BIT(C, 4); return 8;                           //BIT 2,C
                        case 82: BIT(D, 4); return 8;                           //BIT 2,D
                        case 83: BIT(E, 4); return 8;                           //BIT 2,E
                        case 84: BIT(H, 4); return 8;                           //BIT 2,H
                        case 85: BIT(L, 4); return 8;                           //BIT 2,L
                        case 86: BIT(RAM[HL], 4); return 8;                     //BIT 2,(HL)
                        case 87: BIT(A, 4); return 8;                           //BIT 2,A
                        case 88: BIT(B, 8); return 8;                           //BIT 3,B
                        case 89: BIT(C, 8); return 8;                           //BIT 3,C
                        case 90: BIT(D, 8); return 8;                           //BIT 3,D
                        case 91: BIT(E, 8); return 8;                           //BIT 3,E
                        case 92: BIT(H, 8); return 8;                           //BIT 3,H
                        case 93: BIT(L, 8); return 8;                           //BIT 3,L
                        case 94: BIT(RAM[HL], 8); return 8;                     //BIT 3,(HL)
                        case 95: BIT(A, 8); return 8;                           //BIT 3,A
                        case 96: BIT(B, 16); return 8;                          //BIT 4,B
                        case 97: BIT(C, 16); return 8;                          //BIT 4,C
                        case 98: BIT(D, 16); return 8;                          //BIT 4,D
                        case 99: BIT(E, 16); return 8;                          //BIT 4,E
                        case 100: BIT(H, 16); return 8;                         //BIT 4,H
                        case 101: BIT(L, 16); return 8;                         //BIT 4,L
                        case 102: BIT(RAM[HL], 16); return 8;                   //BIT 4,(HL)
                        case 103: BIT(A, 16); return 8;                         //BIT 4,A
                        case 104: BIT(B, 32); return 8;                         //BIT 5,B
                        case 105: BIT(C, 32); return 8;                         //BIT 5,C
                        case 106: BIT(D, 32); return 8;                         //BIT 5,D
                        case 107: BIT(E, 32); return 8;                         //BIT 5,E
                        case 108: BIT(H, 32); return 8;                         //BIT 5,H
                        case 109: BIT(L, 32); return 8;                         //BIT 5,L
                        case 110: BIT(RAM[HL], 32); return 8;                   //BIT 5,(HL)
                        case 111: BIT(A, 32); return 8;                         //BIT 5,A
                        case 112: BIT(B, 64); return 8;                         //BIT 6,B
                        case 113: BIT(C, 64); return 8;                         //BIT 6,C
                        case 114: BIT(D, 64); return 8;                         //BIT 6,D
                        case 115: BIT(E, 64); return 8;                         //BIT 6,E
                        case 116: BIT(H, 64); return 8;                         //BIT 6,H
                        case 117: BIT(L, 64); return 8;                         //BIT 6,L
                        case 118: BIT(RAM[HL], 64); return 8;                   //BIT 6,(HL)
                        case 119: BIT(A, 64); return 8;                         //BIT 6,A
                        case 120: BIT(B, 128); return 8;                        //BIT 7,B
                        case 121: BIT(C, 128); return 8;                        //BIT 7,C
                        case 122: BIT(D, 128); return 8;                        //BIT 7,D
                        case 123: BIT(E, 128); return 8;                        //BIT 7,E
                        case 124: BIT(H, 128); return 8;                        //BIT 7,H
                        case 125: BIT(L, 128); return 8;                        //BIT 7,L
                        case 126: BIT(RAM[HL], 128); return 8;                  //BIT 7,(HL)
                        case 127: BIT(A, 128); return 8;                        //BIT 7,A
                        case 128: RES(ref B, 0); return 8;                      //RES 0,B
                        case 129: RES(ref C, 0); return 8;                      //RES 0,C
                        case 130: RES(ref D, 0); return 8;                      //RES 0,D
                        case 131: RES(ref E, 0); return 8;                      //RES 0,E
                        case 132: RES(ref H, 0); return 8;                      //RES 0,H
                        case 133: RES(ref L, 0); return 8;                      //RES 0,L
                        case 134: RES(ref RAM[HL], 0); return 15;               //RES 0,(HL)
                        case 135: RES(ref A, 0); return 8;                      //RES 0,A
                        case 136: RES(ref B, 1); return 8;                      //RES 1,B
                        case 137: RES(ref C, 1); return 8;                      //RES 1,C
                        case 138: RES(ref D, 1); return 8;                      //RES 1,D
                        case 139: RES(ref E, 1); return 8;                      //RES 1,E
                        case 140: RES(ref H, 1); return 8;                      //RES 1,H
                        case 141: RES(ref L, 1); return 8;                      //RES 1,L
                        case 142: RES(ref RAM[HL], 1); return 15;               //RES 1,(HL)
                        case 143: RES(ref A, 1); return 8;                      //RES 1,A
                        case 144: RES(ref B, 2); return 8;                      //RES 2,B
                        case 145: RES(ref C, 2); return 8;                      //RES 2,C
                        case 146: RES(ref D, 2); return 8;                      //RES 2,D
                        case 147: RES(ref E, 2); return 8;                      //RES 2,E
                        case 148: RES(ref H, 2); return 8;                      //RES 2,H
                        case 149: RES(ref L, 2); return 8;                      //RES 2,L
                        case 150: RES(ref RAM[HL], 2); return 15;               //RES 2,(HL)
                        case 151: RES(ref A, 2); return 8;                      //RES 2,A
                        case 152: RES(ref B, 3); return 8;                      //RES 3,B
                        case 153: RES(ref C, 3); return 8;                      //RES 3,C
                        case 154: RES(ref D, 3); return 8;                      //RES 3,D
                        case 155: RES(ref E, 3); return 8;                      //RES 3,E
                        case 156: RES(ref H, 3); return 8;                      //RES 3,H
                        case 157: RES(ref L, 3); return 8;                      //RES 3,L
                        case 158: RES(ref RAM[HL], 3); return 15;               //RES 3,(HL)
                        case 159: RES(ref A, 3); return 8;                      //RES 3,A
                        case 160: RES(ref B, 4); return 8;                      //RES 4,B
                        case 161: RES(ref C, 4); return 8;                      //RES 4,C
                        case 162: RES(ref D, 4); return 8;                      //RES 4,D
                        case 163: RES(ref E, 4); return 8;                      //RES 4,E
                        case 164: RES(ref H, 4); return 8;                      //RES 4,H
                        case 165: RES(ref L, 4); return 8;                      //RES 4,L
                        case 166: RES(ref RAM[HL], 4); return 15;               //RES 4,(HL)
                        case 167: RES(ref A, 4); return 8;                      //RES 4,A
                        case 168: RES(ref B, 5); return 8;                      //RES 5,B
                        case 169: RES(ref C, 5); return 8;                      //RES 5,C
                        case 170: RES(ref D, 5); return 8;                      //RES 5,D
                        case 171: RES(ref E, 5); return 8;                      //RES 5,E
                        case 172: RES(ref H, 5); return 8;                      //RES 5,H
                        case 173: RES(ref L, 5); return 8;                      //RES 5,L
                        case 174: RES(ref RAM[HL], 5); return 15;               //RES 5,(HL)
                        case 175: RES(ref A, 5); return 8;                      //RES 5,A
                        case 176: RES(ref B, 6); return 8;                      //RES 6,B
                        case 177: RES(ref C, 6); return 8;                      //RES 6,C
                        case 178: RES(ref D, 6); return 8;                      //RES 6,D
                        case 179: RES(ref E, 6); return 8;                      //RES 6,E
                        case 180: RES(ref H, 6); return 8;                      //RES 6,H
                        case 181: RES(ref L, 6); return 8;                      //RES 6,L
                        case 182: RES(ref RAM[HL], 6); return 15;               //RES 6,(HL)
                        case 183: RES(ref A, 6); return 8;                      //RES 6,A
                        case 184: RES(ref B, 7); return 8;                      //RES 7,B
                        case 185: RES(ref C, 7); return 8;                      //RES 7,C
                        case 186: RES(ref D, 7); return 8;                      //RES 7,D
                        case 187: RES(ref E, 7); return 8;                      //RES 7,E
                        case 188: RES(ref H, 7); return 8;                      //RES 7,H
                        case 189: RES(ref L, 7); return 8;                      //RES 7,L
                        case 190: RES(ref RAM[HL], 7); return 15;               //RES 7,(HL)
                        case 191: RES(ref A, 7); return 8;                      //RES 7,A
                        case 192: SET(ref B, 0); return 8;                      //SET 0,B
                        case 193: SET(ref C, 0); return 8;                      //SET 0,C
                        case 194: SET(ref D, 0); return 8;                      //SET 0,D
                        case 195: SET(ref E, 0); return 8;                      //SET 0,E
                        case 196: SET(ref H, 0); return 8;                      //SET 0,H
                        case 197: SET(ref L, 0); return 8;                      //SET 0,L
                        case 198: SET(ref RAM[HL], 0); return 15;               //SET 0,(HL)
                        case 199: SET(ref A, 0); return 8;                      //SET 0,A
                        case 200: SET(ref B, 1); return 8;                      //SET 1,B
                        case 201: SET(ref C, 1); return 8;                      //SET 1,C
                        case 202: SET(ref D, 1); return 8;                      //SET 1,D
                        case 203: SET(ref E, 1); return 8;                      //SET 1,E
                        case 204: SET(ref H, 1); return 8;                      //SET 1,H
                        case 205: SET(ref L, 1); return 8;                      //SET 1,L
                        case 206: SET(ref RAM[HL], 1); return 15;               //SET 1,(HL)
                        case 207: SET(ref A, 1); return 8;                      //SET 1,A
                        case 208: SET(ref B, 2); return 8;                      //SET 2,B
                        case 209: SET(ref C, 2); return 8;                      //SET 2,C
                        case 210: SET(ref D, 2); return 8;                      //SET 2,D
                        case 211: SET(ref E, 2); return 8;                      //SET 2,E
                        case 212: SET(ref H, 2); return 8;                      //SET 2,H
                        case 213: SET(ref L, 2); return 8;                      //SET 2,L
                        case 214: SET(ref RAM[HL], 2); return 15;               //SET 2,(HL)
                        case 215: SET(ref A, 2); return 8;                      //SET 2,A
                        case 216: SET(ref B, 3); return 8;                      //SET 3,B
                        case 217: SET(ref C, 3); return 8;                      //SET 3,C
                        case 218: SET(ref D, 3); return 8;                      //SET 3,D
                        case 219: SET(ref E, 3); return 8;                      //SET 3,E
                        case 220: SET(ref H, 3); return 8;                      //SET 3,H
                        case 221: SET(ref L, 3); return 8;                      //SET 3,L
                        case 222: SET(ref RAM[HL], 3); return 15;               //SET 3,(HL)
                        case 223: SET(ref A, 3); return 8;                      //SET 3,A
                        case 224: SET(ref B, 4); return 8;                      //SET 4,B
                        case 225: SET(ref C, 4); return 8;                      //SET 4,C
                        case 226: SET(ref D, 4); return 8;                      //SET 4,D
                        case 227: SET(ref E, 4); return 8;                      //SET 4,E
                        case 228: SET(ref H, 4); return 8;                      //SET 4,H
                        case 229: SET(ref L, 4); return 8;                      //SET 4,L
                        case 230: SET(ref RAM[HL], 4); return 15;               //SET 4,(HL)
                        case 231: SET(ref A, 4); return 8;                      //SET 4,A
                        case 232: SET(ref B, 5); return 8;                      //SET 5,B
                        case 233: SET(ref C, 5); return 8;                      //SET 5,C
                        case 234: SET(ref D, 5); return 8;                      //SET 5,D
                        case 235: SET(ref E, 5); return 8;                      //SET 5,E
                        case 236: SET(ref H, 5); return 8;                      //SET 5,H
                        case 237: SET(ref L, 5); return 8;                      //SET 5,L
                        case 238: SET(ref RAM[HL], 5); return 15;               //SET 5,(HL)
                        case 239: SET(ref A, 5); return 8;                      //SET 5,A
                        case 240: SET(ref B, 6); return 8;                      //SET 6,B
                        case 241: SET(ref C, 6); return 8;                      //SET 6,C
                        case 242: SET(ref D, 6); return 8;                      //SET 6,D
                        case 243: SET(ref E, 6); return 8;                      //SET 6,E
                        case 244: SET(ref H, 6); return 8;                      //SET 6,H
                        case 245: SET(ref L, 6); return 8;                      //SET 6,L
                        case 246: SET(ref RAM[HL], 6); return 15;               //SET 6,(HL)
                        case 247: SET(ref A, 6); return 8;                      //SET 6,A
                        case 248: SET(ref B, 7); return 8;                      //SET 7,B
                        case 249: SET(ref C, 7); return 8;                      //SET 7,C
                        case 250: SET(ref D, 7); return 8;                      //SET 7,D
                        case 251: SET(ref E, 7); return 8;                      //SET 7,E
                        case 252: SET(ref H, 7); return 8;                      //SET 7,H
                        case 253: SET(ref L, 7); return 8;                      //SET 7,L
                        case 254: SET(ref RAM[HL], 7); return 15;               //SET 7,(HL)
                        case 255: SET(ref A, 7); return 8;                      //SET 7,A
                    }
                    break;
                #endregion
                case 204: return CALL(fZ);                                      //CALL Z, nn
                case 205: return CALL(true);                                    //CALL nn
                case 206: ADC(RAM[PC++]); return 7;                             //ADC A,n
                case 207: return RST(8);                                        //RST 8
                case 208: return RET(!fC);                                      //RET NC
                case 209: return POP(ref D, ref E);                             //POP DE
                case 210: return JP(!fC);                                       //JP NC,nn
                case 211: OUT(RAM[PC++], A); return 11;                         //OUT (n),A
                case 212: return CALL(!fC);                                     //CALL NC,nn
                case 213: return PUSH(D, E);                                    //PUSH DE
                case 214: SUB(RAM[PC++]); return 7;                             //SUB n
                case 215: return RST(16);                                       //RST 10
                case 216: return RET(fC);                                       //RET C
                case 217: EXX(); return 4;                                      //EXX
                case 218: return JP(fC);                                        //JP C,nn
                case 219: A = IN[RAM[PC] + A * 256]; return 11;                 //IN A,(n)
                case 220: return CALL(fC);                                      //CALL C,nn
                case 221: return IndexOperation(ref IX);                        //-------------------- Префикс DD
                case 222: SBC(RAM[PC++]); return 7;                             //SBC A,n
                case 223: return RST(24);                                       //RST 18
                case 224: return RET(!fV);                                      //RET PO
                case 225: return POP(ref H, ref L);                             //POP HL
                case 226: return JP(!fV);                                       //JP PO,nn
                case 227: EX(ref RAM[SP + 1], ref RAM[SP], ref H, ref L); return 19;    //EX (SP),HL
                case 228: return CALL(!fV);                                     //CALL PO,nn
                case 229: return PUSH(H, L);                                    //PUSH HL
                case 230: AND(RAM[PC++]); return 7;                             //AND n
                case 231: return RST(32);                                       //RST 20
                case 232: return RET(fV);                                       //RET PE
                case 233: PC = (ushort)(HL); return 4;                          //JP (HL)
                case 234: return JP(fV);                                        //JP PE,nn
                case 235: EX(ref D, ref E, ref H, ref L); return 4;             //EX DE,HL
                case 236: return CALL(fV);                                      //CALL PE,nn
                #region case 237 (Префикс ED)                                   
                case 237:                                                       //-------------------- Префикс ED
                    R++;
                    if (R > 127) R = 0;
                    switch (RAM[PC++])
                    {
                        case 64: B = IN[BC]; return 12;                         //IN B,(C)
                        case 65: OUT(BC, B); return 12;                         //OUT (C),B
                        case 66: SBC(BC); return 15;                            //SBC HL,BC
                        case 67: POKE(BC); return 20;                           //LD (nn),BC
                        case 68: NEG(); return 8;                               //NEG
                        //case 69:                                              //RETN
                        case 70: IM = 0; return 8;                              //IM 0
                        case 71: I = A; return 9;                               //LD I,A
                        case 72: C = IN[BC]; return 12;                         //IN C,(C)
                        case 73: OUT(BC, C); return 12;                         //OUT (C),C
                        case 74: ADC(BC); return 8;                             //ADC HL,BC
                        case 75: BC = PEEK(); return 20;                        //LD BC,(nn)
                        case 76: NEG(); return 8;                               //NEG
                        //case 77:                                              //RETI
                        case 79: R = A; return 9;                               //LD R,A
                        case 80: D = IN[BC]; return 12;                         //IN D,(C)
                        case 81: OUT(BC, D); return 12;                         //OUT (C),D
                        case 82: SBC(DE); return 15;                            //SBC HL,DE
                        case 83: POKE(DE); return 20;                           //LD (nn),DE
                        case 84: NEG(); return 8;                               //NEG
                        case 86: IM = 1; return 8;                              //IM 1
                        case 87: A = I; return 9;                               //LD A,I        //Флаги!!!
                        case 88: E = IN[BC]; return 12;                         //IN E,(C)
                        case 89: OUT(BC, E); return 12;                         //OUT (C),E
                        case 90: ADC(DE); return 8;                             //ADC HL,DE
                        case 91: DE = PEEK(); return 20;                        //LD DE,(nn)
                        case 92: NEG(); return 8;                               //NEG
                        case 94: IM = 2; return 8;                              //IM 2
                        case 95: A = R; return 9;                               //LD A,R        //Флаги!!!
                        case 96: H = IN[BC]; return 12;                         //IN H,(C)
                        case 97: OUT(BC, H); return 12;                         //OUT (C),H
                        case 98: SBC(HL); return 15;                            //SBC HL,HL
                        case 99: POKE(HL); return 20;                           //LD (nn),HL
                        //case 103: RRD(); return 18;                             //RRD
                        case 100: NEG(); return 8;                              //NEG
                        case 104: L = IN[BC]; return 12;                        //IN L,(C)
                        case 105: OUT(BC, L); return 12;                        //OUT (C),L
                        case 106: ADC(HL); return 15;                           //ADC HL,HL
                        case 107: HL = PEEK(); return 20;                       //LD HL,(nn)
                        case 108: NEG(); return 8;                              //NEG
                        case 111: RLD(); return 18;                             //RLD
                        case 112: F = IN[BC]; return 12;                        //IN L,(C)
                        case 114: SBC(SP); return 15;                           //SBC HL,SP
                        case 115: POKE(SP); return 20;                          //LD (nn),SP
                        case 116: NEG(); return 8;                              //NEG
                        case 120: A = IN[BC]; return 12;                        //IN A,(C)
                        case 121: OUT(BC, A); return 12;                        //OUT (C),A
                        case 122: ADC(SP); return 11;                           //ADC HL,SP
                        case 123: SP = PEEK(); return 20;                       //LD SP,(nn)
                        case 124: NEG(); return 8;                              //NEG
                        case 168: return LDI(false, true);                      //LDD
                        //case 169:                                             //CPD
                        //case 170:                                             //IND
                        //case 171:                                             //OUTD
                        case 176: return LDI(true, false);                      //LDIR
                        //case 177:                                             //CPIR
                        //case 178:                                             //INIR
                        //case 179:                                             //OTIR
                        case 184: return LDI(true, true);                       //LDDR
                        //case 169:                                             //CPDR
                        //case 170:                                             //INDR
                        //case 171:                                             //OTDR
                    }
                    break;
                #endregion
                case 238: XOR(RAM[PC++]); return 7;                             //XOR n
                case 239: return RST(40);                                       //RST 28
                case 240: return RET(!fS);                                      //RET P
                case 241: F = RAM[SP++]; A = RAM[SP++]; return 10;              //POP AF
                case 242: return JP(!fS);                                       //JP P,nn
                case 243: IM = 0; return 40;                                    //DI
                case 244: return CALL(!fS);                                     //CALL P,nn
                case 245: return PUSH(A, F);                                    //PUSH AF
                case 246: OR(RAM[PC++]); return 7;                              //OR n
                case 247: return RST(48);                                       //RST 30
                case 248: return RET(fS);                                       //RET M
                case 249: SP = (ushort)(HL); return 6;                          //LD SP, HL
                case 250: return JP(fS);                                        //JP M, nn
                case 251: IM = 1; return 4;                                     //EI
                case 252: return CALL(fS);                                      //CALL M,nn
                case 253: return IndexOperation(ref IY);                        //-------------------- Префикс FD
                case 254: CP(RAM[PC++]); return 7;                              //CP n
                case 255: return RST(56);                                       //RST 38
            }
            PC = pc;
            IM = 0; //Для тестов, потом убрать
            return 1;
        }

        //Операции с индексами IX (DD/221), IY (FE/253)
        static int IndexOperation(ref ushort II)
        {
            R++;
            if (R > 127) R = 0;
            switch (RAM[PC++])
            {
                case 9: II = ADD(II, BC); return 15;                            //ADD II,BC
                case 25: II = ADD(II, DE); return 15;                           //ADD II,DE
                case 33:                                                        //LD IY,nn
                    II = (ushort)(RAM[PC++] + RAM[PC++] * 256);
                    return 14;
                case 34: POKE(II); return 20;                                   //LD (nn),II
                case 35: II++; return 10;                                       //INC II
                case 42: II = RAM[RAM[PC++] + RAM[PC++] * 256]; return 20;      //LD II,(nn)
                case 52: INC(ref RAM[IplusS(II)]); return 23;                   //INC (II+S)
                case 53: DEC(ref RAM[IplusS(II)]); return 23;                   //DEC (II+S)
                case 54: RAM[IplusS(II)] = RAM[PC++]; return 19;                //LD (II+S),N
                case 70: B = RAM[IplusS(II)]; return 19;                        //LD B,(II+S)
                case 78: C = RAM[IplusS(II)]; return 19;                        //LD C,(II+S)
                case 86: D = RAM[IplusS(II)]; return 19;                        //LD D,(II+S)
                case 94: E = RAM[IplusS(II)]; return 19;                        //LD E,(II+S)
                case 102: H = RAM[IplusS(II)]; return 19;                       //LD H,(II+S)
                case 110: L = RAM[IplusS(II)]; return 19;                       //LD L,(II+S)
                case 112: RAM[IplusS(II)] = B; return 19;                       //LD (II+S),B
                case 113: RAM[IplusS(II)] = C; return 19;                       //LD (II+S),C
                case 114: RAM[IplusS(II)] = D; return 19;                       //LD (II+S),D
                case 115: RAM[IplusS(II)] = E; return 19;                       //LD (II+S),E
                case 116: RAM[IplusS(II)] = H; return 19;                       //LD (II+S),H
                case 117: RAM[IplusS(II)] = L; return 19;                       //LD (II+S),L
                case 119: RAM[IplusS(II)] = A; return 19;                       //LD (II+S),A
                case 126: A = RAM[IplusS(II)]; return 19;                       //LD A,(II+S)
                case 132: ADD((byte)(II / 256)); return 8;                      //ADD IIH
                case 133: ADD((byte)II); return 8;                              //ADD IIL
                case 134: ADD(RAM[IplusS(II)]); return 19;                      //ADD A,(II+S)
                case 140: ADC((byte)(II / 256)); return 8;                      //ADC IIH
                case 141: ADC((byte)II); return 8;                              //ADC IIL
                case 142: ADC(RAM[IplusS(II)]); return 19;                      //ADC A,(II+S)
                case 148: SUB((byte)(II / 256)); return 8;                      //SUB IIH
                case 149: SUB((byte)II); return 8;                              //SUB IIL
                case 150: SUB(RAM[IplusS(II)]); return 19;                      //SUB (II+S)
                case 156: SBC((byte)(II / 256)); return 8;                      //SBC IIH
                case 157: SBC((byte)II); return 8;                              //SBC IIL
                case 158: SBC(RAM[IplusS(II)]); return 19;                      //SBC (II+S)
                case 164: AND((byte)(II / 256)); return 8;                      //AND IIH
                case 165: AND((byte)II); return 8;                              //AND IIL
                case 166: AND(RAM[IplusS(II)]); return 19;                      //AND (II+S)
                case 172: XOR((byte)(II / 256)); return 8;                      //XOR IIH
                case 173: XOR((byte)II); return 8;                              //XOR IIL
                case 174: XOR(RAM[IplusS(II)]); return 19;                      //XOR (II+S)
                case 180: OR((byte)(II / 256)); return 8;                       //OR IIH
                case 181: OR((byte)II); return 8;                               //OR IIL
                case 182: OR(RAM[IplusS(II)]); return 19;                       //OR (II+S)
                case 188: CP((byte)(II/256)); return 8;                         //CP IIH
                case 189: CP((byte)II); return 8;                               //CP IIL
                case 190: CP(RAM[IplusS(II)]); return 19;                       //CP (II+S)
                case 203:
                    PC++;
                    switch (RAM[PC])
                    {
                        /*   Не проверено, игде-то явно ошибка
                        case 38: SRA(ref RAM[IplusS4(II)]; return 23;           //SRA (II+S)
                        case 46: SRA(ref RAM[IplusS4(II)]; return 23;           //SRA (II+S)
                        case 62: SRL(ref RAM[IplusS4(II)]; return 23;           //SRL (II+S)*/
                        case 70: BIT(RAM[IplusS4(II)], 1); return 23;           //BIT 0,(II+S)
                        case 78: BIT(RAM[IplusS4(II)], 2); return 23;           //BIT 1,(II+S)
                        case 86: BIT(RAM[IplusS4(II)], 4); return 23;           //BIT 2,(II+S)
                        case 94: BIT(RAM[IplusS4(II)], 8); return 23;           //BIT 3,(II+S)
                        case 102: BIT(RAM[IplusS4(II)], 16); return 23;         //BIT 4,(II+S)
                        case 110: BIT(RAM[IplusS4(II)], 32); return 23;         //BIT 5,(II+S)
                        case 118: BIT(RAM[IplusS4(II)], 64); return 23;         //BIT 6,(II+S)
                        case 126: BIT(RAM[IplusS4(II)], 128); return 23;        //BIT 7,(II+S)
                        case 134: RES(ref RAM[IplusS4(II)], 0); return 23;      //RES 0,(II+S)
                        case 142: RES(ref RAM[IplusS4(II)], 1); return 23;      //RES 1,(II+S)
                        case 150: RES(ref RAM[IplusS4(II)], 2); return 23;      //RES 2,(II+S)
                        case 158: RES(ref RAM[IplusS4(II)], 3); return 23;      //RES 3,(II+S)
                        case 166: RES(ref RAM[IplusS4(II)], 4); return 23;      //RES 4,(II+S)
                        case 174: RES(ref RAM[IplusS4(II)], 5); return 23;      //RES 5,(II+S)
                        case 182: RES(ref RAM[IplusS4(II)], 6); return 23;      //RES 6,(II+S)
                        case 190: RES(ref RAM[IplusS4(II)], 7); return 23;      //RES 7,(II+S)
                        case 198: SET(ref RAM[IplusS4(II)], 0); return 23;      //SET 0,(II+S)
                        case 206: SET(ref RAM[IplusS4(II)], 1); return 23;      //SET 1,(II+S)
                        case 214: SET(ref RAM[IplusS4(II)], 2); return 23;      //SET 2,(II+S)
                        case 222: SET(ref RAM[IplusS4(II)], 3); return 23;      //SET 3,(II+S)
                        case 230: SET(ref RAM[IplusS4(II)], 4); return 23;      //SET 4,(II+S)
                        case 238: SET(ref RAM[IplusS4(II)], 5); return 23;      //SET 5,(II+S)
                        case 246: SET(ref RAM[IplusS4(II)], 6); return 23;      //SET 6,(II+S)
                        case 254: SET(ref RAM[IplusS4(II)], 7); return 23;      //SET 7,(II+S)
                    }
                    PC--;
                    break;
                case 225: return POP(ref II);                                   //POP II


                case 229: return PUSH(II);                                      //PUSH II
                case 233: PC = II; return 8;                                    //JP (II)
            }
            PC -= 2;
            return 1;
        }

        #region IO
        static void POKE(ushort Reg)        //Без указания регистра адрес берётся из команды, например для LD A,(NN)
        {
            ushort adr = (ushort)(RAM[PC++] + RAM[PC++] * 256);
            if (adr < 16384) return;
            RAM[adr++] = (byte)(Reg % 256);
            RAM[adr] = (byte)(Reg / 256);
        }

        static void POKE(ushort adr, byte b)
        {
            if (adr < 16384) return;
            RAM[adr++] = (byte)(b % 256);
            RAM[adr] = (byte)(b / 256);
        }

        static ushort PEEK()
        {
            ushort adr = (ushort)(RAM[PC++] + RAM[PC++] * 256);
            return (ushort)(RAM[adr] + RAM[adr + 1] * 256);
        }


        static int LDI(bool Rep, bool Down)
        {
            RAM[DE] = RAM[HL];
            t = RAM[DE];
            C--; if (C == 255) B--;
            if (!Down) { E++; if (E == 0) D++; L++; if (L == 0) H++; }
            else { E--; if (E == 255) D--; L--; if (L == 255) H--; }
            if (Rep & (B != 0 | C != 0)) { PC -= 2; return 21; }
            else
            {
                f5 = (t & m5) != 0;
                fH = false;
                f3 = (t & m3) != 0;
                fV = false;
                fN = false;
                return 16;
            }
        }
        #endregion
        #region Сложение и вычитание (протестировано)
        //INC
        static void INC(ref byte Reg)
        {
            Reg++;
            fS = Reg > 127;
            fZ = Reg == 0;
            f5 = (Reg & 32) != 0;
            fH = (Reg & 15) == 0;
            f3 = (Reg & 8) != 0;
            fV = Reg == 128;
            fN = false;
        }
        static void INC(ref byte r1, ref byte r2)
        {
            r2++;
            if (r2 == 0) r1++;
        }
        //DEC
        static void DEC(ref byte Reg)
        {
            Reg--;
            fS = Reg > 127;
            fZ = Reg == 0;
            f5 = (Reg & 32) != 0;
            fH = (Reg & 15) == 15;
            f3 = (Reg & 8) != 0;
            fV = Reg == 127;
            fN = true;
        }
        static void DEC(ref byte r1, ref byte r2)
        {
            r2--;
            if (r2 == 255) r1--;
        }

        //ADD 8
        static void ADD(byte b)
        {
            int ans = (A + b) & mFF;
            fS = (ans & mS) != 0;           
            fZ = ans == 0;
            f5 = (ans & m5) != 0;
            fH = (((A & mF) + (b & mF)) & mH) != 0;
            f3 = (ans & m3) != 0;
            fV = ((A ^ ((mFF - b) & mFFFF)) & (A ^ ans) & 128) != 0;
            fN = false;
            fC = ((A + b) & 256) != 0;
            A = (byte)ans;
        }

        //ADD 16
        static ushort ADD(ushort a, ushort b)
        {
            int ans = (a + b) & mFFFF;
            fZ = ans == 0;
            f5 = (ans & (m5 * 256)) != 0;
            fH = (((a & mFFF) + (b & mFFF)) & mHH) != 0;
            f3 = (ans & (m3 * 256)) != 0;
            fN = false;
            fC = ((a + b) & 65536) != 0;
            return (ushort)ans;
        }

        //ADC 8
        static void ADC(byte b)
        {
            int c = fC ? 1 : 0;
            int ans = (A + b + c) & mFF;
            fS = (ans & mS) != 0;
            fZ = ans == 0;
            f5 = (ans & m5) != 0;
            fH = (((A & mF) + (b & mF) + c) & mH) != 0;
            f3 = (ans & m3) != 0;
            fV = ((A ^ ((mFF - b) & mFFFF)) & (A ^ ans) & 128) != 0;
            fN = false;
            fC = ((A + b + c) & 256) != 0;
            A = (byte)ans;
        }

        //ADC 16
        static void ADC(ushort b)
        {
            int c = fC ? 1 : 0;
            int ans = (HL + b + c) & mFFFF;
            fS = (ans & (mS * 256)) != 0;
            fZ = ans == 0;
            f5 = (ans & (m5 * 256)) != 0;
            fH = (((HL & mFFF) + (b & mFFF)) & mHH) != 0;
            f3 = (ans & (m3 * 256)) != 0;
            fV = ((HL ^ ((mFF - b) & mFFFF)) & (HL ^ ans) & 32768) != 0;
            fN = false;
            fC = ((HL + b + c) & 65536) != 0;
            HL = (ushort)ans;
        }

        //SUB
        static void SUB(byte b)
        {
            int ans = (A - b) & mFF;
            fS = (ans & mS) != 0;
            fZ = ans == 0;
            f5 = (ans & m5) != 0;
            fH = (((A & mF) - (b & mF)) & mH) != 0;
            f3 = (ans & m3) != 0;
            fV = ((A ^ b) & (A ^ ans) & 128) != 0;
            fN = true;
            fC = ((A - b) & 256) != 0;
            A = (byte)ans;
        }

        //SBC 8
        static void SBC(byte b)
        {
            int c = fC ? 1 : 0;
            int ans = (A - b - c) & mFF;
            fS = (ans & mS) != 0;
            fZ = ans == 0;
            f5 = (ans & m5) != 0;
            fH = (((A & mF) - (b & mF) - c) & mH) != 0;
            f3 = (ans & m3) != 0;
            fV = ((A ^ b) & (A ^ ans) & 128) != 0;
            fN = true;
            fC = ((A - b - c) & 256) != 0;
            A = (byte)ans;
        }

        //SBC 16
        static void SBC(ushort b)
        {
            int c = fC ? 1 : 0;
            int ans = (HL - b - c) & mFFFF;
            fS = (ans & (mS * 256)) != 0;
            fZ = ans == 0;
            f5 = (ans & (m5 * 256)) != 0;
            fH = (((HL & mFFF) - (b & mFFF) - c) & 4096) != 0;
            f3 = (ans & (m3 * 256)) != 0;
            fV = ((HL ^ b) & (HL ^ ans) & 32768) != 0;
            fN = true;
            fC = ((HL - b - c) & 65536) != 0;
            HL = (ushort)ans;
        }

        #endregion
        #region Вращение/сдвиг битов (протестировано)
        static void RL(ref byte b)
        {
            int c = fC ? 1 : 0;         //Сначала делаем сдвиг
            fC = (b & 128) != 0;        //Потом проверяем знак переноса
            b = (byte)((b * 2) | c);
            fS = (b & mS) != 0;
            fZ = b == 0;
            f5 = (b & m5) != 0;
            fH = false;
            f3 = (b & m3) != 0;
            fV = Parity(b);
            fN = false;
        }
        static void RLA()
        {
            int c = fC ? 1 : 0;         //Сначала делаем сдвиг
            fC = (A & 128) != 0;        //Потом проверяем знак переноса
            A = (byte)((A * 2) | c);
            f5 = (A & m5) != 0;
            fH = false;
            f3 = (A & m3) != 0;
            fN = false;
        }

        static void RLC(ref byte b)
        {
            fC = (b & 128) != 0;        //Сначала проверяем флаг переноса
            int c = fC ? 1 : 0;         //Потом делаем сдвиг
            b = (byte)((b * 2) | c);
            fS = (b & mS) != 0;
            fZ = b == 0;
            f5 = (b & m5) != 0;
            fH = false;
            f3 = (b & m3) != 0;
            fV = Parity(b);
            fN = false;
        }

        static void RLCA()
        {
            fC = (A & 128) != 0;        //Сначала проверяем флаг переноса
            int c = fC ? 1 : 0;         //Потом делаем сдвиг
            A = (byte)((A * 2) | c);
            f5 = (A & m5) != 0;
            fH = false;
            f3 = (A & m3) != 0;
            fN = false;
        }

        static void RR(ref byte b)
        {
            int c = fC ? 1 : 0;         //Сначала делаем сдвиг
            fC = (b & 1) != 0;          //Потом проверяем знак переноса
            b = (byte)((b / 2) | (c * 128));
            fS = (b & mS) != 0;
            fZ = b == 0;
            f5 = (b & m5) != 0;
            fH = false;
            f3 = (b & m3) != 0;
            fV = Parity(b);
            fN = false;
        }
        static void RRA()
        {
            int c = fC ? 1 : 0;         //Сначала делаем сдвиг
            fC = (A & 1) != 0;          //Потом проверяем знак переноса
            A = (byte)((A / 2) | (c * 128));
            f5 = (A & m5) != 0;
            fH = false;
            f3 = (A & m3) != 0;
            fN = false;
        }

        static void RRC(ref byte b)
        {
            fC = (b & 1) != 0;          //Сначала проверяем флаг переноса
            int c = fC ? 1 : 0;         //Потом делаем сдвиг
            b = (byte)((b / 2) | (c * 128));
            fS = (b & mS) != 0;
            fZ = b == 0;
            f5 = (b & m5) != 0;
            fH = false;
            f3 = (b & m3) != 0;
            fV = Parity(b);
            fN = false;
        }

        static void RRCA()
        {
            fC = (A & 1) != 0;          //Сначала проверяем флаг переноса
            int c = fC ? 1 : 0;         //Потом делаем сдвиг
            A = (byte)((A / 2) | (c * 128));
            f5 = (A & m5) != 0;
            fH = false;
            f3 = (A & m3) != 0;
            fN = false;
        }

        static void SLA(ref byte b)
        {
            fC = (b & 128) != 0;
            b = (byte)(b * 2);
            fS = (b & mS) != 0;
            fZ = b == 0;
            f5 = (b & m5) != 0;
            fH = false;
            f3 = (b & m3) != 0;
            fV = Parity(b);
            fN = false;
        }

        static void SLS(ref byte b)
        {
            //В книге не найдена...
        }

        static void SRA(ref byte b)
        {
            fC = (b & 1) != 0;
            b = (byte)((b / 2) | (b & 128));
            fS = (b & mS) != 0;
            fZ = b == 0;
            f5 = (b & m5) != 0;
            fH = false;
            f3 = (b & m3) != 0;
            fV = Parity(b);
            fN = false;
        }

        static void SRL(ref byte b)
        {
            fC = (b & 1) != 0;
            b = (byte)(b / 2);
            fS = (b & mS) != 0;
            fZ = b == 0;
            f5 = (b & m5) != 0;
            fH = false;
            f3 = (b & m3) != 0;
            fV = Parity(b);
            fN = false;
        }

        #endregion
        #region Ротация полубайтов

        static void RRD()
        {

        }
        static void RLD()
        {
            byte t = RAM[HL];
            byte q = t;
            t = (byte)(((t * 16) | (A & mF)) & mFF);
            A = (byte)((A & 240) | (q / 16));
            POKE(HL, t);
            fS = (A & mS) != 0;
            fZ = A == 0;
            f5 = (A & m5) != 0;
            fH = false;
            f3 = (A & m3) != 0;
            fV = intIFF2;
            fN = false;
            //Протестировано... почти
        }

        #endregion
        #region Логика (протестировано)
        //AND
        static void AND(byte Reg)
        {
            A &= Reg;
            fS = (A & mS) != 0;
            fZ = A == 0;
            f5 = (A & m5) != 0;
            fH = true;
            f3 = (A & m3) != 0;
            fV = Parity(A);
            fN = false;
            fC = false;
        }
        //OR
        static void OR(byte Reg)
        {
            A |= Reg;
            fS = (A & mS) != 0;
            fZ = A == 0;
            f5 = (A & m5) != 0;
            fH = false;
            f3 = (A & m3) != 0;
            fV = Parity(A);
            fN = false;
            fC = false;
        }
        //XOR
        static void XOR(byte Reg)
        {
            A ^= Reg;
            fS = (A & mS) != 0;
            fZ = A == 0;
            f5 = (A & m5) != 0;
            fH = false;
            f3 = (A & m3) != 0;
            fV = Parity(A);
            fN = false;
            fC = false;
        }
        //CP
        static void CP(byte Reg)
        {
            ushort a = (ushort)(A - Reg);
            fS = (a & 128) != 0;
            fZ = a == 0;
            f5 = (Reg & 32) != 0;
            fH = (((A & 15) - (Reg & 15)) & 16) != 0;
            f3 = (Reg & 8) != 0;
            fV = ((A ^ Reg) & (A ^ a) & 128) != 0; //проверить
            fN = true;
            fC = (a & 256) != 0;
        }
        #endregion
        #region Побитовые операции (протестировано)
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
        static void BIT(byte b, byte bit)
        {
            bool ans = (b & bit) != 0;
            fS = ans & (bit == mS);
            fZ = !ans;
            f5 = (b & m5) != 0;
            fH = true;
            f3 = (b & m3) != 0;
            fV = fZ;
            fN = false;
        }
        #endregion
        #region Переходы
        static int JR(bool If)
        {
            if (If)
            {
                byte TO = RAM[PC];
                if (TO < 128) PC = (ushort)(PC + TO + 1);
                else PC = (ushort)(PC + TO - 255);
                return 12;
            }
            else { PC++; return 7; }
        }
        static int JP(bool If)
        {
            if (If) PC = (ushort)(RAM[PC] + RAM[PC + 1] * 256);
            else PC += 2;
            return 10;
        }
        static int CALL(bool If)
        {
            if (If)
            {
                RAM[--SP] = (byte)((PC + 2) / 256);
                RAM[--SP] = (byte)((PC + 2) % 256);
                PC = (ushort)(RAM[PC] + RAM[PC + 1] * 256);
                return 17;
            }
            else { PC += 2; return 10; }
        }
        public static int RET(bool If)
        {
            if (If) { PC = (ushort)(RAM[SP++] + RAM[SP++] * 256); return 11; }
            else return 5;
        }
        static int RST(ushort Adr)
        {
            RAM[--SP] = (byte)((PC) / 256);
            RAM[--SP] = (byte)((PC) % 256);
            PC = Adr;
            return 11;
        }
        public static int Interrupt()
        {
            if (RAM[PC] == 118) PC++;
            if (IM == 1) return RST(56);
            else return 0;
        }
        #endregion
        #region Стэк
        static int PUSH(byte r1, byte r2) { RAM[--SP] = r1; RAM[--SP] = r2; return 11; }
        static int PUSH(ushort Reg) { RAM[--SP] = (byte)(Reg / 256); RAM[--SP] = (byte)(Reg % 256); return 15; }
        static int POP(ref byte r1, ref byte r2) { r2 = RAM[SP++]; r1 = RAM[SP++]; return 10; }
        static int POP(ref ushort Reg) { Reg = (ushort)(RAM[SP++] + RAM[SP++] * 256); return 14; }
        //static 
        #endregion
        #region Разное
        static void DAA()
        {
            int incr = 0;
            if (fH | ((A & mF) > 9)) incr |= 6;
            if (fC | (A > 159)) incr |= 96;
            if ((A > 143) & ((A & mF) > 9)) incr |= 96;
            if (A > 153) fC = true;
            if (fN)
            {
                fH = (((A & 15) - (incr & 15)) & mH) != 0;
                A = (byte)((A - incr) & mFF);
            }
            else
            {
                fH = (((A & 15) + (incr & 15)) & mH) != 0;
                A = (byte)((A + incr) & mFF);
            }
            fS = (A & mS) != 0;
            fZ = A == 0;
            f5 = (A & m5) != 0;
            f3 = (A & m3) != 0;
            fV = Parity(A);
            //Протестировано, но не полностью
        }

        static void CPL()
        {
            A ^= 255;
            f5 = (A & 32) != 0;
            fH = true;
            f3 = (A & 8) != 0;
            fN = true;
        }
        static void NEG()
        {
            byte t = A;
            A = 0;
            SUB(t);
            //Протестировано
        }
        static void EX(ref byte r1, ref byte r2, ref byte ra1, ref byte ra2)
        {
            byte t = r1; r1 = ra1; ra1 = t;
            t = r2; r2 = ra2; ra2 = t;
        }
        static void EXAF()
        {
            byte t = A; A = Aa; Aa = t;
            t = F; F = Fa; Fa = t;
        }
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
            ushort tmp = (ushort)(IReg + t);
            if (t > 127) tmp -= 256;
            return tmp;
        }
        static ushort IplusS4(ushort IReg) //Тоже самое, но для тупых 4-х байтных команд где сначала S, потом код команды
        {
            byte t = RAM[PC++ - 1];
            ushort tmp = (ushort)(IReg + t);
            if (t > 127) tmp -= 256;
            return tmp;
        }
        //Высчитывание чётности
        static bool Parity(byte b)
        {
            int p = 0;
            if ((b & 1) != 0) p++;
            if ((b & 2) != 0) p++;
            if ((b & 4) != 0) p++;
            if ((b & 8) != 0) p++;
            if ((b & 16) != 0) p++;
            if ((b & 32) != 0) p++;
            if ((b & 64) != 0) p++;
            if ((b & 128) != 0) p++;
            return p % 2 == 0;
        }
        static void SCF()
        {
            f5 = (A & m5) != 0;
            fH = false;
            f3 = (A & m5) != 0;
            fN = false;
            fC = true;
        }
        static void CCF()
        {
            f5 = (A & m5) != 0;
            fH = fC;
            f3 = (A & m3) != 0;
            fN = false;
            fC ^= true;
        }
        public static void OUT(ushort port, byte Byte)
        {
            if (port % 256 == 254) Screen.Border = Byte & 7;
        }
        #endregion
    }
}