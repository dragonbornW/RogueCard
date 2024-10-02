
using System.Collections.Generic;
using UnityEngine;

namespace Worlds.Battles {

    public class Actor {

        public int hp;
        public int hp_max;

        public int shield;
        public int strength;

        public bool vulnerable;

        public ActorView view;

        private Dictionary<string, ActorPower> _powers = new Dictionary<string, ActorPower>();

        public virtual bool alive => hp > 0;

        public virtual void destroy() {
            view.fini();
        }

        public virtual void add_shield(int shield) {
            this.shield += shield;
            view.panel.update_shield(this.shield);
        }

        public virtual void reset_shield() {
            shield = 0;
            view.panel.update_shield(shield);
        }

        public virtual void do_attack(int base_damage, Actor target) {
            var damage = base_damage;
            
            damage += strength;

            target.make_hurt(damage);
        }

        public virtual void make_hurt(int damage) {

            if (vulnerable) {
                damage += damage / 2;
            }

            var blocked = Mathf.Min(damage, shield);
            if (blocked > 0) {
                damage -= blocked;
                shield -= blocked;
                view.panel.update_shield(shield);
            }
            if (damage > 0) {
                hp -= damage;
                view.panel.update_hp(hp, hp_max);
            }
        }

        public void add_power(BattleCore core, string name, int value) {
            if (_powers.TryGetValue(name, out var power)) {
                power.overlap(core, this, value);
            } else if (ActorPower.creators.TryGetValue(name, out var creator)) {
                power = creator();
                _powers.Add(name, power);
                power.enter(core, this, value);
            }
        }

        public void remove_power(BattleCore core, string name) {
            if (_powers.TryGetValue(name, out var power)) {
                _powers.Remove(name);
                power.exit(core, this);
            }
        }

        public virtual void on_round_begin(BattleCore core) {
            
            reset_shield();

            var powers = new List<ActorPower>(_powers.Count);
            foreach (var kvp in _powers) {
                powers.Add(kvp.Value);
            }
            foreach (var power in powers) {
                power.on_round_begin(core, this);
            }
        }

        public virtual void on_round_end(BattleCore core) {
            var powers = new List<ActorPower>(_powers.Count);
            foreach (var kvp in _powers) {
                powers.Add(kvp.Value);
            }
            foreach (var power in powers) {
                power.on_round_end(core, this);
            }
        }
    }

}