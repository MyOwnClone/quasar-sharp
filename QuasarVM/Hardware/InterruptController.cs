using System;
using System.Collections.Generic;

namespace GruntXProductions.Quasar.VM
{
	public class InterruptController : Device
	{
		private Queue<byte> irqQueue = new Queue<byte>();
		private Emulator host;
		private bool interruptInProgress = false;
		private byte interruptInServiceRegister;
		private byte irqMaskRegister;
		private byte statusRegister;
		
		public bool IrqPending
		{
			get
			{
				return this.irqQueue.Count != 0;
			}
		}
		
		public void InterruptRequest(byte irq)
		{
			lock(irqQueue)
			{
				irqQueue.Enqueue(irq);
			}
		}
		
		
		public override void Update (Emulator emu)
		{
			if(!interruptInProgress && irqQueue.Count != 0)
			{
				byte intr = irqQueue.Dequeue();
				if(((1 >> 8) & irqMaskRegister) == 0)
					this.host.Interrupt(intr);
			}
		}
		
		public override void Init (Emulator emu)
		{
			this.host = emu;
			emu.PeripheralController.RequestIOPort(this, 0x00);
			emu.PeripheralController.RequestIOPort(this, 0x01);
			emu.PeripheralController.RequestIOPort(this, 0x02);
			emu.PeripheralController.RequestIOPort(this, 0x03);
		}
		
		public override void RecieveData (int port, uint data)
		{
			switch(port) {
			case 0x00:
				this.statusRegister = (byte)data;
				break;
			case 0x01:
				this.irqMaskRegister = (byte)data;
				break;
			case 0x02:
				this.interruptInServiceRegister = (byte)data;
				break;
			}
		}
		
		public override uint RequestData (int port)
		{
			switch(port) {
			case 0x00:
				return this.statusRegister;
			case 0x01:
				return this.irqMaskRegister;
			case 0x02:
				return this.interruptInServiceRegister;
			}
			return 0;
		}
		
	}
}

