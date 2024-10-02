

namespace Worlds.Battles.IntentGenerators {

    public class Test : MonsterIntentGeneratorCreate {

        public readonly bool attack_first;
        public readonly int damage;
        public readonly int shield;

        public Test(int damage, int shield) : this(damage, shield, true) {

        }

        public Test(int damage, int shield, bool attack_first) {
            this.attack_first = attack_first;
            this.damage = damage;
            this.shield = shield;
        }

        public override MonsterIntentGenerator create(Monster owner) {
            return new Generator(this);
        }

        private class Generator : MonsterIntentGenerator {
            public readonly Test create;
            private bool _last_attack;

            public Generator(Test create) {
                this.create = create;
                _last_attack = !create.attack_first;
            }

            public override MonsterIntent generate(BattleCore core) {
                if (_last_attack) {
                    _last_attack = false;
                    return new Intents.AddShield(create.shield);
                } else {
                    _last_attack = true;
                    return new Intents.Attack(create.damage);
                }
            }
        }
    }

}