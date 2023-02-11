using AmigaNet.Amos;
using AmigaNet.Amos.MemoryBanks;
using AmigaNet.Amos.Screens;
using AmigaNet.Amos.Screens.Amal;
using System.Text;

namespace AmigaNet.Legion
{
    public partial class Legion
    {
        private const char UpArrowChar = (char)139;
        private const char DownArrowChar = (char)155;

        private const int KeyEscape = 27;
        private const int KeyEnter = 13;
        private const int KeyBackspace = 8;
        private const int KeyLeft = 37;
        private const int KeyUp = 38;
        private const int KeyRight = 39;        
        private const int KeyDown = 40;
        private const int KeyF1 = 112;
        private const int KeyF2 = 113;
        private const int KeyF3 = 114;
        private const int KeyF4 = 115;
        private const int KeyF5 = 116;
        private const int KeyF6 = 117;
        private const int KeyF7 = 118;
        private const int KeyF8 = 119;
        private const int KeyF9 = 120;
        private const int KeyF10 = 121;

        private readonly AmosBase amos;
        private readonly ScreensManager screens;
        private readonly MemoryBanksManager banks;

        public Legion(String resourcesPath, String dataPath, IGameEngine gameEngine)
        {
            KAT_S = dataPath;
            amos = new AmosBase(resourcesPath, gameEngine);
            banks = new MemoryBanksManager(gameEngine);
            screens = new ScreensManager(dataPath, banks, gameEngine);
        }

        public IScreensManager ScreensManager => screens;

        public void Run()
        {
            PREFS[1] = 1;
            PREFS[2] = 1;
            PREFS[4] = 1;

            WCZYTAJ_BRON();
            WCZYTAJ_RASY();
            WCZYTAJ_BUDYNKI();
            WCZYTAJ_ROZMOWE();
            WCZYTAJ_PRZYGODY();
            WCZYTAJ_GULE();

            _INTRO();
            EKRAN_WYBOR();

            var KONIEC = false;
            do
            {
                var A_S = "";
                var K1 = 0;
                var K2 = 0;

                if (screens.MouseZone() == 21)
                {
                    //Rainbow 1,3,140,23
                    //Rainbow 2,3,163,23
                    //Rainbow 3,3,187,23
                    screens.Bob(1, -1, 190, 1);
                    screens.View();
                    screens.BobUpdate();
                    screens.WaitVbl();
                    if (screens.MouseClick() == 1)
                    {
                        //Rainbow Del;
                        screens.View();
                        //Fade 1;
                        amos.Wait(15);
                        screens.ScreenOpen(0, 192, 200, 32, PixelMode.Lowres);
                        //Flash Off;
                        //Curs Off;
                        screens.Cls(0);
                        //Double Buffer;
                        screens.BobUpdateOn();
                        //Priority Off
                        screens.ScreenDisplay(0, 200, -1, -1, -1);
                        screens.GetBobPalette();
                        //Colour 3,$462
                        screens.Colour(3, 40, 60, 20);
                        //Set Rainbow 0,1,528,"(3,1,1)","(3,1,1)","(3,1,1)"
                        screens.Bob(1, 0, 60, 2);
                        screens.Bob(2, 0, 60, 3);
                        screens.BobUpdate();
                        //Rainbow 0,20,110,26

                        //A_S = "S: Move -128,0,387; Move 0,0,50; Move 128,0,387; Move 0,0,50; Jump S"
                        //B_S = "S: Move -387,0,387; Move 0,0,50; Move 387,0,387; Move 0,0,50; Jump S"
                        //Channel 1 To Bob 1;
                        //Channel 2 To Bob 2
                        //Amal 1,A_S;
                        //Amal 2,B_S
                        //Amal On 2;
                        //Amal On 1

                        screens.AmalRun(1, 1, new AmalBuilder()
                            .Label("S")
                            .Move(-128, 0, 387)
                            .Move(0, 0, 50)
                            .Move(128, 0, 387)
                            .Move(0, 0, 50)
                            .Jump("S")
                            .Compile());

                        screens.AmalRun(2, 2, new AmalBuilder()
                            .Label("S")
                            .Move(-387, 0, 387)
                            .Move(0, 0, 50)
                            .Move(387, 0, 387)
                            .Move(0, 0, 50)
                            .Jump("S")
                            .Compile());

                        OKX = 0;
                        OKY = 0;
                        screens.SetFont(FON2);
                        screens.Ink(28, 0);
                        screens.Text(OKX + 10, OKY + 20, "Podaj swoje imię:");
                        screens.View();
                        WPISZ(OKX + 30, OKY + 40, 28, 0, 12);
                        if (WPI_S != "")
                        {
                            IMIONA_S[1] = WPI_S;
                        }
                        else
                        {
                            var res = ROB_IMIE();
                            IMIONA_S[1] = res;
                        }
                        for (var I = 1; I <= 3; I++)
                        {
                            screens.Ink(0);
                            screens.Bar(OKX + 2, OKY + 2, 200, 70);
                            screens.Ink(28, 0);
                            screens.Text(OKX + 2, OKY + 20, "Imię" + amos.Str_S(I) + "-go przeciwnika");
                            WPISZ(OKX + 30, OKY + 40, 28, 0, 12);
                            if (WPI_S != "")
                            {
                                IMIONA_S[I + 1] = WPI_S;
                            }
                            else
                            {
                                var res = ROB_IMIE();
                                IMIONA_S[I + 1] = res;
                            }
                        }
                        //Rainbow Del;
                        screens.View();
                        //Fade 2;
                        _TRACK_FADE(1);
                        screens.BobUpdateOff();
                        screens.AmalOff();
                        DZIEN = 1;
                        POWER = 5;
                        SX = 0;
                        SY = 0;
                        for (var I = 1; I <= 4; I++)
                        {
                            GRACZE[I, 1] = 5000;
                        }
                        GRACZE[1, 3] = 20;
                        GRACZE[2, 3] = 16;
                        GRACZE[3, 3] = 18;
                        GRACZE[4, 3] = 22;
                        PREFS[1] = 1;
                        PREFS[2] = 1;
                        PREFS[4] = 1;
                        screens.ScreenClose(0);
                        SETUP0();
                        ZROB_MIASTA();
                        ZROB_ARMIE();
                        screens.View();
                        MAIN();
                        EKRAN_WYBOR();
                    }
                }
                if (screens.MouseZone() == 22)
                {
                    //Rainbow 2,3,140,23
                    //Rainbow 3,3,187,23
                    //Rainbow 1,3,163,23
                    screens.Bob(1, -1, 237, 1);
                    screens.View();
                    screens.BobUpdate();
                    screens.WaitVbl();
                    if (screens.MouseClick() == 1)
                    {
                        OKX = 320;
                        OKY = 180;
                        screens.GetBlock(100, OKX, OKY, 176, 180);
                        screens.Ink(0);
                        screens.Bar(320, 180, 480, 350);
                        screens.Ink(7);
                        screens.Box(320, 180, 480, 350);
                        screens.Ink(5);
                        screens.Box(321, 181, 479, 349);
                        screens.SetFont(FON2);
                        screens.GrWriting(0);
                        GADGET(OKX + 10, OKY + 8, 120, 15, A_S, K1, 0, K2, 31, -1);

                        for (var I = 0; I <= 4; I++)
                        {
                            var NAME_S = "";
                            var fileName = Path.Combine(SAVE_FOLDER_NAME, "zapis" + amos.Str_S(I + 1));
                            if (File.Exists(fileName))
                            {
                                var bytes = File.ReadAllBytes(fileName);
                                var nameBytes = new Byte[20];
                                Array.Copy(bytes, 0, nameBytes, 0, 20);
                                NAME_S = Encoding.UTF8.GetString(nameBytes);
                            }
                            else
                            {
                                NAME_S = "Pusty Slot";
                            }
                            screens.Ink(7);
                            screens.Text(OKX + 20, OKY + 28 + (I * 25), NAME_S);
                            screens.SetZone(1 + I, 0, 0, 640, OKY + 28 + (I * 25));
                        }
                        screens.Text(OKX + 20, OKY + 28 + (5 * 25), "Exit");
                        screens.SetZone(6, 0, 200, 640, 500);
                        var KONIEC2 = false;
                        do
                        {
                            var STREFA = screens.MouseZone();
                            screens.Bob(1, -1, OKY + ((STREFA - 1) * 25), 1);
                            screens.BobUpdate();
                            screens.WaitVbl();
                            if (screens.MouseClick() == 1)
                            {
                                if (STREFA == 6)
                                {
                                    KONIEC2 = true;
                                    for (var I = 1; I <= 19; I++)
                                    {
                                        screens.ResetZone(I);
                                    }
                                    screens.PutBlock(100);
                                    //Screen Swap;
                                    screens.PutBlock(100);
                                }
                                var fileName = Path.Combine(SAVE_FOLDER_NAME, "zapis" + amos.Str_S(STREFA));
                                if (STREFA > 0 && STREFA < 6 && File.Exists(fileName))
                                {
                                    KONIEC2 = true;
                                    var NSAVE = STREFA;
                                    Load(fileName);
                                    screens.DelBlock();
                                    //Rainbow Del;
                                    screens.View();
                                    //Fade 2;
                                    _TRACK_FADE(1);
                                    screens.ScreenClose(0);
                                    SX = 0;
                                    SY = 0;
                                    SETUP0();
                                    VISUAL_OBJECTS();
                                    MAIN();
                                    EKRAN_WYBOR();
                                }
                            }
                        } while (!KONIEC2);
                    }
                }
                if (screens.MouseZone() == 23)
                {
                    //Rainbow 2,3,140,23
                    //Rainbow 3,3,163,23
                    //Rainbow 1,3,187,23
                    screens.Bob(1, -1, 285, 1);
                    screens.View();
                    screens.BobUpdate();
                    screens.WaitVbl();
                    if (screens.MouseClick() == 1)
                    {
                        KONIEC = true;
                    }
                }
            } while (!KONIEC);
            //Rainbow Del;
            screens.View();
            //Fade 2;
            _TRACK_FADE(1);
            screens.ScreenClose(0);
            banks.EraseAll();
            //Led On
            //Edit
        }

