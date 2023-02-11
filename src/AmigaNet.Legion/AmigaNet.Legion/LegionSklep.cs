using AmigaNet.Amos.Screens;

namespace AmigaNet.Legion
{
    public partial class Legion
    {
        void SKLEP_(int MIASTO, int SNR, int A, int NR2)
        {
            var NR = NR2;
            var TYP = MIASTA[MIASTO, SNR, M_LUDZIE];
            var PUPIL = MIASTA[MIASTO, SNR, M_PODATEK];
            var A_S = "";
            if (TYP == 4)
            {
                A_S = "sklep1.hb";
            }
            if (TYP == 5)
            {
                A_S = "sklep2.hb";
            }
            if (TYP == 6)
            {
                A_S = "sala.hb";
            }
            if (TYP == 7)
            {
                A_S = "sklep1.hb";
            }
            if (TYP == 8)
            {
                A_S = "yodi.hb";
            }
            if (TYP == 9)
            {
                SKLEP_SPICHLERZ(MIASTO);
                SKLEP_OVER(A, NR2);
                return;
            }
            screens.ScreenClose(2);
            for (var I = 50; I <= 13; I--)
            {
                //Mvolume I;
                screens.WaitVbl();
            }
            _LOAD("dane/grafika/" + A_S, 5);
            if (TYP == 5)
            {
                //Flash 1,"(eee,5)(fff,6)"
                //Flash 29,"(ec8,5)(fd9,1)(fea,1)(ffb,2)(fea,1)(fd9,2)"
                //Flash 30,"(fd8,5)(fe9,2)(ffa,2)(fe9,1)"
                //Flash 28,"(ea8,5)(fb9,3)(fca,4)"
                //Flash 24,"(e96,5)(fa7,3)(fb8,3)"
            }
            if (TYP == 4 || TYP == 7)
            {
                //Flash 1,"(eee,9)(fff,6)"
                //Flash 29,"(f72,6)(f83,6)(f94,6)"
                //Flash 30,"(fc6,9)(fd6,9)"
                //Flash 28,"(d85,8)(e96,6)(ea7,4)"
            }
            screens.ScreenDisplay(2, 130, 40, 320, 244);
            screens.ScreenToFront(2);
            screens.ReserveZone(100);
            screens.SetFont(FON1);
            screens.Ink(21, 0);
            screens.GrWriting(0);
            screens.GetBlock(1, 20, 180, 112, 30);
            screens.GetBlock(2, 240, 5, 64, 25);
            screens.GetBlock(3, 240, 180, 80, 20);

            screens.ScreenToFront(1);
            screens.ScreenDisplay(1, 130, 255, 320, 44);
            screens.Screen(1);
            screens.View();
            screens.ResetZone();
            screens.SetFont(FON1);
            screens.PasteBob(0, 0, 1);
            screens.Colour(0, 3, 1, 0);
            GADGET(6, 2, 200, 40, "", 19, 6, 0, 1, -1);
            GADGET(234, 2, 80, 40, "", 19, 6, 0, 1, -1);
            GADGET(210, 2, 20, 16, " " + UpArrowChar, 5, 0, 8, 1, 1);
            GADGET(210, 24, 20, 16, " " + DownArrowChar, 5, 0, 8, 1, 2);
            SKLEP_KLIENT(A, NR);
            SKLEP_SZMAL();
            screens.GrWriting(1);

            var II = 0;
            for (var Y = 0; Y <= 1; Y++)
            {
                for (var X = 0; X <= 9; X++)
                {
                    if (SKLEP[SNR, II] > 0)
                    {
                        var B1 = BRON[SKLEP[SNR, II], B_BOB];
                        screens.PasteBob(8 + X * 20, 4 + Y * 20, BROBY + B1);
                    }
                    screens.SetZone(10 + II, 8 + X * 20, 4 + Y * 20, 28 + X * 20, 24 + Y * 20);
                    II++;
                }
            }

            SKLEP_PLECAK(A, NR);
            var KONIEC = false;
            do
            {
                if (screens.MouseClick() == 1)
                {
                    var I = screens.MouseZone();
                    if (I == 1)
                    {
                        GADGET(210, 2, 20, 16, " " + UpArrowChar, 0, 5, 10, 1, 0);
                        GADGET(210, 2, 20, 16, " " + DownArrowChar, 5, 0, 8, 1, -1);
                    AG2:
                        amos.Add(ref NR, 1, 1, 10);
                        if (ARMIA[ARM, NR, TE] <= 0)
                        {
                            goto AG2;
                        }
                        SKLEP_PLECAK(A, NR);
                        SKLEP_KLIENT(A, NR);
                    }
                    if (I == 2)
                    {
                        GADGET(210, 24, 20, 16, " " + UpArrowChar, 0, 5, 10, 1, 0);
                        GADGET(210, 24, 20, 16, " " + DownArrowChar, 5, 0, 8, 1, -1);
                    AG1:
                        amos.Add(ref NR, -1, 1, 10);
                        if (ARMIA[ARM, NR, TE] <= 0)
                        {
                            goto AG1;
                        }
                        SKLEP_PLECAK(A, NR);
                        SKLEP_KLIENT(A, NR);
                    }

                    if (I > 9 && I < 38)
                    {
                        var I2 = I - 10;
                        var BRO = SKLEP[SNR, I2];
                        if (BRO > 0)
                        {
                            var B1 = BRON[BRO, B_BOB];
                            var BRO1_S = BRON2_S[BRON[BRO, B_TYP]];
                            var BRO2_S = BRON_S[BRO];
                            var TYPB = BRON[BRO, B_TYP];
                            var CENA = BRON[BRO, B_CENA] + ((BRON[BRO, B_CENA] * MIASTA[MIASTO, TYPB, M_MUR]) / 100);
                            if (GRACZE[1, 1] - CENA >= 0)
                            {
                                var ZNAK = -1;
                                A_S = BRO1_S + " " + BRO2_S;
                                var B_S = "kosztuje :" + CENA;
                                SKLEP_NAPISZ(A_S, B_S);
                                SKLEP[SNR, I2] = 0;
                                var X = 0;
                                var Y = 0;
                                if (I2 < 10)
                                {
                                    Y = 4;
                                    X = 8 + I2 * 20;
                                }
                                else
                                {
                                    Y = 24;
                                    X = 8 + (I2 - 10) * 20;
                                }
                                screens.Ink(0);
                                screens.Bar(X, Y, X + 16, Y + 16);
                                SKLEP_PICK(BRO, SNR, A, NR, CENA, ZNAK, ref KONIEC);
                            }
                            else
                            {
                                A_S = BRO1_S + " " + BRO2_S;
                                var B_S = "Nie dla ciebie " + CENA;
                                SKLEP_NAPISZ(A_S, B_S);
                            }
                        }
                    }
                    if (I > 39 && I < 48)
                    {
                        var I2 = I - 40;
                        var BRO = ARMIA[A, NR, TPLECAK + I2];
                        if (BRO > 0)
                        {
                            var B1 = BRON[BRO, B_BOB];
                            var BRO1_S = BRON2_S[BRON[BRO, B_TYP]];
                            var BRO2_S = BRON_S[BRO];
                            A_S = BRO1_S + " " + BRO2_S;
                            var TYPB = BRON[BRO, B_TYP];
                            var CENA = BRON[BRO, B_CENA] + ((BRON[BRO, B_CENA] * MIASTA[MIASTO, TYPB, M_MUR]) / 100);
                            CENA = CENA - ((CENA * 10) / 100);
                            var ZNAK = 1;
                            var B_S = "Zapłacę " + CENA;
                            SKLEP_NAPISZ(A_S, B_S);
                            ARMIA[A, NR, TPLECAK + I2] = 0;
                            var X = 0;
                            var Y = 0;
                            if (I2 < 4)
                            {
                                Y = 4;
                                X = 236 + I2 * 20;
                            }
                            else
                            {
                                Y = 24;
                                X = 236 + (I2 - 4) * 20;
                            }
                            screens.Ink(0);
                            screens.Bar(X, Y, X + 16, Y + 16);
                            SKLEP_PICK(BRO, SNR, A, NR, CENA, ZNAK, ref KONIEC);
                        }
                    }
                }
                if (screens.Inkey_S() == "q" || screens.MouseKey() == PRAWY)
                {
                    KONIEC = true;
                }
            }
            while (!KONIEC);

            SKLEP_OVER(A, NR2);

        }

