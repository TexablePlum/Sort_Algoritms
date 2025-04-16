using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FontStashSharp;

namespace UI
{
	/// <summary>
	/// LogoPanel is a UI element that displays a logo or text within a specified rectangular area.
	/// It supports dynamic resizing by updating the font based on the current panel size.
	/// </summary>
	public class LogoPanel : IResize
	{
		// The current rectangle area where the logo is drawn.
		private Rectangle rect;

		// Text to be displayed as the logo.
		private string text;
		// Color of the text.
		private Color textColor;
		// Ratio of the text size relative to the shorter side of the rectangle.
		private float textSizeRatio;

		// Font system used to load and manage fonts.
		private FontSystem fontSystem;
		// The dynamically generated font based on the logo panel's size.
		private DynamicSpriteFont font;

		/// <summary>
		/// Constructs a new LogoPanel with the specified rectangle, text, text color, and text size ratio.
		/// </summary>
		/// <param name="rect">The rectangular area where the logo will be drawn.</param>
		/// <param name="text">The logo text to display.</param>
		/// <param name="textColor">The color of the logo text.</param>
		/// <param name="textSizeRatio">
		/// The ratio used to calculate the font size relative to the panel's shorter dimension.
		/// Default value is 0.5f.
		/// </param>
		public LogoPanel(Rectangle rect, string text, Color textColor, float textSizeRatio = 0.5f)
		{
			this.rect = rect;
			this.text = text;
			this.textColor = textColor;
			this.textSizeRatio = textSizeRatio;
		}

		/// <summary>
		/// Loads the font content from the specified file path.
		/// Initializes the FontSystem and updates the font based on the current rectangle dimensions.
		/// </summary>
		/// <param name="fontPath">Path to the font file.</param>
		public void LoadContent(string fontPath)
		{
			// Initialize font system and add the specified font file.
			fontSystem = new FontSystem();
			fontSystem.AddFont(File.ReadAllBytes(fontPath));

			// Update the font based on the current rectangle dimensions.
			UpdateFont();
		}

		/// <summary>
		/// Draws the logo text centered within the panel's rectangle.
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch instance used for drawing.</param>
		public void Draw(SpriteBatch spriteBatch)
		{
			// Measure the text dimensions using the current font.
			Vector2 textSize = font.MeasureString(text);
			// Calculate the centered position for the logo text.
			Vector2 textPosition = new Vector2(
				rect.X + (rect.Width - textSize.X) / 2f,
				rect.Y + (rect.Height - textSize.Y) / 2f
			);

			// Begin the sprite batch, draw the text, and then end the sprite batch.
			spriteBatch.Begin();
			font.DrawText(spriteBatch, text, textPosition, textColor);
			spriteBatch.End();
		}

		/// <summary>
		/// Resizes the logo panel by updating its rectangle and recalculating the font size.
		/// </summary>
		/// <param name="newRect">The new rectangle defining the panel's position and dimensions.</param>
		public void Resize(Rectangle newRect)
		{
			// Update the panel's rectangle.
			rect = newRect;
			// Recalculate the font size and update the font.
			UpdateFont();
		}

		/// <summary>
		/// Updates the font based on the current rectangle dimensions.
		/// The font size is set relative to the shorter side of the rectangle.
		/// </summary>
		private void UpdateFont()
		{
			// If the font system is not initialized, exit early.
			if (fontSystem == null)
				return;

			// Determine the shorter side of the rectangle.
			int shorterSide = Math.Min(rect.Width, rect.Height);
			// Calculate the font size using the provided text size ratio.
			float fontSize = shorterSide * textSizeRatio;
			// Retrieve the font from the font system using the calculated size.
			font = fontSystem.GetFont(fontSize);
		}
	}
}
