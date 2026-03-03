using System.Collections;
using System.Collections.Generic;
using Core;
using DataTransfer.Data;
using UnityEngine;
using Utils;
using War.Entities;
using War.Systems.Worms;


/* СМЕНА СОСТОЯНИЙ В ТП4
     * 
     * 1 НАЧАЛО ИГРЫ
     *     делаем проверку на 0 хп, чисто просто так
     *     проверяем условия победы
     * 2 СИНХРОНИЗАЦИЯ
     *     синхронизируемся через сервер и дожидаемся его ответа кто ходит
     *     если не совпадет объявляем рассинхрон
     * 3 ПЕРЕД ХОДОМ
     *     сбрасываем ящики, тушим огни, обновляем щиты
     *     здесь мы делаем все то что ничего не взрывает и не наносит урон
     * 4 ГОТОВНОСТЬ
     *     обновляем мир, но при попытке игрока что-то сделать начинаем ход
     *     здесь нужно отсылать или принимать данные хода
     * 5 ХОД
     *     делаем ход, при получении урона или же по таймеру прекращаем
     * 6 УСТАКАНИВАНИЕ
     *     не переходим, пока не устаканились снаряды и объекты на поле
     *     при необходимости передаем данные по сети
     * 7 ПОСЛЕ ХОДА
     *     срабатывает яд, 1 раз
     * 8 ЯДЕРНЫЕ РАКЕТЫ
     *     если есть ядерные ракеты, кидаем их
     *     проверяем условие победы, причем если ракеты еще остались, условие не выполнится
     * 9 НОВЫЙ ХОД
     *     передаем ход следующей команде
     *
     * шаг 1 выполняется 1 раз за всю игру
     * шаги 2-7 выполняются только если команда может ходить
     *
     * ПРАВИЛА И ОГРАНИЧЕНИЯ СМЕНЫ СОСТОЯНИЙ
     * 
     * после синхронизации активная команда не меняется
     * яд должен сработать 1 раз в конце хода (кстати и подъем воды тоже)
     * ядерные ракеты падают по очереди, и яд не должен в этот момент срабатывать
     * перед ходом мы ничего не взрываем, после хода все взрываем
     * если осталась всего 1 команда, надо дождаться срабатывания эффектов и падения всех ядерных ракет
     * ракет надо дождаться даже если команд вообще не осталось
     */


namespace War {

    public partial class WarScene {

        private int _time;
        public int Time {
            get         { return _time;  }
            private set { _time = value; }
        }
        public int TweenAt { get; private set; }
        public int ReadyAt { get; private set; }


        private int _turnTime;
        public int TurnTime {
            get { return _turnTime; }
            set {
                _turnTime = value;
                Clock.text = Utils.Time.CeilToSeconds (value).ToString ();
            }
        }


        private bool _turnPaused;
        public bool TurnPaused {
            get { return _turnPaused; }
            set {
                _turnPaused = value;
                Clock.color = value ? Colors.UIInactive : Colors.UISelected;
            }
        }


