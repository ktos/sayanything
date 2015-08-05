using Microsoft.Xna.Framework;
using System;
using System.Windows.Threading;

namespace Ktos.WindowsPhone
{
	public class XnaFakeLoop
	{
		private DispatcherTimer dt;
		public TimeSpan Interval
		{
			get
			{
				return this.dt.Interval;
			}

			set
			{
				this.dt.Interval = value;
			}
		}

		public XnaFakeLoop()
		{
			dt = new DispatcherTimer();
			this.Interval = TimeSpan.FromMilliseconds(50);
			dt.Tick += (s, a) =>
			{
				try
				{
					FrameworkDispatcher.Update();
				}
				catch
				{
				}
			};
		}

		public void Start()
		{
			dt.Start();
		}

		public void Stop()
		{
			dt.Stop();
		}
	}
}
