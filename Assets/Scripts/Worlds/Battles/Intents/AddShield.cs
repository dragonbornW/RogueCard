
using System.Collections;
using UnityEngine;

namespace Worlds.Battles.Intents {

    public class AddShield : MonsterIntent {

        public readonly int shield;

        public override string prefab_path => "intent/defend";

        public AddShield(int shield) {
            this.shield = shield;
        }

        public override object perform(BattleCore core, Monster owner) {
            return new BattleActionCoroutine(_preform(owner));
        }

        private IEnumerator _preform(Monster owner) {
            fini();
            owner.add_shield(shield);
            yield return new WaitForSeconds(0.5f);
        }
    }

}