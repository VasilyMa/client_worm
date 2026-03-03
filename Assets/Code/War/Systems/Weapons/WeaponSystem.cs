using System;
using Core;
using DataTransfer.Data;
using War.Weapons;


namespace War.Systems.Weapons {

    #warning я отключил систему оружия на время
    public class WeaponSystem {

        public bool   WeaponsLocked;
        public Weapon Weapon { get; private set; }


        public void Update (TurnData td) 
        {
            if (td == null) return; 
            if (td.Weapon == 0) return; // ничего не выбрано
            
            if (WeaponsLocked) {
                throw new InvalidOperationException ("Пришел ненулевой индекс оружия при заблокированном оружии.");
            }
            // Weapon?.Worm.UnequipWeaponSlowly (); - лишнее, все равно червяк убирает его мгновенно
            Weapon = WeaponDescriptors.Get (td.Weapon).MakeWeapon ();
            _.War.ActiveWorm.EquipWeapon (Weapon, td);
            
            // теперь оружие как раньше компонент висящий на червяке, мы его не обновляем
        }


        public void Deselect () {
            return;
            
            Weapon?.Worm.UnequipWeaponSlowly ();
            Weapon = null;
        }

    }

}
