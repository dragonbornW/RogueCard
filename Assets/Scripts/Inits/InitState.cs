using UnityEngine.SceneManagement;

namespace Inits {

    public class InitState : GameState {
        
        public override void enter( GameState last ) {
            SceneManager.LoadScene( "init" );
        }
    }
}