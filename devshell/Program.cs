using System.Diagnostics;
using System.Runtime.InteropServices;
using Kw.Common;
using Math.Computer;

namespace Math
{
    // ReSharper disable AccessToDisposedClosure

    internal class Program
    {
        static bool _quit;

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler? _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        static void Read1Billion(PrimeRegistry registry)
        {
            using (var s = File.OpenText(@"C:\mixed\code\bin\primes.txt"))
            {
                string? line;

                long count = 0;
                long x = 0;

                while (null != (line = s.ReadLine()))
                {
                    x = long.Parse(line);

                    registry.Add(x);

                    count++;
                }

                registry.KnownBoundary = x;
                registry.KnownCount = count;
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "Computing new data";

            var g20 = 20000000000L;

            const long MYSIZE =
                    (uint.MaxValue + 1L)
                    * 32
                ;

            _handler += sig => { _quit = true; return true; };

            SetConsoleCtrlHandler(_handler, true);
            
            var v = new AppSetting("registryPath").Value;

            using (var registry = new PrimeRegistry(v, MYSIZE))
            {
                //var backPrime = registry.BackPrime(registry.KnownBoundary, registry.KnownBoundary);
                //var nextPrime = registry.NextPrime(registry.KnownBoundary);
                //long count = 0;
                //for (long x = 1; x <= registry.KnownBoundary; x++)
                //{
                //    if(registry.Contains(x))
                //        count ++;
                //}
                //// temp
                //registry.KnownBoundary = 12315483841L;
                //registry.KnownCount = 555142429L;

                //if(0 == registry.KnownBoundary)
                //    Read1Billion(registry);

                //var max = 12318900000L;

                //for (long x = registry.KnownBoundary + 1; x <= max; x++)
                //{
                //    registry.Bits!.Write(x, false);
                //}

                var computer = new Computer.PrimeComputer(registry);

                //computer.IsPrime(12315483877L);

                var sw = Stopwatch.StartNew();

                for (long x = registry.KnownBoundary + 1; x <= g20; x ++)
                {
                    if (_quit)
                        break;

                    if (computer.IsPrime(x))
                        registry.Add(x);

                    registry.KnownBoundary = x;

                    if (0 == x % 100000)
                    {
                        sw.Stop();
                        Console.WriteLine($"{x}: 100K in {sw.ElapsedMilliseconds} ms");

                        registry.Sync();

                        sw.Reset();
                        sw.Start();
                    }
                }

                Console.WriteLine("Exit loop");
            }

            Console.WriteLine("Registry closed");
        }
    }
}
