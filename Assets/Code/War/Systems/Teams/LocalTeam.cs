using Core;
using DataTransfer.Data;


namespace War.Systems.Teams {

    public class LocalTeam : Team {

        public LocalTeam (int colorId, string [] wormsNames) : base (colorId, wormsNames) {}


        public override TurnData GetTurnData () => _.War.InputSystem.GetLocalTurnData ();

    }

}