        void _INTRO()
        {
            //return;
            screens.AutoViewOff();
            //screens.SetSpriteBuffer(300);
            screens.ScreenOpen(2, 64, 10, 32, PixelMode.Lowres);
            //Flash Off;
            screens.ScreenHide();
            screens.ScreenOpen(3, 64, 10, 32, PixelMode.Lowres);
            //Flash Off;
            screens.ScreenHide();
            screens.ScreenOpen(0, 640, 512, 16, PixelMode.HiresAndLaced);
            //Flash Off;
            screens.Cls(0);
            screens.ScreenHide();
            screens.HideOn();//Hide
            
            _LOAD("miecz", 0);
            _LOAD("mod.intro", 6);
            amos.TrackLoopOn();
            //Led Off
            USTAW_FONT("bodacious", 42);
            screens.GrWriting(0);
            //Flash Off;
            screens.GetBobPalette();
            for (var I = 0; I <= 15; I++)
            {
                var pixel = screens.Colour(I);
                screens.Colour(I+16, pixel.R, pixel.G, pixel.B);
                //Colour I+16,Colour(I);
            }

            byte r = 0;
            byte g = 0;
            byte b = 0;
            for (var I = 0; I <= 15; I++)
            {
                screens.Colour(I, r, g, b);
                r += 20;
                g += 20;
                b += 20;
            }
            screens.Screen(2);
            screens.GetPalette(0);
            screens.ScreenOpen(1, 640, 512, 16, PixelMode.HiresAndLaced);
            screens.Cls(0);
            //Flash Off 
            screens.Screen(1);
            screens.GetPalette(0);
            screens.ScreenHide();
            screens.HideOn();//Hide
            screens.ScreenDisplay(1, 150, -1, -1, -1);
            screens.AutoViewOn();
            _LOAD("title2", 2);
            for (var I = 16; I <= 31; I++)
            {
                screens.Screen(0);
                var KOL = screens.Colour(I);
                screens.Screen(1);
                screens.Colour(I, KOL.R, KOL.G, KOL.B);
            }
            screens.Screen(3);
            screens.GetPalette(1);
            screens.Screen(0);
            _LOAD("gobilog.16", 2);
            screens.ScreenShow(0);
            screens.View();
            amos.TrackPlay(3);
            //Amos To Front;
            screens.View();
            _WAIT(150);
            //Fade 2;
            amos.Wait(30);
            screens.Cls(0);
            for (var I = 0; I <= 16; I++)
            {
                //Colour I,0;
                screens.Colour(I, 0, 0, 0);
            }
            var X1 = 200;
            var Y1 = 235;
            var A_S = "przedstawia";
            var X2 = 130;
            var Y2 = 270;
            var B_S = "";
            _INTRO_FADING(A_S, B_S, X1, Y1, X2, Y2);

            screens.Sprite(0, 365, 70, 1);
            //A_S = "Anim 0,(1,3)(2,3)(3,3)(4,3)(5,3)(6,3)(7,3)(8,3)(9,3)(10,3)(11,3)(12,3)(13,3)(14,3)(15,3)(16,3)(17,3)(18,3)(19,3);"
            //Amal 0,A_S
            do { } while (!(amos.TIMER % 3 == 0));
            //Amal On 0
            screens.Screen(1);
            for (var I = 0; I <= 16; I++)
            {
                screens.Colour(I, 0, 0, 0);
            }

            do
            {
                screens.Screen(1);
                screens.ScreenToFront(1);
                screens.ScreenShow(1);
                //Fade 2 To 3
                _WAIT(200);
                if (KONIEC_INTRA > 2) break;
                //Fade 2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
                _WAIT(30);
                if (KONIEC_INTRA > 2) break;
                screens.Screen(0);
                screens.ScreenDisplay(0, 140, -1, -1, -1);
                for (var I = 0; I <= 16; I++)
                {
                    screens.Colour(I, 0, 0, 0);
                }

                screens.ScreenToFront(0);
                screens.ScreenShow();
                X1 = 130;
                Y1 = 220;
                A_S = "   program   ";
                X2 = 130;
                Y2 = 270;
                B_S = "Marcin Puchta";
                _INTRO_FADING(A_S, B_S, X1, Y1, X2, Y2);
                if (KONIEC_INTRA > 2) break;
                X1 = 135;
                Y1 = 220;
                A_S = "   grafika";
                X2 = 130;
                Y2 = 270;
                B_S = "Andrzej Puchta";
                _INTRO_FADING(A_S, B_S, X1, Y1, X2, Y2);
                if (KONIEC_INTRA > 2) break;
                X1 = 135;
                Y1 = 220;
                A_S = "muzyka i sfx";
                X2 = 130;
                Y2 = 270;
                B_S = "Marcin Puchta";
                _INTRO_FADING(A_S, B_S, X1, Y1, X2, Y2);
                if (KONIEC_INTRA > 2) break;
                X1 = 135;
                Y1 = 240;
                A_S = "  pomagali";
                X2 = 130;
                Y2 = 270;
                B_S = "";
                _INTRO_FADING(A_S, B_S, X1, Y1, X2, Y2);
                if (KONIEC_INTRA > 2) break;
                X1 = 135;
                Y1 = 220;
                A_S = "M.Malachowski";
                X2 = 130;
                Y2 = 270;
                B_S = " Marek Lech";
                _INTRO_FADING(A_S, B_S, X1, Y1, X2, Y2);
                if (KONIEC_INTRA > 2) break;

                KONIEC_INTRA = 2;
            } while (!(KONIEC_INTRA > 2));

            //Fade 2
            amos.Wait(25);
            screens.AmalOff();
            screens.SpriteOff();
            screens.ScreenClose(2);
            screens.ScreenClose(3);
            if (screens.Screen() == 0)
            {
                screens.ScreenClose(1);
                screens.ScreenClose(0);
            }
            else
            {
                screens.ScreenClose(0);
                screens.ScreenClose(1);
            }
            banks.Erase(1);
            screens.AutoViewOff();
        }

        void _INTRO_FADING(String A_S, String B_S, int X1, int Y1, int X2, int Y2)
        {
            //screens.Colour(28, 238, 238, 238); //not included in original Legion game, added to show text correctly
            //screens.Ink(28); //not included in original Legion game, added to show text correctly
            do { } while (!(amos.TIMER % 3 == 1));
            screens.Text(X1, Y1, A_S);
            do { } while (!(amos.TIMER % 3 == 1));
            screens.Text(X2, Y2, B_S);
            //Fade 2 To 2;
            _WAIT(100);
            //Fade 2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0;
            amos.Wait(25);
            do { } while (!(amos.TIMER % 3 == 0));
            screens.Ink(0);
            screens.Bar(130, 190, 435, 280);
        }

        void EKRAN_WYBOR()
        {
            screens.ScreenOpen(0, 640, 512, 16, PixelMode.HiresAndLaced);
            screens.HideOn();
            _LOAD("kam.pic", 3);
            _LOAD("mieczyk", 0);
            _LOAD("intro", 1);
            USTAW_FONT("garnet", 16);
            FON2 = FONR;
            USTAW_FONT("defender2", 8);
            FON1 = FONR;
            screens.ReserveZone(25);
            screens.SetZone(21, 0, 0, 640, 238);
            screens.SetZone(22, 0, 238, 640, 285);
            screens.SetZone(23, 0, 285, 640, 500);
            //Set Rainbow 2,6,256,"(2,-1,1)","(2,-1,1)","(3,-1,1)"
            //Set Rainbow 3,6,256,"","(2,-1,1)","(3,-1,1)"
            //Set Rainbow 1,6,256,"(3,-1,1)","(2,-1,1)",""
            //Double Buffer
            //Autoback 1
            screens.BobUpdateOff();
            screens.Bob(1, 50, 190, 1);
            screens.BobUpdate();
            screens.WaitVbl();
            //A_S = "E:Move 4,0,4; Move 4,0,3; Move 4,0,2; Move 16,0,6; Move 4,0,2; Move 4,0,3; Move 4,0,4;"
            //A_S = A_S + "Move -4,0,4; Move -4,0,3; Move -4,0,2; Move -16,0,6; Move -4,0,2; Move -4,0,3; Move -4,0,4;Jump E"
            //Channel 1 To Bob 1
            //Amal 1,A_S
            //Amal On 1

            screens.AmalRun(0, 1, new AmalBuilder()
                            .Label("E")
                            .Move(4, 0, 4)
                            .Move(4, 0, 3)
                            .Move(4, 0, 2)
                            .Move(16, 0, 6)
                            .Move(4, 0, 2)
                            .Move(4, 0, 3)
                            .Move(4, 0, 4)
                            .Move(-4, 0, 4)
                            .Move(-4, 0, 3)
                            .Move(-4, 0, 2)
                            .Move(-16, 0, 6)
                            .Move(-4, 0, 2)
                            .Move(-4, 0, 3)
                            .Move(-4, 0, 4)
                            .Jump("E")
                            .Compile());

            //Limit Mouse 150,140 To 150,205
            screens.View();
            screens.WaitVbl();
        }

