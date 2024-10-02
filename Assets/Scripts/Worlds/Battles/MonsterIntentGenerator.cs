

using ParseInvocation;

namespace Worlds.Battles {

    public abstract class MonsterIntentGenerator {
        public abstract MonsterIntent generate(BattleCore core);
    }

    public abstract class MonsterIntentGeneratorCreate : Invocation {

        public static bool try_parse(string content, out MonsterIntentGeneratorCreate generator_create, out string err_msg) {
            return try_parse(content, "Worlds.Battles.IntentGenerators.", "Assembly-CSharp", out generator_create, out err_msg);
        }

        public abstract MonsterIntentGenerator create(Monster owner);
    }

}