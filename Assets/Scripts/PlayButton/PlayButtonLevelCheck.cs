using UnityEngine;
using UnityEngine.UI;

// Attach this to the Play Button GameObject to automatically check level selection
public class PlayButtonLevelCheck : MonoBehaviour
{
    private Button button;
    private MapCardUI mapCardUI;
    
    // Caching: Store ChooseLevelWarning reference to avoid repeated FindObjectsByType searches
    // This is a fallback case, but caching still improves performance by searching only once
    private ChooseLevelWarning cachedWarning;
    
    private void Awake()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogWarning("PlayButtonLevelCheck: Button component not found!");
            return;
        }
        
        // Find MapCardUI in parent
        mapCardUI = GetComponentInParent<MapCardUI>();
        if (mapCardUI == null)
        {
            Debug.LogWarning("PlayButtonLevelCheck: MapCardUI not found in parent hierarchy!");
        }
        
        // Cache warning reference once during Awake (performance optimization)
        // This avoids searching every time the button is clicked
        cachedWarning = FindFirstObjectByType<ChooseLevelWarning>();
        
        // Override button's onClick to check level first
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnButtonClicked);
        }
    }
    
    private void OnButtonClicked()
    {
        // If MapCardUI is found, use its method which now delegates to GameStartService
        if (mapCardUI != null)
        {
            mapCardUI.OnPlayButtonClicked();
            return;
        }

        // Fallback: Use cached warning reference instead of searching every time
        // This improves performance by avoiding repeated FindObjectsByType calls
        ChooseLevelWarning warning = cachedWarning;
        
        if (warning != null)
        {
            // Try to find MapCardUI to pass to warning
            MapCardUI card = GetComponentInParent<MapCardUI>();
            bool canProceed = warning.CheckLevelSelection(card);
            if (!canProceed)
            {
                // Warning shown, don't proceed
                return;
            }
        }
        
        // If no checks found, allow the button's original onClick to work
        // (This shouldn't happen if properly set up, but provides fallback)
        Debug.LogWarning("PlayButtonLevelCheck: No level check components found. Button click may proceed without validation.");
    }
}

