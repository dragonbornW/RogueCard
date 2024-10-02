

using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards {

    public class HandCardView : UIBehaviour {

        public Text cardName;
        public Text cardDesc;
        public Text cardCost;
        public Text cardType;

        private Card _card;

        public Card card => _card;

        public void init(Card card) {
            _card = card;

            var desc = card.desc;
            cardName.text = desc.name;
            cardDesc.text = desc.desc;
            if (desc.cost_type == CardCostType.FixedCost) {
                cardCost.text = desc.fixed_cost.ToString();
            }
            cardType.text = desc.type.ToString();
        }
        
    }

}