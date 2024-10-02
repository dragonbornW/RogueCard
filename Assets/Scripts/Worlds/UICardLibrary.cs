
using Cards;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Worlds {

    public class UICardLibrary : UIBehaviour {

        public RectTransform content;
        public UICardView cardPrefab;

        public void show() {

            var player = WorldState.current.player;

            foreach (var card in player.cards) {
                var view = Instantiate(cardPrefab, content, false);
                view.init(card);
            }

        }

        public void close() {
            Destroy(gameObject);
        }
    }

}