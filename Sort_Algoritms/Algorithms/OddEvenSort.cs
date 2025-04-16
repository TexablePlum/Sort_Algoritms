using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Sort_Algoritms.Algorithms;

namespace Sort_Algoritms
{
	/// <summary>
	/// Implements the Odd-Even Sort algorithm (also known as Brick Sort) for sorting lines by height.
	/// The algorithm repeatedly performs two passes:
	/// one on odd-indexed pairs and one on even-indexed pairs, until the list is sorted.
	/// Color changes during comparisons and swaps are used for visualization.
	/// </summary>
	public class OddEvenSort : SortAlgorithm
	{
		/// <summary>
		/// Gets the name of the algorithm.
		/// </summary>
		public override string Name => "ODD_EVEN_SORT";

		/// <summary>
		/// Sorts the list of lines using the Odd-Even Sort algorithm.
		/// The algorithm alternates between comparing odd-index pairs and even-index pairs,
		/// swapping elements if they are out of order. A delay and color changes are applied
		/// to visualize the sorting process. When the list is sorted, a green fill animation is triggered.
		/// </summary>
		/// <param name="lines">List of Line objects to sort.</param>
		/// <param name="delay">Delay in milliseconds between comparisons/swap steps for visualization.</param>
		/// <param name="token">Cancellation token to allow interruption of the sorting process.</param>
		/// <returns>A Task representing the asynchronous sorting operation.</returns>
		public override async Task Sort(List<Line> lines, int delay, CancellationToken token = default)
		{
			bool sorted = false;
			int n = lines.Count;

			// Keep iterating until no swaps occur, which means the list is sorted.
			while (!sorted)
			{
				token.ThrowIfCancellationRequested();
				IsSorting = true;
				sorted = true;

				// -------- Odd-index pass --------
				// Process pairs starting at index 1 (odd-indexed), comparing elements at positions i and i+1.
				for (int i = 1; i <= n - 2; i += 2)
				{
					token.ThrowIfCancellationRequested();

					// Highlight the two compared lines by setting them to the swapColor.
					lines[i].Color = swapColor;
					lines[i + 1].Color = swapColor;

					// Wait to allow the visual comparison.
					await Task.Delay(delay, token);

					// If the element at i has a greater Height than the element at i+1, swap them.
					if (lines[i].Height > lines[i + 1].Height)
					{
						(lines[i], lines[i + 1]) = (lines[i + 1], lines[i]);
						// Visually swap their positions.
						SwapPoints(lines[i], lines[i + 1]);
						// Indicate that a swap occurred.
						sorted = false;
					}

					// Reset their colors back to the default after comparison.
					lines[i].Color = defaultColor;
					lines[i + 1].Color = defaultColor;
				}

				// -------- Even-index pass --------
				// Process pairs starting at index 0 (even-indexed), comparing elements at positions i and i+1.
				for (int i = 0; i <= n - 2; i += 2)
				{
					token.ThrowIfCancellationRequested();

					// Highlight the two compared lines.
					lines[i].Color = swapColor;
					lines[i + 1].Color = swapColor;

					// Wait for the specified delay.
					await Task.Delay(delay, token);

					// Swap elements if the left one is higher than the right one.
					if (lines[i].Height > lines[i + 1].Height)
					{
						(lines[i], lines[i + 1]) = (lines[i + 1], lines[i]);
						// Update visual positions for the swapped elements.
						SwapPoints(lines[i], lines[i + 1]);
						sorted = false;
					}

					// Reset their colors after the comparison.
					lines[i].Color = defaultColor;
					lines[i + 1].Color = defaultColor;
				}
			}
			// Once sorted, run the green fill animation to signal completion.
			await FillLinesGreenAnimation(lines);
		}
	}
}
