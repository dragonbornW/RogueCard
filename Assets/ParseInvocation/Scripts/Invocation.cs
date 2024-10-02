
using System.Collections.Generic;

namespace ParseInvocation {
    public abstract class Invocation : IArgument {
        public static bool try_parse<T>(string content, System.Func<string, ArraySlice<IArgument>, Invocation> creator, out T invocation, out string errmsg) where T : Invocation {
            using (var parser = new Parser { creator = creator }) {
                var length = System.Text.Encoding.UTF8.GetBytes(content, 0, content.Length, s_buffer, 0);
                Utility.parse(s_buffer, length, parser);
                if (parser.count != 1 || s_args[0] == null) {
                    invocation = null;
                    errmsg = parser.err_msg;
                    return false;
                }
                invocation = s_args[0] as T;
                errmsg = null;
                return true;
            }
        }

        public static bool try_parse<T>(string content, string class_prefix, string assembly_name, out T invocation, out string errmsg) where T : Invocation {
            return try_parse(content, (name, args) => reflection_creator($"{class_prefix}{name}, {assembly_name}", args), out invocation, out errmsg);
        }

        public static bool try_parse<T>(string content, int start_index, int length, System.Func<string, ArraySlice<IArgument>, Invocation> creator, out T invocation, out string errmsg) where T : Invocation {
            using (var parser = new Parser { creator = creator }) {
                var byte_length = System.Text.Encoding.UTF8.GetBytes(content, start_index, length, s_buffer, 0);
                Utility.parse(s_buffer, byte_length, parser);
                if (parser.count != 1 || s_args[0] == null) {
                    invocation = null;
                    errmsg = parser.err_msg;
                    return false;
                }
                invocation = s_args[0] as T;
                errmsg = null;
                return true;
            }
        }

        public static bool try_parse<T>(string content, int start_index, int length, string class_prefix, string assembly_name, out T invocation, out string errmsg) where T : Invocation {
            return try_parse(content, start_index, length, (name, args) => reflection_creator($"{class_prefix}{name}, {assembly_name}", args), out invocation, out errmsg);
        }

        ArgumentType IArgument.type => ArgumentType.Func;
        bool IArgument.try_assign(System.Type pi, ref object p) {
            if (pi.IsAssignableFrom(this.GetType())) {
                p = this;
                return true;
            }
            return false;
        }

        private static byte[] s_buffer = new byte[8192];
        private static IArgument[] s_args = new IArgument[16];
        private static Stack<(string name, int argv_offset, int argc)> s_invocations = new Stack<(string name, int argv_offset, int argc)>();

        private class Parser : Utility.IParseResult, System.IDisposable {
            public int count = 0;
            public string err_msg;
            public System.Func<string, ArraySlice<IArgument>, Invocation> creator;

            public void begin_invocation(string name, int argc) {
                s_invocations.Push((name, count, argc));
            }
            public void end_invocation() {
                var r = s_invocations.Pop();
                var args = ArraySlice.create(s_args, r.argv_offset, r.argc);
                Invocation invocation;
                if (err_msg == null) {
                    invocation = creator(r.name, args);
                } else {
                    invocation = null;
                }
                foreach (ref var a in args) {
                    a = null;
                }
                count -= r.argc;
                push_arg(invocation);
                if (invocation == null) {
                    if (err_msg == null) {
                        err_msg = $"unknown invocation \'{r.name}\'";
                    }
                }
            }
            public void boolean(bool value) {
                push_arg(new Bool { value = value });
            }
            public void number(Number value) {
                push_arg(value);
            }
            public void identifier(string name) {
                push_arg(new Identifier { value = name });
            }
            public void string_literal(string value) {
                push_arg(new StringLiteral { value = value });
            }
            public void err(string msg) {
                err_msg = msg;
            }

            public void Dispose() {
                foreach (ref var arg in ArraySlice.create(s_args, 0, count)) {
                    arg = null;
                }
            }

            private void push_arg(IArgument arg) {
                if (count == s_args.Length) {
                    var args = new IArgument[count * 2];
                    s_args.CopyTo(args, 0);
                    s_args = args;
                }
                s_args[count] = arg;
                ++count;
            }
        }

        private static Invocation reflection_creator(string name, ArraySlice<IArgument> args) {
            try {
                var ti = System.Type.GetType(name);
                if (ti != null && typeof(Invocation).IsAssignableFrom(ti)) {
                    var ctor = ti.GetConstructor(s_default_ctor_types);
                    if (ctor != null) {
                        return (Invocation)ctor.Invoke(new object[] { args });
                    }
                    List<object> ps = new List<object>();
                    foreach (var ci in ti.GetConstructors()) {
                        var pia = ci.GetParameters();
                        object p = null;
                        if (pia.Length == 0) {
                            if (args.length == 0) {
                                return (Invocation)ci.Invoke(null);
                            }
                        } else {
                            ps.Clear();
                            ctor = ci;
                            var last = pia.Length - 1;
                            for (int i = 0; i < last; ++i) {
                                if (i == args.length) {
                                    ctor = null;
                                    break;
                                }
                                if (!args[i].try_assign(pia[i].ParameterType, ref p)) {
                                    ctor = null;
                                    break;
                                }
                                ps.Add(p);
                            }
                            if (ctor == null) {
                                continue;
                            }
                            var pi = pia[last];
                            foreach (var _ in pi.GetCustomAttributes(typeof(System.ParamArrayAttribute), false)) {
                                var et = pi.ParameterType.GetElementType();
                                var array = System.Array.CreateInstance(et, args.length - last);
                                for (int i = 0; i < array.Length; ++i) {
                                    if (!args[last + i].try_assign(et, ref p)) {
                                        ctor = null;
                                        break;
                                    }
                                    array.SetValue(p, i);
                                }
                                if (ctor == null) {
                                    break;
                                } 
                                ps.Add(array);
                                return (Invocation)ctor.Invoke(ps.ToArray());
                            }
                            if (ctor == null) {
                                continue;
                            }
                            if (last + 1 != args.length) {
                                continue;
                            }
                            
                            if (!args[last].try_assign(pia[last].ParameterType, ref p)) {
                                continue;
                            }
                            ps.Add(p);
                            return (Invocation)ctor.Invoke(ps.ToArray());
                        }
                    }
                }
            } catch (System.Exception) {
                
            }
            return null;
        }

        private static System.Type[] s_default_ctor_types = new System.Type[] { typeof(ArraySlice<IArgument>) };
    }

}