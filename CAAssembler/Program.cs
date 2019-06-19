using CALibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace CAAssembler
{
    class Program
    {
        
        static byte ReadRegister(ref ReadOnlySpan<char> data)
        {
            int afterSpace = data.IndexOf(' ');
            byte ret;
            if(afterSpace >= 0)
            {
                ret = byte.Parse(data.Slice(1, afterSpace));
                data = data.Slice(afterSpace).Trim();
                return ret;
            }
            ret = byte.Parse(data.Slice(1));
            data = ReadOnlySpan<char>.Empty;
            return ret;
        }

        static void WriteThreeRegister(OpCodes code, ReadOnlySpan<char> restOfInst, List<byte> binary)
        {
            binary.Add((byte)code);
            binary.Add(ReadRegister(ref restOfInst));
            binary.Add(ReadRegister(ref restOfInst));
            binary.Add(ReadRegister(ref restOfInst));
        }
        static void WriteTwoReg(OpCodes code, ReadOnlySpan<char> rest, List<byte> binary)
        {
            binary.Add((byte)code);
            binary.Add(ReadRegister(ref rest));
            binary.Add(ReadRegister(ref rest));
            binary.Add(0);
        }
        static void WriteOneReg(OpCodes code, ReadOnlySpan<char> reg, List<byte> binary)
        {
            binary.Add((byte)code);
            binary.Add(ReadRegister(ref reg));
            binary.Add(0);
            binary.Add(0);
        }
        static void WriteOneAddress(OpCodes code, ReadOnlySpan<char> inst, List<byte> binary, 
            Dictionary<int, string> labelReplacements)
        {
            binary.Add((byte)code);
            binary.Add(0);
            if (ushort.TryParse(inst, out var addr))
            {
                binary.Add((byte)(addr >> 8 & 0x0FF));
                binary.Add((byte)(addr & 0x0FF));
            }
            else
            {
                labelReplacements.Add(binary.Count, inst.ToString());
                binary.Add(0);
                binary.Add(0);
            }
        }
        static void WriteOneRegAndOneAddr(OpCodes code, ReadOnlySpan<char> inst, List<byte> binary,
            Dictionary<int, string> labelReplacements)
        {
            binary.Add((byte)code);
            binary.Add(ReadRegister(ref inst));
            if (ushort.TryParse(inst, out var addr))
            {
                binary.Add((byte)(addr >> 8 & 0x0FF));
                binary.Add((byte)(addr & 0x0FF));
            }
            else
            {
                labelReplacements.Add(binary.Count, inst.ToString());
                binary.Add(0);
                binary.Add(0);
            }
        }
        static void WriteTwoRegOneOffSet(OpCodes opcode, ReadOnlySpan<char> instr, List<byte> binary)
        {
            binary.Add((byte)opcode);
            binary.Add(ReadRegister(ref instr));
            binary.Add(ReadRegister(ref instr));
            if(sbyte.TryParse(instr, out var result))
            {
                binary.Add((byte)result);
            }
            else { binary.Add(0); }
        }
        static void ParseInstruction(ReadOnlySpan<char> instruction, List<byte> binary, Dictionary<int, string> labelReplacements)
        {
            int firstSpace = instruction.IndexOf(' ');
           
            if (firstSpace < 0)
            {
                firstSpace = instruction.ToString().Length;
            }
            ReadOnlySpan<char> opCodeSpan = instruction.Slice(0, firstSpace);
            instruction = instruction.Slice(firstSpace).Trim();
            OpCodes opCode = Enum.Parse<OpCodes>(opCodeSpan.ToString(), true);
            OpCodeTypes opType = OpCodeHelpers.OpCodeTypeMap[opCode];
            
            switch (opType)
            {
                case OpCodeTypes.NoArgs:
                    binary.Add((byte)opCode);
                    for (int i = 0; i < 3; i++)
                       binary.Add(0);
                    break;
                case OpCodeTypes.OneReg:
                    WriteOneReg(opCode, instruction, binary);
                    break;
                case OpCodeTypes.TwoReg:
                    WriteTwoReg(opCode, instruction, binary);
                    break;
                case OpCodeTypes.ThreeReg:
                    WriteThreeRegister(opCode, instruction, binary);
                    break;
                case OpCodeTypes.OneAddr:
                    WriteOneAddress(opCode, instruction, binary,  labelReplacements);
                    break;
                case OpCodeTypes.OneRegOneAddr:
                    WriteOneRegAndOneAddr(opCode, instruction, binary, labelReplacements);
                    break;
                case OpCodeTypes.TwoRegOneOffset:
                    WriteTwoRegOneOffSet(opCode, instruction, binary);
                    break;
                default:
                    throw new Exception("Something happened");
                 
            }
        }
        static void Main(string[] args)
        {
            //for Assembler, and dissasembler
            string[] lines = File.ReadAllLines("MyProgram.txt");


            List<byte> binary = new List<byte>(0x7FFF);
            Dictionary<int, string> labelReplacements = new Dictionary<int, string>();
            Dictionary<string, int> labels = new Dictionary<string, int>();
            foreach (var stringline in lines)
            {
                ReadOnlySpan<char> line = stringline;
                ReadOnlySpan<char> label = ReadOnlySpan<char>.Empty; //change line

                int findComment = line.IndexOf("//");
                if(findComment >= 0)
                {
                    line = line.Slice(0, findComment);
                }



                int FindLabel = line.IndexOf(':');
                if(FindLabel >= 0)
                {
                    label = line.Slice(0, FindLabel).Trim();
                    line = line.Slice(FindLabel + 1).Trim();
                }
                if(!label.IsEmpty)
                {
                    labels.Add(label.ToString(), binary.Count);
                }
                //Do something with label
                if (line.IsEmpty)
                {
                    continue;
                }

                ParseInstruction(line, binary, labelReplacements);
            }
            foreach (var toReplace in labelReplacements)
            {
                var expectedSymbol = toReplace.Value;
                var replacement = labels[expectedSymbol] + 0x8000;
                binary[toReplace.Key] = (byte)((replacement >> 8) & 0xFF);
                binary[toReplace.Key + 1] = (byte)(replacement & 0xFF); 
            }

            var docsFold = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var outputFile = Path.Combine(docsFold, "asmBinaries", "Binary.bin");
            File.WriteAllBytes(outputFile, binary.ToArray());
             Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
