namespace AmigaNet.Legion
{
    public partial class Legion
    {
        private void WCZYTAJ_BRON()
        {
            for (var i = 1; i <= 19; i++)
            {
                BRON2_S[i] = amos.ReadS();
            }

            MX_WEAPON = 120;

            for (var i = 1; i <= MX_WEAPON; i++)
            {
                BRON_S[i] = amos.ReadS();
                for (var j = 1; j <= 11; j++)
                {
                    BRON[i, j] = amos.Read();
                }
            }

            _MODULO = 50 + amos.Rnd(130);
        }

        private void WCZYTAJ_RASY()
        {
            for (var i = 0; i <= 19; i++)
            {
                RASY_S[i] = amos.ReadS();

                for (var j = 0; j <= 7; j++)
                {
                    RASY[i, j] = amos.Read();
                }
            }
        }

        private void WCZYTAJ_BUDYNKI()
        {
            for (var i = 1; i <= 9; i++)
            {
                BUDYNKI_S[i] = amos.ReadS();

                for (var j = 0; j <= 6; j++)
                {
                    BUDYNKI[i, j] = amos.Read();
                }
            }
        }

        private void WCZYTAJ_ROZMOWE()
        {
            for (var i = 0; i <= 33; i++)
            {
                ROZMOWA2_S[i] = amos.ReadS();
            }
            for (var i = 1; i <= 2; i++)
            {
                for (var j = 0; j <= 4; j++)
                {
                    ROZMOWA_S[i, j] = amos.ReadS();
                }
            }
        }

        private void WCZYTAJ_PRZYGODY()
        {
            for (var i = 1; i <= 13; i++)
            {
                for (var j = 0; j <= 8; j++)
                {
                    PRZYGODY_S[i, j] = amos.ReadS();
                }
            }
            for (var i = 0; i <= 3; i++)
            {
                PRZYGODY[i, P_TYP] = 0;
            }
        }

        private void WCZYTAJ_GULE()
        {
            for (var i = 0; i <= 9; i++)
            {
                GUL_S[i] = amos.ReadS();
            }
        }
    }
}



