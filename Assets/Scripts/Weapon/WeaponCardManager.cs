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

		// Use shared container-finding logic (Scroll View/Viewport/Content)
		SetupCardContainer();

		SpawnDisplayAndData();
		GenerateWeaponCards();
	}

	private void SpawnDisplayAndData()
	{
		// Spawn weapon card display in canvas (respects prefab position)
		if (weaponCardDisplayPrefab != null && canvas != null)
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
		if (weaponCardDataPrefab != null && canvas != null)
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

	protected override void InitialiseCard(WeaponCardSelection card, WeaponData data, int index)
	{
		card.Initialize(data, this);
	}

	public void AddWeapon(WeaponData weaponData)
	{
		// Dynamically add a new weapon card at runtime
		if (weaponData == null) return;

		weaponsData.Add(weaponData);
		SpawnSingleCard(weaponCardSelectionPrefab, weaponData, weaponsData.Count - 1);
	}

	public void ClearCards()
	{
		ClearSpawnedCards();
	}

	private void OnDestroy()
	{
		// Clean up instantiated display and data objects
		if (instantiatedDisplay != null)
		{
			Destroy(instantiatedDisplay.gameObject);
		}
		if (instantiatedData != null)
		{
			Destroy(instantiatedData.gameObject);
		}
	}

	public void RefreshCards()
	{
		// Regenerate all cards from Weapons Data
		GenerateWeaponCards();
	}

	private void OnValidate()
	{
		// Auto-generate test weapons based on numberOfWeapons, preserving existing images
		if (weaponsData.Count != numberOfWeapons)
		{
			// Preserve existing WeaponData (with images) when possible
			List<WeaponData> preservedData = new List<WeaponData>(weaponsData);
			weaponsData.Clear();
			
			for (int i = 0; i < numberOfWeapons; i++)
			{
				// If we have preserved data for this index, reuse it (preserves images)
				if (i < preservedData.Count && preservedData[i] != null)
				{
					weaponsData.Add(preservedData[i]);
				}
				else
				{
					// Create new WeaponData only for new indices
					// Default values set to 0 - configure stats in Unity Inspector
					weaponsData.Add(new WeaponData(
						$"Weapon {i + 1}",
						damage: 0,
						dispersion: 0,
						rateOfFire: 0f,
						reloadSpeed: 0,
						ammunition: 0
					));
				}
			}
		}
	}

	// Called by WeaponCardSelection when a card is clicked
	public void OnWeaponCardSelected(WeaponCardSelection card)
	{
		if (card == null) return;

		// Update current selection reference (no visual highlight)
		currentSelection = card;

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
