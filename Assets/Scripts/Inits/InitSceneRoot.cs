
using UnityEngine;
using Foundation;
using MapEdit;

public class InitSceneRoot : MonoBehaviourSingleton<InitSceneRoot>{

    public void start_new() {

        var world = new Worlds.WorldState();
        var map_asset = Resources.Load<MapAsset>("test_map");
        world.init( map_asset );
        Game.instance.next_state = world;
    }
}
