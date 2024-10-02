
using UnityEngine;

namespace Worlds.Battles {

    public abstract class MonsterIntent {

        protected MonsterIntentView _view;

        public abstract string prefab_path { get; }

        public virtual void init(BattleCore core, Monster owner) {
            var prefab = Resources.Load<MonsterIntentView>(prefab_path);
            if (prefab != null) {
                _view = Object.Instantiate(prefab, owner.view.panel.intentContent, false);
            }
        }

        public virtual object perform(BattleCore core, Monster owner) {
            fini();
            return null;
        }

        public virtual void fini() {
            if (_view != null) {
                Object.Destroy(_view.gameObject);
            }
        }
    }

}