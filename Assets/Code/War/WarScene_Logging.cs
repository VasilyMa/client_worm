using Core;
using System.Collections;
using UnityEngine;
using War.Entities;
using War.Systems.Teams;

namespace War
{
    public partial class WarScene : MonoBehaviour
    {
        private int _turnNumber = 0;

        private void InitializeLogging()
        {
            Core.Logger.Initialize();
            Core.Logger.LogBattleEvent("WarScene инициализирована");
        }

        public void LogTurnStart(Team currentTeam, Worm activeWorm, int timeLimit)
        {
            _turnNumber++;
            int teamId = currentTeam == null ? -1 : currentTeam.TeamId;
            Core.Logger.LogTurnInfo(_turnNumber, teamId, activeWorm?.Name ?? "Unknown", timeLimit);
        }

        public void LogWormDeath(Worm worm, Team team, string cause)
        {
            int teamId = team?.TeamId ?? -1;
            Core.Logger.LogWormAction(teamId, worm.Name, 0, "Смерть", $"Причина: {cause}");
        }

        public void LogWormDamaged(Worm worm, Team team, int oldHp, int newHp, string cause)
        {
            int teamId = team?.TeamId ?? -1;
            Core.Logger.LogHealthChange(teamId, worm.Name, 0, oldHp, newHp, cause);
        }

        public void LogGameEnd(Team winnerTeam, string reason)
        {
            int winnerIndex = winnerTeam?.TeamId ?? -1;
            Core.Logger.LogGameEnded(winnerIndex, reason);
        }

        private void OnDestroy()
        {
            Core.Logger.Shutdown();
        }
    }
}