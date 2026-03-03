using System.IO;
using System.Text;
using Math;


namespace DataTransfer.Data {

    public class TurnData : ISerializable, IDeserializable {

        public bool W, A, S, D, MB; // byte - space reserved for Tab etc.
        public XY   XY;             // 2 floats
        public byte Weapon;         // byte
        public byte NumKey;         // 3 bits: 0-5
        
        // todo:
        // сделать кнопки с цифрами отдельно
        // таким образом у нас будут битовые флаги W A S D мышка 1 2 3 4 5 - 10 бит
        // возможно будет для веревки клавиша или еще какие-нибудь
        // под оружие останется 1 байт

        public byte Flags {
            get {
                return (byte) (
                    (W  ? 0x01 : 0) |
                    (A  ? 0x02 : 0) |
                    (S  ? 0x04 : 0) |
                    (D  ? 0x08 : 0) |
                    (MB ? 0x10 : 0)
                );
            }
            set {
                W  = (value & 0x01) != 0;
                A  = (value & 0x02) != 0;
                S  = (value & 0x04) != 0;
                D  = (value & 0x08) != 0;
                MB = (value & 0x10) != 0;
            }
        }


        public bool Empty => !(W || A || S || D || MB || NumKey != 0);


        public void ReadMembers (BinaryReader reader) {
            Flags  = reader.ReadByte ();
            XY     = new XY (reader.ReadSingle (), reader.ReadSingle ());
            Weapon = reader.ReadByte ();
            NumKey = reader.ReadByte ();
        }


        public void WriteMembers (BinaryWriter writer) {
            writer.Write (Flags);
            writer.Write (XY.X);
            writer.Write (XY.Y);
            writer.Write (Weapon);
            writer.Write (NumKey);
        }


        public override string ToString () {
            var sb = new StringBuilder ();
            sb.Append (W ? "W" : "w");
            sb.Append (A ? "A" : "a");
            sb.Append (S ? "S" : "s");
            sb.Append (D ? "D" : "d");
            sb.Append (NumKey);
            sb.Append (" ");
            sb.Append (MB ? "MB" : "mb");
            sb.Append (" ");
            sb.Append (Weapon);
            return sb.ToString ();
        }

    }

}