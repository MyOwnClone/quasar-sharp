using System;

namespace GruntXProductions.Quasar.VM
{
    public abstract class Device
    {
        public abstract void Init(Emulator emu);
        public virtual void RecieveData(int port, uint data) { }
        public virtual uint RequestData(int port) { return 0; }
        public virtual void Update(Emulator emu) { }
    }
}