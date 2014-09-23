using System;
using System.Collections.Generic;

namespace GruntXProductions.Quasar.VM
{
	public class InterruptController : Device
	{
		private Queue<byte> irqQueue = new Queue<byte>();
		private Emulator host;
		private bool interruptInProgress = false;
		
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
				this.host.Interrupt(irqQueue.Dequeue());
			}
		}
		
		public override void Init (Emulator emu)
		{
			this.host = emu;
		}
		
		
	}
}

