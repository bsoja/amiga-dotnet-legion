using AmigaNet.Types.Graphics;

namespace AmigaNet.Amos.Screens
{
    public class Sprite
    {
        public int X { get; set; }
        
        public int Y { get; set; }

        public ImageData Data { get; set; }

        public bool IsModified { get; set; } = false;
    }
}