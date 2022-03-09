using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Kw.Common;

namespace Mixed.Primes
{
	public class Entry
	{
		private Color _color;

		public int Value { get; set; }

		public Color Color
		{
			get { return _color; }
			set { _color = value; Label.Foreground = new SolidColorBrush(_color); }
		}

		public Label Label { get; }

		public Entry(int value, Color color)
		{
			Value = value;
			Label = new Label { Content = $"N = {Value}", Tag = this, FontWeight = FontWeights.Black };
			Color = color;

			_entries[value] = this;
		}

		private static readonly Dictionary<int, Entry> _entries = new Dictionary<int, Entry>();

		public static Entry[] Entries => _entries.Values.OrderBy(e => e.Value.IsPrime()).ThenBy(e => e.Value).ToArray();

		public static Entry Get(int value)
		{
			return _entries.ValueOrDefault(value);
		}

		public static void Clear()
		{
			_entries.Clear();
		}
	}
}