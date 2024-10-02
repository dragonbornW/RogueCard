
using System;
using System.Runtime.InteropServices;

namespace ParseInvocation {

    public static class Utility {
        internal const string PLUGIN_NAME =
#if UNITY_STANDALONE_WIN
            "parse_invocation"
#elif UNITY_IOS
            "__Internal"
#else
            "libparse_invocation"
#endif
            ;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void BeginInvocationFunc(IntPtr obj, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]byte[] name, UIntPtr name_len, UIntPtr argc);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void EndInvocationFunc(IntPtr obj);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void IntFunc(IntPtr obj, int value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void LongFunc(IntPtr obj, long value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void FloatFunc(IntPtr obj, float value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void DoubleFunc(IntPtr obj, double value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void StringFunc(IntPtr obj, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]byte[] buf, UIntPtr buf_len);


        [StructLayout(LayoutKind.Sequential)] 
        internal struct CallbackFunctions {
            public BeginInvocationFunc begin_invocation;
            public EndInvocationFunc end_invocation;
            public IntFunc boolean;
            public IntFunc int32;
            public LongFunc int64;
            public FloatFunc float32;
            public DoubleFunc float64;
            public StringFunc identifier;
            public StringFunc string_literal;
            public StringFunc errmsg;
        }

        [DllImport(PLUGIN_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void parse_invocation(byte[] content, UIntPtr content_len, IntPtr obj, ref CallbackFunctions cb);

        public interface IParseResult {
            void begin_invocation(string name, int argc);
            void end_invocation();
            void boolean(bool value);
            void number(Number value);
            void identifier(string name);
            void string_literal(string value);
            void err(string msg);
        }

        [AOT.MonoPInvokeCallback(typeof(BeginInvocationFunc))]
        private static void r_func(IntPtr obj, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]byte[] name, UIntPtr name_len, UIntPtr argc) {
            var r = GCHandle.FromIntPtr(obj).Target as IParseResult;
            r.begin_invocation(System.Text.Encoding.UTF8.GetString(name), (int)argc.ToUInt32());
        }

        [AOT.MonoPInvokeCallback(typeof(EndInvocationFunc))]
        private static void r_func_end(IntPtr obj) {
            var r = GCHandle.FromIntPtr(obj).Target as IParseResult;
            r.end_invocation();
        }

        [AOT.MonoPInvokeCallback(typeof(IntFunc))]
        private static void r_bool(IntPtr obj, int value) {
            var r = GCHandle.FromIntPtr(obj).Target as IParseResult;
            r.boolean(value != 0 ? true : false);
        }

        [AOT.MonoPInvokeCallback(typeof(IntFunc))]
        private static void r_int(IntPtr obj, int value) {
            var r = GCHandle.FromIntPtr(obj).Target as IParseResult;
            r.number(new NumberInt { value = value });
        }

        [AOT.MonoPInvokeCallback(typeof(LongFunc))]
        private static void r_long(IntPtr obj, long value) {
            var r = GCHandle.FromIntPtr(obj).Target as IParseResult;
            r.number(new NumberLong { value = value });
        }

        [AOT.MonoPInvokeCallback(typeof(FloatFunc))]
        private static void r_float(IntPtr obj, float value) {
            var r = GCHandle.FromIntPtr(obj).Target as IParseResult;
            r.number(new NumberFloat { value = value });
        }

        [AOT.MonoPInvokeCallback(typeof(DoubleFunc))]
        private static void r_double(IntPtr obj, double value) {
            var r = GCHandle.FromIntPtr(obj).Target as IParseResult;
            r.number(new NumberDouble { value = value });
        }

        [AOT.MonoPInvokeCallback(typeof(StringFunc))]
        private static void r_identifier(IntPtr obj, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]byte[] buf, UIntPtr buf_len) {
            var r = GCHandle.FromIntPtr(obj).Target as IParseResult;
            r.identifier(System.Text.Encoding.UTF8.GetString(buf));
        }

        [AOT.MonoPInvokeCallback(typeof(StringFunc))]
        private static void r_string_literal(IntPtr obj, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] buf, UIntPtr buf_len) {
            var r = GCHandle.FromIntPtr(obj).Target as IParseResult;
            r.string_literal(System.Text.Encoding.UTF8.GetString(buf));
        }

        [AOT.MonoPInvokeCallback(typeof(StringFunc))]
        private static void r_err(IntPtr obj, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]byte[] buf, UIntPtr buf_len) {
            var r = GCHandle.FromIntPtr(obj).Target as IParseResult;
            r.err(System.Text.Encoding.UTF8.GetString(buf));
        }  

        private static CallbackFunctions r_ft = new CallbackFunctions {
            begin_invocation = r_func,
            end_invocation = r_func_end,
            boolean = r_bool,
            int32 = r_int,
            int64 = r_long,
            float32 = r_float,
            float64 = r_double,
            identifier = r_identifier,
            string_literal = r_string_literal,
            errmsg = r_err,
        };

        public static void parse(byte[] content, int length, IParseResult result) {
            var handle = GCHandle.Alloc(result);
            parse_invocation(content, new UIntPtr((uint)length), GCHandle.ToIntPtr(handle), ref r_ft);
            handle.Free();
        }
    }

}