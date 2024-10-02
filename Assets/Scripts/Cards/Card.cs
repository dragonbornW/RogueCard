

using Worlds.Battles;

namespace Cards {

    public class Card {
        public readonly CardDesc desc;

        public int action_cost;

        public Card(CardDesc desc) {
            this.desc = desc;
        }

        public bool validate(BattleCore core) {
            if (desc.cost_type == CardCostType.AllRemains) {
                return true;
            }
            if (desc.fixed_cost <= core.energy) {
                return true;
            }
            return false;
        }
    }

}