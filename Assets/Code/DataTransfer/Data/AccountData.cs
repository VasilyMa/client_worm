using System.IO;


namespace DataTransfer.Data {

    public class AccountData : IDeserializable {

        public int       Id    { get; private set; }
        public int       Money { get; private set; }
        public int       Cards { get; private set; }
        public int       Stars { get; private set; }
        public string [] Worms { get; private set; }


        public void ReadMembers (BinaryReader reader) {
            Id    = reader.ReadInt32 ();
            Money = reader.ReadInt32 ();
            Cards = reader.ReadInt32 ();
            Stars = reader.ReadInt32 ();
            
            byte n = reader.ReadByte ();
            Worms = new string [n];
            for (int i = 0; i < n; i++) {
                Worms [i] = reader.ReadString ();
            }
        }

    }

}