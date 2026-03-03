using System;
using System.IO;


namespace DataTransfer.Server {

    public class LeftLobbyCmd : ServerCommand {

        public static event Action <LeftLobbyCmd> OnReceive;


        public override void ReadMembers (BinaryReader reader) {}


        public override void Execute () {
            OnReceive?.Invoke (this);
        }

    }

}