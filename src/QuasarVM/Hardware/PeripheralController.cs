using System;
using System.Collections.Generic;

namespace GruntXProductions.Quasar.VM
{
    public class PeripheralController : Device
    {
        private Dictionary<int, Device> ioPorts = new Dictionary<int, Device>();
        private Emulator host;

        public Emulator Host
        {
            get
            {
                return this.host;
            }
        }

        public PeripheralController(Emulator host)
        {
            this.host = host;
        }

        public bool RequestIOPort(Device dev, int port)
        {
            if (!ioPorts.ContainsKey(port))
            {
                ioPorts[port] = dev;
                return true;
            }
            else
                return false;
        }

        public void FreeIOPort(int port)
        {
            ioPorts.Remove(port);
        }

        public void Out(int port, uint data)
        {
            if (ioPorts.ContainsKey(port))
                ioPorts[port].RecieveData(port, data);
        }

        public uint In(int port)
        {
            if (ioPorts.ContainsKey(port))
                return ioPorts[port].RequestData(port);
            return 0;
        }

        public override void Init(Emulator emu)
        {
            emu.Memory.MapRegion(new DeviceMappedRegion(0xFFE00000, 0xFFFFFFFF, writeCallback, readCallback));
        }

        private void writeCallback(uint address, byte data)
        {
            Out((int)(address & 0xFFFFF), data);
        }

        private void readCallback(uint address, ref byte data)
        {
            data = (byte)In((int)(address & 0xFFFFF));
        }


    }
}

