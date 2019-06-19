using System;
using System.Collections.Generic;
using System.Text;

namespace CALibrary
{
    public static class OpCodeHelpers
    {
        public static IReadOnlyDictionary<OpCodes, OpCodeTypes> OpCodeTypeMap = new Dictionary<OpCodes, OpCodeTypes>()
        {
            //finish later
            [OpCodes.Nop] = OpCodeTypes.NoArgs,

            [OpCodes.Or] = OpCodeTypes.ThreeReg,
            [OpCodes.And] = OpCodeTypes.ThreeReg,
            [OpCodes.Xor] = OpCodeTypes.ThreeReg,
            [OpCodes.Shr] = OpCodeTypes.ThreeReg,
            [OpCodes.Shl] = OpCodeTypes.ThreeReg,
            [OpCodes.Sar] = OpCodeTypes.ThreeReg,
            [OpCodes.Not] = OpCodeTypes.TwoReg,

            [OpCodes.Add] = OpCodeTypes.ThreeReg,
            [OpCodes.Sub] = OpCodeTypes.ThreeReg,
            [OpCodes.Mlp] = OpCodeTypes.ThreeReg,
            [OpCodes.Div] = OpCodeTypes.ThreeReg,
            [OpCodes.Mod] = OpCodeTypes.ThreeReg,

            [OpCodes.Pop] = OpCodeTypes.OneReg,
            [OpCodes.Push] = OpCodeTypes.OneReg,
            [OpCodes.PSC] = OpCodeTypes.OneAddr,

            [OpCodes.Eq] = OpCodeTypes.ThreeReg,
            [OpCodes.NEq] = OpCodeTypes.ThreeReg,
            [OpCodes.GT] = OpCodeTypes.ThreeReg,
            [OpCodes.LT] = OpCodeTypes.ThreeReg,

            [OpCodes.Set] = OpCodeTypes.OneRegOneAddr,
            [OpCodes.Mov] = OpCodeTypes.TwoReg,
            [OpCodes.Jmp] = OpCodeTypes.OneAddr,
            [OpCodes.JZ] = OpCodeTypes.OneRegOneAddr,
            [OpCodes.JNZ] = OpCodeTypes.OneRegOneAddr,

            [OpCodes.Ldr] = OpCodeTypes.OneRegOneAddr,
            [OpCodes.Str] = OpCodeTypes.OneRegOneAddr,
            [OpCodes.Ldi] = OpCodeTypes.TwoRegOneOffset,
            [OpCodes.Sti] = OpCodeTypes.TwoRegOneOffset,
            [OpCodes.Call] = OpCodeTypes.OneAddr,
            [OpCodes.Ret] = OpCodeTypes.NoArgs,
            [OpCodes.Brk] = OpCodeTypes.NoArgs
        };
    }
}
