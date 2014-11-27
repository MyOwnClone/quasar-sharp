using System;
using System.IO;
using System.Collections.Generic;

namespace GruntXProductions.Quasar.VM
{
    public class DeviceROMController : Device
    {

        private Stream bootFirmware;

        public DeviceROMController(Stream bootCode)
        {
            bootFirmware = bootCode;
        }

        public override void Init(Emulator emu)
        {
            emu.Memory.MapRegion(new DeviceMappedRegion(0xFFD10000, 0xFFDFFFFF, null, bootReadCallback));
        }

        private void bootReadCallback(uint address, ref byte data)
        {
            uint actualAddress = address - 0xFFD10000;
            bootFirmware.Seek(actualAddress, SeekOrigin.Begin);
            data = (byte)bootFirmware.ReadByte();
        }


        private void romReadCallback(uint address, ref byte data)
        {
            uint actualAddress = address & 0xFFFFF;
            bootFirmware.Seek(actualAddress, SeekOrigin.Begin);
            data = (byte)bootFirmware.ReadByte();

        }
    }
}

