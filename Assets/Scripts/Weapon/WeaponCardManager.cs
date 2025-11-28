using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCardManager : MonoBehaviour
{
	[SerializeField] private GameObject weaponCardPrefab;
	[SerializeField] private List<WeaponData> weaponsData = new List<WeaponData>();
	[SerializeField] private WeaponCardDisplay weaponCardDisplay; // big image panel
	[SerializeField] private WeaponCardData weaponCardData;       // stats panel

	[Header("Layout Settings")]
	[SerializeField] private float cardSpacing = 90f;
	[SerializeField] private float leftPadding = -275f;
	[SerializeField] private float rightPadding = 120f;
	
	[Header("Testing")]
	[SerializeField] private int numberOfWeapons = 5;

	private Transform cardContainer;
	private List<WeaponCardSelection> instantiatedCards = new List<WeaponCardSelection>();
	private WeaponCardSelection currentSelection;

	private void Start()
	{
		SetupCardContainer();
		GenerateWeaponCards();
	}
	
	private void SetupCardContainer()
	{
		// Find the Content object inside Scroll View
		Canvas canvas = GetComponentInParent<Canvas>();
		if (canvas == null)
		{
			Debug.LogError("WeaponCardManager: Must be a child of Canvas!");
			return;
		}
		
		Transform content = canvas.transform.Find("Scroll View/Viewport/Content");
		cardContainer = content != null ? content : canvas.transform;
	}
	
	private void UpdateContentSize()
	{
		// Update Content width based on number of cards and card width
		if (cardContainer == null || instantiatedCards.Count == 0) return;
		
		RectTransform contentRect = cardContainer.GetComponent<RectTransform>();
		if (contentRect == null || weaponCardPrefab == null) return;
		
		// Calculate: left padding + cards + spacing + last card width + right padding
		// Last card X position: leftPadding + (count - 1) * cardSpacing
		// Last card right edge: last card X + cardWidth
		// Total width: last card right edge + rightPadding
		float cardWidth = GetCardWidth();
		float lastCardX = leftPadding + ((instantiatedCards.Count - 1) * cardSpacing);
		float totalWidth = lastCardX + cardWidth + rightPadding;
		
		contentRect.sizeDelta = new Vector2(totalWidth, contentRect.sizeDelta.y);
	}
	
	private float GetCardWidth()
	{
		// Get actual card width from first spawned card (accounts for any scaling)
		if (instantiatedCards.Count > 0 && instantiatedCards[0] != null)
		{
			RectTransform rect = instantiatedCards[0].GetComponent<RectTransform>();
			if (rect != null) return rect.rect.width;
		}
		
		// Fallback to prefab width
		if (weaponCardPrefab != null)
		{
			RectTransform prefabRect = weaponCardPrefab.GetComponent<RectTransform>();
			if (prefabRect != null) return prefabRect.rect.width;
		}
		
		return 252f; // Default fallback width
	}

	public void GenerateWeaponCards()
	{
		ClearCards();

		if (weaponCardPrefab == null)
		{
			Debug.LogError("WeaponCardManager: Weapon Card Prefab is not assigned!");
			return;
		}

		// Spawn all cards from Weapons Data list
		for (int i = 0; i < weaponsData.Count; i++)
		{
			if (weaponsData[i] == null) continue;
			SpawnCard(weaponsData[i], i);
		}

		// Select first weapon by default
		if (instantiatedCards.Count > 0)
		{
			OnWeaponCardSelected(instantiatedCards[0]);
		}
		
		// Update Content size for scrolling
		UpdateContentSize();
	}
	
	private void SpawnCard(WeaponData weaponData, int index)
	{
		// Instantiate card prefab inside Content container
		GameObject cardInstance = Instantiate(weaponCardPrefab, cardContainer);

		WeaponCardSelection card = cardInstance.GetComponent<WeaponCardSelection>();
		if (card == null)
		{
			Debug.LogWarning($"WeaponCardManager: WeaponCardSelection component not found on prefab '{weaponCardPrefab.name}'");
			Destroy(cardInstance);
			return;
		}

		card.Initialize(weaponData, this);

		// Add card to instantiated cards list
		instantiatedCards.Add(card);
		
		// Position card horizontally (first card at middle-left of Content)
		RectTransform rectTransform = cardInstance.GetComponent<RectTransform>();
		if (rectTransform != null)
		{
			float xPos = leftPadding + (index * cardSpacing);
			rectTransform.anchoredPosition = new Vector2(xPos, 0f); // Y = 0 for vertical center
		}
	}

	public void AddWeapon(WeaponData weaponData)
	{
		// Dynamically add a new weapon card at runtime
		if (weaponData == null || weaponCardPrefab == null || cardContainer == null) return;

		weaponsData.Add(weaponData);
		SpawnCard(weaponData, weaponsData.Count - 1);
		UpdateContentSize();
	}

	public void ClearCards()
	{
		// Remove all spawned card instances
		foreach (WeaponCardSelection card in instantiatedCards)
		{
			if (card != null) Destroy(card.gameObject);
		}
		instantiatedCards.Clear();
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
					// Default values for testing
					weaponsData.Add(new WeaponData(
						$"Weapon {i + 1}",
						damage: 10f + (i * 5f),
						dispersion: 1f + (i * 0.5f),
						rateOfFire: 5f + (i * 2f),
						reloadSpeed: 2f - (i * 0.2f),
						ammunition: 30 + (i * 10)
					));
				}
			}
		}
	}

	// Called by WeaponCardSelection when a card is clicked
	public void OnWeaponCardSelected(WeaponCardSelection card)
	{
		if (card == null) return;

		// Update selection state visuals
		if (currentSelection != null && currentSelection != card)
		{
			currentSelection.SetSelected(false);
		}

		currentSelection = card;
		currentSelection.SetSelected(true);

		WeaponData data = currentSelection.WeaponData;

		// Update big display and stats panel
		if (weaponCardDisplay != null)
		{
			weaponCardDisplay.SetWeapon(data);
		}

		if (weaponCardData != null)
		{
			weaponCardData.SetWeapon(data);
		}
	}
}
