using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sort_Algoritms.Algorithms;
using System.Collections.Generic;
using System.Threading;
using UI;

namespace Sort_Algoritms.Screens
{
	/// <summary>
	/// MainScreen is the primary screen where the visualization and sorting take place.
	/// It contains a container that holds the lines for visualization, and a bottom info panel 
	/// that displays key options for user interactions such as randomizing, sorting, stopping, or opening settings.
	/// The screen responds to keyboard inputs to trigger sorting or shuffle operations.
	/// </summary>
	public class MainScreen : IScreen
	{
		// The container holds the lines which represent the elements to be sorted.
		private Container container;
		// The currently active sorting algorithm.
		private SortAlgorithm sorter;
		// Graphics device obtained from the main game class.
		private GraphicsDevice _graphicsDevice;
		// The bottom info panel displays key option instructions and the timer.
		private BottomInfoPanel bottomInfoPanel;

		// Dimensions of the sorting container (full screen minus a bottom margin).
		private int width;
		private int height;
		private int bottomMargin = 30;

		// Flags to track previous keyboard states to detect new key presses.
		private bool wasSpacePressed = false;
		private bool wasEPressed = false;
		private bool wasSPressed = false;
		private bool wasEscapePressed = false;

		/// <summary>
		/// Initializes the MainScreen.
		/// Sets up the graphics, creates and initializes the container, sets the UI parameters,
		/// and initializes the bottom info panel with key options.
		/// </summary>
		public void Initialize()
		{
			_graphicsDevice = Game1._graphics.GraphicsDevice;

			// Set container dimensions: full screen width and screen height minus bottom margin.
			width = Game1.ScreenWidth;
			height = Game1.ScreenHeight - bottomMargin;

			// Initialize the container which will hold the lines.
			container = new Container(new Vector2(width, height), Game1.ScreenMargin, Game1.LinesCount, Game1.LinesMargin, Game1.LinesColor);
			// Initialize the static aspects of the Line class with the graphics device.
			Line.Initialize(_graphicsDevice);

			// Set up the bottom info panel which displays key options and timer.
			bottomInfoPanel = new BottomInfoPanel(
				width,
				Game1.ScreenMargin + bottomMargin,
				new Vector2(0, height - Game1.ScreenMargin),
				Color.Black,
				new List<KeyCapOption>
				{
					new KeyCapOption("[SPACE]", Color.Yellow, " Randomize", Color.White),
					new KeyCapOption("[E]", Color.Yellow, " Sort", Color.White),
					new KeyCapOption("[S]", Color.Yellow, " Stop", Color.White),
					new KeyCapOption("[ESC]", Color.Yellow, " Settings", Color.White)
				},
				Color.White,
				Color.Yellow,
				Game1.ScreenMargin
			);
		}

		/// <summary>
		/// Loads content (textures, fonts, etc.) for the MainScreen.
		/// Loads content for the container and the bottom info panel, as well as all buttons used on the start screen.
		/// </summary>
		/// <param name="content">ContentManager used to load assets.</param>
		public void LoadContent(ContentManager content)
		{
			// Load the container's content (creates the lines).
			container.Load();
			// Load content for the bottom info panel including its font.
			bottomInfoPanel.LoadContent(content, "Content/Fonts/8-bit Operator+ 8 Regular.ttf");
		}

