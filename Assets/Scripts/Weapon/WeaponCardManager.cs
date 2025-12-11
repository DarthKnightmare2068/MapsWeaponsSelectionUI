using System.Collections.Generic;
using UnityEngine;

public class WeaponCardManager : SpawnCardLogic<WeaponData, WeaponCardSelection>
{
	[SerializeField] private GameObject weaponCardSelectionPrefab; // Prefab for selectable cards in scroll view
	[SerializeField] private List<WeaponData> weaponsData = new List<WeaponData>();
	[SerializeField] private GameObject weaponCardDisplayPrefab; // Prefab for big image panel (spawned in canvas)
	[SerializeField] private GameObject weaponCardDataPrefab; // Prefab for stats panel (spawned in canvas)

	[Header("Testing")]
	[SerializeField] private int numberOfWeapons = 5;
	[SerializeField] private int weaponMaxLv = 6; // Maximum level for all weapons (e.g., 6 means base level 1 + 5 upgrades, sets WeaponData.DefaultWeaponMaxLv)

	private Canvas canvas;
	private WeaponCardSelection currentSelection;
	private WeaponCardDisplay instantiatedDisplay; // Reference to spawned display
	private WeaponCardData instantiatedData; // Reference to spawned data panel

	private void Start()
	{
		// Find the Canvas used for weapon UI
		canvas = GetComponentInParent<Canvas>();
		if (canvas == null)
		{
			Debug.LogError("WeaponCardManager: Must be a child of Canvas!");
			return;
		}

		// Set the static default max level in WeaponData
		WeaponData.DefaultWeaponMaxLv = weaponMaxLv;

		// Set max level for all existing weapons
		SetMaxLevelForAllWeapons();

		// Use shared container-finding logic (Scroll View/Viewport/Content)
		// Pass the cached canvas to avoid duplicate lookup
		SetupCardContainer(canvas);

		SpawnDisplayAndData();
		GenerateWeaponCards();
	}

	private void SpawnDisplayAndData()
	{
		// Clean up existing instances first to prevent memory leaks
		if (instantiatedDisplay != null)
		{
			Destroy(instantiatedDisplay.gameObject);
			instantiatedDisplay = null;
		}
		if (instantiatedData != null)
		{
			Destroy(instantiatedData.gameObject);
			instantiatedData = null;
		}

		// Spawn weapon card display in canvas (respects prefab position)
		if (weaponCardDisplayPrefab != null)
		{
			GameObject displayInstance = Instantiate(weaponCardDisplayPrefab, canvas.transform);
			// Keep prefab's position by not modifying RectTransform
			instantiatedDisplay = displayInstance.GetComponentInChildren<WeaponCardDisplay>();
			if (instantiatedDisplay == null)
			{
				Debug.LogWarning("WeaponCardManager: WeaponCardDisplay component not found on weaponCardDisplayPrefab or its children. Make sure the prefab has the WeaponCardDisplay script component attached!");
			}
		}
		else
		{
			Debug.LogWarning("WeaponCardManager: weaponCardDisplayPrefab or canvas is not assigned!");
		}

		// Spawn weapon card data in canvas (respects prefab position)
		if (weaponCardDataPrefab != null)
		{
			GameObject dataInstance = Instantiate(weaponCardDataPrefab, canvas.transform);
			// Keep prefab's position by not modifying RectTransform
			instantiatedData = dataInstance.GetComponentInChildren<WeaponCardData>();
			if (instantiatedData == null)
			{
				Debug.LogWarning("WeaponCardManager: WeaponCardData component not found on weaponCardDataPrefab or its children. Make sure the prefab has the WeaponCardData script component attached!");
			}
		}
		else
		{
			Debug.LogWarning("WeaponCardManager: weaponCardDataPrefab or canvas is not assigned!");
		}
	}

	public void GenerateWeaponCards()
	{
		// Use shared spawn logic
		GenerateCards(weaponsData, weaponCardSelectionPrefab);

		// Select first weapon by default
		if (instantiatedCards.Count > 0)
		{
			OnWeaponCardSelected(instantiatedCards[0]);
		}
	}

