using System;
using System.Collections.Generic;

namespace AptlyParser.Tests
{
    public class VersionComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (string.Equals(x, y, StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }

            var xParts = x.Split('.', '-', '~');
            var yParts = y.Split('.', '-', '~');

            var minLength = Math.Min(xParts.Length, yParts.Length);

            for (var i = 0; i < minLength; i++)
            {
                int result;

                int a;
                var b = 0;

                var success = int.TryParse(xParts[i], out a) && int.TryParse(yParts[i], out b);
                if (!success)
                {
                    result = string.Compare(yParts[i], xParts[i], StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    result = a.CompareTo(b);
                }

                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }
    }
}