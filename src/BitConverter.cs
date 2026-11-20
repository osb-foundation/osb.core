namespace System;

/// <summary>
/// Helper for converting primitive types to/from byte arrays.
/// Freestanding stub — only the methods used by Value.cs are implemented.
/// </summary>
public static class BitConverter
{
    public static unsafe ulong DoubleToUInt64Bits(double value)
    {
        ulong result;
        Buffer.Memcpy((byte*)&result, (byte*)&value, 8);
        return result;
    }

    public static unsafe double UInt64BitsToDouble(ulong value)
    {
        double result;
        Buffer.Memcpy((byte*)&result, (byte*)&value, 8);
        return result;
    }
}
