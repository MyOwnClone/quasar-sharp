using System;

namespace GruntXProductions.Quasar.VM
{
	public interface IDisplay
	{
		void SetPixel(int x, int y, int color);
		int GetPixel(int x, int y);
		Resolution GetResolution();
		void SetResolution(Resolution res);
	}
}

