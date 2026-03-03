using DataTransfer.Data;
using War.Entities;
using War.Systems.Updating;


namespace War.Components.Controllers {

    public abstract class Controller <T> : Component <T> /*where T : Entity*/ {

        public abstract void Update (TurnData td);

    }

}
