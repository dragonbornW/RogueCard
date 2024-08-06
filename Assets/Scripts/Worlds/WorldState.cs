using UnityEngine.SceneManagement;

namespace Worlds {
    public class WorldState : GameState {
        
        public override void enter( GameState last ) {
            SceneManager.LoadScene( "World" );
        }
    }
}
