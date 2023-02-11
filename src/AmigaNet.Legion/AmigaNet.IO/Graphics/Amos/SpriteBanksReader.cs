using AmigaNet.Types.Graphics;
using System.Text;

namespace AmigaNet.IO.Graphics.Amos
{
    public class SpriteBanksReader : IImagesReader
    {
        private const String AmosSpriteBankHeader = "AmSp";
        private const String AmosIconBankHeader = "Amlc";

        public String Name => "AMOS Sprite Bank File";

        public ImagesContainer Read(String fileName)
        {
            var container = new ImagesContainer { Images = new List<ImageData>() };

            var name = Path.GetFileName(fileName);
            var bytes = File.ReadAllBytes(fileName);
            var reader = new BytesReader(bytes);

            var headerId = Encoding.UTF8.GetString(reader.Read(4));
            if (headerId == AmosSpriteBankHeader || headerId == AmosIconBankHeader)
            {
                container.Palette = ReadPalette(bytes);

                var spritesCount = reader.Read16();
                for (var si = 0; si < spritesCount; si++)
                {
                    var widthWords = reader.Read16();
                    var widthPx = widthWords * 16;
                    var heightPx = reader.Read16();
                    var bitplanes = reader.Read16();
                    var hotSpotX = reader.Read16();
                    var hotSpotY = reader.Read16();
                    var length = widthWords * 2 * heightPx * bitplanes;
                    var bitsPerPlane = widthPx * heightPx;
                    var destSize = widthPx * heightPx;

                    //NOTE: dest data will contains list of indexes to color palette
                    var destData = new Byte[destSize];
                    var srcData = reader.Read(length);

                    // iterate over bitplanes
                    for (var bp = 0; bp < bitplanes; bp++)
                    {
                        var bitplanePos = bp * bitsPerPlane;
                        // iterate over bits
                        for (var x = 0; x < bitsPerPlane; x++)
                        {
                            var byteNr = (x + bitplanePos) / 8;
                            if ((srcData[byteNr] & (1 << (7 - (x % 8)))) != 0)
                            {
                                var destByte = destData[x];
                                destData[x] = (Byte)(destByte | (1 << bp));
                            }
                        }
                    }

                    var imageName = $"{name} - {si}";
                    var imageData = new ImageData(imageName, widthPx, heightPx, hotSpotX, hotSpotY);
                    for (var i = 0; i < destData.Length; i++)
                    {
                        imageData.Pixels[i] = container.Palette[destData[i]];
                    }
                    container.Images.Add(imageData);
                }
            }
            return container;
        }

        private Pixel[] ReadPalette(Byte[] fileBytes)
        {
            var paletteBytes = new Byte[64];
            // Palette bytes are located at the end of file
            Array.Copy(fileBytes, fileBytes.Length - 64, paletteBytes, 0, 64);
            var paletteReader = new BytesReader(paletteBytes);
            var palette = new Pixel[32];
            // 64 bytes: a 32-entry colour palette. Each entry has the Amiga COLORx hardware register format,
            // which is 0x0RGB, where R, G and B represent red, green and blue colour components
            // and are between 0x0 (minimum) and 0xF (maximum).
            for (var i = 0; i < 32; i++)
            {
                //0x0RGB
                var color = paletteReader.Read16();

                var r = (color & 0x0F00) >> 8;
                r |= r << 4; // convert 4 bits color to 8 bits

                var g = (color & 0x00F0) >> 4;
                g |= g << 4; // convert 4 bits color to 8 bits

                var b = color & 0x000F;
                b |= b << 4; // convert 4 bits color to 8 bits

                palette[i] = new Pixel((Byte)r, (Byte)g, (Byte)b, i);
            }

            return palette;
        }
    }
}
