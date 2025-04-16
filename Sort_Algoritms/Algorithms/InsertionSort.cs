using Sort_Algoritms.Algorithms;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sort_Algoritms
{
	/// <summary>
	/// Implements the Insertion Sort algorithm for sorting lines based on their height.
	/// During the sort, comparisons and swaps are visualized by temporarily changing the colors of the lines.
	/// Once sorting is complete, a green fill animation is triggered.
	/// </summary>
	public class InsertionSort : SortAlgorithm
	{
		/// <summary>
		/// Gets the name of the algorithm.
		/// </summary>
		public override string Name { get => "INSERTION_SORT"; }

		/// <summary>
		/// Sorts a list of lines using the Insertion Sort algorithm.
		/// It iterates through the list starting from the second element, inserting each element into its correct position in the sorted portion.
		/// Color changes and delays are used to visualize comparisons and swaps.
		/// </summary>
		/// <param name="lines">The list of Line objects to sort.</param>
		/// <param name="delay">Delay in milliseconds to pause between operations for visualization.</param>
		/// <param name="token">Cancellation token to allow stopping the sort.</param>
		/// <returns>A Task representing the asynchronous sorting operation.</returns>
		public override async Task Sort(List<Line> lines, int delay, CancellationToken token = default)
		{
			// Mark the sorting process as active.
			IsSorting = true;
			int n = lines.Count;

			// Begin by iterating from the second element to the end of the list.
			for (int i = 1; i < n; i++)
			{
				// Check for cancellation before each insertion operation.
				token.ThrowIfCancellationRequested();
				int j = i;

				// Insert the element at index i into the sorted portion (to the left of i).
				while (j > 0 && lines[j - 1].Height > lines[j].Height)
				{
					token.ThrowIfCancellationRequested();

					// Visualize comparison by changing the color of the lines being compared.
					lines[j].Color = swapColor;
					lines[j - 1].Color = swapColor;

					// Pause to allow the visualization of the comparison.
					await Task.Delay(delay, token);

					// Swap the current line with the previous line.
					(lines[j], lines[j - 1]) = (lines[j - 1], lines[j]);
					// Update the positions visually on screen.
					SwapPoints(lines[j], lines[j - 1]);

					// Reset the colors to default after the swap.
					lines[j].Color = defaultColor;
					lines[j - 1].Color = defaultColor;
					// Move one step left in the sorted portion.
					j--;
				}
			}
			// Once sorting is complete, animate the lines with a green fill effect.
			await FillLinesGreenAnimation(lines);
		}
	}
}
