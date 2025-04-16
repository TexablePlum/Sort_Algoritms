using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sort_Algoritms
{
	/// <summary>
	/// Container class represents a collection of visual lines to be displayed in the application.
	/// It handles the creation, updating, and drawing of the lines based on provided layout parameters.
	/// </summary>
	public class Container
	{
		// The overall resolution (width and height) for the container area.
		private Vector2 resolution;
		// Margin around the frame to create padding from the container's edges.
		private int frameMargin;
		// Total number of lines to display.
		private int linesCount;
		// Margin between individual lines.
		private int linesMargin;
		// Color of the lines.
		private Color linesColor;

		// List of Line objects representing the visual lines.
		private List<Line> lines;

		/// <summary>
		/// Gets or sets the collection of lines in the container.
		/// </summary>
		public List<Line> Lines { get => lines; set => lines = value; }

		/// <summary>
		/// Indicates whether the container is currently in a sorted state.
		/// </summary>
		public bool IsSorted { get; set; }

		/// <summary>
		/// Constructor for the Container.
		/// Initializes a new instance with the specified resolution, margins, number of lines, and color.
		/// </summary>
		/// <param name="resolution">The overall width and height of the container.</param>
		/// <param name="frameMargin">The margin around the container's frame.</param>
		/// <param name="linesCount">The number of lines to be created.</param>
		/// <param name="linesMargin">The margin between each line.</param>
		/// <param name="linesColor">The color used for the lines.</param>
		public Container(Vector2 resolution, int frameMargin, int linesCount, int linesMargin, Color linesColor)
		{
			this.resolution = resolution;
			this.frameMargin = frameMargin;
			this.linesCount = linesCount;
			this.linesMargin = linesMargin;
			this.linesColor = linesColor;

			// Initializes the list that will hold the line objects.
			lines = new List<Line>();
		}

		/// <summary>
		/// Loads the lines based on the current configuration parameters.
		/// Calculates the starting point, thickness, and height steps for each line.
		/// </summary>
		public void Load()
		{
			// Starting point is at the bottom left within the frame margin.
			Vector2 startPoint = new Vector2(frameMargin, resolution.Y - frameMargin);
			// Minimal line height starts at 1 unit.
			float startHeight = 1;
			// Final available height for lines after excluding frame margins.
			float finalHeight = resolution.Y - (2 * frameMargin);
			// Calculate thickness based on the available width and margins.
			float thickness = Thickness_Calculations();

			// Determine the incremental height change between successive lines.
			float heightStep = (finalHeight - startHeight) / (linesCount - 1);

			// Create each line and update the starting X position for the next line.
			for (int i = 0; i < linesCount; i++)
			{
				float currentHeight = startHeight + (heightStep * i);
				// Create and add a new line to the list.
				lines.Add(new Line(startPoint, thickness, currentHeight, linesColor));
				// Move the starting point to the right by the thickness plus the margin.
				startPoint.X += thickness + linesMargin;
			}
			// Mark the container as sorted by default after initial load.
			IsSorted = true;
		}

		/// <summary>
		/// Draws all the lines in the container using the provided SpriteBatch.
		/// Each line handles its own drawing logic.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch used to draw the lines.</param>
		public void Draw(SpriteBatch spriteBatch)
		{
			// Iterate over each line in the container and render it.
			foreach (var line in lines)
			{
				line.Draw(spriteBatch);
			}
		}

		/// <summary>
		/// Updates the container configuration if the resolution, line count, or line margin have changed.
		/// If any changes are detected, the container will clear and reload its lines.
		/// </summary>
		/// <param name="resolution">The new resolution for the container.</param>
		/// <param name="linesCount">The new number of lines.</param>
		/// <param name="linesMargin">The new margin between lines.</param>
		public void Update(Vector2 resolution, int linesCount, int linesMargin)
		{
			// Check if any of the key configuration parameters have changed.
			if (this.resolution != resolution || this.linesCount != linesCount || this.linesMargin != linesMargin)
			{
				// Update the container parameters.
				this.resolution = resolution;
				this.linesCount = linesCount;
				this.linesMargin = linesMargin;

				// Clear existing lines and recreate them with updated parameters.
				lines.Clear();
				Load();
			}
		}

		/// <summary>
		/// Calculates the thickness for each line based on the available width, frame margins, and spacing between lines.
		/// </summary>
		/// <returns>The calculated thickness for the lines.</returns>
		private float Thickness_Calculations()
		{
			// Calculation: available width divided by the number of lines.
			return (resolution.X - 2 * frameMargin - linesMargin * (linesCount - 1)) / linesCount;
		}
	}
}