        void _LOAD(String A, int TRYB)
        {
            var fileName = Path.Combine(KAT_S, A);
            
            /* Sprites always bank 1 */
            if (TRYB == 0) banks.Load(fileName);
            if (TRYB == 1) banks.Load(fileName, 1);
            if (TRYB == 2) screens.LoadIff(fileName);
            if (TRYB == 3) screens.LoadIff(fileName, 0);
            if (TRYB == 4) screens.LoadIff(fileName, 1);
            if (TRYB == 5) screens.LoadIff(fileName, 2);
            if (TRYB == 6) banks.TrackLoad(fileName, 3);
            if (TRYB == 7) ;//TODO
            if (TRYB == 8) ;//TODO
            if (TRYB == 9) ;//TODO

            /*
             Procedure _LOAD[A$,B$,NAPI$,TRYB]
               If TRYB=0 : Load A$ : End If 
               If TRYB=1 : Load A$,1 : End If 
               If TRYB=6 : Track Load A$,3 : End If 
               If TRYB=7 : Load A$,3 : End If 
               If TRYB=8 : Load A$,4 : End If 
               If TRYB=9 : Load A$,5 : End If 
   
            End Proc
             */
        }

        void SETUP0()
        {
            banks.EraseAll();
            _LOAD("mapa2.org", 3);
            screens.ScreenDisplay(0, 130, 40, 320, 255);
            screens.ReserveZone(130);
            //Limit Mouse 128,40 To 447,298
            screens.BobUpdateOn();
            screens.SpriteUpdateOn();
            //Flash Off 
            USTAW_FONT("garnet", 16);
            FON2 = FONR;
            USTAW_FONT("defender2", 8);
            FON1 = FONR;
            _LOAD("mapa", 0);
            screens.ChangeMouse(5);
            CELOWNIK = 43;
            screens.ShowOn();
            BUBY = -1;
            SPX = 425;
            SPY = 270;
            MSX = 320;
            MSY = 256;
            screens.Sprite(2, SPX, SPY, 1);
            if (PREFS[3] == 1)
            {
                _LOAD("mod.legion", 6);
                amos.TrackPlay();
                amos.TrackLoopOn();
            }
        }

        void USTAW_FONT(String FONTNAME, int WIEL)
        {
            var I = 0;

            if (FONTNAME == "garnet")
            {
                I = 1;
            }
            if (FONTNAME == "defender2")
            {
                I = 2;
            }
            if (FONTNAME == "bodacious")
            {
                I = 3;
            }

            FONR = I;
            FONT = WIEL;
            screens.SetFont(I);
        }

        void VISUAL_OBJECTS()
        {
            for (var I = 0; I <= 49; I++)
            {
                var CZYJE = MIASTA[I, 0, M_CZYJE];
                var B1 = 8 + CZYJE * 2;
                var LUD = MIASTA[I, 0, M_LUDZIE];
                if (LUD > 700)
                {
                    B1++;
                }
                var X = MIASTA[I, 0, M_X];
                var Y = MIASTA[I, 0, M_Y];
                screens.PasteBob(X - 8, Y - 8, B1);
                screens.SetZone(70 + I, X - 8, Y - 8, X + 8, Y + 8);
            }

            for (var A = 0; A <= 39; A++)
            {
                if (ARMIA[A, 0, TE] > 0)
                {
                    var X = ARMIA[A, 0, TX];
                    var Y = ARMIA[A, 0, TY];
                    var B1 = ARMIA[A, 0, TBOB];
                    B_DRAW(A, X, Y, B1);
                }
            }

            for (var I = 0; I <= 3; I++)
            {
                if (PRZYGODY[I, P_TYP] > 0 && PRZYGODY[I, P_LEVEL] == 0)
                {
                    var X = PRZYGODY[I, P_X];
                    var Y = PRZYGODY[I, P_Y];
                    var X2 = X;
                    var Y2 = Y;

                    //LOSUJ:
                    while (true)
                    {
                        if (screens.Zone(X2, Y2) != 0 || X2 < 8 || X2 > 630 || Y2 < 10 || Y2 > 500)
                        {
                            X2 = X + amos.Rnd(60) - 30;
                            Y2 = Y + amos.Rnd(60) - 30;
                        }
                        else
                        {
                            break;
                        }
                    }

                    PRZYGODY[I, P_X] = X2;
                    PRZYGODY[I, P_Y] = Y2;

                    if (PRZYGODY[I, P_TEREN] == 0)
                    {
                        TEREN(X2, Y2);
                        PRZYGODY[I, P_TEREN] = LOK;
                    }

                    screens.PasteBob(X2, Y2, 18);
                    screens.SetZone(121 + I, X2, Y2, X2 + 6, Y2 + 6);
                }
            }

            screens.View();
            screens.WaitVbl();
        }


        void B_DRAW(int NR, int X, int Y, int O)
        {
            screens.ResetZone(NR + 20);
            var Z1 = screens.Zone(X - 4, Y - 7);
            var Z2 = screens.Zone(X + 4, Y - 7);
            var Z3 = screens.Zone(X - 4, Y);
            var Z4 = screens.Zone(X + 4, Y);
            if (Z1 >= 20 && Z1 < 60) B_OFF(Z1 - 20);
            if (Z2 >= 20 && Z2 < 60) B_OFF(Z2 - 20);
            if (Z3 >= 20 && Z3 < 60) B_OFF(Z3 - 20);
            if (Z4 >= 20 && Z4 < 60) B_OFF(Z4 - 20);
            screens.GetBlock(NR + 1, X - 4, Y - 7, 8, 8, 1);
            if (Z1 >= 20 && Z1 < 60) B_UPDATE(Z1 - 20);
            if (Z2 >= 20 && Z2 < 60) B_UPDATE(Z2 - 20);
            if (Z3 >= 20 && Z3 < 60) B_UPDATE(Z3 - 20);
            if (Z4 >= 20 && Z4 < 60) B_UPDATE(Z4 - 20);
            screens.PasteBob(X - 4, Y - 7, O);
            screens.SetZone(NR + 20, X - 4, Y - 7, X + 4, Y);
        }

        void B_UPDATE(int NR)
        {
            var X = ARMIA[NR, 0, TX];
            var Y = ARMIA[NR, 0, TY];
            var O = ARMIA[NR, 0, TBOB];
            screens.PasteBob(X - 4, Y - 7, O);
        }

        void B_OFF(int NR)
        {
            var X = ARMIA[NR, 0, TX];
            var Y = ARMIA[NR, 0, TY];
            screens.PutBlock(NR + 1, X - 4, Y - 7);
        }

        void TEREN(int X1, int Y1)
        {
            var STREFA = screens.Zone(X1, Y1);
            if (STREFA > 69 && STREFA < 125)
            {
                LOK = STREFA;
                goto OVER;
            }

            if (STREFA > 19 && STREFA < 40)
            {
                LOK = ARMIA[STREFA - 20, 0, TNOGI];
                goto OVER;
            }

            var KOLORY = new int[32];
            for (var Y = Y1 - 4; Y <= Y1 + 4; Y++)
            {
                for (var X = X1 - 4; X <= X1 + 4; X++)
                {
                    var KOL = screens.Point(X, Y);
                    if (KOL > -1)
                    {
                        KOLORY[KOL] += 1;
                    }
                }
            }

            var FINAL = 0;
            var KOLOR = 0;
            for (var I = 0; I <= 31; I++)
            {
                if (KOLORY[I] > KOLOR)
                {
                    KOLOR = KOLORY[I];
                    FINAL = I;
                }
                KOLORY[I] = 0;
            }

            //Pen 23
            LOK = 0;

            if (FINAL > 8 && FINAL < 11)
            {
                LOK = 7; //bagno 
            }
            if (FINAL > 12 && FINAL < 16)
            {
                LOK = 2; //??ka
            }
            if (FINAL > 10 && FINAL < 13)
            {
                LOK = 1; //las
            }
            if (FINAL > 6 && FINAL < 9)
            {
                LOK = 3; //pustynia
            }
            if (FINAL > 0 && FINAL < 4)
            {
                LOK = 1; //las
            }
            if (FINAL > 28)
            {
                LOK = 5; //lodowiec
            }
            if (FINAL > 3 && FINAL < 7)
            {
                LOK = 4; //skały
            }
            if (FINAL > 23 && FINAL < 29)
            {
                LOK = 4; //skały
            }

        OVER:
            if (LOK <= 0) LOK = 2;
        }

