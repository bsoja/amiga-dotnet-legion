using AmigaNet.Types.Graphics;

namespace AmigaNet.Amos.Screens
{
    public class Shape : IGraphicElement
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Pixel[] Data { get; set; }

        public bool IsModified { get; set; }
    }
}
