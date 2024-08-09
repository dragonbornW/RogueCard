using Foundation;
using UnityEngine;

namespace Worlds {

    public class WorldSceneRoot : MonoBehaviourSingleton<WorldSceneRoot> {

        public MapView map_view;
        public Canvas ui_root;

        protected override void Awake() {

            base.Awake();

            var world = WorldState.current;
            map_view.init( world.map );
            world.notity_scene_load();
        }
    }
}