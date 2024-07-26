using System;
using System.Collections.Generic;

namespace Sort_Algoritms
{
	public class Randomize_Shuffle : Sort_Algorithms
	{
		public override void Sort(List<Line> lines)
		{
			Random random = new Random();

			// Tasowanie elementów listy lines
			for (int i = 0; i < lines.Count; i++)
			{
				int j = random.Next(0, i + 1);
				// Zamiana miejscami elementów listy
				(lines[j], lines[i]) = (lines[i], lines[j]);

				// Zamiana miejscami wartości Point
				SwapPoints(lines[i], lines[j]);
			}
		}

		
	}
}
