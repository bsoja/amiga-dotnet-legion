using System.Text;

namespace AmigaNet.IO
{
    public class BytesWriter
    {
        private readonly Byte[] data;
        private Int32 pos = 0;

        public BytesWriter(Int32 length)
        {
            data = new Byte[length];
        }

        public Byte[] Data => data;

        public void Write(Byte[] arr)
        {
            foreach (Byte b in arr)
            {
                Write8(b);
            }
        }

        public void Write8(Byte val)
        {
            data[pos++] = val;
        }

        public void Write16(Int16 val)
        {
            var arr = BitConverter.GetBytes(val);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(arr);
            }
            Write(arr);
        }

        public void Write32(Int32 val)
        {
            var arr = BitConverter.GetBytes(val);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(arr);
            }
            Write(arr);
        }

        public void WriteText(String s)
        {
            var arr = Encoding.UTF8.GetBytes(s);
            Write(arr);
        }
    }
}
