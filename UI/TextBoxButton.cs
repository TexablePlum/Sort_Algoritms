using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace UI
{
	/// <summary>
	/// TextBoxButton is a specialized button that supports text editing.
	/// It extends the Button class and allows the user to modify the editable portion 
	/// of the button's text (e.g., numeric values) while appending a fixed suffix.
	/// </summary>
	public class TextBoxButton : Button
	{
		// Flag indicating whether the editable (lower) text is currently in edit mode.
		private bool isEditing = false;

		// Buffer for the text being entered; initially contains the editable portion.
		private StringBuilder inputText = new StringBuilder();

		// Blinking cursor handling.
		private bool cursorVisible;
		private double cursorTimer;
		// Time interval in seconds for the cursor to blink.
		private readonly double cursorBlinkTime = 0.5;

		// Keeps track of the keyboard state from the previous frame.
		private KeyboardState previousKeyboardState;
		// Stores the mouse state from the previous frame – useful for detecting new clicks.
		private MouseState previousMouseState;

		// Stores the original editable portion of the text (without the suffix).
		private string originalText;
		// Suffix that is always appended to the editable text; if not provided, remains empty.
		private string fixedSuffix;

		// Validator function to check if the new text is valid. Defaults to always true.
		private Func<string, bool> validator { get; set; } = (text) => true;

		// The current numeric value derived from the editable text.
		public int Value;

		/// <summary>
		/// Constructor for TextBoxButton.
		/// Parameter 'buttonOptionText' is the editable part (e.g., "50"),
		/// and 'fixedSuffix' is the constant part appended to the displayed text (e.g., "_LINES").
		/// The displayed text is: editableText + fixedSuffix.
		/// The 'buttonAdditionalText' remains as a label above the button.
		/// </summary>
		/// <param name="position">Screen position for the button.</param>
		/// <param name="width">Width of the button.</param>
		/// <param name="height">Height of the button.</param>
		/// <param name="borderSize">Border thickness of the button.</param>
		/// <param name="buttonOptionText">Editable portion of the button text.</param>
		/// <param name="isActive">Determines if the button is active (clickable).</param>
		/// <param name="primaryColor">Primary color used for button styling.</param>
		/// <param name="secondaryColor">Secondary color used for button styling.</param>
		/// <param name="buttonTheme">Button theme affecting visual appearance.</param>
		/// <param name="onClick">Delegate to invoke when the button is clicked.</param>
		/// <param name="buttonAdditionalText">Additional label text for the button.</param>
		/// <param name="fixedSuffix">Fixed suffix appended to the editable part.</param>
		/// <param name="validator">Optional validator function for the new text.</param>
		public TextBoxButton(
			Vector2 position,
			int width,
			int height,
			int borderSize,
			string buttonOptionText,
			bool isActive,
			Color primaryColor,
			Color secondaryColor,
			ButtonTheme buttonTheme,
			Action onClick,
			string? buttonAdditionalText,
			string fixedSuffix = "",
			Func<string, bool>? validator = null
		)
			: base(
				  position,
				  width,
				  height,
				  borderSize,
				  // If a fixedSuffix is provided, append it to the buttonOptionText for display.
				  string.IsNullOrEmpty(fixedSuffix) ? buttonOptionText : buttonOptionText + fixedSuffix,
				  isActive,
				  primaryColor,
				  secondaryColor,
				  buttonTheme,
				  onClick,
				  buttonAdditionalText)
		{
			// Store only the editable part without the fixed suffix.
			originalText = buttonOptionText;
			inputText = new StringBuilder(originalText);
			this.fixedSuffix = fixedSuffix;
			this.validator = validator ?? ((text) => true);
			// Convert the initial text to an integer for the Value property.
			Value = int.Parse(buttonOptionText);
		}

		/// <summary>
		/// Loads assets (texture and font) for the button.
		/// Calls the base LoadContent method.
		/// </summary>
		/// <param name="content">ContentManager to load assets.</param>
		/// <param name="texturePath">Path to the button texture asset.</param>
		/// <param name="fontPath">Path to the font file for button text.</param>
		public new void LoadContent(ContentManager content, string texturePath, string fontPath)
		{
			base.LoadContent(content, texturePath, fontPath);
		}

		/// <summary>
		/// Updates the TextBoxButton.
		/// - Calls the base button Update method.
		/// - If a new click is detected:
		///   - When clicked inside the button, starts editing:
		///     removes the suffix from display and clears the text buffer.
		///   - When clicked outside, cancels editing and restores full text (editable part + fixedSuffix).
		/// - In edit mode, processes keyboard input and handles the blinking cursor.
		/// - Finally, sets the button state: Pressed while editing, otherwise Normal.
		/// </summary>
		/// <param name="gameTime">Provides timing information for the update.</param>
		public new void Update(GameTime gameTime)
		{
			// Call base update for normal button behavior.
			base.Update(gameTime);

			// Define the button's area.
			Rectangle buttonRect = new Rectangle((int)position.X, (int)position.Y, width, height);
			MouseState mouseState = Mouse.GetState();

			// Check for a new mouse click (transition from Released to Pressed).
			if (mouseState.LeftButton == ButtonState.Pressed &&
				previousMouseState.LeftButton == ButtonState.Released)
			{
				if (buttonRect.Contains(new Point(mouseState.X, mouseState.Y)))
				{
					// Click inside the button:
					if (!isEditing)
					{
						// If not currently editing, store the editable part 
						// (by removing the suffix if it exists) and clear the buffer.
						if (!string.IsNullOrEmpty(fixedSuffix))
							originalText = buttonOptionText.Replace(fixedSuffix, "");
						else
							originalText = buttonOptionText;
						inputText.Clear();
					}
					// Enter edit mode.
					isEditing = true;
					// Clear the visible text to show only the editable part.
					buttonOptionText = "";
				}
				else
				{
					// Click outside the button: cancel editing and restore full text.
					isEditing = false;
					buttonOptionText = originalText + fixedSuffix;
					inputText.Clear();
					inputText.Append(originalText);
				}
			}

			// Process keyboard input if in editing mode.
			if (isEditing)
			{
				KeyboardState keyboardState = Keyboard.GetState();
				// Temporarily clear the visible text.
				buttonOptionText = "";

				// Process each pressed key.
				foreach (Keys key in keyboardState.GetPressedKeys())
				{
					// Only process keys that weren't pressed in the previous frame.
					if (!previousKeyboardState.IsKeyDown(key))
					{
						switch (key)
						{
							case Keys.Back:
								if (inputText.Length > 0)
									inputText.Remove(inputText.Length - 1, 1);
								break;
							case Keys.Enter:
								// When Enter is pressed, finalize the input.
								string newText = inputText.ToString();

								if (validator(newText))
								{
									// Update display text and store the new valid value.
									buttonOptionText = newText + fixedSuffix;
									originalText = newText;
									Value = int.Parse(newText);
									UpdateFontsAndText();
								}
								else
								{
									// If validation fails, restore the original text.
									buttonOptionText = originalText + fixedSuffix;
									inputText.Clear();
									inputText.Append(originalText);
								}
								isEditing = false;
								break;
							case Keys.Space:
								inputText.Append(' ');
								break;
							default:
								// Process alphabetical keys.
								if (key >= Keys.A && key <= Keys.Z)
								{
									bool shift = keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift);
									// Convert key to corresponding character (shift for uppercase).
									char c = (char)(shift ? key : key + 32);
									inputText.Append(c);
								}
								// Process number keys.
								else if (key >= Keys.D0 && key <= Keys.D9)
								{
									inputText.Append((char)('0' + (key - Keys.D0)));
								}
								break;
						}
					}
				}

				// Update cursor blinking timer.
				cursorTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (cursorTimer >= cursorBlinkTime)
				{
					cursorVisible = !cursorVisible;
					cursorTimer = 0;
				}

				previousKeyboardState = keyboardState;
			}
			else
			{
				// When not editing, ensure the cursor is not visible.
				cursorVisible = false;
				previousKeyboardState = Keyboard.GetState();
			}

			// Disable button interactivity while editing.
			isActive = !isEditing;
			previousMouseState = mouseState;
		}

		/// <summary>
		/// Draws the TextBoxButton.
		/// - Calls the base Draw method to render the button.
		/// - If editing, draws the current input text (without the suffix) and a blinking cursor.
		///   The vertical position is derived from buttonOptionTextPosition as in the original Button.
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch used for drawing UI elements.</param>
		public new void Draw(SpriteBatch spriteBatch)
		{
			// Draw the base button (panels, texture, static texts).
			base.Draw(spriteBatch);

			// If in editing mode and font is available, draw the typed text with a blinking cursor.
			if (isEditing && buttonOptionTextFont != null)
			{
				spriteBatch.Begin();

				// Get the currently typed text.
				string typedStr = inputText.ToString();
				// Determine its width for centering.
				float textWidth = buttonOptionTextFont.MeasureString(typedStr).X;
				// Calculate horizontal position to center the text in the text field.
				float posX = textField.X + (textField.Width - textWidth) / 2;
				// Use the pre-calculated vertical position.
				float posY = buttonOptionTextPosition.Y;
				Vector2 drawPos = new Vector2(posX, posY);

				// Draw the typed text.
				buttonOptionTextFont.DrawText(spriteBatch, typedStr, drawPos, buttonOptionTextColor);

				// Draw the blinking cursor next to the text if visible.
				if (cursorVisible)
				{
					Vector2 cursorPos = new Vector2(drawPos.X + textWidth, drawPos.Y);
					buttonOptionTextFont.DrawText(spriteBatch, "|", cursorPos, buttonOptionTextColor);
				}

				spriteBatch.End();
			}
		}
	}
}
