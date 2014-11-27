using System;

namespace GruntXProductions.Quasar.VM
{
    public class SegmentationFaultException : Exception
    {
        private uint badAddress;

        public uint FaultingAddress
        {
            get
            {
                return badAddress;
            }
        }

        public SegmentationFaultException(uint address)
        {
            this.badAddress = address;
        }
    }
}

