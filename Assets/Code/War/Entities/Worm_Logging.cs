using Core;
using War.Systems.Teams;

namespace War.Entities
{
    public partial class Worm
    {
        public string Name => _name;

        private void LogAction(string action, string details = "")
        {
            int teamIndex = Team.TeamId;
            Logger.LogWormAction(teamIndex, _name, 0, action, details);
        }

        private void LogStateChange(string oldState, string newState, string details = "")
        {
            LogAction($"Смена состояния: {oldState} → {newState}", details);
        }

        public void LogDamage(int damage, string cause)
        {
            int teamIndex = Team.TeamId;
            int oldHp = HP + damage;
            Logger.LogHealthChange(teamIndex, _name, 0, oldHp, HP, cause);
        }

        public void LogWeaponEquip(string weaponName)
        {
            int teamIndex = Team.TeamId;
            Logger.LogWeaponEvent(teamIndex, _name, 0, weaponName, "Экипировано", $"HP: {HP}");
        }

        public void LogWeaponUnequip(string weaponName)
        {
            int teamIndex = Team.TeamId;
            Logger.LogWeaponEvent(teamIndex, _name, 0, weaponName, "Демонтировано", $"HP: {HP}");
        }
    }
}