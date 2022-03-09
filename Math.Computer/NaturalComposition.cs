using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math.Computer
{
    [DebuggerDisplay("{ToString()}")]
    public class NaturalComposition
    {
        public readonly long[] Layout;

        public override string ToString() => ArrayHelper.Print(Layout);

        public NaturalComposition(long value, PrimeRegistry registry)
        {
            var running = value;
            var layout = new List<long>();

            while (0 != running)
            {
                var x = registry.BackPrime(running, value);

                layout.Add(x);
                running -= x;
            }

            Layout = layout.ToArray();
        }
    }
}
