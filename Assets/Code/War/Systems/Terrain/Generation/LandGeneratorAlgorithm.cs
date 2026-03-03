namespace War.Systems.Terrain.Generation {

    public partial class LandGenerator {

        public abstract class Algorithm {

            public abstract void OnAdd (LandGenerator gen);
            public abstract bool Apply (LandGenerator gen); // true if algorithm finished executing

        }

    }

}