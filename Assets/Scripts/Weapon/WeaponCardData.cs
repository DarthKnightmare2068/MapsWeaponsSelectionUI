using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

// Stats panel: shows weapon name, attributes (names on left, values on right), and upgrade buttons
public class WeaponCardData : MonoBehaviour
{
	[Header("Text Elements")]
	[SerializeField] private TextMeshProUGUI weaponNameText; // Weapon name at top of stats panel
	[SerializeField] private TextMeshProUGUI attributesNamesText; // Left side: attribute names with \n\n spacing
	[SerializeField] private TextMeshProUGUI attributesValuesText; // Right side: attribute values

	[Header("Upgrade Buttons")]
	[SerializeField] private Button upgradeButton1; // First upgrade button
	[SerializeField] private Button upgradeButton2; // Second upgrade button
	[SerializeField] private TextMeshProUGUI upgradeButton1Text; // Text to show currency 1 cost on button 1
	[SerializeField] private TextMeshProUGUI upgradeButton2Text; // Text to show currency 2 cost on button 2

	private WeaponData currentWeapon;
	private WeaponUpgrade weaponUpgrade;

	// Weapon attributes enum
	private enum WeaponAttribute
	{
		Damage,
		Dispersion,
		RateOfFire,
		ReloadSpeed,
		Ammunition
	}

	// Dictionary to store attribute names
	private static readonly Dictionary<WeaponAttribute, string> attributeNames = new Dictionary<WeaponAttribute, string>
	{
		{ WeaponAttribute.Damage, "Damage" },
		{ WeaponAttribute.Dispersion, "Dispersion" },
		{ WeaponAttribute.RateOfFire, "Rate of Fire" },
		{ WeaponAttribute.ReloadSpeed, "Reload Speed" },
		{ WeaponAttribute.Ammunition, "Ammunition" }
	};

	// Dictionary to store attribute format strings
	private static readonly Dictionary<WeaponAttribute, string> attributeFormats = new Dictionary<WeaponAttribute, string>
	{
		{ WeaponAttribute.Damage, "0" },        // Damage: integer format
		{ WeaponAttribute.Dispersion, "0" },   // Dispersion: integer format
		{ WeaponAttribute.RateOfFire, "0.0" }, // Rate of Fire: one decimal format
		{ WeaponAttribute.ReloadSpeed, "0" },  // Reload Speed: integer format
		{ WeaponAttribute.Ammunition, "0" }    // Ammunition: integer format
	};

	// Ordered list of attributes (defines the display order)
	private static readonly WeaponAttribute[] attributeOrder = new WeaponAttribute[]
	{
		WeaponAttribute.Damage,
		WeaponAttribute.Dispersion,
		WeaponAttribute.RateOfFire,
		WeaponAttribute.ReloadSpeed,
		WeaponAttribute.Ammunition
	};

	private void Awake()
	{
		// Find or auto-add WeaponUpgrade component
		weaponUpgrade = GetComponent<WeaponUpgrade>();
		if (weaponUpgrade == null)
		{
			weaponUpgrade = GetComponentInParent<WeaponUpgrade>();
		}
		if (weaponUpgrade == null)
		{
			weaponUpgrade = gameObject.AddComponent<WeaponUpgrade>();
		}

		// Setup upgrade buttons
		upgradeButton1?.onClick.AddListener(OnUpgradeButton1Clicked);
		upgradeButton2?.onClick.AddListener(OnUpgradeButton2Clicked);
	}

	private void OnDestroy()
	{
		// Clean up button listeners
		if (upgradeButton1 != null)
		{
			upgradeButton1.onClick.RemoveListener(OnUpgradeButton1Clicked);
		}

		if (upgradeButton2 != null)
		{
			upgradeButton2.onClick.RemoveListener(OnUpgradeButton2Clicked);
		}
	}

