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
        protected object? _target;
        protected IntPtr _methodPtr;
        protected IntPtr _methodPtrAux;
        protected IntPtr _extraFunctionPointerOrData;

        public static Delegate InitializeClosedInstance(Delegate d, object? target, IntPtr methodPtr)
        {
            d._target = target;
            d._methodPtr = methodPtr;
            return d;
        }

        public static Delegate InitializeOpenStaticThunk(Delegate d, IntPtr methodPtr)
        {
            d._target = null;
            d._methodPtr = methodPtr;
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