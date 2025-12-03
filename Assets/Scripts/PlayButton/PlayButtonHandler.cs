using UnityEngine;

public class PlayButtonHandler : MonoBehaviour
{
    [SerializeField] private ChooseLevelWarning warning;
    [SerializeField] private SceneLoader sceneLoader;
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
        if (sceneLoader == null)
        {
            sceneLoader = FindFirstObjectByType<SceneLoader>();
        }
        
        // Try to find MapCardUI if not assigned (for alternative scene loading)
        if (mapCardUI == null)
        {
            mapCardUI = GetComponentInParent<MapCardUI>();
        }
    }
    
    // Method to be called when play button is clicked
    public void OnPlayButtonClicked()
    {
        // Prefer using the map card's centralized logic when available
        if (mapCardUI != null)
        {
            mapCardUI.OnPlayButtonClicked();
            return;
        }

        // If there is no MapCardUI (generic play button), delegate to GameStartService
        CustomTMPDropdown dropdown = null;
        MapCardUI card = GetComponentInParent<MapCardUI>();
        if (card != null)
        {
            // Try to locate dropdown on the same card for level selection
            dropdown = card.GetComponentInChildren<CustomTMPDropdown>();
        }

        // When using a generic scene name, MapData isn't available.
        // NOTE: This fallback bypasses the Map Menu -> Weapon Menu -> Map Scene flow
        // It should only be used for non-map-card play buttons (e.g., generic menu buttons)
        // For map cards, MapCardUI should always be present and this code should not execute
        if (sceneLoader != null && !string.IsNullOrEmpty(sceneName))
        {
            // If a warning exists and dropdown is present, reuse its validation.
            if (warning != null && dropdown != null && !dropdown.HasSelection())
            {
                warning.CheckLevelSelection(card);
                return;
            }

            // WARNING: Direct scene load - this bypasses Weapon Menu flow
            // Only use this for non-map-card buttons
            Debug.LogWarning($"PlayButtonHandler: Direct scene load bypassing Weapon Menu flow! Scene: {sceneName}");
            sceneLoader.LoadSceneByName(sceneName);
        }
        else if (sceneLoader != null)
        {
            Debug.LogWarning("PlayButtonHandler: Scene name is not set!");
        }
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

