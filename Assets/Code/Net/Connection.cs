using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using DataTransfer;
using DataTransfer.Client;
using DataTransfer.Server;
using Net.Utils;
using UnityEngine;
using NativeWebSocket;

namespace Net {

    public class Connection : MonoBehaviour {

        private WebSocket _socket;
        private int _turnDataRead;
        private Coroutine _coroutine;
        private Queue<byte[]> _messageQueue = new Queue<byte[]>();
        private bool _isConnected = false;

        private void Awake() {
            _.Connection = this;
        }

        public void Work() { }

        private void Update() {
            if (_socket == null) return;

            #if !UNITY_WEBGL || UNITY_EDITOR
            _socket.DispatchMessageQueue();
            #endif

            // Обработка входящих сообщений (аналогично старому коду)
            lock (_messageQueue) {
                for (_turnDataRead = 0; _turnDataRead < 2 && _messageQueue.Count > 0; _turnDataRead++) {
                    var bytes = _messageQueue.Dequeue();
                    Parse(bytes);
                }
            }
        }

        private async void OnDisable() {
            await CloseSocket();
        }

        private void Parse(byte[] bytes) {
            Debug.Log(BitConverter.ToString(bytes));

            using (var stream = new MemoryStream(bytes))
            using (var reader = new BigEndianBinaryReader(stream)) {
                var cmd = (ServerCommand)DTO.Read(reader);
                cmd.Execute();
            }
        }

        public void Send(ClientCommand cmd) {
            DoSend(cmd);
        }

        public void Authorize(int id, string url) {
            if (_coroutine != null) {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(AuthCoroutine(url, id));
        }

        private IEnumerator AuthCoroutine(string url, int id) {
            // Закрываем существующее соединение
            if (_socket != null) {
                yield return StartCoroutine(CloseSocketCoroutine());
            }

            // Создаем WebSocket соединение
            _socket = new WebSocket(url);
            _isConnected = false;

            // Настраиваем обработчики событий
            _socket.OnMessage += (bytes) => {
                lock (_messageQueue) {
                    _messageQueue.Enqueue(bytes);
                }
            };

            _socket.OnOpen += () => {
                Debug.Log("WebSocket connected successfully");
                _isConnected = true;
                
                // Отправляем авторизацию после подключения
                DoSend(new AuthRequestCmd(id));
                StartCoroutine(SendPing());
            };

            _socket.OnError += (error) => {
                Debug.LogError($"WebSocket error: {error}");
                _isConnected = false;
            };

            _socket.OnClose += (code) => {
                Debug.Log($"WebSocket closed with code: {code}");
                _isConnected = false;
            };

            // Подключаемся
            Debug.Log("Connecting to WebSocket...");
            yield return StartCoroutine(ConnectSocketCoroutine());
        }

        private IEnumerator ConnectSocketCoroutine() {
            if (_socket == null) yield break;

            var connectTask = _socket.Connect();
            
            // Ждем завершения подключения с таймаутом
            float timeout = 10f;
            float elapsed = 0f;
            
            while (!connectTask.IsCompleted && elapsed < timeout) {
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            if (connectTask.IsFaulted) {
                Debug.LogError($"Connection failed: {connectTask.Exception}");
            }
            else if (elapsed >= timeout) {
                Debug.LogError("Connection timeout");
            }
        }

        private IEnumerator CloseSocketCoroutine() {
            if (_socket != null) {
                _isConnected = false;
                var closeTask = _socket.Close();
                
                float timeout = 5f;
                float elapsed = 0f;
                
                while (!closeTask.IsCompleted && elapsed < timeout) {
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                
                _socket = null;
            }
        }

        private async Task CloseSocket() {
            if (_socket != null) {
                _isConnected = false;
                await _socket.Close();
                _socket = null;
            }
        }

        private void DoSend(ClientCommand cmd) {
            if (_socket == null) {
                Debug.LogWarning("WebSocket is null");
                return;
            }

            if (!_isConnected || _socket.State != WebSocketState.Open) {
                Debug.LogWarning($"WebSocket is not connected. State: {_socket.State}, Connected: {_isConnected}");
                return;
            }

            try {
                using (var stream = new MemoryStream())
                using (var writer = new BigEndianBinaryWriter(stream)) {
                    cmd.Write(writer);
                    var data = stream.ToArray();
                    
                    // Проверяем что данные не пустые
                    if (data.Length > 0) {
                        _socket.Send(data);
                    }
                    else {
                        Debug.LogWarning("Attempted to send empty data");
                    }
                }
            }
            catch (Exception e) {
                Debug.LogError($"Error in DoSend: {e}");
            }
        }

        private IEnumerator SendPing() {
            while (_socket != null && _isConnected && _socket.State == WebSocketState.Open) {
                yield return new WaitForSecondsRealtime(20);
                
                if (_isConnected && _socket.State == WebSocketState.Open) {
                    try {
                        // Отправляем пустое сообщение как ping (как в оригинальном коде)
                        _socket.Send(new byte[0]);
                    }
                    catch (Exception e) {
                        Debug.LogError($"Error sending ping: {e}");
                        break;
                    }
                }
            }
        }
    }
}