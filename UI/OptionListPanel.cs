using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FontStashSharp;
using System.Text;
using System.Collections.Generic;

namespace UI
{
	/// <summary>
	/// OptionListPanel represents a panel with a title and a list of selectable options.
	/// It extends the Panel class and implements IResize for responsive layout adjustments.
	/// When an option is clicked, the OnOptionSelected event is triggered with the option index.
	/// </summary>
	public class OptionListPanel : Panel, IResize
	{
		// Title text for the panel (displayed on the top portion).
		private string title;
		// List of option strings.
		private List<string> options;
		// Index of the currently selected option.
		private int selectedIndex;

		// Stores the mouse state from the previous frame to detect new clicks.
		private MouseState previousMouseState;

		// Font system for rendering text.
		private FontSystem fontSystem;
		// Fonts for the title and options.
		private DynamicSpriteFont titleFont;
		private DynamicSpriteFont optionFont;

		// Rectangles defining layout areas for title and option list.
		private Rectangle titleRect;
		private Rectangle optionsRect;
		// A list of rectangles corresponding to each option row.
		private List<Rectangle> optionRectangles;

		// Texture for highlighting the selected option.
		private Texture2D highlightTexture;

		/// <summary>
		/// Event fired when an option is selected, providing the selected option's index.
		/// </summary>
		public event Action<int> OnOptionSelected;

		/// <summary>
		/// Gets the currently selected option text.
		/// If no options are available, returns an empty string.
		/// </summary>
		public string SelectedOption => options.Count > 0 ? options[selectedIndex] : string.Empty;

		/// <summary>
		/// Constructs a new OptionListPanel.
		/// </summary>
		/// <param name="width">Width of the panel.</param>
		/// <param name="height">Height of the panel.</param>
		/// <param name="position">Screen position where the panel is drawn.</param>
		/// <param name="fillColor">Background fill color of the panel.</param>
		/// <param name="title">Title text to be displayed on top.</param>
		/// <param name="options">List of option strings to display.</param>
		/// <param name="defaultSelectedIndex">Default selected option index (default is 0).</param>
		public OptionListPanel(int width, int height, Vector2 position, Color fillColor, string title, List<string> options, int defaultSelectedIndex = 0)
			: base(width, height, position, fillColor)
		{
			this.title = title;
			this.options = options;
			selectedIndex = defaultSelectedIndex;
			// Initialize list to hold rectangles for each option.
			optionRectangles = new List<Rectangle>();
		}

		/// <summary>
		/// Loads content for the panel, including fonts.
		/// </summary>
		/// <param name="content">ContentManager to load content assets.</param>
		/// <param name="fontPath">Path to the font file to use for rendering text.</param>
		public void LoadContent(ContentManager content, string fontPath)
		{
			// Initialize the font system and load the font file from disk.
			fontSystem = new FontSystem();
			fontSystem.AddFont(File.ReadAllBytes(fontPath));
			// Update fonts and layout based on current panel dimensions.
			UpdateFontsAndLayout();
		}

		/// <summary>
		/// Calculates and sets up fonts and layout rectangles for title and options.
		/// Called upon loading content and when resizing the panel.
		/// </summary>
		private void UpdateFontsAndLayout()
		{
			// Allocate 15% height of the panel for the title.
			int titleHeight = (int)(Height * 0.15f);
			// The remaining height will be used for the list of options.
			int listHeight = Height - titleHeight;

			// Set the title area rectangle.
			titleRect = new Rectangle((int)Position.X, (int)Position.Y, Width, titleHeight);
			// Set the rectangle for options area directly below the title.
			optionsRect = new Rectangle((int)Position.X, (int)Position.Y + titleHeight, Width, listHeight);

			// Calculate font size relative to panel width.
			float titleFontSize = Width * 0.06f;
			titleFont = fontSystem.GetFont(titleFontSize);

			float optionFontSize = Width * 0.08f;
			optionFont = fontSystem.GetFont(optionFontSize);

			// Divide the options area into equal-height rows for each option.
			float rowHeight = options.Count > 0 ? listHeight / (float)options.Count : listHeight;
			optionRectangles.Clear();
			for (int i = 0; i < options.Count; i++)
			{
				// Calculate the Y position for each option row.
				int rowY = optionsRect.Y + (int)(i * rowHeight);
				// Define the rectangle corresponding to the option row.
				Rectangle rowRect = new Rectangle(optionsRect.X, rowY, optionsRect.Width, (int)rowHeight);
				optionRectangles.Add(rowRect);
			}
		}