        public void MAIN()
        {
            GAME_OVER = false;
            REAL_KONIEC = false;
            do
            {
                var A_S = screens.Inkey_S();
                var KLAW = screens.Scancode();
                if (KLAW >= KeyLeft && KLAW <= KeyDown)
                {
                    KLAWSKROL(KLAW);
                }
                screens.ClearKey();
                var X = screens.XMouse();
                var Y = screens.YMouse();

                screens.WaitVbl();

                if (screens.MouseKey() == PRAWY)
                {
                    if (X > SPX - 16 && Y > SPY - 16 && X < SPX + 16 && Y < SPY + 16)
                    {
                        do
                        {
                            SPX = screens.XMouse();
                            SPY = screens.YMouse();
                            screens.Sprite(2, SPX, SPY, 1);
                            screens.WaitVbl();
                        }
                        while (screens.MouseKey() == PRAWY);
                    }
                    else
                    {
                        SKROL(0);
                    }
                }

                var STREFA = screens.MouseZone();
                if (PREFS[5] == 1)
                {
                    if (STREFA > 69 && STREFA < 121)
                    {
                        screens.GrWriting(3);
                        var XX = MIASTA[STREFA - 70, 0, M_X];
                        var YY = MIASTA[STREFA - 70, 0, M_Y];
                        screens.Box(XX - 8, YY - 8, XX + 8, YY + 8);
                        amos.Wait(2);

                        screens.Box(XX - 8, YY - 8, XX + 8, YY + 8);
                        screens.Box(XX - 7, YY - 7, XX + 7, YY + 7);
                        amos.Wait(2);

                        screens.Box(XX - 7, YY - 7, XX + 7, YY + 7);
                        screens.GrWriting(0);
                    }
                    if (STREFA > 19 && STREFA < 61)
                    {
                        screens.GrWriting(3);
                        var XX = ARMIA[STREFA - 20, 0, TX];
                        var YY = ARMIA[STREFA - 20, 0, TY];
                        screens.Box(XX - 4, YY - 7, XX + 4, YY);
                        amos.Wait(2);

                        screens.Box(XX - 4, YY - 7, XX + 4, YY);
                        screens.GrWriting(0);
                    }
                }

                if (screens.MouseClick() == 1)
                {
                    STREFA = screens.MouseZone();
                    if (X > SPX - 14 && Y > SPY - 14 && X < SPX + 14 && Y < SPY - 1)
                    {
                        MAPA_AKCJA();
                        STREFA = 0;
                    }
                    if (X > SPX - 14 && Y > SPY + 1 && X < SPX + 14 && Y < SPY + 15)
                    {
                        OPCJE();
                        STREFA = 0;
                    }
                    if (STREFA > 19 && STREFA < 61)
                    {
                        ARMIA_(STREFA - 20);
                    }
                    if (STREFA > 69 && STREFA < 121)
                    {
                        MIASTO(STREFA - 70);
                    }
                    if (STREFA > 120 && STREFA < 125)
                    {
                        PRZYGODA_INFO(STREFA - 121);
                    }
                }

                if (REAL_KONIEC || GAME_OVER) break;
            } while (true);

            //Fade 2

            if (PREFS[3] == 1)
            {
                _TRACK_FADE(1);
            }
            CLEAR_TABLES();
            banks.EraseAll();
            //Sam Stop 
            screens.SpriteOff();
            if (GAME_OVER)
            {
                EXTRO();
            }
        }

        void SKROL(int A)
        {
            screens.AutoViewOn();
            var X = screens.XMouse();
            var Y = screens.YMouse();
            var X0 = X;
            var Y0 = Y;
            var XF = screens.XScreen(X);
            var YF = screens.YScreen(Y);
            if (PREFS[2] == 0)
            {
            }
            else
            {
                //Limit Mouse 0,0 To 600,600
                screens.HideOn();
            }

            do
            {
                screens.WaitVbl();
                if (PREFS[2] == 0)
                {
                    var X2 = screens.XScreen(screens.XMouse());
                    var Y2 = screens.YScreen(screens.YMouse());
                    var DX = XF - X2;
                    var DY = YF - Y2;
                    SX = SX + DX;
                    SY = SY + DY;
                }
                else
                {
                    var DX = (X - screens.XMouse()) * 2;
                    var DY = (Y - screens.YMouse()) * 2;
                    SX = SX - DX;
                    SY = SY - DY;
                    X = screens.XMouse();
                    Y = screens.YMouse();
                }

                if (SX < 0) SX = 0;
                if (SY < 0) SY = 0;
                if (SX > MSX) SX = MSX;
                if (SY > MSY) SY = MSY;

                screens.ScreenOffset(0, SX, SY);
            }
            while (screens.MouseKey() == PRAWY);

            if (PREFS[2] == 1)
            {
                //Limit Mouse 128,40 To 447,298
                //X Mouse = XO
                //Y Mouse = YO
                screens.ShowOn();
            }

            screens.AutoViewOff();

            if (A == 1) screens.ChangeMouse(CELOWNIK);
        }

        void KLAWSKROL(int KLAW)
        {
            screens.AutoViewOn();

            do
            {
                if (KLAW == KeyUp) SY += -4;
                if (KLAW == KeyDown) SY += 4;
                if (KLAW == KeyRight) SX += 4;
                if (KLAW == KeyLeft) SX += -4;

                if (SX < 0) SX = 0;
                if (SY < 0) SY = 0;
                if (SX > MSX) SX = MSX;
                if (SY > MSY) SY = MSY;

                screens.ScreenOffset(0, SX, SY);
                screens.WaitVbl();
            }
            while (screens.KeyState(KLAW));

            screens.AutoViewOff();
        }

        void OKNO(int OKX, int OKY, int SZER, int WYS)
        {
            OKX += SX;
            OKY += SY;
            this.OKX = OKX;
            this.OKY = OKY;

            screens.GetBlock(100, OKX, OKY, SZER + 16, WYS + 16, 1);
            screens.Ink(17);
            screens.Box(OKX, OKY, OKX + SZER, OKY + WYS);
        }

        void ZOKNO()
        {
            screens.PutBlock(100, OKX, OKY);
            for (var I = 1; I <= 19; I++)
            {
                screens.ResetZone(I);
            }
        }

        void GADGET(int X, int Y, int SZER, int WYS, String TX_S, int K1, int K2, int K3, int K4, int STREFA)
        {
            var DEEPX = 1;
            var DEEPY = 1;
            var DLUGTX = TX_S.Length;
            var X1 = X + SZER;
            var Y1 = Y + WYS;
            if (STREFA > 0)
            {
                screens.SetZone(STREFA, X, Y, X1, Y1);
            }
            screens.Ink(K1);
            screens.Bar(X, Y, X1, Y1);
            screens.Ink(K3);
            screens.Bar(X + DEEPX, Y + DEEPY, X1 - DEEPX, Y1 - DEEPY);
            screens.Ink(K2);
            screens.Polyline(X, Y1, X1, Y1, X1, Y);

            if (!String.IsNullOrEmpty(TX_S))
            {
                if (amos.Upper_S(amos.Left_S(TX_S, 3)) == "BOB")
                {
                    var BNR = amos.Val(amos.Mid_S(TX_S, 4, 2));
                    screens.PasteBob(X, Y, BNR);
                }
                else
                {
                    //If CIENIE> 0 and CIENIE<4
                    //    Ink K2, K3
                    //    Text X+FONT / 2,Y + FONT + FONT / 3,TX$
                    //    Ink K4, K3
                    //    Text(X + FONT / 2) + CIENIE,(Y + FONT + FONT / 3) + CIENIE,TX$
                    //Else
                    screens.Ink(K4, K3);
                    screens.Text(X + FONT / 2, Y + FONT + FONT / 3, TX_S);
                }
            }

            if (STREFA == 0)
            {
                do
                {
                } while (screens.MouseKey() == LEWY);
            }
        }

        void OPCJE()
        {
            var KONIEC = false;

            OPCJE_RYSUJ();

            do
            {
                if (screens.MouseClick() == 1)
                {
                    var STREFA = screens.MouseZone();
                    if (STREFA == 1)
                    {
                        ZOKNO();
                        var JEST = _LOAD_GAME();
                        if (JEST)
                        {
                            KONIEC = true;
                            screens.ResetZone();
                            screens.DelBlock();
                            screens.SpriteOff(2);
                            SETUP0();
                            VISUAL_OBJECTS();
                            screens.ChangeMouse(5);
                            screens.Sprite(2, SPX, SPY, 1);
                            CENTER(10, 10, 0);
                        }
                        else
                        {
                            OPCJE_RYSUJ();
                        }
                    }
                    if (STREFA == 2)
                    {
                        ZOKNO();
                        _SAVE_GAME();
                        OPCJE_RYSUJ();
                    }
                    if (STREFA == 3)
                    {
                        ZOKNO();
                        STATUS();
                        OPCJE_RYSUJ();
                    }
                    if (STREFA == 4)
                    {
                        ZOKNO();
                        PREFERENCJE();
                        OPCJE_RYSUJ();
                    }
                    if (STREFA == 5)
                    {
                        REAL_KONIEC = true;
                        KONIEC = true;
                        ZOKNO();
                    }
                    if (STREFA == 6)
                    {
                        KONIEC = true;
                        ZOKNO();
                    }
                }
            }
            while (!KONIEC);
        }

