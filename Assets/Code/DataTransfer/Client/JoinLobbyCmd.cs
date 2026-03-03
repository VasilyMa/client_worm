using System.IO;


namespace DataTransfer.Client {

    public class JoinLobbyCmd : ClientCommand {

        private readonly byte _lobbyId;


        public JoinLobbyCmd (byte lobbyId) {
            _lobbyId = lobbyId;
        }


        public JoinLobbyCmd (int lobbyId) : this ((byte) lobbyId) {}


        public override void WriteMembers (BinaryWriter writer) {
            writer.Write (_lobbyId);
        }

    }

}
