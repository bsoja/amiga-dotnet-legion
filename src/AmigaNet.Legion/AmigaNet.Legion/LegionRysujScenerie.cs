namespace AmigaNet.Legion
{
    public partial class Legion
    {
        void RYSUJ_SCENERIE(int TYP, int WIES)
        {
            SCENERIA = TYP;
            screens.Screen(0);
            screens.ScreenHide(0);
            screens.Cls(20);
            var LX = 20;
            var LY = 20;
            var LSZER = 600;
            var LWYS = 490;
            var D = 0;
            if (WIES == -1)
            {
                D = 1;
            }
            else
            {
                D = 2;
            }
            if (WIES > -1)
            {
                FUNDAMENTY(WIES);
            }

            //'las   
            if (TYP == 1)
            {
                _LOAD("dane/scen-las", 1);
                screens.GetBobPalette();
                _LOAD("dane/muzyka/mus-las", 7);
                //Music 1
                var X = 0;
                var Y = 0;
                for (var I = 1; I <= amos.Rnd(12) + 1; I++)
                {
                    X = amos.Rnd(47);
                    Y = amos.Rnd(3);
                    var R = amos.Rnd(10);
                    var CO = 0;
                    if (R < 5) { CO = 37; }
                    if (R == 5) { CO = 36; }
                    if (R == 6) { CO = 36; }
                    if (R == 7) { CO = 32; }
                    if (R == 8) { CO = 32; }
                    if (R == 9) { CO = 39; } //NOTE: here was C0 (zero) instead of CO
                    if (R == 10) { CO = 0; }
                    GLEBA[X, Y] = CO;
                }

                for (Y = 0; Y <= 11; Y++)
                {
                    for (X = 0; X <= 15; X++)
                    {
                        screens.PasteBob(X * 50, Y * 50, BIBY + 1);
                    }
                }

                WIDOCZNOSC = 150;
                for (var I = 0; I <= 30; I++)
                {
                    X = amos.Rnd(620) + 20;
                    Y = (I * 20);
                    var NR = amos.Rnd(1) + 5;
                    screens.PasteBob(X, Y, BIBY + NR);
                }
                for (var I = 0; I <= 15 / D; I++)
                {
                    var B = amos.Rnd(2) + 2;
                    RYSUJ_SCENERIE_LOSUJ(ref B, ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, BIBY + B);
                    screens.SetZone(60 + I, X + 4, Y + 4, X + 28, Y + 22);
                }
                if (WIES == -1)
                {
                    for (var J = 0; J <= 3; J++)
                    {
                        var X2 = amos.Rnd(640);
                        for (var I = 0; I <= 18; I++)
                        {
                            X = X2 + amos.Rnd(100) - 50;
                            Y = (J * 100) + (I * 4) - 60;
                            var NR = amos.Rnd(2) + 7;
                            screens.PasteBob(X, Y, NR + BIBY);
                        }
                        var ZX1 = X2 - 50;
                        var ZY1 = (J * 100) - 60;
                        var ZX2 = ZX1 + 190;
                        var ZY2 = ZY1 + 130;
                        var ZX3 = ZX1 + 40;
                        var ZX4 = ZX2 - 45;
                        var ZY3 = ZY1 + 130;
                        var ZY4 = ZY1 + 180;
                        if (ZX1 < 0) { ZX1 = 0; }
                        if (ZY1 < 0) { ZY1 = 0; }
                        if (ZX2 > 640) { ZX2 = 640; }
                        if (ZY2 > 512) { ZY2 = 512; }
                        if (ZX3 < 0) { ZX3 = 0; }
                        if (ZY3 < 0) { ZY3 = 0; }
                        if (ZX4 > 640) { ZX4 = 640; }
                        if (ZY4 > 512) { ZY4 = 512; }
                        screens.SetZone(90 + J, ZX1, ZY1, ZX2, ZY2);
                        screens.SetZone(94 + J, ZX3, ZY3, ZX4, ZY4);
                    }
                }
            }

            //'Step
            if (TYP == 2)
            {
                WIDOCZNOSC = 500;
                _LOAD("dane/scen-step", 1);
                screens.GetBobPalette();
                _LOAD("dane/muzyka/mus-step", 7);
                // Music 1
                var X = 0;
                var Y = 0;
                for (var I = 1; I <= amos.Rnd(6) + 1; I++)
                {
                    X = amos.Rnd(47);
                    Y = amos.Rnd(3);
                    var R = amos.Rnd(10);
                    var CO = 0;
                    if (R < 5) { CO = 37; }
                    if (R == 5) { CO = 36; }
                    if (R == 6) { CO = 36; }
                    if (R == 7) { CO = 32; }
                    if (R == 8) { CO = 32; }
                    if (R == 9) { CO = 39; } //NOTE: here was C0 (zero) instead of CO
                    if (R == 10) { CO = 0; }
                    GLEBA[X, Y] = CO;
                }

                for (Y = 0; Y <= 11; Y++)
                {
                    for (X = 0; X <= 15; X++)
                    {
                        screens.PasteBob(X * 50, Y * 50, BIBY + 1);
                    }
                }
                for (var I = 1; I <= 20; I++)
                {
                    var B = BIBY + 2 + amos.Rnd(1);
                    RYSUJ_SCENERIE_LOSUJ(ref B, ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                }
                for (var I = 1; I <= 3; I++)
                {
                    var B = BIBY + 4;
                    RYSUJ_SCENERIE_LOSUJ2(ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                    screens.SetZone(60 + I, X + 4, Y, X + 60, Y + 36);
                }
            }

            // 'ska�y 
            if (TYP == 4)
            {
                WIDOCZNOSC = 250;
                _LOAD("dane/scen-skaly", 1);
                screens.GetBobPalette();
                _LOAD("dane/muzyka/mus-gory", 7);
                //Music 1
                var X = 0;
                var Y = 0;
                for (var I = 1; I <= 50; I++)
                {
                    X = amos.Rnd(47);
                    Y = amos.Rnd(3);
                    GLEBA[X, Y] = 64 + amos.Rnd(4);
                }
                for (Y = 0; Y <= 10; Y++)
                {
                    for (X = 0; X <= 12; X++)
                    {
                        screens.PasteBob(X * 50, Y * 50, BIBY + 1);
                    }
                }
                for (var I = 1; I <= 6; I++)
                {
                    screens.PasteBob(amos.Rnd(600) + 20, amos.Rnd(490) + 20, BIBY + 10);
                }
                for (var I = 1; I <= 50; I++)
                {
                    var B = BIBY + 2 + amos.Rnd(1);
                    RYSUJ_SCENERIE_LOSUJ(ref B, ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                }
                for (var I = 1; I <= 5 / D; I++)
                {
                    var B = BIBY + 7 + amos.Rnd(1);
                    RYSUJ_SCENERIE_LOSUJ(ref B, ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                    screens.SetZone(30 + I, X + 20, Y + 12, X + 90, Y + 37);
                    PLAPKI[I, 0] = 1;
                }
                for (var I = 1; I <= 5 / D; I++)
                {
                    var B = BIBY + 6;
                    RYSUJ_SCENERIE_LOSUJ(ref B, ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                    screens.SetZone(35 + I, X + 16, Y + 10, X + 50, Y + 25);
                    PLAPKI[I + 5, 0] = 1;
                }
                for (var I = 1; I <= 20 / (D * 2); I++)
                {
                    var B = BIBY + 4 + amos.Rnd(1);
                    RYSUJ_SCENERIE_LOSUJ(ref B, ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                    screens.SetZone(60 + I, X, Y, X + 48, Y + 36);
                }
                for (var I = 1; I <= 5 / D; I++)
                {
                    var B = BIBY + 9;
                    RYSUJ_SCENERIE_LOSUJ(ref B, ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                    screens.SetZone(85 + I, X + 10, Y, X + 86, Y + 62);
                }
            }

            //'pustynia
            if (TYP == 3)
            {
                WIDOCZNOSC = 500;
                _LOAD("dane/scen-pustynia", 1);
                screens.GetBobPalette();
                _LOAD("dane/muzyka/mus-pustnia", 7);
                var X = 0;
                var Y = 0;
                var R = 0;
                var CO = 0;
                var B = 0;
                for (var I = 1; I <= amos.Rnd(2) + 1; I++)
                {
                    X = amos.Rnd(47);
                    Y = amos.Rnd(3);
                    R = amos.Rnd(2);
                    if (R == 1) { CO = 35; }
                    if (R == 9) { CO = 39; }
                    GLEBA[X, Y] = CO;
                }

                //Music 1
                for (Y = 0; Y <= 10; Y++)
                {
                    for (X = 0; X <= 12; X++)
                    {
                        screens.PasteBob(X * 50, Y * 50, BIBY + 1);
                    }
                }
                for (var I = 1; I <= 40; I++)
                {
                    B = BIBY + 2 + amos.Rnd(3);
                    RYSUJ_SCENERIE_LOSUJ(ref B, ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                }
                for (var I = 1; I <= 10; I++)
                {
                    B = BIBY + 6;
                    RYSUJ_SCENERIE_LOSUJ(ref B, ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                }
                for (var I = 1; I <= 10 / D; I++)
                {
                    B = BIBY + 7 + amos.Rnd(1);
                    RYSUJ_SCENERIE_LOSUJ(ref B, ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                    screens.SetZone(60 + I, X, Y, X + 48, Y + 35);
                }
                for (var I = 1; I <= 5 / D; I++)
                {
                    B = BIBY + 9;
                AG1:
                    RYSUJ_SCENERIE_LOSUJ(ref B, ref X, ref Y, LX, LY, LSZER, WIES);
                    if (screens.Zone(X + 60, Y + 40) > 0) { goto AG1; }
                    screens.PasteBob(X, Y, B);
                    screens.SetZone(70 + I, X, Y, X + 64, Y + 45);
                }
            }

            //'lodowiec
            if (TYP == 5)
            {
                WIDOCZNOSC = 400;
                _LOAD("dane/scen-lodowiec", 1);
                screens.GetBobPalette();
                _LOAD("dane/muzyka/mus-zima", 7);
                //Music 1
                //'zimno---------------- 
                var X = 0;
                var Y = 0;
                var B = 0;
                for (var I = 1; I <= 10; I++)
                {
                    if (ARMIA[ARM, I, TE] > 0)
                    {
                        if (ARMIA[ARM, I, TNOGI] == 0 && ARMIA[ARM, I, TKORP] == 0)
                        {
                            amos.Add(ref ARMIA[ARM, I, TE], -amos.Rnd(10) - 10, 5, ARMIA[ARM, I, TE]);
                        }
                    }
                }
                //'--------------------- 
                for (Y = 0; Y <= 10; Y++)
                {
                    for (X = 0; X <= 12; X++)
                    {
                        screens.PasteBob(X * 50, Y * 50, BIBY + 1);
                    }
                }
                for (var I = 1; I <= 30; I++)
                {
                    B = BIBY + 2;
                    RYSUJ_SCENERIE_LOSUJ(ref B, ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                }
                for (var I = 1; I <= 15; I++)
                {
                    B = BIBY + 3;
                    RYSUJ_SCENERIE_LOSUJ(ref B, ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                }
                for (var I = 1; I <= 5 / D; I++)
                {
                    B = BIBY + 6;
                    RYSUJ_SCENERIE_LOSUJ(ref B, ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                    screens.SetZone(30 + I, X + 8, Y, X + 60, Y + 40);
                    PLAPKI[I, 0] = 3;
                }
                for (var I = 1; I <= 10 / D; I++)
                {
                    B = BIBY + 4;
                    RYSUJ_SCENERIE_LOSUJ2(ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                    screens.SetZone(65 + I, X, Y, X + 32, Y + 40);
                }
                B = BIBY + 7;
                RYSUJ_SCENERIE_LOSUJ2(ref X, ref Y, LX, LY, LSZER, WIES);
                screens.PasteBob(X, Y, B);
                screens.SetZone(65 + /* I */ (10 / D), X, Y, X + 64, Y + 82);
                if (WIES == -1)
                {
                    for (var J = 0; J <= 3; J++)
                    {
                        var X2 = amos.Rnd(640);
                        for (var I = 0; I <= 10; I++)
                        {
                            X = X2 + amos.Rnd(80) - 40;
                            Y = (J * 100) + (I * 4) - 60;
                            B = BIBY + 5;
                            screens.PasteBob(X, Y, B);
                        }
                        var ZX1 = X2 - 30;
                        var ZY1 = (J * 100) - 50;
                        var ZX2 = ZX1 + 100;
                        var ZY2 = ZY1 + 100;
                        if (ZX1 < 0) { ZX1 = 0; }
                        if (ZY1 < 0) { ZY1 = 0; }
                        if (ZX2 > 640) { ZX2 = 640; }
                        if (ZY2 > 512) { ZY2 = 512; }
                        screens.SetZone(90 + J, ZX1, ZY1, ZX2, ZY2);
                    }
                }
            }

            //'bagna 
            if (TYP == 7)
            {
                WIDOCZNOSC = 250;
                RYSUJ_SCENERIE_OBCINANIE();
                _LOAD("dane/scen-bagno", 1);
                screens.GetBobPalette();
                _LOAD("dane/muzyka/mus-bagna", 7);
                //Music 1
                var X = 0;
                var Y = 0;
                var R = 0;
                var B = 0;
                var CO = 0;
                for (var I = 1; I <= amos.Rnd(10) + 1; I++)
                {
                    X = amos.Rnd(47);
                    Y = amos.Rnd(3);
                    R = amos.Rnd(2);
                    if (R == 1) { CO = 34; }
                    if (R == 9) { CO = 33; }
                    GLEBA[X, Y] = CO;
                }

                for (Y = 0; Y <= 11; Y++)
                {
                    for (X = 0; X <= 15; X++)
                    {
                        screens.PasteBob(X * 50, Y * 50, BIBY + 1);
                    }
                }
                for (var I = 1; I <= 15; I++)
                {
                    B = BIBY + 2 + amos.Rnd(1);
                    RYSUJ_SCENERIE_LOSUJ(ref B, ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                }
                for (var I = 1; I <= 2; I++)
                {
                    B = BIBY + 4;
                    RYSUJ_SCENERIE_LOSUJ2(ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                    screens.SetZone(60 + I, X + 25, Y, X + 54, Y + 20);
                }
                for (var I = 1; I <= 3; I++)
                {
                    B = BIBY + 5;
                    RYSUJ_SCENERIE_LOSUJ2(ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                    screens.SetZone(62 + I, X + 15, Y, X + 35, Y + 30);
                }
                for (var I = 1; I <= 6; I++)
                {
                    B = BIBY + 6;
                    RYSUJ_SCENERIE_LOSUJ(ref B, ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, B);
                    screens.SetZone(65 + I, X + 4, Y, X + 44, Y + 40);
                }
                for (var I = 1; I <= 5; I++)
                {
                    RYSUJ_SCENERIE_LOSUJ2(ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.SetZone(30 + I, X, Y, X + 64, Y + 48);
                    PLAPKI[I, 0] = 2;
                }
            }

            //'jaskinia
            if (TYP == 8)
            {
                WIDOCZNOSC = 100;
                _LOAD("dane/scen-jaskinia", 1);
                screens.GetBobPalette();
                _LOAD("dane/muzyka/mus-jaskinia", 7);
                LX = 40;
                LSZER = 530;
                LY = 75;
                LWYS = 470;
                //Music 1
                var X = 0;
                var Y = 0;
                var CO = 0;
                for (Y = 0; Y <= 6; Y++)
                {
                    for (X = 0; X <= 4; X++)
                    {
                        screens.PasteBob(X * 144, Y * 75, BIBY + 1);
                    }
                }
                //'rozrzucanie z�ota 
                for (var I = 1; I <= amos.Rnd(8) + 2; I++)
                {
                    X = amos.Rnd(29) + 70;
                    Y = amos.Rnd(3);
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
                for (var I = 0; I <= 1; I++)
                {
                    screens.PasteBob(I * 112, 0, BIBY + 10);
                }
                screens.PasteBob(200, -10, BIBY + 9);
                screens.SetZone(60, 0, 0, 230, 70);
                for (var I = 4; I <= 5; I++)
                {
                    screens.PasteBob(I * 112, 0, BIBY + 10);
                };
                //screensManager.PasteBob(420, -10, Hrev(BIBY + 9));
                screens.SetZone(61, 420, 0, 640, 70);
                for (var I = 0; I <= 15; I++)
                {
                    ;
                    screens.PasteBob(0, (I * 30) + amos.Rnd(10), BIBY + 9);
                }
                screens.SetZone(62, 0, 0, 40, 512);
                //B2 = Hrev(BIBY + 9)
                //for (var I = 0; I <= 15; I++)
                //{
                //    screensManager.PasteBob(600, (I * 30) + amos.Rnd(10), B2);
                //}
                screens.SetZone(63, 600, 0, 640, 512);
                for (var I = 1; I <= 40; I++)
                {
                    RYSUJ_SCENERIE_LOSUJ2(ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, BIBY + 2 + amos.Rnd(3));
                }
                for (var I = 1; I <= 10; I++)
                {
                    RYSUJ_SCENERIE_LOSUJ2(ref X, ref Y, LX, LY, LSZER, WIES);
                    screens.PasteBob(X, Y, BIBY + 8);
                    screens.SetZone(64 + I, X, Y, X + 32, Y + 28);
                }
                for (var J = 1; J <= 4; J++)
                {
                    var X2 = amos.Rnd(640);
                    for (var I = 0; I <= 30; I++)
                    {
                        X = X2 + amos.Rnd(60) - 30;
                        Y = (J * 80) + (I * 2) - 60;
                        var B = BIBY + 7;
                        screens.PasteBob(X, Y, B);
                    }
                    var ZX1 = X2 - 15;
                    var ZY1 = (J * 80) - 50;
                    var ZX2 = ZX1 + 65;
                    var ZY2 = ZY1 + 80;
                    if (ZX1 < 0) { ZX1 = 0; }
                    if (ZY1 < 0) { ZY1 = 0; }
                    if (ZX2 > 640) { ZX2 = 640; }
                    if (ZY2 > 512) { ZY2 = 512; }
                    screens.SetZone(80 + J, ZX1, ZY1, ZX2, ZY2);
                }
            }

            //'grobowiec
            if (TYP == 9)
            {
                WIDOCZNOSC = 120;
                _LOAD("dane/scen-grobowiec", 1);
                screens.GetBobPalette();
                _LOAD("dane/muzyka/mus-jaskinia", 7);
                //Music 1
                var X = 0;
                var Y = 0;
                var I = 0;
                var J = 0;
                var RAN = 0;
                for (Y = 0; Y <= 10; Y++)
                {
                    for (X = 0; X <= 10; X++)
                    {
                        screens.PasteBob(X * 60, Y * 52, BIBY + 1);
                    }
                }
                for (I = 1; I <= 50; I++)
                {
                    RYSUJ_SCENERIE_LOSUJ2(ref X, ref Y, LX, LY, LSZER, WIES); ;
                    screens.PasteBob(X, Y, BIBY + 2);
                }
                for (I = 1; I <= 25; I++)
                {
                    RYSUJ_SCENERIE_LOSUJ2(ref X, ref Y, LX, LY, LSZER, WIES); ;
                    screens.PasteBob(X, Y, BIBY + 3);
                }
                for (I = 1; I <= 5; I++)
                {
                    RYSUJ_SCENERIE_LOSUJ2(ref X, ref Y, LX, LY, LSZER, WIES); ;
                    screens.PasteBob(X, Y, BIBY + 8);
                }
                //'rozrzucanie z�ota 
                for (I = 1; I <= amos.Rnd(8) + 2; I++)
                {
                    X = amos.Rnd(99);
                    Y = amos.Rnd(3);
                    var BB = BIBY + 12;
                    var CO = amos.Rnd(3) + 80;
                    GLEBA[X, Y] = CO;
                    var XB = (X % 10) * 64 + amos.Rnd(30);
                    var YB = (X / 10) * 50 + amos.Rnd(20);
                    screens.PasteBob(XB, YB, BB);
                }

                if (PRZYGODY[TRWA_PRZYGODA, P_TYP] == 9)
                {
                    RAN = 55;
                    I = 0;
                }
                else
                {
                    screens.PasteBob(6 * 60, 0, BIBY + 4);
                    screens.SetZone(61, 6 * 60, 0, 6 * 60 + 78, 80);
                    screens.PasteBob(9 * 60, 0, BIBY + 4);
                    screens.SetZone(62, 9 * 60 - 18, 0, 9 * 60 + 60, 80);
                    screens.PasteBob(6 * 60, 0, BIBY + 9);
                    screens.PasteBob(9 * 60, 0, BIBY + 9);
                    screens.PasteBob(7 * 60, 0, BIBY + 6);
                    screens.PasteBob(520, 0, BIBY + 6);
                    screens.PasteBob(7 * 60, 40, BIBY + 6);
                    screens.PasteBob(520, 40, BIBY + 6);

                    screens.PasteBob(452, 30, BIBY + 8);
                    screens.SetZone(60, 6 * 60, 0, 9 * 60 + 60, 180);
                    I = 3;
                    RAN = 45;
                }
                J = 0;
                for (Y = 0; Y <= 20; Y++)
                {
                    for (X = 0; X <= 10; X++)
                    {
                        var LOS = amos.Rnd(RAN);
                        var XB = X * 60;
                        var YB = Y * 25;
                        if (I < 39)
                        {
                            if ((LOS == 1 || LOS == 2) && screens.Zone(XB + 4, YB + 18) != 60)
                            {
                                I++;
                                screens.PasteBob(XB, YB, BIBY + 4);
                                screens.SetZone(60 + I, XB, YB, XB + 60, YB + 80);
                            }

                            if (LOS == 0 && screens.Zone(XB + 4, YB + 18) != 60)
                            {
                                I++;
                                screens.PasteBob(XB, YB, BIBY + 9);
                                screens.SetZone(60 + I, XB, YB, XB + 60, YB + 80);
                            }

                            if (LOS == 3 && screens.Zone(XB + 4, YB + 18) != 60)
                            {
                                I++;
                                screens.PasteBob(XB, YB, BIBY + 5);
                                screens.SetZone(60 + I, XB + 5, YB, XB + 35, YB + 30);
                            }
                            if (LOS == 4 && screens.Zone(XB + 4, YB + 4) == 0)
                            {
                                I++;
                                screens.PasteBob(XB, YB, BIBY + 6);
                                screens.SetZone(60 + I, XB + 5, YB, XB + 18, YB + 38);
                            }
                            if (LOS == 5 && J < 9 && screens.Zone(XB + 4, YB + 4) == 0)
                            {
                                J++;
                                screens.SetZone(30 + J, XB, YB + 14, XB + 34, YB + 38);
                                PLAPKI[J, 0] = 5;
                                PLAPKI[J, 1] = XB;
                                PLAPKI[J, 2] = YB;
                                PLAPKI[J, 3] = XB + 34;
                                PLAPKI[J, 4] = YB + 38;
                            }
                            if (LOS == 7 && J < 9 && screens.Zone(XB + 4, YB + 4) == 0)
                            {
                                J++;
                                screens.SetZone(30 + J, XB + 13, YB + 8, XB + 45, YB + 35);
                                PLAPKI[J, 0] = 4;
                                PLAPKI[J, 1] = XB + 5;
                                PLAPKI[J, 2] = YB + 5;
                                PLAPKI[J, 3] = XB + 48 + 5;
                                PLAPKI[J, 4] = YB + 35;
                            }
                            if (LOS == 6 && screens.Zone(XB + 4, YB + 4) == 0 && amos.Rnd(2) == 0)
                            {
                                var S = SEKTOR(XB + 15, YB + 10);
                                GLEBA[S, amos.Rnd(3)] = amos.Rnd(MX_WEAPON);
                                screens.PasteBob(XB, YB, BIBY + 11);
                            }
                        }
                    }
                }
                screens.ResetZone(60);
            }

            //'grota w�adcy
            if (TYP == 10)
            {
                WIDOCZNOSC = 500;
                _LOAD("dane/scen-grota", 1);
                screens.GetBobPalette();
                _LOAD("dane/muzyka/mus-grota", 7);
                //Shift Down 3,20,23,1
                //Music 1
                var X = 0;
                var Y = 0;
                for (Y = 0; Y <= 8; Y++)
                {
                    for (X = 0; X <= 9; X++)
                    {
                        screens.PasteBob(X * 64, Y * 64, BIBY + 1);
                    }
                }
                for (Y = 2; Y <= 8; Y++)
                {
                    for (X = 2; X <= 7; X++)
                    {
                        screens.PasteBob(X * 64, Y * 40, BIBY + 2);
                    }
                }
                for (Y = 1; Y <= 4; Y++)
                {
                    screens.PasteBob(128, Y * 64, BIBY + 3);
                    //screensManager.PasteBob(496, Y * 64, Hrev(BIBY + 3));
                }
                for (X = 2; X <= 7; X++)
                {
                    screens.PasteBob(X * 64, 64, BIBY + 4);
                    screens.PasteBob(X * 64, 304, BIBY + 5);
                }
                for (Y = 0; Y <= 12; Y++)
                {
                    X = -amos.Rnd(20);
                    screens.PasteBob(X, amos.Rnd(10) + 20 + Y * 40, BIBY + 7);
                    screens.PasteBob(X, Y * 40, BIBY + 6);

                    X = amos.Rnd(20);
                    //screensManager.PasteBob(X + 560, amos.Rnd(10) + 20 + Y * 40, Hrev(BIBY + 7));
                    //screensManager.PasteBob(X + 560, Y * 40, Hrev(BIBY + 6));

                }

                var XB = 100;
                var YB = 322;
                screens.SetZone(31, XB, YB, XB + 420, YB + 100);
                PLAPKI[1, 0] = 6;
                PLAPKI[1, 1] = XB;
                PLAPKI[1, 2] = YB;
                PLAPKI[1, 3] = XB + 34;
                PLAPKI[1, 4] = 115;

                XB = 100;
                YB = 0;
                screens.SetZone(32, XB, YB, XB + 420, YB + 63);
                PLAPKI[2, 0] = 6;
                PLAPKI[2, 1] = XB;
                PLAPKI[2, 2] = YB;
                PLAPKI[2, 3] = XB + 34;
                PLAPKI[2, 4] = 0;

                XB = 0;
                YB = 0;
                screens.SetZone(33, XB, YB, XB + 118, YB + 340);
                PLAPKI[3, 0] = 6;
                PLAPKI[3, 1] = XB;
                PLAPKI[3, 2] = YB;
                PLAPKI[3, 3] = XB + 34;
                PLAPKI[3, 4] = 85;

                XB = 522;
                YB = 0;
                screens.SetZone(34, XB, YB, XB + 100, YB + 340);
                PLAPKI[4, 0] = 6;
                PLAPKI[4, 1] = XB;
                PLAPKI[4, 2] = YB;
                PLAPKI[4, 3] = XB + 34;
                PLAPKI[4, 4] = 85;
            }

            if (WIES >= 0)
            {
                RYSUJ_WIES(WIES);
                //'doda� domy (9)
                BSIBY = BIBY + 12 + 9;
            }
            if (WIES < -1)
            {
                RYSUJ_MUR(WIES);
                //'doda� mury (2)  
                BSIBY = BIBY + 12 + 2;
            }

            //'wytnij strza�y
            screens.Screen(2);
            screens.Cls(0);
            for (var I = 1; I <= 20; I++)
            {
                //Get Bob BSIBY + I,0,0 , 31,31
            }
            screens.Screen(0);
            screens.ScreenShow(0);

            //while (true) Thread.Sleep(100);
        }

        void RYSUJ_SCENERIE_LOSUJ(ref int B, ref int X, ref int Y, int LX, int LY, int LSZER, int WIES)
        {
            if (amos.Rnd(1) == 0)
            {
                //B = Hrev(B);
            }
            RYSUJ_SCENERIE_LOSUJ2(ref X, ref Y, LX, LY, LSZER, WIES);
        }

        void RYSUJ_SCENERIE_LOSUJ2(ref int X, ref int Y, int LX, int LY, int LSZER, int WIES)
        {
            X = LX + amos.Rnd(LSZER);
            if (WIES < -1)
            {
                Y = amos.Rnd(300) + 200;
            }
            else
            {
                Y = LY + amos.Rnd(LSZER);
            }
            if (screens.Zone(X, Y) > 120)
            {
                RYSUJ_SCENERIE_LOSUJ2(ref X, ref Y, LX, LY, LSZER, WIES);
            }
        }

        void RYSUJ_SCENERIE_OBCINANIE()
        {
            screens.ScreenHide(1);
            screens.ScreenHide(2);
            screens.View();
            screens.Screen(2);
            for (var I = BUBY + 4; I <= BUBY + 163; I++)
            {
                screens.Cls(0);
                screens.PasteBob(0, 0, I);
                screens.GetSprite(I, 0, 0, 32, 32);
                screens.WaitVbl();
                screens.HotSpot(I, 12);
            }
            screens.ScreenShow(1);
            screens.Screen(0);
        }

        void FUNDAMENTY(int NR)
        {
            for (var I = 2; I <= 20; I++)
            {
                var TYP = MIASTA[NR, I, M_LUDZIE];
                if (TYP > 0)
                {
                    var X = MIASTA[NR, I, M_X];
                    var Y = MIASTA[NR, I, M_Y];
                    var SZER = BUDYNKI[TYP, 0];
                    var WYS = BUDYNKI[TYP, 1];
                    screens.SetZone(120 + I, X, Y, X + SZER, Y + WYS + 40);
                }
            }
        }

        void RYSUJ_WIES(int NR)
        {
            WIDOCZNOSC = 250;
            var SOR = new int[7];
            _LOAD("dane/scen-domy", 1);
            for (var I = 2; I <= 20; I++)
            {
                var TYP = MIASTA[NR, I, M_LUDZIE];
                if (TYP > 0)
                {
                    var X = MIASTA[NR, I, M_X];
                    var Y = MIASTA[NR, I, M_Y];

                    var SZER = BUDYNKI[TYP, 0];
                    var WYS = BUDYNKI[TYP, 1];
                    var B = BUDYNKI[TYP, 4];
                    var DR = BUDYNKI[TYP, 6];
                    screens.PasteBob(X, Y, BIBY + 12 + B);
                    screens.SetZone(120 + I, X, Y, X + SZER, Y + WYS);
                    if (DR > 0)
                    {
                        screens.SetZone(100 + I, X + DR, Y + WYS - 32, X + DR + 32, Y + WYS);

                        var RANGA = MIASTA[NR, I, M_CZYJE];
                        if (TYP == 4)
                        {
                            SOR[0] = 1; SOR[1] = 2; SOR[2] = 3; SOR[3] = 4; SOR[4] = 5; SOR[5] = 7; SOR[6] = 13;
                        }
                        if (TYP == 5)
                        {
                            SOR[0] = 1; SOR[1] = 3; SOR[2] = 2; SOR[3] = 2; SOR[4] = 11; SOR[5] = 7; SOR[6] = 6;
                        }
                        if (TYP == 6)
                        {
                            SOR[0] = 12; SOR[1] = 12; SOR[2] = 12; SOR[3] = 12; SOR[4] = 12; SOR[5] = 12; SOR[6] = 12;
                        }
                        if (TYP == 7)
                        {
                            SOR[0] = 4; SOR[1] = 5; SOR[2] = 8; SOR[3] = 9; SOR[4] = 10; SOR[5] = 15; SOR[6] = 16;
                        }
                        if (TYP == 8)
                        {
                            SOR[0] = 13; SOR[1] = 13; SOR[2] = 13; SOR[3] = 18; SOR[4] = 18; SOR[5] = 18; SOR[6] = 18;
                        }
                        if (TYP == 9)
                        {
                            SOR[0] = 17; SOR[1] = 17; SOR[2] = 17; SOR[3] = 17; SOR[4] = 17; SOR[5] = 17; SOR[6] = 17;
                        }
                        var S = 0;
                        if (TYP < 9)
                        {
                            for (Y = 0; Y <= 1; Y++)
                            {
                                for (X = 0; X <= 9; X++)
                                {
                                    if (amos.Rnd(3) == 1)
                                    {
                                        SKLEP[I, S] = 0;
                                    }
                                    else
                                    {
                                    AGAIN:
                                        var BR = amos.Rnd(MX_WEAPON);
                                        for (B = 0; B <= 6; B++)
                                        {
                                            if (BRON[BR, B_TYP] == SOR[B] && BRON[BR, B_CENA] < 1000)
                                            {
                                                goto JEST;
                                            }
                                        }
                                        goto AGAIN;
                                    JEST:
                                        SKLEP[I, S] = BR;
                                    }
                                    S++;
                                }
                            }
                        }
                    }
                }
            }
        }

        void RYSUJ_MUR(int TYP)
        {
            WIDOCZNOSC = 400;
            var SILA = 0;
            if (TYP == -2)
            {
                _LOAD("dane/mur2", 1);
                SILA = 220;
            }
            if (TYP == -3)
            {
                _LOAD("dane/mur1", 1);
                SILA = 300;
            }
            if (TYP == -4)
            {
                _LOAD("dane/mur3", 1);
                SILA = 400;
            }

            for (var I = 0; I <= 9; I++)
            {
                screens.PasteBob(I * 64, 10, BIBY + 12 + 1);
                screens.SetZone(21 + I, I * 64, 140, (I * 64) + 64, 200);
                MUR[I] = SILA;
            }
        }

    }
}