        void OPCJE_RYSUJ()
        {
            OKNO(100, 60, 140, 160);
            var SZMAL_S = GRACZE[1, 1]; //SZMAL$=Str$(GRACZE(1,1))-" "
            var DZIEN_S = DZIEN; //DZIEN$=Str$(DZIEN)-" "
            GADGET(OKX + 10, OKY + 8, 120, 15, "Dzień " + DZIEN_S + "  Skarbiec:" + SZMAL_S, 7, 0, 4, 31, -1);
            GADGET(OKX + 10, OKY + 28, 120, 15, "Odczyt Gry", 8, 1, 6, 31, 1);
            GADGET(OKX + 10, OKY + 48, 120, 15, "Zapis Gry", 8, 1, 6, 31, 2);
            GADGET(OKX + 10, OKY + 68, 120, 15, "Statystyka", 8, 1, 6, 31, 3);
            GADGET(OKX + 10, OKY + 88, 120, 15, "Preferencje", 8, 1, 6, 31, 4);
            GADGET(OKX + 10, OKY + 108, 120, 15, "Koniec Gry", 8, 1, 6, 31, 5);
            GADGET(OKX + 10, OKY + 128, 120, 15, "Exit", 8, 1, 6, 31, 6);
        }

        bool _LOAD_GAME()
        {
            var KONIEC = false;
            var JEST = false;
            var PAT_S = SDIR("Archiwum - Odczyt Gry", 17, 16);
            do
            {
                if (screens.MouseClick() == 1)
                {
                    var STREFA = screens.MouseZone();
                    var fileName = Path.Combine(SAVE_FOLDER_NAME, "zapis" + amos.Str_S(STREFA));
                    if (STREFA > 0 && STREFA < 6 && File.Exists(fileName))
                    {
                        var NSAVE = STREFA;
                        KONIEC = true;
                        ZOKNO();
                        JEST = true;
                        Load(fileName);
                    }
                    if (STREFA == 6)
                    {
                        ZOKNO();
                        KONIEC = true;
                    }
                }
            } while (!KONIEC);
            return JEST;
        }

        void CENTER(int X, int Y, int FLOOD)
        {
            X += -160;
            Y += -CENTER_V;
            if (X < 0) X = 0;
            if (X > MSX) X = MSX;
            if (Y < 0) Y = 0;
            if (Y > MSY) Y = MSY;
            if (FLOOD > 0)
            {
                var ZX = amos.Sgn(X - SX) * 8 * FLOOD;
                var ZY = amos.Sgn(Y - SY) * 8 * FLOOD;
                var DX = 0;
                var DY = 0;
                do
                {
                    if (Math.Abs(DX) > Math.Abs(ZX)) SX += ZX;
                    if (Math.Abs(DY) > Math.Abs(ZY)) SY += ZY;
                    DX = (SX - X);
                    DY = (SY - Y);
                    screens.WaitVbl();
                    screens.ScreenOffset(0, SX, SY);
                    screens.View();
                }
                while (!(Math.Abs(DX) <= Math.Abs(ZX) && Math.Abs(DY) <= Math.Abs(ZY)));
            }
            else
            {
                SX = X;
                SY = Y;
                screens.ScreenOffset(0, SX, SY);
                screens.View();
            }
        }

        void SETUP(String A_S, String B_S, String C_S)
        {
            if (PREFS[3] == 1)
            {
                _TRACK_FADE(1);
            }
            banks.EraseAll();
            screens.HideOn();
            screens.ScreenOpen(2, 80, 50, 32, PixelMode.Lowres);
            //Curs Off 
            //Flash Off 
            screens.ScreenDisplay(2, 252, 140, 80, 50);
            screens.Cls(0);
            screens.SetFont(FON2);
            screens.Ink(19, 0);
            var X = 40 - (screens.TextLength(A_S) / 2);
            screens.Text(X, 15, A_S);
            X = 40 - (screens.TextLength(B_S) / 2);
            screens.Text(X, 28, B_S);
            X = 40 - (screens.TextLength(C_S) / 2);
            screens.Text(X, 40, C_S);
            screens.View();
            screens.ScreenOpen(0, 640, 512, 32, PixelMode.Lowres);
            screens.ScreenDisplay(0, 130, 40, 320, 234);
            screens.Screen(0);
            //Curs Off 
            //Flash Off
            screens.Cls(0);
            //Priority On : Priority Reverse Off 
            //Double Buffer : Autoback 1 :
            screens.BobUpdateOff();
            screens.ScreenHide();
            screens.ReserveZone(200);
            //'------------------
            screens.ScreenOpen(1, 320, 160, 32, PixelMode.Lowres);
            screens.ScreenDisplay(1, 130, 275, 320, 25);
            screens.Screen(1);
            screens.ScreenHide(1);
            screens.ScreenToFront(2);
            screens.Screen(1);
            //Curs Off 
            //Flash Off
            screens.Cls(10);
            screens.ReserveZone(100);
            _LOAD("dane/gad", 0);
            USTAW_FONT("defender2", 8);
            screens.GetBobPalette();
            screens.ScreenShow();
            //'------------------
            screens.Screen(0);
            screens.ScreenShow();
            //Sam Bank 4
            _LOAD("dane/sound", 8);
            //Mvolume 50
            _LOAD("dane/glowny", 1);
            USTAW_FONT("defender2", 8);
            screens.GetBobPalette();
            GOBY = 0;
            PIKIETY = 50;
            BUBY = PIKIETY + 18 + 10;
            POTWORY = BUBY + 131 + 32;
            BIBY = POTWORY + 16;
            BSIBY = BIBY + 12;
            CELOWNIK = 52;
            MSX = 320;
            MSY = 278;
            screens.HotSpot(3 + BUBY, 0);
            screens.ScreenToBack(2);
        }

        void NOWA_POSTAC(int A, int NR, int RASA)
        {
            for (var I = 0; I <= 30; I++)
            {
                ARMIA[A, NR, I] = 0;
            }
            ARMIA[A, NR, TRASA] = RASA;
            ARMIA[A, NR, TSI] = amos.Rnd(10) + (RASY[RASA, 1] / 2);
            ARMIA[A, NR, TSZ] = amos.Rnd(10) + RASY[RASA, 2];
            ARMIA[A, NR, TE] = (amos.Rnd(20) + RASY[RASA, 0]) * 3;
            ARMIA[A, NR, TEM] = ARMIA[A, NR, TE];
            ARMIA[A, NR, TKLAT] = amos.Rnd(3);
            if (RASA > 9)
            {
                ARMIA[A, NR, TKORP] = 150 + amos.Rnd(60);
                ARMIA[A, NR, TMAG] = BRON[RASY[RASA, 6], B_MAG] * 5;
                ARMIA[A, NR, TMAGMA] = ARMIA[A, NR, TMAG];
                //'potwory w plecaku przechowuj� czar  
                ARMIA[A, NR, TPLECAK] = RASY[RASA, 6];
                ARMIA[A, NR, TAMO] = amos.Rnd(20);
                var ODP = RASY[RASA, 5];
                ARMIA[A, NR, TP] = amos.Rnd(ODP / 2) + ODP / 2;
                ARMIA[A, NR, TDOSW] = amos.Rnd(30);
            }
            else
            {
                ARMIA[A, NR, TMAG] = amos.Rnd(5) + RASY[RASA, 3];
                ARMIA[A, NR, TMAGMA] = ARMIA[A, NR, TMAG];
            }
            if (PREFS[1] == 1)
            {
                ARMIA_S[A, NR] = ROB_IMIE();
            }
            else
            {
                ARMIA_S[A, NR] = "wojownik" + NR;
            }
            if (A > 19)
            {
                if (RASA < 10)
                {
                    ARMIA[A, NR, TSI] += amos.Rnd(POWER);
                    ARMIA[A, NR, TP] = amos.Rnd(POWER / 2) + POWER / 2;
                    ARMIA[A, NR, TDOSW] = amos.Rnd(POWER / 2) + POWER / 2;
                }
            }
            else
            {
                //'zapasowa pr�dko�� w tamo
                ARMIA[A, NR, TAMO] = ARMIA[A, NR, TSZ];
                WAGA(A, NR);
            }
        }

        String ROB_IMIE()
        {
            var IM_S = "";
            var SAMOGL_S = new String[] { "a", "e", "i", "o", "u", "i", "a", "a", "e", "o" };
            var DZW_S = new String[] { "c", "f", "h", "k", "p", "s", "t", "p", "j", "s", "s", "k", "t", "p", "t", "f", "b", "d", "g", "l", "m", "n", "r", "w", "z", "r", "r", "r", "d", "z", "b", "g" };

            var DL = amos.Rnd(2) + 1;
            for (var I = 0; I <= DL; I++)
            {
                var SAM_S = SAMOGL_S[amos.Rnd(9)];
                var SPD_S = DZW_S[amos.Rnd(30)];
                var A = amos.Rnd(2);
                if (A == 0)
                {
                    IM_S = IM_S + SAM_S;
                    IM_S = IM_S + SPD_S;
                }
                if (A == 1)
                {
                    IM_S = IM_S + SPD_S;
                    IM_S = IM_S + SAM_S;
                }
                if (A == 2)
                {
                    IM_S = IM_S + SAM_S;
                }
            }
            var L_S = amos.Left_S(IM_S, 1);
            L_S = amos.Upper_S(L_S);
            var R_S = amos.Right_S(IM_S, IM_S.Length - 1);
            var F_S = L_S + R_S;
            return F_S;
        }

