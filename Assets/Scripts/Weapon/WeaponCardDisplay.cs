using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using TMPro;

// Big weapon preview: shows image of the currently selected weapon
public class WeaponCardDisplay : MonoBehaviour
{
	[SerializeField] private Image weaponImage;

	[Header("Buttons")]
	[SerializeField] private Button useButton; // Use button for later use
	[SerializeField] private Button rentOutButton; // Rent out button for later use
	[SerializeField] private TextMeshProUGUI useButtonText; // Text component for Use button
	[SerializeField] private TextMeshProUGUI rentOutButtonText; // Text component for Rent Out button

	private WeaponCardSelection currentSelectedCard; // Reference to currently selected card
	
	// Localization subscriptions for button texts (auto-update when locale changes)
	private LocalizedString localizedUseText;
	private LocalizedString localizedRentOutText;

	private void Awake()
	{
		// Setup buttons
		if (useButton != null)
		{
			useButton.onClick.AddListener(OnUseButtonClicked);
		}

		if (rentOutButton != null)
		{
			rentOutButton.onClick.AddListener(OnRentOutButtonClicked);
		}

		// Setup localized button texts with subscriptions (auto-update when locale changes)
		if (useButtonText != null)
		{
			localizedUseText = new LocalizedString("Weapon Info", "weaponInfo.Use");
			localizedUseText.StringChanged += (text) => { if (useButtonText != null) useButtonText.text = text; };
			// Set initial text
			useButtonText.text = LocalizationManager.GetWeaponInfoSync("weaponInfo.Use");
		}

		if (rentOutButtonText != null)
		{
			localizedRentOutText = new LocalizedString("Weapon Info", "weaponInfo.RentOut");
			localizedRentOutText.StringChanged += (text) => { if (rentOutButtonText != null) rentOutButtonText.text = text; };
			// Set initial text
			rentOutButtonText.text = LocalizationManager.GetWeaponInfoSync("weaponInfo.RentOut");
		}
	}

	private void OnDestroy()
	{
		// Clean up button listeners
		if (useButton != null)
		{
			useButton.onClick.RemoveListener(OnUseButtonClicked);
		}

		if (rentOutButton != null)
		{
			rentOutButton.onClick.RemoveListener(OnRentOutButtonClicked);
		}
		
		// Clean up localization subscriptions
		// Note: LocalizedString doesn't require explicit unsubscription as it uses weak references,
		// but we clear references to help GC
		localizedUseText = null;
		localizedRentOutText = null;
	}

	public void SetWeapon(WeaponData data, WeaponCardSelection selectedCard = null)
	{
		currentSelectedCard = selectedCard;

		if (weaponImage != null)
		{
			weaponImage.sprite = data != null ? data.WeaponImage : null;
			weaponImage.enabled = weaponImage.sprite != null;
		}
	}

	// Button handlers
	private void OnUseButtonClicked()
	{
		// Set status to "Use" on the currently selected card
		if (currentSelectedCard != null)
		{
			currentSelectedCard.SetStatus(WeaponStatus.Use);
		}
	}

	private void OnRentOutButtonClicked()
	{
		// Set status to "Rent Out" on the currently selected card
		if (currentSelectedCard != null)
		{
			currentSelectedCard.SetStatus(WeaponStatus.RentOut);
		}
	}
}

