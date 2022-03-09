using Kw.Common;

namespace Math.Computer
{
    public class PrimeRegistry : IDisposable
    {
        RegistryHeader _header;

        public const string DEFAULT_PATH = @"C:\math\data\registry.data";
        public const long LOW32 = (uint.MaxValue + 1L);

        public static string DefaultPath => new AppSetting("registryPath", DEFAULT_PATH).Value;

        public long Length => _header.Length;

        readonly MappedArray _buffer;
        readonly BitArray? _bits;

        public long KnownBoundary
        {
            get => _header.KnownBoundary;
            set
            {
                if (value != _header.KnownBoundary)
                {
                    _header.KnownBoundary = value;
                    _buffer.Write(0, _header);
                }
            }
        }

        public long KnownCount
        {
            get => _header.KnownCount;
            set
            {
                if (value != _header.KnownCount)
                {
                    _header.KnownCount = value;
                    _buffer.Write(0, _header);
                }
            }
        }

        public string Path { get; }

        public PrimeRegistry(long length) : this(DefaultPath, length) { }

        public unsafe PrimeRegistry(string? path, long length)
        {
            path ??= DefaultPath;

            Path = path;
            
            if (0 != length % 8)
                throw new ArgumentException("Length must be a whole number divisible by 8.", nameof(length));

            var bitSize = length / 8;
            var hdrSize = sizeof(RegistryHeader);

            var fsize = bitSize + hdrSize;

            if (QueryFile(path, out long s))
            {
                if(s != fsize)
                    throw new InvalidOperationException(
                        $"Size of the file '{path}' doesn't match the parameter '{nameof(fsize)}'.");

                _buffer = new MappedArray(path);

                _buffer.Read(0, out _header);

                if(_header.Length != length)
                    throw new InvalidOperationException($"The file '{path}' appears to be corrupt.");

                _header.Validate(path);
            }
            else
            {
                _header = new RegistryHeader(length);

                _buffer = new MappedArray(fsize, path);
                _buffer.Write(0, _header);
            }

            Console.WriteLine($"Registry at '{Path}':");

            Console.WriteLine($"Total records: {Length}");
            Console.WriteLine($"Known boundary: {KnownBoundary}");
            Console.WriteLine($"Known count: {KnownCount}");

            _bits = new BitArray(_buffer, hdrSize);

        }

        //public void

        public bool Add(long value)
        {
            if (Contains(value))
                return false;

            _bits!.Write(value - 1, true);
            KnownCount ++;

            return true;
        }

        public bool Contains(long value) => _bits!.Read(value - 1);

        public void Dispose()
        {
            if(null != _buffer)
                _buffer.Dispose();
        }

        bool QueryFile(string path, out long size)
        {
            size = 0;

            if (File.Exists(path))
            {
                size = new FileInfo(path).Length;
                return true;
            }

            return false;
        }

        public long BackPrime(long x, long upper)
        {
            for (long i = x; i > 1; i--)
            {
                if(i == upper)
                    continue;
                
                if (Contains(i))
                    return i;
            }

            return 1L;
        }

        public long NextPrime(long x)
        {
            for (long i = x + 1; i <= KnownBoundary; i++)
            {
                if (Contains(i))
                    return i;
            }

            return 0L;
        }

        public void Sync() => _buffer.Flush();
    }
}
