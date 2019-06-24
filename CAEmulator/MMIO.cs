using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CAEmulator
{
    class MMIO
    {
        readonly Memory<byte> mmioAddr;
        readonly Random rand = new Random();


        public MMIO(byte[] fullAddr)
        {
            mmioAddr = fullAddr.AsMemory().Slice(0, 0xFF);
        }
        void CharFunctions(Span<ushort> mmioShort)
        {
            //WRCHRFLAg 10 11
            //WRCHR 8 9 maybe 
            //RDCHR 20 21
            //RdChrFlg 22 23

            if (mmioShort[10 / 2] != 0)
            {
                Console.Write((char)mmioShort[8 / 2]);
                if(mmioShort[20/2] == 13)
                {
                    Console.WriteLine();
                }
                mmioShort[10 / 2] = 0;
            }
            if (Console.KeyAvailable && mmioShort[22 / 2] == 0)
            {
                
                mmioShort[22 / 2] = 1;
                mmioShort[20 / 2] = Console.ReadKey().KeyChar;
                if(mmioShort[20/2] == 13)
                {
                    Console.WriteLine();
                }
            }
        }
        void IntFunctions(Span<ushort> mmioShort)
        {
            //WRIntFlag 14 15
            //Wrint 12 13
            //ReadInt 16 17
            //ReadIntFlag 18 19

            if (mmioShort[14 / 2] != 0)
            {
                Console.Write(mmioShort[12 / 2]);
                mmioShort[14 / 2] = 0;
            }
            
        }

  
        public void Update()
        {
            Span<byte> mmio = mmioAddr.Span;
            Span<ushort> mmioShort = MemoryMarshal.Cast<byte, ushort>(mmio);
            rand.NextBytes(mmio.Slice(4, 2)); //Random is 4
           
            CharFunctions(mmioShort);
            IntFunctions(mmioShort);
        }
    }
}
