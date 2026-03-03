using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Система логирования для записи детальной информации о ходе боя в файл
    /// </summary>
    public static class Logger
    {
        private static StreamWriter _logWriter;
        private static string _logFilePath;
        private static readonly object _lockObject = new object();
        private static bool _isInitialized = false;
        private static int _gameTime = 0;

        /// <summary>
        /// Обновляет текущее игровое время (вызывается из WarScene.DoUpdate)
        /// </summary>
        public static void UpdateGameTime(int gameTime)
        {
            _gameTime = gameTime;
        }

        /// <summary>
        /// Инициализирует логгер и создает файл лога
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialized) return;

            try
            {
                string logDirectory = Path.Combine(Application.persistentDataPath, "Logs");
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
                _logFilePath = Path.Combine(logDirectory, $"battle_{timestamp}.log");

                _logWriter = new StreamWriter(_logFilePath, false, Encoding.UTF8)
                {
                    AutoFlush = true
                };

                _isInitialized = true;
                WriteInternal($"[{GetTimestamp()}] [Δt: {Time.deltaTime:F4}] === ЛОГИРОВАНИЕ НАЧАТО ===");
                Debug.Log($"[Logger] Файл логов создан: {_logFilePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Logger] Ошибка инициализации логгера: {ex.Message}");
                _isInitialized = false;
            }
        }

        /// <summary>
        /// Логирует основное событие боя
        /// </summary>
        public static void LogBattleEvent(string message)
        {
            string fullMessage = $"[BATTLE] {message} | Время: {_gameTime}";
            WriteLog(fullMessage);
        }

        /// <summary>
        /// Логирует событие инициализации боя
        /// </summary>
        public static void LogGameInitialization(int seed, int playerCount, int wormsPerTeam)
        {
            string message = $"[INIT] Инициализация боя | Seed: {seed}, Игроков: {playerCount}, Червяков на команду: {wormsPerTeam}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует завершение инициализации и начало боя
        /// </summary>
        public static void LogGameStarted()
        {
            string message = $"[GAME_START] Боевой цикл начат | Игровое время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует начало тура
        /// </summary>
        public static void LogTurnStart(int turnNumber, int teamId, string wormName, int timeLimit)
        {
            string message = $"[TURN_START] Тур #{turnNumber} | Команда {teamId}, Червяк '{wormName}' | Лимит времени: {timeLimit}ms | Игровое время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует конец тура
        /// </summary>
        public static void LogTurnEnd(int turnNumber, int teamId, string reason)
        {
            string message = $"[TURN_END] Тур #{turnNumber} завершён | Команда {teamId} | Причина: {reason} | Игровое время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует передачу хода
        /// </summary>
        public static void LogTurnTransfer(int fromTeamId, int toTeamId, int nextTurnNumber)
        {
            string message = $"[TURN_TRANSFER] Ход передан | От команды {fromTeamId} → Команде {toTeamId} | Следующий тур: #{nextTurnNumber} | Игровое время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует запуск оружия
        /// </summary>
        public static void LogWeaponFire(int teamId, string wormName, string weaponName, float angle, float power)
        {
            string message = $"[WEAPON_FIRE] Оружие [{weaponName}] | Червяк '{wormName}' (Команда {teamId}) | Угол: {angle:F2}° | Сила: {power:F2} | Игровое время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует паузу боя
        /// </summary>
        public static void LogTurnPaused(int teamId, string reason)
        {
            string message = $"[TURN_PAUSED] Пауза включена | Команда {teamId} | Причина: {reason} | Игровое время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует возобновление боя
        /// </summary>
        public static void LogTurnResumed(int teamId)
        {
            string message = $"[TURN_RESUMED] Пауза отключена | Команда {teamId} | Игровое время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует сброс таймера
        /// </summary>
        public static void LogTurnTimerReset(int teamId, int newTimeLimit)
        {
            string message = $"[TIMER_RESET] Таймер сброшен | Команда {teamId} | Новый лимит: {newTimeLimit}ms | Игровое время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует действие червяка
        /// </summary>
        public static void LogWormAction(int teamId, string wormName, int wormIndex, string action, string details = "")
        {
            string message = $"[WORM] Червяк #{wormIndex} '{wormName}' (Команда {teamId}): {action}";
            if (!string.IsNullOrEmpty(details))
                message += $" | {details}";
            message += $" | Время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует изменение HP
        /// </summary>
        public static void LogHealthChange(int teamId, string wormName, int wormIndex, int oldHp, int newHp, string cause)
        {
            int damage = oldHp - newHp;
            string message = $"[HEALTH] Червяк #{wormIndex} '{wormName}' (Команда {teamId}): HP {oldHp} → {newHp} (урон: {damage}) | Причина: {cause} | Время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует события сетевой синхронизации
        /// </summary>
        public static void LogNetworkEvent(string playerId, string eventType, string details = "")
        {
            string message = $"[NETWORK] Игрок {playerId}: {eventType}";
            if (!string.IsNullOrEmpty(details))
                message += $" | {details}";
            message += $" | Время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует события команды
        /// </summary>
        public static void LogTeamEvent(int teamId, string teamType, string eventType, string details = "")
        {
            string message = $"[TEAM] Команда {teamId} ({teamType}): {eventType}";
            if (!string.IsNullOrEmpty(details))
                message += $" | {details}";
            message += $" | Время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует события оружия
        /// </summary>
        public static void LogWeaponEvent(int teamId, string wormName, int wormIndex, string weaponName, string action, string details = "")
        {
            string message = $"[WEAPON] [{weaponName}] Червяк #{wormIndex} '{wormName}' (Команда {teamId}): {action}";
            if (!string.IsNullOrEmpty(details))
                message += $" | {details}";
            message += $" | Время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует события ввода
        /// </summary>
        public static void LogInputEvent(int playerId, string wormName, int wormIndex, bool w, bool a, bool s, bool d, bool mb, float mouseX, float mouseY, byte weapon)
        {
            string keys = $"Keys[W:{w} A:{a} S:{s} D:{d} MB:{mb}]";
            string mouse = $"Mouse[X:{mouseX:F2} Y:{mouseY:F2}]";
            string message = $"[INPUT] Игрок {playerId} → Червяк #{wormIndex} '{wormName}': {keys} {mouse} Оружие#{weapon} | Время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует информацию о туре
        /// </summary>
        public static void LogTurnInfo(int turnNumber, int currentTeamId, string currentWormName, int timeRemaining)
        {
            string message = $"[TURN_INFO] Тур #{turnNumber}: Команда {currentTeamId}, Червяк '{currentWormName}', Осталось времени: {timeRemaining}ms | Игровое время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует события столкновения
        /// </summary>
        public static void LogCollisionEvent(string entity1, string entity2, string details = "")
        {
            string message = $"[COLLISION] {entity1} ↔ {entity2}";
            if (!string.IsNullOrEmpty(details))
                message += $" | {details}";
            message += $" | Время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует события взрыва
        /// </summary>
        public static void LogExplosion(string weaponName, float posX, float posY, int damage, string affectedEntities = "")
        {
            string message = $"[EXPLOSION] [{weaponName}] Позиция: ({posX:F2}, {posY:F2}) | Урон: {damage}";
            if (!string.IsNullOrEmpty(affectedEntities))
                message += $" | Затронуто: {affectedEntities}";
            message += $" | Время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Логирует ошибки
        /// </summary>
        public static void LogError(string category, string message)
        {
            string fullMessage = $"[ERROR] {category}: {message} | Время: {_gameTime}";
            WriteLog(fullMessage);
            Debug.LogError($"[Logger] {fullMessage}");
        }

        /// <summary>
        /// Логирует состояние всех червяков на конец тура
        /// </summary>
        public static void LogWormsState(List<(string name, int teamId, float x, float y, int hp)> wormsData)
        {
            if (wormsData == null || wormsData.Count == 0) return;

            StringBuilder sb = new StringBuilder();
            sb.Append($"[WORMS_STATE] Состояние червяков | Время: {_gameTime} | Позиции: ");

            double sumX = 0, sumY = 0;

            for (int i = 0; i < wormsData.Count; i++)
            {
                var worm = wormsData[i];
                sb.Append($"{worm.name}[T{worm.teamId}]({worm.x:F0},{worm.y:F0},HP{worm.hp})");
                
                sumX += worm.x;
                sumY += worm.y;

                if (i < wormsData.Count - 1)
                    sb.Append(", ");
            }

            sb.Append($" | СУММА КООРДИНАТ: ({sumX:F0}, {sumY:F0})");

            WriteLog(sb.ToString());
        }

        /// <summary>
        /// Логирует завершение игры
        /// </summary>
        public static void LogGameEnded(int winnerTeamId, string reason)
        {
            string message = $"[GAME_END] Игра завершена | Победитель: Команда {winnerTeamId} | Причина: {reason} | Игровое время: {_gameTime}";
            WriteLog(message);
        }

        /// <summary>
        /// Закрывает логгер и сохраняет файл
        /// </summary>
        public static void Shutdown()
        {
            if (!_isInitialized) return;

            lock (_lockObject)
            {
                try
                {
                    WriteInternal($"[{GetTimestamp()}] [Δt: {Time.deltaTime:F4}] === ЛОГИРОВАНИЕ ЗАВЕРШЕНО ===");
                    _logWriter?.Close();
                    _logWriter?.Dispose();
                    _isInitialized = false;
                    Debug.Log($"[Logger] Логгер закрыт. Файл: {_logFilePath}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[Logger] Ошибка при закрытии логгера: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Открывает папку логов в файловом менеджере
        /// </summary>
        public static void OpenLogsFolder()
        {
            string logsDirectory = Path.Combine(Application.persistentDataPath, "Logs");

            if (!Directory.Exists(logsDirectory))
            {
                Debug.LogWarning($"[Logger] Папка логов не найдена: {logsDirectory}");
                return;
            }

#if UNITY_EDITOR_WIN
            System.Diagnostics.Process.Start("explorer.exe", logsDirectory);
#elif UNITY_EDITOR_OSX
            System.Diagnostics.Process.Start("open", logsDirectory);
#elif UNITY_EDITOR_LINUX
            System.Diagnostics.Process.Start("xdg-open", logsDirectory);
#endif

            Debug.Log($"[Logger] Папка логов открыта: {logsDirectory}");
        }

        /// <summary>
        /// Открывает текущий лог-файл
        /// </summary>
        public static void OpenCurrentLogFile()
        {
            if (string.IsNullOrEmpty(_logFilePath) || !File.Exists(_logFilePath))
            {
                Debug.LogWarning("[Logger] Лог-файл не найден");
                return;
            }

#if UNITY_EDITOR_WIN
            System.Diagnostics.Process.Start(_logFilePath);
#elif UNITY_EDITOR_OSX
            System.Diagnostics.Process.Start("open", _logFilePath);
#elif UNITY_EDITOR_LINUX
            System.Diagnostics.Process.Start("xdg-open", _logFilePath);
#endif

            Debug.Log($"[Logger] Файл логов открыт: {_logFilePath}");
        }

        private static void WriteLog(string message)
        {
            if (!_isInitialized) Initialize();

            string fullMessage = $"[{GetTimestamp()}] [Δt: {Time.deltaTime:F4}] {message}";
            WriteInternal(fullMessage);
            // Опционально вывести в консоль для важных событий
            if (message.Contains("[TURN_") || message.Contains("[WEAPON_FIRE]") || message.Contains("[GAME_"))
            {
                Debug.Log($"[Logger] {fullMessage}");
            }
        }

        private static void WriteInternal(string message)
        {
            if (!_isInitialized || _logWriter == null) return;

            lock (_lockObject)
            {
                try
                {
                    _logWriter.WriteLine(message);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[Logger] Ошибка записи в файл лога: {ex.Message}");
                }
            }
        }

        private static string GetTimestamp()
        {
            return DateTime.Now.ToString("HH:mm:ss.fff");
        }

        public static string GetLogFilePath()
        {
            return _logFilePath;
        }
    }
}