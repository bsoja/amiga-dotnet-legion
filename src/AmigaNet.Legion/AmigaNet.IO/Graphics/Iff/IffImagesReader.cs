using AmigaNet.Types.Graphics;
using System.Text;

namespace AmigaNet.IO.Graphics.Iff
{
    public class IffImagesReader : IImagesReader
    {
        private const UInt32 CAMG_MODE_EXTRA_HALFBRITE = 0x0080;
        private const UInt32 CAMG_MODE_HAM = 0x0800;

        public String Name => "IFF Image File";

        public ImagesContainer Read(String fileName)
        {
            var iffImage = ReadIlbm(fileName);

            if (iffImage.HamMode)
            {
                HandleHamMode(iffImage);
            }

            if (iffImage.EhbMode)
            {
                HandleEhbMode(iffImage);
            }

            var imageData = new ImageData(iffImage.Name, iffImage.Width, iffImage.Height, 0, 0);
            for (var i = 0; i < iffImage.Data.Length; i++)
            {
                var idx = iffImage.Data[i];
                imageData.Pixels[i] = iffImage.Palette[idx];
            }

            return new ImagesContainer
            {
                Images = new List<ImageData> { imageData },
                Palette = iffImage.Palette
            };
        }

        private IffImage ReadIlbm(String fileName)
        {
            var iffImage = new IffImage();
            iffImage.Name = Path.GetFileName(fileName);

            var bytes = File.ReadAllBytes(fileName);
            var reader = new BytesReader(bytes);

            var chunkId = Encoding.UTF8.GetString(reader.Read(4));
            var chunkSize = reader.Read32();
            var formSize = chunkSize;

            if (chunkId == "FORM")
            {
                chunkId = Encoding.UTF8.GetString(reader.Read(4));
                if (chunkId == "ILBM")
                {
                    while (reader.Position < formSize)
                    {
                        chunkId = Encoding.UTF8.GetString(reader.Read(4));
                        chunkSize = reader.Read32();
                        var chunkBytes = reader.Read(chunkSize);

                        // padding for odd length chunks
                        if (chunkSize % 2 > 0) reader.Read8();

                        if (chunkId == "BMHD")
                        {
                            ReadBmhd(iffImage, chunkBytes);
                        }
                        if (chunkId == "CMAP")
                        {
                            ReadCmap(iffImage, chunkBytes);
                        }
                        if (chunkId == "CAMG")
                        {
                            ReadCamg(iffImage, chunkBytes);
                        }
                        if (chunkId == "BODY")
                        {
                            ReadBody(iffImage, chunkBytes);
                        }
                    }
                }
            }

            return iffImage;
        }

        private void ReadBmhd(IffImage iffImage, Byte[] chunkBytes)
        {
            var reader = new BytesReader(chunkBytes);

            iffImage.Width = reader.Read16();
            iffImage.Height = reader.Read16();
            iffImage.X = reader.Read16();
            iffImage.Y = reader.Read16();
            iffImage.Bitplanes = reader.Read8();
            iffImage.Masking = reader.Read8();
            iffImage.Compression = reader.Read8();
            reader.Read8(); // pad1 - unused
            iffImage.TransparentColor = reader.Read16();
            iffImage.AspectX = reader.Read8();
            iffImage.AspectY = reader.Read8();
            iffImage.PageWidth = reader.Read16();
            iffImage.PageHeight = reader.Read16();
            iffImage.Data = new Byte[iffImage.Width * iffImage.Height];
        }

        private void ReadCmap(IffImage iffImage, Byte[] chunkBytes)
        {
            var reader = new BytesReader(chunkBytes);

            var colorsCount = chunkBytes.Length / 3;
            iffImage.Palette = new Pixel[colorsCount];
            for (var i = 0; i < colorsCount; i++)
            {
                var r = reader.Read8();
                var g = reader.Read8();
                var b = reader.Read8();
                var pixel = new Pixel(r, g, b, i);
                iffImage.Palette[i] = pixel;
            }
        }

        private void ReadCamg(IffImage iffImage, Byte[] chunkBytes)
        {
            var reader = new BytesReader(chunkBytes);

            var val = reader.Read32();
            iffImage.EhbMode = (val & CAMG_MODE_EXTRA_HALFBRITE) == CAMG_MODE_EXTRA_HALFBRITE;
            iffImage.HamMode = (val & CAMG_MODE_HAM) == CAMG_MODE_HAM;
            //TODO: set other modes if needed
        }

        private void ReadBody(IffImage iffImage, Byte[] chunkBytes)
        {
            var srcBytes = chunkBytes;

            if (iffImage.Compression == 1)
            {
                var length = iffImage.Width * iffImage.Height * iffImage.Bitplanes / 8;
                srcBytes = UnpackBits(chunkBytes, length);
            }

            var bytesPerScanline = iffImage.Bitplanes * iffImage.Width / 8;
            // iterate over scanlines
            for (var y = 0; y < iffImage.Height; y++)
            {
                // iterate over bitplanes
                for (var bp = 0; bp < iffImage.Bitplanes; bp++)
                {
                    var bitplaneBitPos = bp * iffImage.Width;
                    // iterate over row bits
                    for (var x = 0; x < iffImage.Width; x++)
                    {
                        var byteNr = (y * bytesPerScanline) + ((x + bitplaneBitPos) / 8);

                        if ((srcBytes[byteNr] & (1 << (7 - (x % 8)))) != 0)
                        {
                            iffImage.Data[x + (y * iffImage.Width)] |= (Byte)(1 << bp);
                        }
                    }
                    //TODO: if masking enabled then read additional row with mask data
                }
            }
        }

        private Byte[] UnpackBits(Byte[] bytes, Int32 length)
        {
            var destBytes = new List<Byte>();

            var reader = new BytesReader(bytes);
            while (destBytes.Count < length)
            {
                var b = (SByte)reader.Read8();

                if (b == SByte.MinValue)
                {
                    continue;
                }
                if (b >= 0)
                {
                    destBytes.AddRange(reader.Read(b + 1));
                }
                else if (b < 0)
                {
                    var count = -b + 1;
                    var nextByte = reader.Read8();
                    for (var x = 0; x < count; x++)
                    {
                        destBytes.Add(nextByte);
                    }
                }
            }

            return destBytes.ToArray();
        }

        private void HandleEhbMode(IffImage iffImage)
        {
            // handle EHB - Extra-Halfbrite mode
            var extendedPalette = new Pixel[iffImage.Palette.Length * 2];
            Array.Copy(iffImage.Palette, extendedPalette, iffImage.Palette.Length);

            for (var i = 0; i < iffImage.Palette.Length; i++)
            {
                var index = iffImage.Palette.Length + i;
                var p = iffImage.Palette[i];
                var halfBritePixel = new Pixel((Byte)(p.R >> 1), (Byte)(p.G >> 1), (Byte)(p.B >> 1), index);
                extendedPalette[index] = halfBritePixel;
            }
            iffImage.Palette = extendedPalette;
        }

        private void HandleHamMode(IffImage iffImage)
        {
            // TODO: handle HAM mode
        }
    }
}
