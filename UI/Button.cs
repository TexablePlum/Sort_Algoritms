using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FontStashSharp;
using Microsoft.Xna.Framework.Input;

namespace UI
{
	/// <summary>
	/// Enumeration to define the button theme.
	/// </summary>
	public enum ButtonTheme
	{
		Dark,
		Light
	}

	/// <summary>
	/// The Button class represents an interactive UI element.
	/// It handles layout, appearance, mouse input, and text rendering.
	/// Implements IResize for responsive UI layout.
	/// </summary>
	public class Button : IResize
	{
		// Defines possible states for the button.
		protected enum Button_State
		{
			Normal,
			Hovered,
			Pressed,
			Disabled
		}

		// Button layout and styling properties.
		protected Vector2 position;           // Position of the button on the screen.
		protected int width;                  // Overall width of the button.
		protected int height;                 // Overall height of the button.
		protected int borderSize;             // Size of the button border.
		protected ButtonTheme buttonTheme;    // Theme of the button which influences color and layout.
		protected Panel borderPanel;          // Panel representing the button border.
		protected Panel fillPanel;            // Panel representing the button fill area.
		protected bool isActive;              // Indicates if the button is active (clickable).
		protected Action onClick;             // Delegate for the action to perform on click.

		// Primary and secondary colors for the button styling.
		protected Color primaryColor;
		protected Color secondaryColor;

		// Current state of the button.
		protected Button_State buttonState;

		// Texture-related fields for rendering a graphic/icon on the button.
		protected int textureSize;
		protected Vector2 texturePosition;
		protected Color textureColor;
		protected Texture2D buttonTexture;

		// Font system and font-related fields for rendering text on the button.
		protected FontSystem fontSystem;
		protected string buttonOptionText;
		protected Vector2 buttonOptionTextPosition;
		protected Color buttonOptionTextColor;
		protected float buttonOptionTextFontSize;
		protected DynamicSpriteFont buttonOptionTextFont;

		// Optional additional text fields.
		protected string? buttonAdditionalText;
		protected Vector2 buttonAdditionalTextPosition;
		protected Color buttonAdditionalTextColor;
		protected float buttonAdditionalTextFontSize;
		protected DynamicSpriteFont buttonAdditionalTextFont;

		// Rectangles used for layout of text fields.
		protected Rectangle textField;
		protected Rectangle additionalTextField;

		/// <summary>
		/// Initializes a new instance of the Button class with the specified parameters.
		/// </summary>
		/// <param name="position">Screen position for the button.</param>
		/// <param name="width">The width of the button.</param>
		/// <param name="height">The height of the button.</param>
		/// <param name="borderSize">The thickness of the button border.</param>
		/// <param name="buttonOptionText">Primary text displayed on the button.</param>
		/// <param name="isActive">If true, the button is active and clickable; otherwise, it is disabled.</param>
		/// <param name="primaryColor">The primary color for button styling.</param>
		/// <param name="secondaryColor">The secondary color for button styling.</param>
		/// <param name="buttonTheme">The button theme affecting visual aspects.</param>
		/// <param name="onClick">Action to execute when the button is clicked.</param>
		/// <param name="buttonAdditionalText">Optional additional text for the button.</param>
		public Button(
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
			string? buttonAdditionalText = null)
		{
			this.position = position;
			this.width = width;
			this.height = height;
			this.borderSize = borderSize;
			this.buttonOptionText = buttonOptionText;
			this.buttonAdditionalText = buttonAdditionalText;
			this.isActive = isActive;
			this.primaryColor = primaryColor;
			this.secondaryColor = secondaryColor;
			this.buttonTheme = buttonTheme;
			this.onClick = onClick;

			// Set initial button state based on the active flag.
			if (isActive)
				buttonState = Button_State.Normal;
			else
				buttonState = Button_State.Disabled;

			// Initialize internal components and calculate layout.
			Initialize();
		}

