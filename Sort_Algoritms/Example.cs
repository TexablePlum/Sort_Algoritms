using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sort_Algoritms
{
	public class Example : Sort_Algorithms
	{
		public async override void Sort(List<Line> lines)
		{
			bool swapped;

			for (int i = 0; i < lines.Count - 1; i++)
			{
				swapped = false;
				for (int j = 0; j < lines.Count - i - 1; j++)
				{
					// Ustawienie koloru porównywanych linii na czerwono
					lines[j].Color = Color.Red;
					lines[j + 1].Color = Color.Red;

					// Odczekaj chwilę, aby zmiana koloru była widoczna
					await Task.Delay(100);
					

					// Porównanie dwóch sąsiednich elementów
					if (lines[j].Height > lines[j + 1].Height)
					{
						// Zamiana miejscami elementów listy
						(lines[j], lines[j + 1]) = (lines[j + 1], lines[j]);

						// Zamiana miejscami wartości Point
						SwapPoints(lines[j], lines[j + 1]);

						swapped = true;
					}
					// Przywrócenie oryginalnego koloru
					lines[j].Color = Color.White;
					await Task.Delay(100);
					lines[j + 1].Color = Color.White;
				}
				// Jeśli wewnątrz pętli nie było zamiany, lista jest już posortowana
				if (!swapped)
					break;
			}
		}
	}
}
