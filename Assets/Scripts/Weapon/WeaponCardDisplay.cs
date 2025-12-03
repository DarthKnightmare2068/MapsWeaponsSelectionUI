using UnityEngine;
using UnityEngine.UI;

// Big weapon preview: shows image of the currently selected weapon
public class WeaponCardDisplay : MonoBehaviour
{
	[SerializeField] private Image weaponImage;

	[Header("Buttons")]
	[SerializeField] private Button useButton; // Use button for later use
	[SerializeField] private Button rentOutButton; // Rent out button for later use

	private WeaponData currentWeapon;
	private WeaponCardSelection currentSelectedCard; // Reference to currently selected card

	private void Awake()
	{
		// Setup buttons (for future use)
		if (useButton != null)
		{
			useButton.onClick.AddListener(OnUseButtonClicked);
		}

		if (rentOutButton != null)
		{
			rentOutButton.onClick.AddListener(OnRentOutButtonClicked);
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
	}

	public void SetWeapon(WeaponData data, WeaponCardSelection selectedCard = null)
	{
		currentWeapon = data;
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
		// Set status to "Used" on the currently selected card
		if (currentSelectedCard != null)
		{
			currentSelectedCard.SetStatus(WeaponStatus.Used);
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

