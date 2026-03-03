using Core;
using UnityEngine;
using War.Entities.Projectiles;


namespace War.Weapons {

    public class NapalmStrikeWpn : GenericAirstrike {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "напалмовый удар",
            desc => new NapalmStrikeWpn (desc),
            () => _.War.Assets.NapalmStrikeIcon
        );


        protected NapalmStrikeWpn (WeaponDescriptor desc) : base (desc) {}


        protected override void OnAttack () {
            Debug.Log (Target);
            
            _.War.LaunchAirstrike (() => new NapalmBomb (), Target, 7, LeftToRight, Crosshair);
            // Crosshair.References--;
        }

    }

}
