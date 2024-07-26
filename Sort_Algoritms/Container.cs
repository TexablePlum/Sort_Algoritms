using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace Sort_Algoritms
{
	public class Container
	{
		private Vector2 resolution;
		private int frame_margin;
		private int lines_count;
		private int lines_margin;

		private List<Line> lines;

		public List<Line> Lines { get => lines; set => lines = value; }

		public Container(Vector2 resolution, int frame_margin, int lines_count, int lines_margin)
		{
			this.resolution = resolution;
			this.frame_margin = frame_margin;
			this.lines_count = lines_count;
			this.lines_margin = lines_margin;
		}

		public void Load()
		{
			lines = new List<Line>();
			Vector2 start_point = new Vector2(frame_margin, resolution.Y - frame_margin); // Początkowy punkt (x, y)
			float start_height = 1; // Wysokość pierwszej linii
			float final_height = resolution.Y - (2 * frame_margin); // Wysokość ostatniej linii
			float thickness = Thickness_Calculations(); // Grubość linii
			Color line_color = Color.White;

			// Oblicz różnicę wysokości pomiędzy liniami
			float heightStep = (final_height - start_height) / (lines_count - 1);

			for (int i = 0; i < lines_count; i++)
			{
				float currentHeight = start_height + (heightStep * i);
				lines.Add(new Line(start_point, thickness, currentHeight, line_color));
				start_point.X += thickness + lines_margin; // Przesuń punkt początkowy o szerokość linii + odstęp
			}
		}

		public void Draw()
		{
			foreach (var line in lines)
			{
				line.Draw();
			}
		}

		private float Thickness_Calculations()
		{
			return (resolution.X - 2 * frame_margin - lines_margin * (lines_count - 1)) / lines_count;
		}

	}
}
