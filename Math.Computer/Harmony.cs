using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math.Computer
{
    public static class Harmony
    {
        public static float Value(long x, long y) => System.Math.Abs(1f * x * y / ((x + y) * (x - y)));

        public static float Value(long[] xn)
        {
            var values = new List<float>();

            for (int m = 0; m < xn.Length; m++)
                for (int n = m + 1; n < xn.Length; n++)
                    values.Add(Value(xn[m], xn[n]));

            return values.Average();
        }
    }
}
