using UnityEngine;

// Handles weapon upgrade logic for UI testing
public class WeaponUpgrade : MonoBehaviour
{
	[Header("Currency Display (UI Only)")]
	[SerializeField] private int baseCurrency1Amount = 2000; // Base currency cost for button 1
	[SerializeField] private int baseCurrency2Amount = 3; // Base currency cost for button 2
	
	private int currency1Amount; // Current currency cost for button 1
	private int currency2Amount; // Current currency cost for button 2
	private WeaponCardData weaponCardData;

	private void Awake()
	{
		weaponCardData = GetComponent<WeaponCardData>();
		if (weaponCardData == null)
		{
			weaponCardData = GetComponentInParent<WeaponCardData>();
		}
		
		// Initialize currency amounts from base values
		currency1Amount = baseCurrency1Amount;
		currency2Amount = baseCurrency2Amount;
	}

	// Upgrade using currency 1 (button 1) - UI test only, no currency check
	public bool UpgradeWithCurrency1(WeaponData weaponData)
	{
		if (!ValidateUpgrade(weaponData)) return false;

		bool success = weaponData.Upgrade();
		if (success)
		{
			// Increase currency costs for next upgrade
			currency1Amount += 500;
			currency2Amount += 2;
			weaponCardData?.RefreshDisplay();
		}

		return success;
	}

	// Upgrade using currency 2 (button 2) - UI test only, no currency check
	public bool UpgradeWithCurrency2(WeaponData weaponData)
	{
		if (!ValidateUpgrade(weaponData)) return false;

		bool success = weaponData.Upgrade();
		if (success)
		{
			// Increase currency costs for next upgrade
			currency1Amount += 500;
			currency2Amount += 2;
			weaponCardData?.RefreshDisplay();
		}

		return success;
	}

	// Validate if weapon can be upgraded
	private bool ValidateUpgrade(WeaponData weaponData)
	{
		if (weaponData == null) return false;
		return weaponData.CanUpgrade;
	}

	// Get currency costs (for UI display)
	public int GetCurrency1Cost() => currency1Amount;
	public int GetCurrency2Cost() => currency2Amount;
	
	// Reset currency costs (call when setting a new weapon)
	public void ResetCurrencyCosts(WeaponData weaponData)
	{
		if (weaponData == null)
		{
			currency1Amount = baseCurrency1Amount;
			currency2Amount = baseCurrency2Amount;
		}
		else
		{
			// Calculate current costs based on upgrade level
			currency1Amount = baseCurrency1Amount + (weaponData.UpgradeCount * 500);
			currency2Amount = baseCurrency2Amount + (weaponData.UpgradeCount * 2);
		}
	}
}
