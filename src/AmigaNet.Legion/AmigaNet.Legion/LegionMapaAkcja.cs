using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmigaNet.Legion
{
    public partial class Legion
    {
        void MAPA_AKCJA()
        {
            DZIEN++;
            BUSY_ANIM();
            //'wygaszanie konflikt�w zbrojnych przez NATO
            for (var I = 0; I <= 5; I++)
            {
                for (var J = 0; J <= 5; J++)
                {
                    amos.Add(ref WOJNA[I, J], -1, 0, WOJNA[I, J]);
                }
            }
            //'wszczynanie du�ych wojen
            var SR = 0;
            for (var PL = 1; PL <= 4; PL++)
            {
                var res = OBLICZ_POWER(PL);
                SR = SR + res;
            }
            POWER = (GRACZE[1, 2] / 900) + 7;
            if (POWER > 99)
            {
                POWER = 99;
            }
            SR = SR / 4;
            //Print At(1,0); "power:",POWER,GRACZE[1, 2],SR
            var OLDSI = 0;
            for (var I = 1; I <= 4; I++)
            {
                var SI = GRACZE[I, 2];
                //Print At(1,I); SI
                if (SI > SR + ((40 * SR) / 100) && SI > OLDSI)
                {
                    OLDSI = SI;
                    for (var J = 2; J <= 4; J++)
                    {
                        WOJNA[J, I] = amos.Rnd(15) + 10;
                    }
                }
            }

            //'nowe legiony chaosu 
            var P = 50 - POWER;
            if (P < 2)
            {
                P = 2;
            }
            if (amos.Rnd(P) == 1)
            {
                for (var I = 20; I <= 39; I++)
                {
                    if (ARMIA[I, 0, TE] == 0)
                    {
                        ARMIA_S[I, 0] = "Wojownicy Chaosu";
                        MESSAGE2(I, "przybyli z piekieł by szerzyć śmierć", 38, 0, 0);
                        NOWA_ARMIA(I, 10, 11);
                        ARMIA[I, 0, TMAG] = 5;
                        var X = amos.Rnd(600) + 20;
                        var Y = amos.Rnd(490) + 10;
                        ARMIA[I, 0, TX] = X;
                        ARMIA[I, 0, TY] = Y;
                        ARMIA[I, 0, TBOB] = 7;
                        TEREN(X, Y);
                        ARMIA[I, 0, TNOGI] = LOK;
                        B_DRAW(I, X, Y, 7);
                        I = 39;
                    }
                }
            }

            //'obs�uga moich armii   
            for (var A = 0; A <= 19; A++)
            {
                if (GAME_OVER) break;
                if (ARMIA[A, 0, TE] > 0)
                {
                    //'comiesi�czne douczanki
                    if (DZIEN % 30 == 0)
                    {
                        for (var I = 1; I <= 10; I++)
                        {
                            if (ARMIA[A, I, TE] > 0)
                            {
                                //var MUNDRY = RASY[RASA, 6];
                                var RASA = ARMIA[A, I, TRASA];
                                var MUNDRY = RASY[RASA, 6];
                                amos.Add(ref ARMIA[A, I, TDOSW], amos.Rnd(MUNDRY), ARMIA[A, I, TDOSW], 95);
                            }
                        }
                    }
                    ARMIA[A, 0, TWAGA] = 0;
                    var TRYB = ARMIA[A, 0, TTRYB];
                    var ZARCIE = ARMIA[A, 0, TAMO];
                    var DNI = ZARCIE / ARMIA[A, 0, TE];
                    if (DNI < 5 && DNI > 0)
                    {
                        MESSAGE(A, "Kończy nam się żywność.", 0, 0);
                    }
                    if (ZARCIE <= 0)
                    {
                        MESSAGE(A, "Oddział rozwiązany", 0, 0);
                        ZABIJ_ARMIE(A);
                        B_CLEAR(A);
                        goto OVER_NEXT;
                    }
                    if (TRYB > 0 && TRYB < 4)
                    {
                        MA_RUCH(A, TRYB);
                    }
                    if (TRYB == 0)
                    {
                        MA_OBOZ(A);
                    }
                    if (TRYB == 4)
                    {
                        MA_POLOWANIE(A);
                    }
                }
            OVER_NEXT:;
            }
            if (GAME_OVER)
            {
                goto OVER;
            }
            //'obs�uga cudzych armii 
            for (var A = 20; A <= 39; A++)
            {
                if (ARMIA[A, 0, TE] > 0)
                {
                    var PL = ARMIA[A, 0, TMAG];
                    if (ARMIA[A, 0, TMAGMA] < 28 && ARMIA[A, 0, TMAGMA] > 0)
                    {
                        ARMIA[A, 0, TMAGMA]--;
                    }
                    var TRYB = ARMIA[A, 0, TTRYB];
                    if (TRYB > 0 && TRYB < 4)
                    {
                        if (amos.Rnd(6) == 1)
                        {
                            MA_WYDAJ_ROZKAZ(PL, A);
                        }
                        else
                        {
                            MA_RUCH(A, TRYB);
                        }
                    }
                    if (TRYB == 0)
                    {
                        MA_OBOZ(A);
                        MA_WYDAJ_ROZKAZ(PL, A);
                    }
                    if (TRYB == 4)
                    {
                        MA_POLOWANIE(A);
                    }
                }
            }
            //'MIASTA
            for (var M = 0; M <= 49; M++)
            {
                var LUDZIE = MIASTA[M, 0, M_LUDZIE];
                if (amos.Rnd(50) == 1 && LUDZIE > 800)
                {
                    PLAGA(M, amos.Rnd(2));
                }
                var CZYJE = MIASTA[M, 0, M_CZYJE];
                //'szajba & podatek modification 
                if (amos.Rnd(5) == 1)
                {
                    amos.Add(ref MIASTA[M, 1, M_MORALE], amos.Rnd(2) - 1, 0, 25);
                }
                var PODATEK = MIASTA[M, 0, M_PODATEK];
                GRACZE[CZYJE, 1] += PODATEK * MIASTA[M, 0, M_LUDZIE] / 25;
                //'obs�uga spichlerzy
                var SPI = 0;
                for (var I = 2; I <= 20; I++)
                {
                    if (MIASTA[M, I, M_LUDZIE] == 9)
                    {
                        SPI++;
                    }
                }
                if (SPI > 0)
                {
                    amos.Add(ref MIASTA[M, 1, M_LUDZIE], LUDZIE / 15, MIASTA[M, 1, M_LUDZIE], SPI * 200);
                }

                if (CZYJE > 1)
                {
                    MIASTA[M, 0, M_LUDZIE] += amos.Rnd(10) - 2;
                    if (GRACZE[CZYJE, 1] > 10000 && amos.Rnd(3) == 1 && MIASTA[M, 1, M_PODATEK] == 0)
                    {
                        for (var I = 20; I <= 39; I++)
                        {
                            if (ARMIA[I, 0, TE] <= 0)
                            {
                                GRACZE[CZYJE, 1] += -10000;
                                MIASTA[M, 1, M_PODATEK] = 20 + amos.Rnd(10);
                                NOWA_ARMIA(I, 10, -1);
                                ARMIA[I, 0, TMAG] = CZYJE;
                                var X = MIASTA[M, 0, M_X];
                                var Y = MIASTA[M, 0, M_Y];
                                ARMIA[I, 0, TX] = X;
                                ARMIA[I, 0, TY] = Y;
                                ARMIA[I, 0, TBOB] = 2 + CZYJE;
                                ARMIA[I, 0, TNOGI] = MIASTA[M, 1, M_X];
                                var KON_S = "";
                                if (amos.Upper_S(amos.Right_S(IMIONA_S[CZYJE], 1)) == "I")
                                {
                                    KON_S = "ego";
                                }
                                else
                                {
                                    KON_S = "a";
                                }
                                ARMIA_S[I, 0] = amos.Str_S(I) + " Legion " + IMIONA_S[CZYJE] + KON_S;
                                B_DRAW(I, X, Y, 2 + CZYJE);
                                I = 39;
                            }
                        }
                    }
                }
                if (CZYJE == 1)
                {
                    var _ATAK = 0;
                    var MORALE = MIASTA[M, 0, M_MORALE];
                    var SZAJBA = MIASTA[M, 1, M_MORALE];
                    LUDZIE = MIASTA[M, 0, M_LUDZIE];
                    LUDZIE += SZAJBA - PODATEK;
                    MORALE += SZAJBA - PODATEK;
                    if (MORALE > 150)
                    {
                        MORALE = 150;
                    }
                    if (MORALE <= 0)
                    {
                        var A_S = "W mieście wybuchł bunt ! ";
                        var JEST = false;
                        for (var I = 0; I <= 19; I++)
                        {
                            if (ARMIA[I, 0, TE] > 0 && ARMIA[I, 0, TNOGI] - 70 == M)
                            {
                                JEST = true;
                                _ATAK = I;
                                I = 19;
                            }
                        }

                        if (JEST)
                        {
                            A_S = A_S + ARMIA_S[_ATAK, 0] + " będzie walczył z rebeliantami.";
                            var TEREN = MIASTA[M, 1, M_X];
                            var LWOJ = (LUDZIE / 70) + 1;
                            if (LWOJ > 10)
                            {
                                LWOJ = 10;
                            }
                            NOWA_ARMIA(40, LWOJ, -1);
                            //'wie�niacy w�r�d buntownik�w 
                            for (var I = 1; I <= 2 + amos.Rnd(2); I++)
                            {
                                NOWA_POSTAC(40, I, 9);
                            }
                            screens.AmalOff(0);
                            screens.ShowOn();
                            CENTER(MIASTA[M, 0, M_X], MIASTA[M, 0, M_Y], 1);
                            MESSAGE(M, A_S, 0, 1);
                            BITWA(_ATAK, 40, 1, 1, 0, 1, 1, 1, TEREN, M);
                            LUDZIE += -(LUDZIE / 4);
                            CENTER(MIASTA[M, 0, M_X], MIASTA[M, 0, M_Y], 0);
                            if (ARMIA[_ATAK, 0, TE] == 0)
                            {
                                JEST = false;
                                A_S = "Rebelianci przejęli miasto.";
                            }
                            else
                            {
                                MORALE = 50;
                                MIASTA[M, 1, M_MORALE] += amos.Rnd(3) + 5;
                            }
                        }

                        if (!JEST)
                        {
                            CENTER(MIASTA[M, 0, M_X], MIASTA[M, 0, M_Y], 1);
                            MESSAGE(M, A_S, 0, 1);
                            MIASTA[M, 0, M_CZYJE] = 0;
                            var MB = 8;
                            if (LUDZIE > 700)
                            {
                                MB++;
                            }
                            screens.PasteBob(MIASTA[M, 0, M_X] - 8, MIASTA[M, 0, M_Y] - 8, MB);
                            MORALE = 30;
                        }
                    }
                    if (LUDZIE < 30)
                    {
                        LUDZIE = 30;
                    }
                    MIASTA[M, 0, M_LUDZIE] = LUDZIE;
                    MIASTA[M, 0, M_MORALE] = MORALE;
                }
                if (MIASTA[M, 1, M_Y] < 25 && MIASTA[M, 1, M_Y] > 0)
                {
                    MIASTA[M, 1, M_Y]--;
                }
                if (MIASTA[M, 1, M_PODATEK] > 0)
                {
                    MIASTA[M, 1, M_PODATEK]--;
                }
            }
            OBLICZ_POWER(1);
        OVER:;
            screens.AmalOff();
            screens.ShowOn();
        }

        int OBLICZ_POWER(int PL)
        {
            var OPOWER = 0;
            if (!GAME_OVER)
            {
                if (PL == 1)
                {
                    GAME_OVER = true;
                }
                for (var I = 0; I <= 40; I++)
                {
                    if (ARMIA[I, 0, TMAG] == PL)
                    {
                        if (ARMIA[I, 0, TE] > 0)
                        {
                            if (PL == 1)
                            {
                                GAME_OVER = false;
                            }
                            OPOWER += ARMIA[I, 0, TSI];
                        }
                    }
                }
                for (var I = 0; I <= 49; I++)
                {
                    if (MIASTA[I, 0, M_CZYJE] == PL)
                    {
                        if (PL == 1)
                        {
                            GAME_OVER = false;
                        }
                        OPOWER += MIASTA[I, 0, M_LUDZIE] * 2;
                    }
                }
                OPOWER += DZIEN * 20;
                OPOWER += GRACZE[PL, 1] / 10;
                GRACZE[PL, 2] = OPOWER;
            }
            return OPOWER;
        }

        void BUSY_ANIM()
        {
            screens.HideOn();
            //Sprite 0,X Mouse, screens.YMouse(),36
            //A_S = "Anim 0,(42,4)(43,4)(44,4)(43,4) ; S: Move XM-X,YM-Y,1 ; Jump S"
            //Amal 0,A_S
            //Amal On 0
        }

        void NOWA_ARMIA(int A, int ILU, int RASA)
        {
            if (A < 20)
            {
                var A_S = "Legion " + amos.Str_S(A + 1);
                ARMIA_S[A, 0] = A_S;
            }
            else
            {
                //'utajnianie
                ARMIA[A, 0, TMAGMA] = 30;
                ARMIA[A, 0, TKORP] = 150 + amos.Rnd(50) + POWER;
            }
            ARMIA[A, 0, TAMO] = amos.Rnd(200);
            ARMIA[A, 0, TE] = ILU;
            var R = RASA;
            var SILA = 0;
            var SPEED = 0;
            for (var I = 1; I <= ILU; I++)
            {
                if (RASA == -1)
                {
                    R = amos.Rnd(8);
                }
                NOWA_POSTAC(A, I, R);
                SILA += ARMIA[A, I, TSI];
                SILA += ARMIA[A, I, TE];
                SPEED += ARMIA[A, I, TSZ];
            }
            SPEED = ((SPEED / ILU) / 5);
            ARMIA[A, 0, TSZ] = SPEED;
            ARMIA[A, 0, TSI] = SILA;
            ARMIA[A, 0, TTRYB] = 0;
        }

        void ZABIJ_ARMIE(int A)
        {
            ARMIA[A, 0, TE] = 0;
            ARMIA[A, 0, TAMO] = 0;
            ARMIA[A, 0, TSI] = 0;
            ARMIA[A, 0, TSZ] = 0;
            ARMIA[A, 0, TTRYB] = 0;
            for (var I = 1; I <= 10; I++)
            {
                ARMIA[A, I, TE] = 0;
            }
        }

        void B_CLEAR(int NR)
        {
            var X = ARMIA[NR, 0, TX];
            var Y = ARMIA[NR, 0, TY];
            screens.ResetZone(NR + 20);
            screens.PutBlock(NR + 1, X - 4, Y - 7);
            var Z1 = screens.Zone(X - 4, Y - 7);
            var Z2 = screens.Zone(X + 4, Y - 7);
            var Z3 = screens.Zone(X - 4, Y);
            var Z4 = screens.Zone(X + 4, Y);
            if (Z1 >= 20 && Z1 < 60)
            {
                B_UPDATE(Z1 - 20);
            }
            if (Z2 >= 20 && Z2 < 60)
            {
                B_UPDATE(Z2 - 20);
            }
            if (Z3 >= 20 && Z3 < 60)
            {
                B_UPDATE(Z3 - 20);
            }
            if (Z4 >= 20 && Z4 < 60)
            {
                B_UPDATE(Z4 - 20);
            }
        }

        void MA_RUCH(int A, int TRYB)
        {
            var PL = ARMIA[A, 0, TMAG];
            var PL2 = 0;
            var X1 = ARMIA[A, 0, TX];
            var Y1 = ARMIA[A, 0, TY];
            if (ARMIA[A, 0, TMAGMA] == 100)
            {
                CENTER(X1, Y1, 1);
            }
            var WOJ = ARMIA[A, 0, TE];
            var ZARCIE = ARMIA[A, 0, TAMO];
            var SPEED = ARMIA[A, 0, TSZ];
            var BB = ARMIA[A, 0, TBOB];
            var X2 = 0;
            var Y2 = 0;
            var ILE_ZRE = 0;
            var A_S = "";
            var SKIP = false;
            var SYMULACJA = false;
            var B = 0;
            var C = 0;

            if (TRYB == 1)
            {
                X2 = ARMIA[A, 0, TCELX];
                Y2 = ARMIA[A, 0, TCELY];
                ILE_ZRE = 1;
            }
            if (TRYB == 2)
            {
                X2 = ARMIA[A, 0, TCELX];
                Y2 = ARMIA[A, 0, TCELY];
                ILE_ZRE = 3;
                SPEED = ARMIA[A, 0, TSZ] * 2;
            }
            if (TRYB == 3)
            {
                C = ARMIA[A, 0, TCELY];
                B = ARMIA[A, 0, TCELX];
                if (C == 0)
                {
                    X2 = ARMIA[B, 0, TX];
                    Y2 = ARMIA[B, 0, TY];
                    A_S = ARMIA_S[B, 0];
                    PL2 = ARMIA[B, 0, TMAG];
                    if (ARMIA[B, 0, TE] <= 0)
                    {
                        ARMIA[A, 0, TTRYB] = 0;
                        return;
                    }
                }
                else
                {
                    X2 = MIASTA[B, 0, M_X];
                    Y2 = MIASTA[B, 0, M_Y];
                    A_S = MIASTA_S[B];
                    PL2 = MIASTA[B, 0, M_CZYJE];
                    if (PL == PL2)
                    {
                        ARMIA[A, 0, TTRYB] = 0;
                        return;
                    }
                }
                ILE_ZRE = 1;

                if (PL != 1 && PL2 != 1)
                {
                    SYMULACJA = true;
                }
            }
            if (A < 20)
            {
                ZARCIE += -WOJ * ILE_ZRE;
                if (ZARCIE < 0)
                {
                    ZARCIE = 0;
                }
                ARMIA[A, 0, TAMO] = ZARCIE;
            }
            var DX = X2 - X1;
            var DY = Y2 - Y1;

            var LTRYB = 0;
            var LOAX = 0;
            var LOAX2 = 0;
            var LOAY = 0;
            var LOAY2 = 0;
            if (Math.Abs(DX) > Math.Abs(DY))
            {
                LTRYB = 3;
                if (DX >= 0)
                {
                    LOAX = 0;
                    LOAX2 = 2;
                }
                else
                {
                    LOAX = 2;
                    LOAX2 = 0;
                }
            }
            else
            {
                LTRYB = 2;
                if (DY >= 0)
                {
                    LOAY = 0;
                    LOAY2 = 2;
                }
                else
                {
                    LOAY = 2;
                    LOAY2 = 0;
                }
            }
            var L_R = Math.Sqrt(DX * DX + DY * DY) + 0.2;
            var VX_R = DX / L_R;
            var VY_R = DY / L_R;
            double X_R = X1;
            double Y_R = Y1;
            B_CLEAR(A);
            for (var I = 0; I <= SPEED; I++)
            {
                X_R = X_R + VX_R;
                Y_R = Y_R + VY_R;
                X1 = (int)X_R;
                Y1 = (int)Y_R;
                DX = X2 - X1;
                DY = Y2 - Y1;
                screens.Bob(1, X1, Y1, BB);
                screens.WaitVbl();
                if (Math.Abs(DX) < 3 && Math.Abs(DY) < 3)
                {
                    break;
                }
                //Thread.Sleep(100);
            }
            screens.BobOff(1);
            screens.WaitVbl();
            TEREN(X1, Y1);
            if (LOK > 69 && LOK < 120 && A < 20)
            {
                MIASTA[LOK - 70, 1, M_Y] = 0;
            }
            DX = X2 - X1;
            DY = Y2 - Y1;
            ARMIA[A, 0, TX] = X1;
            ARMIA[A, 0, TY] = Y1;
            B_DRAW(A, X1, Y1, BB);
            if (LOK > 120 && A < 20)
            {
                var NR = LOK - 121;
                LOK = PRZYGODY[NR, P_TEREN];
                CENTER(X1, Y1, 1);
                MA_PRZYGODA(A, NR);
                //'nie chc� ju� wi�cej przyg�d 
                SKIP = true;
            }
            ARMIA[A, 0, TNOGI] = LOK;
            if (Math.Abs(DX) < 3 && Math.Abs(DY) < 3)
            {
                screens.AmalOff(0);
                screens.ShowOn();
                if (TRYB == 3 && C == 0)
                {
                    var MT = 0;
                    var TEREN = ARMIA[B, 0, TNOGI];
                    ARMIA[A, 0, TNOGI] = TEREN;
                    if (TEREN > 69 && TEREN < 121)
                    {
                        MT = TEREN - 70;
                        TEREN = MIASTA[TEREN - 70, 1, M_X];
                    }
                    else
                    {
                        MT = -1;
                    }
                    if (SYMULACJA)
                    {
                        var res = BITWA_SYMULOWANA(A, B);
                        var LOSER = res;
                        if (ARMIA[LOSER, 0, TMAGMA] == 100)
                        {
                            MESSAGE2(LOSER, " został rozbity.", 33, 0, 0);
                        }
                    }
                    else
                    {
                        if (A > 19)
                        {
                            CENTER(X1, Y1, 1);
                            MESSAGE(A, "zaatakował nasz " + A_S + " ", 0, 0);
                            BITWA(B, A, LOAX2, LOAY2, LTRYB, LOAX, LOAY, LTRYB, TEREN, MT);
                            ARMIA[A, 0, TMAGMA] = 0;
                        }
                        else
                        {
                            CENTER(X1, Y1, 1);
                            MESSAGE(A, "Rozpoczynamy atak na " + A_S, 0, 0);
                            var ILEDNI = amos.Rnd(30) + 10;
                            WOJNA[PL, PL2] = ILEDNI;
                            WOJNA[PL2, PL] = ILEDNI;
                            ARMIA[B, 0, TMAGMA] = 0;
                            BITWA(A, B, LOAX, LOAY, LTRYB, LOAX2, LOAY2, LTRYB, TEREN, MT);
                        }
                        CENTER(X1, Y1, 0);
                        if (ARMIA[B, 0, TE] == 0)
                        {
                            MESSAGE2(B, "został rozbity ", 33, 0, 0);
                        }
                        if (ARMIA[A, 0, TE] == 0)
                        {
                            MESSAGE2(A, "został rozbity ", 33, 0, 0);
                        }
                    }
                }
                if (TRYB == 3 && C == 1)
                {
                    var TEREN = MIASTA[B, 1, M_X];
                    var MUR = MIASTA[B, 0, M_MUR];
                    if (MUR == 0 || PL == 5)
                    {
                        MUR = B;
                    }
                    else
                    {
                        MUR = -MUR - 1;
                    }
                    var MORALE = MIASTA[B, 0, M_MORALE];
                    var LUDZIE = MIASTA[B, 0, M_LUDZIE];
                    var LWOJ = (LUDZIE / 70) + 1;
                    if (LWOJ > 10)
                    {
                        LWOJ = 10;
                    }
                    var MB = 8 + PL * 2;
                    if (LUDZIE > 700)
                    {
                        MB++;
                    }
                    KTO_ATAKUJE = A;
                    if (SYMULACJA)
                    {
                        var OLDPOWER = POWER;
                        POWER = (MORALE / 3) + 10;
                        NOWA_ARMIA(40, LWOJ, -1);
                        //'wi�kszy ostrza� 
                        ARMIA[40, 0, TKORP] = 150 + POWER;
                        BITWA_SYMULOWANA(A, 40);
                        amos.Add(ref MIASTA[B, 0, M_LUDZIE], -(LUDZIE / 4), 20, MIASTA[B, 0, M_LUDZIE]);
                        POWER = OLDPOWER;
                        if (ARMIA[40, 0, TE] == 0)
                        {
                            if (PL == 5)
                            {
                                MIASTA[B, 0, M_CZYJE] = 0;
                                amos.Add(ref MIASTA[B, 0, M_LUDZIE], -(LUDZIE / 2), 20, MIASTA[B, 0, M_LUDZIE]);
                                for (var I = 2; I <= 10; I++)
                                {
                                    if (amos.Rnd(1) == 1)
                                    {
                                        MIASTA[B, I, M_LUDZIE] = 0;
                                    };
                                }
                                MESSAGE2(B, "Wojownicy Chaosu spalili miasto.", 32, 1, 0);
                                MB = 8;
                            }
                            else
                            {
                                MIASTA[B, 0, M_CZYJE] = PL;
                            }
                            B_OFF(A);
                            screens.PasteBob(MIASTA[B, 0, M_X] - 8, MIASTA[B, 0, M_Y] - 8, MB);
                            screens.WaitVbl();
                            B_DRAW(A, X1, Y1, BB);
                            if (ARMIA[A, 0, TMAGMA] == 100)
                            {
                                CENTER(X1, Y1, 1);
                                MESSAGE2(A, "Zdobył osadę " + A_S, 32, 0, 0);
                            }
                        }
                        if (ARMIA[A, 0, TMAGMA] == 100)
                        {
                            if (ARMIA[A, 0, TE] == 0)
                            {
                                CENTER(X1, Y1, 1);
                                MESSAGE2(A, "został rozbity w czasie szturmu na miasto " + A_S, 33, 0, 0);
                            }
                        }
                    }
                    else
                    {
                        if (A > 19 && PL2 == 1)
                        {
                            var OBRONA = -1;
                            for (var I = 0; I <= 19; I++)
                            {
                                if (ARMIA[I, 0, TE] > 0 && ARMIA[I, 0, TNOGI] == 70 + B)
                                {
                                    OBRONA = I;
                                    I = 19;
                                }
                            }
                            if (OBRONA > -1)
                            {
                                CENTER(X1, Y1, 1);
                                var KON_S = "";
                                if (PL == 5)
                                {
                                    KON_S = "ą";
                                }
                                else
                                {
                                    KON_S = "e";
                                }
                                MESSAGE(A, "atakuj" + KON_S + " naszą osadę " + A_S + " ", 0, 0);
                                for (var I = 1; I <= 10; I++)
                                {
                                    ARMIA[A, I, TAMO] = 30;
                                }
                                BITWA(OBRONA, A, 0, 0, 2, 0, 2, 2, TEREN, MUR);
                                amos.Add(ref MIASTA[B, 0, M_LUDZIE], -(LUDZIE / 4), 20, MIASTA[B, 0, M_LUDZIE]);
                                CENTER(X1, Y1, 0);
                                if (WYNIK_AKCJI != 1)
                                {
                                    if (PL == 5)
                                    {
                                        MIASTA[B, 0, M_CZYJE] = 0;
                                        amos.Add(ref MIASTA[B, 0, M_LUDZIE], -(LUDZIE / 2), 20, MIASTA[B, 0, M_LUDZIE]);
                                        for (var I = 2; I <= 20; I++)
                                        {
                                            if (amos.Rnd(2) == 1)
                                            {
                                                MIASTA[B, I, M_LUDZIE] = 0;
                                            };
                                        }
                                        MESSAGE2(B, "Wojownicy Chaosu zdobylii i spalili miasto.", 32, 1, 0);
                                        MB = 8;
                                    }
                                    else
                                    {
                                        MIASTA[B, 0, M_CZYJE] = PL;
                                        MESSAGE2(A, "zdobył naszą osadę " + A_S + " ", 30, 0, 0);
                                    }
                                    B_OFF(A);
                                    screens.PasteBob(MIASTA[B, 0, M_X] - 8, MIASTA[B, 0, M_Y] - 8, MB);
                                    screens.WaitVbl();
                                    B_DRAW(A, X1, Y1, BB);
                                }
                            }
                            else
                            {
                                if (PL == 5)
                                {
                                    MIASTA[B, 0, M_CZYJE] = 0;
                                    amos.Add(ref MIASTA[B, 0, M_LUDZIE], -(LUDZIE / 2), 20, MIASTA[B, 0, M_LUDZIE]);
                                    for (var I = 2; I <= 20; I++)
                                    {
                                        if (amos.Rnd(2) == 1)
                                        {
                                            MIASTA[B, I, M_LUDZIE] = 0;
                                        };
                                    }
                                    MESSAGE2(B, "miasto zostało spalone ", 32, 1, 0);
                                    MB = 8;
                                }
                                else
                                {
                                    MIASTA[B, 0, M_CZYJE] = PL;
                                    CENTER(X1, Y1, 1);
                                    MESSAGE(A, "zajął naszą osadę " + A_S + " ", 0, 0);
                                }
                                B_OFF(A);
                                screens.PasteBob(MIASTA[B, 0, M_X] - 8, MIASTA[B, 0, M_Y] - 8, MB);
                                screens.WaitVbl();
                                B_DRAW(A, X1, Y1, BB);
                            }
                        }
                        else
                        {
                            CENTER(X1, Y1, 1);
                            MESSAGE(A, "Rozpoczynamy atak na osadę " + A_S, 0, 0);
                            var ILEDNI = amos.Rnd(30) + 10;
                            WOJNA[PL, PL2] = ILEDNI;
                            WOJNA[PL2, PL] = ILEDNI;
                            var OLDPOWER = POWER;
                            POWER = (MORALE / 3) + 10;
                            NOWA_ARMIA(40, LWOJ, -1);
                            for (var I = 1; I <= 10; I++)
                            {
                                ARMIA[40, I, TAMO] = 30;
                            }
                            BITWA(A, 40, 0, 2, 2, 0, 0, 2, TEREN, MUR);
                            amos.Add(ref MIASTA[B, 0, M_LUDZIE], -(LUDZIE / 4), 20, MIASTA[B, 0, M_LUDZIE]);
                            POWER = OLDPOWER;
                            CENTER(X1, Y1, 0);
                            if (ARMIA[40, 0, TE] == 0)
                            {
                                MIASTA[B, 0, M_CZYJE] = PL;
                                B_OFF(A);
                                screens.PasteBob(MIASTA[B, 0, M_X] - 8, MIASTA[B, 0, M_Y] - 8, MB);
                                screens.WaitVbl();
                                B_DRAW(A, X1, Y1, BB);
                                for (var J = 1; J <= 19; J++)
                                {
                                    MIASTA[B, J, M_MUR] = amos.Rnd(20);
                                }
                                MESSAGE2(A, "Zdobyliśmy osadę " + A_S + " ", 30, 0, 0);
                            }
                            if (ARMIA[A, 0, TE] == 0)
                            {
                                MESSAGE2(A, "został rozbity w czasie szturmu na miasto " + A_S, 33, 0, 0);
                            }
                        }
                    }
                }
                ARMIA[A, 0, TTRYB] = 0;
                BUSY_ANIM();
            }
            else
            {
                if (A < 20 && !SKIP)
                {
                    if (LOK == 7)
                    {
                        if (amos.Rnd(3) == 1)
                        {
                            MA_WYPADEK(A, 3);
                        }
                    }
                    if (LOK == 5)
                    {
                        if (amos.Rnd(6) == 1)
                        {
                            MA_WYPADEK(A, 1);
                        }
                    }
                    if (LOK == 1)
                    {
                        var P = amos.Rnd(45);
                        if (P < 4)
                        {
                            MA_WYPADEK(A, 4 + P);
                        }
                        if (P == 5)
                        {
                            MA_WYPADEK(A, 2);
                        }
                    }
                    if (LOK == 2)
                    {
                        var P = amos.Rnd(45);
                        if (P == 1)
                        {
                            MA_WYPADEK(A, 2);
                        }
                        if (P == 2)
                        {
                            MA_WYPADEK(A, 6);
                        }
                    }
                    if (LOK == 3)
                    {
                        var P = amos.Rnd(45);
                        if (P == 1)
                        {
                            MA_WYPADEK(A, 2);
                        }
                    }
                    if (LOK == 4)
                    {
                        var P = amos.Rnd(45);
                        if (P == 1)
                        {
                            MA_WYPADEK(A, 2);
                        }
                        if (P == 5)
                        {
                            MA_WYPADEK(A, 7);
                        }
                    }

                }
            }
        }

        void MA_WYPADEK(int A, int TYP)
        {
            var TEREN = ARMIA[A, 0, TNOGI];
            var A_S = "";
            var PO_S = "";
            var XT = 0;
            var YT = 0;
            var POT = 0;
            var ILE = 0;
            var BB = 0;

            if (TYP == 1)
            {
                A_S = "Osaczyło nas stado wściekłych wilków ! ";
                PO_S = "wilk";
                XT = 1;
                YT = 1;
                POT = 12;
                ILE = amos.Rnd(5) + 5;
                BB = 34;
            }
            if (TYP == 2)
            {
                A_S = "Zaatakowali nas zbóje ";
                POT = -1;
                ILE = amos.Rnd(5) + 5;
                XT = amos.Rnd(2);
                YT = amos.Rnd(2);
                BB = 31;
            }
            if (TYP == 3)
            {
                A_S = "Utknęliśmy na bagnach ";
                PO_S = "gloom";
                POT = 14;
                ILE = amos.Rnd(5);
                XT = 1;
                YT = 1;
                BB = 34;
            }
            if (TYP == 4)
            {
                A_S = "Okrążyły nas leśne trole ";
                POT = 6;
                ILE = amos.Rnd(5) + 5;
                XT = 1;
                YT = 1;
                BB = 31;
            }
            if (TYP == 5)
            {
                A_S = "Gargoil !!!";
                PO_S = "gargoil";
                POT = 10;
                ILE = amos.Rnd(1) + 1;
                XT = 1;
                YT = 1;
                BB = 34;
            }
            if (TYP == 6)
            {
                A_S = "Spotkaliśmy samotnego wojownika ";
                POT = -1;
                ILE = 1;
                XT = amos.Rnd(2);
                YT = amos.Rnd(2);
                BB = 34;
            }
            if (TYP == 7)
            {
                A_S = "Odnaleźliśmy wejście do jaskini ";
                POT = 18;
                ILE = 5;
                XT = 1;
                YT = 0;
                PO_S = "pająk";
                TEREN = 8;
                BB = 34;
            }
            screens.AmalOff();
            screens.ShowOn();
            var X = ARMIA[A, 0, TX];
            var Y = ARMIA[A, 0, TY];
            CENTER(X, Y, 1);
            MESSAGE2(A, A_S, BB, 0, 1);
            ARM = A;
            WRG = 40;
            screens.SpriteOff(2);
            SETUP("Akcja", "w", "terenie");
            for (var I = 0; I <= 10; I++)
            {
                ARMIA[40, I, TE] = 0;
            }
            if (POT > 9)
            {
                POTWOR(40, PO_S, ILE, POT);
                if (TYP == 7)
                {
                    for (var I = 6; I <= 10; I++)
                    {
                        NOWA_POSTAC(40, I, amos.Rnd(8));
                        ARMIA[40, I, TKORP] = 150 + amos.Rnd(50);
                    }
                }
            }
            else
            {
                NOWA_ARMIA(40, ILE, POT);
                var AGRESJA = ARMIA[WRG, 0, TKORP];
                if (TYP == 6)
                {
                    ARMIA[WRG, 1, TDOSW] = amos.Rnd(30) + 20;
                    AGRESJA = 40;
                }
                for (var I = 1; I <= 10; I++)
                {
                    ARMIA[WRG, I, TKORP] = AGRESJA;
                }
            }
            RYSUJ_SCENERIE(TEREN, -1);
            USTAW_WOJSKO(ARM, XT, YT, 0);
            USTAW_WOJSKO(WRG, XT, YT, 1);
            MAIN_ACTION();
            for (var I = 0; I <= 10; I++)
            {
                ARMIA[40, I, TE] = 0;
            }
            SETUP0();
            VISUAL_OBJECTS();
            screens.Sprite(2, SPX, SPY, 1);
            CENTER(ARMIA[A, 0, TX], ARMIA[A, 0, TY], 0);
            BUSY_ANIM();
        }

        void POTWOR(int A, String A_S, int ILU, int RASA)
        {
            for (var I = 1; I <= 16; I++)
            {
                screens.DelBob(POTWORY + 1);
            }
            _LOAD("dane/potwory/" + A_S, 1);
            _LOAD("dane/potwory/" + A_S + ".snd", 9);
            ARMIA[A, 0, TE] = ILU;
            ARMIA[A, 0, TKORP] = RASY[RASA, 5];
            for (var I = 1; I <= ILU; I++)
            {
                NOWA_POSTAC(A, I, RASA);
            }
        }

        void BITWA(int A, int B, int X1, int Y1, int T1, int X2, int Y2, int T2, int SCENERIA, int WIES)
        {
            ARM = A;
            WRG = B;
            var PL2 = ARMIA[B, 0, TMAG];
            screens.SpriteOff(2);
            SETUP("", "Bitwa", "");
            if (ARMIA[B, 0, TMAG] == 5)
            {
                for (var I = 1; I <= 16; I++)
                {
                    screens.DelBob(POTWORY + 1);
                }
                _LOAD("dane/potwory/szkielet", 1);
                _LOAD("dane/potwory/szkielet.snd", 9);
            }
            RYSUJ_SCENERIE(SCENERIA, WIES);
            var AGRESJA = ARMIA[B, 0, TKORP];
            for (var I = 1; I <= 10; I++)
            {
                ARMIA[WRG, I, TKORP] = AGRESJA;
            }
            USTAW_WOJSKO(A, X1, Y1, T1);
            USTAW_WOJSKO(B, X2, Y2, T2);
            MAIN_ACTION();
            SETUP0();
            VISUAL_OBJECTS();
            screens.Sprite(2, SPX, SPY, 1);
        }

        int BITWA_SYMULOWANA(int A, int B)
        {
            var S1 = ARMIA[A, 0, TSI] + amos.Rnd(100);
            var S2 = ARMIA[B, 0, TSI] + amos.Rnd(100);
            var DS = S1 - S2;

            var WINNER = 0;
            var LOSER = 0;
            var S3 = 0;
            var WOJ = 0;
            var SILA = 0;
            var SPEED = 0;

            if (DS >= 0)
            {
                WINNER = A;
                LOSER = B;
                S3 = S2 / 15;
            }
            else
            {
                WINNER = B;
                LOSER = A;
                S3 = S2 / 15;
            }
            ZABIJ_ARMIE(LOSER);
            if (LOSER < 40)
            {
                B_CLEAR(LOSER);
            }
            for (var I = 1; I <= 10; I++)
            {
                ARMIA[WINNER, I, TE] += -S3;
                if (ARMIA[WINNER, I, TE] < 0)
                {
                    ARMIA[WINNER, I, TE] = 0;
                }
                if (ARMIA[WINNER, I, TE] > 0)
                {
                    WOJ++;
                    SILA += ARMIA[WINNER, I, TSI];
                    SILA += ARMIA[WINNER, I, TE];
                    SPEED += ARMIA[WINNER, I, TSZ];
                }
            }
            SPEED = ((SPEED / WOJ) / 5);
            ARMIA[WINNER, 0, TSZ] = SPEED;
            ARMIA[WINNER, 0, TSI] = SILA;
            ARMIA[WINNER, 0, TE] = WOJ;

            return LOSER;
        }

        void MA_PRZYGODA(int A, int NR)
        {
            var B = 0;
            var CO = 0;
            screens.AmalOff(0);
            screens.ShowOn();
            var TEREN = PRZYGODY[NR, P_TEREN];
            var TYP = PRZYGODY[NR, P_TYP];
            var A_S = PRZYGODY_S[TYP, 7];
            MESSAGE(A, A_S, NR, 0);
            screens.SpriteOff(2);
            //'�adnych komunikat�w po akcji 0 armie lub 1 miasta   
            var M = -1;
            ARM = A;
            WRG = 40;
            TRWA_PRZYGODA = NR;
            SETUP("Akcja", "w", "terenie");

            var POT = 0;
            var ILE = 0;
            var PO_S = "";

            if (TYP == 1)
            {
                //'kopalnia koboldy
                if (amos.Rnd(1) == 0)
                {
                    POT = 16;
                    ILE = 3;
                    PO_S = "skirial";
                }
                else
                {
                    POT = 18;
                    ILE = 5;
                    PO_S = "pająk";
                }
                POTWOR(40, PO_S, ILE, POT);
                for (var I = ILE + 1; I <= 10; I++)
                {
                    NOWA_POSTAC(40, I, 3);
                }
                for (var I = 1; I <= 10; I++)
                {
                    ARMIA[WRG, I, TPRAWA] = -1;
                }
                RYSUJ_SCENERIE(TEREN, -1);
                //'dosypujemy troch� skarb�w 
                for (var I = 1; I <= amos.Rnd(8) + 8; I++)
                {
                    var X = amos.Rnd(29) + 70;
                    var Y = amos.Rnd(3);
                    var BB = BIBY + 12;
                    CO = amos.Rnd(3) + 80;
                    if (amos.Rnd(3) == 1)
                    {
                        CO = amos.Rnd(MX_WEAPON);
                        BB = BIBY + 11;
                    }
                    GLEBA[X, Y] = CO;
                    var XB = (X % 10) * 64 + amos.Rnd(30);
                    var YB = (X / 10) * 50 + amos.Rnd(20);
                    screens.PasteBob(XB, YB, BB);
                }
                for (var I = 1; I <= 10; I++)
                {
                    ARMIA[WRG, I, TKORP] = 180;
                }
                USTAW_WOJSKO(ARM, 1, 0, 0);
                USTAW_WOJSKO(WRG, 1, 2, 2);
                MAIN_ACTION();
            }
            if (TYP == 2 || TYP == 8)
            {
                //'grobowiec upiory
                if (amos.Rnd(1) == 0)
                {
                    POT = 17;
                    ILE = 9;
                    PO_S = "humanoid";
                }
                else
                {
                    POT = 18;
                    ILE = 9;
                    PO_S = "pająk";
                }
                POTWOR(40, PO_S, ILE, POT);
                for (var I = 1; I <= 10; I++)
                {
                    ARMIA[WRG, I, TKORP] = 180;
                }
                NOWA_POSTAC(WRG, 10, 9);
                ARMIA[WRG, 10, TKORP] = 40;
                ARMIA[WRG, 10, TPRAWA] = -1;
                RYSUJ_SCENERIE(TEREN, -1);
                //'umieszczenie skarbu 

                var res = SEKTOR(460 + 12, 28 + 22);
                GLEBA[res, 0] = PRZYGODY[NR, P_BRON];
                for (var I = 1; I <= 3; I++)
                {
                    GLEBA[res, I] = 80;
                }
                screens.PasteBob(460, 28, BIBY + 11);

                USTAW_WOJSKO(ARM, 0, 2, 0);
                USTAW_WOJSKO(WRG, 0, 2, 1);
                MAIN_ACTION();
            }
            if (TYP == 3)
            {
                //'bandyci 
                POT = -1;
                ILE = amos.Rnd(5) + 5;
                NOWA_ARMIA(40, ILE, POT);
                for (var I = 1; I <= ILE; I++)
                {
                    ARMIA[WRG, I, TPRAWA] = -1;
                }
                RYSUJ_SCENERIE(TEREN, -1);
                //'silna i do�wiadczona za�oga show no mercy && kill them all 
                for (var I = 1; I <= 10; I++)
                {
                    ARMIA[WRG, I, TKORP] = 250;
                }
                for (var I = 1; I <= 10; I++)
                {
                    ARMIA[WRG, I, TDOSW] = amos.Rnd(25);
                }
                USTAW_WOJSKO(ARM, 1, 1, 1);
                USTAW_WOJSKO(WRG, 1, 1, 0);
                MAIN_ACTION();
                if (WYNIK_AKCJI == 1)
                {
                    M = 0;
                    CO = ARM;
                    A_S = "W nagrodę otrzymujesz " + amos.Str_S(PRZYGODY[NR, P_NAGRODA]) + " sztuk złota.";
                    B = 41;
                    GRACZE[1, 1] += PRZYGODY[NR, P_NAGRODA];
                }
            }
            if (TYP == 4)
            {
                //'c�rka kr�la 
                POT = -1;
                ILE = 9;
                var POS = 4;
                NOWA_ARMIA(40, ILE, POT);
                RYSUJ_SCENERIE(TEREN, -1);
                for (var I = 1; I <= 10; I++)
                {
                    ARMIA[WRG, I, TKORP] = 170;
                }
                NOWA_POSTAC(WRG, 10, POS);
                ARMIA[WRG, 10, TKORP] = 40;
                ARMIA[WRG, 10, TPRAWA] = -1;
                USTAW_WOJSKO(ARM, 1, 2, 0);
                USTAW_WOJSKO(WRG, 1, 0, 0);
                MAIN_ACTION();
                if (WYNIK_AKCJI == 1 && ARMIA[WRG, 10, TE] > 0)
                {
                    CO = PRZYGODY[NR, P_NAGRODA];
                    M = 1;
                    B = 30;
                    A_S = "Przechodzi w twoje władanie jako nagroda.";
                    MIASTA[CO, 0, M_CZYJE] = 1;
                }
            }
            if (TYP == 5)
            {
                //'g�ra szczerbiec 
                POTWOR(WRG, "skirial", 5, 16);
                RYSUJ_SCENERIE(TEREN, -1);
                GLEBA[5, amos.Rnd(3)] = 7;
                screens.PasteBob(330, 12, BIBY + 11);
                for (var I = 1; I <= 10; I++)
                {
                    ARMIA[WRG, I, TKORP] = 170;
                }
                USTAW_WOJSKO(ARM, 1, 2, 0);
                USTAW_WOJSKO(WRG, 1, 0, 2);
                MAIN_ACTION();
            }
            if (TYP == 6)
            {
                //'super mag i wilki 
                POTWOR(WRG, "wilk", 9, 12);
                NOWA_POSTAC(WRG, 10, 8);
                var MAGIA = 50 + amos.Rnd(50);
                var ENERGIA = 30 + amos.Rnd(40);
                var SILA = 20 + amos.Rnd(10);
                var SZYBKOSC = 10 + amos.Rnd(10);
                var _DOS = 50;
                RYSUJ_SCENERIE(TEREN, -1);
                for (var I = 1; I <= 10; I++)
                {
                    ARMIA[WRG, I, TKORP] = 170;
                }
                ARMIA_S[WRG, 10] = IM_PRZYGODY_S[NR];
                ARMIA[WRG, 10, TGLOWA] = 1;
                ARMIA[WRG, 10, TKORP] = 40;
                ARMIA[WRG, 10, TEM] += ENERGIA;
                ARMIA[WRG, 10, TSI] += SILA;
                ARMIA[WRG, 10, TSZ] += SZYBKOSC;
                ARMIA[WRG, 10, TMAGMA] += MAGIA;
                ARMIA[WRG, 10, TDOSW] += _DOS;
                //'b�dzie gada�
                ARMIA[WRG, 10, TPRAWA] = -1;
                USTAW_WOJSKO(ARM, 0, 1, 0);
                USTAW_WOJSKO(WRG, 2, 1, 0);
                MAIN_ACTION();
            }
            if (TYP == 7)
            {
                //'super paladyn i szkielety 
                POTWOR(WRG, "szkielet", 9, 11);
                NOWA_POSTAC(WRG, 10, 7);
                RYSUJ_SCENERIE(TEREN, -1);
                for (var I = 1; I <= 10; I++)
                {
                    ARMIA[WRG, I, TKORP] = 240;
                }
                var MAGIA = 20 + amos.Rnd(20);
                var ENERGIA = 60 + amos.Rnd(50);
                var SILA = 20 + amos.Rnd(20);
                var SZYBKOSC = 20 + amos.Rnd(20);
                var _DOS = 80;
                ARMIA[WRG, 10, TKORP] = 35;
                ARMIA_S[WRG, 10] = IM_PRZYGODY_S[NR];
                ARMIA[WRG, 10, TEM] += ENERGIA;
                ARMIA[WRG, 10, TSI] += SILA;
                ARMIA[WRG, 10, TSZ] += SZYBKOSC;
                ARMIA[WRG, 10, TMAGMA] += MAGIA;
                ARMIA[WRG, 10, TDOSW] += _DOS;
                ARMIA[WRG, 10, TPRAWA] = -1;
                USTAW_WOJSKO(ARM, 1, 0, 0);
                USTAW_WOJSKO(WRG, 1, 2, 2);
                MAIN_ACTION();
            }

            if (TYP == 9)
            {
                //'�wi�tynia ork�w 
                NOWA_ARMIA(40, 10, 1);
                for (var I = 1; I <= 10; I++)
                {
                    ARMIA[WRG, I, TPRAWA] = -1;
                }
                for (var I = 1; I <= 10; I++)
                {
                    ARMIA[WRG, I, TKORP] = 180;
                }
                RYSUJ_SCENERIE(TEREN, -1);
                WIDOCZNOSC = 250;
                //'umieszczenie trup�w 
                for (var Y = 0; Y <= 20; Y++)
                {
                    for (var X = 0; X <= 10; X++)
                    {
                        var XB = X * 60;
                        var YB = Y * 25;
                        if (amos.Rnd(8) == 1 && screens.Zone(XB + 10, YB + 10) == 0)
                        {
                            var BB = amos.Rnd(9) * 16 + 18 + 63 + 16;
                            screens.PasteBob(XB + 10, YB + 10, BB);
                        }
                    }
                }
                USTAW_WOJSKO(ARM, 1, 2, 0);
                USTAW_WOJSKO(WRG, 1, 2, 1);
                MAIN_ACTION();
            }

            if (TYP == 10)
            {
                //'barbarzy�ca na bagnach
                var MAGIA = 10 + amos.Rnd(20);
                var ENERGIA = 40 + amos.Rnd(40);
                var SILA = 20 + amos.Rnd(20);
                var SZYBKOSC = 10 + amos.Rnd(10);
                var _DOS = 60;
                POTWOR(40, "gloom", 9, 14);
                RYSUJ_SCENERIE(TEREN, -1);
                for (var I = 1; I <= 10; I++)
                {
                    ARMIA[WRG, I, TKORP] = 170;
                }
                NOWA_POSTAC(WRG, 10, 0);
                ARMIA_S[WRG, 10] = IM_PRZYGODY_S[NR];
                ARMIA[WRG, 10, TGLOWA] = 1;
                ARMIA[WRG, 10, TKORP] = 40;
                ARMIA[WRG, 10, TEM] += ENERGIA;
                ARMIA[WRG, 10, TSI] += SILA;
                ARMIA[WRG, 10, TSZ] += SZYBKOSC;
                ARMIA[WRG, 10, TMAGMA] += MAGIA;
                ARMIA[WRG, 10, TDOSW] += _DOS;
                //'b�dzie gada�
                ARMIA[WRG, 10, TPRAWA] = -1;
                USTAW_WOJSKO(ARM, 0, 1, 0);
                USTAW_WOJSKO(WRG, 2, 1, 0);
                MAIN_ACTION();
            }

            if (TYP == 11)
            {
                //'wataha z gargoilami 
                POTWOR(WRG, "gargoil", 2, 10);
                for (var I = 3; I <= 5 + amos.Rnd(4); I++)
                {
                    NOWA_POSTAC(WRG, I, amos.Rnd(8));
                }
                RYSUJ_SCENERIE(TEREN, -1);
                //'umieszczenie trup�w 
                for (var Y = 0; Y <= 20; Y++)
                {
                    for (var X = 0; X <= 10; X++)
                    {
                        var XB = X * 60;
                        var YB = Y * 25;
                        if (amos.Rnd(8) == 1 && screens.Zone(XB + 10, YB + 10) == 0)
                        {
                            var BB = amos.Rnd(9) * 16 + 18 + 63 + 16;
                            screens.PasteBob(XB + 10, YB + 10, BB);
                        }
                    }
                }
                //'silna i do�wiadczona za�oga show no mercy && kill them all 
                for (var I = 1; I <= 10; I++)
                {
                    ARMIA[WRG, I, TKORP] = 250;
                }
                for (var I = 1; I <= 10; I++)
                {
                    ARMIA[WRG, I, TDOSW] = amos.Rnd(25);
                }
                //'tworzenie bossa 
                var MAGIA = 10 + amos.Rnd(20);
                var ENERGIA = 40 + amos.Rnd(40);
                var SILA = 20 + amos.Rnd(20);
                var SZYBKOSC = 10 + amos.Rnd(10);
                var _DOS = 60;
                NOWA_POSTAC(WRG, 10, PRZYGODY[NR, P_BRON]);
                ARMIA_S[WRG, 10] = IM_PRZYGODY_S[NR];
                ARMIA[WRG, 10, TGLOWA] = 1;
                ARMIA[WRG, 10, TKORP] = 170;
                ARMIA[WRG, 10, TEM] += ENERGIA;
                ARMIA[WRG, 10, TE] += ENERGIA;
                ARMIA[WRG, 10, TSI] += SILA;
                ARMIA[WRG, 10, TSZ] += SZYBKOSC;
                ARMIA[WRG, 10, TMAG] += MAGIA;
                ARMIA[WRG, 10, TDOSW] += _DOS;
                //'b�dzie gada�
                ARMIA[WRG, 10, TPRAWA] = -1;
                USTAW_WOJSKO(ARM, 1, 2, 0);
                USTAW_WOJSKO(WRG, 1, 0, 0);
                MAIN_ACTION();
                if (ARMIA[WRG, 10, TE] <= 0)
                {
                    M = 0;
                    CO = ARM;
                    A_S = "Za głowę zbira otrzymujesz " + amos.Str_S(PRZYGODY[NR, P_NAGRODA]) + " sztuk złota.";
                    B = 41;
                    GRACZE[1, 1] += PRZYGODY[NR, P_NAGRODA];
                }
            }

            if (TYP == 12)
            {
                //'jaskinia wiedzy 
                NOWA_POSTAC(WRG, 10, 8);
                RYSUJ_SCENERIE(TEREN, -1);
                ARMIA[WRG, 10, TGLOWA] = 1;
                ARMIA[WRG, 10, TKORP] = 40;
                ARMIA[WRG, 10, TDOSW] = 60 + amos.Rnd(40);
                ARMIA[WRG, 10, TPRAWA] = -1;
                USTAW_WOJSKO(ARM, 1, 0, 0);
                USTAW_WOJSKO(WRG, 1, 2, 0);
                for (var I = 1; I <= 10; I++)
                {
                    ARMIA[ARM, I, TDOSW] += 40 + amos.Rnd(40);
                    if (ARMIA[ARM, I, TDOSW] > 95)
                    {
                        ARMIA[ARM, I, TDOSW] = 95;
                    }
                }
                MAIN_ACTION();
            }
            if (TYP == 13)
            {
                //'goto SKIP2
                //'sparing ko�cowy 
                //'obrona bagien 
                POTWOR(WRG, "szkielet", 10, 11);
                RYSUJ_SCENERIE(7, -3);
                KTO_ATAKUJE = ARM;
                USTAW_WOJSKO(ARM, 0, 2, 2);
                USTAW_WOJSKO(WRG, 0, 0, 2);
                MAIN_ACTION();
                if (WYNIK_AKCJI == 1)
                {
                    for (var I = 0; I <= 10; I++)
                    {
                        ARMIA[40, I, TE] = 0;
                    }
                    screens.HideOn();
                    _LOAD("grob.hb", 3);
                    _LOAD("mod.end2", 6);
                    amos.TrackLoopOn();
                    amos.TrackPlay(3);

                    USTAW_FONT("defender2", 8);
                    OUTLINE(10, 200, "Władca Chaosu przebywa", 1, 0);
                    OUTLINE(10, 210, "w swojej podziemnej", 1, 0);
                    OUTLINE(10, 220, "krypcie grobowej,", 1, 0);
                    OUTLINE(10, 230, "z której zatruwa całe", 1, 0);
                    OUTLINE(10, 240, "królestwo.", 1, 0);
                    screens.View();
                    _WAIT(1500);
                    //Fade 2
                    _TRACK_FADE(1);
                    //Erase 3
                    SETUP("Bitwa", "z Władcą", "Chaosu");
                    //'sparing z bossem
                    POTWOR(WRG, "boss", 1, 19);
                    RYSUJ_SCENERIE(TEREN, -1);
                    for (var I = 1; I <= 10; I++)
                    {
                        ARMIA[WRG, I, TKORP] = 180;
                    }
                    USTAW_WOJSKO(ARM, 1, 1, 0);
                    USTAW_WOJSKO(WRG, 1, 1, 0);
                    MAIN_ACTION();
                    if (WYNIK_AKCJI == 1)
                    {
                        PRZYGODY[NR, P_TYP] = 0;
                        GAME_OVER = true;
                        screens.HideOn();
                        _LOAD("pobieda.hb", 3);
                        _LOAD("mod.2sample+", 6);
                        amos.TrackLoopOn();
                        amos.TrackPlay(3);

                        USTAW_FONT("defender2", 8);
                        OUTLINE(10, 200, "Oto ten, który niszczył", 1, 0);
                        OUTLINE(10, 210, "wszelkie życie ", 1, 0);
                        OUTLINE(10, 220, "leży teraz martwy", 1, 0);
                        OUTLINE(10, 230, "u twych stóp", 1, 0);
                        OUTLINE(10, 240, "", 1, 0);
                        A_S = "Twoja przygoda dobiegła końca.";
                        M = 0;
                        CO = ARM;
                        B = 34;
                        screens.View();
                        _WAIT(3500);
                        //Fade 2
                        _TRACK_FADE(1);
                        banks.Erase(3);
                    }
                }
            }

            TRWA_PRZYGODA = -1;
            for (var I = 0; I <= 10; I++)
            {
                ARMIA[40, I, TE] = 0;
            }
            ARMIA[A, 0, TTRYB] = 0;
            if (TYP != 13)
            {
                PRZYGODY[NR, P_TYP] = 0;
            }
            SETUP0();
            VISUAL_OBJECTS();
            screens.Sprite(2, SPX, SPY, 1);
            CENTER(ARMIA[A, 0, TX], ARMIA[A, 0, TY], 0);
            if (M > -1)
            {
                MESSAGE2(CO, A_S, B, M, 0);
            }
            BUSY_ANIM();
        }

        void _WAIT(int ILE)
        {
            var I = 0;
            do
            {
                I++;
                if (screens.MouseClick() > 0)
                {
                    KONIEC_INTRA++;
                    I = ILE;
                }
                screens.WaitVbl();
            } while (I != ILE);
        }

        void MA_POLOWANIE(int A)
        {
            var TEREN = ARMIA[A, 0, TNOGI];
            ARMIA[A, 0, TLEWA] = 1;
            ARMIA[A, 0, TPRAWA] = 1;
            var WOJ = ARMIA[A, 0, TE];
            if (TEREN < 70 && A < 20)
            {
                ARMIA[A, 0, TAMO] += -WOJ;
            }
            var R = amos.Rnd(13);
            var ILE = amos.Rnd(9) + 1;
            var PO_S = "";
            var RSA = 0;
            var LOS = 0;
            if (R < 5)
            {
                PO_S = "dzik";
                RSA = 13;
            }
            if (R > 4 && R < 8)
            {
                PO_S = "wilk";
                RSA = 12;
            }
            if (R == 8 || R == 9)
            {
                PO_S = "gargoil";
                RSA = 10;
                ILE = amos.Rnd(3) + 1;
            }
            if (R == 10)
            {
                PO_S = "skirial";
                RSA = 16;
                ILE = amos.Rnd(5) + 1;
            }
            if (R > 10)
            {
                PO_S = "warpun";
                RSA = 15;
            }
            if (TEREN == 1)
            {
                LOS = 1;
            }
            if (TEREN == 2)
            {
                LOS = 2;
            }
            if (TEREN == 3)
            {
                LOS = 6;
            }
            if (TEREN == 4)
            {
                LOS = 5;
            }
            if (TEREN == 5)
            {
                LOS = 4;
            }
            if (TEREN == 7)
            {
                LOS = 3;
                PO_S = "gloom";
                RSA = 14;
            }
            if (amos.Rnd(LOS) == 1)
            {
                screens.AmalOff(0);
                screens.ShowOn();
                var X = ARMIA[A, 0, TX];
                var Y = ARMIA[A, 0, TY];
                CENTER(X, Y, 1);
                MESSAGE(A, "Wytropiliśmy bestię !", 0, 0);
                ARM = A;
                WRG = 40;
                screens.SpriteOff(2);
                SETUP("", "Polowanie", "");
                for (var I = 1; I <= 10; I++)
                {
                    ARMIA[40, I, TE] = 0;
                }
                POTWOR(40, PO_S, ILE, RSA);
                RYSUJ_SCENERIE(TEREN, -1);
                X = amos.Rnd(2);
                Y = amos.Rnd(2);
                USTAW_WOJSKO(ARM, X, Y, 0);
                USTAW_WOJSKO(WRG, X, Y, 1);
                MAIN_ACTION();
                for (var I = 0; I <= 10; I++)
                {
                    ARMIA[40, I, TE] = 0;
                }
                ARMIA[A, 0, TTRYB] = 0;
                SETUP0();
                VISUAL_OBJECTS();
                screens.Sprite(2, SPX, SPY, 1);
                CENTER(ARMIA[A, 0, TX], ARMIA[A, 0, TY], 0);
                BUSY_ANIM();
            }
        }

        void MA_OBOZ(int A)
        {
            var WOJ = ARMIA[A, 0, TE];
            var TEREN = ARMIA[A, 0, TNOGI];
            ARMIA[A, 0, TLEWA] = 1;
            ARMIA[A, 0, TPRAWA] = 1;
            if (TEREN < 70 && A < 20)
            {
                ARMIA[A, 0, TAMO] += -WOJ;
            }
            for (var I = 1; I <= 10; I++)
            {
                var MAGIA = ARMIA[A, I, TMAG];
                var M_MAX = ARMIA[A, I, TMAGMA];
                amos.Add(ref MAGIA, amos.Rnd(5) + 5, MAGIA, M_MAX);
                ARMIA[A, I, TMAG] = MAGIA;
                var EN = ARMIA[A, I, TE];
                var ENM = ARMIA[A, I, TEM];
                if (EN > 0 && EN < ENM)
                {
                    amos.Add(ref EN, amos.Rnd(20) + 10, EN, ENM);
                    ARMIA[A, I, TE] = EN;
                }
            }
        }

        void MA_WYDAJ_ROZKAZ(int PL, int A)
        {
            var XA = ARMIA[A, 0, TX];
            var YA = ARMIA[A, 0, TY];
            var XB = 0;
            var YB = 0;
            var RODZAJ = 3;
            var TARGET = 0;
            var CX = 0;
            var CY = 0;
            var WIDAC = false;

            if (RODZAJ == 1 || RODZAJ == 2 || RODZAJ == 3)
            {
                var STARAODL = 120;
                WIDAC = false;
                for (var I = 0; I <= 49; I++)
                {
                    var PL2 = MIASTA[I, 0, M_CZYJE];
                    var LUDZIE = MIASTA[I, 0, M_LUDZIE];
                    var SZAJBA = 0;
                    //'wszczynaie drobnych konflikt�w
                    if (PL2 == 0 || PL2 == 1)
                    {
                        SZAJBA = 300 - POWER;
                    }
                    else
                    {
                        SZAJBA = 2200 + POWER;
                    }
                    if (PL == 5)
                    {
                        SZAJBA = 1;
                    }
                    if (amos.Rnd(SZAJBA) == 1)
                    {
                        WOJNA[PL, PL2] = amos.Rnd(20) + 8;
                        WOJNA[PL2, PL] = amos.Rnd(20) + 8;
                    }
                    if (WOJNA[PL, PL2] > 0 && PL2 != PL)
                    {
                        //'lito�ci !!! 
                        if (PL == 5 && LUDZIE < 200)
                        {
                            goto SKIP;
                        }
                        XB = MIASTA[I, 0, M_X];
                        YB = MIASTA[I, 0, M_Y];
                        ODL(XA, YA, XB, YB);
                        if (ODLEG < STARAODL)
                        {
                            TARGET = I;
                            CX = XB;
                            CY = 1;
                            STARAODL = ODLEG;
                            WIDAC = true;
                        }
                    }
                SKIP:;
                }

                for (var I = 0; I <= 39; I++)
                {
                    var PL2 = ARMIA[I, 0, TMAG];
                    if (ARMIA[I, 0, TE] > 0 && WOJNA[PL, PL2] > 0 && PL2 != PL)
                    {
                        var M = ARMIA[I, 0, TNOGI];
                        if (M >= 70 && M <= 120)
                        {
                            if (MIASTA[M - 70, 0, M_CZYJE] == PL2)
                            {
                                XB = MIASTA[M - 70, 0, M_X];
                                YB = MIASTA[M - 70, 0, M_Y];
                                ODL(XA, YA, XB, YB);
                                if (ODLEG < STARAODL)
                                {
                                    TARGET = M - 70;
                                    CX = XB;
                                    CY = 1;
                                    STARAODL = ODLEG;
                                    WIDAC = true;
                                }
                            }
                            else
                            {
                                goto DALEJ;
                            }
                        }
                        else
                        {
                            goto DALEJ;
                        }
                        continue;

                    DALEJ:
                        XB = ARMIA[I, 0, TX];
                        YB = ARMIA[I, 0, TY];
                        ODL(XA, YA, XB, YB);
                        if (ODLEG < STARAODL)
                        {
                            TARGET = I;
                            CX = XB;
                            CY = 0;
                            STARAODL = ODLEG;
                            WIDAC = true;
                        }
                    }
                }
            }

            if (RODZAJ == 3 && WIDAC)
            {
                ARMIA[A, 0, TTRYB] = 3;
                ARMIA[A, 0, TCELX] = TARGET;
                ARMIA[A, 0, TCELY] = CY;
            }
        }

        void PLAGA(int MIASTO, int PLAGA)
        {
            var M_S = MIASTA_S[MIASTO];
            var LUDZIE = MIASTA[MIASTO, 0, M_LUDZIE];
            var BB = 0;
            var A_S = "";
            if (PLAGA == 0)
            {
                BB = 32;
                A_S = "Płomienie strawiły wielu miaszkańców i ich domostwa.";
                amos.Add(ref LUDZIE, -(LUDZIE / 4), 50, LUDZIE);
                for (var I = 2; I <= 20; I++)
                {
                    if (amos.Rnd(1) == 1)
                    {
                        MIASTA[MIASTO, I, M_LUDZIE] = 0;
                    }
                }
            }
            if (PLAGA == 1)
            {
                BB = 29;
                amos.Add(ref LUDZIE, -(LUDZIE / 2), 50, LUDZIE);
                A_S = "Epidemia zarazy kosi swe śmiertelne żniwo ! ";
            }
            if (PLAGA == 2)
            {
                BB = 30;
                A_S = "Szczury pożarły cały zapas zboża w spichlerzach.";
                MIASTA[MIASTO, 1, M_LUDZIE] = 0;
            }
            MIASTA[MIASTO, 0, M_LUDZIE] = LUDZIE;
            MESSAGE2(MIASTO, A_S, BB, 1, 0);
        }








    }
}
