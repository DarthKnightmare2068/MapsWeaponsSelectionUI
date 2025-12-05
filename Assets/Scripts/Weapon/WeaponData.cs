using UnityEngine;

[System.Serializable]
public class WeaponData
{
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
	private const int MAX_UPGRADE_COUNT = 5;

	public string WeaponName => weaponName;
	public Sprite WeaponImage => weaponImage;
	public float Damage => damage;
	public float Dispersion => dispersion;
	public float RateOfFire => rateOfFire;
	public float ReloadSpeed => reloadSpeed;
	public float Ammunition => ammunition;
	public int UpgradeCount => upgradeCount;
	public bool CanUpgrade => upgradeCount < MAX_UPGRADE_COUNT;

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
	}

	public WeaponData(string weaponName, string unusedLocalizationKey, float damage, float dispersion, float rateOfFire, float reloadSpeed, float ammunition, Sprite weaponImage = null)
	{
		this.weaponName = weaponName;
		this.weaponImage = weaponImage;
		this.damage = damage;
		this.dispersion = dispersion;
		this.rateOfFire = rateOfFire;
		this.reloadSpeed = reloadSpeed;
		this.ammunition = ammunition;
		this.upgradeCount = 0;
	}

	// Upgrade method: applies upgrade stat changes
	public bool Upgrade()
	{
		if (!CanUpgrade) return false;

		// Apply upgrade stat changes
		damage *= 1.5f;
		dispersion = Mathf.Max(0f, dispersion - 0.1f); // Prevent negative dispersion
		rateOfFire = Mathf.Max(0f, rateOfFire - 0.2f); // Reduce rate of fire by 0.2 (lower is better/faster)
		reloadSpeed = Mathf.Max(0f, reloadSpeed - 0.1f); // Prevent negative reload speed
		ammunition += 1f;
		upgradeCount++;

		return true;
	}
}