		/// <summary>
		/// Performs the initial setup for the button UI elements.
		/// Creates border and fill panels and calculates the initial layout.
		/// </summary>
		private void Initialize()
		{
			// Create the border panel using the given dimensions and primary color.
			borderPanel = new Panel(width, height, position, primaryColor);
			// Create a dummy fill panel; dimensions will be set in CalculateLayout.
			fillPanel = new Panel(0, 0, Vector2.Zero, Color.White);

			// Calculate the layout of text fields, panels, and any texture positions.
			CalculateLayout();
		}

		/// <summary>
		/// Loads the button's texture and font from the provided content paths.
		/// </summary>
		/// <param name="content">ContentManager to load assets.</param>
		/// <param name="texturePath">Path to the button texture asset.</param>
		/// <param name="fontPath">Path to the font file for button text.</param>
		public void LoadContent(ContentManager content, string texturePath, string fontPath)
		{
			// Load the button texture if a path is provided.
			if (!string.IsNullOrEmpty(texturePath))
			{
				buttonTexture = content.Load<Texture2D>(texturePath);
			}
			// Load the font system and add the font file for later use.
			if (!string.IsNullOrEmpty(fontPath))
			{
				fontSystem = new FontSystem();
				fontSystem.AddFont(File.ReadAllBytes(fontPath));
			}

			// Update the fonts and text positions according to the current layout.
			UpdateFontsAndText();
		}

		// Variables for mouse tracking.
		private MouseState previousMouseState;
		private bool clickStartedInside;

		/// <summary>
		/// Updates the button state based on mouse input.
		/// Changes appearance based on whether the button is hovered, pressed, or inactive.
		/// </summary>
		/// <param name="gameTime">Provides the timing values for the update.</param>
		public void Update(GameTime gameTime)
		{
			if (isActive)
			{
				MouseState mouseState = Mouse.GetState();
				Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
				Rectangle buttonArea = new Rectangle((int)position.X, (int)position.Y, width, height);
				bool isHovering = buttonArea.Contains(mousePosition);

				// Handle mouse button pressing logic.
				if (mouseState.LeftButton == ButtonState.Pressed)
				{
					if (previousMouseState.LeftButton == ButtonState.Released)
					{
						// Record if the click started inside the button.
						clickStartedInside = isHovering;
						if (clickStartedInside)
						{
							// Trigger the onClick action if the click started inside.
							onClick?.Invoke();
						}
					}

					// If mouse leaves the button area while pressed, reset the state.
					if (!isHovering)
					{
						clickStartedInside = false;
						buttonState = Button_State.Normal;
					}
					else
					{
						// Update button state to pressed if the click continues inside.
						if (clickStartedInside)
						{
							buttonState = Button_State.Pressed;
						}
						else
						{
							buttonState = Button_State.Hovered;
						}
					}
				}
				else
				{
					// Set the button state based on hover status when not pressed.
					if (isHovering)
					{
						buttonState = Button_State.Hovered;
					}
					else
					{
						buttonState = Button_State.Normal;
					}

					clickStartedInside = false;
				}

				// Update colors based on the button state.
				switch (buttonState)
				{
					case Button_State.Normal:
						UpdateColors(primaryColor, secondaryColor);
						break;
					case Button_State.Hovered:
						UpdateColors(ModifyColor(primaryColor, 50), ModifyColor(secondaryColor, 50));
						break;
					case Button_State.Pressed:
						UpdateColors(ModifyColor(primaryColor, -50), ModifyColor(secondaryColor, -50));
						break;
				}

				// Save the current mouse state for the next update cycle.
				previousMouseState = mouseState;
			}
			else
			{
				// When inactive, mark button as disabled and darken its colors.
				buttonState = Button_State.Disabled;
				UpdateColors(ModifyColor(primaryColor, -100), ModifyColor(secondaryColor, -100));
			}
		}

