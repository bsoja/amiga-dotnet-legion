namespace AmigaNet.Legion
{
    public partial class Legion
    {
        void GADKA(int NR, int B)
        {
            var ODP = 0;
            var ILE = 0;
            var OSOBA = B;
            var AGRESJA = ARMIA[WRG, B, TKORP];
            var AG2 = AGRESJA / 50;
            var CODP = ARMIA[WRG, B, TPRAWA];
            if (CODP != -1)
            {
                if (AG2 >= 3)
                {
                    CODP = amos.Rnd(2);
                }
                if (AG2 == 2)
                {
                    CODP = amos.Rnd(2) + 3;
                }
                if (AG2 == 1)
                {
                    CODP = amos.Rnd(2) + 6;
                }
                if (AG2 == 0)
                {
                    if (ARMIA[WRG, B, TPRAWA] == 0)
                    {
                        if (amos.Rnd(2) == 1)
                        {
                            CODP = amos.Rnd(24) + 9;
                        }
                        else
                        {
                            CODP = 9 + amos.Rnd(2);
                        }
                        ARMIA[WRG, B, TPRAWA] = CODP;
                    }
                    else
                    {
                        CODP = ARMIA[WRG, B, TPRAWA];
                    }
                }
            }
            ARMIA[WRG, B, TGLOWA] = 1;
            var GODP = 4 - (AGRESJA / 30);
            var JP = 0;
            if (GODP < 0) GODP = 0;
            if (ARMIA[ARM, 0, TNOGI] > 69 && GODP >= 3 && CODP > -1 && amos.Rnd(7) == 1)
            {
                JP = 1;
            }
            var MIASTO = ARMIA[ARM, 0, TNOGI] - 70;
            var TAK = false;
            screens.Screen(1);
            screens.ScreenDisplay(1, 130, 142, 320, 150);
            screens.View();
            for (var I = 0; I <= 3; I++)
            {
                screens.PasteBob(0, I * 50, 1);
            }
            MSY = MSY + 183;
            var XA = ARMIA[ARM, 0, TX];
            var YA = ARMIA[ARM, 0, TY];
            var XB = ARMIA[ARM, NR, TX];
            var YB = ARMIA[ARM, NR, TY];

            var CELX = ARMIA[WRG, B, TX];
            var CELY = ARMIA[WRG, B, TY];
            var SRX = (XB + CELX) / 2;
            var SRY = (YB + CELY) / 2;
            screens.Screen(0);
            screens.BobOff(50);
            screens.BobOff(51);
            screens.BobUpdate();
            screens.Bob(50, XB, YB, 1 + BUBY);
            screens.Bob(51, CELX, CELY, 2 + BUBY);
            screens.BobUpdate();
            screens.WaitVbl();
            CENTER_V = 60;
            CENTER(SRX, SRY, 1);
            CENTER_V = 100;
            screens.Screen(1);
            for (var I = 0; I <= 2; I++)
            {
                screens.PasteBob(0, I * 50, 1);
            }
            var X = 10;
            var Y = 20;
            GADGET(X, Y - 17, 120, 15, ARMIA_S[ARM, NR], 6, 12, 9, 30, -1);
            GADGET(X + 140, Y - 17, 150, 15, RASY_S[ARMIA[WRG, B, TRASA]] + " " + ARMIA_S[WRG, B], 6, 12, 9, 30, -1);
            GADGET(X, Y, 130, 15, "Co słychać", 26, 24, 25, 30, 1);
            GADGET(X, Y + 18, 130, 15, "Przyłącz się do nas", 26, 24, 25, 30, 2);
            GADGET(X, Y + 36, 130, 15, "Oddaj mi swoje pieniądze !", 26, 24, 25, 30, 3);
            for (var I = 0; I <= 3; I++)
            {
                var TYP = PRZYGODY[I, P_TYP];
                if (TYP > 0)
                {
                    var GTEX_S = PRZYGODY_S[TYP, 2];
                    if (IM_PRZYGODY_S[I] != "")
                    {
                        var DL = GTEX_S.Length;
                        var ZN = amos.Instr(GTEX_S, "$");
                        GTEX_S = GTEX_S.Replace("$", "");
                        var L_S = amos.Left_S(GTEX_S, ZN - 1);
                        var R_S = amos.Right_S(GTEX_S, DL - ZN - 1);
                        GTEX_S = L_S + IM_PRZYGODY_S[I] + R_S;
                    }
                    if (PRZYGODY[I, P_LEVEL] > 0)
                    {
                        GADGET(X, Y + 54 + I * 18, 130, 15, GTEX_S, 26, 24, 25, 30, 4 + I);
                    }
                }
            }
            GADGET(X + 140, Y, 150, 90, "", 26, 24, 25, 30, -1);
            GADGET(X + 140, Y + 100, 150, 15, "                Exit ", 26, 24, 25, 30, 8);

            var KONIEC = false;
            do
            {
                if (screens.MouseClick() == 1)
                {
                    var STREFA = screens.MouseZone();
                    var A_S = "";
                    if (STREFA == 1)
                    {
                        if (JP == 0)
                        {
                            if (CODP == -1)
                            {
                                A_S = PRZYGODY_S[PRZYGODY[TRWA_PRZYGODA, P_TYP], 8];
                            }
                            else
                            {
                                A_S = ROZMOWA2_S[CODP];
                            }
                            NAPISZ(X + 144, Y + 15, 140, 70, A_S, TRWA_PRZYGODA, 30, 25);
                        }
                        else
                        {
                            JP = 0;
                            var TYP2 = 0;
                            if (POWER > 50)
                            {
                            LOSUJ1:
                                TYP2 = 13;
                            }
                            else
                            {
                                //TODO: check this once again:
                                //LOSUJ2:
                                TYP2 = amos.Rnd(11) + 1;
                            }
                        LOSUJ2:
                            for (var I = 0; I <= 3; I++)
                            {
                                if (PRZYGODY[I, P_TYP] == TYP2)
                                {
                                    TYP2 = amos.Rnd(11) + 1;
                                    goto LOSUJ2;
                                }
                            }
                            for (var I = 0; I <= 3; I++)
                            {
                                var TYP = PRZYGODY[I, P_TYP];
                                if (TYP == 0)
                                {
                                    NOWA_PRZYGODA(ARM, I, TYP2, amos.Rnd(3) + 1);
                                    A_S = PRZYGODY_S[TYP2, 1];
                                    NAPISZ(X + 144, Y + 15, 140, 70, A_S, I, 30, 25);
                                    var GTEX_S = PRZYGODY_S[TYP2, 2];
                                    if (IM_PRZYGODY_S[I] != "")
                                    {
                                        var DL = GTEX_S.Length;
                                        var ZN = amos.Instr(GTEX_S, "$");
                                        GTEX_S = GTEX_S.Replace("$", "");
                                        var L_S = amos.Left_S(GTEX_S, ZN - 1);
                                        var R_S = amos.Right_S(GTEX_S, DL - ZN - 1);
                                        GTEX_S = L_S + IM_PRZYGODY_S[I] + R_S;
                                    }
                                    if (PRZYGODY[I, P_LEVEL] > 0)
                                    {
                                        GADGET(X, Y + 54 + I * 18, 130, 15, GTEX_S, 26, 24, 25, 30, 4 + I);
                                    }
                                    I = 3;
                                }
                            }
                        }
                    }
                    if (STREFA == 2)
                    {
                        ODP = GODP;
                        A_S = ROZMOWA_S[1, ODP];
                        screens.Ink(30);
                        NAPISZ(X + 144, Y + 15, 140, 70, A_S, 0, 30, 25);
                        if (ODP == 3 || ODP == 4)
                        {
                            var JEST = false;
                            var L2 = 0;
                            for (var L = 1; L <= 10; L++)
                            {
                                if (ARMIA[ARM, L, TE] <= 0)
                                {
                                    JEST = true;
                                    L2 = L;
                                    L = 10;
                                }
                            }
                            if (JEST)
                            {
                                TAK = true;
                                if (ODP == 3)
                                {
                                    ILE = (ARMIA[WRG, B, TE] * 3) + (ARMIA[WRG, B, TSI] * 18) + (ARMIA[WRG, B, TSZ] * 9) + (ARMIA[WRG, B, TMAG] * 9);
                                    GADKA_BULI(X, Y, ILE, ref TAK);
                                }
                                if (TAK)
                                {
                                    screens.Screen(0);
                                    ARMIA[ARM, L2, TRASA] = ARMIA[WRG, B, TRASA];
                                    ARMIA[ARM, L2, TSI] = ARMIA[WRG, B, TSI];
                                    var MXSI = (RASY[ARMIA[ARM, L2, TRASA], 1] / 2) + 30;
                                    if (ARMIA[ARM, L2, TSI] > MXSI)
                                    {
                                        ARMIA[ARM, L2, TSI] = MXSI;
                                    }
                                    ARMIA[ARM, L2, TSZ] = ARMIA[WRG, B, TSZ];
                                    ARMIA[ARM, L2, TE] = ARMIA[WRG, B, TE];
                                    ARMIA[ARM, L2, TEM] = ARMIA[WRG, B, TEM];
                                    ARMIA[ARM, L2, TKLAT] = ARMIA[WRG, B, TKLAT];
                                    ARMIA[ARM, L2, TMAG] = ARMIA[WRG, B, TMAG];
                                    ARMIA[ARM, L2, TMAGMA] = ARMIA[WRG, B, TMAGMA];
                                    ARMIA[ARM, L2, TAMO] = ARMIA[WRG, B, TSZ];
                                    ARMIA[ARM, L2, TDOSW] = ARMIA[WRG, B, TDOSW];
                                    ARMIA[ARM, L2, TP] = 0;
                                    for (var I = TGLOWA; I <= TPLECAK + 7; I++)
                                    {
                                        ARMIA[ARM, L2, I] = 0;
                                    }
                                    ARMIA_S[ARM, L2] = ARMIA_S[WRG, B];
                                    ARMIA[WRG, B, TE] = 0;
                                    screens.BobOff(10 + B);
                                    screens.ResetZone(10 + B);
                                    screens.BobUpdate();
                                    screens.WaitVbl();
                                    ARMIA[WRG, B, TTRYB] = 0;
                                    ARMIA[ARM, L2, TX] = ARMIA[WRG, B, TX];
                                    ARMIA[ARM, L2, TY] = ARMIA[WRG, B, TY];
                                    var BAZA = RASY[ARMIA[ARM, L2, TRASA], 7];
                                    X = ARMIA[ARM, L2, TX];
                                    Y = ARMIA[ARM, L2, TY];
                                    ARMIA[ARM, L2, TBOB] = BAZA;
                                    screens.Bob(L2, X, Y, BAZA + 1);
                                    screens.BobUpdate();
                                    screens.WaitVbl();
                                    screens.SetZone(L2, X - 16, Y - 20, X + 16, Y);
                                    screens.Screen(1);
                                    while (screens.MouseClick() == 1) { }
                                    KONIEC = true;
                                }
                            }
                            else
                            {
                                A_S = "Niestety ale w twoim oddziale nie ma już miejsca.";
                                NAPISZ(X + 144, Y + 15, 140, 70, A_S, 0, 30, 25);
                            }

                        }
                    }
                    if (STREFA == 3)
                    {
                        ODP = GODP;
                        var GUL = 0;
                        if (ARMIA[WRG, B, TRASA] == 9)
                        {
                            GUL = 30;
                        }
                        else
                        {
                            GUL = 30 + amos.Rnd(50);
                        }
                        amos.Add(ref AGRESJA, GUL, AGRESJA, 190);
                        ARMIA[WRG, B, TKORP] = AGRESJA;
                        GODP = 4 - (AGRESJA / 40);
                        A_S = ROZMOWA_S[2, ODP];
                        screens.Ink(30);
                        NAPISZ(X + 144, Y + 15, 140, 70, A_S, 0, 30, 25);
                        if (ODP == 4)
                        {
                            var F = amos.Rnd(100);
                            GRACZE[1, 1] += F;
                            screens.Text(X + 164, Y + 25, amos.Str_S(F));
                        }
                    }
                    if (STREFA > 3 && STREFA < 8)
                    {
                        TAK = false;
                        NR = STREFA - 4;
                        var PX = PRZYGODY[NR, P_X];
                        var PY = PRZYGODY[NR, P_Y];
                        var LEVEL = PRZYGODY[NR, P_LEVEL];
                        var TYP = PRZYGODY[NR, P_TYP];
                        var CENA = PRZYGODY[NR, P_CENA];
                        if (AGRESJA < 50)
                        {
                            if (PX == MIASTO)
                            {
                                if (OSOBA == PY)
                                {
                                    if (CENA > 0 && amos.Rnd(2) != 0)
                                    {
                                        ILE = CENA;
                                        GADKA_BULI(X, Y, ILE, ref TAK);
                                    }
                                    else
                                    {
                                        TAK = true;
                                    }
                                    if (TAK)
                                    {
                                        if (LEVEL > 2)
                                        {
                                            ODP = 1;
                                        }
                                        if (LEVEL == 2)
                                        {
                                            ODP = 2;
                                        }
                                        if (LEVEL == 1)
                                        {
                                            ODP = 3;
                                        }
                                        PRZYGODY_(XA, YA, NR);
                                        PRZYGODY[NR, P_STAREX] = MIASTO;
                                        if (PRZYGODY[NR, P_LEVEL] == 0)
                                        {
                                            GADGET(X, Y + 54 + NR * 18, 130, 15, "", 26, 24, 25, 30, -1);
                                            screens.ResetZone(4 + NR);
                                        }
                                    }
                                    else
                                    {
                                        ODP = 0;
                                    }
                                    A_S = PRZYGODY_S[TYP, 3 + ODP];
                                }
                                else
                                {
                                    if (amos.Rnd(1) == 1)
                                    {
                                        var B_S = "";
                                        if (ARMIA[40, PY, TRASA] == 4)
                                        {
                                            B_S = " powinna";
                                        }
                                        else
                                        {
                                            B_S = " powinien";
                                        }
                                        A_S = RASY_S[ARMIA[40, PY, TRASA]] + " " + ARMIA_S[40, PY] + B_S + " coś wiedzieć.";
                                    }
                                    else
                                    {
                                        A_S = PRZYGODY_S[TYP, 3];
                                    }
                                }
                            }
                            else
                            {
                                ODP = 0;
                                if (OSOBA > 6 && LEVEL > 1 && PRZYGODY[NR, P_STAREX] != MIASTO)
                                {
                                    if (CENA > 0 && amos.Rnd(2) != 0)
                                    {
                                        ILE = CENA;
                                        GADKA_BULI(X, Y, ILE, ref TAK);
                                    }
                                    else
                                    {
                                        TAK = true;
                                    }
                                    if (TAK)
                                    {
                                        ODP = 2;
                                    }
                                    else
                                    {
                                        ODP = 0;
                                    }
                                }
                                A_S = PRZYGODY_S[TYP, 3 + ODP];
                            }

                        }
                        else
                        {
                            A_S = "Daj mi spokój.";
                        }
                        NAPISZ(X + 144, Y + 15, 140, 70, A_S, NR, 30, 25);
                    }
                    if (STREFA == 8)
                    {
                        KONIEC = true;
                    }
                }
            }
            while (!KONIEC);
            //OVER:
            MSY = 278;
            screens.Screen(0);
            CENTER(XB, YB, 0);
            screens.BobOff(51);
            screens.BobUpdate();
            screens.WaitVbl();
            screens.Screen(1);
            screens.ResetZone();
            screens.ScreenDisplay(1, 130, 275, 320, 25);
            screens.View();
            EKRAN1();
            screens.Screen(0);
        }

        void GADKA_BULI(int X, int Y, int ILE, ref bool TAK)
        {
            var KONIEC2 = false;
            screens.Ink(25);
            screens.Bar(X + 141, Y + 1, X + 140 + 148, Y + 82);
            screens.Ink(30);
            screens.Text(X + 144, Y + 15, "to będzie cię kosztowało ");
            screens.Text(X + 144, Y + 25, ILE + " sztuk złota");
            screens.Text(X + 144, Y + 35, "Płacisz ?");
            GADGET(X + 144, Y + 60, 25, 15, "Tak", 7, 9, 11, 0, 10);
            GADGET(X + 260, Y + 60, 25, 15, "Nie", 7, 9, 11, 0, 11);
            do
            {
                if (screens.MouseClick() == 1)
                {
                    var STREFA = screens.MouseZone();
                    if (STREFA == 10 && GRACZE[1, 1] - ILE >= 0)
                    {
                        GRACZE[1, 1] += -ILE;
                        TAK = true;
                        KONIEC2 = true;
                    }
                    if (STREFA == 11)
                    {
                        TAK = false;
                        KONIEC2 = true;
                    }
                }
            }
            while (!KONIEC2);
            screens.ResetZone(10);
            screens.ResetZone(11);
            screens.Ink(25);
            screens.Bar(X + 141, Y + 1, X + 140 + 148, Y + 82);
        }

        void NAPISZ(int X, int Y, int SZER, int WYS, String A_S, int P, int K1, int K2)
        {
            screens.Ink(K2);
            screens.Bar(X - 2, Y - 10, X + SZER, Y + WYS);
            screens.Ink(K1, K2);
            var DL = A_S.Length;
            var ZDANIE_S = "";
            var WYRA_S = "";
            for (var I = 1; I <= DL; I++)
            {
                var Z_S = amos.Mid_S(A_S, I, 1);
                if (Z_S == "@")
                {
                    if (PRZYGODY[P, P_KIERUNEK] == 0)
                    {
                        Z_S = "północy";
                    }
                    if (PRZYGODY[P, P_KIERUNEK] == 1)
                    {
                        Z_S = "południu";
                    }
                    if (PRZYGODY[P, P_KIERUNEK] == 2)
                    {
                        Z_S = "wschodzie";
                    }
                    if (PRZYGODY[P, P_KIERUNEK] == 3)
                    {
                        Z_S = "zachodzie";
                    }
                }
                if (Z_S == "#")
                {
                    if (PRZYGODY[P, P_KIERUNEK] == 0)
                    {
                        Z_S = "północ";
                    }
                    if (PRZYGODY[P, P_KIERUNEK] == 1)
                    {
                        Z_S = "południe";
                    }
                    if (PRZYGODY[P, P_KIERUNEK] == 2)
                    {
                        Z_S = "wschód";
                    }
                    if (PRZYGODY[P, P_KIERUNEK] == 3)
                    {
                        Z_S = "zachód";
                    }
                }

                if (Z_S == "*")
                {
                    Z_S = ROB_IMIE();
                }
                if (Z_S == "�")
                {
                    Z_S = MIASTA_S[PRZYGODY[P, P_X]];
                }
                if (Z_S == "&")
                {
                    Z_S = MIASTA_S[PRZYGODY[P, P_NAGRODA]];
                }
                if (Z_S == "$")
                {
                    Z_S = IM_PRZYGODY_S[P];
                }
                if (Z_S == "%")
                {
                    var BRO = PRZYGODY[P, P_BRON];
                    Z_S = BRON2_S[BRON[BRO, B_TYP]] + " " + BRON_S[BRO];
                }
                if (Z_S == "�")
                {
                    Z_S = RASY_S[PRZYGODY[P, P_BRON]];
                }
                WYRA_S = WYRA_S + Z_S;

                if (Z_S == " " || Z_S == ".")
                {
                    var DLUG = screens.TextLength(ZDANIE_S + WYRA_S);

                    if (DLUG < SZER)
                    {
                        ZDANIE_S = ZDANIE_S + WYRA_S;
                        WYRA_S = "";
                    }
                    else
                    {
                        screens.Text(X, Y, ZDANIE_S);
                        Y += 10;
                        ZDANIE_S = WYRA_S;
                        WYRA_S = "";
                    }
                }
                if (I == DL)
                {
                    screens.Text(X, Y, ZDANIE_S + WYRA_S);
                }
            }
        }

        void PRZYGODY_(int XA, int YA, int NR)
        {
            var MIASTO = 0;
            var KIER = 0;
            var DX = 0;
            var DY = 0;

            var LEVEL = PRZYGODY[NR, P_LEVEL];
            LEVEL--;
            PRZYGODY[NR, P_LEVEL] = LEVEL;

            if (LEVEL > 0)
            {
                //'wska� konkretn� osob� 
                //'miasto wybrane zgodnie z kierunkiem przygody
                var STARAODL = 600;
                var OLD_KIER = PRZYGODY[NR, P_KIERUNEK];
                for (var I = 0; I <= 49; I++)
                {
                    var X2 = MIASTA[I, 0, M_X];
                    var Y2 = MIASTA[I, 0, M_Y];
                    DX = XA - X2;
                    DY = YA - Y2;
                    if (Math.Abs(DX) >= Math.Abs(DY))
                    {
                        if (DX >= 0)
                        {
                            KIER = 2;
                        }
                        else
                        {
                            KIER = 3;
                        }
                    }
                    else
                    {
                        if (DY >= 0)
                        {
                            KIER = 1;
                        }
                        else
                        {
                            KIER = 0;
                        }
                    }
                    ODL(XA, YA, X2, Y2);
                    if (ODLEG < STARAODL && KIER != OLD_KIER && ODLEG > 30 + amos.Rnd(100))
                    {
                        STARAODL = ODLEG;
                        MIASTO = I;
                    }
                }
                PRZYGODY[NR, P_X] = MIASTO;
                //'osoba 
                PRZYGODY[NR, P_Y] = amos.Rnd(9) + 1;
                DX = XA - MIASTA[MIASTO, 0, M_X];
                DY = YA - MIASTA[MIASTO, 0, M_Y];
                if (Math.Abs(DX) >= Math.Abs(DY))
                {
                    if (DX >= 0)
                    {
                        KIER = 3;
                    }
                    else
                    {
                        KIER = 2;
                    }
                }
                else
                {
                    if (DY >= 0)
                    {
                        KIER = 0;
                    }
                    else
                    {
                        KIER = 1;
                    }
                }
                PRZYGODY[NR, P_KIERUNEK] = KIER;
            }
            if (LEVEL == 0)
            {
                var X = XA + amos.Rnd(100) - 50;
                var Y = YA + amos.Rnd(100) - 50;
                PRZYGODY[NR, P_X] = X;
                PRZYGODY[NR, P_Y] = Y;
                //'wska� miejsce na mapie
                //'strefa o numerze przygody 
            }
        }

        void NOWA_PRZYGODA(int A, int NR, int TYP, int LEVEL)
        {
            PRZYGODY[NR, P_TYP] = TYP;
            PRZYGODY[NR, P_X] = ARMIA[A, 0, TNOGI] - 70;
            PRZYGODY[NR, P_Y] = amos.Rnd(9) + 1;
            PRZYGODY[NR, P_KIERUNEK] = -1;
            PRZYGODY[NR, P_LEVEL] = LEVEL;
            IM_PRZYGODY_S[NR] = "";

            var MIASTO = 0;
            do
            {
                MIASTO = amos.Rnd(49);
            }
            while (!(MIASTA[MIASTO, 0, M_CZYJE] != 1));

            if (TYP == 1)
            {
                //'kopalnia
                PRZYGODY[NR, P_TERMIN] = 100 + amos.Rnd(100);
                PRZYGODY[NR, P_CENA] = 20 * LEVEL;
                PRZYGODY[NR, P_NAGRODA] = LEVEL * 10000;
                PRZYGODY[NR, P_TEREN] = 8;
                PRZYGODY[NR, P_BRON] = 0;
            }
            if (TYP == 2)
            {
                //'kurhan
                PRZYGODY[NR, P_TERMIN] = 100 + amos.Rnd(100);
                PRZYGODY[NR, P_CENA] = amos.Rnd(20 * LEVEL);
                PRZYGODY[NR, P_NAGRODA] = LEVEL * 100;
                var BRON_ = 0;
                var BTYP = 0;
                do
                {
                    BRON_ = amos.Rnd(MX_WEAPON);
                    BTYP = BRON[BRON_, B_TYP];
                }
                while (!(BRON[BRON_, B_CENA] >= 1000 && BRON[BRON_, B_CENA] < 100 + LEVEL * 1000 && BTYP != 5 && BTYP != 8 && BTYP != 13 && BTYP != 14 && BTYP < 16));
                PRZYGODY[NR, P_BRON] = BRON_;
                PRZYGODY[NR, P_TEREN] = 9;
                IM_PRZYGODY_S[NR] = ROB_IMIE();
            }
            if (TYP == 3)
            {
                //'bandyci 
                PRZYGODY[NR, P_TERMIN] = 100 + amos.Rnd(100);
                PRZYGODY[NR, P_CENA] = 0;
                PRZYGODY[NR, P_NAGRODA] = 4000 + amos.Rnd(2000) + LEVEL * 100;
                PRZYGODY[NR, P_TEREN] = 0;
                PRZYGODY[NR, P_BRON] = 0;
            }
            if (TYP == 4)
            {
                //'córa
                PRZYGODY[NR, P_TERMIN] = 100 + amos.Rnd(100);
                PRZYGODY[NR, P_CENA] = 0;
                PRZYGODY[NR, P_NAGRODA] = MIASTO;
                PRZYGODY[NR, P_BRON] = 0;
                PRZYGODY[NR, P_TEREN] = 0;
                IM_PRZYGODY_S[NR] = ROB_IMIE();
            }
            if (TYP == 5)
            {
                //'g�ra jaka� tam
                PRZYGODY[NR, P_TERMIN] = 100 + amos.Rnd(100);
                PRZYGODY[NR, P_CENA] = amos.Rnd(100) + 30;
                //'jedno z miast amos.Rnd(m_czyje)
                PRZYGODY[NR, P_NAGRODA] = MIASTO;
                PRZYGODY[NR, P_TEREN] = 4;
                PRZYGODY[NR, P_BRON] = 0;
                IM_PRZYGODY_S[NR] = ROB_IMIE();
            }
            if (TYP == 6)
            {
                //'super mag 
                PRZYGODY[NR, P_TERMIN] = 100 + amos.Rnd(100);
                PRZYGODY[NR, P_CENA] = amos.Rnd(100) + 10;
                //'jedno z miast amos.Rnd(m_czyje)
                PRZYGODY[NR, P_NAGRODA] = MIASTO;
                PRZYGODY[NR, P_TEREN] = 0;
                PRZYGODY[NR, P_BRON] = 0;
                IM_PRZYGODY_S[NR] = ROB_IMIE();
            }
            if (TYP == 7)
            {
                //'grota paladyna ufola
                PRZYGODY[NR, P_TERMIN] = 100 + amos.Rnd(100);
                PRZYGODY[NR, P_CENA] = amos.Rnd(100) + 10;
                //'jedno z miast amos.Rnd(m_czyje)
                PRZYGODY[NR, P_NAGRODA] = MIASTO;
                PRZYGODY[NR, P_TEREN] = 8;
                PRZYGODY[NR, P_BRON] = 0;
                IM_PRZYGODY_S[NR] = ROB_IMIE();
            }
            if (TYP == 8)
            {
                //'magiczna ksi�ga 
                PRZYGODY[NR, P_LEVEL] = 3;
                PRZYGODY[NR, P_TERMIN] = 100 + amos.Rnd(100);
                PRZYGODY[NR, P_CENA] = 100;
                PRZYGODY[NR, P_TEREN] = 9;
                if (amos.Rnd(1) == 0)
                {
                    //'GB
                    PRZYGODY[NR, P_BRON] = 52;
                }
                else
                {
                    //'NAW 
                    PRZYGODY[NR, P_BRON] = 88;
                }
            }
            if (TYP == 9)
            {
                //'�wi�tynia ork�w 
                PRZYGODY[NR, P_TERMIN] = 100 + amos.Rnd(100);
                PRZYGODY[NR, P_CENA] = amos.Rnd(20);
                PRZYGODY[NR, P_TEREN] = 9;
                PRZYGODY[NR, P_BRON] = 0;
            }
            if (TYP == 10)
            {
                //'barbray�ca na bagnach 
                PRZYGODY[NR, P_TERMIN] = 100 + amos.Rnd(100);
                PRZYGODY[NR, P_CENA] = amos.Rnd(100) + 30;
                PRZYGODY[NR, P_TEREN] = 7;
                PRZYGODY[NR, P_BRON] = 0;
                IM_PRZYGODY_S[NR] = ROB_IMIE();
            }
            if (TYP == 11)
            {
                //'wataha
                PRZYGODY[NR, P_TERMIN] = 100 + amos.Rnd(100);
                PRZYGODY[NR, P_CENA] = 0;
                PRZYGODY[NR, P_NAGRODA] = 3000 * LEVEL;
                PRZYGODY[NR, P_TEREN] = 0;
            AGAIN:
                var RSA = amos.Rnd(8);
                if (RSA == 4)
                {
                    //' bez amazonek 
                    goto AGAIN;
                }
                PRZYGODY[NR, P_BRON] = RSA;
                IM_PRZYGODY_S[NR] = ROB_IMIE();
            }
            if (TYP == 12)
            {
                //'jaskinia wiedzy 
                PRZYGODY[NR, P_TERMIN] = 100 + amos.Rnd(100);
                PRZYGODY[NR, P_CENA] = amos.Rnd(100) + 20;
                PRZYGODY[NR, P_TEREN] = 8;
            }
            if (TYP == 13)
            {
                //'w�adca chaosu 
                PRZYGODY[NR, P_LEVEL] = 4;
                PRZYGODY[NR, P_TERMIN] = 100 + amos.Rnd(100);
                PRZYGODY[NR, P_CENA] = 500 + amos.Rnd(100);
                //'jedno z miast amos.Rnd(m_czyje)
                PRZYGODY[NR, P_NAGRODA] = MIASTO;
                PRZYGODY[NR, P_TEREN] = 10;
                PRZYGODY[NR, P_BRON] = 0;
            }
        }
    }
}
