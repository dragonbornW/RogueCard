
using UnityEngine;
using Foundation;

public class InitSceneRoot : MonoBehaviourSingleton<InitSceneRoot>{

    public void start_new() {

        Game.instance.next_state = new Worlds.WorldState();
    }
}
