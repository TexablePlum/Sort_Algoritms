using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sort_Algoritms.Algorithms;

namespace Sort_Algoritms
{
	/// <summary>
	/// Implements the Cocktail Sort algorithm, also known as bidirectional bubble sort.
	/// It performs a forward pass (left-to-right) and a backward pass (right-to-left)
	/// during each iteration. Color changes during comparisons and swaps are used for visualization.
	/// When sorting is complete, a green animation is triggered.
	/// </summary>
	public class CocktailSort : SortAlgorithm
	{
		/// <summary>
		/// Gets the algorithm name.
		/// </summary>
		public override string Name => "COCKTAIL_SORT";

		/// <summary>
		/// Sorts the provided list of lines using the Cocktail Sort algorithm.
		/// The algorithm performs alternating forward and backward passes to bubble up the largest and smallest elements.
		/// Visual cues (color changes) are applied for comparisons and swaps.
		/// </summary>
		/// <param name="lines">The list of Line objects to sort.</param>
		/// <param name="delay">Delay in milliseconds between sorting steps for visualization.</param>
		/// <param name="token">Cancellation token to allow interrupting the sort process.</param>
		/// <returns>A Task representing the asynchronous sorting operation.</returns>
		public override async Task Sort(List<Line> lines, int delay, CancellationToken token = default)
		{
			// Mark the sorting process as active.
			IsSorting = true;
			// Initialize 'swapped' to true to enter the while loop.
			bool swapped = true;
			// 'start' and 'end' define the current unsorted boundaries of the list.
			int start = 0;
			int end = lines.Count - 1;

			// Continue looping until no swaps are made in both passes.
			while (swapped)
			{
				// Check for cancellation before starting this pass.
				token.ThrowIfCancellationRequested();

				// Reset the swapped flag.
				swapped = false;

				// === Forward pass (left-to-right) ===
				for (int i = start; i < end; i++)
				{
					token.ThrowIfCancellationRequested();

					// Highlight the two adjacent lines under comparison.
					lines[i].Color = swapColor;
					lines[i + 1].Color = swapColor;

					// Wait for the specified delay to make the comparison visible.
					await Task.Delay(delay, token);

					// Swap the lines if the left one is greater (by Height) than the right.
					if (lines[i].Height > lines[i + 1].Height)
					{
						(lines[i], lines[i + 1]) = (lines[i + 1], lines[i]);
						// Visually swap the positions of the two lines.
						SwapPoints(lines[i], lines[i + 1]);
						// Mark that a swap occurred.
						swapped = true;
					}

					// Reset colors to default after the comparison.
					lines[i].Color = defaultColor;
					lines[i + 1].Color = defaultColor;
				}

				// If no swaps occurred in the forward pass, the list is sorted.
				if (!swapped)
					break;

				// Prepare for the backward pass by decreasing the 'end' boundary.
				swapped = false;
				end--;

				// === Backward pass (right-to-left) ===
				for (int i = end; i > start; i--)
				{
					token.ThrowIfCancellationRequested();

					// Highlight the adjacent lines under comparison.
					lines[i].Color = swapColor;
					lines[i - 1].Color = swapColor;

					await Task.Delay(delay, token);

					// Swap if the left line is greater than the right one.
					if (lines[i - 1].Height > lines[i].Height)
					{
						(lines[i - 1], lines[i]) = (lines[i], lines[i - 1]);
						// Visually swap the positions of the two lines.
						SwapPoints(lines[i - 1], lines[i]);
						swapped = true;
					}

					// Reset colors after comparison.
					lines[i].Color = defaultColor;
					lines[i - 1].Color = defaultColor;
				}

				// Increase the start boundary for the next iteration since the smallest element is in place.
				start++;
			}

			// Once sorting is done, play the green fill animation.
			await FillLinesGreenAnimation(lines);
		}
	}
}