        void SZPIEGUJ(int NR, int CO)
        {
            var KONIEC = false;
            OKNO(80, 90, 156, 100);
            GADGET(OKX + 4, OKY + 4, 104, 92, "", 31, 2, 30, 1, -1);
            GADGET(OKX + 112, OKY + 4, 18, 15, " " + UpArrowChar, 8, 2, 6, 31, 1);
            GADGET(OKX + 133, OKY + 4, 18, 15, " " + DownArrowChar, 8, 2, 6, 31, 2);
            GADGET(OKX + 112, OKY + 24, 40, 15, "", 31, 2, 30, 1, -1);
            GADGET(OKX + 112, OKY + 61, 40, 15, "Odwołać", 8, 2, 6, 31, 3);
            GADGET(OKX + 112, OKY + 81, 40, 15, "    Ok", 8, 2, 6, 31, 4);
            screens.PasteBob(OKX + 8, OKY + 8, 35);
            var CENA = 0;
            var DNI = 22;
            SZPIEGUJ_WYPISZ(CENA, DNI);
            do
            {
                if (screens.MouseClick() == 1)
                {
                    var STREFA = screens.MouseZone();
                    var ZN = 0;
                    if (STREFA == 1 || STREFA == 2)
                    {
                        if (STREFA == 1)
                        {
                            ZN = 1;
                        }
                        else
                        {
                            ZN = -1;
                        }
                        amos.Add(ref CENA, ZN * 50, 0, 1000);
                        amos.Add(ref DNI, -ZN, 2, 22);
                        screens.Ink(30);
                        screens.Bar(OKX + 113, OKY + 25, OKX + 150, OKY + 38);
                        screens.Ink(1, 30);
                        screens.Text(OKX + 120, OKY + 34, amos.Str_S(CENA));
                        SZPIEGUJ_WYPISZ(CENA, DNI);
                    }
                    if (STREFA == 4)
                    {
                        if (GRACZE[1, 1] - CENA >= 0)
                        {
                            if (CENA > 100)
                            {
                                if (CO == 0)
                                {
                                    MIASTA[NR, 1, M_Y] = DNI;
                                }
                                if (CO == 1)
                                {
                                    ARMIA[NR, 0, TMAGMA] = DNI;
                                }
                                GRACZE[1, 1] += -CENA;
                            }
                            KONIEC = true;
                        }
                    }
                    if (STREFA == 3)
                    {
                        KONIEC = true;
                    }
                }
            }
            while (!KONIEC);
            ZOKNO();
        }

        void SZPIEGUJ_WYPISZ(int CENA, int DNI)
        {
            var A_S = "";
            var B_S = "";
            if (CENA <= 100)
            {
                A_S = "ładną mamy dziś ";
                B_S = "pogodę.";
                if (CENA == 0)
                {
                    A_S = "Za informacje trzeba";
                    B_S = "zapłacić.";
                }
            }
            else
            {
                A_S = "Za " + DNI + " dni";
                B_S = "będę coś wiedział.";
            }
            screens.Ink(30);
            screens.Bar(OKX + 5, OKY + 68, OKX + 102, OKY + 92);
            screens.Ink(1, 30);
            screens.Text(OKX + 12, OKY + 80, A_S);
            screens.Text(OKX + 12, OKY + 90, B_S);
        }

        void MESSAGE(int A, String A_S, int P, int M)
        {
            var NA_S = "";
            var BB = 0;
            var AX = 0;
            var AY = 0;

            if (M > 0)
            {
                NA_S = MIASTA_S[A];
                var MUR = MIASTA[A, 0, M_MUR];
                BB = 20 + MUR;
                AX = MIASTA[A, 0, M_X];
                AY = MIASTA[A, 0, M_Y];
            }
            else
            {
                AX = ARMIA[A, 0, TX];
                AY = ARMIA[A, 0, TY];
                NA_S = ARMIA_S[A, 0];
                var PL = ARMIA[A, 0, TMAG];
                BB = 23 + PL;
            }
            WJAZD(AX, AY, 80, 80, 150, 100, 10);
            OKNO(80, 80, 150, 100);
            GADGET(OKX + 4, OKY + 4, 142, 74, "", 31, 2, 30, 1, -1);
            GADGET(OKX + 50, OKY + 80, 50, 15, "     Ok  ", 8, 2, 6, 31, 1);
            screens.NoMask(BB);
            screens.PasteBob(OKX + 8, OKY + 8, BB);
            screens.Ink(1, 30);
            screens.Text(OKX + 50, OKY + 15, NA_S);
            NAPISZ(OKX + 46, OKY + 30, 98, 40, A_S, P, 1, 30);

            while (!(screens.MouseClick() != 0 && screens.MouseZone() == 1)) { }
            //Repeat Until Mouse Click && screens.MouseZone() == 1
            ZOKNO();
        }

        void MESSAGE2(int A, String A_S, int B, int M, int WLOT)
        {
            var NA_S = "";
            var X = 0;
            var Y = 0;

            if (M > 0)
            {
                NA_S = MIASTA_S[A];
                //MUR = MIASTA[A, 0, M_MUR]; // it seems that not used anywhere
                X = MIASTA[A, 0, M_X];
                Y = MIASTA[A, 0, M_Y];
            }
            else
            {
                NA_S = ARMIA_S[A, 0];
                X = ARMIA[A, 0, TX];
                Y = ARMIA[A, 0, TY];
            }
            if (WLOT == 1)
            {
                WJAZD(X, Y, 100, 80, 112, 120, 10);
            }
            OKNO(100, 80, 112, 120);
            GADGET(OKX + 4, OKY + 4, 105, 114, "", 31, 2, 30, 1, 1);

            if (amos.Rnd(1) == 1)
            {
                //B = Hrev(B);
            }
            screens.PasteBob(OKX + 8, OKY + 8, B);
            screens.Ink(1, 30);
            screens.Text(OKX + 12, OKY + 80, NA_S);
            //NOTE: P was not declared, it seems it is not needed, so I set it here to 0
            var P = 0;
            NAPISZ(OKX + 10, OKY + 90, 88, 20, A_S, P, 1, 30);
            while (!(screens.MouseClick() != 0 && screens.MouseZone() == 1)) { }
            //Repeat Until Mouse Click && screens.MouseZone() == 1
            ZOKNO();
        }

        void WJAZD(int XS, int YS, int X1, int Y1, int SZER, int WYS, int _WAIT)
        {
            screens.GrWriting(3);
            screens.Ink(20);
            for (var I = 1; I <= _WAIT; I++)
            {
                screens.Box(XS - 4, YS - 4, XS + 4, YS + 4);
                amos.Wait(2);
            }
            X1 += SX;
            Y1 += SY;
            var SPEED = 15;
            var DX1 = ((X1 - XS) / SPEED);
            var DX2 = ((X1 + SZER - XS) / SPEED);
            var DY1 = ((Y1 - YS) / SPEED);
            var DY2 = ((Y1 + WYS - YS) / SPEED);

            var XB1 = XS;
            var YB1 = YS;
            var XB2 = XS;
            var YB2 = YS;
            do
            {
                XB1 += DX1;
                XB2 += DX2;
                YB1 += DY1;
                YB2 += DY2;
                if (Math.Abs(X1 - XB1) < SPEED)
                {
                    XB1 = X1;
                }
                if (Math.Abs(Y1 - YB1) < SPEED)
                {
                    YB1 = Y1;
                }
                if (Math.Abs(X1 + SZER - XB2) < SPEED)
                {
                    XB2 = X1 + SZER;
                }
                if (Math.Abs(Y1 + WYS - YB2) < SPEED)
                {
                    YB2 = Y1 + WYS;
                }
                screens.Box(XB1, YB1, XB2, YB2);
                screens.WaitVbl();
                screens.Box(XB1, YB1, XB2, YB2);

            }
            while (!(XB1 == X1 && YB1 == Y1 && XB2 == X1 + SZER && YB2 == Y1 + WYS));
            //Until XB1 == X1 && YB1 == Y1 && XB2 == X1 + SZER && YB2 == Y1 + WYS
            screens.GrWriting(0);
        }

        void PRZYGODA_INFO(int NR)
        {
            OKNO(70, 100, 180, 23);
            var TYP = PRZYGODY[NR, P_TYP];
            var A_S = PRZYGODY_S[TYP, 0];
            if (IM_PRZYGODY_S[NR] != "")
            {
                var DL = A_S.Length;
                var ZN = amos.Instr(A_S, "$");
                if (ZN > 0)
                {
                    A_S = A_S.Replace('$', '\0');
                    var L_S = amos.Left_S(A_S, ZN - 1);
                    var R_S = amos.Right_S(A_S, DL - ZN - 1);
                    A_S = L_S + IM_PRZYGODY_S[NR] + R_S;
                }
            }
            GADGET(OKX + 4, OKY + 4, 172, 15, A_S, 31, 2, 30, 1, -1);
            do { } while (screens.MouseClick() != 1);
            ZOKNO();
        }

