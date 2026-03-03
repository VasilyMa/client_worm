using System.IO;


namespace DataTransfer.Server {

    public abstract class ServerCommand : IDeserializable {

        public abstract void Execute ();

        public abstract void ReadMembers (BinaryReader reader);

    }

}