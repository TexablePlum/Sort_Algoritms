using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sort_Algoritms.Algorithms
{
	/// <summary>
	/// Abstract base class for sorting algorithms used in the visualization application.
	/// Contains common fields for colors used during sorting animations and provides
	/// base implementations for common functionality (swapping, stop, and post-sort animation).
	/// </summary>
	public abstract class SortAlgorithm
	{
		// Color used to indicate a swap operation.
		public readonly Color swapColor = Color.Red;
		// Default color for lines that are not being modified.
		public readonly Color defaultColor = Color.White;
		// Color used for the animation when "filling" lines (e.g., indicating sorted state).
		public readonly Color animationColor = new Color(20, 255, 9, 255);

		// Abstract property to define the algorithm's name.
		public abstract string Name { get; }

		// Flag indicating if a sorting process is currently running.
		public bool IsSorting { get; set; } = false;
		// Flag indicating if the lines are in the 'green animation' state.
		public bool IsFeelingLinesGreen { get; set; } = false;

		// Token source for cancelling the sorting process.
		public CancellationTokenSource CancelTokenSource { get; set; } = new CancellationTokenSource();

		/// <summary>
		/// Abstract method that performs the sorting on a list of Line objects.
		/// Must be implemented by derived classes.
		/// </summary>
		/// <param name="lines">List of lines representing the elements to sort.</param>
		/// <param name="delay">Delay in milliseconds between sorting steps to allow visualization.</param>
		/// <param name="token">Cancellation token to allow stopping the sort.</param>
		/// <returns>A Task that represents the asynchronous sorting operation.</returns>
		public abstract Task Sort(List<Line> lines, int delay = 0, CancellationToken token = default);

		/// <summary>
		/// Swaps the visual positions (points) of two lines.
		/// This method changes the Point property of both lines to reflect the swap.
		/// </summary>
		/// <param name="line1">The first Line object.</param>
		/// <param name="line2">The second Line object.</param>
		protected virtual void SwapPoints(Line line1, Line line2)
		{
			// Swaps the Point property of the two lines.
			(line2.Point, line1.Point) = (line1.Point, line2.Point);

			/*
             * Alternatively, without tuple swap:
             * Vector2 temp = line1.Point;
             * line1.Point = line2.Point;
             * line2.Point = temp;
            */
		}

		/// <summary>
		/// Runs an animation that fills the lines with the animation color (e.g., green) to indicate completion.
		/// Each line is colored sequentially with a short delay, then all are reset to the default color after a pause.
		/// </summary>
		/// <param name="lines">List of lines to animate.</param>
		/// <returns>A Task representing the asynchronous animation operation.</returns>
		protected virtual async Task FillLinesGreenAnimation(List<Line> lines)
		{
			// Set the flag to indicate that the animation is active.
			IsFeelingLinesGreen = true;
			// Iterate over each line and set its color to animationColor with a brief delay.
			foreach (var line in lines)
			{
				line.Color = animationColor;
				await Task.Delay(1);
			}

			// Pause for 1 second to let the user observe the animation.
			await Task.Delay(1000);

			// Reset each line's color back to the default color.
			foreach (var line in lines)
			{
				line.Color = defaultColor;
			}
			// Reset flags once the animation is complete.
			IsFeelingLinesGreen = false;
			IsSorting = false;
		}

		/// <summary>
		/// Stops the current sorting process by cancelling the provided token and resetting the line colors.
		/// </summary>
		/// <param name="lines">List of lines to reset after stopping the sort.</param>
		public virtual void StopSort(List<Line> lines)
		{
			// Cancel the sorting process via the cancellation token.
			CancelTokenSource?.Cancel();
			// Reset each line to the default color.
			foreach (var line in lines)
			{
				line.Color = defaultColor;
			}
			// Mark sorting as no longer running.
			IsSorting = false;
		}
	}
}
