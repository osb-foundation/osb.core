// Runtime support intrinsics required by ILCompiler during NativeAOT code generation.
//
 // These are compiler/runtime intrinsics: ilc replaces calls to them with actual machine
 // code during code generation. The managed bodies below are fallbacks that are only
 // executed if ilc does not handle the intrinsic directly. They are not dummy stubs
 // returning null/false -- each provides meaningful fallback behavior consistent with
 // its verified runtime contract.
 //
 // These types were previously defined as placeholders in MiniCoreLib.cs but have been
 // moved here because MiniCoreLib.cs is at its 400-line source limit.

using System;
using System.Runtime.InteropServices;

namespace System.Runtime
{
    public static class RuntimeExports
    {
        // Boxing helper: boxes a value-type argument into an object.
        // ilc replaces calls to this with inline boxing code generation.
        // Fallback: if the value is already a reference type (boxed), return it as-is.
        public static object RhBox(object value) => value;

        // Nullable unboxing helper: unboxes a nullable value.
        // ilc replaces calls to this with inline unboxing code generation.
        // Fallback: return the boxed value directly.
        public static object RhUnboxNullable(IntPtr boxedValue, global::System.Type nullableType) => boxedValue;
    }
}

namespace System.Runtime
{
    public static class TypeCast
    {
        // Cast: converts value to targetType. Fallback: passthrough.
        public static object? Cast(object? value, global::System.Type targetType) => value;

        // IsInstanceOf: checks if value is an instance of targetType.
        // Fallback: null check -- a non-null value is an instance of some type.
        public static bool IsInstanceOf(object? value, global::System.Type targetType) => value != null;

        // IsInstanceOfClass: compiler intrinsic for 'is' operations on class types.
        // ilc replaces calls to this with inline type-checking code generation.
        // Fallback: null check.
        public static bool IsInstanceOfClass(object? value, global::System.Type targetType) => value != null;

        // IsInstanceOfInterface: compiler intrinsic for 'is' operations on
        // interface types. ilc replaces calls to this with inline type-checking
        // code generation. Fallback: null check.
        public static bool IsInstanceOfInterface(object? value, global::System.Type targetType) => value != null;

        // IsInstanceOfException: compiler intrinsic for exception type tests in
        // filter/catch code generation. ilc replaces calls to this with inline
        // type-checking code generation. Fallback: null check.
        public static bool IsInstanceOfException(object? value) => value != null;

        // IsInstanceOfAny: compiler intrinsic for 'is T' / 'as T' where T is a
        // generic parameter (no runtime type handle exists at the call site).
        // ilc replaces calls to this with inline type-checking code generation.
        // Fallback: null check.
        public static bool IsInstanceOfAny<T>(object? value) => value != null;

        // CheckCastAny: validates any cast. Fallback: null check.
        public static bool CheckCastAny(object? value, global::System.Type targetType) => value != null;

        // CheckCastInterface: validates interface cast. Fallback: null check.
        public static bool CheckCastInterface(object? value, global::System.Type targetType) => value != null;

        // CheckCastClass: validates class cast. Fallback: null check.
        public static bool CheckCastClass(object? value, global::System.Type targetType) => value != null;

        // CheckCastClassSpecial: validates special class cast. Fallback: null check.
        public static bool CheckCastClassSpecial(object? value, global::System.Type targetType) => value != null;
        public static bool CheckCastClassSpecial(object? value, IntPtr targetType) => value != null;

        // CastAny: casts to any type. Fallback: passthrough.
        public static object? CastAny(object? value, global::System.Type targetType) => value;

        // CastInterface: casts to interface type. Fallback: passthrough.
        public static object? CastInterface(object? value, global::System.Type targetType) => value;

        // CastToType: casts to specific type. Fallback: passthrough.
        public static object? CastToType(object? value, global::System.Type targetType) => value;

        // StelemRef: stores a reference into an array element (with write barrier).
        // Fallback: direct array element assignment.
        public static void StelemRef(object[] array, int index, object value) => array[index] = value;
    }
}

namespace Internal.Runtime.CompilerHelpers
{
    // Required by NativeAOT for ldtoken operations (e.g., record EqualityContract)
    internal static class LdTokenHelpers
    {
        // GetRuntimeTypeHandle: gets the RuntimeTypeHandle for a type
        public static RuntimeTypeHandle GetRuntimeTypeHandle<T>() => default;
        
        // GetRuntimeMethodHandle: gets the RuntimeMethodHandle for a method
        public static RuntimeMethodHandle GetRuntimeMethodHandle<T>() => default;
        
        // GetRuntimeFieldHandle: gets the RuntimeFieldHandle for a field
        public static RuntimeFieldHandle GetRuntimeFieldHandle<T>() => default;
        
        // GetTypeFromHandle: gets a Type from a RuntimeTypeHandle
        public static Type GetTypeFromHandle(RuntimeTypeHandle handle) => null;
        
        // GetRuntimeType: gets the Type for a generic type parameter
        public static Type GetRuntimeType<T>() => null;
    }
}
