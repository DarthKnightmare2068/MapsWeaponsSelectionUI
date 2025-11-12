using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonSceneLoader : MonoBehaviour
{
	// Load specific scenes (for button OnClick events)
	public void LoadMenu()
	{
		SceneManager.LoadScene("Menu");
	}

	public void LoadMapMenu()
	{
		SceneManager.LoadScene("MapMenu");
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
	public void LoadSceneWithLevelCheck(string sceneName)
	{
		// Try to find MapCardUI in parent hierarchy
		MapCardUI mapCardUI = GetComponentInParent<MapCardUI>();
		if (mapCardUI != null)
		{
			// Use MapCardUI's method which handles level checking
			mapCardUI.OnPlayButtonClicked();
			return;
		}
		
		// Fallback: Try to find ChooseLevelWarning and CustomTMPDropdown
		ChooseLevelWarning warning = GetComponentInParent<ChooseLevelWarning>();
		if (warning != null)
		{
			bool canProceed = warning.CheckLevelSelection();
			if (!canProceed)
			{
				// Warning shown, don't load
				return;
			}
		}
		
		// No checks found or level is selected - proceed with loading
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
