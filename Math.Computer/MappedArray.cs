using System.IO.MemoryMappedFiles;

namespace Math.Computer
{
    //#pragma warning disable CS8618

    public class MappedArray : IDisposable
    {
        const long CHUNK_SIZE = 1024 * 1024 * 256;

        MemoryMappedFile? _mappedFile;
        MemoryMappedViewAccessor? _accessor;
        
        public long Size { get; }

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

        /// <summary> Opens an existing mapped file. </summary>
        public MappedArray(string path, long size = 0)
        {
            if (QueryFile(path, out long s))
            {
                if (0 != size && s != size)
                    throw new InvalidOperationException(
                        $"Size of the file '{path}' doesn't match the parameter '{nameof(size)}'.");

                Size = s;
                
                CreateViewOfFile(path);
            }
            else
                throw new FileNotFoundException($"File '{path}' is not found.");
        }

        /// <summary> Creates a new mapped file of the given size. </summary>
        public MappedArray(long size, string path)
        {
            if (QueryFile(path, out long _))
                throw new InvalidOperationException($"File '{path}' already exists.");

            Size = size;

            CreateWafer(size, path);

            CreateViewOfFile(path);
        }

        /// <summary> Maps file into memory. </summary>
        void CreateViewOfFile(string path)
        {
            _mappedFile = MemoryMappedFile.CreateFromFile(path, FileMode.Open);

            _accessor = _mappedFile.CreateViewAccessor(0, Size);
        }

        /// <summary> Creates an empty file. </summary>
        void CreateWafer(long size, string path)
        {
            var chunks = size / CHUNK_SIZE;
            var tail = size % CHUNK_SIZE;

            using var stream = File.OpenWrite(path);
            using var writer = new BinaryWriter(stream);

            var chunk = new byte[CHUNK_SIZE];

            for (long i = 0; i < chunks; i++)
            {
                writer.Write(chunk);
                Console.Write($"\rWriting chunk {i} of {chunks}... ");
            }

            Console.WriteLine("done");

            if (0 != tail)
            {
                chunk = new byte[tail];
                writer.Write(chunk);
            }
        }

        public void Read<T>(long address, out T v) where T:struct => _accessor!.Read(address, out v);
        public void Write<T>(long address, T value) where T : struct => _accessor!.Write(address, ref value);

        public void Dispose()
        {
            if(null != _accessor)
                _accessor.Dispose();
            
            if(null != _mappedFile)
                _mappedFile.Dispose();

            _accessor = null;
            _mappedFile = null;
        }

        public void Flush() => _accessor?.Flush();
    }
}
