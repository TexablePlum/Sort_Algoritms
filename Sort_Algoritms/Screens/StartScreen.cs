using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sort_Algoritms.Algorithms;
using System.Collections.Generic;
using UI;

namespace Sort_Algoritms.Screens
{
	/// <summary>
	/// Represents the initial start screen for the application.
	/// Displays the logo, various buttons to control the application (e.g., resolution, algorithm selection, start, lines count, delay, exit),
	/// as well as option panels for selecting resolution and sorting algorithm.
	/// </summary>
	public class StartScreen : IScreen
	{
		// Graphics devices used for rendering.
		private GraphicsDevice _graphicsDevice;
		private GraphicsDeviceManager _graphicsDeviceManager;

		// UI elements for the start screen.
		private LogoPanel logoPanel;
		private Grid buttonsGrid;

		// Buttons for different actions on the start screen.
		private Button resolutionButton;
		private Button algorithmButton;
		private Button startButton;
		private TextBoxButton linesCountButton;
		private TextBoxButton delayButton;
		private Button exitButton;

		// OptionListPanel for selecting screen resolution.
		private OptionListPanel resolutionPanel;
		private bool isResolutionPanelVisible;         // Flag to control visibility of the resolution panel.
		private float resolutionPanelCurrentX;          // Current X position of the resolution panel (for sliding animation).
		private int resolutionPanelWidth;
		private int resolutionPanelHeight;
		private float slideSpeed = 1000f;               // Speed for sliding animation in pixels per second.

		// OptionListPanel for selecting sorting algorithm.
		private OptionListPanel algorithmPanel;
		private bool isAlgorithmPanelVisible;           // Flag to control visibility of the algorithm panel.
		private float algorithmPanelCurrentX;           // Current X position of the algorithm panel.
		private int algorithmPanelWidth;
		private int algorithmPanelHeight;
		private float slideSpeedAlgorithm = 1000f;        // Speed for sliding animation for the algorithm panel.

		// Default options displayed on the buttons.
		private string resolutionOption = $"{Game1.ScreenWidth}X{Game1.ScreenHeight}";
		private string algorithmOption = Game1.Algorithm.Name;
		private string linesCountOption = $"{Game1.LinesCount}";
		private string delayOption = $"{Game1.AlgorithmDelay}";

		// File path for the font to be used in the UI.
		string fontPath = "Content/Fonts/8-bit Operator+ 8 Regular.ttf";
		// Array of texture paths (icons) used for the buttons.
		private string[] iconPaths = new string[]
		{
			"Icons/resolution",
			"Icons/algorithm",
			"Icons/play",
			"Icons/lines",
			"Icons/delay",
			"Icons/exit"
		};

		/// <summary>
		/// Initializes the start screen by setting up the logo, buttons grid, individual buttons,
		/// as well as the resolution and algorithm option panels.
		/// </summary>
		public void Initialize()
		{
			// Retrieve graphics device and manager from the Game1 class.
			_graphicsDevice = Game1._graphics.GraphicsDevice;
			_graphicsDeviceManager = Game1._graphics;

			// Set up the logo panel to occupy the top 20% of the screen.
			logoPanel = new LogoPanel(
				new Rectangle(0, 0, Game1.ScreenWidth, (int)(Game1.ScreenHeight * 0.2f)),
				Game1.AppName,
				textColor: Color.White,
				textSizeRatio: 0.25f
			);

			// Create a grid for placing buttons, with 2 rows and 3 columns, margins, and offsets.
			buttonsGrid = new Grid(
				Game1.ScreenWidth,
				Game1.ScreenHeight,
				2,
				3,
				0.1f, 0.1f,
				0.2f, 0.2f,
				0.75f
			);

			// Set up all UI buttons.
			SetupButtons();
			// Initialize the resolution selection panel.
			SetupResolutionPanel();
			// Initialize the algorithm selection panel.
			SetupAlgorithmPanel();
		}

