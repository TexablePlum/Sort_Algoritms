using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI
{
	/// <summary>
	/// The Grid class is used to calculate and render a grid layout based on screen dimensions,
	/// margins, gaps (offsets), and a desired cell aspect ratio. It computes the cell sizes and positions,
	/// and provides methods to draw the grid and access the individual cell rectangles.
	/// </summary>
	public class Grid
	{
		// Screen dimensions on which the grid is based.
		private int screenWidth;
		private int screenHeight;

		// The available grid width and height after accounting for margins.
		private int availableGridWidth;
		private int availableGridHeight;

		// Number of rows and columns in the grid.
		private int rows;
		private int cols;

		// Gap (offset) between cells, expressed as a fraction of the available grid dimensions.
		private float cellOffsetX;
		private float cellOffsetY;

		// Margins as percentages of the screen dimensions.
		private float marginPercentX;
		private float marginPercentY;

		// Desired aspect ratio for each cell (width / height).
		private float desiredCellAspectRatio;

		// 2D array that stores the rectangles representing each cell in the grid.
		private Rectangle[,] cells;
		// A simple 1x1 white pixel texture used for drawing grid lines.
		private Texture2D pixel;

		/// <summary>
		/// Constructs a new Grid with specified screen dimensions, grid layout parameters,
		/// offsets, margins, and the desired cell aspect ratio.
		/// </summary>
		/// <param name="screenWidth">Width of the screen.</param>
		/// <param name="screenHeight">Height of the screen.</param>
		/// <param name="rows">Number of rows in the grid.</param>
		/// <param name="cols">Number of columns in the grid.</param>
		/// <param name="cellOffsetX">Horizontal gap factor (fraction of available width) between cells.</param>
		/// <param name="cellOffsetY">Vertical gap factor (fraction of available height) between cells.</param>
		/// <param name="marginPercentX">Horizontal margin as percentage of screen width.</param>
		/// <param name="marginPercentY">Vertical margin as percentage of screen height.</param>
		/// <param name="desiredCellAspectRatio">Desired cell aspect ratio (width / height).</param>
		public Grid(
			int screenWidth, int screenHeight,
			int rows, int cols,
			float cellOffsetX, float cellOffsetY,
			float marginPercentX, float marginPercentY,
			float desiredCellAspectRatio)
		{
			this.screenWidth = screenWidth;
			this.screenHeight = screenHeight;
			this.rows = rows;
			this.cols = cols;
			this.cellOffsetX = cellOffsetX;
			this.cellOffsetY = cellOffsetY;
			this.marginPercentX = marginPercentX;
			this.marginPercentY = marginPercentY;
			this.desiredCellAspectRatio = desiredCellAspectRatio;
			cells = new Rectangle[rows, cols];

			Initialize();
		}

		/// <summary>
		/// Initializes the grid layout by calculating the available space,
		/// determining cell sizes while maintaining the aspect ratio, and positioning cells with gaps.
		/// </summary>
		public void Initialize()
		{
			// Calculate available grid size after applying horizontal and vertical margins.
			availableGridWidth = (int)(screenWidth * (1 - 2 * marginPercentX));
			availableGridHeight = (int)(screenHeight * (1 - 2 * marginPercentY));

			// Calculate absolute gap between cells based on the available grid dimensions.
			int gapX = (int)(cellOffsetX * availableGridWidth);
			int gapY = (int)(cellOffsetY * availableGridHeight);

			// Calculate cell height candidate from available width.
			// This is based on fitting the desired aspect ratio and accounting for the horizontal gaps.
			float candidateCellHeightFromWidth = (availableGridWidth - gapX * (cols - 1)) / (cols * desiredCellAspectRatio);
			// Calculate cell height candidate from available height.
			float candidateCellHeightFromHeight = (availableGridHeight - gapY * (rows - 1)) / (float)rows;

			// Select the smaller candidate to ensure both width and height constraints are met.
			float cellHeight = Math.Min(candidateCellHeightFromWidth, candidateCellHeightFromHeight);
			float cellWidth = desiredCellAspectRatio * cellHeight;

			// Determine the actual grid width and height after placing all cells and their gaps.
			float actualGridWidth = cols * cellWidth + (cols - 1) * gapX;
			float actualGridHeight = rows * cellHeight + (rows - 1) * gapY;

			// Center the grid within the available grid area by calculating offset positions.
			int gridX = (int)(screenWidth * marginPercentX + (availableGridWidth - actualGridWidth) / 2);
			int gridY = (int)(screenHeight * marginPercentY + (availableGridHeight - actualGridHeight) / 2);

			// Populate the cells array with calculated rectangles for each grid cell.
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					int x = gridX + (int)(j * (cellWidth + gapX));
					int y = gridY + (int)(i * (cellHeight + gapY));
					cells[i, j] = new Rectangle(x, y, (int)cellWidth, (int)cellHeight);
				}
			}
		}

		/// <summary>
		/// Loads the required content for the grid, creating a simple white pixel texture
		/// used for drawing grid lines.
		/// </summary>
		/// <param name="graphicsDevice">GraphicsDevice used to create textures.</param>
		public void LoadContent(GraphicsDevice graphicsDevice)
		{
			// Create a 1x1 texture and set it to white.
			pixel = new Texture2D(graphicsDevice, 1, 1);
			pixel.SetData(new[] { Color.White });
		}

		/// <summary>
		/// Draws the grid by rendering the borders of each cell.
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch used for drawing.</param>
		public void Draw(SpriteBatch spriteBatch)
		{
			// Set the color used for grid lines.
			Color lineColor = Color.Red;

			// Begin sprite batch for rendering.
			spriteBatch.Begin();
			// Loop through each cell and draw its borders.
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					Rectangle rect = cells[i, j];

					// Draw top border
					spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, 1), lineColor);
					// Draw bottom border
					spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y + rect.Height, rect.Width, 1), lineColor);
					// Draw left border
					spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, 1, rect.Height), lineColor);
					// Draw right border
					spriteBatch.Draw(pixel, new Rectangle(rect.X + rect.Width, rect.Y, 1, rect.Height), lineColor);
				}
			}
			spriteBatch.End();
		}

		/// <summary>
		/// Retrieves all cell rectangles from the grid.
		/// </summary>
		/// <returns>A list of Rectangles representing each cell.</returns>
		public List<Rectangle> GetCellRectangles()
		{
			List<Rectangle> cellRectangles = new List<Rectangle>();
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					cellRectangles.Add(cells[i, j]);
				}
			}
			return cellRectangles;
		}

		/// <summary>
		/// Resizes the grid to new screen dimensions and recalculates the layout.
		/// </summary>
		/// <param name="newWidth">New screen width.</param>
		/// <param name="newHeight">New screen height.</param>
		public void Resize(int newWidth, int newHeight)
		{
			// Update screen dimensions.
			screenWidth = newWidth;
			screenHeight = newHeight;
			// Recalculate the grid layout based on the new dimensions.
			Initialize();
		}
	}
}
