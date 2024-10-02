
using UnityEngine; 

namespace Common {

    public class Main : MonoBehaviour {

        private void Awake() {

            var game = new GameObject("[Game]").AddComponent<Game>();

            DontDestroyOnLoad(game.gameObject);

            load_configs();

            game.next_state = new Inits.InitState();
        }

        private void load_configs() {
            try {
                var configs = ConfigJson.load_from_streaming_assets("configs.json");

                foreach (var card in configs.cards) {
                    if (!Cards.CardDesc.all_cards.TryAdd(card.name, card)) {
                        Debug.LogError($"duplicated card: {card.name}");
                    } else {
                        card.init();
                    }
                }

                foreach (var monster in configs.monsters) { 
                    if (!Monsters.MonsterDesc.all_monsters.TryAdd(monster.id, monster)) {
                        Debug.LogError($"duplicated monster: {monster.id}");
                    } else {
                        monster.init();
                    }
                }
            } catch (System.Exception e) {
                Debug.LogException(e);
            }
        }
    }
}
