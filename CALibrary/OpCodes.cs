using System;

namespace CALibrary
{
    public enum OpCodes
    {
        Nop = 0,
        Or = 1,
        And = 2,
        Xor = 3,
        Shr = 4,
        Shl = 5, 
        Sar = 6,
        Not = 7,
        Set = 8,
        Add = 10,
        Sub = 11,
        Mlp = 12,
        Div = 13,
        Mod = 14,
        Eq = 15,
        NEq = 16,
        GT = 17,
        LT = 18,

       
        Mov = 21,
        Push = 22, 
        Pop = 23,
        PSC = 24,
        Ldr = 25,
        Str = 26,
        Ldi = 27,
        Sti = 28,

        Jmp = 30,
        JNZ = 31,
        JZ  = 32,
        Call = 33, 
        Ret = 34,
        Jmpi = 35,
        Calli = 36,
        Brk = 39
    }

    public enum OpCodeTypes
    {
        NoArgs,
        OneReg,
        TwoReg,
        ThreeReg,
        OneAddr,
        OneRegOneAddr,
        TwoRegOneOffset,
    }
}
