
namespace Worlds.Battles {

    public class ActorSingleSeat : ActorSeat {

        

        public override void add(ActorView view) {
            // view.transform.position = transform.position;
            view.set_position(transform.position);    
        }

        public override void clear() {
            
        }

        public override void remove(ActorView view) {
            
        }
    }

}