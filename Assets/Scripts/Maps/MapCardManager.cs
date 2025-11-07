using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCardManager : MonoBehaviour
{
	[SerializeField] private GameObject mapCardPrefab;
	[SerializeField] private UIButtonSceneLoader sceneLoader;
	[SerializeField] private List<MapData> mapsData = new List<MapData>();

	[Header("Layout Settings")]
	[SerializeField] private float cardSpacing = 700f;
	[SerializeField] private float leftPadding = 375f;
	[SerializeField] private float rightPadding = 120f;
	
	[Header("Lock Settings")]
	public float lockedCardAlpha = 0.3f; // Alpha value for locked cards (0.3 = 30% opacity)
	
	[Header("Testing")]
	[SerializeField] private int numberOfMaps = 3;

	private Transform cardContainer;
	private List<MapCardUI> instantiatedCards = new List<MapCardUI>();

	private void Start()
	{
		SetupCardContainer();
		GenerateMapCards();
	}
	
	private void SetupCardContainer()
	{
		// Find the Content object inside Scroll View
		Canvas canvas = GetComponentInParent<Canvas>();
		if (canvas == null)
		{
			Debug.LogError("MapCardManager: Must be a child of Canvas!");
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
		if (contentRect == null || mapCardPrefab == null) return;
		
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
		if (mapCardPrefab != null)
		{
			RectTransform prefabRect = mapCardPrefab.GetComponent<RectTransform>();
			if (prefabRect != null) return prefabRect.rect.width;
		}
		
		return 252f; // Default fallback width
	}	

	public void GenerateMapCards()
	{
		ClearCards();

		if (mapCardPrefab == null)
		{
			Debug.LogError("MapCardManager: Map Card Prefab is not assigned!");
			return;
		}

		if (sceneLoader == null)
		{
			sceneLoader = FindFirstObjectByType<UIButtonSceneLoader>();
			if (sceneLoader == null)
			{
				Debug.LogError("MapCardManager: UIButtonSceneLoader not found in scene!");
				return;
			}
		}

		// Spawn all cards from Maps Data list
		for (int i = 0; i < mapsData.Count; i++)
		{
			if (mapsData[i] == null) continue;
			SpawnCard(mapsData[i], i);
		}
		
		// Update Content size for scrolling
		UpdateContentSize();
	}
	
	private void SpawnCard(MapData mapData, int index)
	{
		// Instantiate card prefab inside Content container
		GameObject cardInstance = Instantiate(mapCardPrefab, cardContainer);
		MapCardUI cardUI = cardInstance.GetComponent<MapCardUI>();

		if (cardUI == null)
		{
			Debug.LogWarning($"MapCardManager: MapCardUI component not found on prefab '{mapCardPrefab.name}'");
			Destroy(cardInstance);
			return;
		}

		// Initialize card with map data
		cardUI.Initialize(mapData, sceneLoader, lockedCardAlpha);
		instantiatedCards.Add(cardUI);
		
		// Position card horizontally (first card at middle-left of Content)
		RectTransform rectTransform = cardInstance.GetComponent<RectTransform>();
		if (rectTransform != null)
		{
			float xPos = leftPadding + (index * cardSpacing);
			rectTransform.anchoredPosition = new Vector2(xPos, 0f); // Y = 0 for vertical center
		}
	}

	public void AddMap(MapData mapData)
	{
		// Dynamically add a new map card at runtime
		if (mapData == null || mapCardPrefab == null || cardContainer == null) return;

		mapsData.Add(mapData);
		SpawnCard(mapData, mapsData.Count - 1);
		UpdateContentSize();
	}

	public void ClearCards()
	{
		// Remove all spawned card instances
		foreach (MapCardUI card in instantiatedCards)
		{
			if (card != null) Destroy(card.gameObject);
		}
		instantiatedCards.Clear();
	}

	public void RefreshCards()
	{
		// Regenerate all cards from Maps Data
		GenerateMapCards();
	}

	private void OnValidate()
	{
		// Auto-generate test maps based on numberOfMaps, preserving existing images
		if (mapsData.Count != numberOfMaps)
		{
			// Preserve existing MapData (with images) when possible
			List<MapData> preservedData = new List<MapData>(mapsData);
			mapsData.Clear();
			
			for (int i = 0; i < numberOfMaps; i++)
			{
				// If we have preserved data for this index, reuse it (preserves images)
				if (i < preservedData.Count && preservedData[i] != null)
				{
					mapsData.Add(preservedData[i]);
				}
				else
				{
					// Create new MapData only for new indices
					// Default localization keys based on index (match Unity localization table keys)
					string[] localizationKeys = { "mapCard.DungeonExplorer", "mapCard.TrapCave", "mapCard.ShutterIsland" };
					string localizationKey = i < localizationKeys.Length ? localizationKeys[i] : $"Map_{i + 1}";
					mapsData.Add(new MapData($"Map {i + 1}", localizationKey, $"Map_{i + 1}"));
				}
			}
		}
	}
}
