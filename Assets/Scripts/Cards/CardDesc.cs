
using System.Collections.Generic;
using UnityEngine;

namespace Cards {

    public enum CardType {
        Invalid = 0,
        Attack,
        Skill,
        Power,
        Status,
        Curse,
    }

    public enum CardCostType {
        FixedCost = 0,
        AllRemains,
    }

    public enum CardTarget {
        None = 0,
        Monster,
    }

    [System.Serializable]
    public class CardDesc {
        public string name;
        public string desc;
        public CardType type;
        public CardCostType cost_type;
        public int fixed_cost;
        public CardTarget target;
        public string action;

        private CardAction _action;

        public CardAction real_action => _action;

        public void init() {
            if (!CardAction.try_parse(action, out _action, out var err_msg)) {
                Debug.LogError($"card `{name}`: parse action failed: {err_msg}");
            }
        }

        public readonly static Dictionary<string, CardDesc> all_cards = new Dictionary<string, CardDesc>();
    }

}