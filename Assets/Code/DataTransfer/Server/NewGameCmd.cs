using System.IO;
using Core;
using DataTransfer.Data;


namespace DataTransfer.Server {

    public class NewGameCmd : ServerCommand {

        public GameInitData Data { get; private set; }


        public override void ReadMembers (BinaryReader reader) {
            Data = new GameInitData ();
            Data.ReadMembers (reader);
        }


        public override void Execute () {
            _.SceneSwitcher.Load (Scenes.War, Data);
        }

    }

}