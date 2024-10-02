
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Worlds.Battles {

    public class ActorUIPanel : UIBehaviour {

        public Slider hp;
        public Text hpValue;

        public GameObject shield;
        public Text shieldValue;
        public RectTransform powerContent;
        public RectTransform intentContent;

        public void update_hp(int hp, int hp_max) {
            hpValue.text = $"{hp} / {hp_max}";
            this.hp.value = (float)hp / hp_max;
        }

        public void update_shield(int shield) {
            if (shield == 0) {
                this.shield.SetActive(false);
            } else {
                this.shield.SetActive(true);
                shieldValue.text = shield.ToString();
            }
        }
    }

}