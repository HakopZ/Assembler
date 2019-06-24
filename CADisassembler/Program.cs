using CALibrary;
using System;
using System.IO;

namespace CADisassembler
{
    class Program
    {
      
        static void Main(string[] args)
        {
            byte[] AddressSpace = new byte[0x10000];
            var docsFold = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var binaryFile = Path.Combine(docsFold, "asmBinaries", "Binary.bin");
            byte[] binary = File.ReadAllBytes(binaryFile);
            binary.AsSpan().CopyTo(AddressSpace.AsSpan().Slice(0x8000));
            for (int i = 0; i < binary.Length; i+=4)
            {
                OpCodes x = (OpCodes)binary[i];
                if((int)x == 175)
                {
                    int length = binary[i + 2];
                    length = length * 2;
                    for (int l = 0; l < length; l+=2)
                    {
                        Console.Write($"{(char)binary[(i + 4) + l]}");
                    }
                    i += 2;
                    i += length;
                    Console.WriteLine();
                    continue;
                }
                Console.Write(x);
                OpCodeTypes opType = OpCodeHelpers.OpCodeTypeMap[x];
                switch (opType)
                {
                    case OpCodeTypes.NoArgs:
                        Console.WriteLine();
                        break;
                    case OpCodeTypes.OneAddr:
                        ushort addr = (ushort)((binary[i + 2] << 8) | (binary[i + 3]));
                        if (x == OpCodes.PSC)
                        {
                            Console.Write($" {addr}\n");
                        }
                        else
                        { Console.Write($" {AddressSpace[addr]}\n"); }
                        break;
                    case OpCodeTypes.OneReg:
                        Console.Write($" r{binary[i + 1]}\n");
                        break;
                    case OpCodeTypes.OneRegOneAddr:
                        ushort address = (ushort)((binary[i + 2] << 8) | (binary[i + 3]));
                        if (x == OpCodes.Set)
                        {
                            Console.Write($" r{binary[i + 1]} {address}\n");
                        }
                        else
                        {
                            Console.Write($" r{binary[i+1]} {AddressSpace[address]}\n");
                        }
                        break;
                    case OpCodeTypes.ThreeReg:
                        Console.Write($" r{binary[i + 1]} r{binary[i + 2]} r{binary[i + 3]}\n");
                        break;
                    case OpCodeTypes.TwoReg:
                        Console.Write($" r{binary[i + 1]} r{binary[i + 2]}\n");
                        break;
                    case OpCodeTypes.TwoRegOneOffset:
                        Console.Write($" r{binary[i + 1]} r{binary[i + 2]} {binary[i + 3]}\n");
                        break;
                }
            }
            Console.ReadKey(); 
        }
    }
}
