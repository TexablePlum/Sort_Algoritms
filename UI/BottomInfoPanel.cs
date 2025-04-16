using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FontStashSharp;
using System;
using System.Collections.Generic;

namespace UI
{
	/// <summary>
	/// OptionDimension holds calculated dimensions for a single option,
	/// including the sizes of the key text and the option text as well as their total width.
	/// </summary>
	internal class OptionDimension
	{
		public Vector2 KeySize;    // Size of the key part (e.g., "[ESC]")
		public Vector2 OptionSize; // Size of the remaining option text (e.g., " Settings")
		public float TotalWidth;   // Total width of the option (KeySize.X + spacing + OptionSize.X)
	}

	/// <summary>
	/// BottomInfoPanel is a panel displayed at the bottom of the screen that shows a list of key-cap options and a timer.
	/// It extends Panel and supports resizing via the IResize interface.
	/// It caches layout dimensions to avoid recalculation during drawing.
	/// </summary>
	public class BottomInfoPanel : Panel, IResize
	{
		// List of options to be shown on the left side.
		private List<KeyCapOption> leftOptions;
		// Expose a read-only version of the options list.
		protected IReadOnlyList<KeyCapOption> Options => leftOptions.AsReadOnly();

		// Timer variables.
		private TimeSpan elapsedTime;
		private bool timerRunning;

		// Font system and font used to render text in the panel.
		private FontSystem fontSystem;
		private DynamicSpriteFont font;

		// Default text color for rendering option text.
		private Color textColor;

		// Horizontal padding for layout and spacing between each option.
		private float horizontalPadding;
		private float spacingBetweenOptions = 20f;

		// Timer label configuration.
		private string timerLabel = "TIMER:"; // Label text for the timer.
		private Color timerLabelColor;
		private float timerLabelGap = 5f;      // Gap between the label and the timer value.

		// Cached layout values to avoid recalculation during drawing.
		private List<OptionDimension> cachedOptionDimensions;
		private float cachedTextY;             // Y position for text rendering (vertical centering).
		private float cachedTimerLabelWidth;   // Measured width of the timer label.
		private float cachedTimerValueWidth;   // Measured width of the timer value (formatted string).
		private float cachedTimerX;            // Calculated X position for drawing timer.
		private float cachedTimerY;            // Calculated Y position for drawing timer.

		/// <summary>
		/// Constructs a new BottomInfoPanel.
		/// </summary>
		/// <param name="width">Width of the panel.</param>
		/// <param name="height">Height of the panel.</param>
		/// <param name="position">Position of the panel.</param>
		/// <param name="fillColor">Background color of the panel.</param>
		/// <param name="options">List of key-cap options to display on the left.</param>
		/// <param name="textColor">Default text color (if an option doesn't specify its own color).</param>
		/// <param name="timerLabelColor">Color for the timer label text ("TIMER:").</param>
		/// <param name="horizontalPadding">Padding from the left and right edges of the panel.</param>
		public BottomInfoPanel(
			int width,
			int height,
			Vector2 position,
			Color fillColor,
			List<KeyCapOption> options,
			Color textColor,
			Color timerLabelColor,
			float horizontalPadding
		) : base(width, height, position, fillColor)
		{
			leftOptions = options;
			this.textColor = textColor;
			elapsedTime = TimeSpan.Zero;
			timerRunning = false;
			this.timerLabelColor = timerLabelColor;
			this.horizontalPadding = horizontalPadding;
		}

		/// <summary>
		/// Loads the panel's content, specifically initializing the font system from the specified font file.
		/// </summary>
		/// <param name="content">ContentManager to load assets.</param>
		/// <param name="fontPath">Path to the font file (e.g., "Content/MyFont.ttf").</param>
		public void LoadContent(ContentManager content, string fontPath)
		{
			fontSystem = new FontSystem();
			fontSystem.AddFont(File.ReadAllBytes(fontPath));
			UpdateFont();
		}

		/// <summary>
		/// Updates the panel's internal timer. If the timer is running, increments the elapsed time.
		/// </summary>
		/// <param name="gameTime">Provides elapsed time since the last update.</param>
		public void Update(GameTime gameTime)
		{
			if (timerRunning)
			{
				elapsedTime += gameTime.ElapsedGameTime;
			}
		}

