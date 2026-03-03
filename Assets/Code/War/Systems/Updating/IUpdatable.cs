using DataTransfer.Data;


namespace War.Systems.Updating {

    public interface IUpdatable {

        bool Alive { get; }
        void Update (TurnData td);

    }

}