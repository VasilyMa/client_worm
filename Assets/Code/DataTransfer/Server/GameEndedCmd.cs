using System;
using System.IO;


namespace DataTransfer.Server {

    public class GameEndedCmd : ServerCommand {

        public const int Victory = 0;
        public const int Draw    = 1;
        public const int Ruined  = 2;

        public int Result   { get; private set; }
        public int PlayerId { get; private set; }

        public static event Action <GameEndedCmd> OnReceive;


        public override void ReadMembers (BinaryReader reader) {
            Result = reader.ReadByte ();
            if (Result == Victory) {
                PlayerId = reader.ReadInt32 ();
            }
        }


        public override void Execute () {
            OnReceive?.Invoke (this);
        }

    }

}