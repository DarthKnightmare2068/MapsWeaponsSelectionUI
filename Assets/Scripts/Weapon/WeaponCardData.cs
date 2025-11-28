using UnityEngine;
using TMPro;

// Stats panel: shows numeric data of the currently selected weapon
public class WeaponCardData : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI damageText;
	[SerializeField] private TextMeshProUGUI dispersionText;
	[SerializeField] private TextMeshProUGUI rateOfFireText;
	[SerializeField] private TextMeshProUGUI reloadSpeedText;
	[SerializeField] private TextMeshProUGUI ammunitionText;

	private WeaponData currentWeapon;

	public void SetWeapon(WeaponData data)
	{
		currentWeapon = data;

		if (data == null)
		{
			SetTexts("-", "-", "-", "-", "-");
			return;
		}

		SetTexts(
			data.Damage.ToString("0"),
			data.Dispersion.ToString("0.0"),
			data.RateOfFire.ToString("0.0"),
			data.ReloadSpeed.ToString("0.0"),
			data.Ammunition.ToString("0")
		);
	}

	private void SetTexts(string dmg, string disp, string rof, string reload, string ammo)
	{
		if (damageText != null) damageText.text = dmg;
		if (dispersionText != null) dispersionText.text = disp;
		if (rateOfFireText != null) rateOfFireText.text = rof;
		if (reloadSpeedText != null) reloadSpeedText.text = reload;
		if (ammunitionText != null) ammunitionText.text = ammo;
	}
}


