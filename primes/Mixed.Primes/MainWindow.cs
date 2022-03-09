using System;
using System.Linq;
using System.Drawing.Printing;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Kw.Common;
using Xceed.Wpf.Toolkit;

namespace Mixed.Primes
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private void OnSpin(object sender, SpinEventArgs e)
		{
			if (e.Direction == SpinDirection.Increase)
			{
				Value++;
			}

			else if (Value > 2)
			{
				Value--;
			}
		}

		private void OnDraw(object sender, RoutedEventArgs e)
		{
			var entry = Entry.Get(Value);

			if (null == entry)
			{
				entry = new Entry(Value, Color);
				Entries.Items.Add(entry.Label);
			}
			else
			{
				entry.Color = Color;
			}

			Entries.Visibility = Visibility.Visible;

			var display = new VirtualDisplay(Canvas);

			if (DefaultColors.Length > Entry.Entries.Length)
			{
				Color = DefaultColors[Entry.Entries.Length];
			}
		}

		private void OnClear(object sender, RoutedEventArgs e)
		{
			Entries.Items.Clear();
			Entry.Clear();

			Color = DefaultColors[0];
			Entries.Visibility = Visibility.Hidden;

			var display = new VirtualDisplay(Canvas);
		}

		private void OnCompute(object sender, RoutedEventArgs e)
		{
		}

		private void OnSelectEntry(object sender, SelectionChangedEventArgs e)
		{
			var item = SelectedItems.FirstOrDefault();

			if (null != item)
			{
				var entry = (Entry)item.Tag;

				Color = entry.Color;
				Value = entry.Value;
			}
		}

		private void OnCanvasResize(object sender, SizeChangedEventArgs e)
		{
			var display = new VirtualDisplay(Canvas);
		}

		private void OnNext(object sender, RoutedEventArgs e)
		{
			var v = Value + 1;
			for (;; v++)
			{
				if(v.IsPrime())
					break;
			}

			Value = v;
		}
	}
}
