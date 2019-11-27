using System;
using System.Collections.Generic;
using System.Text;

namespace iSCSI.net
{
    public static class Validation
    {
        public static void ThrowIfRangeViolation<T>(this T value, T minimum, T maximum) where T : IComparable<T>
        {
            if (value.CompareTo(minimum) < 0 || value.CompareTo(maximum) > 0)
                throw new ArgumentOutOfRangeException();
        }

        public static void ThrowIfRangeViolation<T>(this T value, T minimum, T maximum, string paramName) where T : IComparable<T>
        {
            if (value.CompareTo(minimum) < 0 || value.CompareTo(maximum) > 0)
                throw new ArgumentOutOfRangeException(paramName);
        }

        public static void ThrowIfRangeViolation<T>(this T value, T minimum, T maximum, string paramName, string message) where T : IComparable<T>
        {
            if (value.CompareTo(minimum) < 0 || value.CompareTo(maximum) > 0)
                throw new ArgumentOutOfRangeException(paramName, message);
        }
    }
}
