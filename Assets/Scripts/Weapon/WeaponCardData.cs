using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

// Stats panel: shows weapon name, attributes (names on left, values on right), and upgrade buttons
public class WeaponCardData : MonoBehaviour
{
	[Header("Text Elements")]
	[SerializeField] private TextMeshProUGUI attributesNamesText; // Left side: attribute names with \n\n spacing
	[SerializeField] private TextMeshProUGUI attributesValuesText; // Right side: attribute values

	[Header("Upgrade Buttons")]
	[SerializeField] private Button upgradeButton1; // First upgrade button
	[SerializeField] private Button upgradeButton2; // Second upgrade button

	private WeaponData currentWeapon;

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
		{ WeaponAttribute.Dispersion, "0.0" },  // Dispersion: one decimal
		{ WeaponAttribute.RateOfFire, "0.0" },  // Rate of Fire: one decimal
		{ WeaponAttribute.ReloadSpeed, "0.0" }, // Reload Speed: one decimal
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
		// Setup upgrade buttons (for future use)
		if (upgradeButton1 != null)
		{
			upgradeButton1.onClick.AddListener(OnUpgradeButton1Clicked);
		}

		if (upgradeButton2 != null)
		{
			upgradeButton2.onClick.AddListener(OnUpgradeButton2Clicked);
		}
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
			SetEmptyData();
			return;
		}

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

	// Upgrade button handlers (for future implementation)
	private void OnUpgradeButton1Clicked()
	{
		// TODO: Implement upgrade logic for button 1
		Debug.Log("Upgrade Button 1 clicked for weapon: " + (currentWeapon != null ? currentWeapon.WeaponName : "None"));
	}

	private void OnUpgradeButton2Clicked()
	{
		// TODO: Implement upgrade logic for button 2
		Debug.Log("Upgrade Button 2 clicked for weapon: " + (currentWeapon != null ? currentWeapon.WeaponName : "None"));
	}
}
