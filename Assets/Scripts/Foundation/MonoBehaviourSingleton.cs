using UnityEngine;

namespace Foundation {

    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T> {

        protected static T _instance;

        public static T instance => _instance;

        protected virtual void Awake() {
            _instance = (T)this;
        }

        protected virtual void OnDestroy() {
            if( ReferenceEquals( _instance, this ) ) {
                _instance = null;
            }
        }
    }
}
