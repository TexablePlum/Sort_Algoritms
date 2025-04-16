using Sort_Algoritms.Algorithms;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sort_Algoritms
{
	/// <summary>
	/// Implements the Selection Sort algorithm for sorting lines based on their height.
	/// The algorithm repeatedly selects the smallest element from the unsorted portion
	/// of the list and swaps it with the first unsorted element. Visual cues are used for
	/// comparisons and swaps, and a green fill animation is triggered upon completion.
	/// </summary>
	public class SelectionSort : SortAlgorithm
	{
		/// <summary>
		/// Gets the name of the algorithm.
		/// </summary>
		public override string Name { get => "SELECTION_SORT"; }

		/// <summary>
		/// Sorts the list of lines using Selection Sort.
		/// For each position in the list, the algorithm finds the smallest element from the unsorted portion,
		/// and if needed, swaps it with the element at the current position. Colors and delays are used to
		/// visualize the process.
		/// </summary>
		/// <param name="lines">List of Line objects to sort.</param>
		/// <param name="delay">Delay in milliseconds between operations for visualization.</param>
		/// <param name="token">Cancellation token to allow stopping the sort.</param>
		/// <returns>A Task representing the asynchronous sorting operation.</returns>
		public override async Task Sort(List<Line> lines, int delay, CancellationToken token = default)
		{
			// Mark the sorting process as active.
			IsSorting = true;
			int n = lines.Count;

			// Iterate over the list, treating the portion to the left of index i as sorted.
			for (int i = 0; i < n - 1; i++)
			{
				token.ThrowIfCancellationRequested();
				int minIndex = i;

				// Search the unsorted portion for the smallest element.
				for (int j = i + 1; j < n; j++)
				{
					token.ThrowIfCancellationRequested();
					if (lines[j].Height < lines[minIndex].Height)
					{
						minIndex = j;
					}
				}

				// If the smallest element found is not at the current position, swap them.
				if (minIndex != i)
				{
					// Visualize the swap by highlighting both elements.
					lines[i].Color = swapColor;
					lines[minIndex].Color = swapColor;

					// Delay for visualization effect.
					await Task.Delay(delay, token);

					// Swap the elements in the list.
					(lines[i], lines[minIndex]) = (lines[minIndex], lines[i]);
					// Update their positions on the screen.
					SwapPoints(lines[i], lines[minIndex]);

					// Reset their colors to default.
					lines[i].Color = defaultColor;
					lines[minIndex].Color = defaultColor;
				}
			}

			// Animate the completion of the sort by filling the lines with green.
			await FillLinesGreenAnimation(lines);
		}
	}
}
