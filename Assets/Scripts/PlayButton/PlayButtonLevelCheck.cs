using UnityEngine;
using UnityEngine.UI;

// Attach this to the Play Button GameObject to automatically check level selection
public class PlayButtonLevelCheck : MonoBehaviour
{
    private Button button;
    private MapCardUI mapCardUI;
    
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

        // Fallback: Try to find and check ChooseLevelWarning in scene (including inactive)
        ChooseLevelWarning[] warnings = FindObjectsByType<ChooseLevelWarning>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        ChooseLevelWarning warning = (warnings != null && warnings.Length > 0) ? warnings[0] : null;
        
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

