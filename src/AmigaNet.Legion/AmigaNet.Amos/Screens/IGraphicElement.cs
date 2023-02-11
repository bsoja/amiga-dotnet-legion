using AmigaNet.Types.Graphics;

namespace AmigaNet.Amos.Screens
{
    public interface IGraphicElement
    {
        int X { get; }

        int Y { get; }

        int Width { get; }

        int Height { get; }

        Pixel[] Data { get; }

        bool IsModified { get; set; }
    }
}