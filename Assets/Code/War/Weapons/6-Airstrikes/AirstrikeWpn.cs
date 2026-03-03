using Core;
using UnityEngine;
using War.Entities.Projectiles;


namespace War.Weapons {

    public class AirstrikeWpn : GenericAirstrike {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "бомбардировка",
            desc => new AirstrikeWpn (desc),
            () => _.War.Assets.AirstrikeIcon
        );


        protected AirstrikeWpn (WeaponDescriptor desc) : base (desc) {}


        protected override void OnAttack () {
            Debug.Log (Target);
            
            _.War.LaunchAirstrike (() => new Bomb (), Target, 7, LeftToRight, Crosshair);
            // Crosshair.References--;
        }

    }

}