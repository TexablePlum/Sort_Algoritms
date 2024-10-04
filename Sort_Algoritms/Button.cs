using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sort_Algoritms
{
	public class Button
	{
		protected static GraphicsDevice graphicsDevice;
		protected static SpriteBatch spriteBatch;
		protected static SpriteFont spriteFont;

		private readonly Texture2D texture;

		private Vector2 point;
		private Color buttonColor;
		private Color buttonHoverColor;
		private float width;
		private float height;
		private int thickness;

		private string buttonText;
		private Color textColor;
		private Color textHoverColor;

		public static void Initialize(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, SpriteFont spriteFont)
		{
			Button.graphicsDevice = graphicsDevice;
			Button.spriteBatch = spriteBatch;
			Button.spriteFont = spriteFont;
		}

		public Button(Vector2 point, float width, float height, Color buttonColor, int thickness = default, string buttonText = "", Color textColor = default, Color buttonHoverColor = default, Color textHoverColor = default)
		{
			this.point = point;
			this.width = width;
			this.height = height;
			this.buttonColor = buttonColor;
			this.buttonText = buttonText;
			this.thickness = thickness == default(int) ? 1 : thickness;
			this.textColor = textColor == default(Color) ? buttonColor : textColor;
			this.buttonHoverColor = buttonHoverColor == default(Color) ? Color.White : buttonHoverColor;
			this.textHoverColor = textHoverColor == default(Color) ? Color.Black : textHoverColor;

			texture = new Texture2D(graphicsDevice, 1, 1);
			texture.SetData(new[] { buttonColor });
		}

		public void Draw()
		{
			spriteBatch.Begin();
			if (IsMouseOver())
			{
				HoverDraw();
			}
			else
			{
				NoHoverDraw();
			}
			spriteBatch.End();
		}

		private void NoHoverDraw()
		{
			// Górna krawędź
			spriteBatch.Draw(texture, point, null, buttonColor, 0, Vector2.Zero, new Vector2(width, thickness), SpriteEffects.None, 0);

			// Dolna krawędź
			spriteBatch.Draw(texture, new Vector2(point.X, point.Y + height - thickness), null, buttonColor, 0, Vector2.Zero, new Vector2(width, thickness), SpriteEffects.None, 0);

			// Lewa krawędź
			spriteBatch.Draw(texture, point, null, buttonColor, 0, Vector2.Zero, new Vector2(thickness, height), SpriteEffects.None, 0);

			// Prawa krawędź
			spriteBatch.Draw(texture, new Vector2(point.X + width - thickness, point.Y), null, buttonColor, 0, Vector2.Zero, new Vector2(thickness, height), SpriteEffects.None, 0);

			// Rysowanie tekstu
			if (!string.IsNullOrEmpty(buttonText) && spriteFont != null)
			{
				Vector2 textSize = spriteFont.MeasureString(buttonText);
				Vector2 textPosition = point + new Vector2((width - textSize.X) / 2 + 3, (height - textSize.Y) / 2 + 8); // Centrowanie tekstu
				spriteBatch.DrawString(spriteFont, buttonText, textPosition, textColor);
			}
		}

		private void HoverDraw()
		{
			Vector2 drawPosition = new Vector2(point.X, point.Y);

			spriteBatch.Draw(texture, drawPosition, null, buttonHoverColor, 0, Vector2.Zero, new Vector2(width, height), SpriteEffects.None, 0);

			// Rysowanie tekstu
			if (!string.IsNullOrEmpty(buttonText) && spriteFont != null)
			{
				Vector2 textSize = spriteFont.MeasureString(buttonText);
				Vector2 textPosition = point + new Vector2((width - textSize.X) / 2 + 3, (height - textSize.Y) / 2 + 8); // Centrowanie tekstu
				spriteBatch.DrawString(spriteFont, buttonText, textPosition, textHoverColor);
			}
		}

		private bool IsMouseOver()
		{
			MouseState mouseState = Mouse.GetState();
			return mouseState.X >= point.X && mouseState.X <= point.X + width &&
				   mouseState.Y >= point.Y && mouseState.Y <= point.Y + height;
		}
	}
}
