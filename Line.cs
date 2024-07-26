using System;

namespace Sort_Algoritms
{
	public class Line
	{
		private Texture2D pixel;
		private SpriteBatch spriteBatch;

		public Line(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
		{
			// Inicjalizacja tekstury 1x1 piksel
			pixel = new Texture2D(graphicsDevice, 1, 1);
			pixel.SetData(new[] { Color.White });

			// Przypisanie przekazanego obiektu SpriteBatch do pola klasy
			this.spriteBatch = spriteBatch;
		}

		public void Draw(Vector2 start, Vector2 end, float thickness, Color color)
		{
			// Obliczenie kąta i długości odcinka linii
			Vector2 edge = end - start;
			float angle = (float)Math.Atan2(edge.Y, edge.X);
			float length = edge.Length();

			// Narysowanie linii za pomocą tekstury 1x1 piksel
			spriteBatch.Begin();
			spriteBatch.Draw(pixel, start, null, color, angle, Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0);
			spriteBatch.End();
		}
	}
}
