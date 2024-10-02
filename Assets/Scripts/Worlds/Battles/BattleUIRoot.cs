
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Worlds.Battles {

    public class BattleUIRoot : UIBehaviour {

        public Cards.HandCardDeck handCardDeck;
        public Text energyValue;

        public GameObject endRoundButton;

        public RectTransform sceneLayer;

        private WorldBattleState _battle_state;

        public void init(WorldBattleState battle_state) {
            _battle_state = battle_state;
            endRoundButton.SetActive(false);

            handCardDeck.init(battle_state);
        }

        public void update_energy(int value) {
            energyValue.text = value.ToString();
        }

        public void show_end_round() {
            endRoundButton.SetActive(true);
        }

        public void end_round() {
            endRoundButton.SetActive(false);
            _battle_state.core.player_wanna_end_round = true;
        }
    }

}