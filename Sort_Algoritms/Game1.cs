using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Sort_Algoritms
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private Container container;
		private Sort_Algorithms sorter;

		bool wasSpacePressed = false;
		bool wasEPressed = false;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			_graphics.PreferredBackBufferWidth = 1600; // Wymiary szerokości
			_graphics.PreferredBackBufferHeight = 900; // Wymiary wysokości

			// MSAA
			_graphics.PreferMultiSampling = true; // Wygładzanie krawędzi
			_graphics.GraphicsDevice.PresentationParameters.MultiSampleCount = 8;

			_graphics.ApplyChanges();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			Line.Initialize(GraphicsDevice, _spriteBatch);

			// TODO: use this.Content to load your game content here
			container = new Container(new Vector2(1600, 900), 20, 50, 1);
			container.Load();
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// TODO: Add your update logic here
			KeyboardState keyboardState = Keyboard.GetState();
			bool isSpacePressed = keyboardState.IsKeyDown(Keys.Space);
			bool isEPressed = keyboardState.IsKeyDown(Keys.E);

			if (isSpacePressed && !wasSpacePressed)
			{
				sorter = new Randomize_Shuffle();
				sorter.Sort(container.Lines);
			} 

			// Aktualizacja stanu klawisza spacji
			wasSpacePressed = isSpacePressed;

			if (isEPressed && !wasEPressed)
			{
				sorter = new Bubble_Sort();
				sorter.Sort(container.Lines);
			}

			wasEPressed = isEPressed;

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			// TODO: Add your drawing code here
			container.Draw();

			base.Draw(gameTime);
		}
	}
}
