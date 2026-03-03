using System;
using System.IO;
using System.Text;


namespace Net.Utils {

    public class BigEndianBinaryReader : BinaryReader {

        // BinaryReader всегда little endian
        // BitConverter зависит от платформы


        public BigEndianBinaryReader (Stream input) : base(input) {}
        public BigEndianBinaryReader (Stream input, Encoding encoding) : base(input, encoding) {}


        public override short ReadInt16 () {
            var bytes = ReadBytes(sizeof(short));
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }


        public override int ReadInt32 () {
            var bytes = ReadBytes(sizeof(int));
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }


        public override long ReadInt64 () {
            var bytes = ReadBytes(sizeof(long));
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }


        public override ushort ReadUInt16 () {
            var bytes = ReadBytes(sizeof(ushort));
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return BitConverter.ToUInt16(bytes, 0);
        }


        public override uint ReadUInt32 () {
            var bytes = ReadBytes(sizeof(uint));
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }


        public override ulong ReadUInt64 () {
            var bytes = ReadBytes(sizeof(ulong));
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }


        public override float ReadSingle () {
            var bytes = ReadBytes(sizeof(float));
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return BitConverter.ToSingle(bytes, 0);
        }


        public override double ReadDouble () {
            var bytes = ReadBytes(sizeof(double));
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return BitConverter.ToDouble(bytes, 0);
        }

    }

}
