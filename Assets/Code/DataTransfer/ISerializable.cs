using System.IO;


namespace DataTransfer {

    public interface ISerializable {

//        void Write (BinaryWriter writer) {
//            writer.Write((short) _codes[GetType()]);
//            WriteMembers(writer);
//        }


        void WriteMembers (BinaryWriter writer);

    }

}