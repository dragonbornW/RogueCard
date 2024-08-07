namespace Foundation {

    public interface IState<T> where T : class, IState<T> {
        void enter( T last );
        void exit();
    }

    public class StateMachine<T> where T : class, IState<T> {

        private T _current;
        private T _next;
        private bool _next_changed;
        private bool _locked;

        //��ǰ״̬
        public T current => _current;
        //��һ״̬
        public T next {
            get => _next;
            set {
                if( _next != value ) {
                    _next = value;
                    _next_changed = true;
                    if( !_locked ) {
                        check_next();
                    }
                }
            }
        }
        //��ǰ״̬����
        public bool locked {
            get => _locked;
            set {
                if( _locked != value ) {
                    _locked = value;
                    if( !_locked ) {
                        check_next();
                    }
                }
            }
        }

        private void check_next() {
            while( _next_changed ) {
                _next_changed = false;
                var old = _current;
                _current = _next;
                old?.exit();
                _current?.enter( old );
            }
        }
    }
}