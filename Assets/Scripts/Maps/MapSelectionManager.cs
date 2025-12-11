using UnityEngine;

// Static manager to store selected map information between scenes.
// WARNING: This data only persists during the current game session.
// It will be lost when the application is closed/restarted.
// If you need persistence across sessions, use PlayerPrefs or save to file.
public static class MapSelectionManager
{
	private static string selectedMapSceneName;
	
	// Store the selected map's scene name
	public static void SetSelectedMap(string sceneName)
	{
		selectedMapSceneName = sceneName;
	}
	
	// Get the stored map scene name
	public static string GetSelectedMapSceneName()
	{
		return selectedMapSceneName;
	}
	
	// Check if a map has been selected
	public static bool HasSelectedMap()
	{
		return !string.IsNullOrEmpty(selectedMapSceneName);
	}
	
	// Clear the selected map (optional, for cleanup)
	public static void ClearSelectedMap()
	{
		selectedMapSceneName = null;
	}
}

