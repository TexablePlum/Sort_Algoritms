using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sort_Algoritms.Algorithms;

namespace Sort_Algoritms.Algorithms
{
	/// <summary>
	/// Implements a random shuffling algorithm using a randomized approach similar to the Fisher–Yates shuffle.
	/// This algorithm randomizes the order of the lines.
	/// Visualization is achieved by introducing delays between swaps and updating visual positions via SwapPoints.
	/// </summary>
	public class RandomizeShuffle : SortAlgorithm
	{
		/// <summary>
		/// Gets the name of the algorithm.
		/// </summary>
		public override string Name { get => "RANDOMIZE_SHUFFLE"; }

		/// <summary>
		/// Randomizes the list of lines using a variant of the Fisher–Yates shuffle algorithm.
		/// A delay is added between each swap for visualization, and cancellation is supported.
		/// </summary>
		/// <param name="lines">The list of Line objects to randomize.</param>
		/// <param name="delay">Delay in milliseconds between swaps for visualization.</param>
		/// <param name="token">Cancellation token to interrupt the shuffle if requested.</param>
		/// <returns>A Task representing the asynchronous shuffling operation.</returns>
		public override async Task Sort(List<Line> lines, int delay, CancellationToken token = default)
		{
			// Initialize a Random object for generating random indices.
			Random random = new Random();

			// Iterate over each element in the list.
			for (int i = 0; i < lines.Count; i++)
			{
				// Check for cancellation before proceeding.
				token.ThrowIfCancellationRequested();
				// Wait for the specified delay for visualization purposes.
				await Task.Delay(delay, token);

				// Generate a random index between 0 and i (inclusive).
				int j = random.Next(0, i + 1);

				// Swap the elements at index i and index j.
				(lines[j], lines[i]) = (lines[i], lines[j]);
				// Visually swap the positions of the two lines.
				SwapPoints(lines[i], lines[j]);
			}
		}
	}
}
