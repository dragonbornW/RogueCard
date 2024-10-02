

namespace Worlds.Battles.Powers {

    public class Vulnerable : ActorPower {

        public const string NAME = "рвик";

        private int _value;

        public override string prefab_path => "power/vulnerable";

        public override void enter(BattleCore core, Actor owner, int value) {
            base.enter(core, owner, value);
            _value = value;
            _view.value.text = _value.ToString();

            owner.vulnerable = true;
        }

        public override void overlap(BattleCore core, Actor owner, int value) {
            _value += value;
            _view.value.text = _value.ToString();
        }

        public override void exit(BattleCore core, Actor owner) {
            base.exit(core, owner);
            owner.vulnerable = false;
        }

        public override void on_round_end(BattleCore core, Actor owner) {
            if (--_value == 0) {
                owner.remove_power(core, NAME);
            } else {
                _view.value.text = _value.ToString();
            }
        }
    }

}