
using Cards;
using System.Collections.Generic;
using UnityEngine;

namespace Worlds {

    public class Player {

        public readonly List<CardDesc> cards = new List<CardDesc>();
        public int hp_max;
        public int hp;
        public int energy_max = 3;
        public int draw_card_count = 5;

        public void add_cards(string name, int count) {
            if (CardDesc.all_cards.TryGetValue(name, out var desc)) {
                for (int i = 0; i < count; ++i) {
                    cards.Add(desc);
                }
            } else {
                Debug.LogError($"card `{name}` not exist");
            }
        }
    }

}