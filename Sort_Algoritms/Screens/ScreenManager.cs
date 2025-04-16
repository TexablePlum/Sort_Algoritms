using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace Sort_Algoritms.Screens
{
	/// <summary>
	/// Manages the different screens (game states) used in the application.
	/// It handles screen transitions, updates, and draws the current active screen.
	/// Screens are cached to avoid recreating them upon every transition.
	/// </summary>
	public class ScreenManager
	{
		// The currently active screen.
		private IScreen currentScreen;
		// ContentManager to load assets needed by the screens.
		private readonly ContentManager contentManager;

		// Cache for screens to avoid recreating them multiple times.
		private Dictionary<Type, IScreen> screenCache;

		/// <summary>
		/// Constructs a new ScreenManager using the provided ContentManager.
		/// </summary>
		/// <param name="content">The ContentManager used to load screen assets.</param>
		public ScreenManager(ContentManager content)
		{
			contentManager = content;
			screenCache = new Dictionary<Type, IScreen>();
		}

		/// <summary>
		/// Changes the current screen to a new screen of type T.
		/// If the screen has been created before, it is retrieved from the cache; otherwise, it is created.
		/// </summary>
		/// <typeparam name="T">The type of the screen implementing IScreen to switch to.</typeparam>
		public void ChangeScreen<T>() where T : IScreen, new()
		{
			currentScreen = GetOrCreateScreen<T>();
		}

		/// <summary>
		/// Updates the current active screen.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			currentScreen?.Update(gameTime);
		}

		/// <summary>
		/// Draws the current active screen.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch used for drawing screen elements.</param>
		public void Draw(SpriteBatch spriteBatch)
		{
			currentScreen?.Draw(spriteBatch);
		}

		/// <summary>
		/// Retrieves a screen of type T from the cache if it exists, or creates a new instance.
		/// Newly created screens are initialized and their content is loaded.
		/// </summary>
		/// <typeparam name="T">The type of screen to retrieve or create.</typeparam>
		/// <returns>An instance of the screen of type T.</returns>
		private T GetOrCreateScreen<T>() where T : IScreen, new()
		{
			var type = typeof(T);
			if (!screenCache.ContainsKey(type))
			{
				// Create new screen instance since it was not cached.
				var screen = new T();
				// Initialize the screen (set up variables, initial state).
				screen.Initialize();
				// Load necessary content (textures, fonts, etc.) using the ContentManager.
				screen.LoadContent(contentManager);
				// Cache the created screen for future use.
				screenCache[type] = screen;
			}
			// Return the cached screen.
			return (T)screenCache[type];
		}
	}
}
