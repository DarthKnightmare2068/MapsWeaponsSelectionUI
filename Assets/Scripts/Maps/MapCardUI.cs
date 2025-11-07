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
	private UIButtonSceneLoader sceneLoader;
	private LocalizedString localizedString;
	private CanvasGroup canvasGroup; // Used to fade entire card
	private float lockedCardAlpha; // Alpha value for locked cards (set from MapCardManager)

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
	}

	private void OnDestroy()
	{
		// Unsubscribe to prevent memory leaks
		if (localizedString != null)
		{
			localizedString.StringChanged -= UpdateText;
		}
	}

	public void Initialize(MapData data, UIButtonSceneLoader loader, float cardAlpha = 0.3f)
	{
		mapData = data;
		sceneLoader = loader;
		lockedCardAlpha = cardAlpha;

		// Set map name text using localization (Method 2)
		if (mapNameText != null)
		{
			// Unsubscribe from old LocalizedString if exists
			if (localizedString != null)
			{
				localizedString.StringChanged -= UpdateText;
			}

			// Create new LocalizedString with the map's localization key
			string localizationKey = string.IsNullOrEmpty(data.LocalizationKey) ? data.MapName : data.LocalizationKey;
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
		// Load the scene associated with this map card
		if (mapData != null && sceneLoader != null)
		{
			sceneLoader.LoadSceneByName(mapData.SceneName);
		}
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
			
			// Optionally disable interactions on entire card when locked
			canvasGroup.interactable = !mapData.IsLocked;
			canvasGroup.blocksRaycasts = !mapData.IsLocked;
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
