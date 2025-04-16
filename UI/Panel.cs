using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI
{
	/// <summary>
	/// The Panel class represents a rectangular UI element used for backgrounds or borders.
	/// It supports caching of the generated texture to improve drawing performance.
	/// </summary>
	public class Panel
	{
		// Dimensions of the panel.
		private int width;
		private int height;
		// Position on the screen.
		private Vector2 position;
		// The fill color used to render the panel.
		private Color fillColor;

		// Cached texture to avoid re-creating the texture on every draw call.
		private Texture2D cachedTexture;
		// Reference to the GraphicsDevice used for creating the texture.
		private GraphicsDevice graphicsDevice;

		// Previous values for width, height, and fillColor to detect changes.
		private int prevWidth;
		private int prevHeight;
		private Color prevColor;

		// Public properties to access and modify the panel's dimensions, position, and fill color.
		public int Width { get => width; set => width = value; }
		public int Height { get => height; set => height = value; }
		public Vector2 Position { get => position; set => position = value; }
		public Color FillColor { get => fillColor; set => fillColor = value; }

		/// <summary>
		/// Constructs a new Panel with specified dimensions, position, and fill color.
		/// </summary>
		/// <param name="width">The width of the panel.</param>
		/// <param name="height">The height of the panel.</param>
		/// <param name="position">The position on the screen where the panel will be drawn.</param>
		/// <param name="fillColor">The color used to fill the panel.</param>
		public Panel(int width, int height, Vector2 position, Color fillColor)
		{
			this.width = width;
			this.height = height;
			this.position = position;
			this.fillColor = fillColor;

			// Set previous values to initial invalid states to ensure texture creation on first draw.
			prevWidth = -1;
			prevHeight = -1;
			prevColor = Color.Transparent;
		}

		/// <summary>
		/// Draws the panel using the provided SpriteBatch.
		/// Caches the texture and re-creates it only if the dimensions or fill color have changed.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch used to render the panel.</param>
		public void Draw(SpriteBatch spriteBatch)
		{
			// Ensure a valid GraphicsDevice reference is available.
			if (graphicsDevice == null)
			{
				graphicsDevice = spriteBatch.GraphicsDevice;
			}

			// If there is no cached texture or the panel properties have changed,
			// then re-create the texture.
			if (cachedTexture == null || width != prevWidth || height != prevHeight || fillColor != prevColor)
			{
				// Dispose of any previously cached texture to free up resources.
				cachedTexture?.Dispose();

				// Create a new texture with the panel's current dimensions.
				cachedTexture = new Texture2D(graphicsDevice, width, height);
				// Fill the texture with the specified fill color.
				Color[] data = new Color[width * height];
				for (int i = 0; i < data.Length; ++i)
					data[i] = fillColor;

				cachedTexture.SetData(data);

				// Update previous properties for future change detection.
				prevWidth = width;
				prevHeight = height;
				prevColor = fillColor;
			}

			// Begin drawing.
			spriteBatch.Begin();
			// Draw the cached texture at the panel's position.
			spriteBatch.Draw(cachedTexture, position, Color.White);
			spriteBatch.End();
		}
	}
}
