using System.Runtime.InteropServices;

namespace Math.Computer;

/// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
[StructLayout(LayoutKind.Sequential)]
public struct RegistryHeader
{
    readonly byte _m1;
    readonly byte _x1;

    public long Length;
    public long KnownBoundary;
    public long KnownCount;

    readonly byte _x2;
    readonly byte _m2;

    public RegistryHeader()
    {
        _m1 = _m2 = (byte)'M';
        _x1 = _x2 = (byte)'X';

        Length = 0;
        KnownBoundary = 0;
        KnownCount = 0;
    }

    public RegistryHeader(long length)
    {
        _m1 = _m2 = (byte)'M';
        _x1 = _x2 = (byte)'X';

        Length = length;
        KnownBoundary = 0;
        KnownCount = 0;
    }

    public void Validate(string path)
    {
        if (_m1 != (byte)'M' || _m2 != (byte)'M' || _x1 != (byte)'X' || _x2 != (byte)'X')
            throw new InvalidOperationException($"The file '{path}' appears to be corrupt.");
    }
}