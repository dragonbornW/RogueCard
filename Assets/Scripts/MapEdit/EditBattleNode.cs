

namespace MapEdit {
    public class EditBattleNode : EditNode {

        public string[] monsters;

        public override MapAsset.Node create_node() {
            return new MapAsset.BattleNode() {
                monsters = monsters,
            };
        }
    }
}

