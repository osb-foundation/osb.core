// ValueTuple types for freestanding NativeAOT compatibility.
// Used in ITimeProvider return types (DaysToDate, MillisToTime),
// BigIntegerDivision, BigIntegerArithmetic, Value.

using System.Runtime.InteropServices;

namespace System
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ValueTuple<T1, T2> : IEquatable<ValueTuple<T1, T2>>
    {
        public readonly T1 Item1;
        public readonly T2 Item2;
        public ValueTuple(T1 item1, T2 item2) { Item1 = item1; Item2 = item2; }
        public bool Equals(ValueTuple<T1, T2> other)
        {
            if (Item1 == null && other.Item1 == null) return Item2 == null ? other.Item2 == null : Item2.Equals(other.Item2);
            if (Item1 == null) return false;
            if (!Item1.Equals(other.Item1)) return false;
            if (Item2 == null && other.Item2 == null) return true;
            if (Item2 == null) return false;
            return Item2.Equals(other.Item2);
        }
        public override bool Equals(object obj) => obj is ValueTuple<T1, T2> t && Equals(t);
        public override int GetHashCode()
        {
            int h = Item1 != null ? Item1.GetHashCode() : 0;
            h = h * 31 + (Item2 != null ? Item2.GetHashCode() : 0);
            return h;
        }
        public override string ToString()
        {
            string s1;
            if (Item1 == null) s1 = "null";
            else
            {
                string t = Item1.ToString();
                if (t == null) s1 = "null";
                else s1 = t;
            }
            string s2;
            if (Item2 == null) s2 = "null";
            else
            {
                string t = Item2.ToString();
                if (t == null) s2 = "null";
                else s2 = t;
            }
            return "(" + s1 + ", " + s2 + ")";
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ValueTuple<T1, T2, T3> : IEquatable<ValueTuple<T1, T2, T3>>
    {
        public readonly T1 Item1;
        public readonly T2 Item2;
        public readonly T3 Item3;
        public ValueTuple(T1 item1, T2 item2, T3 item3) { Item1 = item1; Item2 = item2; Item3 = item3; }
        public bool Equals(ValueTuple<T1, T2, T3> other)
        {
            if (Item1 == null && other.Item1 == null) return Item2 == null ? other.Item2 == null : Item2.Equals(other.Item2);
            if (Item1 == null) return false;
            if (!Item1.Equals(other.Item1)) return false;
            if (Item2 == null && other.Item2 == null) return Item3 == null ? other.Item3 == null : Item3.Equals(other.Item3);
            if (Item2 == null) return false;
            if (!Item2.Equals(other.Item2)) return false;
            if (Item3 == null && other.Item3 == null) return true;
            if (Item3 == null) return false;
            return Item3.Equals(other.Item3);
        }
        public override bool Equals(object obj) => obj is ValueTuple<T1, T2, T3> t && Equals(t);
        public override int GetHashCode()
        {
            int h = Item1 != null ? Item1.GetHashCode() : 0;
            h = h * 31 + (Item2 != null ? Item2.GetHashCode() : 0);
            h = h * 31 + (Item3 != null ? Item3.GetHashCode() : 0);
            return h;
        }
        public override string ToString()
        {
            string s1;
            if (Item1 == null) s1 = "null";
            else
            {
                string t = Item1.ToString();
                if (t == null) s1 = "null";
                else s1 = t;
            }
            string s2;
            if (Item2 == null) s2 = "null";
            else
            {
                string t = Item2.ToString();
                if (t == null) s2 = "null";
                else s2 = t;
            }
            string s3;
            if (Item3 == null) s3 = "null";
            else
            {
                string t = Item3.ToString();
                if (t == null) s3 = "null";
                else s3 = t;
            }
            return "(" + s1 + ", " + s2 + ", " + s3 + ")";
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ValueTuple<T1, T2, T3, T4> : IEquatable<ValueTuple<T1, T2, T3, T4>>
    {
        public readonly T1 Item1;
        public readonly T2 Item2;
        public readonly T3 Item3;
        public readonly T4 Item4;
        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4) { Item1 = item1; Item2 = item2; Item3 = item3; Item4 = item4; }
        public bool Equals(ValueTuple<T1, T2, T3, T4> other)
        {
            if (Item1 == null && other.Item1 == null) return Item2 == null ? other.Item2 == null : Item2.Equals(other.Item2);
            if (Item1 == null) return false;
            if (!Item1.Equals(other.Item1)) return false;
            if (Item2 == null && other.Item2 == null) return Item3 == null ? other.Item3 == null : Item3.Equals(other.Item3);
            if (Item2 == null) return false;
            if (!Item2.Equals(other.Item2)) return false;
            if (Item3 == null && other.Item3 == null) return Item4 == null ? other.Item4 == null : Item4.Equals(other.Item4);
            if (Item3 == null) return false;
            if (!Item3.Equals(other.Item3)) return false;
            if (Item4 == null && other.Item4 == null) return true;
            if (Item4 == null) return false;
            return Item4.Equals(other.Item4);
        }
        public override bool Equals(object obj) => obj is ValueTuple<T1, T2, T3, T4> t && Equals(t);
        public override int GetHashCode()
        {
            int h = Item1 != null ? Item1.GetHashCode() : 0;
            h = h * 31 + (Item2 != null ? Item2.GetHashCode() : 0);
            h = h * 31 + (Item3 != null ? Item3.GetHashCode() : 0);
            h = h * 31 + (Item4 != null ? Item4.GetHashCode() : 0);
            return h;
        }
        public override string ToString()
        {
            string s1;
            if (Item1 == null) s1 = "null";
            else
            {
                string t = Item1.ToString();
                if (t == null) s1 = "null";
                else s1 = t;
            }
            string s2;
            if (Item2 == null) s2 = "null";
            else
            {
                string t = Item2.ToString();
                if (t == null) s2 = "null";
                else s2 = t;
            }
            string s3;
            if (Item3 == null) s3 = "null";
            else
            {
                string t = Item3.ToString();
                if (t == null) s3 = "null";
                else s3 = t;
            }
            string s4;
            if (Item4 == null) s4 = "null";
            else
            {
                string t = Item4.ToString();
                if (t == null) s4 = "null";
                else s4 = t;
            }
            return "(" + s1 + ", " + s2 + ", " + s3 + ", " + s4 + ")";
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ValueTuple<T1, T2, T3, T4, T5> : IEquatable<ValueTuple<T1, T2, T3, T4, T5>>
    {
        public readonly T1 Item1;
        public readonly T2 Item2;
        public readonly T3 Item3;
        public readonly T4 Item4;
        public readonly T5 Item5;
        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) { Item1 = item1; Item2 = item2; Item3 = item3; Item4 = item4; Item5 = item5; }
        public bool Equals(ValueTuple<T1, T2, T3, T4, T5> other)
        {
            if (Item1 == null && other.Item1 == null) return Item2 == null ? other.Item2 == null : Item2.Equals(other.Item2);
            if (Item1 == null) return false;
            if (!Item1.Equals(other.Item1)) return false;
            if (Item2 == null && other.Item2 == null) return Item3 == null ? other.Item3 == null : Item3.Equals(other.Item3);
            if (Item2 == null) return false;
            if (!Item2.Equals(other.Item2)) return false;
            if (Item3 == null && other.Item3 == null) return Item4 == null ? other.Item4 == null : Item4.Equals(other.Item4);
            if (Item3 == null) return false;
            if (!Item3.Equals(other.Item3)) return false;
            if (Item4 == null && other.Item4 == null) return Item5 == null ? other.Item5 == null : Item5.Equals(other.Item5);
            if (Item4 == null) return false;
            if (!Item4.Equals(other.Item4)) return false;
            if (Item5 == null && other.Item5 == null) return true;
            if (Item5 == null) return false;
            return Item5.Equals(other.Item5);
        }
        public override bool Equals(object obj) => obj is ValueTuple<T1, T2, T3, T4, T5> t && Equals(t);
        public override int GetHashCode()
        {
            int h = Item1 != null ? Item1.GetHashCode() : 0;
            h = h * 31 + (Item2 != null ? Item2.GetHashCode() : 0);
            h = h * 31 + (Item3 != null ? Item3.GetHashCode() : 0);
            h = h * 31 + (Item4 != null ? Item4.GetHashCode() : 0);
            h = h * 31 + (Item5 != null ? Item5.GetHashCode() : 0);
            return h;
        }
        public override string ToString()
        {
            string s1;
            if (Item1 == null) s1 = "null";
            else
            {
                string t = Item1.ToString();
                if (t == null) s1 = "null";
                else s1 = t;
            }
            string s2;
            if (Item2 == null) s2 = "null";
            else
            {
                string t = Item2.ToString();
                if (t == null) s2 = "null";
                else s2 = t;
            }
            string s3;
            if (Item3 == null) s3 = "null";
            else
            {
                string t = Item3.ToString();
                if (t == null) s3 = "null";
                else s3 = t;
            }
            string s4;
            if (Item4 == null) s4 = "null";
            else
            {
                string t = Item4.ToString();
                if (t == null) s4 = "null";
                else s4 = t;
            }
            string s5;
            if (Item5 == null) s5 = "null";
            else
            {
                string t = Item5.ToString();
                if (t == null) s5 = "null";
                else s5 = t;
            }
            return "(" + s1 + ", " + s2 + ", " + s3 + ", " + s4 + ", " + s5 + ")";
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ValueTuple<T1, T2, T3, T4, T5, T6> : IEquatable<ValueTuple<T1, T2, T3, T4, T5, T6>>
    {
        public readonly T1 Item1;
        public readonly T2 Item2;
        public readonly T3 Item3;
        public readonly T4 Item4;
        public readonly T5 Item5;
        public readonly T6 Item6;
        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) { Item1 = item1; Item2 = item2; Item3 = item3; Item4 = item4; Item5 = item5; Item6 = item6; }
        public bool Equals(ValueTuple<T1, T2, T3, T4, T5, T6> other)
        {
            if (Item1 == null && other.Item1 == null) return Item2 == null ? other.Item2 == null : Item2.Equals(other.Item2);
            if (Item1 == null) return false;
            if (!Item1.Equals(other.Item1)) return false;
            if (Item2 == null && other.Item2 == null) return Item3 == null ? other.Item3 == null : Item3.Equals(other.Item3);
            if (Item2 == null) return false;
            if (!Item2.Equals(other.Item2)) return false;
            if (Item3 == null && other.Item3 == null) return Item4 == null ? other.Item4 == null : Item4.Equals(other.Item4);
            if (Item3 == null) return false;
            if (!Item3.Equals(other.Item3)) return false;
            if (Item4 == null && other.Item4 == null) return Item5 == null ? other.Item5 == null : Item5.Equals(other.Item5);
            if (Item4 == null) return false;
            if (!Item4.Equals(other.Item4)) return false;
            if (Item5 == null && other.Item5 == null) return Item6 == null ? other.Item6 == null : Item6.Equals(other.Item6);
            if (Item5 == null) return false;
            if (!Item5.Equals(other.Item5)) return false;
            if (Item6 == null && other.Item6 == null) return true;
            if (Item6 == null) return false;
            return Item6.Equals(other.Item6);
        }
        public override bool Equals(object obj) => obj is ValueTuple<T1, T2, T3, T4, T5, T6> t && Equals(t);
        public override int GetHashCode()
        {
            int h = Item1 != null ? Item1.GetHashCode() : 0;
            h = h * 31 + (Item2 != null ? Item2.GetHashCode() : 0);
            h = h * 31 + (Item3 != null ? Item3.GetHashCode() : 0);
            h = h * 31 + (Item4 != null ? Item4.GetHashCode() : 0);
            h = h * 31 + (Item5 != null ? Item5.GetHashCode() : 0);
            h = h * 31 + (Item6 != null ? Item6.GetHashCode() : 0);
            return h;
        }
        public override string ToString()
        {
            string s1;
            if (Item1 == null) s1 = "null";
            else
            {
                string t = Item1.ToString();
                if (t == null) s1 = "null";
                else s1 = t;
            }
            string s2;
            if (Item2 == null) s2 = "null";
            else
            {
                string t = Item2.ToString();
                if (t == null) s2 = "null";
                else s2 = t;
            }
            string s3;
            if (Item3 == null) s3 = "null";
            else
            {
                string t = Item3.ToString();
                if (t == null) s3 = "null";
                else s3 = t;
            }
            string s4;
            if (Item4 == null) s4 = "null";
            else
            {
                string t = Item4.ToString();
                if (t == null) s4 = "null";
                else s4 = t;
            }
            string s5;
            if (Item5 == null) s5 = "null";
            else
            {
                string t = Item5.ToString();
                if (t == null) s5 = "null";
                else s5 = t;
            }
            string s6;
            if (Item6 == null) s6 = "null";
            else
            {
                string t = Item6.ToString();
                if (t == null) s6 = "null";
                else s6 = t;
            }
            return "(" + s1 + ", " + s2 + ", " + s3 + ", " + s4 + ", " + s5 + ", " + s6 + ")";
        }
    }
}