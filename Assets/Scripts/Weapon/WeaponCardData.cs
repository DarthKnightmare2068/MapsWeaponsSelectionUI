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
	[SerializeField] private Button bmbButton; // BMB upgrade button
	[SerializeField] private Button ewarButton; // EWAR upgrade button
	[SerializeField] private TextMeshProUGUI bmbButtonText; // Text to show BMB cost on button
	[SerializeField] private TextMeshProUGUI ewarButtonText; // Text to show EWAR cost on button
	[SerializeField] private TextMeshProUGUI upgradeText; // Text to localize from localization table

	private WeaponData currentWeapon;
	private WeaponUpgrade weaponUpgrade;

	private void Awake()
	{
		// Setup upgrade buttons
		bmbButton?.onClick.AddListener(OnBmbButtonClicked);
		ewarButton?.onClick.AddListener(OnEwarButtonClicked);
	}

	private void Start()
	{
		// Find WeaponUpgrade component (it will find us in its Awake, so we find it in Start)
		FindWeaponUpgrade();
	}

	// Auto-creates the component if it doesn't exist (auto-find behavior)
	private void FindWeaponUpgrade()
	{
		if (weaponUpgrade == null)
		{
			weaponUpgrade = GetComponent<WeaponUpgrade>();
			if (weaponUpgrade == null)
			{
				weaponUpgrade = GetComponentInParent<WeaponUpgrade>();
			}
			// Auto-create if still not found
			if (weaponUpgrade == null)
			{
				weaponUpgrade = gameObject.AddComponent<WeaponUpgrade>();
			}
		}
	}

	private void OnDestroy()
	{
		// Clean up button listeners
		if (bmbButton != null)
		{
			bmbButton.onClick.RemoveListener(OnBmbButtonClicked);
		}

		if (ewarButton != null)
		{
			ewarButton.onClick.RemoveListener(OnEwarButtonClicked);
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
		FindWeaponUpgrade();
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
		return WeaponData.AttributeOrder.Select(attr => WeaponData.AttributeNames[attr]).ToList();
	}

	private List<string> GetAttributeValues(WeaponData data)
	{
		if (data == null) return new List<string>();

		List<string> values = new List<string>();
		foreach (WeaponData.WeaponAttribute attr in WeaponData.AttributeOrder)
		{
			string format = WeaponData.AttributeFormats[attr];
			float value = GetAttributeValue(data, attr);
			string valueString = value.ToString(format);

			// Add units for specific attributes
			if (attr == WeaponData.WeaponAttribute.RateOfFire)
			{
				valueString += " RPM";
			}
			else if (attr == WeaponData.WeaponAttribute.ReloadSpeed)
			{
				valueString += "%";
			}

			values.Add(valueString);
		}
		return values;
	}

	private float GetAttributeValue(WeaponData data, WeaponData.WeaponAttribute attribute)
	{
		switch (attribute)
		{
			case WeaponData.WeaponAttribute.Damage:
				return data.Damage;
			case WeaponData.WeaponAttribute.Dispersion:
				return data.Dispersion;
			case WeaponData.WeaponAttribute.RateOfFire:
				return data.RateOfFire;
			case WeaponData.WeaponAttribute.ReloadSpeed:
				return data.ReloadSpeed;
			case WeaponData.WeaponAttribute.Ammunition:
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
			for (int i = 0; i < WeaponData.AttributeOrder.Length; i++)
			{
				emptyValues.Add("-");
			}
			attributesValuesText.text = BuildAttributeText(emptyValues);
		}
	}

	// Upgrade button handlers
	private void OnBmbButtonClicked()
	{
		if (currentWeapon != null)
		{
			FindWeaponUpgrade();
			weaponUpgrade?.UpgradeWithCurrency1(currentWeapon);
		}
	}

	private void OnEwarButtonClicked()
	{
		if (currentWeapon != null)
		{
			FindWeaponUpgrade();
			weaponUpgrade?.UpgradeWithCurrency2(currentWeapon);
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
		FindWeaponUpgrade();
		if (weaponUpgrade == null)
		{
			Debug.LogWarning("WeaponCardData: WeaponUpgrade component not found. Currency costs cannot be displayed.");
			return;
		}

		bool canUpgrade = currentWeapon != null && currentWeapon.CanUpgrade;

		// Update BMB button text
		if (bmbButtonText != null)
		{
			bmbButtonText.text = canUpgrade ? $"{weaponUpgrade.GetCurrency1Cost()} BMB" : "MAX";
		}
		else
		{
			Debug.LogWarning("WeaponCardData: bmbButtonText is not assigned in the Inspector.");
		}

		// Update EWAR button text
		if (ewarButtonText != null)
		{
			ewarButtonText.text = canUpgrade ? $"{weaponUpgrade.GetCurrency2Cost()} EWAR" : "MAX";
		}
		else
		{
			Debug.LogWarning("WeaponCardData: ewarButtonText is not assigned in the Inspector.");
		}
	}
}