        void CLEAR_TABLES()
        {
            //'armia(40,10,30) 
            for (var I = 0; I <= 40; I++)
            {
                for (var J = 0; J <= 10; J++)
                {
                    for (var K = 0; K <= 30; K++)
                    {
                        ARMIA[I, J, K] = 0;
                    }
                }
            }
            //'wojna(5,5)
            for (var I = 0; I <= 5; I++)
            {
                for (var J = 0; J <= 5; J++)
                {
                    WOJNA[I, J] = 0;
                }
            }
            //'gracze(4,3) 
            for (var I = 0; I <= 4; I++)
            {
                for (var J = 0; J <= 3; J++)
                {
                    GRACZE[I, J] = 0;
                }
            }
            //'armia_S(40,10) 
            for (var I = 0; I <= 40; I++)
            {
                for (var J = 0; J <= 10; J++)
                {
                    ARMIA_S[I, J] = "";
                }
            }
            //'imiona_S(4)
            for (var I = 0; I <= 4; I++)
            {
                IMIONA_S[I] = "";
            }
            //'prefs(10) 
            for (var I = 0; I <= 10; I++)
            {
                PREFS[I] = 0;
            }
            //'miasta(50,20,6) 
            for (var I = 0; I <= 50; I++)
            {
                for (var J = 0; J <= 20; J++)
                {
                    for (var K = 0; K <= 6; K++)
                    {
                        MIASTA[I, J, K] = 0;
                    }
                }
            }
            //'miasta_S(50) 
            for (var I = 0; I <= 50; I++)
            {
                MIASTA_S[I] = "";
            }
            //'przygody(3,10)
            for (var I = 0; I <= 3; I++)
            {
                for (var J = 0; J <= 10; J++)
                {
                    PRZYGODY[I, J] = 0;
                }
            }
            //'im_przygody_S(3) 
            for (var I = 0; I <= 3; I++)
            {
                IM_PRZYGODY_S[I] = "";
            }
        }

        void STATUS()
        {
            OKNO(80, 80, 160, 120);
            GADGET(OKX + 4, OKY + 4, 152, 92, "", 31, 2, 30, 1, -1);
            GADGET(OKX + 4, OKY + 100, 40, 15, "Wykresy", 8, 1, 6, 31, 2);
            GADGET(OKX + 116, OKY + 100, 40, 15, "   Ok", 8, 1, 6, 31, 1);

            var AM = 0;
            var WOJ = 0;
            var LUDZIE = 0;
            var POD = 0;
            var KONIEC = false;

            for (var A = 0; A <= 19; A++)
            {
                if (ARMIA[A, 0, TE] > 0)
                {
                    AM++;
                    for (var I = 1; I <= 10; I++)
                    {
                        if (ARMIA[A, I, TE] > 0)
                        {
                            WOJ++;
                        }
                    }
                }
            }
            var RES = AM % 10;
            var KON_S = "";
            var KON2_S = "ów";
            if (RES <= 1 || RES > 4)
            {
                KON_S = "ów";
            }
            if (RES > 1 && RES < 5)
            {
                KON_S = "y";
            }
            if (AM == 1)
            {
                KON_S = "";
            }
            if (WOJ == 1)
            {
                KON2_S = "";
            }
            var A_S = amos.Str_S(AM) + " legion" + KON_S + ", " + amos.Str_S(WOJ) + " wojownik" + KON2_S;
            var MS = 0;
            for (var M = 0; M <= 49; M++)
            {
                if (MIASTA[M, 0, M_CZYJE] == 1)
                {
                    MS++;
                    LUDZIE += MIASTA[M, 0, M_LUDZIE];
                    POD += MIASTA[M, 0, M_PODATEK] * MIASTA[M, 0, M_LUDZIE] / 25;
                }
            }
            RES = MS % 10;
            KON_S = "";
            if (RES > 1 && RES < 5)
            {
                KON_S = "a";
            }
            if (MS == 1)
            {
                KON_S = "o";
            }
            var B_S = amos.Str_S(MS) + " miast" + KON_S + ", " + amos.Str_S(LUDZIE) + " mieszkańców";
            var C_S = "Dzienny dochód : " + amos.Str_S(POD);
            STATUS_NAPISZ(A_S, B_S, C_S);
            do
            {
                if (screens.MouseClick() == 1)
                {
                    var STREFA = screens.MouseZone();
                    if (STREFA == 1)
                    {
                        KONIEC = true;
                    }
                    if (STREFA == 2)
                    {
                        screens.Ink(30);
                        screens.Bar(OKX + 5, OKY + 5, OKX + 150, OKY + 90);
                        for (var I = 1; I <= 4; I++)
                        {
                            var WYS = GRACZE[I, 2] / 250;
                            var KOL = GRACZE[I, 3];
                            if (WYS > 100)
                            {
                                WYS = 100;
                            }
                            if (WYS < 4)
                            {
                                WYS = 4;
                            }
                            screens.Ink(1, 30);
                            screens.Text(OKX + 8, OKY + 4 + I * 20, IMIONA_S[I]);
                            screens.Ink(KOL + 1);
                            screens.Box(OKX + 50, OKY - 8 + I * 20, OKX + 49 + WYS, OKY - 9 + I * 20 + 15);
                            screens.Ink(25);
                            screens.Box(OKX + 51, OKY - 7 + I * 20, OKX + 50 + WYS, OKY - 8 + I * 20 + 15);
                            screens.Ink(KOL);
                            screens.Bar(OKX + 51, OKY - 7 + I * 20, OKX + 49 + WYS, OKY - 9 + I * 20 + 15);
                        }
                        do { } while (screens.MouseClick() != 1);
                        screens.Ink(30);
                        screens.Bar(OKX + 5, OKY + 5, OKX + 150, OKY + 90);
                        STATUS_NAPISZ(A_S, B_S, C_S);
                    }
                }
            } while (!KONIEC);
            ZOKNO();
        }

        void STATUS_NAPISZ(String A_S, String B_S, String C_S)
        {
            screens.Ink(1, 30);
            screens.Text(OKX + 8, OKY + 16, "Raport na dzień: " + amos.Str_S(DZIEN));
            screens.Text(OKX + 8, OKY + 16 + 10, "W twoim władaniu :");
            screens.Text(OKX + 8, OKY + 24 + 20, A_S);
            screens.Text(OKX + 8, OKY + 24 + 30, B_S);
            screens.Text(OKX + 8, OKY + 34 + 40, C_S);
            screens.Text(OKX + 8, OKY + 34 + 50, "W skarbcu: " + amos.Str_S(GRACZE[1, 1]));
        }

        void PREFERENCJE()
        {
            var KONIEC = false;
            PREFERENCJE_RYSUJ();
            do
            {
                if (screens.MouseClick() == 1)
                {
                    var STREFA = screens.MouseZone();
                    if (STREFA > 0 && STREFA < 6)
                    {
                        if (PREFS[STREFA] == 1)
                        {
                            PREFS[STREFA] = 0;
                            screens.Text(OKX + 120, OKY + 18 + (STREFA * 20), "  ");
                            if (STREFA == 3)
                            {
                                _TRACK_FADE(1);
                                //Erase 3
                            }
                        }
                        else
                        {
                            PREFS[STREFA] = 1;
                            screens.Text(OKX + 120, OKY + 18 + (STREFA * 20), "@");
                            if (STREFA == 3)
                            {
                                _LOAD("mod.legion", 6);
                                amos.TrackLoopOn();
                                amos.TrackPlay();
                            }
                        }
                    }
                    if (STREFA == 6)
                    {
                        KONIEC = true;
                        ZOKNO();
                    }
                }
            } while (!KONIEC);
        }

        void PREFERENCJE_RYSUJ()
        {
            OKNO(100, 60, 140, 160);
            GADGET(OKX + 10, OKY + 8, 120, 15, " Preferencje Gry", 7, 0, 4, 31, -1);
            GADGET(OKX + 10, OKY + 28, 120, 15, "Imiona Wojowników", 8, 1, 6, 31, 1);
            GADGET(OKX + 10, OKY + 48, 120, 15, "Szybki skrol mapy ", 8, 1, 6, 31, 2);
            GADGET(OKX + 10, OKY + 68, 120, 15, "Muzyka", 8, 1, 6, 31, 3);
            GADGET(OKX + 10, OKY + 88, 120, 15, "Trupy", 8, 1, 6, 31, 4);
            GADGET(OKX + 10, OKY + 108, 120, 15, "Bajery", 8, 1, 6, 31, 5);
            GADGET(OKX + 10, OKY + 128, 120, 15, "Exit", 8, 1, 6, 31, 6);
            screens.GrWriting(1);
            screens.Ink(31, 6);
            for (var STREFA = 1; STREFA <= 5; STREFA++)
            {
                if (PREFS[STREFA] == 0)
                {
                    screens.Text(OKX + 120, OKY + 18 + (STREFA * 20), "  ");
                }
                else
                {
                    screens.Text(OKX + 120, OKY + 18 + (STREFA * 20), "@");
                }
            }
        }

