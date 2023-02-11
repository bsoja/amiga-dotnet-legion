namespace AmigaNet.Legion
{
    public partial class Legion
    {
        void WYBOR(int WT)
        {
            screens.Screen(1);
            screens.ScreenHide();
            screens.View();
            screens.ScreenDisplay(1, 130, 162, 320, 140);
            for (var I = 0; I <= 3; I++)
            {
                screens.PasteBob(0, I * 50, GOBY + 1);
            }
            //Set Rainbow 1,15,512,"","","(8,1,1)"
            //Rainbow 1,1,165,160
            screens.Colour(0, 2, 1, 0);
            var X = 120;
            var Y = 5;
            var X2 = 120;
            var Y2 = 100;
            var SEK = 0;
            var RASA = 0;
            var BB = 0;

            if (WT == 0)
            {
                MSY = MSY + 113;
            }
            var XB = ARMIA[ARM, NUMER, TX];
            var YB = ARMIA[ARM, NUMER, TY];

            GADGET(X, Y, 105, 55, "", 5, 0, 8, 8, -1);
            GADGET(X2, Y2, 105, 30, "", 5, 0, 8, 8, -1);

            GADGET(X + 5, 70, 95, 20, "", 0, 5, 19, 19, -1);
            GADGET(235, Y, 75, 100, "", 0, 5, 19, 19, -1);
            GADGET(235, Y2 + 15, 30, 15, "   <", 5, 0, 8, 1, 21);
            GADGET(280, Y2 + 15, 30, 15, "    >", 5, 0, 8, 1, 22);
            //'--------------strefy do�wiadczenia
            for (var I = 0; I <= 4; I++)
            {
                screens.SetZone(25 + I, 237, 28 + I * 10, 277, 37 + I * 10);
            }

            //custom change, from color 15 to 19, is not look good, maybe because there is no rainbow
            //GADGET(5, 5, 105, 125, "", 0, 5, 15, 15, -1);
            GADGET(5, 5, 105, 125, "", 0, 5, 19, 19, -1);

            screens.PasteBob(19, 10, GOBY + 38);
            screens.GetSprite(GOBY + 42, 47, 8, 47 + 15, 28);
            screens.WaitVbl();
            var GLOB_S = "";
            if (WT == 1)
            {
                GLOB_S = "bob86";
            }
            else
            {
                GLOB_S = "bob42";
            }
            screens.SetZone(20, 235, 5, 310, 18);
            WYBOR_RYSUJ(WT, NUMER, GLOB_S, X, Y, X2, Y2, ref SEK);
            WAGA(ARM, NUMER);
            WYBOR_WYPISZ(Y, NUMER);
            screens.ScreenShow();
            screens.View();
            while (true)
            {
                if (screens.MouseClick() == 1)
                {
                    var BR = 0;
                    var STREFA = screens.MouseZone();
                    if (STREFA > 0 && STREFA < 5)
                    {
                        BR = ARMIA[ARM, NUMER, TPLECAK + STREFA - 1];
                        if (BR > 0)
                        {
                            GADGET(5 + X + ((STREFA - 1) * 25), Y + 5, 20, 20, "", 0, 5, 0, 16, 0);
                            ARMIA[ARM, NUMER, TPLECAK + STREFA - 1] = 0;
                            WYBOR_PICK(BR, X, Y, X2, Y2, NUMER, ref BB, SEK);
                        }
                    }
                    if (STREFA > 4 && STREFA < 9)
                    {
                        BR = ARMIA[ARM, NUMER, TPLECAK + STREFA - 1];
                        if (BR > 0)
                        {
                            GADGET(5 + X + ((STREFA - 5) * 25), Y + 30, 20, 20, "", 0, 5, 0, 16, 0);
                            ARMIA[ARM, NUMER, TPLECAK + STREFA - 1] = 0;
                            WYBOR_PICK(BR, X, Y, X2, Y2, NUMER, ref BB, SEK);
                        }
                    }
                    if (STREFA > 8 && STREFA < 13)
                    {
                        BR = GLEBA[SEK, STREFA - 9];
                        if (BR > 0)
                        {
                            GADGET(5 + X2 + ((STREFA - 9) * 25), Y2 + 5, 20, 20, "", 0, 5, 0, 16, 0);
                            GLEBA[SEK, STREFA - 9] = 0;
                            WYBOR_PICK(BR, X, Y, X2, Y2, NUMER, ref BB, SEK);
                        }
                    }
                    if (STREFA == 13)
                    {
                        BR = ARMIA[ARM, NUMER, TGLOWA];
                        if (BR > 0)
                        {
                            //custom change, from color 15 to 19, is not look good, maybe because there is no rainbow
                            //GADGET(47, 8, 20, 20, GLOB_S, 5, 5, 15, 15, 0);
                            GADGET(47, 8, 20, 20, GLOB_S, 5, 5, 19, 19, 0);

                            screens.Ink(5);
                            screens.Box(47, 8, 67, 28);
                            PRZELICZ(STREFA - 13, -1);
                            WYBOR_WYPISZ(Y, NUMER);
                            ARMIA[ARM, NUMER, TGLOWA + STREFA - 13] = 0;
                            WYBOR_PICK(BR, X, Y, X2, Y2, NUMER, ref BB, SEK);
                        }
                    }
                    if (STREFA == 14)
                    {
                        BR = ARMIA[ARM, NUMER, TGLOWA + STREFA - 13];
                        if (BR > 0)
                        {
                            GADGET(47, 44, 20, 20, "", 5, 5, 0, 16, 0);
                            PRZELICZ(STREFA - 13, -1);
                            WYBOR_WYPISZ(Y, NUMER);
                            ARMIA[ARM, NUMER, TGLOWA + STREFA - 13] = 0;
                            WYBOR_PICK(BR, X, Y, X2, Y2, NUMER, ref BB, SEK);
                        }
                    }
                    if (STREFA == 15)
                    {
                        BR = ARMIA[ARM, NUMER, TGLOWA + STREFA - 13];
                        if (BR > 0)
                        {
                            GADGET(47, 8 + ((STREFA - 13) * 50), 20, 20, "", 5, 5, 0, 16, 0);
                            PRZELICZ(STREFA - 13, -1);
                            WYBOR_WYPISZ(Y, NUMER);
                            ARMIA[ARM, NUMER, TGLOWA + STREFA - 13] = 0;
                            WYBOR_PICK(BR, X, Y, X2, Y2, NUMER, ref BB, SEK);
                        }
                    }

                    if (STREFA == 16)
                    {
                        BR = ARMIA[ARM, NUMER, TLEWA];
                        if (BR > 0)
                        {
                            GADGET(7, 58, 20, 20, "", 5, 5, 0, 16, 16);
                            PRZELICZ(3, -1);
                            WYBOR_WYPISZ(Y, NUMER);
                            ARMIA[ARM, NUMER, TLEWA] = 0;
                            WYBOR_PICK(BR, X, Y, X2, Y2, NUMER, ref BB, SEK);
                        }
                    }
                    if (STREFA == 17)
                    {
                        BR = ARMIA[ARM, NUMER, TPRAWA];
                        if (BR > 0)
                        {
                            GADGET(87, 58, 20, 20, "", 5, 5, 0, 16, 17);
                            PRZELICZ(4, -1);
                            WYBOR_WYPISZ(Y, NUMER);
                            ARMIA[ARM, NUMER, TPRAWA] = 0;
                            WYBOR_PICK(BR, X, Y, X2, Y2, NUMER, ref BB, SEK);
                        }
                    }
                    if (STREFA == 20)
                    {
                        WPISZ(237, 15, 3, 19, 11);
                        ARMIA_S[ARM, NUMER] = WPI_S;
                    }
                    if (STREFA == 25 &&
                        ARMIA[ARM, NUMER, TDOSW] >= 1 &&
                        ARMIA[ARM, NUMER, TEM] < ((20 + RASY[RASA, 0]) * 3) + 20)
                    {
                        ARMIA[ARM, NUMER, TEM]++;
                        ARMIA[ARM, NUMER, TDOSW]--;
                        screens.Ink(19);
                        screens.Bar(295, 88, 306, 95);
                        screens.Ink(16, 19);
                        screens.Text(270, 35, ARMIA[ARM, NUMER, TE] + "/" + ARMIA[ARM, NUMER, TEM]);// - " " + " "
                        screens.Text(295, 95, amos.Str_S(ARMIA[ARM, NUMER, TDOSW]));
                    }
                    if (STREFA == 26 &&
                        ARMIA[ARM, NUMER, TDOSW] >= 3 &&
                        ARMIA[ARM, NUMER, TSI] < (RASY[RASA, 1] / 2) + 40)
                    {
                        ARMIA[ARM, NUMER, TSI]++;
                        ARMIA[ARM, NUMER, TDOSW] += -3;
                        screens.Ink(19);
                        screens.Bar(295, 88, 306, 95);
                        screens.Ink(16, 19);
                        screens.Text(290, 45, ARMIA[ARM, NUMER, TSI] + " ");
                        screens.Text(295, 95, amos.Str_S(ARMIA[ARM, NUMER, TDOSW]));
                    }
                    if (STREFA == 27 &&
                        ARMIA[ARM, NUMER, TDOSW] >= 4 &&
                        ARMIA[ARM, NUMER, TSZ] < RASY[RASA, 2] + 30)
                    {
                        ARMIA[ARM, NUMER, TSZ]++;
                        ARMIA[ARM, NUMER, TDOSW] += -3;
                        screens.Ink(19);
                        screens.Bar(295, 88, 306, 95);
                        screens.Ink(16, 19);
                        screens.Text(290, 55, ARMIA[ARM, NUMER, TSZ] + " ");
                        screens.Text(295, 95, amos.Str_S(ARMIA[ARM, NUMER, TDOSW]));
                    }
                    if (STREFA == 28 && ARMIA[ARM, NUMER, TDOSW] >= 3)
                    {
                        ARMIA[ARM, NUMER, TP]++;
                        ARMIA[ARM, NUMER, TDOSW] += -3;
                        screens.Ink(19);
                        screens.Bar(295, 88, 306, 95);
                        screens.Ink(16, 19);
                        screens.Text(290, 65, ARMIA[ARM, NUMER, TP] + " ");
                        screens.Text(295, 95, amos.Str_S(ARMIA[ARM, NUMER, TDOSW]));
                    }
                    if (STREFA == 29 &&
                        ARMIA[ARM, NUMER, TDOSW] >= 2 &&
                        ARMIA[ARM, NUMER, TMAGMA] < RASY[RASA, 3] + 30)
                    {
                        ARMIA[ARM, NUMER, TMAGMA]++;
                        ARMIA[ARM, NUMER, TDOSW] += -2;
                        screens.Ink(19);
                        screens.Bar(295, 88, 306, 95);
                        screens.Ink(16, 19);
                        screens.Text(270, 75, amos.Str_S(ARMIA[ARM, NUMER, TMAG]) + "/" + ARMIA[ARM, NUMER, TMAGMA]);// - " " + " "
                        screens.Text(295, 95, amos.Str_S(ARMIA[ARM, NUMER, TDOSW]));
                    }
                }

                if (screens.MouseKey() == LEWY)
                {
                    var STREFA = screens.MouseZone();
                    if (STREFA == 21)
                    {
                        GADGET(235, Y2 + 15, 30, 15, "   <", 0, 5, 11, 2, 0);
                        GADGET(235, Y2 + 15, 30, 15, "   <", 5, 0, 8, 1, -1);
                        do
                        {
                            amos.Add(ref NUMER, -1, 1, 10);
                        }
                        while (ARMIA[ARM, NUMER, TE] <= 0);
                        if (WT == 0)
                        {
                            screens.Screen(0);
                            XB = ARMIA[ARM, NUMER, TX];
                            YB = ARMIA[ARM, NUMER, TY];
                            CENTER(XB, YB, 2);
                            screens.Bob(50, XB, YB, 1 + BUBY);
                            screens.BobUpdate();
                            screens.Screen(1);
                        }
                        RASA = ARMIA[ARM, NUMER, TRASA];
                        WAGA(ARM, NUMER);
                        screens.WaitVbl();
                        WYBOR_RYSUJ(WT, NUMER, GLOB_S, X, Y, X2, Y2, ref SEK);
                        WYBOR_WYPISZ(Y, NUMER);
                    }
                    if (STREFA == 22)
                    {
                        GADGET(280, Y2 + 15, 30, 15, "    >", 0, 5, 11, 2, 0);
                        GADGET(280, Y2 + 15, 30, 15, "    >", 5, 0, 8, 1, -1);
                        do
                        {
                            amos.Add(ref NUMER, 1, 1, 10);
                        }
                        while (ARMIA[ARM, NUMER, TE] <= 0);
                        if (WT == 0)
                        {
                            screens.Screen(0);
                            XB = ARMIA[ARM, NUMER, TX];
                            YB = ARMIA[ARM, NUMER, TY];
                            CENTER(XB, YB, 2);
                            screens.Bob(50, XB, YB, 1 + BUBY);
                            screens.BobUpdate();
                            screens.Screen(1);
                        }
                        RASA = ARMIA[ARM, NUMER, TRASA];
                        WAGA(ARM, NUMER);
                        screens.WaitVbl();
                        WYBOR_RYSUJ(WT, NUMER, GLOB_S, X, Y, X2, Y2, ref SEK);
                        WYBOR_WYPISZ(Y, NUMER);
                    }
                }

                if (screens.MouseKey() == PRAWY) break;
            }

            //Rainbow Del 1
            if (WT == 0)
            {
                MSY = 278;
                screens.Screen(0);
                CENTER(XB, YB, 0);
                screens.Screen(1);
                screens.ScreenDisplay(1, 130, 275, 320, 25);
                screens.View();
                screens.Colour(0, 0, 0, 0);
                EKRAN1();
                SELECT(ARM, NUMER);
            }
        }

        void WYBOR_RYSUJ(int WT, int NUMER, String GLOB_S, int X, int Y, int X2, int Y2, ref int SEK)
        {
            if (WT == 0)
            {
                SEK = SEKTOR(ARMIA[ARM, NUMER, TX], ARMIA[ARM, NUMER, TY]);
            }
            else
            {
                SEK = 0;
            }

            //custom change, from color 15 to 19, is not look good, maybe because there is no rainbow
            //GADGET(47, 8, 20, 20, GLOB_S, 5, 5, 15, 15, 13);
            GADGET(47, 8, 20, 20, GLOB_S, 5, 5, 19, 19, 13);

            screens.Ink(5);
            screens.Box(47, 8, 67, 28);
            var B = ARMIA[ARM, NUMER, TGLOWA];
            if (B > 0)
            {
                screens.PasteBob(49, 10, BRON[B, B_BOB] + BROBY + GOBY);
            }
            GADGET(47, 44, 20, 20, "", 5, 5, 0, 16, 14);
            B = ARMIA[ARM, NUMER, TKORP];
            if (B > 0)
            {
                screens.PasteBob(49, 46, BRON[B, B_BOB] + BROBY + GOBY);
            }
            GADGET(47, 108, 20, 20, "", 5, 5, 0, 16, 15);
            B = ARMIA[ARM, NUMER, TNOGI];
            if (B > 0)
            {
                screens.PasteBob(49, 110, BRON[B, B_BOB] + BROBY + GOBY);
            }
            GADGET(7, 58, 20, 20, "", 5, 5, 0, 16, 16);
            B = ARMIA[ARM, NUMER, TLEWA];
            if (B > 0)
            {
                screens.PasteBob(9, 60, BRON[B, B_BOB] + BROBY + GOBY);
            }
            GADGET(87, 58, 20, 20, "", 5, 5, 0, 16, 17);
            B = ARMIA[ARM, NUMER, TPRAWA];
            if (B > 0)
            {
                screens.PasteBob(89, 60, BRON[B, B_BOB] + BROBY + GOBY);
            }
            for (var I = 0; I <= 3; I++)
            {
                GADGET(5 + X + (I * 25), Y + 5, 20, 20, "", 0, 5, 0, 16, 1 + I);
                GADGET(5 + X + (I * 25), Y + 30, 20, 20, "", 0, 5, 0, 16, 5 + I);
                GADGET(5 + X2 + (I * 25), Y2 + 5, 20, 20, "", 0, 5, 0, 16, 9 + I);
                var B1 = ARMIA[ARM, NUMER, TPLECAK + I];
                if (B1 > 0)
                {
                    var BB1 = BRON[B1, B_BOB] + BROBY;
                    screens.PasteBob(X + 7 + (I * 25), Y + 7, BB1 + GOBY);
                }
                var B2 = ARMIA[ARM, NUMER, TPLECAK + I + 4];
                if (B2 > 0)
                {
                    var BB2 = BRON[B2, B_BOB] + BROBY;
                    screens.PasteBob(X + 7 + (I * 25), Y + 32, BB2 + GOBY);
                }
                var B3 = GLEBA[SEK, I];
                if (B3 > 0)
                {
                    var BB3 = BRON[B3, B_BOB] + BROBY;
                    screens.PasteBob(X2 + 7 + (I * 25), Y2 + 7, BB3 + GOBY);
                }
            }
        }

        void WYBOR_WYPISZ(int Y, int NUMER)
        {
            screens.GrWriting(1);
            screens.Ink(19, 19);
            screens.Bar(236, Y + 1, 236 + 73, Y + 98);
            var EN_S = amos.Str_S(ARMIA[ARM, NUMER, TE]) + "/" + ARMIA[ARM, NUMER, TEM];// - " "
            var MAG_S = amos.Str_S(ARMIA[ARM, NUMER, TMAG]) + "/" + ARMIA[ARM, NUMER, TMAGMA];// - " "
            screens.Ink(3, 19);
            screens.Text(237, 15, ARMIA_S[ARM, NUMER]);
            screens.Text(237, 25, RASY_S[ARMIA[ARM, NUMER, TRASA]]);
            screens.Text(237, 35, "Energia:");
            screens.Text(237, 45, "Siła:");
            screens.Text(237, 55, "Szybkość:");
            screens.Text(237, 65, "Odporność:");
            screens.Text(237, 75, "Magia:");
            screens.Text(237, 95, "Doświadczenie:");
            if (ARMIA[ARM, NUMER, TWAGA] > ARMIA[ARM, NUMER, TEM])
            {
                screens.Ink(20, 19);
            }
            screens.Text(237, 85, "Obciążenie:");

            screens.Ink(16, 19);
            screens.Text(270, 35, EN_S);
            screens.Text(290, 45, amos.Str_S(ARMIA[ARM, NUMER, TSI]));
            screens.Text(290, 55, amos.Str_S(ARMIA[ARM, NUMER, TSZ]));
            screens.Text(290, 65, amos.Str_S(ARMIA[ARM, NUMER, TP]));
            screens.Text(270, 75, MAG_S);
            screens.Text(290, 85, amos.Str_S(ARMIA[ARM, NUMER, TWAGA]));
            screens.Text(295, 95, amos.Str_S(ARMIA[ARM, NUMER, TDOSW]));
        }

        void WYBOR_PICK(int BR, int X, int Y, int X2, int Y2, int NUMER, ref int BB, int SEK)
        {
            var KONIEC = false;

            WYBOR_PICK_2(BR, X, ref BB);
            do
            {
                var XM = screens.XScreen(screens.XMouse());
                var YM = screens.YScreen(screens.YMouse());
                screens.Sprite(53, screens.XMouse(), screens.YMouse(), BB + GOBY);
                screens.WaitVbl();
                if (screens.MouseClick() == 1)
                {
                    var I = screens.Zone(XM, YM);
                    if (I != 0)
                    {
                        screens.SpriteOff(53);
                        screens.WaitVbl();
                        screens.HotSpot(BB, 0);
                    }

                    if (I > 0 && I < 5)
                    {
                        var BR1 = ARMIA[ARM, NUMER, TPLECAK + I - 1];
                        var TYP = BRON[BR, B_TYP];
                        if (TYP == 17)
                        {
                            ARMIA[ARM, NUMER, TPLECAK + I - 1] = 0;
                            amos.Add(ref ARMIA[ARM, 0, TAMO],
                                BRON[BR, B_DOSW],
                                ARMIA[ARM, 0, TAMO],
                                320);
                        }
                        else
                        {
                            screens.PasteBob(X + 7 + ((I - 1) * 25), Y + 7, BB + GOBY);
                            ARMIA[ARM, NUMER, TPLECAK + I - 1] = BR;
                        }
                        if (BR1 == 0)
                        {
                            KONIEC = true;
                        }
                        else
                        {
                            BR = BR1;
                            WYBOR_PICK_2(BR, X, ref BB);
                        }
                    }

                    if (I > 4 && I < 9)
                    {
                        var BR1 = ARMIA[ARM, NUMER, TPLECAK + I - 1];
                        var TYP = BRON[BR, B_TYP];
                        if (TYP == 17)
                        {
                            ARMIA[ARM, NUMER, TPLECAK + I - 1] = 0;
                            amos.Add(ref ARMIA[ARM, 0, TAMO],
                                BRON[BR, B_DOSW],
                                ARMIA[ARM, 0, TAMO],
                                320);
                        }
                        else
                        {
                            screens.PasteBob(X + 7 + ((I - 5) * 25), Y + 32, BB + GOBY);
                            ARMIA[ARM, NUMER, TPLECAK + I - 1] = BR;
                        }
                        if (BR1 == 0)
                        {
                            KONIEC = true;
                        }
                        else
                        {
                            BR = BR1;
                            WYBOR_PICK_2(BR, X, ref BB);
                        }
                    }

                    if (I > 8 && I < 13)
                    {
                        var BR1 = GLEBA[SEK, I - 9];
                        screens.PasteBob(X2 + 7 + ((I - 9) * 25), Y2 + 7, BB + GOBY);
                        GLEBA[SEK, I - 9] = BR;
                        if (BR1 == 0)
                        {
                            KONIEC = true;
                        }
                        else
                        {
                            BR = BR1;
                            WYBOR_PICK_2(BR, X, ref BB);
                        }
                    }

                    if (I == 13 || I == 15)
                    {
                        var BR1 = ARMIA[ARM, NUMER, TGLOWA + (I - 13)];
                        var PLACE = BRON[BR, B_PLACE];
                        var TYP = BRON[BR, B_TYP];
                        if (PLACE == I - 12 && BR1 == 0)
                        {
                            ARMIA[ARM, NUMER, TGLOWA + (I - 13)] = BR;
                            PRZELICZ(I - 13, 1);
                            if (TYP == 13 || TYP == 18)
                            {
                                ARMIA[ARM, NUMER, TGLOWA + (I - 13)] = 0;
                            }
                            else
                            {
                                screens.MakeMask(BB + GOBY);
                                screens.PasteBob(49, 10 + ((I - 13) * 50), BB + GOBY);
                            }
                            KONIEC = true;
                        }
                    }

                    if (I == 14)
                    {
                        var BR1 = ARMIA[ARM, NUMER, TGLOWA + (I - 13)];
                        var PLACE = BRON[BR, B_PLACE];
                        var TYP = BRON[BR, B_TYP];
                        if (PLACE == I - 12 && BR1 == 0)
                        {
                            ARMIA[ARM, NUMER, TGLOWA + (I - 13)] = BR;
                            PRZELICZ(I - 13, 1);
                            if (TYP == 13)
                            {
                                ARMIA[ARM, NUMER, TGLOWA + (I - 13)] = 0;
                            }
                            else
                            {
                                screens.PasteBob(49, 46, BB + GOBY);
                            }
                            KONIEC = true;
                        }
                    }

                    if (I == 16)
                    {
                        var BR1 = ARMIA[ARM, NUMER, TLEWA];
                        var BR2 = ARMIA[ARM, NUMER, TPRAWA];
                        var PLACE = BRON[BR, B_PLACE];
                        if (BR1 == 0)
                        {
                            if (PLACE == 4 || (PLACE == 6 && BR2 == 0))
                            {
                                screens.PasteBob(9, 60, BB + GOBY);
                                ARMIA[ARM, NUMER, TLEWA] = BR;
                                PRZELICZ(3, 1);
                                KONIEC = true;
                            }
                        }
                    }

                    if (I == 17)
                    {
                        var BR1 = ARMIA[ARM, NUMER, TPRAWA];
                        var BR2 = ARMIA[ARM, NUMER, TLEWA];
                        var PLACE = BRON[BR, B_PLACE];
                        if (BR1 == 0)
                        {
                            if (PLACE == 4 || (PLACE == 6 && BR2 == 0))
                            {
                                screens.PasteBob(89, 60, BB + GOBY);
                                ARMIA[ARM, NUMER, TPRAWA] = BR;
                                KONIEC = true;
                                PRZELICZ(4, 1);
                            }
                        }
                    }
                }
            }
            while (!KONIEC);

            WAGA(ARM, NUMER);
            WYBOR_WYPISZ(Y, NUMER);
            KONIEC = false;
            screens.ShowOn();
        }


        void WYBOR_PICK_2(int BR, int X, ref int BB)
        {
            screens.HideOn();
            BB = BRON[BR, B_BOB] + BROBY;
            screens.HotSpot(BB, 11);
            var TYP = BRON[BR, B_TYP];
            screens.NoMask(BB + GOBY);
            screens.Ink(19);
            screens.Bar(X + 6, 71, X + 98, 88);
            screens.Ink(3, 19);
            screens.Text(130, 78, BRON2_S[TYP]);
            screens.Text(192, 78, "W:" + BRON[BR, B_WAGA]);
            screens.Text(130, 87, BRON_S[BR]);
        }

        void WAGA(int A, int NR)
        {
            var WAGA = 0;
            for (var I = 0; I <= 12; I++)
            {
                var B = ARMIA[A, NR, TGLOWA + I];
                if (B > 0)
                {
                    WAGA += BRON[B, B_WAGA];
                }
            }
            ARMIA[A, NR, TWAGA] = WAGA;
            var DW = ARMIA[A, NR, TEM] - WAGA;
            if (DW < 0)
            {
                ARMIA[A, NR, TSZ] = ARMIA[A, NR, TAMO] - 20;
                if (ARMIA[A, NR, TSZ] <= 0)
                {
                    ARMIA[A, NR, TSZ] = 1;
                }
            }
            else
            {
                ARMIA[A, NR, TSZ] = ARMIA[A, NR, TAMO];
            }
        }

        int SEKTOR(int X, int Y)
        {
            var XSEK = (X / 64);
            var YSEK = (Y / 50);
            var SEK = XSEK + (YSEK * 10);
            return SEK;
        }

        void PRZELICZ(int I, int ZNAK)
        {
            var B = ARMIA[ARM, NUMER, TGLOWA + I];
            var TYP = BRON[B, B_TYP];
            var TYP2 = RASY[ARMIA[ARM, NUMER, TRASA], 4];
            if (TYP != 4 && TYP != 5 && TYP != 12 && TYP != 15 && TYP != 16)
            {
                var RASA = ARMIA[ARM, NUMER, TRASA];
                var SI = BRON[B, B_SI];
                var PAN = BRON[B, B_PAN];
                var SZ = BRON[B, B_SZ];
                var EN = BRON[B, B_EN];

                if (TYP == 13 || TYP == 18)
                {
                    var MAG = BRON[B, B_MAG];
                    var MAGIA = ARMIA[ARM, NUMER, TMAG];
                    var MAGMA = ARMIA[ARM, NUMER, TMAGMA];
                    MAGIA += MAG;
                    if (MAGIA > ARMIA[ARM, NUMER, TMAGMA])
                    {
                        MAGIA = MAGMA;
                    }
                    ARMIA[ARM, NUMER, TMAG] = MAGIA;
                    var MXSI = (RASY[RASA, 1] / 2) + 30;
                    var MXSZ = RASY[RASA, 2] + 20;
                    if (ARMIA[ARM, NUMER, TSI] + SI > MXSI)
                    {
                        SI = MXSI - ARMIA[ARM, NUMER, TSI];
                    }
                    if (ARMIA[ARM, NUMER, TSZ] + SZ > MXSZ)
                    {
                        SZ = MXSZ - ARMIA[ARM, NUMER, TSZ];
                    }
                }
                ARMIA[ARM, NUMER, TSI] += SI * ZNAK;
                ARMIA[ARM, NUMER, TP] += PAN * ZNAK;
                ARMIA[ARM, NUMER, TSZ] += SZ * ZNAK;
                ARMIA[ARM, NUMER, TAMO] += SZ * ZNAK;
                ARMIA[ARM, NUMER, TE] += EN * ZNAK;
                if (TYP == TYP2)
                {
                    ARMIA[ARM, NUMER, TSI] += 4 * ZNAK;
                }
                if (ARMIA[ARM, NUMER, TSI] > 100)
                {
                    ARMIA[ARM, NUMER, TSI] = 100;
                }
                if (ARMIA[ARM, NUMER, TE] > ARMIA[ARM, NUMER, TEM])
                {
                    ARMIA[ARM, NUMER, TE] = ARMIA[ARM, NUMER, TEM];
                }
                if (ARMIA[ARM, NUMER, TE] < 1)
                {
                    ARMIA[ARM, NUMER, TE] = 1;
                }
            }
        }
    }
}
