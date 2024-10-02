
namespace ParseInvocation {

    public enum ArgumentType {
        Func,
        Bool,
        Number,
        Identifier,
        StringLiteral,
    }

    public interface IArgument {
        ArgumentType type { get; }
        bool try_assign(System.Type pi, ref object p);
    }

    public struct Bool : IArgument {
        public bool value;

        ArgumentType IArgument.type => ArgumentType.Bool;

        public bool try_assign(System.Type pi, ref object p) {
            if (pi.IsAssignableFrom(GetType())) {
                p = this;
                return true;
            }
            if (pi.IsAssignableFrom(typeof(bool))) {
                p = value;
                return true;
            }
            return false;
        }
    }

    public struct Identifier : IArgument {
        public string value;

        ArgumentType IArgument.type => ArgumentType.Identifier;

        public bool try_assign(System.Type pi, ref object p) {
            if (pi.IsAssignableFrom(GetType())) {
                p = this;
                return true;
            }
            if (pi.IsAssignableFrom(typeof(string))) {
                p = value;
                return true;
            }
            return false;
        }
    }

    public struct StringLiteral : IArgument {
        public string value;

        ArgumentType IArgument.type => ArgumentType.StringLiteral;

        public bool try_assign(System.Type pi, ref object p) {
            if (pi.IsAssignableFrom(typeof(string))) {
                p = value;
                return true;
            }
            return false;
        }
    }

}