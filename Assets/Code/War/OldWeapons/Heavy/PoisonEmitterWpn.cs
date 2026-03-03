using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Heavy {

    /**
     * Штука, которая выпускает яд и остается после завершения хода.
     */
    public class PoisonEmitterWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "ядоиспускатель",
            desc => new PoisonEmitterWpn (desc),
            () => _.War.Assets.PoisonEmitterIcon
        );


        public PoisonEmitterWpn (WeaponDescriptor desc) : base (desc) {}

        
        public override void OnEquip () {
            throw new System.NotImplementedException ();
        }


        public override void OnDraw () {
            throw new System.NotImplementedException ();
        }


        public override void Update (TurnData td) {
            throw new System.NotImplementedException ();
        }


        public override void OnInterrupt () {
            throw new System.NotImplementedException ();
        }


        public override void OnUnequip () {
            throw new System.NotImplementedException ();
        }

    }

}
