using AmigaNet.Types.Graphics;

namespace AmigaNet.Amos.Screens
{
    internal class Drawing
    {
        public static void PlotLine(ImageData data, Pixel pixel, int x0, int y0, int x1, int y1)
        {
            if (Math.Abs(y1 - y0) < Math.Abs(x1 - x0))
            {
                if (x0 > x1)
                    PlotLineLow(data, pixel, x1, y1, x0, y0);
                else
                    PlotLineLow(data, pixel, x0, y0, x1, y1);
            }
            else
            {
                if (y0 > y1)
                    PlotLineHigh(data, pixel, x1, y1, x0, y0);
                else
                    PlotLineHigh(data, pixel, x0, y0, x1, y1);
            }
        }

        static void PlotLineLow(ImageData data, Pixel pixel, int x0, int y0, int x1, int y1)
        {
            var dx = x1 - x0;
            var dy = y1 - y0;
            var yi = 1;
            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }
            var D = (2 * dy) - dx;
            var y = y0;

            for (var x = x0; x <= x1; x++)
            {
                Plot(data, pixel, x, y);
                if (D > 0)
                {
                    y = y + yi;
                    D = D + (2 * (dy - dx));
                }
                else
                {
                    D = D + 2 * dy;
                }
            }
        }

        static void PlotLineHigh(ImageData data, Pixel pixel, int x0, int y0, int x1, int y1)
        {
            var dx = x1 - x0;
            var dy = y1 - y0;
            var xi = 1;
            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }
            var D = (2 * dx) - dy;
            var x = x0;

            for (var y = y0; y <= y1; y++)
            {
                Plot(data, pixel, x, y);
                if (D > 0)
                {
                    x = x + xi;
                    D = D + (2 * (dx - dy));
                }
                else
                {
                    D = D + 2 * dx;
                }
            }
        }

        static void Plot(ImageData data, Pixel pixel, int x, int y)
        {
            if (x < 0 || y < 0 || x > data.Width || y > data.Height) return;
            data.Pixels[x + data.Width * y] = pixel;
        }
    }
}