	protected override void InitializeCard(WeaponCardSelection card, WeaponData data, int index)
	{
		card.Initialize(data, this);
	}

	public void AddWeapon(WeaponData weaponData)
	{
		// Dynamically add a new weapon card at runtime
		if (weaponData == null) return;

		// Set max level for the new weapon
		weaponData.SetMaxLevel(WeaponData.DefaultWeaponMaxLv);

		weaponsData.Add(weaponData);
		WeaponCardSelection newCard = SpawnSingleCard(weaponCardSelectionPrefab, weaponData, weaponsData.Count - 1);
		if (newCard == null)
		{
			Debug.LogWarning($"WeaponCardManager: Failed to spawn card for weapon '{weaponData.WeaponName}'. Rolling back data addition.");
			weaponsData.RemoveAt(weaponsData.Count - 1); // Rollback if spawn failed
		}
	}

	// Set max level for all weapons in the list using the static default value
	private void SetMaxLevelForAllWeapons()
	{
		foreach (WeaponData weapon in weaponsData)
		{
			weapon.SetMaxLevel(WeaponData.DefaultWeaponMaxLv);
		}
	}

	public void ClearCards()
	{
		ClearSpawnedCards();
	}


	private void OnDestroy()
	{
		if (isQuitting)
		{
			// Skip cleanup during application quit
			return;
		}

		// Clean up instantiated display and data objects
		// FIXED: Added null checks for gameObject and clear references to prevent memory leaks
		if (instantiatedDisplay != null && instantiatedDisplay.gameObject != null)
		{
			Destroy(instantiatedDisplay.gameObject);
			instantiatedDisplay = null;
		}
		if (instantiatedData != null && instantiatedData.gameObject != null)
		{
			Destroy(instantiatedData.gameObject);
			instantiatedData = null;
		}
	}

	public void RefreshCards()
	{
		// Regenerate all cards from Weapons Data
		GenerateWeaponCards();
	}

#if UNITY_EDITOR
	protected override void OnValidate()
	{
		// Set the static default max level in WeaponData
		WeaponData.DefaultWeaponMaxLv = weaponMaxLv;
		
		// Set max level for all weapons when weaponMaxLv changes
		SetMaxLevelForAllWeapons();
		
		// Call base class validation (handles auto-generation)
		base.OnValidate();
	}
	
	protected override int GetTargetDataCount()
	{
		return numberOfWeapons;
	}
	
	protected override IList<WeaponData> GetDataList()
	{
		return weaponsData;
	}
	
	protected override WeaponData CreateNewDataItem(int index)
	{
		// Create new WeaponData only for new indices
		// Default values set to 0 - configure stats in Unity Inspector
		WeaponData newWeapon = new WeaponData(
			$"Weapon {index + 1}",
			damage: 0,
			dispersion: 0,
			rateOfFire: 0f,
			reloadSpeed: 0,
			ammunition: 0
		);
		newWeapon.SetMaxLevel(WeaponData.DefaultWeaponMaxLv);
		return newWeapon;
	}
	
	protected override void OnDataListResized(IList<WeaponData> dataList)
	{
		// Set max level for all weapons after generation
		SetMaxLevelForAllWeapons();
	}
#endif

	// Called by WeaponCardSelection when a card is clicked
	public void OnWeaponCardSelected(WeaponCardSelection card)
	{
		if (card == null) return;

		// Deselect previous card (set to normal frame)
		if (currentSelection != null && currentSelection != card)
		{
			currentSelection.SetSelected(false);
		}

		// Update current selection reference
		currentSelection = card;

		// Select new card (set to glowing frame)
		currentSelection.SetSelected(true);

		WeaponData data = currentSelection.WeaponData;

		// Update big display and stats panel using instantiated objects
		if (instantiatedDisplay != null)
		{
			instantiatedDisplay.SetWeapon(data, card);
		}

		if (instantiatedData != null)
		{
			instantiatedData.SetWeapon(data);
		}
	}
}