        void SKLEP_NAPISZ(String A_S, String B_S)
        {
            screens.Screen(2);
            screens.PutBlock(1, 20, 180);
            OUTLINE(20, 192, A_S, 31, 0);
            OUTLINE(20, 205, B_S, 31, 0);
            screens.Screen(1);
        }

        void SKLEP_SZMAL()
        {
            screens.Screen(2);
            screens.SetFont(FON2);
            screens.PutBlock(2, 240, 5);
            OUTLINE(240, 15, amos.Str_S(GRACZE[1, 1]), 31, 0);
            screens.SetFont(FON1);
            screens.Screen(1);
        }

        void SKLEP_KLIENT(int A, int NR)
        {
            screens.Screen(2);
            screens.PutBlock(3, 240, 180);
            OUTLINE(240, 190, ARMIA_S[A, NR] + " " + RASY_S[ARMIA[A, NR, TRASA]], 31, 0);
            screens.Screen(1);
        }

        void SKLEP_PLECAK(int A, int NR)
        {
            screens.Ink(0);
            screens.Bar(235, 3, 235 + 78, 3 + 38);
            for (var I = 0; I <= 3; I++)
            {
                if (ARMIA[A, NR, TPLECAK + I] > 0)
                {
                    var B1 = BRON[ARMIA[A, NR, TPLECAK + I], B_BOB];
                    screens.PasteBob(236 + I * 20, 4, BROBY + B1);
                }
                screens.SetZone(40 + I, 236 + I * 20, 4, 256 + I * 20, 24);
                if (ARMIA[A, NR, TPLECAK + I + 4] > 0)
                {
                    var B2 = BRON[ARMIA[A, NR, TPLECAK + I + 4], B_BOB];
                    screens.PasteBob(236 + I * 20, 24, BROBY + B2);
                }
                screens.SetZone(44 + I, 236 + I * 20, 24, 256 + I * 20, 44);
            }
        }

