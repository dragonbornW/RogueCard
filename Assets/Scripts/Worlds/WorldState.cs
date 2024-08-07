
using UnityEngine.SceneManagement;
using Foundation;
using MapEdit;
using UnityEngine;
namespace Worlds {
    public class WorldState : GameState {

        private StateMachine<SubState> _sub_state = new StateMachine<SubState>();
        private static WorldState _current;
        public Map map;
        public WorldState current => _current;
        public SubState sub_state => _sub_state.current;
        public SubState next_sub_state {
            get => _sub_state.next;
            set => _sub_state.next = value;
        }

        public void init( MapAsset map_asset ) {

            map = new Map(map_asset);
            _current = this;
        }

        public override void enter( GameState last ) {

            SceneManager.LoadScene( "World" );
        }

        public override void update() {
            var state = _sub_state.current;
            if( state != null ) {
                _sub_state.locked = true;
                state.update();
                _sub_state.locked = false;
            }
        }

        public override void late_update() {
            var state = _sub_state.current;
            if( state != null ) {
                _sub_state.locked = true;
                state.late_update();
                _sub_state.locked = false;
            }
        }

        public void notity_scene_load() {

            _sub_state.next = new WorldMapState();
        }
    }

}