	public void SetWeapon(WeaponData data)
	{
		currentWeapon = data;

		if (data == null)
		{
			// Clear weapon name and show placeholder/empty stats
			if (weaponNameText != null)
			{
				weaponNameText.text = string.Empty;
			}

			SetEmptyData();
			return;
		}

		// Set weapon name with upgrade level (base level is 1)
		if (weaponNameText != null)
		{
			int currentLevel = data.UpgradeCount + 1; // Base level is 1, so add 1 to upgrade count
			weaponNameText.text = $"{data.WeaponName} Lv.{currentLevel}";
		}
		
		// Reset currency costs in WeaponUpgrade when setting new weapon
		if (weaponUpgrade != null)
		{
			weaponUpgrade.ResetCurrencyCosts(data);
		}
		
		// Update currency costs display
		UpdateCurrencyCosts();

		// Get attribute values from WeaponData using enum
		List<string> attributeValues = GetAttributeValues(data);

		// Set attributes names (left side) with \n\n spacing between each
		if (attributesNamesText != null)
		{
			attributesNamesText.text = BuildAttributeText(GetAttributeNames());
		}

		// Set attributes values (right side) with \n\n spacing between each
		if (attributesValuesText != null)
		{
			attributesValuesText.text = BuildAttributeText(attributeValues);
		}
	}

	private List<string> GetAttributeNames()
	{
		return attributeOrder.Select(attr => attributeNames[attr]).ToList();
	}

	private List<string> GetAttributeValues(WeaponData data)
	{
		if (data == null) return new List<string>();

		List<string> values = new List<string>();
		foreach (WeaponAttribute attr in attributeOrder)
		{
			string format = attributeFormats[attr];
			float value = GetAttributeValue(data, attr);
			values.Add(value.ToString(format));
		}
		return values;
	}

	private float GetAttributeValue(WeaponData data, WeaponAttribute attribute)
	{
		switch (attribute)
		{
			case WeaponAttribute.Damage:
				return data.Damage;
			case WeaponAttribute.Dispersion:
				return data.Dispersion;
			case WeaponAttribute.RateOfFire:
				return data.RateOfFire;
			case WeaponAttribute.ReloadSpeed:
				return data.ReloadSpeed;
			case WeaponAttribute.Ammunition:
				return data.Ammunition;
			default:
				return 0f;
		}
	}

	private string BuildAttributeText(List<string> attributes)
	{
		if (attributes == null || attributes.Count == 0)
			return "";

		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		for (int i = 0; i < attributes.Count; i++)
		{
			sb.Append(attributes[i]);
			if (i < attributes.Count - 1)
			{
				sb.Append("\n\n");
			}
		}
		return sb.ToString();
	}

	private void SetEmptyData()
	{
		// Clear weapon name when no data
		if (weaponNameText != null)
		{
			weaponNameText.text = string.Empty;
		}

		// Set attributes names (left side) - always show names even when no data
		if (attributesNamesText != null)
		{
			attributesNamesText.text = BuildAttributeText(GetAttributeNames());
		}

		// Set empty values (right side)
		if (attributesValuesText != null)
		{
			List<string> emptyValues = new List<string>();
			for (int i = 0; i < attributeOrder.Length; i++)
			{
				emptyValues.Add("-");
			}
			attributesValuesText.text = BuildAttributeText(emptyValues);
		}
	}

	// Upgrade button handlers
	private void OnUpgradeButton1Clicked()
	{
		if (currentWeapon != null && weaponUpgrade != null)
		{
			weaponUpgrade.UpgradeWithCurrency1(currentWeapon);
		}
	}

	private void OnUpgradeButton2Clicked()
	{
		if (currentWeapon != null && weaponUpgrade != null)
		{
			weaponUpgrade.UpgradeWithCurrency2(currentWeapon);
		}
	}

	// Refresh display after upgrade
	public void RefreshDisplay()
	{
		if (currentWeapon != null)
		{
			SetWeapon(currentWeapon);
		}
	}
	
	// Update currency costs on upgrade buttons
	private void UpdateCurrencyCosts()
	{
		if (weaponUpgrade == null) return;
		
		bool canUpgrade = currentWeapon != null && currentWeapon.CanUpgrade;
		
		// Update button 1 text
		if (upgradeButton1Text != null)
		{
			upgradeButton1Text.text = canUpgrade ? weaponUpgrade.GetCurrency1Cost().ToString() : "MAX";
		}
		
		// Update button 2 text
		if (upgradeButton2Text != null)
		{
			upgradeButton2Text.text = canUpgrade ? weaponUpgrade.GetCurrency2Cost().ToString() : "MAX";
		}
	}
}
