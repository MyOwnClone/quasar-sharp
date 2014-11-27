using System;
using System.Drawing;
using System.Windows.Forms;

namespace GruntXProductions.Quasar.VM
{
	public class ScreenForm : Form, IDisplay
	{
		private Bitmap buffer;
		private Timer refreshTimer;
		
		public ScreenForm()
		{
			this.Text = "Quasar 3200";	
			this.SetResolution(new Resolution(720,  400));
			this.DoubleBuffered = true;
			this.refreshTimer = new Timer();
			this.refreshTimer.Interval = 100;
			this.refreshTimer.Tick += refreshScreen;
			this.refreshTimer.Start();
			for(int x = 0; x < this.Width; x++)
				for(int y = 0; y < this.Height; y++)
					this.buffer.SetPixel(x, y, Color.Black);
		}

		private void refreshScreen (object sender, EventArgs e)
		{
			this.Refresh();
		}
		
		public void SetPixel(int x, int y, int color)
		{
			lock(this.buffer)
			{
				buffer.SetPixel(x, y, Color.FromArgb(color));
			}
		}
		
		public int GetPixel(int x, int y)
		{
			return buffer.GetPixel(x, y).ToArgb();
		}
		
		protected override void OnPaint (PaintEventArgs e)
		{
			lock(this.buffer)
			{
				e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				e.Graphics.DrawImage(this.buffer, new Rectangle(0, 0, this.Width, this.Height), new Rectangle(Point.Empty, buffer.Size),  GraphicsUnit.Pixel);
			}
		}
		
		public Resolution GetResolution()
		{
			return new Resolution(this.Width, this.Height);
		}
		
		public void SetResolution(Resolution res)
		{
			this.Height = res.Y;
			this.Width = res.X;
			this.buffer = new Bitmap(res.X, res.Y);
		}

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ScreenForm
            // 
            this.ClientSize = new System.Drawing.Size(442, 349);
            this.Name = "ScreenForm";
            this.ResumeLayout(false);

        }
	}
}

