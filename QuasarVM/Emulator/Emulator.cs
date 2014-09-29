using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GruntXProductions.Quasar.VM
{
	public partial class Emulator
	{
		private QuasarRam memory;
		private PeripheralController peripheralController;
		private List<Device> devices = new List<Device>();
		private Stack stack;
		
		private uint ivtRegister;
		private uint pageDirectory;
		private uint userRegisterFileBase;
		private uint supervisorRegisterFileBase;
		private uint controlRegister;
		private InterruptController interruptController;
		
		private bool cpuException = false;
		private bool doubleFault = false;
		
		private long instructionsPerSecond = 100000;
		private long instructionCount = 0;
		
		private static Opcode[] PrivillagedInstructions = new Opcode[]{Opcode.SURF, Opcode.SSRF, Opcode.SIVT,
			Opcode.SPDR, Opcode.SCTL, Opcode.LURF, Opcode.LSRF, Opcode.LIVT, Opcode.LCTL, Opcode.LPDR, Opcode.OUT,
			Opcode.OUT, Opcode.IN, Opcode.IRTN, Opcode.WAIT};
		
		
		public uint InterruptVectorTable
		{
			get
			{
				return this.ivtRegister;
			}
		}
		
		public uint PageDirectory
		{
			get
			{
				return this.pageDirectory;
			}
		}
		
		public uint UserRegisterFile
		{
			get
			{
				return this.userRegisterFileBase;
			}
		}
		
		public uint SupervisorRegisterFile
		{
			get
			{
				return this.supervisorRegisterFileBase;
			}
		}
		
		public uint ControlRegister
		{
			get
			{
				return this.controlRegister;
			}
		}
		
		public PeripheralController PeripheralController
		{
			get
			{
				return this.peripheralController;
			}
		}
		
		public QuasarRam Memory
		{
			get
			{
				return this.memory;
			}
		}
		
		public Stack Stack
		{
			get
			{
				return this.stack;
			}
		}
	
		public InterruptController InterruptController
		{
			get	
			{
				return this.interruptController;
			}
		}
		
		public Emulator(QuasarRam memory)
		{
			this.memory = memory;
		}
		
		public uint GetGeneralPurposeRegister(Register reg)
		{
			uint regBase = IsSupervisor() ? supervisorRegisterFileBase : userRegisterFileBase;
			return memory.ReadInt32(regBase + ((uint)reg * 4u), true);
		}
		
		public uint SetGeneralPurposeRegister(Register reg, uint val)
		{
			uint regBase = IsSupervisor() ? supervisorRegisterFileBase : userRegisterFileBase;
			uint prev = memory.ReadInt32(regBase + ((uint)reg * 4u));
			memory.WriteInt32(regBase + ((uint)reg * 4u), val, true);
			return prev;
		}
		
		public bool IsSupervisor()
		{
			return (controlRegister & 0x1) == 0;
		}
		
		public void RegisterDevice(Device dev)
		{
			this.devices.Add(dev);
		}
		
		public void Emulate()
		{
			this.Reset();
			
			int second = System.DateTime.Now.Second;
			while(true)
			{
				uint pc = GetGeneralPurposeRegister(Register.R15);
				Instruction ins = Instruction.Fetch(this.memory, pc);
				pc += (uint)ins.Size;
				SetGeneralPurposeRegister(Register.R15, pc);
				try
				{
					if(!decodeInstruction(ins))
						Trap(0x02);
				}
				catch(DivideByZeroException)
				{
					Trap(0x01);
				}
				catch(InvalidOpcodeException)
				{
					Trap(0x02);
				}
				catch(SegmentationFaultException ex)
				{
					Trap(0x03, ex.FaultingAddress);
				}
				catch(PageFaultException ex)
				{
					Trap(0x04, ex.FaultingAddress);
				}
				catch(GeneralProtectionException ex)
				{
					Trap(0x05, (uint)ex.Opcode);
				}
				updateDevices();
				if(instructionCount > instructionsPerSecond)
				{
					while(second == System.DateTime.Now.Second)
						Thread.Yield();
					second = System.DateTime.Now.Second;
					instructionCount = 0;
				}
				else
					instructionCount++;
			}
		}
		
		public void Interrupt(byte intnum)
		{
			uint oldCtl = controlRegister;
			controlRegister &= ~0x01u;
			stack.PushInt32(oldCtl);
			for(int i = 0; i < 16; i++)
				stack.PushInt32(GetGeneralPurposeRegister((Register)i));
			SetGeneralPurposeRegister(Register.R15, memory.ReadInt32(ivtRegister + (uint)(intnum * 4)));
		}
		
		public void Trap(byte exception, uint errorcode)
		{
			Console.WriteLine("Trap {0}", exception);
			if(this.cpuException)
				Trap(0x06, exception);
			else if(doubleFault)
				this.Reset();
			else
			{
				this.cpuException = true;
				
				this.Interrupt(exception);
				
				SetGeneralPurposeRegister(Register.R11, (uint)errorcode);
			}
			this.doubleFault = this.cpuException;
		}
		
		public void Trap(byte exception)
		{
			Trap(exception, 0);
		}
		
		public void Reset()
		{
			this.supervisorRegisterFileBase = 0x1000;
			this.controlRegister = 0;
			this.doubleFault = false;
			this.cpuException = false;
			this.peripheralController = new PeripheralController(this);
			this.stack = new Stack(this);
			this.interruptController = new InterruptController();
			this.RegisterDevice(this.interruptController);
			foreach(Device dev in this.devices)
				dev.Init(this);
		}
		
		private void updateDevices()
		{
			for(int i = 0; i < devices.Count; i++)
				devices[i].Update(this);
		}
		
		private bool privillagedInstruction(Opcode opcode)
		{
			for(int i = 0; i < PrivillagedInstructions.Length; i++)
				if(PrivillagedInstructions[i] == opcode)
					return true;
			return false;
		}
		
		private bool decodeInstruction(Instruction ins)	
		{
			if(((controlRegister & 0x1) != 0 && privillagedInstruction(ins.OperationCode))
			   || ((controlRegister & 0x3) == 3 && (memory.GetPagePermissions(GetGeneralPurposeRegister(Register.R15)) & Page.EXECUTE) == 0))
				throw new GeneralProtectionException(ins.OperationCode);
			switch(ins.OperationCode)
			{
			case Opcode.MOV:
				interpretMov(ins);
				return true;
			case Opcode.SWP:
				interpretSwp(ins);
				return true;
			case Opcode.SCTL:
				interpretSctl(ins);
				return true;
			case Opcode.SIVT:
				interpretSivt(ins);
				return true;
			case Opcode.SPDR:
				interpretSpdr(ins);
				return true;
			case Opcode.SURF:
				interpretSurf(ins);
				return true;
			case Opcode.SSRF:
				interpretSctl(ins);
				return true;
			case Opcode.LCTL:
				interpretLctl(ins);
				return true;
			case Opcode.LIVT:
				interpretLivt(ins);
				return true;
			case Opcode.LPDR:
				interpretLpdr(ins);
				return true;
			case Opcode.LURF:
				interpretLurf(ins);
				return true;
			case Opcode.LSRF:
				interpretLctl(ins);
				return true;
			case Opcode.CMP:
				interpretCmp(ins);
				return true;
			case Opcode.TST:
				interpretTst(ins);
				return true;
			case Opcode.BR:
				interpretBranch(ins);
				return true;
			case Opcode.BSR:
				interpretBsr(ins);
				return true;
			case Opcode.RTN:
				interpretRtn(ins);
				return true;
			case Opcode.IRTN:
				interpretIRtn(ins);
				return true;
			case Opcode.ADD:
				interpretAdd(ins);
				return true;
			case Opcode.SUB:
				interpretSub(ins);
				return true;
			case Opcode.MUL:
				interpretMul(ins);
				return true;
			case Opcode.DIV:
				interpretDiv(ins);
				return true;
			case Opcode.IMUL:
				interpretIMul(ins);
				return true;
			case Opcode.IDIV:
				interpretIDiv(ins);
				return true;
			case Opcode.MOD:
				interpretMod(ins);
				return true;
			case Opcode.AND:
				interpretAnd(ins);
				return true;
			case Opcode.BOR:
				interpretBor(ins);
				return true;
			case Opcode.XOR:
				interpretXor(ins);
				return true;
			case Opcode.SHL:
				interpretShl(ins);
				return true;
			case Opcode.SHR:
				interpretShr(ins);
				return true;
			case Opcode.SAL:
				interpretSal(ins);
				return true;
			case Opcode.SAR:
				interpretSar(ins);
				return true;
			case Opcode.SC:
				interpretSc (ins);
				return true;
			case Opcode.WAIT:
				interpretWait (ins);
				return true;
			default:
				return false;
			}
		}
	}
}

