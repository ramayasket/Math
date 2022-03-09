namespace Math.Computer
{
    public class BitArray
    {
        readonly MappedArray _memory;
        readonly long _offset;

        readonly byte[] _cache;

        const long LOW32 = uint.MaxValue + 1L;
        const long CACHE = LOW32 / 8;

        /// <summary>
        /// Size in bits.
        /// </summary>
        public long Size => RegionSize * 8;

        public long RegionSize { get; }

        public static byte GetMask(long offset)
        {
            byte x = 1;

            for(long i=0; i<offset; i++)
                x <<= 1;

            return x;
        }

        public BitArray(MappedArray memory, long offset)
        {
            _memory = memory;
            _offset = offset;

            RegionSize = (memory.Size - offset);

            //var haveLow32 = KnownBoundary >= LOW32;

            _cache = new byte[CACHE];

            Console.Write("Caching low 2^32... ");

            for (long i = 0; i < CACHE; i++)
                _memory.Read(i + _offset, out _cache[i]);

            Console.WriteLine("done");
        }

        public byte ReadRegion(long at)
        {
            var address = at / 8;
            var offset = at % 8;

            byte data;

            if (null != _cache && address < CACHE)
                data = _cache[address];

            else
                _memory.Read(address + _offset, out data);

            return data;
        }


        public bool Read(long at)
        {
            var data = ReadRegion(at);

            var offset = at % 8;

            var @byte = data & GetMask(offset);

            return 0 != @byte;
        }

        public void Write(long at, bool value)
        {
            var address = at / 8;
            var offset = at % 8;

            if (null != _cache && address < CACHE)
            {
                
            }

            var data = ReadRegion(at);
            
            var mask = GetMask(offset);

            if (value)
                data |= mask;

            else
                data &= (byte)~mask;

            _memory.Write(address + _offset, data);
        }
    }
}
