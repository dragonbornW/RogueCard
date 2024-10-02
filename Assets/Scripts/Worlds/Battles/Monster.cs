
using Monsters;
using UnityEngine;

namespace Worlds.Battles {

    public class Monster : Actor {

        public readonly MonsterDesc desc;

        private MonsterIntent _intent;
        private MonsterIntentGenerator _generator;

        public Monster(MonsterDesc desc) {
            this.desc = desc;
            hp = hp_max = Mathf.Max(1, desc.hp);
            _generator = desc.real_intent_generator?.create(this);
        }

        public virtual void generate_intent(BattleCore core) {
            if (_generator != null) {
                _intent = _generator.generate(core);
                _intent?.init(core, this);
            }
        }

        public virtual object preform_intent(BattleCore core) {
            var intent = _intent;
            _intent = null;
            return intent?.perform(core, this);
        }
    }

}