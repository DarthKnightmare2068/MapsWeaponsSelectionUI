using UnityEngine;

public class PlayButtonHandler : MonoBehaviour
{
    [SerializeField] private ChooseLevelWarning warning;
    [SerializeField]
    [Tooltip("Assign via Inspector for best performance. Searches scene as fallback.")]
    private SceneLoader sceneLoader;
    [SerializeField] private string sceneName;
    [SerializeField] private MapCardUI mapCardUI; // Optional: if using MapCardUI's scene loading

    private void Awake()
    {
        // Try to find warning if not assigned
        if (warning == null)
        {
            warning = GetComponentInParent<ChooseLevelWarning>();
            if (warning == null)
            {
                Transform parent = transform.parent;
                if (parent != null)
                {
                    warning = parent.GetComponentInChildren<ChooseLevelWarning>();
                }
            }
        }

        // Try to find scene loader if not assigned
        // FIXED: Added warning log for expensive FindFirstObjectByType fallback
        if (sceneLoader == null)
        {
            Debug.LogWarning("PlayButtonHandler: SceneLoader not assigned in Inspector. Using expensive FindFirstObjectByType as fallback. Please assign in Inspector for better performance.");
            sceneLoader = FindFirstObjectByType<SceneLoader>();
        }

        // Try to find MapCardUI if not assigned (for alternative scene loading)
        if (mapCardUI == null)
        {
            mapCardUI = GetComponentInParent<MapCardUI>();
        }
    }

    // Method to be called when play button is clicked
    // FIXED: Simplified with early returns and extracted helper methods
    public void OnPlayButtonClicked()
    {
        // Prefer using the map card's centralized logic when available
        if (TryPlayFromMapCard())
        {
            return;
        }

        // Fallback: Try to play with scene loader and validation
        if (TryPlayWithValidation())
        {
            return;
        }

        Debug.LogWarning("PlayButtonHandler: No valid play method found!");
    }

    // Helper method: Try to play from MapCardUI
    private bool TryPlayFromMapCard()
    {
        if (mapCardUI == null) return false;

        mapCardUI.OnPlayButtonClicked();
        return true;
    }

    // Helper method: Try to play with scene loader and level validation
    private bool TryPlayWithValidation()
    {
        if (sceneLoader == null || string.IsNullOrEmpty(sceneName))
        {
            return false;
        }

        // Check if we need to show level warning
        CustomTMPDropdown dropdown = null;
        MapCardUI card = GetComponentInParent<MapCardUI>();
        if (card != null)
        {
            dropdown = card.GetComponentInChildren<CustomTMPDropdown>();
        }

        // Show warning if dropdown exists but has no selection
        if (warning != null && dropdown != null && !dropdown.HasSelection())
        {
            warning.CheckLevelSelection(card);
            return true;
        }

        // WARNING: Direct scene load - bypasses Weapon Menu flow
        // Only use this for non-map-card buttons
        Debug.LogWarning($"PlayButtonHandler: Direct scene load bypassing Weapon Menu flow! Scene: {sceneName}");
        sceneLoader.LoadSceneByName(sceneName);
        return true;
    }

    // Overload to load a specific scene
    public void OnPlayButtonClicked(string sceneToLoad)
    {
        // Prefer using the map card's centralized logic when available
        if (mapCardUI != null)
        {
            mapCardUI.OnPlayButtonClicked();
            return;
        }

        // Generic fallback: minimal validation and direct load
        // NOTE: This bypasses the Map Menu -> Weapon Menu -> Map Scene flow
        // Should only be used for non-map-card buttons
        if (sceneLoader != null && !string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogWarning($"PlayButtonHandler: Direct scene load bypassing Weapon Menu flow! Scene: {sceneToLoad}");
            sceneLoader.LoadSceneByName(sceneToLoad);
        }
    }
}

