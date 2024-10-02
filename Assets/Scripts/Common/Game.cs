
using Foundation;
using UnityEngine;


public class GameState : IState<GameState> {

    public virtual void enter(GameState last) {
    
    }

    public virtual void exit() {

    }

    public virtual void update() {

    }

    public virtual void late_update() {

    }
}

public class Game : MonoBehaviourSingleton<Game> {
    //Game状态机
    private StateMachine<GameState> _state = new StateMachine<GameState>();
    //当前状态
    public GameState state => _state.current;
    //下一状态
    public GameState next_state {
        get => _state.next;
        set => _state.next = value;
    }


    private void Update() {
        var state = _state.current;
        if (state != null) {
            _state.locked = true;
            state.update();
            _state.locked = false;
        }
    }

    private void LateUpdate() {
        var state = _state.current;
        if (state != null) {
            _state.locked = true;
            state.late_update();
            _state.locked = false;
        }
    }
}
