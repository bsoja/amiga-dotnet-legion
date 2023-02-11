using AmigaNet.Amos;
using AmigaNet.IO;
using System.Text;

namespace AmigaNet.Legion
{
    public partial class Legion
    {
        const string SAVE_FOLDER_NAME = "archiwum";

        /// <summary>
        /// Procedure ODCZYT[MEM]
        /// </summary>
        /// <param name="path"></param>
        /// <param name="></param>
        public void Load(string path)
        {
            var bytes = File.ReadAllBytes(path);
            var reader = new BytesReader(bytes);
            var archiveName = reader.ReadText(20);

            for (var i = 0; i <= 40; i++)
            {
                for (var j = 0; j <= 10; j++)
                {
                    for (var k = 0; k <= 30; k++)
                    {
                        ARMIA[i, j, k] = reader.Read16();
                    }
                }
            }

            for (var i = 0; i <= 5; i++)
            {
                for (var j = 0; j <= 5; j++)
                {
                    WOJNA[i, j] = reader.Read8();
                }
            }

            for (var i = 0; i <= 4; i++)
            {
                for (var j = 0; j <= 3; j++)
                {
                    GRACZE[i, j] = reader.Read32();
                }
            }

            for (var i = 0; i <= 40; i++)
            {
                for (var j = 0; j <= 10; j++)
                {
                    var length = reader.Read8();
                    ARMIA_S[i, j] = reader.ReadText(length);
                }
            }

            for (var i = 0; i <= 4; i++)
            {
                var length = reader.Read8();
                IMIONA_S[i] = reader.ReadText(length);
            }

            for (var i = 0; i <= 10; i++)
            {
                PREFS[i] = reader.Read8();
            }

            for (var i = 0; i <= 50; i++)
            {
                for (var j = 0; j <= 20; j++)
                {
                    for (var k = 0; k <= 6; k++)
                    {
                        MIASTA[i, j, k] = reader.Read16();
                    }
                }
            }

            for (var i = 0; i <= 50; i++)
            {
                var length = reader.Read8();
                MIASTA_S[i] = reader.ReadText(length);
            }

            DZIEN = reader.Read16();
            POWER = reader.Read16();

            for (var i = 0; i <= 3; i++)
            {
                for (var j = 0; j <= 10; j++)
                {
                    PRZYGODY[i, j] = reader.Read16();
                }
            }

            for (var i = 0; i <= 3; i++)
            {
                var length = reader.Read8();
                IM_PRZYGODY_S[i] = reader.ReadText(length);
            }
        }

        public void Save(string path, string name)
        {
            var writer = new BytesWriter(60000);
            writer.WriteText($"{name,-20}");

            //'armia(40,10,30) 
            for (var I = 0; I <= 40; I++)
            {
                for (var J = 0; J <= 10; J++)
                {
                    for (var K = 0; K <= 30; K++)
                    {
                        var DAT = ARMIA[I, J, K];
                        writer.Write16((Int16)DAT);
                    }
                }
            }
            //'wojna(5,5)
            for (var I = 0; I <= 5; I++)
            {
                for (var J = 0; J <= 5; J++)
                {
                    var DAT = WOJNA[I, J];
                    writer.Write8((Byte)DAT);
                }
            }
            //'gracze(4,3) 
            for (var I = 0; I <= 4; I++)
            {
                for (var J = 0; J <= 3; J++)
                {
                    var DAT = GRACZE[I, J];
                    writer.Write32(DAT);
                }
            }
            //'armia_S(40,10) 
            for (var I = 0; I <= 40; I++)
            {
                for (var J = 0; J <= 10; J++)
                {
                    var DAT_S = ARMIA_S[I, J];
                    if (string.IsNullOrEmpty(DAT_S)) DAT_S = "";
                    writer.Write8((Byte)DAT_S.Length);
                    writer.WriteText(DAT_S);
                }
            }
            //'imiona_S(4)
            for (var I = 0; I <= 4; I++)
            {
                var DAT_S = IMIONA_S[I];
                writer.Write8((Byte)DAT_S.Length);
                writer.WriteText(DAT_S);
            }
            //'prefs(10) 
            for (var I = 0; I <= 10; I++)
            {
                var DAT = PREFS[I];
                writer.Write8((Byte)DAT);
            }
            //'MIASTA[50, 20, 6] 
            for (var I = 0; I <= 50; I++)
            {
                for (var J = 0; J <= 20; J++)
                {
                    for (var K = 0; K <= 6; K++)
                    {
                        var DAT = MIASTA[I, J, K];
                        writer.Write16((Int16)DAT);
                    }
                }
            }
            //'miasta_S(50) 
            for (var I = 0; I <= 50; I++)
            {
                var DAT_S = MIASTA_S[I];
                writer.Write8((Byte)DAT_S.Length);
                writer.WriteText(DAT_S);
            }
            writer.Write16((Int16)DZIEN);
            writer.Write16((Int16)POWER);
            //'przygody(3,10)
            for (var I = 0; I <= 3; I++)
            {
                for (var J = 0; J <= 10; J++)
                {
                    var DAT = PRZYGODY[I, J];
                    writer.Write16((Int16)DAT);
                }
            }
            //'im_przygody_S(3) 
            for (var I = 0; I <= 3; I++)
            {
                var DAT_S = IM_PRZYGODY_S[I];
                writer.Write8((Byte)DAT_S.Length);
                writer.WriteText(DAT_S);
            }

            var bytes = writer.Data;
            File.WriteAllBytes(path, bytes);
        }


        void _SAVE_GAME()
        {
            var KONIEC = false;

            SDIR("Archiwum - Zapis Gry", 21, 20);

            do {
                if (screens.MouseClick() == 1)
                {
                    var STREFA = screens.MouseZone();
                    if (STREFA > 0 && STREFA < 6)
                    {
                        var NSAVE = STREFA;
                        WPISZ(OKX + 14, OKY + 38 + ((STREFA - 1) * 20), 31, 6, 20);
                        var NAME_S = WPI_S;
                        if (NAME_S == "")
                        {
                            NAME_S = "Zapis " + amos.Str_S(STREFA);
                        }

                        BUSY_ANIM();
                        Save(Path.Combine(SAVE_FOLDER_NAME, "zapis" + amos.Str_S(NSAVE)), NAME_S);
                        screens.ChangeMouse(42);
                        screens.ChangeMouse(5);
                    }
                    if (STREFA == 6)
                    {
                        KONIEC = true;
                    }
                }
            } while (!KONIEC);
            ZOKNO();
        }


        String SDIR(String A_S, int K1, int K2)
        {
            OKNO(100, 60, 140, 160);
            GADGET(OKX + 10, OKY + 8, 120, 15, A_S, K1, 0, K2, 31, -1);
            var PAT_S = "";
            Directory.CreateDirectory("archiwum");

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
                GADGET(OKX + 10, OKY + 28 + (I * 20), 120, 15, NAME_S, 8, 1, 6, 31, I + 1);
            }
            GADGET(OKX + 10, OKY + 128, 120, 15, "Exit", 8, 1, 6, 31, 6);
            return PAT_S;
        }



    }
}
