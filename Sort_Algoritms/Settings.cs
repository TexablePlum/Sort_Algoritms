
namespace Sort_Algoritms
{
	/// <summary>
	/// Represents the application settings.
	/// These values are used to configure the screen, line visualization, and sorting delay.
	/// </summary>
	public class Settings
	{
		public int ScreenWidth { get; set; } = 800;            // Width of the game window
		public int ScreenHeight { get; set; } = 600;           // Height of the game window
		public int LinesCount { get; set; } = 50;              // Number of lines to visualize
		public int LinesMargin { get; set; } = 1;              // Margin between each visual line
		public int ScreenMargin { get; set; } = 15;            // Margin within the game window for content display
		public int AlgorithmDelay { get; set; } = 20;          // Delay (in ms) between steps of the sorting algorithm
		public string AppName { get; set; } = "/// SORTING_ALGORITHMS_APP_ \\\\\\\r\n";  // Application name
	}
}
