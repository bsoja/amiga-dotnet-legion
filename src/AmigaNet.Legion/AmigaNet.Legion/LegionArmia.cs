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
        void ARMIA_(int A)
        {
            var AX = ARMIA[A, 0, TX];
            var AY = ARMIA[A, 0, TY];
            var PL = ARMIA[A, 0, TMAG];
            if (PREFS[5] == 1)
            {
                WJAZD(AX, AY, 80, 80, 150, 100, 4);
            }

            OKNO(80, 80, 150, 100);

            var GD = 40;
            var RO_S = "";
            var D_S = "";
            var DANE = false;

            if (A < 20)
            {
                RO_S = "Rozkazy";
                DANE = true;
            }
            else
            {
                RO_S = "Wywiad";
                if (ARMIA[A, 0, TMAGMA] > 28 && ARMIA[A, 0, TMAGMA] < 100)
                {
                    DANE = false;
                    D_S = "Brak informacji.";
                }
                else
                {
                    var DNI_S = "";
                    if (ARMIA[A, 0, TMAGMA] > 1)
                        DNI_S = " dni.";
                    else
                        DNI_S = " dzień.";
                    DANE = false;
                    D_S = "Informacje za " + amos.Str_S(ARMIA[A, 0, TMAGMA]) + DNI_S;
                }

                if (ARMIA[A, 0, TMAGMA] == 0 || ARMIA[A, 0, TMAGMA] == 100)
                {

                    RO_S = "śledzenie";
                    GD = 52;
                    DANE = true;
                }
            }

            GADGET(OKX + 4, OKY + 4, 142, 74, "", 31, 2, 30, 1, -1);
            GADGET(OKX + 4, OKY + 80, GD, 15, RO_S, 8, 2, 6, 31, 10);
            GADGET(OKX + 106, OKY + 80, 40, 15, "   Ok  ", 8, 2, 6, 31, 1);

            if (ARMIA[A, 0, TMAGMA] == 100)
            {
                screens.Ink(31, 6);
                screens.Text(OKX + 48, OKY + 89, "@");
            }

            screens.NoMask(23 + PL);
            screens.PasteBob(OKX + 8, OKY + 8, 23 + PL);
            screens.SetZone(11, OKX + 50, OKY + 5, OKX + 120, OKY + 15);
            screens.Ink(1, 30);
            screens.Text(OKX + 50, OKY + 15, ARMIA_S[A, 0]);

            var TEREN = 0;

            if (DANE)
            {
                var WOJ = 0;
                var SILA = 0;
                var SPEED = 0;

                for (var I = 1; I <= 10; I++)
                {
                    if (ARMIA[A, I, TE] > 0)
                    {
                        WOJ += 1;
                        SILA += ARMIA[A, I, TSI];
                        SILA += ARMIA[A, I, TE];
                        SPEED += ARMIA[A, I, TSZ];
                    }
                }

                ARMIA[A, 0, TE] = WOJ;
                SPEED = ((SPEED / WOJ) / 5);
                ARMIA[A, 0, TSZ] = SPEED;
                ARMIA[A, 0, TSI] = SILA;
                AX = ARMIA[A, 0, TX];
                AY = ARMIA[A, 0, TY];
                var TRYB = ARMIA[A, 0, TTRYB];
                var CELX = ARMIA[A, 0, TCELX];
                var CELY = ARMIA[A, 0, TCELY];
                var ROZ = ARMIA[A, 0, TTRYB];
                TEREN = ARMIA[A, 0, TNOGI];
                var WOJ_S = " wojowników";
                if (WOJ == 1) { WOJ_S = " wojownik"; }
                if (ROZ == 0)
                {
                    RO_S = "Oddział obozuje";
                    if (TEREN > 69)
                    {
                        RO_S += " w " + MIASTA_S[TEREN - 70];
                    }
                }
                if (ROZ == 1 || ROZ == 2)
                {
                    RO_S = "Oddział w ruchu";
                }
                if (ROZ == 3)
                {
                    var R2_S = "";
                    if (CELY == 0)
                    {
                        R2_S = ARMIA_S[CELX, 0];
                    }
                    else
                    {
                        R2_S = MIASTA_S[CELX];
                    }
                    RO_S = "Atakujemy " + R2_S;
                }
                if (ROZ == 4)
                {
                    RO_S = "Tropimy zwierzyne";
                }
                var ZARCIE = ARMIA[A, 0, TAMO] / WOJ;
                var DNI_S = "żywność na" + amos.Str_S(ZARCIE) + " dni";
                if (ZARCIE == 1) { DNI_S = "żywność na 1 dzień"; }
                if (ZARCIE == 0) { DNI_S = "Brak żywności !"; }
                screens.Ink(1, 30);
                screens.Text(OKX + 50, OKY + 35, "Siła      :" + amos.Str_S(SILA));
                screens.Text(OKX + 50, OKY + 25, amos.Str_S(WOJ) + WOJ_S);
                screens.Text(OKX + 50, OKY + 45, DNI_S);
                screens.Text(OKX + 50, OKY + 55, "Szybkość  :" + amos.Str_S(SPEED));
                screens.Text(OKX + 12, OKY + 65, RO_S);
            }
            else
            {
                screens.Text(OKX + 25, OKY + 60, D_S);
            }

            var KONIEC = false;

            do
            {
                if (screens.MouseClick() == 1)
                {
                    var STREFA = screens.MouseZone();
                    if (STREFA == 1 || STREFA == 0)
                    {
                        KONIEC = true;
                        ZOKNO();
                    }
                    if (STREFA == 11)
                    {
                        WPISZ(OKX + 50, OKY + 15, 1, 30, 14);
                        ARMIA_S[A, 0] = WPI_S;
                    }
                    if (STREFA == 10 && A < 20)
                    {
                        ZOKNO();
                        KONIEC = true;
                        ARMIA_RYSUJ_ROZKAZY(A, TEREN);
                        var KONIEC2 = false;
                        do
                        {
                            if (screens.MouseClick() == 1)
                            {
                                var STREFA2 = screens.MouseZone();
                                if (STREFA2 > 0 && STREFA2 < 4)
                                {
                                    ZOKNO();
                                    M_RUCH(A, STREFA2);
                                    KONIEC2 = true;
                                    KONIEC = true;
                                }
                                if (STREFA2 == 4 && TEREN < 70)
                                {
                                    ZOKNO();
                                    ARMIA[A, 0, TTRYB] = 4;
                                    KONIEC2 = true;
                                    KONIEC = true;
                                }
                                if (STREFA2 == 4 && TEREN > 69)
                                {
                                    if (MIASTA[TEREN - 70, 0, M_CZYJE] == 1)
                                    {
                                        ZOKNO();
                                        ARMIA[A, 0, TTRYB] = 0;
                                        REKRUTACJA(10, TEREN - 70, A);
                                        ARMIA_RYSUJ_ROZKAZY(A, TEREN);
                                    }
                                }

                                if (STREFA2 == 6)
                                {
                                    ZOKNO();
                                    screens.SpriteOff(2);
                                    _LOAD("dane/gad", 1);
                                    screens.ScreenOpen(1, 320, 160, 32, PixelMode.Lowres);
                                    screens.Screen(1);
                                    //Curs Off : Flash Off 
                                    screens.ReserveZone(60);
                                    screens.GetBobPalette();
                                    screens.SetFont(FON1);
                                    GOBY = 44;
                                    ARM = A;
                                    for (var I = 1; I <= 10; I++)
                                    {
                                        if (ARMIA[A, I, TE] > 0)
                                        {
                                            NUMER = I;
                                            I = 10;
                                        }
                                    }
                                    WYBOR(1);
                                    screens.ScreenClose(1);
                                    for (var I = 1; I <= 50; I++)
                                    {
                                        screens.DelBob(GOBY + 1);
                                    }
                                    screens.Screen(0);
                                    screens.Sprite(2, SPX, SPY, 1);
                                    ARMIA_RYSUJ_ROZKAZY(A, TEREN);
                                }

                                if (STREFA2 == 8)
                                {
                                    KONIEC = true;
                                    KONIEC2 = true;
                                    ARMIA[A, 0, TWAGA] = 1;
                                    ARM = A;
                                    WRG = 40;
                                    screens.SpriteOff(2);
                                    SETUP("Akcja", "w", "terenie");
                                    if (TEREN > 69)
                                    {
                                        var TER2 = MIASTA[TEREN - 70, 1, M_X];
                                        RYSUJ_SCENERIE(TER2, TEREN - 70);
                                        WRG = 40;
                                        //'ustaw wiesniakow
                                        for (var I = 1; I <= 7; I++)
                                        {
                                            NOWA_POSTAC(40, I, 9);
                                        }
                                        for (var I = 8; I <= 10; I++)
                                        {
                                            NOWA_POSTAC(40, I, amos.Rnd(8));
                                        }
                                        for (var I = 1; I <= 7; I++)
                                        {
                                            ARMIA[40, I, TKORP] = 20;
                                        }
                                        for (var I = 8; I <= 10; I++)
                                        {
                                            ARMIA[WRG, I, TKORP] = 40;
                                        }
                                        ARMIA[40, 0, TE] = 10;
                                        USTAW_WOJSKO(WRG, 1, 1, 1);
                                    }
                                    else
                                    {
                                        ARMIA[WRG, 0, TE] = 0;
                                        RYSUJ_SCENERIE(TEREN, -1);
                                    }
                                    USTAW_WOJSKO(ARM, 1, 1, 0);
                                    MAIN_ACTION();
                                    //'skasuj wiesniakow 
                                    for (var I = 0; I <= 10; I++)
                                    {
                                        ARMIA[40, I, TE] = 0;
                                    }
                                    SETUP0();
                                    VISUAL_OBJECTS();
                                    CENTER(AX, AY, 0);
                                    screens.Sprite(2, SPX, SPY, 1);
                                }

                                if (STREFA2 == 5)
                                {
                                    ZOKNO();
                                    ARMIA[A, 0, TTRYB] = 0;
                                    KONIEC = true;
                                    KONIEC2 = true;
                                }

                                if (STREFA2 == 7)
                                {
                                    ZOKNO();
                                    KONIEC2 = true;
                                }
                            }
                        }
                        while (!KONIEC2);
                    }
                    if (STREFA == 10 && A > 19)
                    {
                        if (ARMIA[A, 0, TMAGMA] == 0)
                        {
                            screens.Ink(31, 6);
                            screens.Text(OKX + 48, OKY + 89, "@");
                            ARMIA[A, 0, TMAGMA] = 100;
                            goto SKIP;
                        }
                        if (ARMIA[A, 0, TMAGMA] == 100)
                        {
                            screens.GrWriting(1);
                            screens.Ink(6, 6);
                            screens.Text(OKX + 47, OKY + 89, "  ");
                            ARMIA[A, 0, TMAGMA] = 0;
                        }
                        if (ARMIA[A, 0, TMAGMA] > 0 && ARMIA[A, 0, TMAGMA] < 100)
                        {
                            ZOKNO();
                            KONIEC = true;
                            SZPIEGUJ(A, 1);
                        }
                    SKIP:;
                    }
                }
            }
            while (!KONIEC);

        }

        void ARMIA_RYSUJ_ROZKAZY(int A, int TEREN)
        {
            var AWT = ARMIA[A, 0, TWAGA];
            var WYS = 0;
            if (AWT == 1)
            {
                WYS = 135;
            }
            else
            {
                WYS = 150;
            }
            OKNO(110, 70, 80, WYS);
            GADGET(OKX + 4, OKY + 4, 72, 15, "Ruch", 8, 2, 6, 31, 1);
            GADGET(OKX + 4, OKY + 40, 72, 15, "Atak", 8, 2, 6, 31, 3);
            GADGET(OKX + 4, OKY + 2 + 20, 72, 15, "Szybki Ruch", 8, 2, 6, 31, 2);
            if (TEREN < 70)
            {
                GADGET(OKX + 4, OKY + 58, 72, 15, "Polowanie", 8, 2, 6, 31, 4);
            }
            else
            {
                GADGET(OKX + 4, OKY + 58, 72, 15, "Rekrutacja", 8, 2, 6, 31, 4);
            }
            GADGET(OKX + 4, OKY + 76, 72, 15, "Obóz", 8, 2, 6, 31, 5);
            GADGET(OKX + 4, OKY + 94, 72, 15, "Ekwipunek", 8, 2, 6, 31, 6);
            if (AWT == 0)
            {
                GADGET(OKX + 4, OKY + 112, 72, 15, "Akcja w terenie", 8, 2, 6, 31, 8);
                GADGET(OKX + 4, OKY + 130, 72, 15, "Exit", 8, 2, 6, 31, 7);
            }
            else
            {
                GADGET(OKX + 4, OKY + 112, 72, 15, "Exit", 8, 2, 6, 31, 7);
            }
            var TRYB = ARMIA[A, 0, TTRYB];
            screens.Ink(23, 6);
            if (TRYB > 0)
            {
                screens.Text(OKX + 65, OKY - 4 + 18 * TRYB, "@");
            }
            if (TRYB == 0)
            {
                screens.Text(OKX + 65, OKY - 4 + 18 * 5, "@");
            }
        }

        void USTAW_WOJSKO(int A, int XW, int YW, int TYP)
        {
            var X = 0;
            var Y = 0;
            var X1 = XW * 200;
            var Y1 = YW * 160;
            var I2 = 0;
            for (var I = 1; I <= 10; I++)
            {
                if (ARMIA[A, I, TE] > 0)
                {
                    if (A == WRG)
                    {
                        I2 = I + 10;
                    }
                    if (A == ARM)
                    {
                        I2 = I;
                    }
                    if (TYP == 1)
                    {
                    AGAIN2:
                        var XW2 = amos.Rnd(2);
                        var YW2 = amos.Rnd(2);
                        if (XW2 == XW && YW2 == YW)
                        {
                            goto AGAIN2;
                        }
                        X1 = XW2 * 200;
                        Y1 = YW2 * 160;
                    }
                    if (TYP == 2)
                    {
                        X1 = amos.Rnd(2) * 200;
                    }
                    if (TYP == 3)
                    {
                        Y1 = amos.Rnd(2) * 160;
                    }
                AGAIN:
                    X = amos.Rnd(200) + X1 + 16;
                    Y = amos.Rnd(160) + Y1 + 20;
                    if (screens.Zone(X, Y) == 0)
                    {
                        ARMIA[A, I, TX] = X;
                        ARMIA[A, I, TY] = Y;
                        var BAZA = RASY[ARMIA[A, I, TRASA], 7];
                        ARMIA[A, I, TBOB] = BAZA;
                        if (A == ARM)
                        {
                            var BNR = 0;
                            if (KTO_ATAKUJE == ARM)
                            {
                                BNR = BAZA + 1;
                            }
                            else
                            {
                                BNR = BAZA + 7;
                            }
                            screens.Bob(I2, X, Y, BNR);
                            var STREFA2 = screens.Zone(X, Y + 30);
                            if (STREFA2 > 20 && STREFA2 < 31)
                            {
                                //Limit Bob I2,0,0 To 640,114
                            }
                            else
                            {
                                //Limit Bob I2,0,0 To 640,512
                            }
                        }
                        screens.SetZone(I2, X - 16, Y - 20, X + 16, Y);
                    }
                    else
                    {
                        goto AGAIN;
                    }
                }
            }
        }

        void M_RUCH(int A, int TYP)
        {
            var AX = ARMIA[A, 0, TX];
            var AY = ARMIA[A, 0, TY];
            _GET_XY(1, AX, AY);
            var STREFA = screens.Zone(OX, OY);
            ODL(AX, AY, OX, OY);
            var SPEED = ARMIA[A, 0, TSZ];
            if (TYP == 2) SPEED = SPEED * 2;
            var CZAS = ODLEG / SPEED;
            var DNI_S = "";
            if (CZAS == 0 || CZAS == 1)
            {
                DNI_S = " dzień";
                CZAS = 1;
            }
            else
            {
                DNI_S = " dni";
            }

            if (TYP == 3)
            {
                if (STREFA >= 20 && STREFA < 120)
                {
                    var A_S = "Osiągniemy cel za " + amos.Str_S(CZAS) + DNI_S;
                    ARMIA[A, 0, TTRYB] = TYP;
                    if (STREFA < 61)
                    {
                        if (STREFA - 20 > 19)
                        {
                            ARMIA[A, 0, TCELX] = STREFA - 20;
                            ARMIA[A, 0, TCELY] = 0;
                        }
                        else
                        {
                            ARMIA[A, 0, TTRYB] = 0;
                            A_S = "Nie zaatakujemy naszych !";
                        }
                    }
                    else
                    {
                        if (MIASTA[STREFA - 70, 0, M_CZYJE] != 1)
                        {
                            ARMIA[A, 0, TCELX] = STREFA - 70;
                            ARMIA[A, 0, TCELY] = 1;
                        }
                        else
                        {
                            ARMIA[A, 0, TTRYB] = 0;
                            A_S = "Nie zaatakujemy naszej osady !";
                        }
                    }
                    M_RUCH_INFO(A_S);
                }
                else
                {
                    var A_S = "Kogo mamy zaatakować ?";
                    M_RUCH_INFO(A_S);
                    ARMIA[A, 0, TTRYB] = 0;
                }
            }
            else
            {
                if (OY < 8)
                {
                    OY = 8;
                }
                if (OY > 511)
                {
                    OY = 511;
                }
                if (OX < 4)
                {
                    OX = 4;
                }
                if (OX > 636)
                {
                    OX = 636;
                }

                ARMIA[A, 0, TCELX] = OX;
                ARMIA[A, 0, TCELY] = OY;
                ARMIA[A, 0, TTRYB] = TYP;
                var A_S = "Dotrzemy tam za " + amos.Str_S(CZAS) + DNI_S;
                M_RUCH_INFO(A_S);
            }
        }

        void M_RUCH_INFO(string A_S)
        {
            OKNO(90, 100, 145, 22);
            GADGET(OKX + 4, OKY + 4, 137, 15, "", 31, 2, 30, 1, 0);
            screens.Ink(1, 30);
            screens.Text(OKX + 8, OKY + 15, A_S);
            var KONIEC = false;
            do
            {
                if (screens.MouseClick() == 1)
                {
                    KONIEC = true;
                }
            }
            while (!KONIEC);
            ZOKNO();
        }

        void REKRUTACJA(int ILU, int MIASTO, int A1)
        {
            var REKRUCI = new int[11];
            var JEST = false;
            var KONIEC = false;
            var CENA = 0;
            ILU = 0;
            var A2 = 0;
            var WOJ = 0;

            if (MIASTA[MIASTO, 1, M_PODATEK] == 0)
            {
                var A = 0;
                if (A1 == -1)
                {
                    for (A = 0; A <= 19; A++)
                    {
                        if (ARMIA[A, 0, TE] == 0)
                        {
                            REKRUTACJA_RYSUJ(A, A1, ref A2, ref JEST, MIASTO, REKRUCI);
                            break;
                        }
                    }
                }
                else
                {
                    A = A1;
                    REKRUTACJA_RYSUJ(A, A1, ref A2, ref JEST, MIASTO, REKRUCI);
                }

                if (JEST)
                {
                    do
                    {
                        if (screens.MouseKey() == LEWY)
                        {
                            var STREFA = screens.MouseZone();
                            if (STREFA > 0 && STREFA < 11)
                            {
                                var I = STREFA;
                                if (ARMIA[A2, I, TE] > 0)
                                {
                                    ARMIA[A2, I, TE] = 0;
                                    screens.Ink(4);
                                    screens.Bar(OKX + 145, OKY + 26 + I * 15, OKX + 155, OKY + 36 + I * 15);
                                    CENA += -1000;
                                    WOJ--;
                                    screens.Ink(6);
                                    screens.Bar(OKX + 64, OKY + 193, OKX + 96, OKY + 205);
                                    screens.Ink(31, 6);
                                    screens.Text(OKX + 66, OKY + 202, amos.Str_S(CENA));
                                }
                                else
                                {
                                    ARMIA[A2, I, TE] = ARMIA[A2, I, TEM];
                                    screens.Ink(31, 4);
                                    screens.Text(OKX + 149, OKY + 33 + I * 15, "@");
                                    CENA += 1000;
                                    WOJ++;
                                    screens.Ink(6);
                                    screens.Bar(OKX + 64, OKY + 193, OKX + 96, OKY + 205);
                                    screens.Ink(31, 6);
                                    screens.Text(OKX + 66, OKY + 202, amos.Str_S(CENA));
                                }
                            }
                            if (STREFA == 11)
                            {
                                WPISZ(OKX + 8, OKY + 31, 31, 4, 14);
                                ARMIA_S[A2, 0] = WPI_S;
                            }
                            if (STREFA == 13 && WOJ > 0 && GRACZE[1, 1] - CENA >= 0)
                            {
                                GRACZE[1, 1] += -CENA;
                                ARMIA[A2, 0, TE] = WOJ;
                                MIASTA[MIASTO, 1, M_PODATEK] = 30;
                                ZOKNO();
                                if (A1 == -1)
                                {
                                    ARMIA[A2, 0, TAMO] = 100;
                                    ARMIA[A2, 0, TMAG] = 1;
                                    var XA = MIASTA[MIASTO, 0, M_X];
                                    var YA = MIASTA[MIASTO, 0, M_Y];
                                    ARMIA[A2, 0, TX] = XA;
                                    ARMIA[A2, 0, TY] = YA;
                                    ARMIA[A2, 0, TNOGI] = MIASTO + 70;
                                    ARMIA[A2, 0, TBOB] = 3;
                                    B_DRAW(A2, XA, YA, 3);
                                }
                                KONIEC = true;
                            }
                            if (STREFA == 12)
                            {
                                for (var I = 1; I <= 10; I++)
                                {
                                    if (REKRUCI[I] == 1)
                                    {
                                        ARMIA[A2, I, TE] = 0;
                                    };
                                }
                                ZOKNO();
                                KONIEC = true;
                            }
                            while (screens.MouseKey() == LEWY) { }
                        }

                    }
                    while (!KONIEC);
                }
                else
                {
                    MESSAGE(MIASTO, "Nie możesz dowodzić większą liczbą wojsk.", 0, 1);
                }
            }
            else
            {
                var DNI = MIASTA[MIASTO, 1, M_PODATEK];
                MESSAGE(MIASTO, "Wystawimy nowych wojowników za " + amos.Str_S(DNI) + " Dni.", 0, 1);
            }
        }

        void REKRUTACJA_RYSUJ(int A, int A1, ref int A2, ref bool JEST, int MIASTO, int[] REKRUCI)
        {
            A2 = A;
            JEST = true;
            OKNO(70, 30, 162, 210);
            var A_S = MIASTA_S[MIASTO] + " wystawiło rekrutów.";
            GADGET(OKX + 4, OKY + 4, 154, 15, A_S, 31, 2, 30, 1, -1);
            if (A1 == -1)
            {
                A_S = "Legion " + amos.Str_S(A + 1);
            }
            else
            {
                A_S = ARMIA_S[A, 0];
            }
            GADGET(OKX + 4, OKY + 22, 70, 13, A_S, 7, 0, 4, 31, 11);
            ARMIA_S[A2, 0] = A_S;
            GADGET(OKX + 77, OKY + 22, 70, 13, "En  Si  Sz  Mag", 7, 0, 4, 31, -1);
            GADGET(OKX + 4, OKY + 192, 45, 15, "Odwołać", 7, 0, 4, 31, 12);
            GADGET(OKX + 113, OKY + 192, 45, 15, "   Ok", 7, 0, 4, 31, 13);
            GADGET(OKX + 55, OKY + 192, 50, 15, "", 8, 1, 6, 31, -1);
            for (var I = 1; I <= 10; I++)
            {
                if (A1 > -1 && ARMIA[A, I, TE] <= 0 || A1 == -1)
                {
                    REKRUCI[I] = 1;
                    NOWA_POSTAC(A, I, amos.Rnd(8));
                    ARMIA[A, I, TE] = 0;
                    A_S = ARMIA_S[A, I] + " " + RASY_S[ARMIA[A, I, TRASA]];
                    GADGET(OKX + 4, OKY + 24 + I * 15, 138, 13, A_S, 8, 1, 6, 31, -1);
                    GADGET(OKX + 144, OKY + 24 + I * 15, 15, 13, "", 7, 0, 4, 31, I);
                    screens.Ink(23, 6);
                    screens.Text(OKX + 77, OKY + 34 + I * 15, amos.Str_S(ARMIA[A, I, TEM]));
                    screens.Text(OKX + 95, OKY + 34 + I * 15, amos.Str_S(ARMIA[A, I, TSI]));
                    screens.Text(OKX + 110, OKY + 34 + I * 15, amos.Str_S(ARMIA[A, I, TSZ]));
                    screens.Text(OKX + 125, OKY + 34 + I * 15, amos.Str_S(ARMIA[A, I, TMAG]));
                }
            }
        }


    }
}
