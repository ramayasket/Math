using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math.Computer
{
    public static class ArrayHelper
    {
        public static string Print(long[] data) => string.Join("+", data.Select(x => x.ToString()));
    }
}
