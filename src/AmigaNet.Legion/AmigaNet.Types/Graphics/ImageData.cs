namespace AmigaNet.Types.Graphics
{
    public class ImageData
    {
        private readonly String name;
        private readonly Pixel[] pixels;
        private readonly Int32 width;
        private readonly Int32 height;
        //private readonly Int32 hotspotX;
        //private readonly Int32 hotspotY;

        public ImageData(String name, Pixel[] pixels, Int32 width, Int32 height, Int32 hotspotX = 0, Int32 hotspotY = 0)
        {
            this.name = name;
            this.pixels = pixels;
            this.width = width;
            this.height = height;
            //this.hotspotX = hotspotX;
            //this.hotspotY = hotspotY;
            this.HotspotX = hotspotX;
            this.HotspotY = hotspotY;
        }

        public ImageData(String name, Int32 width, Int32 height, Int32 hotspotX = 0, Int32 hotspotY = 0)
            : this(name, new Pixel[width * height], width, height, hotspotX, hotspotY)
        {
        }

        public String Name => name;

        public Int32 Width => width;

        public Int32 Height => height;

        public Pixel[] Pixels => pixels;

        public Int32 HotspotX { get; set; }// => hotspotX;

        public Int32 HotspotY { get; set; }// => hotspotY;


        /// <summary>
        /// A "mask" means that the background colour (colour zero) around an image is made transparent, so that the screen
        /// graphics show through. A mask is automatically set up for every image, and the NO MASK command takes away this mask, 
        /// so that the entire image is drawn on the screen, including its original background colour and any other graphics in colour zero.
        /// </summary>
        public Boolean UseMask { get; set; } = true;
    }
}
