using Sort_Algoritms.Algorithms;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Sort_Algoritms
{
	/// <summary>
	/// Implements the Quick Sort algorithm for sorting lines based on their height.
	/// This algorithm partitions the list around a pivot element and recursively sorts
	/// the subarrays. Visual cues (color changes and delays) help to animate comparisons
	/// and swaps. Once sorting completes, a green fill animation is triggered.
	/// </summary>
	public class QuickSort : SortAlgorithm
	{
		/// <summary>
		/// Gets the name of the algorithm.
		/// </summary>
		public override string Name { get => "QUICK_SORT"; }

		/// <summary>
		/// Initiates the QuickSort algorithm on the list of lines.
		/// After the sorting process is complete, triggers the green animation.
		/// </summary>
		/// <param name="lines">List of Line objects to sort.</param>
		/// <param name="delay">Delay in milliseconds between operations for visualization.</param>
		/// <param name="token">Cancellation token allowing the sort to be cancelled.</param>
		/// <returns>A Task representing the asynchronous sort operation.</returns>
		public override async Task Sort(List<Line> lines, int delay, CancellationToken token = default)
		{
			IsSorting = true;
			// Start the recursive quick sort on the entire array.
			await QuickSortInternal(lines, 0, lines.Count - 1, delay, token);
			// After sorting is complete, trigger a green fill animation.
			await FillLinesGreenAnimation(lines);
		}

		/// <summary>
		/// Recursively sorts the subarray defined by the indices low to high using QuickSort.
		/// </summary>
		/// <param name="lines">List of Line objects.</param>
		/// <param name="low">Starting index of the subarray.</param>
		/// <param name="high">Ending index of the subarray.</param>
		/// <param name="delay">Delay for visualization between swaps.</param>
		/// <param name="token">Cancellation token for stopping the sort.</param>
		/// <returns>A Task representing the recursive operation.</returns>
		private async Task QuickSortInternal(List<Line> lines, int low, int high, int delay, CancellationToken token)
		{
			// Ensure no cancellation has been requested.
			token.ThrowIfCancellationRequested();
			// If there is more than one element in the subarray, proceed with partitioning.
			if (low < high)
			{
				// Partition the subarray and get the pivot index.
				int pivotIndex = await Partition(lines, low, high, delay, token);
				// Recursively sort the subarray elements before the pivot.
				await QuickSortInternal(lines, low, pivotIndex - 1, delay, token);
				// Recursively sort the subarray elements after the pivot.
				await QuickSortInternal(lines, pivotIndex + 1, high, delay, token);
			}
		}

		/// <summary>
		/// Partitions the subarray into elements less than the pivot and elements greater than or equal to the pivot.
		/// The pivot is chosen as the last element in the subarray.
		/// Elements are compared using their Height value. Visual cues highlight comparisons and swaps.
		/// </summary>
		/// <param name="lines">List of Line objects.</param>
		/// <param name="low">Starting index of the subarray.</param>
		/// <param name="high">Ending index of the subarray, chosen as the pivot.</param>
		/// <param name="delay">Delay for visualization between swaps.</param>
		/// <param name="token">Cancellation token to allow interruption.</param>
		/// <returns>A Task that returns the partition index after partitioning.</returns>
		private async Task<int> Partition(List<Line> lines, int low, int high, int delay, CancellationToken token)
		{
			token.ThrowIfCancellationRequested();
			// Choose the pivot as the height of the last element.
			float pivot = lines[high].Height;
			// Initialize the partition index.
			int i = low - 1;
			// Process each element in the subarray except the pivot.
			for (int j = low; j < high; j++)
			{
				token.ThrowIfCancellationRequested();
				// If the current element is less than the pivot, it belongs to the left partition.
				if (lines[j].Height < pivot)
				{
					i++;
					// Highlight the two elements being swapped.
					lines[i].Color = swapColor;
					lines[j].Color = swapColor;
					await Task.Delay(delay, token);

					// Swap the elements in the list.
					(lines[i], lines[j]) = (lines[j], lines[i]);
					// Swap their positions for visualization.
					SwapPoints(lines[i], lines[j]);

					// Reset colors after the swap.
					lines[i].Color = defaultColor;
					lines[j].Color = defaultColor;
				}
			}
			// Place the pivot in its correct sorted position.
			lines[i + 1].Color = swapColor;
			lines[high].Color = swapColor;
			await Task.Delay(delay, token);

			// Swap the pivot element with the element at i+1.
			(lines[i + 1], lines[high]) = (lines[high], lines[i + 1]);
			SwapPoints(lines[i + 1], lines[high]);

			// Reset the colors.
			lines[i + 1].Color = defaultColor;
			lines[high].Color = defaultColor;

			// Return the pivot index.
			return i + 1;
		}
	}
}
