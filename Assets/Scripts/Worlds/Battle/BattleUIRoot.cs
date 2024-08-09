using UnityEngine.EventSystems;

namespace Worlds.Battle {

    public class BattleUIRoot : UIBehaviour {

        public void skip() {

            var world = WorldState.current;

            world.map.make_current_done();
            world.next_sub_state = new WorldMapState();
        }
    }
}