        void SKLEP_PICK(int BRO, int SNR, int A, int NR, int CENA, int ZNAK, ref bool KONIEC)
        {
            var X = 0;
            var Y = 0;
            //screensManager.HideOn();
            var BB = BRON[BRO, B_BOB] + BROBY;
            var TYP = BRON[BRO, B_TYP];
            do
            {
                var XM = screens.XScreen(screens.XMouse());
                var YM = screens.YScreen(screens.YMouse());
                screens.HotSpot(BB, 11);
                screens.Sprite(53, screens.XMouse(), screens.YMouse(), BB);
                screens.WaitVbl();
                if (screens.MouseClick() == 1)
                {
                    screens.SpriteOff(53);
                    screens.WaitVbl();
                    screens.HotSpot(BB, 0);
                    var J = screens.Zone(XM, YM);
                    if (J > 9 && J < 38)
                    {
                        var J2 = J - 10;
                        var BR1 = SKLEP[SNR, J2];
                        if (BR1 == 0)
                        {
                            if (J2 < 10)
                            {
                                Y = 4;
                                X = 8 + J2 * 20;
                            }
                            else
                            {
                                Y = 24;
                                X = 8 + (J2 - 10) * 20;
                            }
                            screens.PasteBob(X, Y, BB);
                            SKLEP[SNR, J2] = BRO;
                            KONIEC = true;
                            if (ZNAK == 1)
                            {
                                GRACZE[1, 1] += CENA * ZNAK;
                                SKLEP_SZMAL();
                            }
                        }
                    }
                    if (J > 39 && J < 48)
                    {
                        var J2 = J - 40;
                        var BR1 = ARMIA[A, NR, TPLECAK + J2];
                        if (BR1 == 0)
                        {
                            if (J2 < 4)
                            {
                                Y = 4;
                                X = 236 + J2 * 20;
                            }
                            else
                            {
                                Y = 24;
                                X = 236 + (J2 - 4) * 20;
                            }
                            screens.PasteBob(X, Y, BB);
                            ARMIA[A, NR, TPLECAK + J2] = BRO;
                            KONIEC = true;
                            if (ZNAK == -1 && GRACZE[1, 1] + (CENA * ZNAK) >= 0)
                            {
                                GRACZE[1, 1] += CENA * ZNAK;
                                SKLEP_SZMAL();
                            }
                        }
                    }

                }
            }
            while (!KONIEC);

            KONIEC = false;
            screens.ShowOn();
        }

