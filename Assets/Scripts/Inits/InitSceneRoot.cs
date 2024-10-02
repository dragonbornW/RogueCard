using Foundation;
using MapEdit;
using UnityEngine;

namespace Inits {

    public class InitSceneRoot : MonoBehaviourSingleton<InitSceneRoot> {

        public void start_new() {

            var world = new Worlds.WorldState();

            var map_asset = Resources.Load<MapAsset>("test_map");

            var player = new Worlds.Player();


            player.add_cards("打击", 5);
            player.add_cards("防御", 4);
            player.add_cards("痛击", 1);
            player.add_cards("连续拳", 1);
            player.add_cards("活动肌肉", 10);

            player.hp = player.hp_max = 80;

            world.init(map_asset, player);

            Game.instance.next_state = world;
        }

    }

}
