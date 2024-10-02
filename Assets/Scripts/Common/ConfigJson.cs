
using UnityEngine;

namespace Common {

    [System.Serializable]
    public class ConfigJson {

        public Cards.CardDesc[] cards;
        public Monsters.MonsterDesc[] monsters;

        public static ConfigJson load_from_streaming_assets(string path) {
            return load_from_path(System.IO.Path.Combine(Application.streamingAssetsPath, path));
        }

        public static ConfigJson load_from_path(string path) {
            var content = System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8);
            return JsonUtility.FromJson<ConfigJson>(content);
        }
    }
}