using CALibrary;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CAEmulator
{



    class Program
    {
        static byte[] AddressSpace = new byte[0x10000];
    
       
        static void Main(string[] args)
        {
            MemoryHandler memoryHandler = new MemoryHandler(ref AddressSpace);
           

            var docsFold = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var binaryFile = Path.Combine(docsFold, "asmBinaries", "Binary.bin");
            byte[] binary = File.ReadAllBytes(binaryFile);

            binary.AsSpan().CopyTo(AddressSpace.AsSpan().Slice(0x8000));

            ushort sp = 0x8000;
            ushort pc = 0x8000;
            Registers Register = new Registers(ref sp, ref pc);
            
            Register[31] = 0x8000;
          
            Register[30] = 0x8000;
            Execution execution = new Execution(memoryHandler, Register);
            MMIO mmio = new MMIO(AddressSpace);
            
            while (true)
            {
                mmio.Update();
                Span<byte> instruction = memoryHandler.ByteInstructionSpace.Slice(Register[31] - 0x8000, 4);
                Register[31] += 4;
                int code = instruction[0];
                OpCodeTypes opType = OpCodeHelpers.OpCodeTypeMap[(OpCodes)code];
                switch (opType)
                {
                    case OpCodeTypes.NoArgs:
                        execution.NoArgs(code);
                        break;
                    case OpCodeTypes.OneAddr:
                        ushort addr = (ushort)((instruction[2] << 8) | (instruction[3]));
                        execution.OneAddress(code, addr);
                        break;
                    case OpCodeTypes.OneRegOneAddr:
                        ushort address = (ushort)((instruction[2] << 8) | (instruction[3]));
                        execution.OneRegOneAdr(code, instruction[1], address);
                        break;
                    case OpCodeTypes.OneReg:
                        execution.OneReg(code, instruction[1]);
                        break;
                    case OpCodeTypes.TwoReg:
                        execution.TwoReg(code, instruction[1], instruction[2]);
                        break;

                    case OpCodeTypes.TwoRegOneOffset:
                        execution.TwoRegOneOffset(code, instruction[1], instruction[2], instruction[3]);
                        break;
                    case OpCodeTypes.ThreeReg:
                        execution.ThreeReg(code, instruction[1], instruction[2], instruction[3]);
                        break;
                }
            }

        }
    }
}
