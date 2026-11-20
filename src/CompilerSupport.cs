// Compiler-support types required by C# compiler (csc) for freestanding compilation.
// These types are not used at runtime but must exist for compilation to succeed.

using System;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public sealed class DefaultMemberAttribute : Attribute
    {
        public DefaultMemberAttribute(string memberName) { }
    }
}

namespace System
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ParamArrayAttribute : Attribute { }
}

namespace System.Runtime.CompilerServices
{
    // Required for record init-only properties (AstNodes.cs, Token.cs)
    internal static class IsExternalInit { }

    // Required for named tuples (BigIntegerDivision, BigIntegerArithmetic, Value)
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    internal sealed class TupleElementNamesAttribute : Attribute
    {
        public TupleElementNamesAttribute(string[] transformNames) { }
    }
}

// Namespaces referenced by C# implicit usings but not used by Engine.Core.
// Declared empty so `using` directives resolve under NoStdLib compilation.
namespace System.Linq { }
namespace System.Threading.Tasks { }
namespace System.IO { }
namespace System.Net { }
namespace System.Net.Http { }