		/// <summary>
		/// Updates the panel logic, including handling option selection.
		/// Listens for mouse click events and determines if an option was clicked.
		/// </summary>
		/// <param name="gameTime">Provides timing information for updates.</param>
		public void Update(GameTime gameTime)
		{
			MouseState currentMouseState = Mouse.GetState();
			// Define the panel area rectangle.
			Rectangle panelRect = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

			// Process mouse click events.
			// When left button changes from Released to Pressed, check options.
			if (currentMouseState.LeftButton == ButtonState.Pressed &&
				previousMouseState.LeftButton == ButtonState.Released)
			{
				for (int i = 0; i < optionRectangles.Count; i++)
				{
					// If the click falls within an option's rectangle, select that option.
					if (optionRectangles[i].Contains(currentMouseState.X, currentMouseState.Y))
					{
						selectedIndex = i;
						OnOptionSelected?.Invoke(selectedIndex);
						break;
					}
				}
			}

			// Update the previous mouse state for next frame.
			previousMouseState = currentMouseState;
		}

		/// <summary>
		/// Draws the OptionListPanel.
		/// First draws the base panel, then the title, and finally each option.
		/// The selected option is highlighted.
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch used for drawing.</param>
		public new void Draw(SpriteBatch spriteBatch)
		{
			// Draw the panel background using the base Panel class.
			base.Draw(spriteBatch);
			spriteBatch.Begin();

			// Calculate the size and centered position for the title text.
			Vector2 titleSize = titleFont.MeasureString(title);
			Vector2 titlePos = new Vector2(
				titleRect.X + (titleRect.Width - titleSize.X) / 2,
				titleRect.Y + (titleRect.Height - titleSize.Y) / 2
			);
			// Draw the title text in black.
			spriteBatch.DrawString(titleFont, title, titlePos, Color.Black);

			// Create the highlight texture if it hasn't been created already.
			if (highlightTexture == null)
			{
				highlightTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
				// Set the highlight color (semi-transparent gray).
				highlightTexture.SetData(new[] { new Color(150, 150, 150, 150) });
			}

			// Draw each option.
			for (int i = 0; i < options.Count; i++)
			{
				Rectangle optRect = optionRectangles[i];
				string optText = options[i];
				// Measure option text dimensions.
				Vector2 textSize = optionFont.MeasureString(optText);
				// Calculate centered position for the option text.
				Vector2 textPos = new Vector2(
					optRect.X + (optRect.Width - textSize.X) / 2,
					optRect.Y + (optRect.Height - textSize.Y) / 2
				);

				// If this option is selected, draw a highlight rectangle underneath.
				if (i == selectedIndex)
					spriteBatch.Draw(highlightTexture, optRect, Color.White);

				// Draw the option text in black.
				spriteBatch.DrawString(optionFont, optText, textPos, Color.Black);
			}
			spriteBatch.End();
		}

		/// <summary>
		/// Resizes the OptionListPanel by updating dimensions, position, and recalculating layout.
		/// </summary>
		/// <param name="newRect">The new rectangle defining panel dimensions and position.</param>
		public void Resize(Rectangle newRect)
		{
			Width = newRect.Width;
			Height = newRect.Height;
			Position = new Vector2(newRect.X, newRect.Y);
			// Recalculate fonts and layout when the panel is resized.
			UpdateFontsAndLayout();
		}
	}
}