		/// <summary>
		/// Renders the button's panels, texture, and text.
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch used for drawing the button elements.</param>
		public void Draw(SpriteBatch spriteBatch)
		{
			// Draw the border and fill panels.
			borderPanel.Draw(spriteBatch);
			fillPanel.Draw(spriteBatch);

			// Draw the button texture if it exists.
			if (buttonTexture != null)
			{
				spriteBatch.Begin();
				spriteBatch.Draw(
					buttonTexture,
					new Rectangle(
						(int)texturePosition.X,
						(int)texturePosition.Y,
						textureSize,
						textureSize
					),
					textureColor
				);
				spriteBatch.End();
			}

			// Draw the primary button text.
			if (buttonOptionTextFont != null)
			{
				spriteBatch.Begin();
				buttonOptionTextFont.DrawText(
					spriteBatch,
					buttonOptionText,
					buttonOptionTextPosition,
					buttonOptionTextColor
				);
				spriteBatch.End();
			}

			// Draw the additional button text if provided.
			if (buttonAdditionalTextFont != null)
			{
				spriteBatch.Begin();
				buttonAdditionalTextFont.DrawText(
					spriteBatch,
					buttonAdditionalText,
					buttonAdditionalTextPosition,
					buttonAdditionalTextColor
				);
				spriteBatch.End();
			}
		}

		/// <summary>
		/// Resizes the button based on a new rectangle.
		/// Updates the layout and font positions.
		/// </summary>
		/// <param name="newRect">The new rectangle for the button dimensions and position.</param>
		public void Resize(Rectangle newRect)
		{
			position.X = newRect.X;
			position.Y = newRect.Y;
			width = newRect.Width;
			height = newRect.Height;

			// Recalculate layout and update text positions.
			CalculateLayout();
			UpdateFontsAndText();
		}

		/// <summary>
		/// Updates the colors of the button based on the provided main and secondary colors.
		/// Adjusts border, fill, texture, and text colors according to the theme.
		/// </summary>
		/// <param name="mainColor">The main color to apply.</param>
		/// <param name="secondColor">The secondary color to apply.</param>
		private void UpdateColors(Color mainColor, Color secondColor)
		{
			borderPanel.FillColor = mainColor;
			fillPanel.FillColor = secondColor;

			// For the texture and text, swap colors based on the current theme.
			textureColor = buttonTheme == ButtonTheme.Light ? secondColor : mainColor;
			buttonOptionTextColor = buttonTheme == ButtonTheme.Light ? mainColor : secondColor;
			buttonAdditionalTextColor = buttonTheme == ButtonTheme.Light ? secondColor : mainColor;
		}

		/// <summary>
		/// Adjusts a given color's brightness.
		/// </summary>
		/// <param name="color">The original color.</param>
		/// <param name="brightness">The brightness modifier (positive to lighten, negative to darken).</param>
		/// <returns>A new Color instance after applying brightness adjustment.</returns>
		private static Color ModifyColor(Color color, int brightness)
		{
			int r = color.R + brightness;
			int g = color.G + brightness;
			int b = color.B + brightness;

			// Clamp the color channels to valid values.
			r = MathHelper.Clamp(r, 0, 255);
			g = MathHelper.Clamp(g, 0, 255);
			b = MathHelper.Clamp(b, 0, 255);

			return new Color(r, g, b);
		}