		/// <summary>
		/// Updates the MainScreen on each frame.
		/// Handles dynamic resizing of UI elements, updates the container and bottom info panel,
		/// and processes keyboard input to perform shuffle, sort, stop, or switch screens.
		/// </summary>
		/// <param name="gameTime">GameTime providing elapsed time information for updates.</param>
		public void Update(GameTime gameTime)
		{
			// If resolution changes occur, adjust dimensions and resize bottom panel.
			if (width != Game1.ScreenWidth || height != Game1.ScreenHeight - bottomMargin)
			{
				width = Game1.ScreenWidth;
				height = Game1.ScreenHeight - bottomMargin;

				// Create a new rectangle for the bottom info panel.
				Rectangle newPanelRect = new Rectangle(0, height - Game1.ScreenMargin, width, Game1.ScreenMargin + bottomMargin);
				bottomInfoPanel.Resize(newPanelRect);
			}

			// Update the container based on current dimensions, number of lines, and margins.
			container.Update(new Vector2(width, height), Game1.LinesCount, Game1.LinesMargin);

			// Read current keyboard state.
			KeyboardState keyboardState = Keyboard.GetState();
			bool isSpacePressed = keyboardState.IsKeyDown(Keys.Space);
			bool isEPressed = keyboardState.IsKeyDown(Keys.E);
			bool isEscapePressed = keyboardState.IsKeyDown(Keys.Escape);
			bool isSPressed = keyboardState.IsKeyDown(Keys.S);

			// If no sort is currently in progress...
			if (!Game1.Algorithm.IsSorting)
			{
				// Enable relevant options in bottom info panel.
				bottomInfoPanel.SetOptionActive(0, true);
				bottomInfoPanel.SetOptionActive(1, true);

				// If SPACE is pressed (new press), trigger a randomization (shuffle).
				if (isSpacePressed && !wasSpacePressed)
				{
					bottomInfoPanel.ResetTimer();
					sorter = new RandomizeShuffle();
					sorter.Sort(container.Lines);
					container.IsSorted = false;
				}
				// If E is pressed (new press) and the container is not sorted, start sorting.
				if (isEPressed && !wasEPressed && !container.IsSorted)
				{
					bottomInfoPanel.StartTimer();
					sorter = Game1.Algorithm;
					// Reset the cancellation token to allow a fresh sorting operation.
					Game1.Algorithm.CancelTokenSource = new CancellationTokenSource();
					sorter.Sort(container.Lines, Game1.AlgorithmDelay, Game1.Algorithm.CancelTokenSource.Token);
					container.IsSorted = true;
				}
			}
			else
			{
				// If sorting is in progress, disable certain options.
				bottomInfoPanel.SetOptionActive(0, false);
				bottomInfoPanel.SetOptionActive(1, false);
			}

			// If S is pressed (new press) while sorting, stop the sort.
			if (isSPressed && !wasSPressed)
			{
				if (Game1.Algorithm != null && Game1.Algorithm.IsSorting && !Game1.Algorithm.IsFeelingLinesGreen)
				{
					Game1.Algorithm.StopSort(container.Lines);
					bottomInfoPanel.StopTimer();
					container.IsSorted = false;
				}
			}

			// If ESCAPE is pressed (new press), stop sorting (if active) and change to the StartScreen.
			if (isEscapePressed && !wasEscapePressed)
			{
				if (Game1.Algorithm != null && Game1.Algorithm.IsSorting && !Game1.Algorithm.IsFeelingLinesGreen)
				{
					Game1.Algorithm.StopSort(container.Lines);
					bottomInfoPanel.StopTimer();
					container.IsSorted = false;
				}
				Game1.ScreenManager.ChangeScreen<StartScreen>();
			}

			// Save current keyboard states for edge detection in the next frame.
			wasSpacePressed = isSpacePressed;
			wasEPressed = isEPressed;
			wasSPressed = isSPressed;
			wasEscapePressed = isEscapePressed;

			// If the algorithm has finished sorting, ensure the timer is stopped.
			if (Game1.Algorithm != null && !Game1.Algorithm.IsSorting)
			{
				bottomInfoPanel.StopTimer();
			}

			// Update the bottom info panel.
			bottomInfoPanel.Update(gameTime);
		}

		/// <summary>
		/// Draws the MainScreen.
		/// Renders the container (visualization of sorting) and the bottom info panel.
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch used for drawing.</param>
		public void Draw(SpriteBatch spriteBatch)
		{
			// Draw the sorted container (lines).
			container.Draw(spriteBatch);
			// Draw the bottom info panel that contains key instructions and timer.
			bottomInfoPanel.Draw(spriteBatch);
		}
	}
}
