using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Attach this to each weapon option card prefab in the horizontal list.
// It represents a single selectable weapon.
public class WeaponCardSelection : MonoBehaviour
{
	[SerializeField] private Image weaponImage;
	[SerializeField] private TextMeshProUGUI weaponNameText;
	[SerializeField] private Button selectButton;
	[SerializeField] private GameObject selectedHighlight;

	private WeaponData weaponData;
	private WeaponCardManager manager;
	private bool isSelected;

	public WeaponData WeaponData => weaponData;

	private void Awake()
	{
		if (selectButton == null)
		{
			selectButton = GetComponent<Button>();
		}

		if (selectButton != null)
		{
			selectButton.onClick.AddListener(OnClick);
		}

		UpdateVisualSelection(false);
	}

	private void OnDestroy()
	{
		if (selectButton != null)
		{
			selectButton.onClick.RemoveListener(OnClick);
		}
	}

	public void Initialize(WeaponData data, WeaponCardManager owner)
	{
		weaponData = data;
		manager = owner;

		if (weaponImage != null)
		{
			weaponImage.sprite = data != null ? data.WeaponImage : null;
			weaponImage.enabled = weaponImage.sprite != null;
		}

		if (weaponNameText != null)
		{
			weaponNameText.text = data != null ? data.WeaponName : string.Empty;
		}

		UpdateVisualSelection(false);
	}

	private void OnClick()
	{
		if (manager != null)
		{
			manager.OnWeaponCardSelected(this);
		}
	}

	public void SetSelected(bool selected)
	{
		isSelected = selected;
		UpdateVisualSelection(isSelected);
	}

	private void UpdateVisualSelection(bool selected)
	{
		if (selectedHighlight != null)
		{
			selectedHighlight.SetActive(selected);
		}
	}
}
