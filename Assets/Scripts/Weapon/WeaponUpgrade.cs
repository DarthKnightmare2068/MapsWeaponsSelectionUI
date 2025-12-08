using UnityEngine;

// Handles weapon upgrade logic for UI testing
public class WeaponUpgrade : MonoBehaviour
{
	[Header("Currency Display (UI Only)")]
	[SerializeField] private int baseBMB = 2000; // Base BMB currency cost for BMB button
	[SerializeField] private int baseEWAR = 3;   // Base EWAR currency cost for EWAR button

	private WeaponCardData weaponCardData;

	private void Awake()
	{
		weaponCardData = GetComponent<WeaponCardData>();
		if (weaponCardData == null)
		{
			weaponCardData = GetComponentInParent<WeaponCardData>();
		}
	}

	// Upgrade using BMB currency - UI test only, no currency check
	public bool UpgradeWithCurrency1(WeaponData weaponData)
	{
		if (!ValidateUpgrade(weaponData)) return false;

		bool success = weaponData.Upgrade();
		if (success)
		{
			// Increase currency costs for next upgrade
			baseBMB += 500;
			baseEWAR += 2;
			weaponCardData?.RefreshDisplay();
		}

		return success;
	}

	// Upgrade using EWAR currency - UI test only, no currency check
	public bool UpgradeWithCurrency2(WeaponData weaponData)
	{
		if (!ValidateUpgrade(weaponData)) return false;

		bool success = weaponData.Upgrade();
		if (success)
		{
			// Increase currency costs for next upgrade
			baseBMB += 500;
			baseEWAR += 2;
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
	public int GetCurrency1Cost() => baseBMB;
	public int GetCurrency2Cost() => baseEWAR;

	// Reset currency costs (call when setting a new weapon)
	public void ResetCurrencyCosts(WeaponData weaponData)
	{
		if (weaponData == null)
		{
			baseBMB = 2000;
			baseEWAR = 3;
		}
		else
		{
			// Calculate current costs based on upgrade level (reset to base, then add upgrade costs)
			baseBMB = 2000 + (weaponData.UpgradeCount * 500);
			baseEWAR = 3 + (weaponData.UpgradeCount * 2);
		}
	}
}