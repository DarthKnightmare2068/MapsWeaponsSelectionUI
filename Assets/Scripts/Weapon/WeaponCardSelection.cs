using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Attach this to each weapon option card prefab in the horizontal list.
// It represents a single selectable weapon.
public class WeaponCardSelection : MonoBehaviour
{
	[SerializeField] private Image weaponImage;

	[Header("Status Text")]
	[SerializeField] private TextMeshProUGUI usedStatusText; // "Used" status text
	[SerializeField] private TextMeshProUGUI rentOutStatusText; // "Rent Out" status text

	[Header("Frame Display")]
	[SerializeField] private GameObject normalFrame; // Normal frame (shown when not selected)
	[SerializeField] private GameObject glowingFrame; // Glowing frame (shown when selected)

	private TextMeshProUGUI weaponNameText;
	private WeaponData weaponData;
	private WeaponCardManager manager;
	private bool isSelected = false;

	public WeaponData WeaponData => weaponData;

	private void Awake()
	{
		// Setup card click detection (click anywhere on the card)
		Button cardButton = GetComponent<Button>();
		if (cardButton != null)
		{
			cardButton.onClick.AddListener(OnCardClicked);
		}

		// Initialize status texts as off
		SetStatus(WeaponStatus.None);
		
		// Initialize frames: NormalFrame active, GlowingFrame inactive by default
		UpdateFrameDisplay(false);
	}

	private void OnDestroy()
	{
		Button cardButton = GetComponent<Button>();
		if (cardButton != null)
		{
			cardButton.onClick.RemoveListener(OnCardClicked);
		}
	}

	public void Initialize(WeaponData data, WeaponCardManager owner)
	{
		weaponData = data;
		manager = owner;

		if (weaponImage != null)
		{
			weaponImage.sprite = data != null ? data.WeaponImage : null;
			weaponImage.enabled = weaponImage.sprite != null;
		}

		if (weaponNameText != null)
		{
			weaponNameText.text = data != null ? data.WeaponName : string.Empty;
		}
	}

	private void OnCardClicked()
	{
		if (manager != null)
		{
			manager.OnWeaponCardSelected(this);
		}
	}
	
	// Update frame display based on selection state
	public void SetSelected(bool selected)
	{
		isSelected = selected;
		UpdateFrameDisplay(selected);
	}
	
	private void UpdateFrameDisplay(bool selected)
	{
		// Show NormalFrame when not selected, hide when selected
		if (normalFrame != null)
		{
			normalFrame.SetActive(!selected);
		}
		
		// Show GlowingFrame when selected, hide when not selected
		if (glowingFrame != null)
		{
			glowingFrame.SetActive(selected);
		}
	}

	// Set weapon status: Used, Rent Out, or None
	public void SetStatus(WeaponStatus status)
	{
		// Turn off both status texts first
		if (usedStatusText != null)
		{
			usedStatusText.gameObject.SetActive(false);
		}

		if (rentOutStatusText != null)
		{
			rentOutStatusText.gameObject.SetActive(false);
		}

		// Turn on the appropriate status text
		switch (status)
		{
			case WeaponStatus.Used:
				if (usedStatusText != null)
				{
					usedStatusText.gameObject.SetActive(true);
				}
				break;

			case WeaponStatus.RentOut:
				if (rentOutStatusText != null)
				{
					rentOutStatusText.gameObject.SetActive(true);
				}
				break;

			case WeaponStatus.None:
			default:
				// Both already turned off above
				break;
		}
	}
}

// Weapon status enum
public enum WeaponStatus
{
	None,    // No status (both texts off)
	Used,    // "Used" text is on
	RentOut  // "Rent Out" text is on
}
