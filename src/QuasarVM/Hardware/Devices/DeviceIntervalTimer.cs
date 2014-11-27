using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruntXProductions.Quasar.VM
{
    public class DeviceIntervalTimer : Device
    {
        private const byte TIMER_ENABLED = 0x01;

        private byte controlRegister;
        private uint interval;

        private ulong lastMillisecond = 0;
        public override void Init(Emulator emu)
        {
            lastMillisecond = (ulong)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            emu.PeripheralController.RequestIOPort(this, 0x110);
            emu.PeripheralController.RequestIOPort(this, 0x111);
            emu.PeripheralController.RequestIOPort(this, 0x112);
            emu.PeripheralController.RequestIOPort(this, 0x113);
            emu.PeripheralController.RequestIOPort(this, 0x114);
        }

        public override void Update(Emulator emu)
        {
            if ((controlRegister & TIMER_ENABLED) != 0)
            {
                ulong elapsed = (ulong)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds - lastMillisecond;
                if (elapsed >= interval)
                {
                    lastMillisecond = (ulong)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
                    emu.InterruptController.InterruptRequest(0x0F);
                }
            }
        }

        public override void RecieveData(int port, uint data)
        {
            switch (port)
            {
                case 0x110:
                    controlRegister = (byte)data;
                    break;
                case 0x111:
                    this.interval |= data;
                    break;
                case 0x112:
                    this.interval |= data << 8;
                    break;
                case 0x113:
                    this.interval |= data << 16;
                    break;
                case 0x114:
                    this.interval |= data << 24;
                    break;
            }
        }
    }
}
