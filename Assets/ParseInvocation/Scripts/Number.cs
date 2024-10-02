
namespace ParseInvocation {

    public enum NumberType {
        Int,
        Long,
        Float,
        Double,
    }

    public abstract class Number : IArgument {
        public abstract NumberType type { get; }
        public abstract int as_int { get; }
        public abstract long as_long { get; }
        public abstract float as_float { get; }
        public abstract double as_double { get; }

        ArgumentType IArgument.type => ArgumentType.Number;
        bool IArgument.try_assign(System.Type pi, ref object p) {
            if (pi.IsAssignableFrom(GetType())) {
                p = this;
                return true;
            }
            return try_assign(pi, ref p);
        }
        protected abstract bool try_assign(System.Type pi, ref object p);
    }
    
    public class NumberInt : Number {
        public int value;
        
        public override NumberType type => NumberType.Int;
        public override int as_int => value;
        public override long as_long => value;
        public override float as_float => value;
        public override double as_double => value;
        protected override bool try_assign(System.Type pi, ref object p) {
            if (pi.IsAssignableFrom(typeof(int))) {
                p = value;
                return true;
            }
            return false;
        }
    }

    public class NumberLong : Number {
        public long value;
        public override NumberType type => NumberType.Long;
        public override int as_int => (int)value;
        public override long as_long => value;
        public override float as_float => value;
        public override double as_double => value;
        protected override bool try_assign(System.Type pi, ref object p) {
            if (pi.IsAssignableFrom(typeof(long))) {
                p = value;
                return true;
            }
            return false;
        }
    }

    public class NumberFloat : Number {
        public float value;
        public override NumberType type => NumberType.Float;
        public override int as_int => (int)value;
        public override long as_long => (long)value;
        public override float as_float => value;
        public override double as_double => value;
        protected override bool try_assign(System.Type pi, ref object p) {
            if (pi.IsAssignableFrom(typeof(float))) {
                p = value;
                return true;
            }
            return false;
        }
    }

    public class NumberDouble : Number {
        public double value;
        public override NumberType type => NumberType.Double;
        public override int as_int => (int)value;
        public override long as_long => (long)value;
        public override float as_float => (float)value;
        public override double as_double => value;

        protected override bool try_assign(System.Type pi, ref object p) {
            if (pi.IsAssignableFrom(typeof(double))) {
                p = value;
                return true;
            }
            if (pi.IsAssignableFrom(typeof(float))) {
                p = (float)value;
                return true;
            }
            return false;
        }
    }

}