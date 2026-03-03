using System;
using System.IO;


namespace DataTransfer.Data {

    public class GameInitData : IDeserializable {

        public int          Seed  { get; private set; }
        public TeamSData [] Teams { get; private set; }


        public void ReadMembers (BinaryReader reader) {
            Seed = reader.ReadInt32 ();

            byte n = reader.ReadByte ();
            Teams = new TeamSData [n];
            for (int i = 0; i < n; i++) {
                Teams [i] = new TeamSData ();
                Teams [i].ReadMembers (reader);
            }
        }


        // public OldWar.GameInitData ToWarFormat () =>
        // new OldWar.GameInitData (false, Seed, Array.ConvertAll (Teams, t => t.ToTeam ()), 15, 10);

    }

}
