using War.Entities;


namespace War.Components {

    public class Component <T> { // TODO  where T : Entity

        public T Entity;

        public virtual void OnAttach () {}
        public virtual void OnDetach () {}

    }

}
