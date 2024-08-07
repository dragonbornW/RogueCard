using UnityEngine;

namespace Worlds {

    public class MapBattleNode : MapNode {

        public readonly MapEdit.MapAsset.BattleNode desc;

        public override Vector2 position => desc.position;

        public MapBattleNode( MapEdit.MapAsset.BattleNode desc) {
            
            this.desc = desc; 
        }

        public override MapNodeView load_prefab() {
            
            return Resources.Load<MapNodeView>("battle_node");
        }
    }
}