		/// <summary>
		/// Loads the content (fonts, textures) for all UI components.
		/// </summary>
		/// <param name="content">ContentManager to load assets.</param>
		public void LoadContent(ContentManager content)
		{
			// Load font content for the logo panel.
			logoPanel.LoadContent(fontPath);
			// Load grid content (mostly for drawing grid lines).
			buttonsGrid.LoadContent(_graphicsDevice);

			// Load content (icon textures and fonts) for each button.
			resolutionButton.LoadContent(content, texturePath: iconPaths[0], fontPath: fontPath);
			algorithmButton.LoadContent(content, texturePath: iconPaths[1], fontPath: fontPath);
			startButton.LoadContent(content, texturePath: iconPaths[2], fontPath: fontPath);
			linesCountButton.LoadContent(content, texturePath: iconPaths[3], fontPath: fontPath);
			delayButton.LoadContent(content, texturePath: iconPaths[4], fontPath: fontPath);
			exitButton.LoadContent(content, texturePath: iconPaths[5], fontPath: fontPath);

			// Load content for the resolution and algorithm option panels.
			resolutionPanel.LoadContent(content, fontPath);
			algorithmPanel.LoadContent(content, fontPath);
		}

		/// <summary>
		/// Updates all UI elements on the start screen.
		/// This includes updating buttons and handling sliding animations for option panels.
		/// Also updates global settings based on TextBoxButton values (lines count and delay).
		/// </summary>
		/// <param name="gameTime">Timing information for the update.</param>
		public void Update(GameTime gameTime)
		{
			// Update individual buttons.
			resolutionButton.Update(gameTime);
			algorithmButton.Update(gameTime);
			startButton.Update(gameTime);
			linesCountButton.Update(gameTime);
			delayButton.Update(gameTime);
			exitButton.Update(gameTime);

			// --- Update Resolution Panel Sliding Animation ---
			// Determine target X position: 0 if visible; otherwise, off-screen to the left.
			float targetXRes = isResolutionPanelVisible ? 0 : -resolutionPanelWidth;
			float deltaRes = slideSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (resolutionPanelCurrentX < targetXRes)
			{
				resolutionPanelCurrentX += deltaRes;
				if (resolutionPanelCurrentX > targetXRes)
					resolutionPanelCurrentX = targetXRes;
			}
			else if (resolutionPanelCurrentX > targetXRes)
			{
				resolutionPanelCurrentX -= deltaRes;
				if (resolutionPanelCurrentX < targetXRes)
					resolutionPanelCurrentX = targetXRes;
			}
			// Resize and update the resolution panel with its new position.
			resolutionPanel.Resize(new Rectangle((int)resolutionPanelCurrentX, 0, resolutionPanelWidth, resolutionPanelHeight));
			resolutionPanel.Update(gameTime);

			// --- Update Algorithm Panel Sliding Animation ---
			float targetXAlgo = isAlgorithmPanelVisible ? 0 : -algorithmPanelWidth;
			float deltaAlgo = slideSpeedAlgorithm * (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (algorithmPanelCurrentX < targetXAlgo)
			{
				algorithmPanelCurrentX += deltaAlgo;
				if (algorithmPanelCurrentX > targetXAlgo)
					algorithmPanelCurrentX = targetXAlgo;
			}
			else if (algorithmPanelCurrentX > targetXAlgo)
			{
				algorithmPanelCurrentX -= deltaAlgo;
				if (algorithmPanelCurrentX < targetXAlgo)
					algorithmPanelCurrentX = targetXAlgo;
			}
			algorithmPanel.Resize(new Rectangle((int)algorithmPanelCurrentX, 0, algorithmPanelWidth, algorithmPanelHeight));
			algorithmPanel.Update(gameTime);

			// Update global settings from UI TextBoxButton values.
			Game1.LinesCount = linesCountButton.Value;
			Game1.AlgorithmDelay = delayButton.Value;
		}

		/// <summary>
		/// Draws the start screen UI.
		/// Renders the logo, buttons, and the two option panels.
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch used for drawing the UI elements.</param>
		public void Draw(SpriteBatch spriteBatch)
		{
			// Draw logo at the top.
			logoPanel.Draw(spriteBatch);

			// Draw all the main buttons.
			resolutionButton.Draw(spriteBatch);
			algorithmButton.Draw(spriteBatch);
			startButton.Draw(spriteBatch);
			linesCountButton.Draw(spriteBatch);
			delayButton.Draw(spriteBatch);
			exitButton.Draw(spriteBatch);

			// Draw the resolution and algorithm option panels.
			resolutionPanel.Draw(spriteBatch);
			algorithmPanel.Draw(spriteBatch);
		}

		/// <summary>
		/// Configures the main buttons that appear on the start screen.
		/// Uses the grid layout to position each button.
		/// </summary>
		private void SetupButtons()
		{
			List<Rectangle> cellRectangles = buttonsGrid.GetCellRectangles();

			// --- Resolution Button ---
			resolutionButton = new Button(
				new Vector2(cellRectangles[0].X, cellRectangles[0].Y),
				cellRectangles[0].Width,
				cellRectangles[0].Height,
				borderSize: 2,
				buttonOptionText: resolutionOption,
				isActive: true,
				primaryColor: Color.White,
				secondaryColor: Color.Black,
				buttonTheme: ButtonTheme.Dark,
				onClick: () => { isResolutionPanelVisible = !isResolutionPanelVisible; },
				buttonAdditionalText: "SCREEN_RESOLUTION"
			);

			// --- Algorithm Button ---
			algorithmButton = new Button(
				new Vector2(cellRectangles[1].X, cellRectangles[1].Y),
				cellRectangles[1].Width,
				cellRectangles[1].Height,
				borderSize: 2,
				buttonOptionText: algorithmOption,
				isActive: true,
				primaryColor: Color.White,
				secondaryColor: Color.Black,
				buttonTheme: ButtonTheme.Dark,
				onClick: () => { isAlgorithmPanelVisible = !isAlgorithmPanelVisible; },
				buttonAdditionalText: "SORTING_ALGORITHM"
			);

			// --- Start Button ---
			startButton = new Button(
				new Vector2(cellRectangles[2].X, cellRectangles[2].Y),
				cellRectangles[2].Width,
				cellRectangles[2].Height,
				borderSize: 2,
				buttonOptionText: "START",
				isActive: true,
				primaryColor: Color.White,
				secondaryColor: Color.Black,
				buttonTheme: ButtonTheme.Light,
				onClick: () => { Game1.SaveCurrentSettings(); Game1.ScreenManager.ChangeScreen<MainScreen>(); },
				buttonAdditionalText: ""
			);

			// --- Lines Count Button ---
			linesCountButton = new TextBoxButton(
				new Vector2(cellRectangles[3].X, cellRectangles[3].Y),
				cellRectangles[3].Width,
				cellRectangles[3].Height,
				borderSize: 2,
				buttonOptionText: linesCountOption,
				isActive: true,
				primaryColor: Color.White,
				secondaryColor: Color.Black,
				buttonTheme: ButtonTheme.Dark,
				onClick: () => { },
				buttonAdditionalText: "LINES_COUNT",
				fixedSuffix: "_LINES",
				validator: (text) =>
				{
					if (int.TryParse(text, out int number))
						return number >= 5 && number <= 500;
					return false;
				}
			);

			// --- Delay Button ---
			delayButton = new TextBoxButton(
				new Vector2(cellRectangles[4].X, cellRectangles[4].Y),
				cellRectangles[4].Width,
				cellRectangles[4].Height,
				borderSize: 2,
				buttonOptionText: delayOption,
				isActive: true,
				primaryColor: Color.White,
				secondaryColor: Color.Black,
				buttonTheme: ButtonTheme.Dark,
				onClick: () => { },
				buttonAdditionalText: "SORTING_DELAY",
				fixedSuffix: "MS",
				validator: (text) =>
				{
					if (int.TryParse(text, out int number))
						return number >= 0 && number <= 1000;
					return false;
				}
			);

			// --- Exit Button ---
			exitButton = new Button(
				new Vector2(cellRectangles[5].X, cellRectangles[5].Y),
				cellRectangles[5].Width,
				cellRectangles[5].Height,
				borderSize: 2,
				buttonOptionText: "EXIT",
				isActive: true,
				primaryColor: Color.White,
				secondaryColor: Color.Black,
				buttonTheme: ButtonTheme.Light,
				onClick: () => { Game1.OnExit(); },
				buttonAdditionalText: ""
			);
		}

		/// <summary>
		/// Configures the resolution selection panel. The panel slides in/out from the left.
		/// A list of available resolutions is provided, and selection updates the application’s resolution.
		/// </summary>
		private void SetupResolutionPanel()
		{
			resolutionPanelWidth = (int)(Game1.ScreenWidth * 0.2f);
			resolutionPanelHeight = Game1.ScreenHeight;
			resolutionPanelCurrentX = -resolutionPanelWidth;  // Start hidden off-screen.

			List<string> resolutionOptions = new List<string>
			{
				"640X480",
				"800X600",
				"1024X768",
				"1280X720",
				"1280X1024",
				"1366X768",
				"1600X900",
				"1920X1080"
			};

			// Add current resolution option if not already in the list.
			if (!resolutionOptions.Contains(resolutionOption))
			{
				resolutionOptions.Add(resolutionOption);
			}

			int defaultIndex = resolutionOptions.IndexOf(resolutionOption);
			if (defaultIndex < 0)
				defaultIndex = 0;

			resolutionPanel = new OptionListPanel(
				resolutionPanelWidth,
				resolutionPanelHeight,
				new Vector2((int)resolutionPanelCurrentX, 0),
				Color.LightGray,
				"SCREEN RESOLUTION",
				resolutionOptions,
				defaultIndex
			);

			// Set up an event handler to update the resolution when an option is selected.
			resolutionPanel.OnOptionSelected += (index) =>
			{
				string selectedRes = resolutionPanel.SelectedOption;
				string[] parts = selectedRes.Split('X');
				if (parts.Length == 2 &&
					int.TryParse(parts[0], out int newWidth) &&
					int.TryParse(parts[1], out int newHeight))
				{
					// Update screen resolution.
					UpdateScreenResolution(newWidth, newHeight);
					resolutionOption = $"{newWidth}X{newHeight}";
					resolutionButton.SetButtonOptionText(resolutionOption);
				}
				isResolutionPanelVisible = false;
			};
		}

		/// <summary>
		/// Configures the algorithm selection panel. The panel slides in/out from the left.
		/// Provides a list of sorting algorithm names to choose from and updates the selected algorithm.
		/// </summary>
		private void SetupAlgorithmPanel()
		{
			algorithmPanelWidth = (int)(Game1.ScreenWidth * 0.2f);
			algorithmPanelHeight = Game1.ScreenHeight;
			algorithmPanelCurrentX = -algorithmPanelWidth;  // Start hidden off-screen.

			List<string> algorithmOptions = new List<string>
			{
				"BUBBLE_SORT",
				"INSERTION_SORT",
				"SELECTION_SORT",
				"MERGE_SORT",
				"QUICK_SORT",
				"HEAP_SORT",
				"COMBO_SORT",
				"COCKTAIL_SORT",
				"ODD_EVEN_SORT"
			};

			string currentAlgoName = Game1.Algorithm.Name;
			int defaultIndex = algorithmOptions.IndexOf(currentAlgoName);
			if (defaultIndex < 0)
				defaultIndex = 0;

			algorithmPanel = new OptionListPanel(
				algorithmPanelWidth,
				algorithmPanelHeight,
				new Vector2((int)algorithmPanelCurrentX, 0),
				Color.LightGray,
				"SORTING ALGORITHM",
				algorithmOptions,
				defaultIndex
			);

			// Set up an event handler to update the selected algorithm when an option is chosen.
			algorithmPanel.OnOptionSelected += (index) =>
			{
				string selectedAlgo = algorithmPanel.SelectedOption;
				Game1.Algorithm = CreateAlgorithmByName(selectedAlgo);
				algorithmOption = selectedAlgo;
				algorithmButton.SetButtonOptionText(algorithmOption);
				isAlgorithmPanelVisible = false;
			};
		}

		/// <summary>
		/// Creates a new sorting algorithm instance based on the provided algorithm name.
		/// </summary>
		/// <param name="algoName">Name of the algorithm.</param>
		/// <returns>An instance of a SortAlgorithm corresponding to the given name.</returns>
		private SortAlgorithm CreateAlgorithmByName(string algoName)
		{
			switch (algoName)
			{
				case "BUBBLE_SORT": return new BubbleSort();
				case "INSERTION_SORT": return new InsertionSort();
				case "SELECTION_SORT": return new SelectionSort();
				case "MERGE_SORT": return new MergeSort();
				case "QUICK_SORT": return new QuickSort();
				case "HEAP_SORT": return new HeapSort();
				case "COMBO_SORT": return new ComboSort();
				case "COCKTAIL_SORT": return new CocktailSort();
				case "ODD_EVEN_SORT": return new OddEvenSort();
				case "RANDOMIZE_SHUFFLE": return new RandomizeShuffle();
				default: return new SelectionSort();
			}
		}

		/// <summary>
		/// Updates the screen resolution of the application.
		/// Adjusts the graphics settings, resizes UI panels, and repositions buttons accordingly.
		/// </summary>
		/// <param name="newWidth">New screen width.</param>
		/// <param name="newHeight">New screen height.</param>
		private void UpdateScreenResolution(int newWidth, int newHeight)
		{
			// Update global screen resolution settings.
			Game1.ScreenWidth = newWidth;
			Game1.ScreenHeight = newHeight;

			// Retrieve native display resolution.
			int nativeWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			int nativeHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

			// Determine if full screen should be enabled.
			if (newWidth == nativeWidth && newHeight == nativeHeight)
			{
				_graphicsDeviceManager.IsFullScreen = true;
			}
			else
			{
				_graphicsDeviceManager.IsFullScreen = false;
			}

			// Update preferred back buffer dimensions.
			_graphicsDeviceManager.PreferredBackBufferWidth = Game1.ScreenWidth;
			_graphicsDeviceManager.PreferredBackBufferHeight = Game1.ScreenHeight;
			_graphicsDeviceManager.ApplyChanges();

			// Resize and reposition UI components to match the new resolution.
			logoPanel.Resize(new Rectangle(0, 0, Game1.ScreenWidth, (int)(Game1.ScreenHeight * 0.2f)));
			buttonsGrid.Resize(Game1.ScreenWidth, Game1.ScreenHeight);

			List<Rectangle> cellRectangles = buttonsGrid.GetCellRectangles();
			resolutionButton.Resize(cellRectangles[0]);
			algorithmButton.Resize(cellRectangles[1]);
			startButton.Resize(cellRectangles[2]);
			linesCountButton.Resize(cellRectangles[3]);
			delayButton.Resize(cellRectangles[4]);
			exitButton.Resize(cellRectangles[5]);

			// Update resolution panel dimensions and position.
			resolutionPanelWidth = (int)(Game1.ScreenWidth * 0.2f);
			resolutionPanelHeight = Game1.ScreenHeight;
			if (!isResolutionPanelVisible)
				resolutionPanelCurrentX = -resolutionPanelWidth;
			resolutionPanel.Resize(new Rectangle((int)resolutionPanelCurrentX, 0, resolutionPanelWidth, resolutionPanelHeight));

			// Update algorithm panel dimensions and position.
			algorithmPanelWidth = (int)(Game1.ScreenWidth * 0.2f);
			algorithmPanelHeight = Game1.ScreenHeight;
			if (!isAlgorithmPanelVisible)
				algorithmPanelCurrentX = -algorithmPanelWidth;
			algorithmPanel.Resize(new Rectangle((int)algorithmPanelCurrentX, 0, algorithmPanelWidth, algorithmPanelHeight));
		}
	}
}
