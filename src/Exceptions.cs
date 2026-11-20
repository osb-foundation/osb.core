// Exception hierarchy for freestanding NativeAOT compatibility.
// Used by StringMap, FrameStack, StringList, UsingList, IntList, EngineHeap.

namespace System
{
    public class Exception
    {
        public string Message { get; }
        public Exception() { }
        public Exception(string message) { Message = message; }
        public override string ToString() => Message ?? "";
    }

    public class ArgumentException : Exception
    {
        public ArgumentException(string message) : base(message) { }
    }

    public class ArgumentNullException : ArgumentException
    {
        public ArgumentNullException(string paramName) : base(paramName) { }
    }

    public class ArgumentOutOfRangeException : ArgumentException
    {
        public ArgumentOutOfRangeException(string paramName) : base(paramName) { }
    }

    public class InvalidOperationException : Exception
    {
        public InvalidOperationException(string message) : base(message) { }
    }

    public class ArithmeticException : Exception
    {
        public ArithmeticException() { }
        public ArithmeticException(string message) : base(message) { }
    }

    public class DivideByZeroException : ArithmeticException { public DivideByZeroException() : base("Division by zero") { } }

    public class KeyNotFoundException : Exception
    {
        public KeyNotFoundException() : base("Key not found") { }
    }

    public class OutOfMemoryException : Exception
    {
        public OutOfMemoryException(string message) : base(message) { }
    }

    public class IndexOutOfRangeException : Exception
    {
        public IndexOutOfRangeException() : base("Index out of range") { }
    }
}