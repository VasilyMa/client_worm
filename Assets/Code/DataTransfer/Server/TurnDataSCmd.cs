using System;
using System.IO;
using DataTransfer.Data;


namespace DataTransfer.Server {

    public class TurnDataSCmd : ServerCommand {

        public static event Action <TurnDataSCmd> OnReceive;

        public TurnData Data { get; private set; }


        public override void ReadMembers (BinaryReader reader) {
            Data = new TurnData ();
            Data.ReadMembers (reader);
        }


        public override void Execute () {
            OnReceive?.Invoke (this);
        }

    }

}