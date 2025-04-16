using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sort_Algoritms.Algorithms;

namespace Sort_Algoritms
{
	/// <summary>
	/// Implements the Combo Sort algorithm, a hybrid sorting method combining gap-based
	/// comparisons (like in comb sort) with bubble sort-like swaps.
	/// The algorithm gradually reduces the gap between compared elements by a shrink factor,
	/// performing swaps when needed, until the gap is 1 and no more swaps occur.
	/// </summary>
	public class ComboSort : SortAlgorithm
	{
		/// <summary>
		/// Gets the name of the algorithm.
		/// </summary>
		public override string Name => "COMBO_SORT";

		/// <summary>
		/// Sorts the given list of lines using the Combo Sort algorithm.
		/// It uses a gap that starts as the length of the list and is gradually reduced by a shrink factor.
		/// Each comparison uses color changes to visualize the process; once sorting is complete,
		/// a green animation is triggered.
		/// </summary>
		/// <param name="lines">List of Line objects to sort.</param>
		/// <param name="delay">Delay (in milliseconds) between operations to control visualization speed.</param>
		/// <param name="token">Cancellation token to allow interruption of the sort.</param>
		/// <returns>A Task representing the asynchronous sorting operation.</returns>
		public override async Task Sort(List<Line> lines, int delay, CancellationToken token = default)
		{
			// Mark sorting as active.
			IsSorting = true;

			// Total number of elements in the list.
			int n = lines.Count;
			// Start with a gap equal to the number of elements.
			int gap = n;
			// Initially assume that a swap has been made.
			bool swapped = true;
			// The shrink factor used to reduce the gap each pass.
			const double shrinkFactor = 1.3;

			// Continue sorting until the gap is 1 and no swaps are made.
			while (gap > 1 || swapped)
			{
				// Check for cancellation request at the beginning of each iteration.
				token.ThrowIfCancellationRequested();

				// Reduce the gap by the shrink factor.
				gap = (int)(gap / shrinkFactor);
				if (gap < 1)
					gap = 1;

				swapped = false;

				// Loop through the list, comparing elements that are 'gap' positions apart.
				for (int i = 0; i + gap < n; i++)
				{
					token.ThrowIfCancellationRequested();

					// Set both elements' color to swapColor for visualization of the comparison.
					lines[i].Color = swapColor;
					lines[i + gap].Color = swapColor;

					// Wait for the specified delay to visually observe the comparison.
					await Task.Delay(delay, token);

					// If the element at the current index is taller than the one gap positions ahead, swap them.
					if (lines[i].Height > lines[i + gap].Height)
					{
						// Swap the elements in the list.
						(lines[i], lines[i + gap]) = (lines[i + gap], lines[i]);
						// Swap the positions visually on the screen.
						SwapPoints(lines[i], lines[i + gap]);
						// Mark that a swap occurred.
						swapped = true;
					}

					// Reset both elements' color to the default color after the comparison.
					lines[i].Color = defaultColor;
					lines[i + gap].Color = defaultColor;
				}
			}
			// After sorting, run a green fill animation to indicate the completed sort.
			await FillLinesGreenAnimation(lines);
		}
	}
}
