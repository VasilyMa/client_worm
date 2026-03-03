using Core;
using DataTransfer.Data;
using Fusion;
using MemoryPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using War;

public class PhotonRunHandler : NetworkBehaviour
{
    public static PhotonRunHandler Instance;
    private NetworkRunner runner;
    private WarScene _scene;

    private Dictionary<string, PlayerSessionData> _players;
    private bool _isInitialized = false;

    public void Init(WarScene scene)
    {
        _scene = scene;
        Core.Logger.LogBattleEvent($"PhotonRunHandler инициализирован для сцены {scene.name}");
    }

    public override void Spawned()
    {
        base.Spawned();

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            // Предотвращаем множественные экземпляры
            Core.Logger.LogError("PhotonRunHandler", "Попытка создать второй экземпляр класса");
            return;
        }

        runner = PhotonInitializer.Instance?.Runner;
        
        if (runner == null)
        {
            Core.Logger.LogError("PhotonRunHandler", "NetworkRunner не найден! PhotonInitializer не инициализирован.");
            Debug.LogError("[PhotonRunHandler] NetworkRunner is null!");
            return;
        }

        Debug.Log("[PhotonRunHandler] Init called. Runner: " + runner.name);
        Core.Logger.LogNetworkEvent("System", "NetworkRunner инициализирован", $"Runner: {runner.name}");

        _players = new Dictionary<string, PlayerSessionData>();
        _isInitialized = true;