        public int Turn { get; private set; }
        private IEnumerable UpdateRoutine()
        {
        initial:
            TurnPaused = true;
            TurnTime = 0;
            WeaponSystem.WeaponsLocked = true;
            Wait();
            foreach (var step in ReapRoutine()) yield return step;
            if (GameOver) yield break;
            beforeTurn:
            // здесь происходят действия, которые не нанесят урона, ничего не взорвут и не поменяют
            FireSystem.Reset();
            //Wind = Random.Int (49) * 0.25f - 6;
            foreach (var step in WaitRoutine()) yield return step;
            ready:
            Debug.Log("ready state");
            ActiveWorm = TeamsSystem.NextWorm(Turn);
            ReadyAt = Time + 300;
            TurnTime = 1800;

            // ЛОГИРУЕМ НАЧАЛО ТУРА
            Core.Logger.LogTurnStart(Turn, ActiveWorm?.Team.TeamId ?? -1, ActiveWorm?.Name ?? "Unknown", TurnTime);

            while (Time < ReadyAt)
            { // ReadyAt меняется при действиях игрока так что норм
              // Hint.text = $"Ход начнется через {Utils.Time.CeilToSeconds (ReadyAt - Time)} с";
                SyncUpdate();
                yield return null;
            }
            Hint.text = "";
        turn:
            Debug.Log("turn state");
            WeaponSystem.WeaponsLocked = false;
            TurnPaused = false;

            // ЛОГИРУЕМ ЗАПУСК БОЕВОГО ЦИКЛА
            Core.Logger.LogBattleEvent($"Боевой цикл запущен | Команда {ActiveWorm?.Team.TeamId}, Червяк '{ActiveWorm?.Name}'");

            while (TurnTime > 0)
            { // bug - есть момент когда игроку показывается 0 но без паузы
                SyncUpdate();
                yield return null;
            }
            WeaponSystem.WeaponsLocked = true;
            ActiveWorm = null;
            TurnPaused = true;
            WeaponSystem.Deselect();
            Wait(60);
        settling:
            // нужно будет также учесть что снаряды могут быть и управляемыми
            foreach (var step in WaitRoutine()) yield return step;
            afterTurn:
            WeaponInfo.Hide();
            if (WormsSystem.TryDealPoisonDamage()) Wait();
            FireSystem.OnTurnEnded();

            foreach (var step in ReapRoutine()) yield return step;
            nextTurn:
            int previousTeamId = Turn % _data.Teams.Length;
            int nextTeamId = (Turn + 1) % _data.Teams.Length;
            int nextTurnNumber = Turn + 1;

            // ЛОГИРУЕМ СОСТОЯНИЕ ВСЕХ ЧЕРВЯКОВ ПЕРЕД ПЕРЕДАЧЕЙ ХОДА
            LogAllWormsState();

            // ЛОГИРУЕМ ПЕРЕДАЧУ ХОДА
            Core.Logger.LogTurnTransfer(previousTeamId, nextTeamId, nextTurnNumber);

            _turnState.SwitchTurn();

            if (TeamsSystem.TeamAlive(++Turn)) goto beforeTurn;
        }

        public void EndTurn(int retreatTime = 0)
        {
            WeaponSystem.WeaponsLocked = true;

            // ЛОГИРУЕМ СОСТОЯНИЕ ПЕРЕД ЗАВЕРШЕНИЕМ ХОДА
            LogAllWormsState();

            // ЛОГИРУЕМ КОНЕЦ ХОДА
            Core.Logger.LogTurnEnd(Turn, ActiveWorm?.Team.TeamId ?? -1, "Время истекло или ход завершён");

            // ЛОГИРУЕМ СБРОС ТАЙМЕРА, если он сбрасывается
            if (retreatTime > 0)
            {
                Core.Logger.LogTurnTimerReset(ActiveWorm?.Team.TeamId ?? -1, retreatTime);
                Core.Logger.LogBattleEvent($"Отступление на {retreatTime}ms перед завершением хода");
            }

            TurnTime = retreatTime;
            TurnPaused = retreatTime <= 0;
            WeaponSystem.Deselect();
        }
        private bool GameOver => TeamsSystem.AllowsGameOver;


        public void Wait (int ticks = 30) {
            int t = Time + ticks;
            if (t > TweenAt) TweenAt = t;
        } 


        private IEnumerable WaitRoutine () {
            while (Time < TweenAt) {
                DoUpdate ();
                yield return null;
            }
        }


        private IEnumerable ReapRoutine () {
            while (true) {
                while (Time < TweenAt) {
                    DoUpdate ();
                    yield return null;
                }
                if (!WormsSystem.TryReap ()) break;
                Wait ();
            }
        } 

        private void CheckWormsOutOfBounds()
        {
            float mapHeight = Land.Height + 600;
            float mapWidth = Land.Width;
            
            var wormsToKill = new List<Worm>();
            
            foreach (var worm in WormsSystem.Worms)
            {
                if (worm.Despawned) continue;
                
                // Проверка: вышел ли червяк за границы с запасом
                if (worm.Position.Y > mapHeight || 
                    worm.Position.X < -200 || 
                    worm.Position.X > mapWidth + 200)
                {
                    Core.Logger.LogWormAction(worm.Team.TeamId, worm.Name, 0, "Выбыл из игры", "Вышел за границы карты");
                    wormsToKill.Add(worm);
                }
            }
            
            // Убиваем червяков за границами
            foreach (var worm in wormsToKill)
            {
                worm.TakeDamage(worm.HP);
            }
        }

    }

}