using Microsoft.Xna.Framework;

namespace UI
{
	/// <summary>
	/// Represents an option for a button in the lower panel, where a portion with a key label (e.g., "[ESC]")
	/// can be rendered in a different color than the rest of the text.
	/// </summary>
	public class KeyCapOption
	{
		/// <summary>
		/// Gets or sets the text representing the key (e.g., "[ESC]").
		/// </summary>
		public string KeyText { get; set; }

		/// <summary>
		/// Gets or sets the color used for rendering the key text.
		/// </summary>
		public Color KeyColor { get; set; }

		/// <summary>
		/// Gets or sets the remainder of the option text (e.g., " Settings").
		/// </summary>
		public string OptionText { get; set; }

		/// <summary>
		/// Gets or sets the color used for rendering the remainder of the option text.
		/// </summary>
		public Color OptionColor { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the option is active.
		/// By default, this property is set to true.
		/// </summary>
		public bool IsActive { get; set; } = true;

		/// <summary>
		/// Constructs a new instance of KeyCapOption with the specified key text, key color,
		/// option text, and option color.
		/// </summary>
		/// <param name="keyText">The text representing the key (e.g., "[ESC]").</param>
		/// <param name="keyColor">The color for the key text.</param>
		/// <param name="optionText">The additional text for the option (e.g., " Settings").</param>
		/// <param name="optionColor">The color for the option text.</param>
		public KeyCapOption(string keyText, Color keyColor, string optionText, Color optionColor)
		{
			KeyText = keyText;
			KeyColor = keyColor;
			OptionText = optionText;
			OptionColor = optionColor;
		}
	}
}
