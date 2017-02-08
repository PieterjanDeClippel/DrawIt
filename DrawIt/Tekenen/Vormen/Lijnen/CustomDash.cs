using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrawIt
{
	public class CustomDash : Control
	{
		public CustomDash()
		{
			this.DoubleBuffered = true;
			dots.Changed += dots_Changed;
			this.MouseClick += CustomDash_MouseClick;
		}

		void CustomDash_MouseClick(object sender, MouseEventArgs e)
		{
			int w = 20;
			if((0 <= e.Location.Y) & (e.Location.Y <= 20))
			{
				int x = ((e.Location.X - e.Location.X % w) / w);
				dots[x] = !dots[x];
				this.Invalidate();
				changed = true;
			}
		}

		void dots_Changed(object sender, Collectie<bool>.CollectieChangedEventArgs e)
		{
			this.Invalidate();
			changed = true;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics gr = e.Graphics;
			int w = 20;
			for(int i = 0; i < dots.Count; i++)
			{
				if(dots[i])
					gr.FillRectangle(Brushes.Black, i * w, 0, w + 1, w + 1);
				else
					gr.DrawRectangle(Pens.LightGray, i * w, 0, w, w);
			}
		}

		private Collectie<bool> dots = new Collectie<bool>();
		public Collectie<bool> Dots
		{
			get { return dots; }
		}

		public int Lengte
		{
			get { return dots.Count; }
			set
			{
				while(dots.Count != value)
				{
					if(dots.Count < value)
						dots.Add(false);
					else
						dots.RemoveAt(dots.Count - 1);
				}
				changed = true;
			}
		}

		private DashStyle lijnstijl = DashStyle.Solid;
		public DashStyle LijnStijl
		{
			get { return lijnstijl; }
			set
			{
				lijnstijl = value;
				if(lijnstijl != DashStyle.Custom)
				{
					bool p = true;
					Pen pen = new Pen(Brushes.Black) { DashStyle = lijnstijl };
					dots.Clear();
					try
					{
						foreach(float flt in pen.DashPattern)
						{
							for(int i = 0; i < flt; i++)
								Dots.Add(p);
							p = !p;
						}
					}
					catch(Exception)
					{
						Dots.Add(true);
					}
				}
				this.Invalidate();
			}
		}

		private bool changed = false;
		public bool Changed
		{
			get { return changed; }
			set { changed = value; }
		}

	}
}
