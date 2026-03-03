using System.IO;


namespace DataTransfer {

    public interface IDeserializable {

        void ReadMembers (BinaryReader reader);

    }

}