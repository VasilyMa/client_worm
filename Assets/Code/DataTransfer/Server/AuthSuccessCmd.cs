using System;
using System.IO;
using Core;
using DataTransfer.Data;


namespace DataTransfer.Server {

    public class AuthSuccessCmd : ServerCommand {

        public static event Action <AuthSuccessCmd> OnReceive;

        public AccountData AccountData { get; private set; }


        public override void ReadMembers (BinaryReader reader) {
            AccountData = new AccountData ();
            AccountData.ReadMembers (reader);
        }


        public override void Execute () {
            _.PlayerInfo = new PlayerInfo (AccountData.Id);
            OnReceive?.Invoke (this);
        }

    }

}