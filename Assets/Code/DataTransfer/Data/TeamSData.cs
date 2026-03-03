using System.IO;


namespace DataTransfer.Data {

    public class TeamSData : IDeserializable {

        public enum TeamType : byte { Player = 0, AI = 1 }


        public TeamType  Type  { get; private set; }
        public int       Id    { get; private set; }
        public byte      Color { get; private set; }
        public string [] Worms { get; private set; }


        public void ReadMembers (BinaryReader reader) {
            Type  = (TeamType) reader.ReadByte ();
            Id    = reader.ReadInt32 ();
            Color = reader.ReadByte ();

            byte n = reader.ReadByte ();
            Worms = new string [n];
            for (int i = 0; i < n; i++) {
                Worms [i] = reader.ReadString ();
            }
        }


        // public Team ToTeam () {
        //     if (Type == TeamType.AI) {
        //         // в зависимости от айдишника должны создаваться разные боты
        //         throw new NotImplementedException ();
        //     }
        //     return Id == _.PlayerInfo.Id ? (Team) new LocalTeam (Color, Worms) : new RemoteTeam (Color, Worms);
        // }

    }

}
