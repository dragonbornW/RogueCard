
using Foundation;
using MapEdit;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Worlds {

    public class WorldState : GameState {

        private static WorldState _current;

        private StateMachine<SubState> _sub_state = new StateMachine<SubState>();

        public Map map;
        public Player player;

        public static WorldState current => _current;

        public void init(MapAsset map_asset, Player player) {
            _current = this;
            map = new Map(map_asset);
            this.player = player;
        }

        public override void enter(GameState last) {
            SceneManager.LoadScene("world");
        }

        public SubState sub_state => _sub_state.current;

        public SubState next_sub_state {
            get => _sub_state.next;
            set => _sub_state.next = value;
        }

        public override void update() {
            var state = _sub_state.current;
            if (state != null) {
                _sub_state.locked = true;
                state.update();
                _sub_state.locked = false;
            }
        }

        public override void late_update() {
            var state = _sub_state.current;
            if (state != null) {
                _sub_state.locked = true;
                state.late_update();
                _sub_state.locked = false;
            }
        }


        public void notity_scene_loaded() {
            _sub_state.next = new WorldMapState();
        }
    }

}