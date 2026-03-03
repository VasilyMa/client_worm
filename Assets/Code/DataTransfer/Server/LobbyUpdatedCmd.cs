using System;
using System.IO;


namespace DataTransfer.Server {

    public class LobbyUpdatedCmd : ServerCommand {

        public int HubId   { get; private set; }
        public int Players { get; private set; }

        public static event Action <LobbyUpdatedCmd> OnReceive;


        public override void ReadMembers (BinaryReader reader) {
            HubId   = reader.ReadByte ();
            Players = reader.ReadByte ();
        }


        public override void Execute () {
            OnReceive?.Invoke (this);
        }

    }

}