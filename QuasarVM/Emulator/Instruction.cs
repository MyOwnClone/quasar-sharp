using System;

namespace GruntXProductions.Quasar.VM
{
	public class Instruction
	{
		private Opcode opcode;
		private Operand operand1;
		private Operand operand2;
		private int size;
		private bool hasOperands = false;
		
		public Opcode OperationCode
		{
			get
			{
				return this.opcode;
			}
		}
		
		public Operand Operand1
		{
			get
			{
				return operand1;
			}
		}
		
		public Operand Operand2
		{
			get
			{
				return operand2;
			}
		}
		
		public int Size
		{
			get
			{
				return this.size;
			}
		}
		
		public bool HasOperands
		{
			get
			{
				return hasOperands;
			}
		}
		
		public Instruction(Opcode opcode)
		{
			this.opcode = opcode;
			this.hasOperands = false;
			this.size = 1;
		}
		
		public Instruction(Opcode opcode, int size, AddressingMode am, object op1)
		{
			this.opcode = opcode;
			this.operand1.Value = op1;
			this.operand1.OperandAddressingMode = am;
			this.hasOperands = true;
			this.size = size;
			
		}
		
		public Instruction(Opcode opcode, int size, AddressingMode am, object op1, AddressingMode am2, object op2)
		{
			this.opcode = opcode;
			this.operand1.Value = op1;
			this.operand1.OperandAddressingMode = am;
			this.operand2.Value = op2;
			this.operand2.OperandAddressingMode = am2;
			this.hasOperands = true;
			this.size = size;
		}
		
		public static Instruction Fetch(QuasarRam memory, uint address)
		{
			uint org_add = address;
			byte original = memory[address++];
			Opcode op = (Opcode)(original & 0x7f);
			AddressingMode am1 = AddressingMode.NONE;
			AddressingMode am2 = AddressingMode.NONE;
			bool hasOperands = ((int)original & 128) == 0;
		
			if(hasOperands)
			{
				byte descr = memory[address++];
				am1 = (AddressingMode)(descr & 0x0F);
				am2 = (AddressingMode)((descr & 0xF0) >> 4);
				object op1 = ReadOperand(memory, ref address, am1);
				object op2 = ReadOperand(memory, ref address, am2);
				int size = (int)(address - org_add);
				return new Instruction(op, size, am1, op1, am2, op2);
				
			}
			else
				return new Instruction(op);
			
		}
		
		private static object ReadOperand(QuasarRam memory, ref uint address, AddressingMode am)
		{
			switch(am)
			{
			case AddressingMode.DIRECT_REGISTER:
				address++;
				return (Register)memory[address - 1];
			case AddressingMode.INDIRECT_REG32:
				address++;
				return (Register)memory[address - 1];
			case AddressingMode.INDIRECT_REG16:
				address++;
				return (Register)memory[address - 1];
			case AddressingMode.INDIRECT_REG8:
				address++;
				return (Register)memory[address - 1];
			case AddressingMode.IMMEDIATE_32:
				address += 4;
				return memory.ReadInt32(address - 4);
			}
			return null;
		}
		
		
	}
}

