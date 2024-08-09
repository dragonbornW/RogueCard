using UnityEngine;

namespace Worlds {

    public class MapBattleNode : MapNode {

        public readonly MapEdit.MapAsset.BattleNode desc;
        private MapNodeView _view;
        public override Vector2 position => desc.position;
        public override MapNodeView view {
            get => _view;
            set => _view = value;
        }

        public MapBattleNode( MapEdit.MapAsset.BattleNode desc) {
            
            this.desc = desc; 
        }

        public override MapNodeView load_prefab() {
            
            return Resources.Load<MapNodeView>("battle_node");
        }

        public override void do_enter() {

            var world = WorldState.current;
            world.next_sub_state = new WorldBattleState( this );
        }
    }
}