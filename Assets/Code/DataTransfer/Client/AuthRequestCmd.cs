using System.IO;


namespace DataTransfer.Client {

    public class AuthRequestCmd : ClientCommand {

        public int Id { get; private set; }


        public AuthRequestCmd (int id) {
            Id = id;
        }


        public override void WriteMembers (BinaryWriter writer) {
            writer.Write (Id);
        }

    }

}
