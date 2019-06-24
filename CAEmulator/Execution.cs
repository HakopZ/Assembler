using CALibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAEmulator
{
    class Execution 
    {
        readonly MemoryHandler memoryHandler;
        readonly Registers Registers;   
        public Execution(MemoryHandler x, Registers Registers)
        {
            this.Registers = Registers;
            memoryHandler = x;
        }
        public void NoArgs(int opCode)
        {
            OpCodes x = (OpCodes)opCode;
            switch (x)
            {
                case OpCodes.Nop:
                    break;
                case OpCodes.Brk:
                    break;
                case OpCodes.Ret:
                    Registers[31] = Registers[28];
                    break;
            }
        }
        public void TwoRegOneOffset(int opCode, int RegOne,  int RegTwo, int offset)
        {
            OpCodes x = (OpCodes)opCode;
            switch(x)
            {
                case OpCodes.Ldi:
                    
                    Registers[RegOne] = memoryHandler.Ram[(Registers[RegTwo] + (sbyte)offset) / 2];
                    break;
                case OpCodes.Sti:
                    memoryHandler.Ram[(Registers[RegTwo] + (sbyte)offset) / 2] = Registers[RegOne]; 
                    break;
            }
        }
       public void OneReg(int opCode, int Reg)
        {
            OpCodes x = (OpCodes)opCode;
            switch (x)
            {
                case OpCodes.Push:
                    if ((Registers[30] & 1) == 1)
                    {
                        throw new Exception("not alligned");
                    }
                    Registers[30] -= 2;
                    memoryHandler.Ram[Registers[30] / 2] = Registers[Reg];
                    break;
                case OpCodes.Pop:
                    if ((Registers[30] & 1) == 1)
                    {
                        throw new Exception("not alligned");
                    }
                    Registers[Reg] = memoryHandler.Ram[Registers[30] / 2];
                    Registers[30] += 2;
                    break;
                case OpCodes.Calli:
                    Registers[28] = Registers[31];
                    Registers[31] = Registers[Reg];
                    break;
            }
        }
        public void OneAddress(int opCode, ushort addr)
        {
            OpCodes x = (OpCodes)opCode;
            switch (x)
            {
                case OpCodes.PSC:
                    Registers[30]--;
                    memoryHandler.Ram[(Registers[30]) / 2] = addr;
                    break;
                case OpCodes.Jmp:
                    Registers[31] = addr;
                    break;
                case OpCodes.Call:
                    Registers[28] = Registers[31];
                    Registers[31] = addr;
                    break;
            }
        }
        public void OneRegOneAdr(int opCode, int reg, ushort addr)
        {
            OpCodes x = (OpCodes)opCode;
            switch (x)
            {
                case OpCodes.Set:
                    Registers[reg] = addr;
                    break;
                case OpCodes.JNZ:
                    if(Registers[reg] != 0) Registers[31] = addr;
                    break;
                case OpCodes.JZ:
                    if(Registers[reg] == 0) Registers[31] = addr;
                    break;
                case OpCodes.Ldr:
                    if ((addr & 1) == 1) throw new Exception("not alligned");
                    Registers[reg] = memoryHandler.Ram[addr / 2];
                    break;
                case OpCodes.Str:
                    if ((addr & 1) == 1) throw new Exception("not alligned");
                    memoryHandler.Ram[addr / 2] = Registers[reg];
                    break;
            }
        }
        public void TwoReg(int opCode, int FirReg, int SecReg)
        {
            OpCodes x = (OpCodes)opCode;
            switch(x)
            {
                case OpCodes.Mov:
                    Registers[FirReg] = Registers[SecReg];
                    break;
                case OpCodes.Not:
                    Registers[FirReg] = (ushort)~Registers[SecReg];
                    break;
            }
        }
        public void ThreeReg(int opCode, int output, int FirReg, int SecReg)
        {
            OpCodes x = (OpCodes)opCode;
            switch (x)
            {
                case OpCodes.Add:
                    Registers[output] = Registers.Add(FirReg, SecReg);
                    break;
                case OpCodes.Sub:
                    Registers[output] = Registers.Sub(FirReg, SecReg);
                    break;
                case OpCodes.Mlp:
                    Registers[output] = Registers.Mlp(FirReg, SecReg);
                    break;
                case OpCodes.Div:
                    Registers[output] = Registers.Div(FirReg, SecReg);
                    break;
                case OpCodes.Mod:
                    Registers[output] = Registers.Mod(FirReg, SecReg);
                    break;
                case OpCodes.Eq:
                    Registers[output] = Registers.Equal(FirReg, SecReg);
                    break;
                case OpCodes.NEq:
                    Registers[output] = Registers.NotEqual(FirReg, SecReg);
                    break;
                case OpCodes.And:
                    Registers[output] = (ushort)(Registers[FirReg] & Registers[SecReg]);
                    break;
                case OpCodes.Or:
                    Registers[output] = (ushort)(Registers[FirReg] | Registers[SecReg]);
                    break;
                case OpCodes.Xor:
                    Registers[output] = (ushort)(Registers[FirReg] ^ Registers[SecReg]);
                    break;
                case OpCodes.Shl:
                    Registers[output] = (ushort)(Registers[FirReg] << Registers[SecReg]);
                    break;
                case OpCodes.Shr:
                    Registers[output] = (ushort)(Registers[FirReg] >> Registers[SecReg]);
                    break;
                case OpCodes.Sar:
                    Registers[output] = (ushort)((short)Registers[FirReg] >> (short)Registers[SecReg]);
                    break;
                case OpCodes.GT:
                    Registers[output] = Registers.GreaterThan(FirReg, SecReg);
                    break;
                case OpCodes.LT:
                    Registers[output] = Registers.LessThan(FirReg, SecReg);
                    break;
            }
        }
    }
   
}   
