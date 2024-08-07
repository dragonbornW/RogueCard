using Foundation;

namespace Worlds {

    public class WorldSenceRoot : MonoBehaviourSingleton<WorldSenceRoot> {

        public MapView map_view;
        protected override void Awake() {

            base.Awake();

            var world = new WorldState().current;
            map_view.init( world.map );
            world.notity_scene_load();
        }
    }
}