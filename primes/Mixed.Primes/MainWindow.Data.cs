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
		private static Color[] DefaultColors => new[]
		{
			Colors.HotPink,
			Colors.CornflowerBlue,
			Colors.LightSeaGreen,
			Colors.DarkOrange,
			Colors.DarkMagenta,
			Colors.DarkBlue,
			Colors.DarkGreen
		};

		public MainWindow()
		{
			InitializeComponent();

			Color = DefaultColors[0];
		}

		private int Value
		{
			get { return Convert.ToInt32(NewValue.Content); }
			set { NewValue.Content = value; }
		}

		private Color Color
		{
			get { return NewColor.SelectedColor.GetValueOrDefault(); }
			set { NewColor.SelectedColor = value; }
		}

		private Label[] SelectedItems =>
			Entries.SelectedItems.Cast<Label>().ToArray();
	}
}
