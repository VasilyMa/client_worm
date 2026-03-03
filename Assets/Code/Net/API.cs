// ReSharper disable InconsistentNaming

namespace Net {

    public class ClientAPI {

        public const byte
            Auth = 0,
            ToHub = 1,
            Quit = 2,
            TurnData = 3,
            EndTurn = 4;

    }


    public class ServerAPI {

        public const byte
            AccountData = 0,
            HubChanged = 1,
            StartGame = 2,
            LeftGame = 3,
            ShowWinner = 4,
            TurnData = 5,
            NoWinner = 6,
            NewTurn = 7;

    }

}
