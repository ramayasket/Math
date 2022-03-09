using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math.Computer
{
    /// <summary> Random access memory. </summary>
    public interface IRAM
    {
        long Size { get; }

        byte Read(long address);
        void Write(long address, byte value);
    }
}
