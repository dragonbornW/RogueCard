using UnityEngine;
using Foundation;

public class GameState : IState<GameState> {

    public virtual void enter( GameState last ) {

    }
    public virtual void exit() {

    }
    public virtual void update() {

    }
    public virtual void late_update() {

    } 
}

public class Game : MonoBehaviourSingleton<Game> { 

    private StateMachine<GameState> _state = new StateMachine<GameState>();

    public GameState state => _state.current;
    public GameState next_state {
        get => _state.next;
        set => _state.next = value;
    }

    private void Update() {

        var state = _state.current;
        if( state != null ) {
            _state.locked = true;
            state.update();
            _state.locked = false;
        }
    }
    private void LateUpdate() {

        var state = _state.current;
        if( state != null ) {
            _state.locked = true;
            state.update();
            _state.locked = false;
        }
    }
}
