// Delegate types for freestanding NativeAOT compatibility.
// Required by EngineHeap.Allocate<T>(Func<T>), MathNamespace, StandardLibrary.

namespace System
{
    public delegate TResult Func<TResult>();
    public delegate TResult Func<T, TResult>(T arg);
    public delegate TResult Func<T1, T2, TResult>(T1 arg1, T2 arg2);
    public delegate TResult Func<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3);
    public delegate TResult Func<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

    public delegate void Action();
    public delegate void Action<T>(T arg);
    public delegate void Action<T1, T2>(T1 arg1, T2 arg2);
    public delegate void Action<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);

    public abstract class Delegate
    {
        // Field layout MUST match the NativeAOT System.Delegate ABI. ilc generates
        // delegate invocation as `call [delegate + 0x20]` (the _functionPointer slot)
        // and passes the target via [delegate + 0x8] (_firstParameter). The managed
        // bodies below are the fallbacks ilc calls from the generated __DelegateCtor
        // (InitializeClosedInstance / InitializeOpenStaticThunk). Keeping the field
        // order identical to the runtime is required so the offsets line up.
        protected object? _firstParameter;            // 0x08: target (closed) / this (open static)
        protected object? _helperObject;              // 0x10: helper (generic / multicast); unused here
        protected IntPtr _extraFunctionPointerOrData; // 0x18: extra function pointer / invocation data
        protected IntPtr _functionPointer;            // 0x20: callable method pointer (read by the invoke thunk)

        public static Delegate InitializeClosedInstance(Delegate d, object? target, IntPtr methodPtr)
        {
            d._firstParameter = target;     // 0x08: the captured target object
            d._functionPointer = methodPtr; // 0x20: the managed method pointer to invoke
            return d;
        }

        public static Delegate InitializeOpenStaticThunk(Delegate d, IntPtr methodPtr)
        {
            d._firstParameter = d;          // 0x08: open static thunks receive the delegate itself
            d._functionPointer = methodPtr; // 0x20: the managed method pointer to invoke
            return d;
        }
    }

    public abstract class MulticastDelegate : Delegate
    {
        public static Delegate Combine(Delegate a, Delegate b)
        {
            if (a == null) return b;
            return a;
        }

        public static Delegate Remove(Delegate source, Delegate value) => source;
    }
}