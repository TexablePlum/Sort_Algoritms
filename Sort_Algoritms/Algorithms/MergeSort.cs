using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Sort_Algoritms.Algorithms;

namespace Sort_Algoritms
{
	/// <summary>
	/// Implements the Merge Sort algorithm to sort lines based on their Height.
	/// This algorithm recursively divides the list into halves and then merges
	/// the sorted halves. During merging, visual cues (color changes and delays) help
	/// animate comparisons and placements.
	/// </summary>
	public class MergeSort : SortAlgorithm
	{
		/// <summary>
		/// Gets the name of the algorithm.
		/// </summary>
		public override string Name { get => "MERGE_SORT"; }

		/// <summary>
		/// Sorts the list of lines using Merge Sort.
		/// The process includes recursively dividing the list, merging sorted segments,
		/// and animating the merge process. Once sorting is complete, a green fill animation
		/// is triggered.
		/// </summary>
		/// <param name="lines">The list of Line objects to be sorted.</param>
		/// <param name="delay">Delay (in milliseconds) between operations for visualization.</param>
		/// <param name="token">Cancellation token to allow interruption of the sort.</param>
		/// <returns>A Task representing the asynchronous sorting operation.</returns>
		public override async Task Sort(List<Line> lines, int delay, CancellationToken token = default)
		{
			// Mark the sorting process as active.
			IsSorting = true;
			// Perform the recursive merge sort on the entire list.
			await MergeSortRecursive(lines, 0, lines.Count - 1, delay, token);
			// Once sorting is finished, run the green fill animation.
			await FillLinesGreenAnimation(lines);
		}

		/// <summary>
		/// Recursively divides the list into subarrays and sorts them.
		/// </summary>
		/// <param name="lines">The list of Line objects.</param>
		/// <param name="left">The starting index of the current subarray.</param>
		/// <param name="right">The ending index of the current subarray.</param>
		/// <param name="delay">Delay (in milliseconds) for visualization purposes.</param>
		/// <param name="token">Cancellation token to allow interruption.</param>
		/// <returns>A Task representing the asynchronous recursive sorting.</returns>
		private async Task MergeSortRecursive(List<Line> lines, int left, int right, int delay, CancellationToken token)
		{
			if (left < right)
			{
				// Find the middle index.
				int mid = (left + right) / 2;
				// Sort the left half.
				await MergeSortRecursive(lines, left, mid, delay, token);
				// Sort the right half.
				await MergeSortRecursive(lines, mid + 1, right, delay, token);
				// Merge the two sorted halves.
				await Merge(lines, left, mid, right, delay, token);
			}
		}

		/// <summary>
		/// Merges two sorted subarrays into a single sorted segment.
		/// Visual cues (color changes) and a delay are used to animate the merging process.
		/// </summary>
		/// <param name="lines">The list of Line objects.</param>
		/// <param name="left">Starting index of the left subarray.</param>
		/// <param name="mid">Ending index of the left subarray (and hence mid point).</param>
		/// <param name="right">Ending index of the right subarray.</param>
		/// <param name="delay">Delay (in milliseconds) for visualization.</param>
		/// <param name="token">Cancellation token for interrupting the process.</param>
		/// <returns>A Task representing the asynchronous merge operation.</returns>
		private async Task Merge(List<Line> lines, int left, int mid, int right, int delay, CancellationToken token)
		{
			// Calculate lengths of left and right segments.
			int n1 = mid - left + 1;
			int n2 = right - mid;

			// Create temporary lists for the left and right subarrays.
			List<Line> leftList = new List<Line>();
			List<Line> rightList = new List<Line>();

			for (int i = 0; i < n1; i++)
				leftList.Add(lines[left + i]);

			for (int j = 0; j < n2; j++)
				rightList.Add(lines[mid + 1 + j]);

			int iIndex = 0, jIndex = 0;
			List<Line> merged = new List<Line>();

			// Merge the two sorted lists by comparing elements.
			while (iIndex < n1 && jIndex < n2)
			{
				token.ThrowIfCancellationRequested();
				if (leftList[iIndex].Height <= rightList[jIndex].Height)
				{
					merged.Add(leftList[iIndex]);
					iIndex++;
				}
				else
				{
					merged.Add(rightList[jIndex]);
					jIndex++;
				}
			}

			// Append any remaining elements from the left list.
			while (iIndex < n1)
			{
				token.ThrowIfCancellationRequested();
				merged.Add(leftList[iIndex]);
				iIndex++;
			}

			// Append any remaining elements from the right list.
			while (jIndex < n2)
			{
				token.ThrowIfCancellationRequested();
				merged.Add(rightList[jIndex]);
				jIndex++;
			}

			// Cache the current positions of the lines in this merged segment,
			// so the visual positions can be preserved after merging.
			List<Vector2> points = new List<Vector2>();
			for (int k = left; k <= right; k++)
			{
				points.Add(lines[k].Point);
			}

			// Critical section: update the original list with sorted merged elements.
			// We do not check for cancellation during this section to ensure an atomic update.
			int mIndex = 0;
			for (int k = left; k <= right; k++)
			{
				// Visualize the placement with swapColor.
				merged[mIndex].Color = swapColor;
				// Delay to allow visualization.
				await Task.Delay(delay);
				// Replace the line in the original list with the merged line.
				lines[k] = merged[mIndex];
				// Restore the original position.
				lines[k].Point = points[k - left];
				// Reset the color back to default.
				lines[k].Color = defaultColor;
				mIndex++;
			}

			// Check for cancellation after completing the merge.
			token.ThrowIfCancellationRequested();
		}
	}
}