        void SKLEP_SPICHLERZ(int MIASTO)
        {
            screens.ScreenClose(2);
            screens.ScreenHide(1);
            for (var I = 50; I <= 13; I--)
            {
                //Mvolume I;
                screens.WaitVbl();
            }
            _LOAD("dane/grafika/piekarz.hb", 5);
            screens.ScreenDisplay(2, 130, 40, 320, 244);
            screens.View();
            //screens.ReserveZone(3);
            screens.ReserveZone(4);
            var WOJSKO = ARMIA[ARM, 0, TAMO];
            var SPICH = MIASTA[MIASTO, 1, M_LUDZIE];
            var SZMAL = GRACZE[1, 1];
            var CENA = 10 + ((10 * MIASTA[MIASTO, 17, M_MUR]) / 100);
            if (WOJSKO < 0)
            {
                WOJSKO = 0;
            }
            screens.GrWriting(0);
            screens.SetFont(FON2);
            screens.GetBlock(1, 245, 12, 64, 20);
            OUTLINE(250, 25, amos.Str_S(SZMAL), 30, 2);
            OUTLINE(15, 25, "Dzisiejsza cena:", 30, 2);
            OUTLINE(60, 40, amos.Str_S(CENA), 30, 2);
            OUTLINE(15, 210, "W Spichlerzu:", 30, 2);
            screens.GetBlock(2, 60, 220, 64, 25);
            OUTLINE(60, 230, amos.Str_S(SPICH), 30, 2);
            OUTLINE(190, 210, "Legion 1 :", 30, 2);
            screens.GetBlock(3, 215, 220, 64, 25);
            OUTLINE(215, 230, amos.Str_S(WOJSKO), 30, 2);
            screens.SetFont(FON1);
            GADGET(140, 215, 12, 15, "<", 25, 7, 16, 30, 1);
            GADGET(160, 215, 12, 15, ">", 25, 7, 16, 30, 3);
            //Pen 30
            var MYSZ = 0;
            do
            {
                MYSZ = screens.MouseKey();
                if (MYSZ == LEWY)
                {
                    var STREFA = screens.MouseZone();
                    if (STREFA == 1 || STREFA == 3)
                    {
                        var ZNAK = STREFA - 2;
                        SKLEP_ROZPIS(ref SPICH, ref ZNAK, ref WOJSKO, ref SZMAL, CENA);
                        //Wait 10
                        while (screens.MouseKey() == LEWY) { SKLEP_ROZPIS(ref SPICH, ref ZNAK, ref WOJSKO, ref SZMAL, CENA); }
                    }
                }
            }
            while (!(MYSZ == PRAWY));

            ARMIA[ARM, 0, TAMO] = WOJSKO;
            MIASTA[MIASTO, 1, M_LUDZIE] = SPICH;
            GRACZE[1, 1] = SZMAL;
            screens.ScreenShow(1);
        }

        void SKLEP_ROZPIS(ref int SPICH, ref int ZNAK, ref int WOJSKO, ref int SZMAL, int CENA)
        {
            if (SPICH == 0 && ZNAK == 1)
            {
                ZNAK = 0;
            }
            if (WOJSKO == 0 && ZNAK == -1)
            {
                ZNAK = 0;
            }
            if (WOJSKO > 320 && ZNAK == 1)
            {
                ZNAK = 0;
            }
            if (SZMAL - ZNAK * CENA < 0 && ZNAK == 1)
            {
                ZNAK = 0;
            }
            if (ZNAK != 0)
            {
                WOJSKO += ZNAK;
                SPICH += -ZNAK;
                SZMAL += -ZNAK * CENA;
                screens.SetFont(FON2);
                screens.PutBlock(1);
                OUTLINE(250, 25, amos.Str_S(SZMAL), 30, 2);
                screens.WaitVbl();
                screens.PutBlock(2);
                screens.PutBlock(3);
                screens.Text(60, 230, amos.Str_S(SPICH));
                screens.Text(215, 230, amos.Str_S(WOJSKO));
            }
        }

        void SKLEP_OVER(int A, int NR2)
        {
            screens.ScreenClose(2);
            //Flash Off 
            screens.ScreenOpen(2, 64, 50, 32, PixelMode.Lowres);
            //Curs Off;
            //Flash Off 
            screens.ScreenHide();
            screens.ScreenToBack();
            //Del Block
            screens.Screen(1);
            screens.Colour(0, 0, 0, 0);
            screens.ScreenDisplay(1, 130, 275, 320, 25);
            screens.ResetZone();
            EKRAN1();
            ARMIA[A, NR2, TY] += 8;
            ARMIA[A, NR2, TTRYB] = 0;
            screens.Screen(0);
            screens.View();
            for (var I = 13; I <= 50; I++)
            {
                //Mvolume I;
                screens.WaitVbl();
            }
        }
    }
}
