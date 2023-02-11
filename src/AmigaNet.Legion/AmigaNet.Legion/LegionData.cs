namespace AmigaNet.Legion
{
    public partial class Legion
    {
        public string KAT_S;

        public int LEWY = 1;
        public int PRAWY = 2;

        public int[,,] ARMIA = new int[41, 11, 31];
        public int[,] WOJNA = new int[6, 6];
        public int[,] GRACZE = new int[5, 4];

        public string[,] ARMIA_S = new string[41, 11];
        public string[] IMIONA_S = new string[5];

        public int[] AN = new int[4] { 0, 1, 0, 2 };

        public double[,] VEKTOR_R = new double[21, 6];

        public int[] PREFS = new int[11];

        public int[,,] MIASTA = new int[51, 21, 7];
        public string[] MIASTA_S = new string[51];
        public int[] MUR = new int[11];
        public int[,] SKLEP = new int[21, 22];
        public int[] STRZALY = new int[11];

        public int TEM = 0, TX = 1, TY = 2, TSI = 3, TSZ = 4, TCELX = 5, TCELY = 6, TTRYB = 7, TE = 8, TP = 9,
            TBOB = 10, TKLAT = 11, TAMO = 12, TLEWA = 16, TPRAWA = 17, TNOGI = 15, TGLOWA = 13,
            TPLECAK = 18, TKORP = 14, TMAG = 26, TDOSW = 27, TRASA = 28, TWAGA = 29, TMAGMA = 30;

        public int M_X = 1, M_Y = 2, M_LUDZIE = 3, M_PODATEK = 4, M_CZYJE = 5, M_MORALE = 6, M_MUR = 0;

        public int OX, OY, NUMER, ARM = 0, WRG = 40, SX, SY, MSX, MSY, FONT, FONR, LOK, DZIEN, FON1, FON2;
        public int FONTSZ = 5;

        public int WPI, IMIONA, ODLEG, WIDOCZNOSC, BUBY, BIBY, BSIBY, PIKIETY, POTWORY;
        public string WPI_S;
        public double WPI_R;

        public int SCENERIA, LAST_GAD, KANAL, POWER, REZULTAT, GOBY, KONIEC_INTRA;
        public bool MUZYKA;
        public int CENTER_V = 100;

        public int[,] RASY = new int[21, 8];
        public string[] RASY_S = new string[21];

        public int[,] BRON = new int[121, 12];
        public string[] BRON_S = new string[121];
        public string[] BRON2_S = new string[26];

        public int[,] GLEBA = new int[111, 5];
        public int[,] PLAPKI = new int[11, 5];

        public int[,] BUDYNKI = new int[13, 7];
        public string[] BUDYNKI_S = new string[13];
        public string[] GUL_S = new string[11];

        public string[,] ROZMOWA_S = new string[6, 6];
        public string[] ROZMOWA2_S = new string[51];
        public string[,] PRZYGODY_S = new string[21, 11];
        public string[] IM_PRZYGODY_S = new string[4];
        public int[,] PRZYGODY = new int[4, 11];

        public int TRWA_PRZYGODA;

        public int P_TYP = 0, P_X = 1, P_Y = 2, P_TERMIN = 3, P_KIERUNEK = 4, P_LEVEL = 5, P_CENA = 6, P_NAGRODA = 7, P_BRON = 8, P_TEREN = 9, P_STAREX = 10;

        public int BROBY = 15;
        public int B_SI = 1, B_PAN = 2, B_SZ = 3, B_EN = 4, B_TYP = 5, B_WAGA = 6;
        public int B_PLACE = 7, B_DOSW = 8, B_MAG = 9, B_CENA = 10, B_BOB = 11;
        public int OKX, OKY, SPX, SPY, WYNIK_AKCJI, TESTING, CELOWNIK;
        public bool REAL_KONIEC;
        public bool KONIEC_AKCJI;
        public int KTO_ATAKUJE = -1;
        public int _MODULO, SUPERVISOR, MX_WEAPON;
        public bool GAME_OVER;

    }
}



