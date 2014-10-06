using System;
using System.IO;

namespace GruntXProductions.Quasar.VM
{
	public class DeviceTextScreen : Device
	{
		private IDisplay display;
		private byte[] fontMap = new byte[]{};
		private byte[] videoMemory = new byte[4000];
		private uint[] colors = new uint[]{0xFF000000, 0xFF000080, 0xFF008000, 0xFF008080, 0xFF800000, 
			0xFF800080, 0xFF808000, 0xFFAAAAAA, 0xFF0000FF, 0xFF00FF00, 0xFF00FFFF, 0xFFFF0000, 0xFFFF00FF, 0xFFFFFF00, 0xFFFFFFFF, 0xFFFFFFFF};
		private int cursorPosition = 0;
		private bool enableCursor = false;
		
		public DeviceTextScreen (IDisplay display)
		{
			this.display = display;

			using(Stream fntStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("GruntXProductions.Quasar.VM.ROM.font.bin"))
			{
				fontMap = new byte[(int)fntStream.Length];
				fntStream.Read(fontMap, 0, (int)fntStream.Length);
			}
		}
		
		public override void Init (Emulator emu)
		{
			emu.PeripheralController.RequestIOPort(this, 0x101);
			emu.PeripheralController.RequestIOPort(this, 0x102);
            emu.Memory.MapRegion(new DeviceMappedRegion(0xFFA00000, 0xFFD0FFFF, writeCallback, readCallback));
		}
		
		
		public override void RecieveData (int port, uint data)
		{
			switch(port)
			{
			case 0x101:
				this.clearCursor(this.cursorPosition);
				this.cursorPosition = (int)data;
				break;
			case 0x102:
				this.enableCursor = data != 0;
				this.clearCursor(this.cursorPosition);
				break;
			}
		}
		
		public override uint RequestData (int port)
		{
			throw new NotImplementedException ();
		}
		
		private void writeCallback(uint address, byte data)
		{
            int pos = (int)(address - 0xFFA00000);
			int pos2 = pos / 2;
			videoMemory[pos] = data;
			
			if(address % 2 == 0)
				drawCharacter((char)data, videoMemory[pos + 1], pos2 % 80, pos2 / 80);
			else
				drawCharacter((char)videoMemory[pos - 1], data, pos2 % 80, pos2 / 80);
		}
		
		private void readCallback(uint address, ref byte data)
		{
		}
		
		private void drawCharacter(char c, byte attributes, int cx, int cy)
		{
			int start = ((int)c * 16);
			byte fg = (byte)(attributes & 0x0F);
			byte bg = (byte)((attributes & 0xF0) >> 4);
			int xP = cx * 8;
			int yP = cy * 16;
			for(int y = 0; y < 16; y++)
			{
				int pixels = fontMap[start + y];
				for(int r = 0; r < 8; r++)
				{
					if(((1 << r) & pixels) != 0)
						display.SetPixel(xP + r, yP + y, (int)colors[fg]);
					else 
						display.SetPixel(xP + r, yP + y, (int)colors[bg]);
				}
			}
		}
		
		private void drawCursor(int position)
		{
			byte attributes = videoMemory[position * 2 + 1];
			char c = (char)videoMemory[position];
			int start = ((int)c * 16);
			int cx = position % 80;
			int cy = position / 80;
			byte fg = (byte)(attributes & 0x0F);
			byte bg = (byte)((attributes & 0xF0) >> 4);
			int xP = cx * 8;
			int yP = cy * 16;
			for(int y = 0; y < 16; y++)
			{
				int pixels = fontMap[start + y];
				for(int r = 0; r < 8; r++)
				{
					if(((1 << r) & pixels) != 0)
						display.SetPixel(xP + r, yP + y, (int)colors[bg]);
					else 
						display.SetPixel(xP + r, yP + y, (int)colors[fg]);
				}
			}
		}
		
		private void clearCursor(int position)
		{
			byte attributes = videoMemory[position * 2 + 1];
			char c = (char)videoMemory[position];
			int start = ((int)c * 16);
			int cx = position % 80;
			int cy = position / 80;
			byte fg = (byte)(attributes & 0x0F);
			byte bg = (byte)((attributes & 0xF0) >> 4);
			int xP = cx * 8;
			int yP = cy * 16;
			for(int y = 0; y < 16; y++)
			{
				int pixels = fontMap[start + y];
				for(int r = 0; r < 8; r++)
				{
					if(((1 << r) & pixels) != 0)
						display.SetPixel(xP + r, yP + y, (int)colors[fg]);
					else 
						display.SetPixel(xP + r, yP + y, (int)colors[bg]);
				}
			}
		}
		
		public override void Update (Emulator emu)
		{
            /*
			if(this.enableCursor && System.DateTime.Now.Second % 2 == 0)
				this.drawCursor(cursorPosition);
			else
				this.clearCursor(cursorPosition);
             */
		}
	}
}

