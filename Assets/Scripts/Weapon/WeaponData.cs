using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections.Generic;

[System.Serializable]
public class WeaponData
{
	// Weapon attributes enum
	public enum WeaponAttribute
	{
		Damage,
		Dispersion,
		RateOfFire,
		ReloadSpeed,
		Ammunition
	}

	// Dictionary to store attribute localization keys
	private static readonly Dictionary<WeaponAttribute, string> AttributeLocalizationKeys = new Dictionary<WeaponAttribute, string>
	{
		{ WeaponAttribute.Damage, "weaponInfo.Damage" },
		{ WeaponAttribute.Dispersion, "weaponInfo.Dispersion" },
		{ WeaponAttribute.RateOfFire, "weaponInfo.RateOfFire" },
		{ WeaponAttribute.ReloadSpeed, "weaponInfo.ReloadSpeed" },
		{ WeaponAttribute.Ammunition, "weaponInfo.Ammunition" }
	};

	// Cached dictionary to store attribute names (localized)
	// Cache is invalidated when locale changes to avoid unnecessary allocations
	private static Dictionary<WeaponAttribute, string> cachedAttributeNames;
	private static string cachedLocaleCode;

	// Dictionary to store attribute names (localized)
	// Cached for performance - automatically refreshes when locale changes
	public static Dictionary<WeaponAttribute, string> AttributeNames
	{
		get
		{
			string currentLocale = LocalizationSettings.SelectedLocale != null 
				? LocalizationSettings.SelectedLocale.Identifier.Code 
				: "en-US";

			// Refresh cache if locale changed or cache is null
			if (cachedAttributeNames == null || cachedLocaleCode != currentLocale)
			{
				cachedAttributeNames = new Dictionary<WeaponAttribute, string>();
				foreach (var kvp in AttributeLocalizationKeys)
				{
					cachedAttributeNames[kvp.Key] = LocalizationManager.GetWeaponInfoSync(kvp.Value);
				}
				cachedLocaleCode = currentLocale;
			}

			return cachedAttributeNames;
		}
	}

	// Public method to invalidate the cache (call when locale changes)
	// This is called automatically when locale changes, but can be called manually if needed
	public static void InvalidateAttributeNamesCache()
	{
		cachedAttributeNames = null;
		cachedLocaleCode = null;
	}

	// Dictionary to store attribute format strings
	public static readonly Dictionary<WeaponAttribute, string> AttributeFormats = new Dictionary<WeaponAttribute, string>
	{
		{ WeaponAttribute.Damage, "0" },        // Damage: integer format
		{ WeaponAttribute.Dispersion, "0" },     // Dispersion: integer format
		{ WeaponAttribute.RateOfFire, "0" },    // Rate of Fire: integer format
		{ WeaponAttribute.ReloadSpeed, "0" },   // Reload Speed: integer format
		{ WeaponAttribute.Ammunition, "0" }      // Ammunition: integer format
	};

	// Ordered list of attributes (defines the display order)
	public static readonly WeaponAttribute[] AttributeOrder = new WeaponAttribute[]
	{
		WeaponAttribute.Damage,
		WeaponAttribute.Dispersion,
		WeaponAttribute.RateOfFire,
		WeaponAttribute.ReloadSpeed,
		WeaponAttribute.Ammunition
	};

	// Static default maximum upgrade level (can be set by WeaponCardManager)
	// Note: This represents the maximum level (e.g., 6), not the number of upgrades
	public static int DefaultWeaponMaxLv { get; set; } = 6;

	[Header("Basic Info")]
	[SerializeField] private string weaponName;
	[SerializeField] private Sprite weaponImage;
	
	[Header("Weapon Attributes")]
	[SerializeField] private float damage;
	[SerializeField] private float dispersion;
	[SerializeField] private float rateOfFire;
	[SerializeField] private float reloadSpeed;
	[SerializeField] private float ammunition;
	
	[Header("Upgrade Info")]
	[SerializeField] private int upgradeCount = 0;
	[SerializeField] private int weaponMaxLv = 6; // Maximum level for this weapon (e.g., 6 means base level 1 + 5 upgrades, initialized from DefaultWeaponMaxLv)

	public string WeaponName => weaponName;
	public Sprite WeaponImage => weaponImage;
	public float Damage => damage;
	public float Dispersion => dispersion;
	public float RateOfFire => rateOfFire;
	public float ReloadSpeed => reloadSpeed;
	public float Ammunition => ammunition;
	public int UpgradeCount => upgradeCount;
	public int WeaponMaxLv => weaponMaxLv;
	public bool CanUpgrade => (upgradeCount + 1) < weaponMaxLv; // Current level (upgradeCount + 1) must be less than max level

	// Set the maximum upgrade level for this weapon
	public void SetMaxLevel(int maxLevel)
	{
		if (maxLevel > 0)
		{
			weaponMaxLv = maxLevel;
		}
	}

	public WeaponData(string weaponName, float damage, float dispersion, float rateOfFire, float reloadSpeed, float ammunition, Sprite weaponImage = null)
	{
		this.weaponName = weaponName;
		this.weaponImage = weaponImage;
		this.damage = damage;
		this.dispersion = dispersion;
		this.rateOfFire = rateOfFire;
		this.reloadSpeed = reloadSpeed;
		this.ammunition = ammunition;
		this.upgradeCount = 0;
		this.weaponMaxLv = DefaultWeaponMaxLv; // Initialize with static default
	}

	// Upgrade method: applies upgrade stat changes
	// Note: These are placeholder upgrade values for UI testing. Adjust for actual game balance.
	public bool Upgrade()
	{
		if (!CanUpgrade) return false;

		// Apply upgrade stat changes
		// Damage: +50% per upgrade (multiplicative)
		damage *= 1.5f;
		// Dispersion: -0.1 per upgrade (lower = more accurate, clamped to 0)
		dispersion = Mathf.Max(0f, dispersion - 0.1f);
		// Rate of Fire: This value represents firing interval in some unit - lower = faster firing
		// If using RPM (rounds per minute), this logic should be ADDING not subtracting
		// TODO: Clarify rate of fire unit and adjust upgrade logic accordingly
		rateOfFire = Mathf.Max(0f, rateOfFire - 2f);
		// Reload Speed: This value represents reload time - lower = faster reload
		// If this should represent reload speed (higher = faster), invert the logic
		// TODO: Clarify reload speed semantics and adjust upgrade logic accordingly
		reloadSpeed = Mathf.Max(0f, reloadSpeed - 0.1f);
		// Ammunition: +1 per upgrade
		ammunition += 1f;
		upgradeCount++;

		return true;
	}
}