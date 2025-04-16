using Microsoft.Xna.Framework;

namespace UI
{
	/// <summary>
	/// IResize interface defines a method to handle dynamic resizing of UI elements.
	/// Classes implementing this interface should update their layout and dimensions
	/// based on the provided new rectangle.
	/// </summary>
	public interface IResize
	{
		/// <summary>
		/// Resizes the UI element to match the dimensions and position of the given rectangle.
		/// </summary>
		/// <param name="newRect">A rectangle representing the new size and position for the element.</param>
		void Resize(Rectangle newRect);
	}
}