		/// <summary>
		/// Draws the BottomInfoPanel.
		/// Uses cached layout values to render key-cap options on the left and the timer on the right.
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch instance used for drawing.</param>
		public new void Draw(SpriteBatch spriteBatch)
		{
			// Draw the background panel using the base Panel class.
			base.Draw(spriteBatch);

			// Format the timer string as "mm:ss:fff" (minutes, seconds, milliseconds).
			string timerString = string.Format("{0:mm\\:ss\\:fff}", elapsedTime);

			// Ensure font and cached layout have been initialized.
			if (font != null && cachedOptionDimensions != null)
			{
				spriteBatch.Begin();

				// Draw options on the left.
				float currentX = Position.X + horizontalPadding;
				for (int i = 0; i < leftOptions.Count; i++)
				{
					KeyCapOption option = leftOptions[i];
					OptionDimension dim = cachedOptionDimensions[i];

					// Choose appropriate colors based on whether the option is active.
					Color effectiveKeyColor = option.IsActive ? option.KeyColor : DarkenColor(option.KeyColor);
					Color effectiveOptionColor = option.IsActive ? option.OptionColor : DarkenColor(option.OptionColor);

					// Draw the key text (e.g., "[ESC]").
					font.DrawText(spriteBatch, option.KeyText, new Vector2(currentX, cachedTextY), effectiveKeyColor);
					// Draw the option text (e.g., " Settings") with a gap of 5 pixels.
					font.DrawText(spriteBatch, option.OptionText, new Vector2(currentX + dim.KeySize.X + 5f, cachedTextY), effectiveOptionColor);

					// Move the current X position by the total width of the option plus extra spacing.
					currentX += dim.TotalWidth + spacingBetweenOptions;
				}

				// Draw the timer.
				// First, draw the timer label ("TIMER:") at the cached position.
				font.DrawText(spriteBatch, timerLabel, new Vector2(cachedTimerX, cachedTimerY), timerLabelColor);
				// Then, draw the timer value next to the label.
				font.DrawText(spriteBatch, timerString, new Vector2(cachedTimerX + cachedTimerLabelWidth + timerLabelGap, cachedTimerY), textColor);

				spriteBatch.End();
			}
		}

		/// <summary>
		/// Resizes the BottomInfoPanel when the window or layout changes.
		/// Updates the panel's position and dimensions and recalculates the font size and layout.
		/// </summary>
		/// <param name="newRect">The new rectangle defining the panel's size and position.</param>
		public void Resize(Rectangle newRect)
		{
			Position = new Vector2(newRect.X, newRect.Y);
			Width = newRect.Width;
			Height = newRect.Height;
			UpdateFont();
		}

		/// <summary>
		/// Updates the font size based on the panel's width and recalculates the layout for options and the timer.
		/// </summary>
		private void UpdateFont()
		{
			if (fontSystem == null)
				return;
			// Calculate font size relative to the panel width; adjust scaling factor as needed.
			float fontSize = Width * 0.015f;
			font = fontSystem.GetFont(fontSize);

			RecalculateLayout();
		}

		/// <summary>
		/// Recalculates positions and dimensions for the key-cap options and the timer.
		/// This is executed when the font, panel size, or layout is updated.
		/// </summary>
		private void RecalculateLayout()
		{
			// Initialize the list for cached option dimensions.
			cachedOptionDimensions = new List<OptionDimension>();
			float keyOptionGap = 5f; // Gap between key and option text.

			// Calculate the Y position to vertically center the text within the panel.
			cachedTextY = Position.Y + (Height - font.MeasureString("A").Y) / 2.0f;

			// Calculate and cache dimensions for each key-cap option.
			foreach (var option in leftOptions)
			{
				OptionDimension dim = new OptionDimension();
				dim.KeySize = font.MeasureString(option.KeyText);
				dim.OptionSize = font.MeasureString(option.OptionText);
				// Total width includes key text, gap, and option text.
				dim.TotalWidth = dim.KeySize.X + keyOptionGap + dim.OptionSize.X;
				cachedOptionDimensions.Add(dim);
			}

			// Calculate dimensions for the timer label.
			cachedTimerLabelWidth = font.MeasureString(timerLabel).X;
			// Assume timer value has fixed format "00:00:000" and measure its width.
			cachedTimerValueWidth = font.MeasureString("00:00:000").X;
			float totalTimerWidth = cachedTimerLabelWidth + timerLabelGap + cachedTimerValueWidth;

			// Position timer on the right side of the panel with horizontal padding.
			cachedTimerX = Position.X + Width - totalTimerWidth - horizontalPadding;
			// Vertically center the timer text within the panel.
			cachedTimerY = Position.Y + (Height - font.MeasureString("00:00:000").Y) / 2.0f;
		}

		// --- Timer control methods ---

		/// <summary>
		/// Starts the timer, allowing elapsed time to be incremented.
		/// </summary>
		public void StartTimer()
		{
			timerRunning = true;
		}

		/// <summary>
		/// Stops (pauses) the timer.
		/// </summary>
		public void StopTimer()
		{
			timerRunning = false;
		}

		/// <summary>
		/// Resets the timer to zero and stops it.
		/// </summary>
		public void ResetTimer()
		{
			timerRunning = false;
			elapsedTime = TimeSpan.Zero;
		}

		/// <summary>
		/// Sets the active state of an option at the specified index.
		/// </summary>
		/// <param name="index">Index of the option to update.</param>
		/// <param name="isActive">True if the option should be active; false otherwise.</param>
		public void SetOptionActive(int index, bool isActive)
		{
			if (index >= 0 && index < leftOptions.Count)
			{
				leftOptions[index].IsActive = isActive;
			}
		}

		/// <summary>
		/// Helper method to darken a given color by a specified factor.
		/// </summary>
		/// <param name="color">The original color.</param>
		/// <param name="factor">Factor to darken (default 0.5 means 50% intensity).</param>
		/// <returns>A new Color that is darkened relative to the original.</returns>
		private Color DarkenColor(Color color, float factor = 0.5f)
		{
			return new Color(
				(int)(color.R * factor),
				(int)(color.G * factor),
				(int)(color.B * factor)
			);
		}
	}
}
