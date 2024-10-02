
using UnityEngine;
using Worlds.Battles;

namespace Worlds {

    public class WorldBattleState : SubState {

        public readonly MapBattleNode node;

        private BattleUIRoot _ui_root;

        public readonly BattleCore core = new BattleCore();
        public BattleUIRoot ui_root => _ui_root;

        public static WorldBattleState instance { get; private set; }

        public WorldBattleState(MapBattleNode node) {
            this.node = node;
        }

        public override void enter(SubState last) {
            instance = this;
            var ui_prefab = Resources.Load<BattleUIRoot>("battle_ui");
            var root = WorldSceneRoot.instance;
            _ui_root = Object.Instantiate(ui_prefab, root.ui_main, false);

            _ui_root.init(this);

            core.init(this);
        }

        public override void update() {
            core.update(this);
        }

        public override void exit() {
            Object.Destroy(_ui_root.gameObject);

            instance = null;
        }
    }

}