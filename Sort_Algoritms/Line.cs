using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sort_Algoritms
{
	/// <summary>
	/// Represents a visual line used for displaying individual elements of the sorting algorithm.
	/// Handles its own creation and rendering.
	/// </summary>
	public class Line
	{
		// A shared GraphicsDevice instance required for creating textures.
		protected static GraphicsDevice graphicsDevice;

		// Texture for drawing the line. This is a 1x1 texture colored based on the specified color.
		private readonly Texture2D texture;

		// Position of the line. Note that the Y-coordinate of this point corresponds to the bottom of the line.
		private Vector2 point;

		// The color of the line.
		private Color color;

		// The width and height of the line.
		private float width;
		private float height;

		// Public getters and setters for the line's properties.
		public Vector2 Point { get => point; set => point = value; }
		public Color Color { get => color; set => color = value; }
		public float Width { get => width; set => width = value; }
		public float Height { get => height; set => height = value; }

		/// <summary>
		/// Initializes the shared GraphicsDevice used for creating textures.
		/// Must be called before any Line instance is created.
		/// </summary>
		/// <param name="graphicsDevice">The graphics device used for rendering textures.</param>
		public static void Initialize(GraphicsDevice graphicsDevice)
		{
			Line.graphicsDevice = graphicsDevice;
		}

		/// <summary>
		/// Constructs a new Line instance with the specified position, dimensions, and color.
		/// </summary>
		/// <param name="point">The bottom-left position of the line within the screen.</param>
		/// <param name="width">The width (thickness) of the line.</param>
		/// <param name="height">The height of the line (represents the value in the visualization).</param>
		/// <param name="color">The color of the line.</param>
		public Line(Vector2 point, float width, float height, Color color)
		{
			this.point = point;
			this.width = width;
			this.height = height;
			this.color = color;

			// Create a 1x1 texture and set its color. This texture is then stretched to draw the line.
			texture = new Texture2D(graphicsDevice, 1, 1);
			texture.SetData(new[] { color });
		}

		/// <summary>
		/// Draws the line using the provided SpriteBatch.
		/// The draw position is adjusted so that the line is rendered correctly from its bottom position.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch instance used for drawing.</param>
		public void Draw(SpriteBatch spriteBatch)
		{
			// Calculate the draw position. We subtract the height from the Y-coordinate so the line grows upward.
			Vector2 drawPosition = new Vector2(point.X, point.Y - height);

			// Start the SpriteBatch, draw the texture scaled to the desired dimensions, then end the SpriteBatch.
			spriteBatch.Begin();
			spriteBatch.Draw(
				texture,           // Texture to draw
				drawPosition,      // Destination position on the screen
				null,              // Source rectangle (null draws the entire texture)
				color,             // Tint color
				0,                 // Rotation angle
				Vector2.Zero,      // Origin point for rotation/scaling
				new Vector2(width, height), // Scale the texture to the desired width and height
				SpriteEffects.None,// No sprite effects applied
				0                  // Layer depth
			);
			spriteBatch.End();
		}
	}
}