        void WPISZ(int X, int Y, int K1, int K2, int ILE)
        {
            screens.Ink(K2, K1);
            screens.Bar(X, Y - screens.TextBase(), X + (ILE * FONTSZ), Y + (FONT / 3));
            screens.ClearKey();

            var NX = 0;
            var NY = 0;

            if (screens.Screen() == 0)
            {
                NX = SX;
                NY = SY;
            }
            else
            {
                NX = 0;
                NY = 0;
            }

            var SUMA_S = "";
            var DLT = 0;
            var SC = 0;

            do
            {
                var K_S = screens.Inkey_S();
                if (K_S != "" && 
                    screens.KeyState(KeyBackspace) == false && 
                    SUMA_S.Length < ILE && 
                    screens.KeyState(KeyEnter) == false && 
                    DLT < 100)
                {
                    screens.Ink(K1, K2);
                    screens.Text(X, Y, K_S);
                    SUMA_S = SUMA_S + K_S;
                    var DZ = screens.TextLength(K_S);
                    DLT = screens.TextLength(SUMA_S);
                    X = X + DZ;
                    if (SUMA_S.Length < ILE)
                    {
                        //screens.XMouse = X Hard(X) - NX
                        //screens.YMouse = Y Hard(Y) - NY
                    }
                    //amos.Wait(10);
                }
                if (SUMA_S.Length >= 0 && screens.KeyState(KeyBackspace))
                {
                    K_S = amos.Right_S(SUMA_S, 1);
                    var DZ = screens.TextLength(K_S);
                    X = X - DZ;
                    //X Mouse = X Hard(X) - NX
                    //screens.YMouse = Y Hard(Y) - NY
                    screens.Ink(K2);
                    screens.Text(X, Y, K_S);
                    var DL = SUMA_S.Length - 1;
                    SUMA_S = amos.Left_S(SUMA_S, DL);
                    DLT = screens.TextLength(SUMA_S);
                    while (screens.KeyState(KeyBackspace)) { }
                    //amos.Wait(10);
                }
                
                SC = screens.Scancode();
            } while (!(SC == KeyEnter || screens.MouseClick() == 2));
            int.TryParse(SUMA_S, out WPI);
            double.TryParse(SUMA_S, out WPI_R);
            WPI_S = SUMA_S;
        }

        void ZROB_MIASTA()
        {
            for (var I = 0; I <= 49; I++)
            {
            AGAIN:
                var X = amos.Rnd(590) + 20;
                var Y = amos.Rnd(460) + 20;
                if (screens.Zone(X, Y) == 0 && screens.Zone(X + 8, Y + 8) == 0 && screens.Zone(X + 4, Y + 4) == 0)
                {
                    var LUD = amos.Rnd(900) + 10;
                    var CZYJE = 0;
                    if (I == 43 || I == 44)
                    {
                        CZYJE = 2;
                    }
                    if (I == 45 || I == 46)
                    {
                        CZYJE = 3;
                    }
                    if (I == 47 || I == 48)
                    {
                        CZYJE = 4;
                    }
                    var B1 = 8 + CZYJE * 2;

                    var MUR = 0;
                    if (LUD > 700)
                    {
                        B1++;
                        MUR = amos.Rnd(2) + 1;
                    }
                    else
                    {
                        MUR = amos.Rnd(1);
                    }
                    var POD = amos.Rnd(25);
                    var MORALE = amos.Rnd(100);
                    TEREN(X + 4, Y + 4);
                    if (LOK == 7)
                    {
                        LOK = 1;
                    }
                    MIASTA[I, 1, M_X] = LOK;
                    screens.PasteBob(X, Y, B1);
                    screens.SetZone(70 + I, X - 20, Y - 20, X + 30, Y + 30);
                    MIASTA[I, 0, M_X] = X + 8;
                    MIASTA[I, 0, M_Y] = Y + 8;
                    MIASTA[I, 0, M_LUDZIE] = LUD;
                    MIASTA[I, 0, M_PODATEK] = POD;
                    MIASTA[I, 0, M_MORALE] = MORALE;
                    MIASTA[I, 1, M_MORALE] = amos.Rnd(10) + 5;
                    MIASTA[I, 0, M_CZYJE] = CZYJE;
                    MIASTA[I, 0, M_MUR] = MUR;
                    MIASTA[I, 1, M_Y] = 30;
                    var res = ROB_IMIE();
                    MIASTA_S[I] = res;
                    X = 50;
                    Y = 50;
                    for (var J = 2; J <= 9; J++)
                    {
                        var BUD = amos.Rnd(9);
                        var SZER = BUDYNKI[BUD, 0];
                        var SZER2 = SZER / 2;
                        var WYS = BUDYNKI[BUD, 1];
                        X += amos.Rnd(50);
                        if (X > 580)
                        {
                            X = 50;
                            Y += 130 + amos.Rnd(30);
                        }
                        MIASTA[I, J, M_X] = X;
                        MIASTA[I, J, M_Y] = Y;
                        MIASTA[I, J, M_LUDZIE] = BUD;
                        X += SZER;
                        //'za co nie�le zap�aci
                        MIASTA[I, J, M_PODATEK] = amos.Rnd(18) + 1;
                    }
                    //'modyfikatory cenowe w % 
                    var WAHANIA = 0;
                    if (CZYJE == 1)
                    {
                        WAHANIA = 20;
                    }
                    else
                    {
                        WAHANIA = 50;
                    }
                    for (var J = 1; J <= 19; J++)
                    {
                        MIASTA[I, J, M_MUR] = amos.Rnd(WAHANIA);
                    }
                }
                else
                {
                    goto AGAIN;
                }
            }
            for (var I = 0; I <= 49; I++)
            {
                var X = MIASTA[I, 0, M_X] - 8;
                var Y = MIASTA[I, 0, M_Y] - 8;
                screens.SetZone(70 + I, X, Y, X + 16, Y + 16);
            }
        }

        void ZROB_ARMIE()
        {
            IMIONA_S[0] = "ufo";
            var X = 0;
            var Y = 0;

            for (var L = 0; L <= 2; L++)
            {
                var XG = amos.Rnd(410) + 100;
                var YG = amos.Rnd(300) + 100;
                var B1 = 4 + L;
                for (var K = 0; K <= 2; K++)
                {
                    var AR = (L * 5) + 20 + K;
                    X = XG + amos.Rnd(200) - 100;
                    Y = YG + amos.Rnd(200) - 100;
                    var KON_S = "";
                    if (amos.Upper_S(amos.Right_S(IMIONA_S[L + 2], 1)) == "I")
                    {
                        KON_S = "ego";
                    }
                    else
                    {
                        KON_S = "a";
                    }
                    ARMIA_S[AR, 0] = amos.Str_S(K + 1) + " Legion " + IMIONA_S[L + 2] + KON_S;
                    ARMIA[AR, 0, TX] = X;
                    ARMIA[AR, 0, TY] = Y;
                    ARMIA[AR, 0, TBOB] = B1;
                    ARMIA[AR, 0, TMAG] = L + 2;
                    NOWA_ARMIA(AR, 10, -1);
                    TEREN(X, Y);
                    ARMIA[AR, 0, TNOGI] = LOK;
                    B_DRAW(AR, X, Y, B1);
                }
            }
            //'ustawianie pocz�tkowej za�ogi 
            X = amos.Rnd(600) + 20;
            Y = amos.Rnd(490) + 10;
            NOWA_ARMIA(0, 5, -1);
            ARMIA[0, 0, TX] = X;
            ARMIA[0, 0, TY] = Y;
            ARMIA[0, 0, TBOB] = 3;
            ARMIA[0, 0, TMAG] = 1;
            TEREN(X, Y);
            ARMIA[0, 0, TNOGI] = LOK;
            ARMIA[0, 0, TAMO] = 100;
            B_DRAW(0, X, Y, 3);
        }

        void EXTRO()
        {
            screens.HideOn();
            _LOAD("czacha.32", 3);
            _LOAD("mod.end2", 6);
            amos.TrackLoopOn();
            amos.TrackPlay(3);

            screens.View();
            _WAIT(1500);
            //Fade 2
            _TRACK_FADE(1);
        }

        void _M_FADE(int SPEED)
        {
            var SPEED2 = SPEED;
            var I = 43;
            do {
                I--;
                //Mvolume I
                amos.Wait(SPEED);
            } while (!(I == 0));
            //Music Stop
            //Mvolume 50
        }

        void _TRACK_FADE(int SPEED)
        {
            var SPEED2 = SPEED;
            var I = 63;
            var A = 0;
            var B = 0;
            var C = 0;
            var D = 0;
            //A = Vumeter(0);
            A += -1;
            //B = Vumeter(1);
            B += -1;
            //C = Vumeter(2);
            C += -1;
            //D = Vumeter(3);
            D += -1;
            do
            {
                SPEED2--;
                if (SPEED2 == 0)
                {
                    amos.Add(ref A, -1, 0, A);
                    amos.Add(ref B, -1, 0, B);
                    amos.Add(ref C, -1, 0, C);
                    amos.Add(ref D, -1, 0, D);
                    I--;
                    SPEED2 = SPEED;
                }
                screens.WaitVbl();
                //Doke $DFF0A8,A
                //Doke $DFF0B8,B
                //Doke $DFF0C8,C
                //Doke $DFF0D8,D
            } while (!(I == 5));

            amos.TrackStop();
        }







    }
}
