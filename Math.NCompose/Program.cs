using Kw.Common;
using Math.Computer;

namespace Math.NCompose
{
    public class Stats<T> : Dictionary<T, long> where T : notnull
    {
        public void Take(T x)
        {
            long count = 0;

            if (ContainsKey(x))
                count = this[x];

            count++;

            this[x] = count;
        }

        public void Report(TextWriter w)
        {
            foreach (var k in Keys.OrderBy(x => x))
            {
                w.WriteLine($"{k} - {this[k]}");
            }
        }
    }

    public class Output : IDisposable
    {
        readonly string? _path;

        readonly Stream? _stream;
        public readonly TextWriter Writer;
        
        public Output(string? path)
        {
            _path = path;
            if (null != path)
            {
                _stream = File.OpenWrite(path);
                Writer = new StreamWriter(_stream);
                _path = path;
            }
            else
                Writer = Console.Out;
        }

        public void Dispose()
        {
            if (null != _path)
            {
                //_stream?.Dispose();
                Writer.Dispose();
            }
        }
    }

    public interface IComposition
    {
        long Sum { get; }
        int Length { get; }

        long[] Layout { get; }
    }

    public struct Layout3 : IComparable<Layout3>, IComposition
    {
        public long Sum => i1 + i2;
        public int Length => 2;

        public long[] Layout => new[] { i1, i2 };

        public int CompareTo(Layout3 other) => Sum.CompareTo(other.Sum);

        public override string ToString() => $"{Sum}: {i1}+{i2}";

        public long i1;
        public long i2;

        public Layout3(long[] tail)
        {
            i1 = tail[0];
            i2 = tail[1];
        }
    }

    public struct Layout4 : IComparable<Layout4>, IComposition
    {
        public long Sum => i1 + i2 + i3;
        public int Length => 3;

        public long[] Layout => new[] { i1, i2, i3 };

        public int CompareTo(Layout4 other) => Sum.CompareTo(other.Sum);

        public override string ToString() => $"{Sum}: {i1}+{i2}+{i3}";

        public long i1;
        public long i2;
        public long i3;

        public Layout4(long[] tail)
        {
            i1 = tail[0];
            i2 = tail[1];
            i3 = tail[2];
        }
    }

    internal class Program
    {
        class Holder
        {
            [ThreadStatic] public static string? Text;
        }

        static async Task Zlp()
        {
            Holder.Text = "Zlp";
            Console.WriteLine($"Part 1: thread {Thread.CurrentThread.ManagedThreadId}: '{Holder.Text}'");

            await Task.Delay(1000);

            Console.WriteLine($"Part 2: thread {Thread.CurrentThread.ManagedThreadId}: '{Holder.Text}'");
        }

        static async Task Main()
        {
            await Task.Run(Zlp);

            long x1 = 2035669;
            long x2 = 12;

            var h12 = Harmony.Value(x1, x2);
            var H12 = h12 * x1;

            const long MYSIZE =
                    (uint.MaxValue + 1L)
                    * 32
                ;

            using (var registry = new PrimeRegistry(MYSIZE))
            {
                var l3stats = new Stats<Layout3>();
                var l4stats = new Stats<Layout4>();

                //var lenstats = new Stats<int>();

                using (var o = new Output(@"C:\math\data\composition.txt"))
                {
                    for (long x = 11; x <= 3000000/*registry.KnownBoundary*/; x++)
                    {
                        if (0 == x % 100000000)
                            Console.WriteLine(x);

                        if (!registry.Contains(x))
                            continue;

                        var next = registry.NextPrime(x);

                        var n = new NaturalComposition(x, registry);

                        var l = n.Layout.Length;

                        var tail = n.Layout.Skip(1).ToArray();

                        o.Writer.WriteLine($"{x}:\t{n} - {next - x} - {Harmony.Value(n.Layout) * x}");

                        if (4 == l)
                        {
                            var l4 = new Layout4(tail);
                            l4stats.Take(l4);
                        }
                        else if (3 == l)
                        {
                            var l3 = new Layout3(tail);
                            l3stats.Take(l3);
                        }
                    }
                    
                    Console.WriteLine(registry.KnownBoundary);

                    o.Writer.WriteLine("===========");

                    o.Writer.WriteLine("3-layout statistics");
                    l3stats.Report(o.Writer);

                    o.Writer.WriteLine("4-layout statistics");
                    l4stats.Report(o.Writer);
                }
            }
        }
    }
}
