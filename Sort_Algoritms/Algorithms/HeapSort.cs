using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Sort_Algoritms.Algorithms;

namespace Sort_Algoritms
{
	/// <summary>
	/// Implements the Heap Sort algorithm for sorting lines based on their height.
	/// The algorithm first builds a max-heap from the list and then repeatedly extracts 
	/// the maximum element, swapping it with the last element in the heap and re-heapifying.
	/// Visual cues (color changes) are applied during comparisons and swaps to facilitate visualization.
	/// </summary>
	public class HeapSort : SortAlgorithm
	{
		/// <summary>
		/// Gets the name of the algorithm.
		/// </summary>
		public override string Name { get => "HEAP_SORT"; }

		/// <summary>
		/// Sorts the list of lines using Heap Sort.
		/// The process consists of building a max-heap, then repeatedly swapping the heap's root 
		/// with the last element, reducing the heap size, and re-heapifying.
		/// Color changes are used to indicate which lines are being swapped.
		/// </summary>
		/// <param name="lines">List of Line objects to be sorted.</param>
		/// <param name="delay">Delay in milliseconds between operations to slow down the visualization.</param>
		/// <param name="token">Cancellation token to interrupt the sort if needed.</param>
		/// <returns>A Task representing the asynchronous sorting operation.</returns>
		public override async Task Sort(List<Line> lines, int delay, CancellationToken token = default)
		{
			// Mark the sorting process as active.
			IsSorting = true;
			int n = lines.Count;

			// --------- BUILD THE MAX-HEAP ---------
			// Start from the last non-leaf node and heapify each node up to the root.
			for (int i = n / 2 - 1; i >= 0; i--)
			{
				token.ThrowIfCancellationRequested();
				await Heapify(lines, n, i, delay, token);
			}

			// --------- EXTRACT ELEMENTS FROM THE HEAP ---------
			// Swap the first element (largest) with the last element and reduce the heap size.
			for (int i = n - 1; i > 0; i--)
			{
				token.ThrowIfCancellationRequested();

				// Visualize the swap: highlight the first element and the element at i.
				lines[0].Color = swapColor;
				lines[i].Color = swapColor;
				// Delay to allow visualization of the swap.
				await Task.Delay(delay, token);

				// Swap the root (largest element) with the element at index i.
				(lines[0], lines[i]) = (lines[i], lines[0]);
				SwapPoints(lines[0], lines[i]);

				// Reset the colors after the swap.
				lines[0].Color = defaultColor;
				lines[i].Color = defaultColor;

				// Heapify on the reduced heap (size i) to maintain the max-heap property.
				await Heapify(lines, i, 0, delay, token);
			}

			// After the sorting is complete, execute the green animation to indicate completion.
			await FillLinesGreenAnimation(lines);
		}

		/// <summary>
		/// Heapifies a subtree rooted at index i in the list of lines.
		/// Ensures the subtree satisfies the max-heap property.
		/// </summary>
		/// <param name="lines">The list of Line objects representing the heap.</param>
		/// <param name="n">The effective size of the heap.</param>
		/// <param name="i">The index of the current root of the subtree.</param>
		/// <param name="delay">Delay in milliseconds for visualization.</param>
		/// <param name="token">Cancellation token for stopping the operation.</param>
		/// <returns>A Task representing the asynchronous heapify operation.</returns>
		private async Task Heapify(List<Line> lines, int n, int i, int delay, CancellationToken token)
		{
			token.ThrowIfCancellationRequested();

			int largest = i;       // Initialize largest as root.
			int left = 2 * i + 1;  // Left child index.
			int right = 2 * i + 2; // Right child index.

			// If left child exists and is greater than the current largest, update largest.
			if (left < n && lines[left].Height > lines[largest].Height)
				largest = left;

			// Similarly, if right child exists and is greater than the current largest, update largest.
			if (right < n && lines[right].Height > lines[largest].Height)
				largest = right;

			// If the largest element is not the root, swap and continue heapifying.
			if (largest != i)
			{
				// Highlight the current node and the largest child to visualize the swap.
				lines[i].Color = swapColor;
				lines[largest].Color = swapColor;
				await Task.Delay(delay, token);

				// Swap the elements in the list.
				(lines[i], lines[largest]) = (lines[largest], lines[i]);
				// Update their positions on the screen.
				SwapPoints(lines[i], lines[largest]);

				// Reset colors to default after swapping.
				lines[i].Color = defaultColor;
				lines[largest].Color = defaultColor;

				// Recursively heapify the affected subtree.
				await Heapify(lines, n, largest, delay, token);
			}
		}
	}
}
