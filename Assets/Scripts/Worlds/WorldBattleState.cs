using UnityEngine;
using Worlds.Battle;

namespace Worlds {

    public class WorldBattleState : SubState {    

        public readonly MapBattleNode node;
        private BattleUIRoot _ui_root;

        public WorldBattleState(MapBattleNode node ) {

            this.node = node;
        }

        public override void enter( SubState state ) {
            
            var ui_prefab = Resources.Load<BattleUIRoot>("BattleUI");
            var root = WorldSceneRoot.instance;
            _ui_root = Object.Instantiate( ui_prefab, root.ui_root.transform, false );
        }

        public override void exit() {
            
            Object.Destroy( _ui_root.gameObject );
        }
    }
}