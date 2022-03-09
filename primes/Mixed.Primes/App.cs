using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Kw.Common;
using Kw.Common.Threading;

namespace Mixed.Primes
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public class App : Application
	{
		[STAThread]
		public static void Main(string[] args)
		{
			//
			//	Suppress data binding errors
			//
			PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Critical;

			//
			//	Prime numbers to factorize numbers up to int.MaxValue
			//
			var max = int.MaxValue;
			var prime = max.IsPrime();

			var app = new App();
			var main = new MainWindow();

			app.Run(main);

			AppCore.Stop();
		}
	}
}
