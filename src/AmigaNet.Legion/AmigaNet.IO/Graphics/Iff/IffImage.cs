using AmigaNet.Types.Graphics;

namespace AmigaNet.IO.Graphics.Iff
{
    internal class IffImage
    {
        public String Name { get; set; }

        public Int16 Width { get; set; }

        public Int16 Height { get; set; }

        public Int16 X { get; set; }

        public Int16 Y { get; set; }

        public Byte Bitplanes { get; set; }

        public Byte Masking { get; set; }

        public Byte Compression { get; set; }

        public Int16 TransparentColor { get; set; }

        public Byte AspectX { get; set; }

        public Byte AspectY { get; set; }

        public Int16 PageWidth { get; set; }

        public Int16 PageHeight { get; set; }

        public bool EhbMode { get; set; }

        public bool HamMode { get; set; }

        public Byte[] Data { get; set; }

        public Pixel[] Palette { get; set; }
    }
}
