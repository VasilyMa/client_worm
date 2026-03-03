using Math;
using War.Entities;


namespace War.Systems.Blasts {

    public interface IBlastable : IMobile {

        bool Alive { get; }
        void TakeBlast (XY impulse);

    }

}
