
using System.Collections;
using UnityEngine;
using Worlds;
using Worlds.Battles;
using static UnityEngine.Rendering.DebugUI;

namespace Cards.Actions {

    public class _seq : CardAction {

        public readonly CardAction[] actions;

        public _seq(params CardAction[] actions) {
            this.actions = actions;
        }

        public override object perform(WorldBattleState battle_state, Card card, Actor target) {
            return _perform(battle_state, card, target);
        }

        private IEnumerator _perform(WorldBattleState battle_state, Card card, Actor target) {
            foreach (var action in actions) {
                yield return action.perform(battle_state, card, target);
            }
        }
    }

    public class Repeat : CardAction {

        public readonly CardAction action;
        public readonly int count;

        public Repeat(CardAction action, int count) {
            this.action = action;
            this.count = count;
        }

        public override object perform(WorldBattleState battle_state, Card card, Actor target) {
            return _perform(battle_state, card, target);
        }

        private IEnumerator _perform(WorldBattleState battle_state, Card card, Actor target) {
            for (int i = 0; i < count; ++i) {
                yield return action.perform(battle_state, card, target);
            }
        }
    }

    public class MakeDamage : CardAction {

        public readonly int damage;

        public MakeDamage(int damage) {
            this.damage = damage;
        }

        public override object perform(WorldBattleState battle_state, Card card, Actor target) {
            return new BattleActionCoroutine(_perform(battle_state, target));
        }

        IEnumerator _perform(WorldBattleState battle_state, Actor target) {
            if (target != null) {
                battle_state.core.player.do_attack(damage, target);
            }
            yield return new WaitForSeconds(0.125f);
        }
    }

    public class GenerateShield : CardAction {
        public readonly int shield;
        public GenerateShield(int shield) {
            this.shield = shield;
        }

        public override object perform(WorldBattleState battle_state, Card card, Actor target) {
            return new BattleActionCoroutine(_perform(battle_state));
        }

        IEnumerator _perform(WorldBattleState battle_state) {
            battle_state.core.player.add_shield(shield);
            yield return new WaitForSeconds(0.125f);
        }
    }

    public class ApplyBuff : CardAction {
        public readonly string name;
        public readonly int level;

        public ApplyBuff(string name, int level) {
            this.name = name;
            this.level = level;
        }

        public override object perform(WorldBattleState battle_state, Card card, Actor target) {
            return new BattleActionCoroutine(_perform(battle_state, target));
        }

        IEnumerator _perform(WorldBattleState battle_state, Actor target) {
            target.add_power(battle_state.core, name, level);
            yield return new WaitForSeconds(0.125f);
        }
    }

    public class ApplySelfBuff : CardAction {
        public readonly string name;
        public readonly int level;

        public ApplySelfBuff(string name, int level) {
            this.name = name;
            this.level = level;
        }

        public override object perform(WorldBattleState battle_state, Card card, Actor target) {
            return new BattleActionCoroutine(_perform(battle_state));
        }

        IEnumerator _perform(WorldBattleState battle_state) {
            battle_state.core.player.add_power(battle_state.core, name, level);
            yield return new WaitForSeconds(0.125f);
        }
    }

    public class AddStrength : CardAction {
        public readonly int strength;

        public AddStrength(int strength) {
            this.strength = strength;
        }

        public override object perform(WorldBattleState battle_state, Card card, Actor target) {
            battle_state.core.player.strength += strength;
            return null;
        }
    }
}