using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	// NOTE: The following wrapper methods exist for Unity button OnClick events
	// Unity's Inspector can only call methods with specific signatures (no parameters or single parameter)
	// These methods provide convenient pre-configured scene loading for UI buttons
	// For programmatic use, prefer LoadSceneByName() or LoadSceneWithLevelCheck()

	// Load the main menu scene. Used by Unity button events.
	public void LoadMenu()
	{
		SceneManager.LoadScene("Menu");
	}

	// Load the map menu scene. Used by Unity button events.
	public void LoadMapMenu()
	{
		SceneManager.LoadScene("Map Menu");
	}

	// Load Map 1 scene directly. Used by Unity button events.
	// WARNING: Bypasses level selection and weapon menu flow.
	public void LoadMap1()
	{
		SceneManager.LoadScene("Map_1");
	}

	// Load Map 2 scene directly. Used by Unity button events.
	// WARNING: Bypasses level selection and weapon menu flow.
	public void LoadMap2()
	{
		SceneManager.LoadScene("Map_2");
	}

	// Load Map 3 scene directly. Used by Unity button events.
	// WARNING: Bypasses level selection and weapon menu flow.
	public void LoadMap3()
	{
		SceneManager.LoadScene("Map_3");
	}

	// Load any scene by name. Preferred method for programmatic scene loading.
	public void LoadSceneByName(string sceneName)
	{
		if (!string.IsNullOrEmpty(sceneName))
		{
			SceneManager.LoadScene(sceneName);
		}
	}

	// Load a scene with level selection validation. Used by map card play buttons.
	// This method validates level selection and routes through Weapon Menu.
	// NOTE: When called from Map Menu, this loads Weapon Menu instead of the map scene directly.
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

	// Load Map 1 with level selection validation. Used by Unity button events.
	// Routes through Weapon Menu for proper game flow.
	public void LoadMap1WithLevelCheck()
	{
		LoadSceneWithLevelCheck("Map_1");
	}

	// Load Map 2 with level selection validation. Used by Unity button events.
	// Routes through Weapon Menu for proper game flow.
	public void LoadMap2WithLevelCheck()
	{
		LoadSceneWithLevelCheck("Map_2");
	}

	// Load Map 3 with level selection validation. Used by Unity button events.
	// Routes through Weapon Menu for proper game flow.
	public void LoadMap3WithLevelCheck()
	{
		LoadSceneWithLevelCheck("Map_3");
	}
}
