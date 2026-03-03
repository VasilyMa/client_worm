using Core;
using UnityEngine;
using War.Entities.Projectiles;


namespace War.Weapons {

    public class PoisonStrikeWpn : GenericAirstrike {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "отравляющий удар",
            desc => new PoisonStrikeWpn (desc),
            () => _.War.Assets.PoisonStrikeIcon
        );


        protected PoisonStrikeWpn (WeaponDescriptor desc) : base (desc) {}


        protected override void OnAttack () {
            Debug.Log (Target);
            
            _.War.LaunchAirstrike (() => new PoisonBomb (), Target, 7, LeftToRight, Crosshair);
            // Crosshair.References--;
        }

    }

}
