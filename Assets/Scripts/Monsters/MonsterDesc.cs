
using System.Collections.Generic;
using UnityEngine;
using Worlds.Battles;

namespace Monsters {

    [System.Serializable]
    public class MonsterDesc {

        public string id;
        public int hp;

        public string view_prefab;

        public string intent_generator;

        private MonsterIntentGeneratorCreate _intent_generator;

        public MonsterIntentGeneratorCreate real_intent_generator => _intent_generator;

        public void init() {
            if (!MonsterIntentGeneratorCreate.try_parse(intent_generator, out _intent_generator, out var err_msg)) {
                Debug.LogError($"Monster `{id}`: parse intent generator failed: {err_msg}"); 
            }
        }

        public readonly static Dictionary<string, MonsterDesc> all_monsters = new Dictionary<string, MonsterDesc>();
    }

}