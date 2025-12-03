using UnityEngine;

// Static manager to store selected map information between scenes
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

