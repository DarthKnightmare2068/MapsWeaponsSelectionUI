using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Big weapon preview: shows image and name of the currently selected weapon
public class WeaponCardDisplay : MonoBehaviour
{
	[SerializeField] private Image weaponImage;
	[SerializeField] private TextMeshProUGUI weaponNameText;

	private WeaponData currentWeapon;

	public void SetWeapon(WeaponData data)
	{
		currentWeapon = data;

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
}


