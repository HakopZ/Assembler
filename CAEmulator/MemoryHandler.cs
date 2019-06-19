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
        public Span<ushort> Ram => MemoryMarshal.Cast<byte, ushort>(AddressSpace.AsSpan().Slice(0, 0x8000));

        public Span<byte> ByteInstructionSpace => AddressSpace.AsSpan().Slice(0x8000);
        public Span<uint> UintInstructionSpace => MemoryMarshal.Cast<byte, uint>(ByteInstructionSpace);
     

    }
}
