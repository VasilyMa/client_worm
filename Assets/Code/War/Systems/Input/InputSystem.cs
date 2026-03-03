using System;
using Core;
using DataTransfer.Data;
using UI.Panels;
using UnityEngine;
using War.Weapons;
using static UnityEngine.Input;


namespace War.Systems.Input {

    public class InputSystem {

        // ответственность класса:
        // посредничество между сырым вводом и игровой логикой

        // учитываем чат и панель арсенала
        // чат должен блокировать WASDQ
        // арсенал должен блокировать ЛКМ и чат


        private enum State { Normal, ArsenalOpen, ChatOpen }


        private          State        _state = State.Normal;
        private readonly ArsenalPanel _arsenal;
        private readonly ChatPanel    _chat;
        private          bool         _mb;
        private          int          _selectedWeapon;


        public InputSystem (ArsenalPanel arsenal, ChatPanel chat) {
            _arsenal = arsenal;
            _chat    = chat;
        }


        private int SelectedWeapon {
            get { return _selectedWeapon; }
            set {
                _selectedWeapon = value;
                _arsenal.Highlight (_selectedWeapon);
            }
        }


        public void Update () {
            switch (_state) {
                case State.Normal:
                    if      (GetKeyDown (KeyCode.Q))      
                        OpenArsenal ();
                    else if (GetKeyDown (KeyCode.Return)) 
                        OpenChat ();
                    else if (GetMouseButtonDown (0))      
                        _mb = true;
                    break;
                case State.ArsenalOpen:
                    if (GetKeyDown (KeyCode.Q)) {
                        CloseArsenal ();
                    }
                    break;
                case State.ChatOpen:
                    if (GetKeyDown (KeyCode.Return) && !_chat.TrySend ()) {
                        CloseChat ();
                    }
                    if (GetMouseButtonDown (0)) {
                        _mb = true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException ();
            }
            if (GetMouseButtonUp (0)) _mb = false;
        }

        public TurnData GetRemoteTurnData()
        {
            return new TurnData();
        }


        public TurnData GetLocalTurnData ()
        {
            var td = new TurnData ();
            if (_state != State.ChatOpen) 
            {
                if (GetKey(KeyCode.W))
                { 
                    td.W = GetKey(KeyCode.W);
                }
                td.A = GetKey (KeyCode.A);
                td.S = GetKey (KeyCode.S);
                td.D = GetKey (KeyCode.D);
                td.NumKey = (byte) (
                    GetKey (KeyCode.Alpha5) ? 5 :
                    GetKey (KeyCode.Alpha4) ? 4 :
                    GetKey (KeyCode.Alpha3) ? 3 :
                    GetKey (KeyCode.Alpha2) ? 2 :
                    GetKey (KeyCode.Alpha1) ? 1 : 0
                );
            }
            td.MB = _mb;
            td.XY = _.War.Camera.MouseXY;
            
            if (_.War.WeaponSystem.WeaponsLocked) return td;

            td.Weapon = (byte) SelectedWeapon;
            SelectedWeapon = 0;
            if (_state == State.ArsenalOpen && td.Weapon != 0) {
                CloseArsenal ();
            } 
            return td;
        }


        private void OpenArsenal () {
            if (_state != State.Normal) throw new Exception ($"Попытка открыть арсенал в состоянии {_state}");
            _arsenal.Show ();
            _state = State.ArsenalOpen;
        }


        private void CloseArsenal () {
            if (_state != State.ArsenalOpen) throw new Exception ($"Попытка закрыть арсенал в состоянии {_state}");
            _arsenal.Hide ();
            _state = State.Normal;
        }


        private void OpenChat () {
            if (_state != State.Normal) throw new Exception ($"Попытка открыть чат в состоянии {_state}");
            _chat.Show ();
            _state = State.ChatOpen;
        }


        private void CloseChat () {
            if (_state != State.ChatOpen) throw new Exception ($"Попытка закрыть чат в состоянии {_state}");
            _chat.Hide ();
            _state = State.Normal;
        }


        public void SelectWeapon (WeaponDescriptor descriptor) {
            SelectedWeapon = SelectedWeapon == descriptor.Id ? 0 : descriptor.Id;
        }

    }

}
