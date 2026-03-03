using War.Entities;


namespace War.Systems.Damage {

    public interface IDamageable : IMobile { // зачем именно так наследуется?

        bool Alive { get; }
        void TakeDamage (int damage);
        void TakeAxeDamage ();

    }

}