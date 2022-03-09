namespace Math.Computer
{
    /// <summary>
    /// Computes prime status of a natural number.
    /// </summary>
    public class PrimeComputer
    {
        public readonly PrimeRegistry Registry;

        /// <summary>
        /// Accepts 1 as prime value.
        /// </summary>
        public PrimeComputer(PrimeRegistry registry)
        {
            Registry = registry;
            Registry.Add(1);
        }

        /// <summary>
        /// Checks whether value is prime.
        /// </summary>
        /// <param name="value">Number to test.</param>
        /// <returns>True if value is prime.</returns>
        public bool IsPrime(long value)
        {
            if (value <= Registry.KnownBoundary && Registry.Contains(value))
                return true;

            if (value <= Registry.KnownBoundary)
                return false;

            var upper = (long)System.Math.Sqrt(value);

            bool result = true;

            for (long factor = 2; factor <= upper; factor++)
            {
                if (IsPrime(factor))
                {
                    var mod = value % factor;

                    if (0 == mod)
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Splits a natural number to factors.
        /// </summary>
        /// <param name="value">Number to test.</param>
        /// <returns>Factors.</returns>
        public long[] Factorize(long value)
        {
            if (IsPrime(value))
                return Array.Empty<long>();

            long reminder = value;
            var factors = new List<long>();

            var upper = (long)System.Math.Sqrt(value);

            for (long factor = 2; factor <= upper; factor++)
            {
                if (IsPrime(factor))
                {
                    long mod = 0;

                    while (0 == mod)
                    {
                        mod = reminder % factor;
                        if (0 == mod)
                        {
                            reminder /= factor;
                            factors.Add(factor);

                            if (IsPrime(reminder))
                            {
                                if(reminder != 1)
                                    factors.Add(reminder);

                                break;
                            }
                        }
                    }
                }
            }

            return factors.ToArray();
        }
    }
}
