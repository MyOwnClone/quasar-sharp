using System;
using System.IO;

namespace GruntXProductions.Quasar.VM
{
	public class DeviceTextScreen : Device
	{
        enum VideoMode
        {
            TEXTMODE_80x25 = 0,
            SVGA_800x600 = 1,
        }

		private IDisplay display;
        private VideoMode mode = VideoMode.TEXTMODE_80x25;
        private byte[] fontMap = new byte[0x8001];
		private byte[] videoMemory = new byte[4000];
		private uint[] colors = new uint[]{0xFF000000, 0xFF000080, 0xFF008000, 0xFF008080, 0xFF800000, 
			0xFF800080, 0xFF808000, 0xFFAAAAAA, 0xFF0000FF, 0xFF00FF00, 0xFF00FFFF, 0xFFFF0000, 0xFFFF00FF, 0xFFFFFF00, 0xFFFFFFFF, 0xFFFFFFFF};
		private int cursorPosition = 0;
		private bool enableCursor = false;
        private Stream videoFirmware;
		public DeviceTextScreen (IDisplay display)
		{
			this.display = display;
		}
		
		public override void Init (Emulator emu)
		{
            videoFirmware = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("GruntXProductions.Quasar.VM.ROM.video.bin");
			emu.PeripheralController.RequestIOPort(this, 0x101);
			emu.PeripheralController.RequestIOPort(this, 0x102);
            emu.Memory.MapRegion(new DeviceMappedRegion(0xFFA00000, 0xFFC3FFFF, writeCallback, readCallback));
            emu.Memory.MapRegion(new DeviceMappedRegion(0xFFC40000, 0xFFCFFFFF, writeFontData, readFontData));
            emu.Memory.MapRegion(new DeviceMappedRegion(0xFFD00000, 0xFFD0FFFF, null, readVideoRom));
        }
		
		public override void RecieveData (int port, uint data)
		{
			switch(port)
			{
            case 0x100:
                this.mode = (VideoMode)data;
                break;
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
            return 0;
		}
		
		private void writeCallback(uint address, byte data)
		{
            int pos = (int)(address - 0xFFA00000);
			int pos2 = pos / 2;
            videoMemory[pos] = data;
            if(mode == VideoMode.TEXTMODE_80x25)
            {
                if (address % 2 == 0)
                    drawCharacter((char)data, videoMemory[pos + 1], pos2 % 80, pos2 / 80);
                else
                    drawCharacter((char)videoMemory[pos - 1], data, pos2 % 80, pos2 / 80);
            }
		}

		private void readCallback(uint address, ref byte data)
		{
		}

        private void writeFontData(uint address, byte data)
        {
            int pos = (int)(address - 0xFFC40000);
            fontMap[pos] = data;
        }

        private void readFontData(uint address, ref byte data)
        {
            int pos = (int)(address - 0xFFC40000);
            data = fontMap[pos];
        }

        private void readVideoRom(uint address, ref byte data)
        {
            int pos = (int)(address - 0xFFD00000);
            videoFirmware.Seek(pos, SeekOrigin.Begin);
            data = (byte)videoFirmware.ReadByte();
          
        }

		private void drawCharacter(char c, byte attributes, int cx, int cy)
		{
            Console.WriteLine(c);
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

