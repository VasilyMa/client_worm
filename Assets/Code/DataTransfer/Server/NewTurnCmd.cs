using System;
using System.IO;


namespace DataTransfer.Server {

    public class NewTurnCmd : ServerCommand {

        public static event Action <NewTurnCmd> OnReceive;


        public int TeamId { get; private set; }


        public override void ReadMembers (BinaryReader reader) {
            TeamId = reader.ReadInt32 ();
        }


        public override void Execute () {
            OnReceive?.Invoke (this);
        }

    }

}
