using System.IO;
using DataTransfer.Data;


namespace DataTransfer.Client {

    public class TurnDataCCmd : ClientCommand {

        public TurnData Data { get; private set; }


        public TurnDataCCmd (TurnData data) {
            Data = data;
        }


        public override void WriteMembers (BinaryWriter writer) {
            Data.WriteMembers (writer);
        }

    }

}
