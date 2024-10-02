
using UnityEngine;

namespace Worlds.Battles {

    public abstract class ActorSeat : MonoBehaviour {

        public abstract void add(ActorView view);

        public abstract void remove(ActorView view);

        public abstract void clear();

    }

}