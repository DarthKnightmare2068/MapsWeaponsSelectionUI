using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using TMPro;

public class MapCardUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI mapNameText;
	[SerializeField] private Button mapButton;
	[SerializeField] private Image mapImage;
	[SerializeField] private string localizationTableName = "Map Labels";
	[SerializeField] private Image lockImageDisplay; // Image component to display lock icon (assign in prefab)

	private MapData mapData;
	private SceneLoader sceneLoader;
	private LocalizedString localizedString;
	private CanvasGroup canvasGroup; // Used to fade entire card
	private float lockedCardAlpha; // Alpha value for locked cards (set from MapCardManager)
	private CustomTMPDropdown levelDropdown; // Reference to the level dropdown on this card
	private ChooseLevelWarning warning; // Reference to the warning component

	private void Awake()
	{
		// Auto-find Button component if not assigned
		if (mapButton == null)
		{
			mapButton = GetComponent<Button>();
		}

		// Get or add CanvasGroup component for fading entire card
		canvasGroup = GetComponent<CanvasGroup>();
		if (canvasGroup == null)
		{
			canvasGroup = gameObject.AddComponent<CanvasGroup>();
		}
		
		// Find the level dropdown in this card
		levelDropdown = GetComponentInChildren<CustomTMPDropdown>();
		
		// Warning object is in the scene, not in prefab - will be found when needed
	}

	private void OnDestroy()
	{
		// Unsubscribe to prevent memory leaks
		if (localizedString != null)
		{
			localizedString.StringChanged -= UpdateText;
		}
	}

	// Dependency Injection: ChooseLevelWarning is passed here instead of searching for it.
	// Why: MapCardUI is instantiated multiple times (one per map card), but ChooseLevelWarning is a single
	// scene object. Without injection, every card would search for the same warning object, causing
	// performance issues (10 cards = 10 searches). By injecting the reference once from MapCardManager,
	// we eliminate all searches and get instant access. This is a one-to-one relationship (one warning
	// for all cards), making dependency injection the ideal solution.
	public void Initialize(MapData data, SceneLoader loader, float cardAlpha = 0.3f, ChooseLevelWarning warningRef = null)
	{
		// Safety check: Validate that data is not null before accessing its properties
		// This prevents NullReferenceException if Initialize is called with null data
		if (data == null)
		{
			Debug.LogError("MapCardUI.Initialize: MapData parameter is null! Cannot initialize card.");
			return;
		}
		
		mapData = data;
		sceneLoader = loader;
		lockedCardAlpha = cardAlpha;
		
		// Set warning reference via dependency injection (if provided)
		// This avoids expensive FindObjectsByType searches that would happen every time TryStartGame is called
		if (warningRef != null)
		{
			warning = warningRef;
		}

		// Set map name text using localization (Method 2)
		if (mapNameText != null)
		{
			// Unsubscribe from old LocalizedString if exists
			if (localizedString != null)
			{
				localizedString.StringChanged -= UpdateText;
			}

			// Create new LocalizedString with the map's localization key
			// data is guaranteed to be non-null due to check at method start
			string localizationKey;
			if (string.IsNullOrEmpty(data.LocalizationKey))
			{
				localizationKey = data.MapName;
			}
			else
			{
				localizationKey = data.LocalizationKey;
			}
			localizedString = new LocalizedString(localizationTableName, localizationKey);
			
			// Subscribe to localization changes (auto-updates when locale or key changes)
			localizedString.StringChanged += UpdateText;
		}

		// Set map image if available
		if (mapImage != null && mapData.MapImage != null)
		{
			mapImage.sprite = mapData.MapImage;
		}

		// Setup button (locked cards are non-interactable)
		if (mapButton != null)
		{
			mapButton.interactable = !mapData.IsLocked;
			mapButton.onClick.RemoveAllListeners();
			mapButton.onClick.AddListener(OnMapCardClicked);
		}

		// Fade entire card when locked and show lock icon
		UpdateCardLockedState();
	}

	private void OnMapCardClicked()
	{
		// Prevent clicking locked maps
		if (mapData != null && mapData.IsLocked)
		{
			return;
		}
		
		// Delegate core game-start rules to central service
		TryStartGame();
	}
	
	// Public method to be called from play button - uses the same shared logic
	public void OnPlayButtonClicked()
	{
		// Prevent play button from working on locked maps
		if (mapData != null && mapData.IsLocked)
		{
			return;
		}
		
		TryStartGame();
	}
	
	// Shared path for starting the game from this card (card click or play button).
	// All validation and scene loading is delegated to GameStartService.
	private void TryStartGame()
	{
		// Find dropdown if not already found (this is local to each card, so no injection needed)
		if (levelDropdown == null)
		{
			levelDropdown = GetComponentInChildren<CustomTMPDropdown>();
		}
		
		// Warning should be set via dependency injection in Initialize().
		// If it's still null, log an error but don't search (performance optimization).
		// The warning reference should be provided by MapCardManager during card initialization.
		if (warning == null)
		{
			Debug.LogError("MapCardUI: ChooseLevelWarning reference not provided! Make sure MapCardManager passes the warning reference during Initialize().");
		}

		GameStartService.TryStartGame(mapData, levelDropdown, warning, sceneLoader, this);
	}

	private void UpdateText(string translatedText)
	{
		if (mapNameText == null) return;

		// Check if translation failed (shows error message)
		if (translatedText.Contains("No translation found"))
		{
			// Fallback to map name if translation failed
			mapNameText.text = mapData != null ? mapData.MapName : translatedText;
		}
		else
		{
			mapNameText.text = translatedText;
		}
	}

	// Update card visual state based on locked status
	private void UpdateCardLockedState()
	{
		if (canvasGroup != null)
		{
			// Fade entire card when locked, full opacity when unlocked
			canvasGroup.alpha = mapData.IsLocked ? lockedCardAlpha : 1f;
			
			// Disable UI interactions (buttons, etc.) when locked, but keep raycasts enabled
			// so the scroll view can still detect drag events for scrolling
			canvasGroup.interactable = !mapData.IsLocked;
			canvasGroup.blocksRaycasts = true; // Always allow raycasts so scroll view can detect drags
		}

		// Show/hide lock icon
		UpdateLockIcon();
	}

	// Update lock icon display
	private void UpdateLockIcon()
	{
		if (lockImageDisplay == null)
		{
			Debug.LogWarning("MapCardUI: LockImageDisplay is not assigned in prefab!");
			return;
		}

		// Show or hide lock icon based on locked status
		if (mapData != null)
		{
			lockImageDisplay.gameObject.SetActive(mapData.IsLocked);
		}
		else
		{
			// If mapData is not set yet, hide the lock icon
			lockImageDisplay.gameObject.SetActive(false);
		}
	}

	public void UpdateCard(MapData data)
	{
		// Update card with new map data (preserve existing alpha)
		Initialize(data, sceneLoader, lockedCardAlpha);
	}
}
