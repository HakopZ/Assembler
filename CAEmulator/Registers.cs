using System;
using System.Collections.Generic;
using System.Text;

namespace CAEmulator
{
    class Registers
    {
        readonly private ushort[] registers = new ushort[32];

        public ref ushort PC => ref registers[31];

        public ref ushort SP => ref registers[30];

        public ref ushort this[int index] => ref registers[index];
        public Registers(ref ushort sp, ref ushort pc)
        {
            PC = pc;
            SP = sp;
        }
        public ushort Add(int FirReg, int SecReg) { return (ushort)(registers[FirReg] + registers[SecReg]); }
        public ushort Div(int FirReg, int SecReg) { return (ushort)(registers[FirReg] / registers[SecReg]); }
        public ushort Sub(int FirReg, int SecReg) { return (ushort)(registers[FirReg] - registers[SecReg]); }
        public ushort Mlp(int FirReg, int SecReg) { return (ushort)(registers[FirReg] * registers[SecReg]); }
        public ushort Mod(int FirReg, int SecReg) { return (ushort)(registers[FirReg] % registers[SecReg]); }

        public ushort Equal(int FirReg, int SecReg)
        {
            return (ushort)(registers[FirReg] == registers[SecReg] ? 1 : 0);
        }
        public ushort NotEqual(int FirReg, int SecReg)
        {
            return (ushort)Math.Abs(Equal(FirReg, SecReg) - 1);
        }
        public ushort GreaterThan(int FirReg, int SecReg)
        {
            return (ushort)(registers[FirReg] > registers[SecReg] ? 1 : 0);
        }
        public ushort LessThan(int FirReg, int SecReg)
        {
            return (ushort)Math.Abs(GreaterThan(FirReg, SecReg) - 1);
        }
    }
}
