using System.IO;


namespace DataTransfer {

    public static class SerializableExtension {

        public static void Write (this ISerializable dto, BinaryWriter writer) {
            writer.Write (DTO.GetCode (dto.GetType ()));
            dto.WriteMembers (writer);
        }

    }

}