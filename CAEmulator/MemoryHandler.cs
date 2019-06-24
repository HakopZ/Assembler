using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CAEmulator
{
    class MemoryHandler
    {
        byte[] AddressSpace { get; }
        public MemoryHandler(ref byte[] AddressSpace)
        {
            this.AddressSpace = AddressSpace;
        }

        public Span<ushort> Ram => MemoryMarshal.Cast<byte, ushort>(AddressSpace.AsSpan());
        public Span<sbyte> BRam => MemoryMarshal.Cast<ushort, sbyte>(Ram);
        public Span<byte> Memory => AddressSpace;
        public Span<byte> ByteInstructionSpace => AddressSpace.AsSpan().Slice(0x8000);
        public Span<uint> UintInstructionSpace => MemoryMarshal.Cast<byte, uint>(ByteInstructionSpace);
     
        

    }
}