        StartCoroutine(WaitConnection());
    }

    IEnumerator WaitConnection()
    {
        // Ожидаем инициализации WarScene
        yield return new WaitUntil(() => WarScene.Instance != null);
         
        if (!_isInitialized)
        {
            Core.Logger.LogError("PhotonRunHandler", "PhotonRunHandler не был инициализирован перед WaitConnection");
            yield break;
        }

        var player = new PlayerSessionData()
        {
            ID = PhotonInitializer.Instance.PlayerID
        };

        Core.Logger.LogNetworkEvent($"Player_{PhotonInitializer.Instance.PlayerID}", "Отправка сообщения готовности", $"PlayerID: {player.ID}");
        
        try
        {
            SendMessageReadyRPC(MemoryPackSerializer.Serialize(player));
        }
        catch (System.Exception ex)
        {
            Core.Logger.LogError("PhotonRunHandler", $"Ошибка при отправке RPC: {ex.Message}");
            Debug.LogException(ex);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void SendMessageReadyRPC(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            Core.Logger.LogError("PhotonRunHandler", "Получены пустые данные в SendMessageReadyRPC");
            return;
        }

        PlayerSessionData player;
        try
        {
            player = MemoryPackSerializer.Deserialize<PlayerSessionData>(data);
        }
        catch (System.Exception ex)
        {
            Core.Logger.LogError("PhotonRunHandler", $"Ошибка десериализации PlayerSessionData: {ex.Message}");
            return;
        }

        Core.Logger.LogNetworkEvent($"Player_{player.ID}", "Получено сообщение готовности", $"PlayerID: {player.ID}");

        if (runner == null || !runner.IsServer)
        {
            // Только сервер обрабатывает присоединения
            return;
        }

        // Проверка на дублирование игрока
        foreach (var existingPlayer in _players.Values)
        {
            if (existingPlayer.ID == player.ID)
            {
                Core.Logger.LogError("PhotonRunHandler", $"Попытка присоединения дублирующегося игрока с ID: {player.ID}");
                return;
            }
        }

        var playerId = $"player_{_players.Count + 1}";

        _players.Add(playerId, player);
        Core.Logger.LogTeamEvent(_players.Count - 1, "Server", "Игрок присоединился", 
            $"PlayerId: {playerId}, NetworkID: {player.ID}, Всего игроков: {_players.Count}");

        if (_players.Count == 2)
        {
            Core.Logger.LogBattleEvent("Оба игрока присоединились, инициализация боя");
            
            var initData = new InitialSessionData()
            {
                Seed = System.DateTime.Now.GetHashCode(),
                Players = 2,
                Worms = 5,
            };
            
            try
            {
                SendMessageInitializeRPC(MemoryPackSerializer.Serialize(initData));
            }
            catch (System.Exception ex)
            {
                Core.Logger.LogError("PhotonRunHandler", $"Ошибка при отправке Initialize RPC: {ex.Message}");
                Debug.LogException(ex);
            }
        }
        else if (_players.Count > 2)
        {
            Core.Logger.LogError("PhotonRunHandler", $"Подключено более 2 игроков! Текущее количество: {_players.Count}");
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void SendMessageInitializeRPC(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            Core.Logger.LogError("PhotonRunHandler", "Получены пустые данные в SendMessageInitializeRPC");
            return;
        }

        InitialSessionData initData;
        try
        {
            initData = MemoryPackSerializer.Deserialize<InitialSessionData>(data);
        }
        catch (System.Exception ex)
        {
            Core.Logger.LogError("PhotonRunHandler", $"Ошибка десериализации InitialSessionData: {ex.Message}");
            return;
        }

        if (WarScene.Instance == null)
        {
            Core.Logger.LogError("PhotonRunHandler", "WarScene.Instance равен null в SendMessageInitializeRPC");
            return;
        }

        Core.Logger.LogGameInitialization(initData.Seed, initData.Players, initData.Worms);
        
        try
        {
            WarScene.Instance.StartCoroutine(WarScene.Instance.RuntimeHandle(initData));
        }
        catch (System.Exception ex)
        {
            Core.Logger.LogError("PhotonRunHandler", $"Ошибка при запуске RuntimeHandle: {ex.Message}");
            Debug.LogException(ex);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void SendMessageTurnRPC(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            Core.Logger.LogError("PhotonRunHandler", "Получены пустые данные в SendMessageTurnRPC");
            return;
        }

        InputData inputData;
        try
        {
            inputData = MemoryPackSerializer.Deserialize<InputData>(data);
        }
        catch (System.Exception ex)
        {
            Core.Logger.LogError("PhotonRunHandler", $"Ошибка десериализации InputData: {ex.Message}");
            return;
        }

        if (_scene == null)
        {
            _scene = WarScene.Instance;
        }

        if (_scene == null)
        {
            Core.Logger.LogError("PhotonRunHandler", "WarScene.Instance равен null в SendMessageTurnRPC");
            return;
        }

        // Получаем информацию о активном червяке безопасно
        string wormName = "Unknown";
        int wormIndex = 0;
        
        if (_scene.ActiveWorm != null)
        {
            wormName = _scene.ActiveWorm.Name;
        }

        Core.Logger.LogInputEvent(
            PhotonInitializer.Instance.PlayerID,
            wormName,
            wormIndex,
            inputData.W,
            inputData.A,
            inputData.S,
            inputData.D,
            inputData.MB,
            inputData.X,
            inputData.Y,
            (byte)inputData.Weapon
        );

        try
        {
            _scene.SetRemoteData(new TurnData()
            {
                XY = new Math.XY(inputData.X, inputData.Y),
                W = inputData.W,
                A = inputData.A,
                S = inputData.S,
                D = inputData.D,
                MB = inputData.MB, 
                Weapon = (byte)inputData.Weapon,
            });
        }
        catch (System.Exception ex)
        {
            Core.Logger.LogError("PhotonRunHandler", $"Ошибка при установке RemoteData: {ex.Message}");
            Debug.LogException(ex);
        }
    }

    private void OnDestroy()
    {
        // Очистка при уничтожении
        if (Instance == this)
        {
            Instance = null;
            Core.Logger.LogNetworkEvent("System", "PhotonRunHandler уничтожен");
        }
    }
}

[MemoryPackable]
public partial struct InputData
{
    public bool W, A, S, D, MB;
    public float X;
    public int Weapon;
    public float Y;
    
    public byte Flags
    {
        get
        {
            return (byte)(
                (W ? 0x01 : 0) |
                (A ? 0x02 : 0) |
                (S ? 0x04 : 0) |
                (D ? 0x08 : 0) |
                (MB ? 0x10 : 0)
            );
        }
        set
        {
            W = (value & 0x01) != 0;
            A = (value & 0x02) != 0;
            S = (value & 0x04) != 0;
            D = (value & 0x08) != 0;
            MB = (value & 0x10) != 0;
        }
    }
}

[MemoryPackable]
public partial struct InitialSessionData
{ 
    public int Seed;
    public int Players;
    public int Worms;
}

[MemoryPackable]
public partial struct PlayerSessionData
{
    public int ID;
}