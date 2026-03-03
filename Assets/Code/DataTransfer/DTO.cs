using System;
using System.Collections.Generic;
using System.IO;
using DataTransfer.Client;
using DataTransfer.Server;


namespace DataTransfer {

    public static class DTO {

        private static Dictionary <short, Type> _types = new Dictionary <short, Type> ();
        private static Dictionary <Type, short> _codes = new Dictionary <Type, short> ();


        public static void Init () {
            Register (0,  typeof (AuthRequestCmd));
            Register (10, typeof (JoinLobbyCmd));
            Register (20, typeof (LeaveLobbyCmd));
            Register (30, typeof (TurnDataCCmd));
            Register (40, typeof (TurnEndedCmd));
            Register (50, typeof (LeaveGameCmd));

            Register (1,  typeof (AuthSuccessCmd));
            Register (11, typeof (LobbyUpdatedCmd));
            Register (21, typeof (LeftLobbyCmd));
            Register (31, typeof (NewGameCmd));
            Register (41, typeof (TurnDataSCmd));
            Register (51, typeof (NewTurnCmd));
            Register (61, typeof (LeftGameCmd));
            Register (71, typeof (GameEndedCmd));
//            Register (81, typeof (DbErrorCmd));
        }


        private static void Register (short code, Type type) {
            _types.Add (code, type);
            _codes.Add (type, code);
        }


        public static IDeserializable Read (BinaryReader reader) {
            short code   = reader.ReadInt16 ();
            var   result = (IDeserializable) Activator.CreateInstance (_types [code]);
            result.ReadMembers (reader);
            return result;
        }


        public static short GetCode (Type type) => _codes [type];

    }

}