/*
 * SayAnything
 *
 * Copyright (C) Marcin Badurowicz 2012-2015
 *
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files
 * (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge,
 * publish, distribute, sublicense, and/or sell copies of the Software,
 * and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
 * BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
 * ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE. 
 */
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
