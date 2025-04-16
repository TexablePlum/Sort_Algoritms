using System;
using System.IO;
using System.Text.Json;

namespace Sort_Algoritms
{
	/// <summary>
	/// Static class responsible for loading and saving application settings to a JSON file
	/// in a folder named "Sorting_Algorithms_App" within the user's AppData folder.
	/// If the folder or file does not exist, they are created and populated with default values.
	/// </summary>
	public static class SettingsManager
	{
		private static readonly string appFolderName = "SortingAlgorithmsApp";
		private static readonly string settingsFileName = "settings.json";

		/// <summary>
		/// Gets the full path to the settings file in the user's AppData folder.
		/// </summary>
		private static string SettingsFilePath
		{
			get
			{
				string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				string folderPath = Path.Combine(appData, appFolderName);
				return Path.Combine(folderPath, settingsFileName);
			}
		}

		/// <summary>
		/// Loads settings from the JSON file.
		/// If the file or folder does not exist, creates them and returns default settings.
		/// </summary>
		/// <returns>A Settings object populated from the file or with default values.</returns>
		public static Settings LoadSettings()
		{
			try
			{
				string filePath = SettingsFilePath;
				// Ensure the folder exists.
				string folder = Path.GetDirectoryName(filePath);
				if (!Directory.Exists(folder))
				{
					Directory.CreateDirectory(folder);
				}

				if (!File.Exists(filePath))
				{
					// File doesn't exist, so create with default settings.
					Settings defaultSettings = new Settings();
					SaveSettings(defaultSettings);
					return defaultSettings;
				}
				else
				{
					// Read the settings from file.
					string json = File.ReadAllText(filePath);
					if (string.IsNullOrWhiteSpace(json))
					{
						// If file is empty, use default settings.
						Settings defaultSettings = new Settings();
						SaveSettings(defaultSettings);
						return defaultSettings;
					}
					Settings settings = JsonSerializer.Deserialize<Settings>(json);
					return settings;
				}
			}
			catch (Exception ex)
			{
				// In case of error, log and return default settings.
				Console.WriteLine("Error loading settings: " + ex.Message);
				return new Settings();
			}
		}

		/// <summary>
		/// Saves the given settings to a JSON file.
		/// </summary>
		/// <param name="settings">The Settings object to save.</param>
		public static void SaveSettings(Settings settings)
		{
			try
			{
				string filePath = SettingsFilePath;
				// Ensure the folder exists.
				string folder = Path.GetDirectoryName(filePath);
				if (!Directory.Exists(folder))
				{
					Directory.CreateDirectory(folder);
				}

				string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
				File.WriteAllText(filePath, json);
			}
			catch (Exception ex)
			{
				// In case of error, log the exception.
				Console.WriteLine("Error saving settings: " + ex.Message);
			}
		}
	}
}
