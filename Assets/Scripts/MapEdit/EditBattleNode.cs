
namespace MapEdit {

    public class EditBattleNode : EditNode {

        public override MapAsset.Node create_node() {
            return new MapAsset.BattleNode();
        }
    }
}


