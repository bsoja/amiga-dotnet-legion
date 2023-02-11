namespace AmigaNet.Types.Graphics
{
    public class Pixel
    {
        public const Int32 UNUSED_INDEX = -1;

        private readonly Int32 index;
        private readonly Byte r;
        private readonly Byte g;
        private readonly Byte b;

        public Pixel(Byte r, Byte g, Byte b, Int32 index = UNUSED_INDEX)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.index = index;
        }

        /// <summary>
        /// Optional index from color palette
        /// </summary>
        public Int32 Index => index;

        /// <summary>
        /// Red color intensity
        /// </summary>
        public Byte R => r;

        /// <summary>
        /// Green color intensity
        /// </summary>
        public Byte G => g;

        /// <summary>
        /// Blue color intensity
        /// </summary>
        public Byte B => b;

        public static Pixel Black => new Pixel(0, 0, 0);

        public static Pixel White => new Pixel(255, 255, 255);

        public override String ToString()
        {
            return $"R: {R}, G: {G}, B: {B}";
        }
    }
}
