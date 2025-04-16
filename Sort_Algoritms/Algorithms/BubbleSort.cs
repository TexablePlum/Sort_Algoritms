using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sort_Algoritms.Algorithms
{
	/// <summary>
	/// Implements the Bubble Sort algorithm for sorting lines based on their Height.
	/// During sorting, it updates the colors of the lines to visualize comparisons and swaps,
	/// and finally fills the sorted lines with an animation.
	/// </summary>
	public class BubbleSort : SortAlgorithm
	{
		/// <summary>
		/// Gets the name of the algorithm.
		/// </summary>
		public override string Name { get => "BUBBLE_SORT"; }

		/// <summary>
		/// Sorts the given list of lines using the bubble sort algorithm.
		/// Each comparison is visualized by temporarily changing line colors.
		/// After sorting is complete (or no swaps occur), a green animation is triggered.
		/// </summary>
		/// <param name="lines">The list of Line objects to sort.</param>
		/// <param name="delay">Delay in milliseconds between each comparison/swap for visualization.</param>
		/// <param name="token">Cancellation token for stopping the sort.</param>
		/// <returns>An asynchronous Task representing the sort process.</returns>
		public async override Task Sort(List<Line> lines, int delay, CancellationToken token = default)
		{
			// Mark the sorting flag as active.
			IsSorting = true;
			bool swapped;

			// Outer loop: iterate through all elements (except the last already sorted ones).
			for (int i = 0; i < lines.Count - 1; i++)
			{
				swapped = false;
				// Inner loop: compare each adjacent pair in the unsorted section.
				for (int j = 0; j < lines.Count - i - 1; j++)
				{
					// Check if cancellation has been requested.
					token.ThrowIfCancellationRequested();

					// Visualize the comparison by setting both lines' color to swapColor.
					lines[j].Color = swapColor;
					lines[j + 1].Color = swapColor;

					// Compare the heights of the adjacent lines.
					if (lines[j].Height > lines[j + 1].Height)
					{
						// Delay to make the swap visible in the visualization.
						await Task.Delay(delay, token);

						// Swap the two lines in the list.
						(lines[j], lines[j + 1]) = (lines[j + 1], lines[j]);

						// Visually swap the positions of the two lines on the screen.
						SwapPoints(lines[j], lines[j + 1]);

						// Indicate that a swap occurred.
						swapped = true;
					}

					// Reset the colors to the default after comparison.
					lines[j].Color = defaultColor;
					lines[j + 1].Color = defaultColor;
				}
				// If no swaps occurred in this pass, then the list is already sorted.
				if (!swapped)
				{
					// Execute a green fill animation to signify completion.
					await FillLinesGreenAnimation(lines);
					return;
				}
			}
			// Once the algorithm is done, run the green fill animation.
			await FillLinesGreenAnimation(lines);
		}
	}
}
