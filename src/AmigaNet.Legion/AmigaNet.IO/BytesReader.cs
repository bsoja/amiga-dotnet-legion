using System.Text;

namespace AmigaNet.IO
{
    public class BytesReader
    {
        private readonly Byte[] data;
        private Int32 pos = 0;

        public BytesReader(Byte[] data)
        {
            this.data = data;
        }

        public Int32 Position => pos;

        public Int32 Length => data.Length;

        public void Seek(Int32 pos)
        {
            this.pos = pos;
        }

        public Byte[] Read(Int32 count)
        {
            if (pos + count > data.Length)
            {
                throw new ArgumentException();
            }

            var subArray = new Byte[count];
            Array.Copy(data, pos, subArray, 0, count);

            pos += count;

            return subArray;
        }

        public Byte Read8()
        {
            return data[pos++];
        }

        public Int16 Read16()
        {
            var arr = Read(2);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(arr);
            }
            return BitConverter.ToInt16(arr);
        }

        public Int32 Read32()
        {
            var arr = Read(4);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(arr);
            }
            return BitConverter.ToInt32(arr);
        }

        public String ReadText(Int32 length)
        {
            var arr = Read(length);
            var text = Encoding.UTF8.GetString(arr);
            return text;
        }
    }
}
