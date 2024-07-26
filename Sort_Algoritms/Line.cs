using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sort_Algoritms
{
	public class Line
	{
		protected static GraphicsDevice graphicsDevice;
		protected static SpriteBatch spriteBatch;

		private readonly Texture2D texture;

		private Vector2 point;
		private Color color;
		private float width;
		private float height;

		public Vector2 Point { get => point; set => point = value; }
		public Color Color { get => color; set => color = value; }
		public float Width { get => width; set => width = value; }
		public float Height { get => height; set => height = value; }

		public static void Initialize(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
		{
			Line.graphicsDevice = graphicsDevice;
			Line.spriteBatch = spriteBatch;
		}

		public Line (Vector2 point, float width, float height, Color color)
		{
			this.point = point;
			this.width = width;
			this.height = height;
			this.color = color;

			texture = new Texture2D(graphicsDevice, 1, 1);
			texture.SetData(new[] { Color.White });
		}

		public void Draw()
		{
			Vector2 drawPosition = new Vector2(point.X, point.Y - height);

			spriteBatch.Begin();
			spriteBatch.Draw(texture, drawPosition, null, color, 0, Vector2.Zero, new Vector2(width, height), SpriteEffects.None, 0);
			spriteBatch.End();
		}
	}
}
