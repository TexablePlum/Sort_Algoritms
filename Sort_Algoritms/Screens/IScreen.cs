using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sort_Algoritms.Screens
{
	/// <summary>
	/// Represents a general interface for game screens.
	/// Each screen (e.g., start screen, main menu, visualizer, etc.) should implement
	/// this interface to support initialization, content loading, updates, and drawing.
	/// </summary>
	public interface IScreen
	{
		/// <summary>
		/// Initializes the screen. This method is used to set up variables,
		/// configure screen settings, or any other startup tasks before content is loaded.
		/// </summary>
		void Initialize();

		/// <summary>
		/// Loads the content needed by the screen.
		/// Typically used to load textures, fonts, and other assets from the ContentManager.
		/// </summary>
		/// <param name="content">The ContentManager used to load resources.</param>
		void LoadContent(ContentManager content);

		/// <summary>
		/// Updates the screen logic.
		/// This method is called every frame to process input, update game state, animations, etc.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		void Update(GameTime gameTime);

		/// <summary>
		/// Draws the screen content.
		/// This method is responsible for rendering the current state of the screen using the provided SpriteBatch.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch used for drawing 2D textures.</param>
		void Draw(SpriteBatch spriteBatch);
	}
}
