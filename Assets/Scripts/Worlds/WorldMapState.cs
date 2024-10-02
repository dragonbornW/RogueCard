

namespace Worlds {

    public class WorldMapState : SubState {

        public override void enter(SubState last) {
            WorldSceneRoot.instance.map_view.gameObject.SetActive(true);
        }

        public override void exit() {
            WorldSceneRoot.instance.map_view.gameObject.SetActive(false);
        }

        public void select_node(MapNode node) {
            var world = WorldState.current;
            if (world.map.select_node(node)) {
                node.do_enter();
            }
        }
    }

}