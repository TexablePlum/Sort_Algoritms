using System.Collections.Generic;

namespace Sort_Algoritms
{
	public abstract class Sort_Algorithms
	{
		public abstract void Sort(List<Line> lines);
		protected virtual void SwapPoints(Line line1, Line line2)
		{
			// Zamiana miejscami wartości Point obiektów Line
			(line2.Point, line1.Point) = (line1.Point, line2.Point);

			/*
			 * Vector2 temp = line1.Point;
			 * line1.Point = line2.Point;
			 * line2.Point = temp;
			*/
		}
	}
}
