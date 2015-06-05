using System;
using System.Runtime.InteropServices;
using System.Security;

namespace OmnivoreApi.Utils
{
    public static class Extensions
    {
        public static SecureString AppendText(this SecureString target, string input, bool makeReadOnly = true)
        {
            if (!string.IsNullOrEmpty(input))
            {
                foreach (var c in input) target.AppendChar(c);
            }
            if (makeReadOnly) target.MakeReadOnly();
            return target;
        }

        public static string ToPlainText(this SecureString src)
        {
            var ptr = Marshal.SecureStringToBSTR(src);
            var output = Marshal.PtrToStringBSTR(ptr);
            Marshal.ZeroFreeBSTR(ptr);
            return output;
        }

        public static DateTime ToDateTime(this long seconds)
            => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds);
    }
}