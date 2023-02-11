using AmigaNet.IO.Graphics.Amos;
using AmigaNet.Types.Graphics;

namespace AmigaNet.Amos.MemoryBanks
{
    public class MemoryBanksManager
    {
        private readonly IGameEngine gameEngine;

        public MemoryBanksManager(IGameEngine gameEngine)
        {
            this.gameEngine = gameEngine;
        }

        private readonly MemoryBank<ImageData> bank1 = new MemoryBank<ImageData>();
        //private MemoryBank<ImageData> bank2;
        //private MemoryBank<MusicData> bank3;
        //private MemoryBank<AmalData> bank4;
        //private MemoryBank<AudioSamplesData> bank5;

        public MemoryBank<ImageData> Bank1 => bank1;

        public Pixel[] BobPalette { get; private set; }

        public void Load(String fileName)
        {
            Load(fileName, 1);
        }

        public void Load(String fileName, int bankNumber)
        {
            if (bankNumber == 1)
            {
                var sprites = new SpriteBanksReader().Read(fileName);
                bank1.Data.AddRange(sprites.Images.Where(s => s.Width > 0));
                BobPalette = sprites.Palette;
            }
        }

        public void TrackLoad(String fileName, int bankNumber)
        {
            gameEngine.LoadTrack(fileName);
        }

        /// <summary>
        /// ERASE
        /// instruction: clear a single memory bank
        /// Erase bank number
        /// </summary>
        public void Erase(int bankNumber)
        {
            switch (bankNumber)
            {
                case 1: bank1.Data.Clear(); break;
                    //TODO: handle other banks if needed
            }
        }

        /// <summary>
        /// ERASE ALL
        /// instruction: clear all current memory banks
        /// Erase All
        /// </summary>
        public void EraseAll()
        {
            //TODO: for each bank
            bank1.Data.Clear();
        }

        public void Set(int bankNumber, int imageNumber, ImageData imageData)
        {
            if (bankNumber == 1)
            {
                if (Bank1.Data.Count < imageNumber)
                {
                    var diff = imageNumber - Bank1.Data.Count;
                    for (var i =0; i<diff; i++)
                    {
                        Bank1.Data.Add(null);
                    }
                }
                Bank1.Data[imageNumber - 1] = imageData;
            }
        }

    }
}
