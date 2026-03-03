using Core;
using DataTransfer.Data;


namespace War.OldWeapons {

    public abstract class Weapon {

        public readonly WeaponDescriptor Descriptor;
        

        protected Weapon (WeaponDescriptor desc) {
            Descriptor = desc;
        }

        // вызывается при выборе оружия
        public abstract void OnEquip ();
        
        // вызывается при доставании оружия, после выбора или после вставания
        public abstract void OnDraw ();

        // вызывается пока оружие активно
        public abstract void Update (TurnData td);

        // вызывается когда тебя сбивают с ног без урона
        public abstract void OnInterrupt ();
        
        // вызывается извне когда оружие полностью использовано или в конце хода
        public abstract void OnUnequip ();


        protected void Declare () {
            _.War.WeaponInfo.Declare (Descriptor.Name);
        }
        
        
        /* Какие переменные состояния должны быть в классе
         *
         * функция обновления, функция прерывания - их должно быть можно подменять изнутри
         *
         * (обычное состояние)
         *     время когда достали оружие
         *     время когда отблочится атака
         *
         * а вот тут может быть например указание цели для ракеты короче че угодно
         *
         * (регулировка силы)
         *     время когда мы начали регулировку
         *
         * (атака)
         *     время когда мы начали атаку - по нему можно однозначно определить все выстрелы
         *
         * также нужно хранить количество оставшихся атак и патронов
         */

    }
}
