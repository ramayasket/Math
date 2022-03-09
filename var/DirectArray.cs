using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kw.Common;

namespace Math.Computer
{
    public class DirectArray : IRAM
    {
        byte[] _array;

        public long Size { get; }

        public DirectArray(long size) => _array = new byte[size];

        public byte Read(long address) => _array[address];

        public void Write(long address, byte value) => _array[address] = value;
    }
}
