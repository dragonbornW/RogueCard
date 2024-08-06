using UnityEngine;

namespace Common {

    public class Main : MonoBehaviour {

        private void Awake() {

            var game = new GameObject("[Game]").AddComponent<Game>();

            DontDestroyOnLoad( game.gameObject );

            game.next_state = new Inits.InitState();
        }
    }
}







