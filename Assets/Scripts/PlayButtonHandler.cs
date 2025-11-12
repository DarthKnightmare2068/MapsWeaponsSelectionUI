using UnityEngine;

public class PlayButtonHandler : MonoBehaviour
{
    [SerializeField] private ChooseLevelWarning warning;
    [SerializeField] private UIButtonSceneLoader sceneLoader;
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
            sceneLoader = FindFirstObjectByType<UIButtonSceneLoader>();
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
        // Try MapCardUI first (if available) - it handles level checking internally
        if (mapCardUI != null)
        {
            mapCardUI.OnPlayButtonClicked();
            return;
        }
        
        // Fallback: Check level selection manually if MapCardUI not available
        if (warning != null)
        {
            bool canProceed = warning.CheckLevelSelection();
            if (!canProceed)
            {
                // Warning is shown, don't proceed
                return;
            }
        }
        
        // Level is selected or no warning component - proceed with loading scene
        if (sceneLoader != null && !string.IsNullOrEmpty(sceneName))
        {
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
        // Try MapCardUI first (if available) - it handles level checking internally
        if (mapCardUI != null)
        {
            mapCardUI.OnPlayButtonClicked();
            return;
        }
        
        // Fallback: Check level selection manually if MapCardUI not available
        if (warning != null)
        {
            bool canProceed = warning.CheckLevelSelection();
            if (!canProceed)
            {
                // Warning is shown, don't proceed
                return;
            }
        }
        
        // Level is selected or no warning component - proceed with loading scene
        if (sceneLoader != null && !string.IsNullOrEmpty(sceneToLoad))
        {
            sceneLoader.LoadSceneByName(sceneToLoad);
        }
    }
}

