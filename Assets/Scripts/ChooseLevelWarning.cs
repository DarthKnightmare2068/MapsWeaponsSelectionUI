using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Localization.Settings;

public class ChooseLevelWarning : MonoBehaviour
{
    private CustomTMPDropdown dropdown;
    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine;
    private MapCardUI currentMapCard; // The Map Card that triggered this warning
    
    // Caching: Store MapCardUI[] array to avoid repeated FindObjectsByType searches
    // Why: ChooseLevelWarning.CheckLevelSelection() can be called frequently (every time user clicks play),
    // and each call was searching through all objects in the scene. With caching, we search once and reuse
    // the result. This is a one-to-many relationship (one warning needs many cards), and the list can change
    // at runtime, so caching with invalidation is better than dependency injection here.
    private MapCardUI[] cachedMapCards;
    private bool isMapCardsCacheValid = false;

    [SerializeField] private TextMeshProUGUI warningText; // Reference to the TextMeshPro component
    [SerializeField] private float displayDuration = 1f;   // Time to show before fading
    [SerializeField] private float fadeDuration = 0.5f;     // Time to fade out

    // Manual text translations
    private const string ENGLISH_TEXT = "You must choose one level first";
    private const string VIETNAMESE_TEXT = "Bạn phải chọn độ khó trước";

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Get or add CanvasGroup for fading
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        // Configure CanvasGroup for proper visibility
        canvasGroup.alpha = 1f; // Start fully visible
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Auto-find TextMeshProUGUI if not assigned
        if (warningText == null)
        {
            warningText = GetComponentInChildren<TextMeshProUGUI>();
        }

        // Deactivate the object initially
        gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (dropdown != null)
        {
            dropdown.OnValueChanged -= OnDropdownValueChanged;
        }
    }
    
    private void OnDropdownValueChanged(int value)
    {
        // When a level is selected (value >= 0), hide the warning
        if (value >= 0)
        {
            HideWarning();
        }
    }
    
    // Method to check if level is selected and show warning if not
    // Can be called with a specific MapCardUI to check that card's dropdown
    // Returns true if level is selected (can proceed), false if not (warning shown)
    public bool CheckLevelSelection(MapCardUI mapCard = null)
    {
        // Find the Map Card that triggered this (passed as parameter or find the one that called)
        if (mapCard != null)
        {
            currentMapCard = mapCard;
        }
        else if (currentMapCard == null)
        {
            // Caching optimization: Use cached MapCardUI array instead of searching every time
            // If cache is invalid or doesn't exist, search once and cache the result
            // This prevents expensive FindObjectsByType calls on every user click
            if (!isMapCardsCacheValid || cachedMapCards == null)
            {
                cachedMapCards = FindObjectsByType<MapCardUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                isMapCardsCacheValid = true;
            }
            
            // Use cached array instead of searching again
            if (cachedMapCards != null && cachedMapCards.Length > 0)
            {
                // Use the first active one, or the first one found
                foreach (MapCardUI card in cachedMapCards)
                {
                    if (card != null && card.gameObject.activeInHierarchy)
                    {
                        currentMapCard = card;
                        break;
                    }
                }
                // Safety check: Only use cachedMapCards[0] if it's not null
                // The array could contain null entries if cards were destroyed but cache wasn't invalidated
                if (currentMapCard == null && cachedMapCards[0] != null)
                {
                    currentMapCard = cachedMapCards[0];
                }
            }
        }
        
        // Find the dropdown from the current Map Card
        if (currentMapCard != null)
        {
            dropdown = currentMapCard.GetComponentInChildren<CustomTMPDropdown>();
            if (dropdown != null)
            {
                // Subscribe to this dropdown's changes
                dropdown.OnValueChanged -= OnDropdownValueChanged; // Remove old subscription
                dropdown.OnValueChanged += OnDropdownValueChanged; // Add new subscription
            }
        }
        
        if (dropdown == null)
        {
            Debug.LogWarning("ChooseLevelWarning: CustomTMPDropdown not found in any Map Card!");
            return true; // Allow to proceed if dropdown not found
        }
        
        // Check if a level is selected
        if (!dropdown.HasSelection())
        {
            // No level selected - show warning
            ShowWarning();
            return false;
        }
        
        // Level is selected - hide warning and allow to proceed
        HideWarning();
        return true;
    }

    // Show the warning with fade effect
    private void ShowWarning()
    {
        // Stop any existing fade coroutine
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
        
        // Update localized text
        UpdateLocalizedText();

        // Activate the object first
        gameObject.SetActive(true);

        // Ensure all children are also active
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        // Ensure CanvasGroup is properly configured for visibility
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        
        // Force update to ensure visibility
        Canvas.ForceUpdateCanvases();
        
        // Start fade out after display duration
        if (gameObject.activeInHierarchy)
        {
            fadeCoroutine = StartCoroutine(FadeOutAfterDelay());
        }
    }
    
    // Update the warning text based on current locale
    private void UpdateLocalizedText()
    {
        if (warningText == null) return;

        // Get current locale code
        string currentLocale = LocalizationManager.GetCurrentLocaleCode();

        // Set text based on locale
        if (currentLocale == "vi-VN")
        {
            warningText.text = VIETNAMESE_TEXT;
        }
        else
        {
            // Default to English for any other locale
            warningText.text = ENGLISH_TEXT;
        }
    }

    // Coroutine to fade out after delay
    private IEnumerator FadeOutAfterDelay()
    {
        // Wait for display duration
        yield return new WaitForSeconds(displayDuration);
        
        // Fade out
        if (canvasGroup != null)
        {
            float elapsed = 0f;
            float startAlpha = canvasGroup.alpha;
            
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = 0f;
        }

        // Deactivate after fade
        gameObject.SetActive(false);
        fadeCoroutine = null;
    }

    // Public method to hide the warning (can be called when level is selected)
    public void HideWarning()
    {
        // Stop any fade coroutine
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
        
        // Immediately hide
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
        gameObject.SetActive(false);
    }
    
    // Invalidate the cached MapCardUI array
    // Call this when map cards are added or removed at runtime to refresh the cache
    // This ensures the cache stays accurate if the number of cards changes dynamically
    public void InvalidateMapCardsCache()
    {
        isMapCardsCacheValid = false;
        cachedMapCards = null;
    }
}
