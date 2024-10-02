


using UnityEngine;

namespace Worlds.Battles {

    public class ActorView : MonoBehaviour {

        private Actor _actor;
        private ActorUIPanel _panel;

        public Actor actor => _actor;
        public ActorUIPanel panel => _panel;

        public void init(Actor actor) {
            _actor = actor;

            var prefab = Resources.Load<ActorUIPanel>("actor_ui_panel");

            var state = WorldBattleState.instance;

            _panel = Instantiate(prefab, state.ui_root.sceneLayer, false);

            _panel.update_hp(actor.hp, actor.hp_max);

            _panel.update_shield(actor.shield);
        }

        public void fini() {
            Destroy(_panel.gameObject);
            Destroy(gameObject);
        }


        public void set_position(Vector2 position) {
            transform.position = position;

            var root = WorldSceneRoot.instance;

            _panel.transform.position = root.mainCamera.WorldToScreenPoint(position);
        }
    }

}