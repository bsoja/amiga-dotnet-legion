using System.Text;

namespace AmigaNet.IO.Fonts
{
    public class FontsReader
    {
		public FontInfo Read(String fileName)
		{
			var bytes = File.ReadAllBytes(fileName);
			var reader = new BytesReader(bytes);

			var fontInfo = new FontInfo();

			reader.Read(32 + 26); //ignore

			var nameBytes = reader.Read(32);
			fontInfo.FontName = Encoding.UTF8.GetString(nameBytes);

			reader.Read(20); //ignore

			fontInfo.YSize = reader.Read16();
			fontInfo.Style = reader.Read8();
			fontInfo.Flags = reader.Read8();
			fontInfo.XSize = reader.Read16();
			fontInfo.Baseline = reader.Read16();
			fontInfo.BoldSmear = reader.Read16();
			reader.Read16(); //ignore - accessors
			fontInfo.LoChar = reader.Read8();
			fontInfo.HiChar = reader.Read8();
			fontInfo.Numchars = fontInfo.HiChar - fontInfo.LoChar + 2;
			fontInfo.FontDataStart = reader.Read32() + 32;
			fontInfo.Modulo = reader.Read16();
			fontInfo.LocationDataStart = reader.Read32() + 32;

			reader.Seek(fontInfo.LocationDataStart);

			fontInfo.Location = new int[fontInfo.Numchars];
			fontInfo.Bitlength = new int[fontInfo.Numchars];

			for (var i = 0; i < fontInfo.Numchars; i++)
			{
				fontInfo.Location[i] = reader.Read16();
				fontInfo.Bitlength[i] = reader.Read16();
			}

			reader.Seek(fontInfo.FontDataStart);
			fontInfo.FontData = reader.Read(fontInfo.Modulo * fontInfo.YSize);

			return fontInfo;
		}
	}
}
