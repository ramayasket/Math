using System;
using System.Linq;
using System.Drawing.Printing;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace Mixed.Primes
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private void OnPrint(object sender, RoutedEventArgs e)
		{
			var dialogue = new PrintDialog { PrintQueue = new PrintQueue(new PrintServer(), new PrinterSettings().PrinterName) };

			var show = dialogue.ShowDialog();

			if (show.Value)
			{
				var capabilities = dialogue.PrintQueue.GetPrintCapabilities(dialogue.PrintTicket);

				var area = capabilities.PageImageableArea;

				if (null != area)
				{
					var width = area.ExtentWidth - area.OriginWidth;
					var height = area.ExtentHeight - area.OriginHeight;

					var visual = new Canvas
					{
						Width = width,
						Height = height
					};

					visual.BeginInit();

					visual.Width = width;
					visual.Height = height;

					visual.Measure(new Size(width, height));
					visual.Arrange(new Rect(0, 0, width, height));

					var display = new VirtualDisplay(visual);

					visual.EndInit();

					var scale = Math.Min(area.ExtentWidth / visual.ActualWidth, area.ExtentHeight / visual.ActualHeight);

					visual.LayoutTransform = new ScaleTransform(scale, scale);

					var size = new Size(area.ExtentWidth, area.ExtentHeight);

					visual.Measure(size);
					visual.Arrange(new Rect(new Point(area.OriginWidth, area.OriginHeight), size));

					dialogue.PrintVisual(visual, "PrimeNumberDisplay");
				}
			}
		}
	}
}
