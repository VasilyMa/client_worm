using System.Collections.Generic;
using System.Linq;
using War.Entities;


namespace War.Systems.Teams {

    public class TeamsSystem {

        // храним все команды в т.ч. вылетевшие
        // так как команд немного то мы можем использовать полный перебор на них
        private List <Team> _teams;


        public TeamsSystem (IEnumerable<Team> teams) {
            _teams = teams.ToList ();
        }


        public Team this [int turn] => _teams [turn % _teams.Count];


        public bool AllowsGameOver {
            get {
                int teams = 0;
                return _teams.All (t => !t.Alive || ++teams < 2); // вернет true если нашлось 2 команды живые
            }
        }


        public bool TeamAlive (int id) {
            return this [id].Alive;
        }


        public Worm NextWorm (int teamId) {
            return this [teamId].NextWorm ();
        }

        public Team GetCurrentTeam(int playerID)
        {
            return _teams[playerID - 1];
        }
    }

}
