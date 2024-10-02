using Foundation;
using System.Collections.Generic;
using UnityEngine;
using Worlds.Battles;

namespace Worlds {

    public class WorldSceneRoot : MonoBehaviourSingleton<WorldSceneRoot> {

        public Canvas ui_root;
        public RectTransform ui_main;
        public RectTransform ui_top;
        public MapView map_view;
        public Camera mainCamera;

        public readonly Dictionary<string, ActorSeat> seats = new Dictionary<string, ActorSeat>();

        protected override void Awake() {
            base.Awake();

            foreach (var seat in GetComponentsInChildren<ActorSeat>()) {
                seats[seat.name] = seat;
            }

            var world = WorldState.current;

            map_view.init(world.map);

            world.notity_scene_loaded();
        }


        public void open_card_lib() {
            var prefab = Resources.Load<UICardLibrary>("ui_card_library");
            var panel = Instantiate(prefab, ui_root.transform, false);
            panel.show();
        }
    }


}