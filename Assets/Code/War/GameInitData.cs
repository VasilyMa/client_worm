using War.Systems.Teams;


namespace War {

    public class GameInitData {

        public bool    Local   { get; }
        public int     Seed    { get; }
        public Team [] Teams   { get; }
        public int     Mines   { get; }
        public int     Barrels { get; }


        public GameInitData (bool local, int seed, Team [] teams, int mines, int barrels) {
            Local   = local;
            Seed    = seed;
            Teams   = teams;
            Mines   = mines;
            Barrels = barrels;
        }

    }

}
