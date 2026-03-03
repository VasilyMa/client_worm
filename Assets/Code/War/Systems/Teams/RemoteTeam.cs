using DataTransfer.Data;

namespace War.Systems.Teams {

    public class RemoteTeam : Team
    {

        public RemoteTeam(int colorId, string[] wormsNames)
            : base(colorId, wormsNames) { }

        public override TurnData GetTurnData()
        {
            return Core._.War.InputSystem.GetRemoteTurnData();
        }
    }

}