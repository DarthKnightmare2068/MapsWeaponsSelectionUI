using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using TMPro;

// Handles Back and Done buttons in Weapon Menu scene
public class WeaponMenuButtons : MonoBehaviour
{
	[SerializeField] private GameObject backButton;
	[SerializeField] private GameObject doneButton;
	[SerializeField] private SceneLoader sceneLoader;
	
	private TextMeshProUGUI backButtonText; // Text component for back button (auto-found in Awake)
	private LocalizedString localizedBackButton;
	
	private void Awake()
	{
		// Auto-find button GameObjects if not assigned
		if (backButton == null)
		{
			Transform backTransform = transform.Find("Back");
			if (backTransform != null)
			{
				backButton = backTransform.gameObject;
			}
		}
		
		if (doneButton == null)
		{
			Transform doneTransform = transform.Find("Done");
			if (doneTransform != null)
			{
				doneButton = doneTransform.gameObject;
			}
		}
		
		// Auto-find SceneLoader if not assigned
		if (sceneLoader == null)
		{
			Debug.LogWarning("WeaponMenuButtons: SceneLoader not assigned in Inspector. Using expensive FindFirstObjectByType as fallback. Please assign in Inspector for better performance.");
			sceneLoader = FindFirstObjectByType<SceneLoader>();
		}
		
		// Auto-find back button text if not assigned
		if (backButtonText == null && backButton != null)
		{
			backButtonText = backButton.GetComponentInChildren<TextMeshProUGUI>();
		}
	}
	
	private void Start()
	{
		// Initialize back button text localization
		InitializeBackButtonText();
		
		// Setup Back button - goes back to Map Menu
		if (backButton != null)
		{
			Button backButtonComponent = backButton.GetComponent<Button>();
			if (backButtonComponent != null)
			{
				backButtonComponent.onClick.RemoveAllListeners();
				backButtonComponent.onClick.AddListener(OnBackButtonClicked);
			}
			else
			{
				Debug.LogWarning("WeaponMenuButtons: Back button GameObject found but Button component is missing!");
			}
		}
		else
		{
			Debug.LogWarning("WeaponMenuButtons: Back button GameObject not found!");
		}
		
		// Setup Done button - loads the selected map scene
		if (doneButton != null)
		{
			Button doneButtonComponent = doneButton.GetComponent<Button>();
			if (doneButtonComponent != null)
			{
				doneButtonComponent.onClick.RemoveAllListeners();
				doneButtonComponent.onClick.AddListener(OnDoneButtonClicked);
			}
			else
			{
				Debug.LogWarning("WeaponMenuButtons: Done button GameObject found but Button component is missing!");
			}
		}
		else
		{
			Debug.LogWarning("WeaponMenuButtons: Done button GameObject not found!");
		}
	}
	
	private void OnBackButtonClicked()
	{
		// Load Map Menu scene
		if (sceneLoader != null)
		{
			sceneLoader.LoadSceneByName("Map Menu");
		}
		else
		{
			Debug.LogError("WeaponMenuButtons: SceneLoader not found! Cannot load Map Menu.");
		}
	}
	
	private void OnDoneButtonClicked()
	{
		// Get the selected map scene name from MapSelectionManager
		string mapSceneName = MapSelectionManager.GetSelectedMapSceneName();
		
		if (string.IsNullOrEmpty(mapSceneName))
		{
			Debug.LogWarning("WeaponMenuButtons: No map selected! Cannot proceed to game.");
			return;
		}
		
		// Load the selected map scene
		if (sceneLoader != null)
		{
			sceneLoader.LoadSceneByName(mapSceneName);
		}
		else
		{
			Debug.LogError("WeaponMenuButtons: SceneLoader not found! Cannot load map scene.");
		}
	}
	
	private void InitializeBackButtonText()
	{
		if (backButtonText == null) return;
		
		// Setup localization for back button text
		localizedBackButton = new LocalizedString("Buttons Name", "languageButton.BackButton");
		localizedBackButton.StringChanged += UpdateBackButtonText;
		
		// Get initial localized value immediately
		try
		{
			string initialText = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("Buttons Name", "languageButton.BackButton");
			if (!string.IsNullOrEmpty(initialText))
			{
				backButtonText.text = initialText;
			}
		}
		catch (System.Exception e)
		{
			Debug.LogWarning($"WeaponMenuButtons: Failed to get localized back button text. Keeping original. Error: {e.Message}");
		}
	}
	
	private void UpdateBackButtonText(string translatedText)
	{
		if (backButtonText != null)
		{
			backButtonText.text = translatedText;
		}
	}
	
	private void OnDestroy()
	{
		// Unsubscribe to prevent memory leaks
		if (localizedBackButton != null)
		{
			localizedBackButton.StringChanged -= UpdateBackButtonText;
		}
	}
}
