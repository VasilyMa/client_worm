using System;
using System.IO;
using System.Text;


namespace Net.Utils {

    public class BigEndianBinaryWriter : BinaryWriter {

        // BinaryWriter всегда little endian
        // BitConverter зависит от платформы


        public BigEndianBinaryWriter (Stream output) : base(output) {}
        public BigEndianBinaryWriter (Stream output, Encoding encoding) : base(output, encoding) {}


        public override void Write (short value) {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            OutStream.Write(bytes, 0, sizeof(short));
        }


        public override void Write (int value) {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            OutStream.Write(bytes, 0, sizeof(int));
        }


        public override void Write (long value) {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            OutStream.Write(bytes, 0, sizeof(long));
        }


        public override void Write (ushort value) {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            OutStream.Write(bytes, 0, sizeof(ushort));
        }


        public override void Write (uint value) {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            OutStream.Write(bytes, 0, sizeof(uint));
        }


        public override void Write (ulong value) {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            OutStream.Write(bytes, 0, sizeof(ulong));
        }


        public override void Write (float value) {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            OutStream.Write(bytes, 0, sizeof(float));
        }


        public override void Write (double value) {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            OutStream.Write(bytes, 0, sizeof(double));
        }

    }

}
