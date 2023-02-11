using AmigaNet.Types.Graphics;

namespace AmigaNet.IO.Fonts
{
	public class FontInfo
	{
		public String FontName { get; internal set; }

		public Int32 YSize { get; internal set; }

		public Int32 XSize { get; internal set; }

		public Byte Style { get; internal set; }

		public Byte Flags { get; internal set; }

		public Int32 Baseline { get; internal set; }

		public Int32 BoldSmear { get; internal set; }

		public Byte LoChar { get; internal set; }

		public Byte HiChar { get; internal set; }

		public Int32 Numchars { get; internal set; }

		public Int32 FontDataStart { get; internal set; }

		public Int32 Modulo { get; internal set; }

		public Int32 LocationDataStart { get; internal set; }

		public Int32[] Location { get; internal set; }

		public Int32[] Bitlength { get; internal set; }

		public Byte[] FontData { get; internal set; }

		public Int32 GetCharWidth(Char ch)
		{
			var a = (Int32)ch;
			if (a > Bitlength.Length) return 0;
			var width = Bitlength[a];
			return width;
		}

		public ImageData GetCharData(Char ch)
		{
			var a = (Int32)ch;
			var width = GetCharWidth(ch);
			if (width == 0) return null;

			var height = YSize;
			var imageData = new ImageData("Char: " + ch, width, height);

			for (var y = 0; y < YSize; y++)
			{
				for (var x = 0; x < Bitlength[a]; x++)
				{
					var k = Location[a] + x + y * Modulo * 8;
					var pos = k / 8;
					var ind = 7 - (k - pos * 8);
					var c = 0;

					if ((FontData[pos] & (1 << ind)) > 0) { c = 1; } else { c = 0; }
					imageData.Pixels[x + width * y] = c > 0 ? Pixel.White : Pixel.Black;
				}
			}

			return imageData;
		}

	}
}
