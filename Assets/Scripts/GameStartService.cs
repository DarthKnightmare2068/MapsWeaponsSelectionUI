using UnityEngine;

// Centralizes the core game start logic around choosing a map and a level.
// This is a plain C# service (no MonoBehaviour) so that the rules live in one place
// and UI scripts only forward user intent here.
public static class GameStartService
{
    // Try to start the game from a map card or play button.
    // Returns true if the scene load was triggered, false if validation failed.
    // mapData: The selected map data.
    // levelDropdown: Optional dropdown used to choose the level/difficulty.
    //                If provided, a valid selection is required.
    // warning: Optional warning view used to inform the user when the level is not chosen.
    // sceneLoader: Scene loader used to load the target scene when validation passes.
    // mapCard: Optional map card that owns the dropdown, so warning logic can target
    //          the correct card when showing messages.
    public static bool TryStartGame(
        MapData mapData,
        CustomTMPDropdown levelDropdown,
        ChooseLevelWarning warning,
        SceneLoader sceneLoader,
        MapCardUI mapCard = null)
    {
        if (mapData == null)
        {
            Debug.LogWarning("GameStartService: MapData is null, cannot start game.");
            return false;
        }

        if (mapData.IsLocked)
        {
            Debug.LogWarning($"GameStartService: Map '{mapData.MapName}' is locked, cannot start game.");
            return false;
        }

        // If a dropdown is supplied, require a valid selection.
        if (levelDropdown != null)
        {
            if (!levelDropdown.HasSelection())
            {
                // If we have a warning view, use it to inform the player.
                if (warning != null)
                {
                    // Prefer passing the specific card if provided so the warning
                    // can bind to the right dropdown.
                    warning.CheckLevelSelection(mapCard);
                }
                else
                {
                    Debug.LogWarning("GameStartService: No level selected and no warning component assigned.");
                }

                return false;
            }
        }

        if (sceneLoader == null)
        {
            Debug.LogError("GameStartService: SceneLoader is not assigned, cannot load scene.");
            return false;
        }

        if (string.IsNullOrEmpty(mapData.SceneName))
        {
            Debug.LogWarning($"GameStartService: Map '{mapData.MapName}' has no scene name configured.");
            return false;
        }

        // Store selected map scene name for later use in Weapon Menu
        MapSelectionManager.SetSelectedMap(mapData.SceneName);
        
        // Load Weapon Menu scene instead of directly loading the map scene
        sceneLoader.LoadSceneByName("Weapon Menu");
        return true;
    }
}


