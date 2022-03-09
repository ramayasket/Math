using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Kw.Common;

namespace Mixed.Primes
{
	public class VirtualDisplay
	{
		private Canvas Canvas { get; }

		private double Width { get; }
		private double Height { get; }
		private double Actuality { get; }

		private double HCenter { get; }
		private double VCenter { get; }

		private double Diameter { get; }
		private double PrimeThickness { get; }
		private double NPrimeThickness { get; }

		private double HOrigin { get; }
		private double VOrigin { get; }

		private double Font { get; }

		private double Point { get; }

		public VirtualDisplay(Canvas canvas)
		{
			Canvas = canvas;

			canvas.Children.Clear();

			Width = canvas.ActualWidth.InCase(0.0, canvas.Width);
			Height = canvas.ActualHeight.InCase(0.0, canvas.Height);

			if (Width.Equals(double.NaN) || Height.Equals(double.NaN))
				return;

			HCenter = Width/2;
			VCenter = Height / 2;
			Actuality = Math.Min(Width, Height);
			Diameter = Actuality * 0.9D;
			PrimeThickness = Actuality/250;
			NPrimeThickness = PrimeThickness/2;
			Point = Actuality/70;

			HOrigin = HCenter - Diameter/2;
			VOrigin = VCenter - Diameter / 2;
			Font = Actuality/40;

			DrawCircle();

			var entries = Entry.Entries;
			var offset = 0D;

			if (entries.Any(e => e.Value >= 20))
			{
				PrimeThickness = NPrimeThickness;
			}

			foreach (var entry in entries)
			{
				DrawEntry(entry);
				DrawLabel(entry, offset);
				offset -= Font * 1.2;
			}

			DrawPoint(HCenter, VCenter, Point, Colors.Black);
		}

		private void DrawLabel(Entry entry, double offset)
		{
			var textBlock = new TextBlock
			{
				Text = $"N = {entry.Value}",
				Foreground = new SolidColorBrush(entry.Color),
				FontWeight = FontWeights.Black,
				FontSize = Font,
			};

			textBlock.SetValue(Window.LeftProperty, HOrigin);
			textBlock.SetValue(Window.TopProperty, VOrigin - offset);

			Canvas.Children.Add(textBlock);
		}

		private void DrawEntry(Entry entry)
		{
			var points = new Point[entry.Value];

			var pi = Math.PI;
			var step = 2 * pi / entry.Value;
			var radius = (Diameter / 2) * 0.995D;

			var center = new Point(HCenter, VCenter);

			var prime = entry.Value.IsPrime();
			var thickness = prime ? PrimeThickness : NPrimeThickness;

			for (int i = 0; i < entry.Value; i++)
			{
				var angle = -(pi / 2) + step * i;

				var x = HCenter + radius * Math.Cos(angle);
				var y = VCenter + radius * Math.Sin(angle);

				var point = new Point(x, y);
				points[i] = point;

				if (prime)
				{
					DrawPoint(x, y, Point, entry.Color);
				}
			}

			for (int i = 0; i < entry.Value; i++)
			{
				var from = i;
				var to = (i + 1).InCase(entry.Value, (int)0);

				DrawLine(points[from], points[to], thickness, entry.Color);
				DrawLine(center, points[to], thickness, entry.Color);
			}
		}

		private void DrawLine(Point from, Point to, double thickness, Color color)
		{
			var line = new Line
			{
				Stroke = new SolidColorBrush(color),
				X1 = from.X,
				X2 = to.X,
				Y1 = from.Y,
				Y2 = to.Y,
				StrokeThickness = thickness
			};

			Canvas.Children.Add(line);
		}

		private void DrawCircle()
		{
			var brush = Brushes.Black;

			var ellipse = new Ellipse
			{
				StrokeThickness = PrimeThickness,
				Stroke = brush,
			};

			var d = Diameter;
			ellipse.Width = ellipse.Height = d;

			ellipse.SetValue(Window.LeftProperty, HCenter - d/2);
			ellipse.SetValue(Window.TopProperty, VCenter - d/2);

			Canvas.Children.Add(ellipse);
		}

		private void DrawPoint(double x, double y, double size, Color color)
		{
			var brush = new SolidColorBrush(color);

			var ellipse = new Ellipse
			{
				StrokeThickness = 0,
				Stroke = brush,
				Fill = brush,
			};

			ellipse.Width = ellipse.Height = size;

			ellipse.SetValue(Window.LeftProperty, x - size / 2);
			ellipse.SetValue(Window.TopProperty, y - size / 2);

			Canvas.Children.Add(ellipse);
		}
	}
}
