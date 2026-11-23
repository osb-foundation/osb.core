// Exception hierarchy for freestanding NativeAOT compatibility.
// Used by StringMap, FrameStack, StringList, UsingList, IntList, EngineHeap.

namespace System
{
    public class Exception
    {
        public string Message { get; }
        public Exception() { }
        public Exception(string message) { Message = message; }
        public override string ToString()
        {
            if (Message == null) return "";
            return Message;
        }
    }

    public class ArgumentException : Exception
    {
        public ArgumentException() { }
        public ArgumentException(string message) : base(message) { }
        public ArgumentException(string message, string paramName) : base(message + " (Parameter '" + paramName + "')") { }
    }

    public class ArgumentNullException : ArgumentException
    {
        public ArgumentNullException(string paramName) : base(paramName) { }
    }

    public class ArgumentOutOfRangeException : ArgumentException
    {
        public ArgumentOutOfRangeException() { }
        public ArgumentOutOfRangeException(string paramName) : base(paramName) { }
        public ArgumentOutOfRangeException(string message, string paramName) : base(message + " (Parameter '" + paramName + "')") { }
    }

    public class InvalidOperationException : Exception
    {
        public InvalidOperationException() { }
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

    public class OverflowException : Exception
    {
        public OverflowException() : base("Arithmetic operation resulted in an overflow.") { }
        public OverflowException(string message) : base(message) { }
    }

    public class FormatException : Exception
    {
        public FormatException() { }
        public FormatException(string message) : base(message) { }
    }

    public class ObjectDisposedException : Exception
    {
        public ObjectDisposedException() { }
        public ObjectDisposedException(string objectName) : base("Cannot access a disposed object: " + objectName) { }
        public ObjectDisposedException(string objectName, string message) : base("Cannot access a disposed object: " + objectName + ". " + message) { }
    }
}