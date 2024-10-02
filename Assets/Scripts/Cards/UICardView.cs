using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards {

    public class UICardView : UIBehaviour {

        public Text cardName;
        public Text cardDesc;
        public Text cardCost;
        public Text cardType;

        public void init(CardDesc desc) {

            cardName.text = desc.name;
            cardDesc.text = desc.desc;
            if (desc.cost_type == CardCostType.FixedCost) {
                cardCost.text = desc.fixed_cost.ToString();
            } else {
                cardCost.text = "X";
            }
            cardType.text = desc.type.ToString();
        }

    }

}