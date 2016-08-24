using System;

namespace Spectrum
{
    static class Z80
    {
        public static byte[] RAM = new byte[65536];
        public static bool[] Be = new bool[65536];
        public static byte A, B, C, D, E, H, L, I, R;
        public static byte Aa, Fa, Ba, Ca, Da, Ea, Ha, La;
        public static UInt16 PC, SP, IX, IY;
        public static bool fS, fZ, f5, fH, f3, fV, fN, fC;
        public static byte IM;
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
                case 2: RAM[B * 256 + C] = A; return 7;                         //LD (BC),A
                case 3: INC(ref B, ref C); return 6;                            //INC BC
                case 4: INC(ref B); return 4;                                   //INC B
                case 5: DEC(ref B); return 4;                                   //DEC B
                case 6: B = RAM[PC++]; return 7;                                //LD B,n
                case 7: RL(ref A, false); return 4;                             //RLCA
                case 8: EXAF(); return 4;                                       //EX AF,AF'
                case 9: ADDHL(B, C, false); return 11;                          //ADD HL,BC
                case 10: A = RAM[B * 256 + C]; return 7;                        //LD A,(BC);
                case 11: DEC(ref B, ref C); return 6;                           //DEC BC
                case 12: INC(ref C); return 4;                                  //INC C
                case 13: DEC(ref C); return 4;                                  //DEC C
                case 14: C = RAM[PC++]; return 7;                               //LD C,n
                case 15: RR(ref A, false); return 4;                            //RRCA
                case 16: B--; return JR(B != 0) + 1;                            //DJNZ n
                case 17: E = RAM[PC++]; D = RAM[PC++]; return 10;               //LD DE,nn
                case 18: RAM[D * 256 + E] = A; return 7;                        //LD (DE),A
                case 19: INC(ref D, ref E); return 6;                           //INC DE
                case 20: INC(ref D); return 4;                                  //INC D
                case 21: DEC(ref D); return 4;                                  //DEC D
                case 22: D = RAM[PC++]; return 7;                               //LD D,n
                case 23: RL(ref A, true); return 4;                             //RLA
                case 24: JR(true); return 4;                                    //JR n
                case 25: ADDHL(D, E, false); return 11;                         //ADD HL,DE
                case 26: A = RAM[D * 256 + E]; return 7;                        //LD A,(DE)
                case 27: DEC(ref D, ref E); return 6;                           //DEC DE
                case 28: INC(ref E); return 4;                                  //INC E
                case 29: DEC(ref E); return 4;                                  //DEC E
                case 30: E = RAM[PC++]; return 7;                               //LD E,n
                case 31: RR(ref A, true); return 4;                             //RRA
                case 32: return JR(!fZ);                                        //JR NZ,s
                case 33: L = RAM[PC++]; H = RAM[PC++]; return 10;               //LD HL,nn
                case 34: return POKE(H, L);                                     //LD (nn),HL
                case 35: INC(ref H, ref L); return 6;                           //INC HL
                case 36: INC(ref H); return 4;                                  //INC H
                case 37: DEC(ref H); return 4;                                  //DEC H
                case 38: H = RAM[PC++]; return 7;                               //LD H,n
                case 40: return JR(fZ);                                         //JR Z,n
                case 41: ADDHL(H, L, false); return 11;                         //ADD HL,HL
                case 42: PEEK(ref H, ref L); return 16;                         //LD HL,(nn)
                case 43: DEC(ref H, ref L); return 6;                           //DEC HL
                case 44: INC(ref L); return 4;                                  //INC L
                case 45: DEC(ref L); return 4;                                  //DEC L
                case 46: L = RAM[PC++]; return 7;                               //LD L,n
                case 47: A ^= 255; return 4;                                    //CPL
                case 48: return JR(!fC);                                        //JR NC,n
                case 49: SP = (ushort)(RAM[PC++] + RAM[PC++] * 256); return 10; //LD SP,nn
                case 50: RAM[RAM[PC++] + RAM[PC++] * 256] = A; return 13;       //LD (nn),A
                case 51: SP++; return 6;                                        //INC SP                //Не проверено
                case 52: INC(ref RAM[H * 256 + L]); return 11;                  //INC (HL)
                case 53: DEC(ref RAM[H * 256 + L]); return 11;                  //DEC (HL)
                case 54: RAM[H * 256 + L] = RAM[PC++]; return 10;               //LD (HL),n
                case 55: SCF(); return 4;                                       //SCF
                case 56: return JR(fC);                                         //JR C,n
                case 57: ADDHL((byte)(SP / 256), (byte)(SP % 256), false); return 11;   //ADD HL,SP
                case 58: A = RAM[RAM[PC++] + RAM[PC++] * 256]; return 13;       //LD A,(nn)
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
                case 70: B = RAM[H * 256 + L]; return 7;                        //LD B,(HL)
                case 71: B = A; return 4;                                       //LD B,A
                case 72: C = B; return 4;                                       //LD C,B
                case 73: return 4;                                              //LD C,C
                case 75: C = E; return 4;                                       //LD C,E
                case 76: C = H; return 4;                                       //LD C,H
                case 77: C = L; return 4;                                       //LD C,L
                case 78: C = RAM[H * 256 + L]; return 7;                        //LD C,(HL)
                case 79: C = A; return 4;                                       //LD C,A
                case 81: D = C; return 4;                                       //LD D,C
                case 82: return 4;                                              //LD D,D
                case 83: D = E; return 4;                                       //LD D,E
                case 84: D = H; return 4;                                       //LD D,H
                case 85: D = L; return 4;                                       //LD D,H
                case 86: D = RAM[H * 256 + L]; return 7;                        //LD D,(HL)
                case 87: D = A; return 4;                                       //LD D,A
                case 88: E = B; return 4;                                       //LD E,B
                case 90: E = D; return 4;                                       //LD E,D
                case 92: E = H; return 4;                                       //LD E,H
                case 93: E = L; return 4;                                       //LD E,L
                case 94: E = RAM[H * 256 + L]; return 7;                        //LD E,(HL)
                case 95: E = A; return 4;                                       //LD E,A
                case 96: H = B; return 4;                                       //LD H,B
                case 97: H = C; return 4;                                       //LD H,C
                case 98: H = D; return 4;                                       //LD H,D
                case 99: H = E; return 4;                                       //LD H,E
                case 101: H = L; return 4;                                      //LD H,L
                case 102: H = RAM[H * 256 + L]; return 7;                       //LD H,(HL)
                case 103: H = A; return 4;                                      //LD H,A
                case 104: L = B; return 4;                                      //LD L,B
                case 105: L = C; return 4;                                      //LD L,C
                case 106: L = D; return 4;                                      //LD L,D
                case 107: L = E; return 4;                                      //LD L,E
                case 110: L = RAM[H * 256 + L]; return 7;                       //LD L,(HL)
                case 111: L = A; return 4;                                      //LD L,A
                case 112: RAM[H * 256 + L] = B; return 7;                       //LD (HL),B
                case 113: RAM[H * 256 + L] = C; return 7;                       //LD (HL),C
                case 114: RAM[H * 256 + L] = D; return 7;                       //LD (HL),D
                case 115: RAM[H * 256 + L] = E; return 7;                       //LD (HL),E
                case 118: fC = true; PC--; return 4;                            //HALT
                case 119: RAM[H * 256 + L] = A; return 7;                       //LD (HL),A
                case 120: A = B; return 4;                                      //LD A,B
                case 121: A = C; return 4;                                      //LD A,C
                case 122: A = D; return 4;                                      //LD A,D
                case 123: A = E; return 4;                                      //LD A,E
                case 124: A = H; return 4;                                      //LD A,H
                case 125: A = L; return 4;                                      //LD A,L
                case 126: A = RAM[H * 256 + L]; return 7;                       //LD A,(HL)
                case 128: ADD(B, false); return 4;                              //ADD A,B
                case 129: ADD(C, false); return 4;                              //ADD A,C
                case 130: ADD(D, false); return 4;                              //ADD A,D
                case 131: ADD(E, false); return 4;                              //ADD A,E
                case 132: ADD(H, false); return 4;                              //ADD A,H
                case 133: ADD(L, false); return 4;                              //ADD A,L
                case 134: ADD(RAM[H * 256 + L], true); return 7;                //ADD A,(HL)
                case 135: ADD(A, false); return 4;                              //ADD A,A
                case 136: ADD(B, true); return 4;                               //ADC A,B
                case 137: ADD(C, true); return 4;                               //ADC A,C
                case 142: ADD(RAM[H * 256 + L], true); return 7;                //ADC A,(HL)
                case 143: ADD(A, true); return 4;                               //ADC A,A
                case 144: SUB(B, false); return 4;                              //SUB B
                case 145: SUB(C, false); return 4;                              //SUB C
                case 146: SUB(D, false); return 4;                              //SUB D
                case 148: SUB(H, false); return 4;                              //SUB H
                case 149: SUB(L, false); return 4;                              //SUB L
                case 150: SUB(RAM[H * 256 + L], true); return 7;                //SUB (HL)
                case 152: SUB(B, true); return 4;                               //SBC A,B
                case 155: SUB(E, true); return 4;                               //SBC A,E
                case 159: SUB(A, true); return 4;                               //SBC A,A
                case 160: AND(B); return 4;                                     //AND B
                case 161: AND(C); return 4;                                     //AND C
                case 162: AND(D); return 4;                                     //AND D
                case 166: AND(RAM[H * 256 + L]); return 7;                      //AND (HL)
                case 167: AND(A); return 4;                                     //AND A
                case 168: XOR(B); return 4;                                     //XOR B
                case 169: XOR(C); return 4;                                     //XOR C
                case 171: XOR(E); return 4;                                     //XOR E
                case 174: XOR(RAM[H * 256 + L]); return 7;                      //XOR (HL)
                case 175: XOR(A); return 4;                                     //XOR A
                case 176: OR(B); return 4;                                      //OR B
                case 177: OR(C); return 4;                                      //OR C
                case 179: OR(E); return 4;                                      //OR E
                case 180: OR(H); return 4;                                      //OR H
                case 181: OR(L); return 4;                                      //OR L
                case 182: OR(RAM[H * 256 + L]); return 4;                       //OR (HL)
                case 183: OR(A); return 4;                                      //OR A
                case 184: CP(B); return 4;                                      //CP B
                case 185: CP(C); return 4;                                      //CP C
                case 186: CP(D); return 4;                                      //CP D
                case 188: CP(H); return 4;                                      //CP H
                case 189: CP(L); return 4;                                      //CP L
                case 190: CP(RAM[H * 256 + L]); return 7;                       //CP (HL)
                case 191: CP(A); return 4;                                      //CP A
                case 192: return RET(!fZ);                                      //RET NZ
                case 193: return POP(ref B, ref C);                             //POP BC
                case 194: return JP(!fZ);                                       //JP NZ, nn
                case 195: return JP(true);                                      //JP nn
                case 196: return CALL(!fZ);                                     //CALL NZ, nn
                case 197: return PUSH(B, C);                                    //PUSH BC
                case 198: ADD(RAM[PC++], false); return 7;                      //ADD A, n
                case 200: return RET(fZ);                                       //RET Z
                case 201: RET(true); return 10;                                 //RET
                case 202: return JP(fZ);                                        //JP Z,nn
                #region case 203 (Префикс CD)
                case 203:                                                       //-------------------- Префикс CD
                    R++;
                    if (R > 127) R = 0;
                    switch (RAM[PC++])
                    {
                        case 0: RL(ref B, false); return 8;                     //RLC B
                        case 7: RL(ref B, false); return 8;                     //RLC A
                        case 9: RR(ref C, false); return 8;                     //RRC C
                        case 16: RL(ref B, true); return 8;                     //RL C
                        case 17: RL(ref C, true); return 8;                     //RL C
                        case 19: RL(ref E, true); return 8;                     //RL E
                        case 22: RL(ref RAM[H * 256 + L], true); return 15;     //RL (HL)
                        case 24: RR(ref B, true); return 8;                     //RR B
                        case 25: RR(ref C, true); return 8;                     //RR C
                        case 28: RR(ref H, true); return 8;                     //RR H
                        case 29: RR(ref L, true); return 8;                     //RR L
                        case 33: SLA(ref C); return 8;                          //SLA C
                        case 56: SRL(ref B); return 8;                          //SRL B
                        case 57: SRL(ref C); return 8;                          //SRL C
                        case 58: SRL(ref D); return 8;                          //SRL D
                        case 60: SRL(ref H); return 8;                          //SRL H
                        case 61: SRL(ref L); return 8;                          //SRL L
                        case 63: SRL(ref A); return 8;                          //SRL A
                        case 64: BIT(B, 0); return 8;                           //BIT 0,B
                        case 71: BIT(A, 0); return 8;                           //BIT 0,A
                        case 79: BIT(A, 1); return 8;                           //BIT 1,A
                        case 86: BIT(D, 2); return 8;                           //BIT 2,D
                        case 87: BIT(A, 2); return 8;                           //BIT 2,A
                        case 88: BIT(B, 3); return 8;                           //BIT 3,B
                        case 90: BIT(D, 3); return 8;                           //BIT 3,D
                        case 95: BIT(A, 3); return 8;                           //BIT 3,A
                        case 103: BIT(A, 4); return 8;                          //BIT 4,A
                        case 104: BIT(B, 5); return 8;                          //BIT 5,B
                        case 111: BIT(A, 5); return 8;                          //BIT 5,A
                        case 112: BIT(B, 6); return 8;                          //BIT 6,B
                        case 120: BIT(B, 7); return 8;                          //BIT 7,B
                        case 121: BIT(C, 7); return 8;                          //BIT 7,C
                        case 126: BIT(RAM[H * 256 + L], 7); return 12;          //BIT 7,(HL)
                        case 127: BIT(A, 7); return 8;                          //BIT 7,A
                        case 128: RES(ref B, 0); return 8;                      //RES 0,B
                        case 134: RES(ref RAM[H * 256 + L], 0); return 15;      //RES 0,(HL)
                        case 144: RES(ref B, 2); return 8;                      //RES 2,B
                        case 150: RES(ref RAM[H * 256 + L], 2); return 15;      //RES 2,(HL)
                        case 152: RES(ref B, 3); return 8;                      //RES 3,B
                        case 158: RES(ref RAM[H * 256 + L], 3); return 15;      //RES 3,(HL)
                        case 160: RES(ref B, 4); return 8;                      //RES 4,B
                        case 174: RES(ref RAM[H * 256 + L], 5); return 15;      //RES 5,(HL)
                        case 176: RES(ref B, 6); return 8;                      //RES 6,B
                        case 177: RES(ref C, 6); return 8;                      //RES 6,C
                        case 184: RES(ref B, 7); return 8;                      //RES 7,B
                        case 188: RES(ref H, 7); return 8;                      //RES 7,H
                        case 191: RES(ref A, 7); return 8;                      //RES 7,A
                        case 194: SET(ref D, 0); return 8;                      //SET 0,D
                        case 198: SET(ref RAM[H * 256 + L], 0); return 15;      //SET 0,(HL)
                        case 202: SET(ref D, 1); return 8;                      //SET 1,D
                        case 210: SET(ref D, 2); return 8;                      //SET 2,D
                        case 218: SET(ref D, 3); return 8;                      //SET 3,D
                        case 222: SET(ref RAM[H * 256 + L], 3); return 15;      //SET 3,(HL)
                        case 226: SET(ref D, 4); return 8;                      //SET 4,D
                        case 232: SET(ref B, 5); return 8;                      //SET 5,B
                        case 233: SET(ref C, 5); return 8;                      //SET 5,C
                        case 238: SET(ref RAM[H * 256 + L], 5); return 15;      //SET 5,(HL)
                        case 241: SET(ref C, 6); return 8;                      //SET 6,C
                        case 248: SET(ref B, 7); return 8;                      //SET 7,B
                        case 253: SET(ref L, 7); return 8;                      //SET 7,L
                        case 254: SET(ref RAM[H * 256 + L], 7); return 15;      //SET 7,(HL)
                        case 255: SET(ref A, 7); return 8;                      //SET 7,A
                    }
                    break;
                #endregion
                case 204: return CALL(fZ);                                      //CALL Z, nn
                case 205: return CALL(true);                                    //CALL nn
                case 206: ADD(RAM[PC++], true); return 7;                       //ADC A,n
                case 207: return RST(8);                                        //RST 8
                case 208: return RET(!fC);                                      //RET NC
                case 209: return POP(ref D, ref E);                             //POP DE
                case 210: return JP(!fC);                                       //JP NC,nn
                case 211: OUT(RAM[PC++], A); return 11;                         //OUT (n),A
                case 212: return CALL(!fC);                                     //CALL NC, nn
                case 213: return PUSH(D, E);                                    //PUSH DE
                case 214: SUB(RAM[PC++], false); return 7;                      //SUB n
                case 215: return RST(16);                                       //RST 10
                case 216: return RET(fC);                                       //RET C
                case 217: EXX(); return 4;                                      //EXX
                case 218: return JP(fC);                                        //JP C,nn
                case 219: A = IN[RAM[PC] + A * 256]; return 11;                 //IN A,(n)
                case 220: return CALL(fC);                                      //CALL C, nn
                case 223: return RST(24);                                       //RST 18
                case 225: return POP(ref H, ref L);                             //POP HL
                case 227: EX(ref RAM[SP + 1], ref RAM[SP], ref H, ref L); return 19;    //EX (SP),HL
                case 228: return CALL(!fV);                                     //CALL PO,nn
                case 229: return PUSH(H, L);                                    //PUSH HL
                case 230: AND(RAM[PC++]); return 7;                             //AND n
                case 231: return RST(32);                                       //RST 20
                case 233: PC = (ushort)(H * 256 + L); return 4;                 //JP (HL)
                case 235: EX(ref D, ref E, ref H, ref L); return 4;
                #region case 237 (Префикс ED)                                   
                case 237:                                                       //-------------------- Префикс ED
                    R++;
                    if (R > 127) R = 0;
                    switch (RAM[PC++])
                    {
                        case 66: SBC(B, C); return 15;                          //SBC HL,BC
                        case 67: return POKE(B, C);                             //LD (nn),BC
                        case 68:                                                //NEG
                            A = (byte)(256 - A);
                            fZ = A == 0;
                            //флаги... остальные
                            return 8;
                        case 71: I = A; return 9;                               //LD I,A
                        case 74: ADDHL(B, C, true); return 8;                   //ADC HL, BC
                        case 75: PEEK(ref B, ref C); return 20;                 //LD BC,(nn)
                        case 82: SBC(D, E); return 15;                          //SBC HL,DE
                        case 83: return POKE(D, E);                             //LD (nn),DE
                        case 86: IM = 1; return 8;                              //IM 1
                        case 90: ADDHL(D, E, true); return 8;                   //ADC HL,DE
                        case 91: PEEK(ref D, ref E); return 20;                 //LD DE,(nn)
                        case 98: SBC(H, L); return 15;                          //SBC HL,HL
                        case 106: ADDHL(H, L, true); return 15;                 //ADC HL,HL
                        case 114:                                               //SBC HL,SP
                            SBC((byte)(SP / 256), (byte)(SP % 256));
                            return 15;   
                        case 115: return POKE(SP);                              //LD (nn),SP
                        case 120: A = IN[B * 256 + C]; return 12;               //IN A,(C)
                        case 121: OUT((ushort)(B * 256 + C), A); return 12;     //OUT (C),A
                        case 122: ADDHL((byte)(SP / 256), (byte)(SP % 256), true); return 11;   //ADC HL, SP
                        case 123:                                               //LD SP,(nn)
                            tmp = (ushort)(RAM[PC++] + RAM[PC++] * 256);
                            SP = (ushort)(RAM[tmp++] + RAM[tmp] * 256);
                            return 20;
                        case 176: return LDI(true, false);                      //LDIR
                        case 184: return LDI(true, true);                       //LDDR
                    }
                    break;
                #endregion
                case 238: XOR(RAM[PC++]); return 7;                             //XOR N
                case 239: return RST(40);                                       //RST 28
                case 241: F = RAM[SP++]; A = RAM[SP++]; return 10;              //POP AF
                case 242: return JP(!fS);                                       //JP P,nn
                case 243: IM = 0; return 40;                                    //DI
                case 244: return CALL(!fS);                                     //CALL P,nn
                case 245: return PUSH(A, F);                                    //PUSH AF
                case 246: OR(RAM[PC++]); return 7;                              //OR n
                case 248: return RET(fS);                                       //RET M
                case 249: SP = (ushort)(H * 256 + L); return 6;                 //LD SP, HL
                case 250: return JP(fS);                                        //JP M, nn
                case 251: IM = 1; return 4;                                     //EI
                case 221: return IndexOperation(ref IX);                        //-------------------- Префикс DD
                case 253: return IndexOperation(ref IY);                        //-------------------- Префикс FD
                case 254: CP(RAM[PC++]); return 7;                              //CP n
                case 255: return RST(56);                                       //RST 38
            }
            PC = pc;
            IM = 0; //Для тестов, потом убрать
            return 1;
        }

        //Операции с индексами IX, IY (221, 253)
        static int IndexOperation(ref ushort II)
        {
            R++;
            if (R > 127) R = 0;
            switch (RAM[PC++])
            {
                case 9: ADD(ref II, B, C); return 15;                           //ADD II,BC
                case 25: ADD(ref II, D, E); return 15;                          //ADD II,DE
                case 33:                                                        //LD IY,nn
                    II = (ushort)(RAM[PC++] + RAM[PC++] * 256);
                    return 14;
                case 34: return POKE(II);                                       //LD (nn),II
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
                case 134: ADD(RAM[IplusS(II)], false); return 19;               //ADD A,(II+S)
                case 150: SUB(RAM[IplusS(II)], false); return 19;               //SUB (II+S)
                case 203:
                    PC++;
                    switch (RAM[PC])
                    {
                        case 70: BIT(RAM[IplusS4(II)], 0); return 23;           //BIT 0,(II+S)
                        case 78: BIT(RAM[IplusS4(II)], 1); return 23;           //BIT 1,(II+S)
                        case 86: BIT(RAM[IplusS4(II)], 2); return 23;           //BIT 2,(II+S)
                        case 94: BIT(RAM[IplusS4(II)], 3); return 23;           //BIT 3,(II+S)
                        case 102: BIT(RAM[IplusS4(II)], 4); return 23;          //BIT 4,(II+S)
                        case 110: BIT(RAM[IplusS4(II)], 5); return 23;          //BIT 5,(II+S)
                        case 118: BIT(RAM[IplusS4(II)], 6); return 23;          //BIT 6,(II+S)
                        case 126: BIT(RAM[IplusS4(II)], 7); return 23;          //BIT 7,(II+S)
                        case 134: RES(ref RAM[IplusS4(II)], 0); return 23;      //RES 0,(II+S)
                        case 142: RES(ref RAM[IplusS4(II)], 1); return 23;      //RES 1,(II+S)
                        case 150: RES(ref RAM[IplusS4(II)], 2); return 23;      //RES 2,(II+S)
                        case 158: RES(ref RAM[IplusS4(II)], 3); return 23;      //RES 3,(II+S)
                        case 166: RES(ref RAM[IplusS4(II)], 4); return 23;      //RES 4,(II+S)
                        case 174: RES(ref RAM[IplusS4(II)], 5); return 23;      //RES 5,(II+S)
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
                case 182: OR(RAM[IplusS4(II)]); return 19;                      //OR (II+S)
                case 190: CP(RAM[IplusS4(II)]); return 19;                      //CP (II+S)
                case 225: return POP(ref II);                                   //POP II


                case 229: return PUSH(II);                                      //PUSH II
                case 233: PC = II; return 8;                                    //JP (II)
            }
            PC -= 2;
            IM = 0; //Для тестов, потом убрать
            return 1;
        }
        #region IO
        static int POKE(byte r1, byte r2)
        {
            ushort adr = (ushort)(RAM[PC++] + RAM[PC++] * 256);
            RAM[adr++] = r2;
            RAM[adr] = r1;
            return 20;
        }
        static int POKE(ushort Reg)
        {
            ushort adr = (ushort)(RAM[PC++] + RAM[PC++] * 256);
            RAM[adr++] = (byte)(Reg % 256);
            RAM[adr] = (byte)(Reg / 256);
            return 20;
        }
        static void PEEK(ref byte r1, ref byte r2)
        {
            ushort adr = (ushort)(RAM[PC++] + RAM[PC++] * 256);
            r2 = RAM[adr++];
            r1 = RAM[adr];
        }
        static int LDI(bool Rep, bool Down)
        {
            RAM[D * 256 + E] = RAM[H * 256 + L];
            t = RAM[D * 256 + E];
            C--; if (C == 255) B--;
            if (!Down) { E++; if (E == 0) D++; L++; if (L == 0) H++; }
            else { E--; if (E == 255) D--; L--; if (L == 255) H--; }
            if (Rep & (B != 0 | C != 0)) { PC -= 2; return 21; }
            else
            {
                //fY = ((t + A) & 1) != 0; //Опять врут в найденной документации :-(
                f5 = (t & 32) != 0;
                fH = false;
                //fX = ((t + A) & 8) != 0;
                f3 = (t & 8) != 0;
                fV = false;
                fN = false;
                return 16;
            }
        }
        #endregion
        #region Сложение и вычитание
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
            //Протестировано
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
            //fC?
        }
        static void DEC(ref byte r1, ref byte r2)
        {
            r2--;
            if (r2 == 255) r1--;
        }

        //ADD, C - ADC
        static void ADD(byte B, bool C)
        {
            byte a = A;
            byte c = (C & fC) ? (byte)1 : (byte)0;
            A += B;
            A += c;
            if (C & fC) A++;
            fS = (A & 128) != 0;
            fZ = A == 0;
            f5 = (A & 32) != 0;
            fH = (((a & 15) + (B & 15) + c) & 16) != 0;
            f3 = (A & 8) != 0;
            fV = ((a ^ ((255 - B) & 65535)) & (a ^ A) & 128) != 0;
            fN = false;
            fC = a > (B + c);
        }
        static void ADD(ref ushort ii, byte r1, byte r2) //Для IX, IY
        {
            ii += (ushort)(r1 * 256 + r2);
        }
        static void ADDHL(byte r1, byte r2, bool C)
        {
            byte h = H;
            byte c2 = (C & fC) ? (byte)1 : (byte)0;
            byte c1 = L + r2 + c2> 255 ? (byte)1 : (byte)0;
            L = (byte)(L + r2);
            H = (byte)(H + r1 + c1);
            f5 = (H & 32) != 0;
            fH = (((h & 15) + (r1 & 15) + c1) & 16) != 0;
            f3 = (H & 8) != 0;
            //fV = ((a ^ ((255 - B) & 65535)) & (a ^ A) & 128) != 0;
            fN = false;
            fC = H + r1 + c2 > 255;
        }
  
        //SUB, C - SBC
        static void SUB(byte Reg, bool C)
        {
            byte a = A;
            byte c = (C & fC) ? (byte)1 : (byte)0;
            A = (byte)(A - Reg - c);
            fS = (A & 128) != 0;
            fZ = A == 0;
            f5 = (A & 32) != 0;
            fH = (((a & 15) - (Reg & 15) - c) & 16) != 0;
            f3 = (A & 8) != 0;
            fV = ((a ^ Reg) & (a ^ ((a-Reg) & 255)) & 128) != 0;
            fN = true;
            fC = a - Reg - c < 0;
        }
        static void SUB(ref ushort ii, byte r1, byte r2)
        {
            ii -= (ushort)(r1 * 256 + r2);
        }
        static void SBC(byte r1, byte r2)
        {
            byte h = H;
            byte l = L;
            byte c2 = fC ? (byte)1 : (byte)0;
            byte c1 = L - r2 - c2 < 0 ? (byte)1 : (byte)0;
            L = (byte)(L - r2 - c2);
            H = (byte)(H - r1 - c1);
            fS = (h & 128) != 0;
            fZ = (H == 0) & (L == 0);
            f5 = (h & 32) != 0;
            fH = (((h & 15) - (r1 & 15) - c1) & 16) != 0;
            f3 = (H & 8) != 0;
            fV = ((h ^ r1) & (h ^ ((h - r1 - c1) & 255)) & 128) != 0;
            fN = true;
            fC = h - r1 - c1 < 0;
        }

        #endregion
        #region Вращение/сдвиг битов
        //RL/R[C][A] bool c - участие регистра переноса true в RL, RLA, RR, RRA, в остальных false
        static void RL(ref byte B, bool c)
        {
            bool fc = (B & 128) == 128;
            B *= 2;
            if ((c & fC) | (!c & fc)) B |= 1;
            f5 = (B & 32) == 32;
            fH = false;
            f3 = (B & 8) == 8;
            fN = false;
            fC = fc;
        }
        static void RR(ref byte B, bool c)
        {
            bool fc = (B & 1) == 1;
            B /= 2;
            if ((c & fC) | (!c & fc)) B |= 128;
            f5 = (B & 32) == 32;
            fH = false;
            f3 = (B & 8) == 8;
            fN = false;
            fC = fc;
        }
        static void SLA(ref byte Reg)
        {
            fC = (Reg & 128) != 0;
            Reg *= 2;
        }
        static void SRL(ref byte Reg)
        {
            fC = (Reg & 1) != 0;
            Reg /= 2;
            fZ = Reg == 0;
        }
        #endregion
        #region Логика
        //AND
        static void AND(byte Reg)
        {
            A &= Reg;
            fS = (A & 128) != 0;
            fZ = A == 0;
            f5 = (A & 32) != 0;
            fH = true;
            f3 = (A & 8) != 0;
            fV = Parity(A);
            fN = false;
            fC = false;
        }
        //OR
        static void OR(byte Reg)
        {
            A |= Reg;
            fS = (A & 128) != 0;
            fZ = A == 0;
            f5 = (A & 32) != 0;
            fH = false;
            f3 = (A & 8) != 0;
            fV = Parity(A);
            fN = false;
            fC = false;
        }
        //XOR
        static void XOR(byte Reg)
        {
            A ^= Reg;
            fS = (A & 128) != 0;
            fZ = A == 0;
            f5 = (A & 32) != 0;
            fH = false;
            f3 = (A & 8) != 0;
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
        #region Побитовые операции
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
            fS = (bit == 7) & !fZ;
            f5 = (bit == 5) & !fZ;
            fH = true;
            //fX = (bit == 4) & !fZ; //По документам должно быть именно так, но почему-то нет...
            f3 = true;
            //http://www.emuverse.ru/wiki/Zilog_Z80/%D0%A1%D0%B8%D1%81%D1%82%D0%B5%D0%BC%D0%B0_%D0%BA%D0%BE%D0%BC%D0%B0%D0%BD%D0%B4/BIT
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
            f5 = (A & 32) != 0;
            fH = false;
            f3 = (A & 8) != 0;
            fN = false;
            fC = true;
        }
        static void CCF()
        {
            f5 = (A & 32) != 0;
            fH = fC;
            f3 = (A & 8) != 0;
            fN = false;
            fC ^= true;
        }
        static void OUT(ushort port, byte Byte)
        {
            if (port % 256 == 254) Screen.Border = Byte & 7;
        }
        #endregion
    }
}