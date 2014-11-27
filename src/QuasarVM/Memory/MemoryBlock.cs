using System;

namespace GruntXProductions.Quasar.VM
{
    public class MemoryBlock
    {
        private byte[] memory = new byte[0x1000];
        private uint address;

        public uint Address
        {
            get
            {
                return address;
            }
        }

        public MemoryBlock(uint address)
        {
            this.address = address;
        }

        public byte this[uint index]
        {
            get
            {
                return memory[index & 0xFFF];
            }
            set
            {
                memory[index & 0xFFF] = value;
            }
        }
    }
}

