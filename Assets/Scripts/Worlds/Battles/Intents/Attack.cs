
using System.Collections;
using UnityEngine;

namespace Worlds.Battles.Intents {

    public class Attack : MonsterIntent {
        public override string prefab_path => "intent/attack";

        public readonly int damage;

        public Attack(int damage) {
            this.damage = damage;
        }

        public override void init(BattleCore core, Monster owner) {
            base.init(core, owner);
            _view.value.text = damage.ToString();
        }

        public override object perform(BattleCore core, Monster owner) {
            return new BattleActionCoroutine(_preform(core, owner));
        }

        private IEnumerator _preform(BattleCore core, Monster owner) {
            fini();
            owner.do_attack(damage, core.player);
            yield return new WaitForSeconds(0.5f);
        }
    }

}