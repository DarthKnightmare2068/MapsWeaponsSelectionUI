using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	// Load specific scenes (for button OnClick events)
	public void LoadMenu()
	{
		SceneManager.LoadScene("Menu");
	}

	public void LoadMapMenu()
	{
		SceneManager.LoadScene("Map Menu");
	}

	public void LoadMap1()
	{
		SceneManager.LoadScene("Map_1");
	}

	public void LoadMap2()
	{
		SceneManager.LoadScene("Map_2");
	}

	public void LoadMap3()
	{
		SceneManager.LoadScene("Map_3");
	}

	// Generic method to load any scene by name
	public void LoadSceneByName(string sceneName)
	{
		if (!string.IsNullOrEmpty(sceneName))
		{
			SceneManager.LoadScene(sceneName);
		}
	}
	
	// Load scene with level selection check (for play buttons on map cards)
	// This method checks if a level is selected before loading
	// NOTE: When called from Map Menu, this will go through GameStartService which loads Weapon Menu
	public void LoadSceneWithLevelCheck(string sceneName)
	{
		// Try to find MapCardUI in parent hierarchy
		MapCardUI mapCardUI = GetComponentInParent<MapCardUI>();
		if (mapCardUI != null)
		{
			// Use MapCardUI's method which now delegates to GameStartService
			// GameStartService will store the map and load Weapon Menu instead of direct map scene
			mapCardUI.OnPlayButtonClicked();
			return;
		}
		
		// If no MapCardUI found, this method cannot proceed properly
		// Map card play buttons should always have MapCardUI in their hierarchy
		Debug.LogWarning($"SceneLoader.LoadSceneWithLevelCheck: MapCardUI not found! Cannot proceed with proper flow. Scene: {sceneName}");
		
		// Fallback: Try to use GameStartService directly if we can locate required pieces
		// This is a minimal fallback for edge cases, but should not be used in normal flow
		CustomTMPDropdown dropdown = GetComponentInParent<CustomTMPDropdown>();
		ChooseLevelWarning warning = GetComponentInParent<ChooseLevelWarning>();

		if (dropdown != null || warning != null)
		{
			// Without a MapData reference this acts as a minimal level check:
			// if a dropdown exists and is unselected, show warning and abort.
			if (dropdown != null && !dropdown.HasSelection())
			{
				if (warning != null)
				{
					warning.CheckLevelSelection();
				}
				return;
			}
		}
		
		// WARNING: Direct scene load bypasses Weapon Menu flow
		// This should only happen in edge cases where MapCardUI is not available
		Debug.LogWarning($"SceneLoader.LoadSceneWithLevelCheck: Bypassing Weapon Menu flow! Directly loading: {sceneName}");
		LoadSceneByName(sceneName);
	}
	
	// Load Map1 with level check
	public void LoadMap1WithLevelCheck()
	{
		LoadSceneWithLevelCheck("Map_1");
	}
	
	// Load Map2 with level check
	public void LoadMap2WithLevelCheck()
	{
		LoadSceneWithLevelCheck("Map_2");
	}
	
	// Load Map3 with level check
	public void LoadMap3WithLevelCheck()
	{
		LoadSceneWithLevelCheck("Map_3");
	}
}
