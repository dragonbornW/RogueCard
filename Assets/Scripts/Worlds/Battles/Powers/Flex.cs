
namespace Worlds.Battles.Powers {

    public class Flex : ActorPower {

        public const string NAME = "»î¶¯¼¡Èâ";

        private int _value;

        public override string prefab_path => "power/flex";

        
        public override void enter(BattleCore core, Actor owner, int value) {
            base.enter(core, owner, value);
            _value = value;
            _view.value.text = _value.ToString();
        }

        public override void overlap(BattleCore core, Actor owner, int value) {
            _value += value;
            _view.value.text = _value.ToString();
        }

        public override void exit(BattleCore core, Actor owner) {
            base.exit(core, owner);
            owner.strength -= _value;
        }

        public override void on_round_end(BattleCore core, Actor owner) {
            owner.remove_power(core, NAME);
        }
    }

}