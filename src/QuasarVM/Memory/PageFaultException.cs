using System;

namespace GruntXProductions.Quasar.VM
{
    public class PageFaultException : Exception
    {
        private uint badAddress;

        public uint FaultingAddress
        {
            get
            {
                return this.badAddress;
            }
        }

        public PageFaultException(uint badAddress)
        {
            this.badAddress = badAddress;
        }
    }
}