		/// <summary>
		/// Updates the fonts and text positions for both the primary and additional texts.
		/// Calculates font size proportional to text field dimensions.
		/// </summary>
		protected void UpdateFontsAndText()
		{
			// If the font system has not been loaded, exit early.
			if (fontSystem == null)
				return;

			// Calculate the primary text font size based on the text field dimensions.
			buttonOptionTextFontSize = Math.Max(textField.Width, textField.Height) * 0.075f;
			buttonOptionTextFont = fontSystem.GetFont(buttonOptionTextFontSize);

			// Calculate the additional text font size based on its designated field.
			buttonAdditionalTextFontSize = Math.Max(additionalTextField.Width, additionalTextField.Height) * 0.05f;
			buttonAdditionalTextFont = fontSystem.GetFont(buttonAdditionalTextFontSize);

			// Center the primary text within the text field.
			var textSize = buttonOptionTextFont.MeasureString(buttonOptionText);
			buttonOptionTextPosition = new Vector2(
				textField.X + (textField.Width - textSize.X) / 2,
				textField.Y + (textField.Height - textSize.Y) / 2
			);

			// Center the additional text within its field.
			var additionalTextSize = buttonAdditionalTextFont.MeasureString(buttonAdditionalText);
			buttonAdditionalTextPosition = new Vector2(
				additionalTextField.X + (additionalTextField.Width - additionalTextSize.X) / 2,
				additionalTextField.Y + (additionalTextField.Height - additionalTextSize.Y) / 2
			);
		}

		/// <summary>
		/// Calculates the layout of all button components including text fields,
		/// panels, and texture positioning depending on the button theme.
		/// </summary>
		protected void CalculateLayout()
		{
			// Define the primary text field occupying the lower 25% of the button area.
			textField = new Rectangle(
				(int)position.X,
				(int)(position.Y + (0.75 * height)),
				width,
				(int)(0.25 * height)
			);
			// Initialize additional text field.
			additionalTextField = new Rectangle();

			// If additional text is provided, allocate a space above the text field.
			if (!string.IsNullOrEmpty(buttonAdditionalText))
			{
				float newHeight = height * 0.2f;
				float newY = textField.Y - newHeight;

				additionalTextField = new Rectangle(
					(int)position.X,
					(int)newY,
					width,
					(int)newHeight
				);
			}

			// Update border panel layout.
			borderPanel.Width = width;
			borderPanel.Height = height;
			borderPanel.Position = position;
			borderPanel.FillColor = primaryColor;

			int fillPanelWidth;
			int fillPanelHeight;
			float fillPanelPosX;
			float fillPanelPosY;
			var fillPanelColor = secondaryColor;

			// Layout adjustments depend on the chosen button theme.
			if (buttonTheme == ButtonTheme.Light)
			{
				fillPanelWidth = width - 2 * borderSize;
				fillPanelHeight = height / 4;
				fillPanelPosX = position.X + borderSize;
				fillPanelPosY = position.Y + height - fillPanelHeight - borderSize;

				textureSize = Math.Max(width, height - fillPanelHeight) / 2;
				texturePosition = new Vector2(
					position.X + (width - textureSize) / 2,
					position.Y + (height - fillPanelHeight - textureSize) / 2
				);
				textureColor = secondaryColor;

				buttonOptionTextColor = primaryColor;
				buttonAdditionalTextColor = secondaryColor;
			}
			else
			{
				fillPanelWidth = width - (2 * borderSize);
				fillPanelHeight = (height / 4) * 3;
				fillPanelPosX = position.X + borderSize;
				fillPanelPosY = position.Y + borderSize;

				textureSize = Math.Max(width, fillPanelHeight) / 2;
				texturePosition = new Vector2(
					position.X + (width - textureSize) / 2,
					position.Y + (fillPanelHeight - textureSize) / 2
				);
				textureColor = primaryColor;

				buttonOptionTextColor = secondaryColor;
				buttonAdditionalTextColor = primaryColor;
			}

			// Update fill panel with calculated dimensions and position.
			fillPanel.Width = fillPanelWidth;
			fillPanel.Height = fillPanelHeight;
			fillPanel.Position = new Vector2(fillPanelPosX, fillPanelPosY);
			fillPanel.FillColor = fillPanelColor;
		}

		/// <summary>
		/// Sets new primary button text and updates the font and text layout accordingly.
		/// </summary>
		/// <param name="newText">The new text to display as the primary button option.</param>
		public void SetButtonOptionText(string newText)
		{
			buttonOptionText = newText;
			UpdateFontsAndText();
		}
	}
}
