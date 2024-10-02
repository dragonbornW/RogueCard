

namespace ParseInvocation {

    public abstract class ArraySlice {
        public static ArraySlice<T> create<T>(T[] array) {
            return new ArraySlice<T>(array);
        }

        public static ArraySlice<T> create<T>(T[] array, int offset, int length) {
            return new ArraySlice<T>(array, offset, length);
        }
    }

    public struct ArraySlice<T> {
        public ArraySlice(T[] array) {
            _array = array;
            _offset = 0;
            _length = array.Length;
        }

        public ArraySlice(T[] array, int offset, int length) {
            _array = array;
            _offset = offset;
            _length = length;
        }

        public ref T this[int index] => ref _array[index + _offset];
        public int length => _length;

        public struct ArraySliceEnumerator {
            ArraySlice<T> slice;
            int index;
            int end;

            internal ArraySliceEnumerator(ArraySlice<T> slice) {
                this.slice = slice;
                index = slice._offset - 1;
                end = slice._offset + slice._length;
            }

            public bool MoveNext() {
                return ++index < end;
            }

            public ref T Current => ref slice._array[index];
        }

        public ArraySliceEnumerator GetEnumerator() => new ArraySliceEnumerator(this);

        private T[] _array;
        private int _offset, _length;
    }

}