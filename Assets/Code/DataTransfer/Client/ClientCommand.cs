using System.IO;


namespace DataTransfer.Client {

    public abstract class ClientCommand : ISerializable {

        public abstract void WriteMembers (BinaryWriter writer);

    }

}