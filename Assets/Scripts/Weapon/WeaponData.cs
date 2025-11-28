using UnityEngine;

[System.Serializable]
public class WeaponData
{
	[SerializeField] private string weaponName;
	[SerializeField] private Sprite weaponImage;
	
	[Header("Weapon Attributes")]
	[SerializeField] private float damage;
	[SerializeField] private float dispersion;
	[SerializeField] private float rateOfFire;
	[SerializeField] private float reloadSpeed;
	[SerializeField] private int ammunition;

	public string WeaponName => weaponName;
	public Sprite WeaponImage => weaponImage;
	public float Damage => damage;
	public float Dispersion => dispersion;
	public float RateOfFire => rateOfFire;
	public float ReloadSpeed => reloadSpeed;
	public int Ammunition => ammunition;

	public WeaponData(string weaponName, float damage, float dispersion, float rateOfFire, float reloadSpeed, int ammunition, Sprite weaponImage = null)
	{
		this.weaponName = weaponName;
		this.weaponImage = weaponImage;
		this.damage = damage;
		this.dispersion = dispersion;
		this.rateOfFire = rateOfFire;
		this.reloadSpeed = reloadSpeed;
		this.ammunition = ammunition;
	}

	public WeaponData(string weaponName, string unusedLocalizationKey, float damage, float dispersion, float rateOfFire, float reloadSpeed, int ammunition, Sprite weaponImage = null)
	{
		this.weaponName = weaponName;
		this.weaponImage = weaponImage;
		this.damage = damage;
		this.dispersion = dispersion;
		this.rateOfFire = rateOfFire;
		this.reloadSpeed = reloadSpeed;
		this.ammunition = ammunition;
	}
}
