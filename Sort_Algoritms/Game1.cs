using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sort_Algoritms.Algorithms;
using Sort_Algoritms.Screens;
using System;

namespace Sort_Algoritms
{
	/// <summary>
	/// Main class for the sorting algorithms visualization application.
	/// This class handles graphics configuration, screen management, the game loop,
	/// and loading/saving user settings.
	/// </summary>
	public class Game1 : Game
	{
		// Game window properties and visualization settings.
		public static int ScreenWidth;			// Width of the game window.
		public static int ScreenHeight;			// Height of the game window.
		public static int LinesCount;			// Number of lines to visualize.
		public static int LinesMargin;			// Margin between each visual line.
		public static int ScreenMargin;			// Margin for content within the window.
		public static int AlgorithmDelay;		// Delay in milliseconds between sorting steps.
		public static string AppName;			// Application name/identifier.

		// Sorting algorithm and visualization appearance.
		public static SortAlgorithm Algorithm;          // Instance of the sort algorithm (default is Bubble Sort).
		public static Color LinesColor = Color.White;	// Color used for drawing lines.

		// Delegate for exiting the application.
		public static Action OnExit;

		// Screen management.
		public static ScreenManager ScreenManager;

		// Graphics-related members.
		public static GraphicsDeviceManager _graphics;
		public static SpriteBatch _spriteBatch;

		// Stores settings loaded from file.
		private Settings settings;

		/// <summary>
		/// Constructor: initializes graphics settings, loads settings from file,
		/// applies them to static variables, and sets up a delegate for exiting the application.
		/// </summary>
		public Game1()
		{
			// Initialize graphics device manager.
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;

			// Set up the exit action to save settings on exit.
			OnExit = () => { SaveCurrentSettings(); Exit(); };

			// Load settings from file.
			settings = SettingsManager.LoadSettings();
			// Apply loaded settings.
			ScreenWidth = settings.ScreenWidth;
			ScreenHeight = settings.ScreenHeight;
			LinesCount = settings.LinesCount;
			LinesMargin = settings.LinesMargin;
			ScreenMargin = settings.ScreenMargin;
			AlgorithmDelay = settings.AlgorithmDelay;
			AppName = settings.AppName;

			// Set the default sorting algorithm (Bubble Sort).
			Algorithm = new BubbleSort();
			// Initialize the screen manager.
			ScreenManager = new ScreenManager(Content);
		}

		/// <summary>
		/// Initialize method: Called before the game loop starts.
		/// Sets up window dimensions, graphics settings, and default game components.
		/// Also forces full-screen mode if the loaded resolution matches the native resolution.
		/// </summary>
		protected override void Initialize()
		{
			// Set preferred back buffer dimensions.
			_graphics.PreferredBackBufferWidth = ScreenWidth;
			_graphics.PreferredBackBufferHeight = ScreenHeight;

			// Enable Multi-Sample Anti-Aliasing (MSAA) for smoother graphics.
			_graphics.PreferMultiSampling = true;
			_graphics.GraphicsDevice.PresentationParameters.MultiSampleCount = 8;

			// Determine native display resolution.
			int nativeWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			int nativeHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

			// Set full screen if the loaded resolution matches the native resolution.
			if (ScreenWidth == nativeWidth && ScreenHeight == nativeHeight)
			{
				_graphics.IsFullScreen = true;
			}
			else
			{
				_graphics.IsFullScreen = false;
			}
			_graphics.ApplyChanges();

			base.Initialize();
		}

		/// <summary>
		/// LoadContent method: Called once per game.
		/// Initializes the SpriteBatch and sets the starting screen.
		/// </summary>
		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			// Change to the initial screen (StartScreen) using the screen manager.
			ScreenManager.ChangeScreen<StartScreen>();
		}

		/// <summary>
		/// Update method: Called every frame to update game logic.
		/// Delegates update processing to the current screen.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			ScreenManager.Update(gameTime);
			base.Update(gameTime);
		}

		/// <summary>
		/// Draw method: Called every frame to render the game.
		/// Clears the screen and delegates drawing to the current screen.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			ScreenManager.Draw(_spriteBatch);
			base.Draw(gameTime);
		}

		/// <summary>
		/// Saves the current settings to a file using SettingsManager.
		/// Called before exiting the application.
		/// </summary>
		public static void SaveCurrentSettings()
		{
			Settings currentSettings = new Settings
			{
				ScreenWidth = ScreenWidth,
				ScreenHeight = ScreenHeight,
				LinesCount = LinesCount,
				LinesMargin = LinesMargin,
				ScreenMargin = ScreenMargin,
				AlgorithmDelay = AlgorithmDelay,
				AppName = AppName
			};

			SettingsManager.SaveSettings(currentSettings);
		}
	}
}
