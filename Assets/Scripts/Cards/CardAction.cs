
using ParseInvocation;
using Worlds.Battles;
using Worlds;

namespace Cards {

    public abstract class CardAction : Invocation {

        public static bool try_parse(string content, out CardAction action, out string err_msg) {
            return try_parse(content, "Cards.Actions.", "Assembly-CSharp", out action, out err_msg);
        }

        public abstract object perform(WorldBattleState battle_state, Card card, Actor target);
    }

}