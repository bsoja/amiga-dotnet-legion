using AmigaNet.Amos.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmigaNet.Legion
{
    public partial class Legion
    {
        void MIASTO(int NR)
        {
            var KONIEC = false;
            var KONIEC2 = false;
            var KONIEC3 = false;
            var CZYJE = MIASTA[NR, 0, M_CZYJE];

            var RO_S = "";
            var D_S = "";
            var DANE = false;

            if (CZYJE == 1)
            {
                RO_S = "Rozkazy";
                DANE = true;
            }
            else
            {
                RO_S = "Wywiad";
                if (MIASTA[NR, 1, M_Y] > 25)
                {
                    DANE = false;
                    D_S = "Brak informacji.";
                }
                else
                {
                    var DNI = MIASTA[NR, 1, M_Y];
                    var DN_S = "";
                    if (DNI == 1)
                    {
                        DN_S = " dzień.";
                    }
                    else
                    {
                        DN_S = " dni.";
                    }
                    DANE = false;
                    D_S = "Informacje za " + amos.Str_S(DNI) + DN_S;
                }
                if (MIASTA[NR, 1, M_Y] == 0)
                {
                    RO_S = "";
                    D_S = "";
                    DANE = true;
                }
            }
            if (PREFS[5] == 1)
            {
                WJAZD(MIASTA[NR, 0, M_X], MIASTA[NR, 0, M_Y], 80, 80, 150, 100, 4);
            }
            OKNO(80, 80, 150, 100);
            var GDX = OKX + 106;
            GADGET(OKX + 4, OKY + 4, 142, 74, "", 31, 2, 30, 1, -1);
            if (RO_S != "")
            {
                GADGET(OKX + 4, OKY + 80, 40, 15, RO_S, 8, 2, 6, 31, 1);
            }
            else
            {
                GDX = OKX + 55;
            }
            GADGET(GDX, OKY + 80, 40, 15, "   Ok  ", 8, 2, 6, 31, 2);
            var LUDZIE = MIASTA[NR, 0, M_LUDZIE];
            var PODATEK = MIASTA[NR, 0, M_PODATEK];
            var MORALE = MIASTA[NR, 0, M_MORALE];
            var MORALE2 = MORALE / 20;
            if (MORALE2 > 4) MORALE2 = 4;
            var MUR = MIASTA[NR, 0, M_MUR];
            screens.PasteBob(OKX + 8, OKY + 8, 20 + MUR);
            var M_S = "";
            if (LUDZIE > 700)
            {
                M_S = "Miasto : ";
            }
            else
            {
                M_S = "Osada  : ";
            }
            screens.Ink(1, 30);
            screens.Text(OKX + 50, OKY + 15, M_S + MIASTA_S[NR]);
            screens.SetZone(3, OKX + 85, OKY + 5, OKX + 145, OKY + 16);
            if (DANE)
            {
                screens.Text(OKX + 50, OKY + 25, amos.Str_S(LUDZIE) + " mieszkańców");
                screens.Text(OKX + 50, OKY + 35, "Podatek : " + amos.Str_S(PODATEK));
                screens.Text(OKX + 50, OKY + 45, "Morale :" + GUL_S[MORALE2]);

                for (var I = 2; I <= 10; I++)
                {
                    var TYP = MIASTA[NR, I, M_LUDZIE];
                    if (TYP > 3)
                    {
                        var C_S = BUDYNKI_S[TYP];
                        if (!D_S.Contains(C_S)) D_S = D_S + C_S + " ";
                        //D_S = D_S - C_S;
                        //D_S = D_S + C_S + " ";
                    }
                }
                NAPISZ(OKX + 8, OKY + 57, 134, 20, D_S, 0, 1, 30);
            }
            else
            {
                screens.Text(OKX + 25, OKY + 60, D_S);
            }
            do
            {
                if (screens.MouseClick() == 1)
                {
                    var STREFA = screens.MouseZone();
                    if (STREFA == 2 || STREFA == 0)
                    {
                        KONIEC = true;
                        ZOKNO();
                    }
                    if (STREFA == 3)
                    {
                        WPISZ(OKX + 84, OKY + 15, 1, 30, 12);
                        MIASTA_S[NR] = WPI_S;
                    }
                    if (STREFA == 1 && CZYJE != 1)
                    {
                        ZOKNO();
                        KONIEC = true;
                        SZPIEGUJ(NR, 0);
                    }
                    if (STREFA == 1 && CZYJE == 1)
                    {
                        ZOKNO();
                        KONIEC = true;
                        MIASTO_RYSUJ_ROZKAZY();
                        KONIEC2 = false;
                        do
                        {
                            if (screens.MouseClick() == 1)
                            {
                                var STREFA2 = screens.MouseZone();
                                if (STREFA2 == 2)
                                {
                                    ZOKNO();
                                    REKRUTACJA(amos.Rnd(20), NR, -1);
                                    MIASTO_RYSUJ_ROZKAZY();
                                }
                                if (STREFA2 == 4 && MIASTA[NR, 0, M_MUR] < 3)
                                {
                                    ZOKNO();
                                    BUDOWA_MURU(NR);
                                    MIASTO_RYSUJ_ROZKAZY();
                                }
                                if (STREFA2 == 3)
                                {
                                    ZOKNO();
                                    ROZBUDOWA(NR);
                                    KONIEC = true;
                                    KONIEC2 = true;
                                }
                                if (STREFA2 == 5)
                                {
                                    KONIEC2 = true;
                                    ZOKNO();
                                }
                                if (STREFA2 == 1)
                                {
                                    ZOKNO();
                                    OKNO(90, 90, 135, 70);
                                    GADGET(OKX + 4, OKY + 4, 100, 64, "", 31, 2, 30, 1, 0);
                                    screens.PasteBob(OKX + 6, OKY + 6, 41);
                                    GADGET(OKX + 110, OKY + 4, 20, 15, " " + UpArrowChar + " ", 8, 2, 6, 31, 1);
                                    GADGET(OKX + 110, OKY + 24, 20, 15, " " + DownArrowChar + " ", 8, 2, 6, 31, 2);
                                    GADGET(OKX + 110, OKY + 52, 20, 15, "Ok", 8, 2, 6, 31, 3);
                                    screens.SetFont(FON2);
                                    screens.GetBlock(120, OKX + 6, OKY + 6, 32, 32);
                                    screens.GrWriting(0);
                                    screens.Ink(23);
                                    screens.Text(OKX + 8, OKY + 20, MIASTA[NR, 0, M_PODATEK].ToString());
                                    KONIEC3 = false;

                                    do
                                    {
                                        if (screens.MouseKey() == LEWY)
                                        {
                                            var STREFA3 = screens.MouseZone();
                                            if (STREFA3 == 1)
                                            {
                                                amos.Add(ref MIASTA[NR, 0, M_PODATEK], 1, 0, 25);
                                                screens.PutBlock(120);
                                                screens.Text(OKX + 8, OKY + 20, MIASTA[NR, 0, M_PODATEK].ToString());
                                                amos.Wait(5);
                                            }

                                            if (STREFA3 == 2)
                                            {
                                                amos.Add(ref MIASTA[NR, 0, M_PODATEK], -1, 0, 25);
                                                screens.PutBlock(120);
                                                screens.Text(OKX + 8, OKY + 20, MIASTA[NR, 0, M_PODATEK].ToString());
                                                amos.Wait(5);
                                            }

                                            if (STREFA3 == 3)
                                            {
                                                KONIEC3 = true;
                                                screens.SetFont(FON1);
                                                ZOKNO();
                                                MIASTO_RYSUJ_ROZKAZY();
                                            }
                                        }
                                    } while (!KONIEC3);

                                }
                            }
                        } while (!KONIEC2);
                    }
                }
            } while (!KONIEC);
        }

        void MIASTO_RYSUJ_ROZKAZY()
        {
            OKNO(110, 90, 90, 110);
            GADGET(OKX + 8, OKY + 8, 72, 15, "Podatki", 8, 2, 6, 31, 1);
            GADGET(OKX + 8, OKY + 28, 72, 15, "Nowy Legion", 8, 2, 6, 31, 2);
            GADGET(OKX + 8, OKY + 48, 72, 15, "Rozbudowa", 8, 2, 6, 31, 3);
            GADGET(OKX + 8, OKY + 68, 72, 15, "Budowa Murów", 8, 2, 6, 31, 4);
            GADGET(OKX + 8, OKY + 88, 72, 15, "Exit", 8, 2, 6, 31, 5);
        }

        void BUDOWA_MURU(int MIASTO)
        {
            OKNO(110, 100, 80, 84);
            var MUR = MIASTA[MIASTO, 0, M_MUR];
            GADGET(OKX + 4, OKY + 44, 72, 15, "Granitowy", 8, 1, 6, 31, 3);
            screens.Ink(23, 6);
            screens.Text(OKX + 52, OKY + 54, "10000");
            if (MUR < 2)
            {
                GADGET(OKX + 4, OKY + 24, 72, 15, "Kamienny ", 8, 1, 6, 31, 2);
                screens.Ink(23, 6);
                screens.Text(OKX + 52, OKY + 34, "7000");
            }
            if (MUR < 1)
            {
                GADGET(OKX + 4, OKY + 4, 72, 15, "Palisada ", 8, 1, 6, 31, 1);
                screens.Ink(23, 6);
                screens.Text(OKX + 52, OKY + 14, "4000");
            }
            GADGET(OKX + 4, OKY + 64, 72, 15, "       Exit", 8, 1, 6, 31, 10);

            var KONIEC = false;
            do
            {
                if (screens.MouseClick() == 1)
                {
                    var STREFA = screens.MouseZone();
                    if (STREFA == 10)
                    {
                        KONIEC = true;
                    }
                    if (STREFA > 0 && STREFA < 4 && GRACZE[1, 1] - (4000 + (STREFA - 1) * 3000) >= 0)
                    {
                        KONIEC = true;
                        GRACZE[1, 1] += -(4000 + (STREFA - 1) * 3000);
                        MIASTA[MIASTO, 0, M_MUR] = STREFA;
                    }
                }
            } while (!KONIEC);
            ZOKNO();
        }

        void ROZBUDOWA(int MIASTO)
        {
            var KONIEC = false;
            var MIA_S = MIASTA_S[MIASTO];
            var TEREN = MIASTA[MIASTO, 1, M_X];
            if (PREFS[3] == 1)
            {
                _TRACK_FADE(1);
            }
            screens.SpriteOff(2);
            //'----
            screens.ShowOn();
            banks.EraseAll();
            screens.ScreenOpen(2, 80, 50, 32, PixelMode.Lowres);
            //Curs Off;
            //Flash Off;
            screens.ScreenDisplay(2, 252, 140, 80, 50);
            screens.Cls(0);
            screens.ScreenHide(2);
            screens.ScreenOpen(0, 640, 512, 32, PixelMode.Lowres);
            screens.ScreenDisplay(0, 130, 40, 320, 234);
            //Curs Off;
            //Flash Off;
            screens.Cls(0);
            screens.ScreenHide();
            screens.ReserveZone(200);
            //'------------------
            screens.ScreenOpen(1, 320, 160, 32, PixelMode.Lowres);
            screens.ScreenDisplay(1, 130, 265, 320, 35);
            //Curs Off;
            //Flash Off;
            screens.Cls(10);
            screens.ScreenHide();
            screens.ReserveZone(30);
            _LOAD("dane/gad", 0);
            screens.GetBobPalette();
            _LOAD("dane/gad3", 1);
            USTAW_FONT("defender2", 8);
            screens.ChangeMouse(53);
            screens.PasteBob(0, 0, 1);
            screens.GrWriting(0);
            screens.Ink(21);
            screens.Text(8, 10, "Rozbudowa miasta " + MIA_S);
            screens.Ink(1);
            screens.Text(7, 9, "Rozbudowa miasta " + MIA_S);
            for (var I = 0; I <= 5; I++)
            {
                var BB_S = "bob" + (51 + I);
                GADGET(4 + I * 24, 12, 20, 20, BB_S, 5, 12, 9, 1, I + 1);
            }
            GADGET(165, 2, 110, 30, "", 0, 5, 22, 1, -1);
            GADGET(280, 6, 30, 20, " exit", 7, 0, 11, 1, 10);
            //'------------------
            screens.Screen(0);
            USTAW_FONT("defender2", 8);
            BIBY = 62;
            MSX = 320;
            MSY = 278;
            //'----- 
            RYSUJ_SCENERIE(TEREN, MIASTO);
            screens.ScreenShow(0);
            screens.ScreenShow(1);
            screens.View();
            amos.TrackStop();
            screens.Screen(1);

            var X = 0;
            var Y = 0;
            var SZER = 0;
            var SZER2 = 0;
            var WYS = 0;
            var WYS2 = 0;
            var CENA = 0;
            var CZAS = 0;
            var B1 = 0;
            var A_S = "";
            var TYP = 0;
            var MOZNA = false;

            do
            {
                var HY = screens.YMouse();
                if (HY > 263)
                {
                    screens.Screen(1);
                    if (screens.MouseClick() == 1)
                    {
                        var STREFA = screens.MouseZone();
                        if (STREFA > 0 && STREFA < 8)
                        {
                            var I = STREFA + 3;
                            var BB_S = "bob" + (56 + STREFA);
                            GADGET(4 + (STREFA - 1) * 24, 12, 20, 20, BB_S, 12, 5, 11, 1, 0);
                            SZER = BUDYNKI[I, 0];
                            SZER2 = SZER / 2;
                            WYS = BUDYNKI[I, 1];
                            WYS2 = WYS / 2;
                            CENA = BUDYNKI[I, 2];
                            CZAS = BUDYNKI[I, 3];
                            B1 = BUDYNKI[I, 4];
                            A_S = BUDYNKI_S[I];
                            TYP = I;
                            ROZBUDOWA_WYPISZ(A_S, CENA);
                            BB_S = "bob" + (50 + STREFA);
                            GADGET(4 + (STREFA - 1) * 24, 12, 20, 20, BB_S, 5, 12, 9, 1, -1);
                        }
                        if (STREFA == 10)
                        {
                            GADGET(280, 6, 30, 20, " exit", 0, 7, 12, 1, 0);
                            KONIEC = true;
                            GADGET(280, 6, 30, 20, " exit", 7, 0, 11, 1, 0);
                        }
                    }
                }
                else
                {
                    HY = screens.YMouse();
                    screens.Screen(0);
                    var B_S = screens.Inkey_S();
                    var KLAW = screens.Scancode();
                    if (screens.MouseKey() == PRAWY)
                    {
                        SKROL(0);
                    }
                    if (KLAW >= KeyLeft && KLAW <= KeyDown)
                    {
                        KLAWSKROL(KLAW);
                    }
                    if (SZER > 0)
                    {
                        screens.GrWriting(3);
                        X = screens.XScreen(screens.XMouse()) - SZER2;
                        Y = screens.YScreen(screens.YMouse()) - WYS2;
                        screens.Box(X, Y, X + SZER, Y + WYS);
                        screens.WaitVbl();
                        screens.Box(X, Y, X + SZER, Y + WYS);
                    }
                    if (screens.MouseClick() == 1 && SZER > 0 && GRACZE[1, 1] - CENA >= 0)
                    {
                        ROZBUDOWA_CHECK(ref MOZNA, X, Y, SZER, WYS, SZER2, WYS2);
                        for (var I = 2; I <= 20; I++)
                        {

                            if (MIASTA[MIASTO, I, M_LUDZIE] == 0 && MOZNA)
                            {
                                screens.PasteBob(X, Y, BIBY + 12 + B1);
                                MIASTA[MIASTO, I, M_X] = X;
                                MIASTA[MIASTO, I, M_Y] = Y;
                                MIASTA[MIASTO, I, M_LUDZIE] = TYP;
                                MIASTA[MIASTO, 1, M_MORALE] += 2;
                                MIASTA[MIASTO, 0, M_MORALE] += 20;
                                screens.SetZone(120 + I, X, Y, X + SZER, Y + WYS);
                                GRACZE[1, 1] += -CENA;
                                screens.Screen(1);
                                ROZBUDOWA_WYPISZ(A_S, CENA);
                                screens.Screen(0);
                                break;
                            }
                        }
                    }
                }
            } while (!KONIEC);

            banks.EraseAll();
            screens.ScreenClose(2);
            screens.ScreenClose(1);
            screens.ScreenClose(0);
            SETUP0();
            VISUAL_OBJECTS();
            screens.Sprite(2, SPX, SPY, 1);
            CENTER(MIASTA[MIASTO, 0, M_X], MIASTA[MIASTO, 0, M_Y], 0);
        }

        void ROZBUDOWA_CHECK(ref bool MOZNA, int X, int Y, int SZER, int WYS, int SZER2, int WYS2)
        {
            MOZNA = false;
            if (X > 0 && X + SZER < 640 && Y > 0 && Y + WYS < 512)
            {
                if (screens.Zone(X, Y) == 0 &&
                    screens.Zone(X + SZER, Y) == 0 &&
                    screens.Zone(X, Y + WYS) == 0 &&
                    screens.Zone(X + SZER, Y + WYS) == 0 &&
                    screens.Zone(X + SZER2, Y + WYS2) == 0)
                {
                    MOZNA = true;
                }
            }
        }

        void ROZBUDOWA_WYPISZ(String A_S, int CENA)
        {
            screens.Ink(22);
            screens.Bar(167, 3, 167 + 105, 3 + 28);
            screens.Ink(1, 22);
            screens.Text(172, 15, A_S);
            screens.Text(172, 25, "cena :" + amos.Str_S(CENA));
            screens.SetFont(FON2);
            screens.Text(210, 12, amos.Str_S(GRACZE[1, 1]));
            screens.SetFont(FON1);
        }
    }
}
