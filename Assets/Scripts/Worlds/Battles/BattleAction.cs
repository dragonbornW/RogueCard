
using System.Collections;
using UnityEngine;

namespace Worlds.Battles {

    public interface IBattleAction {
        void start(WorldBattleState state);
        bool done { get; }
    }

    public class BattleActionCoroutine : IBattleAction {

        private IEnumerator _coroutine;
        private bool _done;
        
        public BattleActionCoroutine(IEnumerator coroutine) {
            _coroutine = coroutine;
        }

        bool IBattleAction.done => _done;

        void IBattleAction.start(WorldBattleState state) {
            state.ui_root.StartCoroutine(func());
        }

        IEnumerator func() {
            yield return _coroutine;
            _done = true;
        }
    }
}