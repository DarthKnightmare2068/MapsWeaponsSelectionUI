using System.Collections.Generic;
using UnityEngine;

public class MapCardManager : MonoBehaviour
{
	[SerializeField] private GameObject mapCardPrefab;
	[SerializeField] private UIButtonSceneLoader sceneLoader;
	[SerializeField] private List<MapData> mapsData = new List<MapData>();
	
	[Header("Card Positioning")]
	[SerializeField] private Vector2 firstCardPosition = new Vector2(365f, -45f);
	[SerializeField] private float horizontalSpacing = 700f;

	private Transform cardContainer;
	private List<MapCardUI> instantiatedCards = new List<MapCardUI>();

	private void Start()
	{
		SetupCardContainer();
		GenerateMapCards();
	}
	
	private void SetupCardContainer()
	{
		// Use Canvas directly as container
		Canvas canvas = GetComponentInParent<Canvas>();
		if (canvas == null)
		{
			Debug.LogError("MapCardManager: Must be a child of Canvas!");
			return;
		}
		
		// Just use the Canvas itself as the container
		cardContainer = canvas.transform;
		
		Debug.Log($"Using Canvas '{canvas.name}' as card container");
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

		int cardIndex = 0;
		foreach (MapData mapData in mapsData)
		{
			if (mapData == null) continue;

			GameObject cardInstance = Instantiate(mapCardPrefab, cardContainer);
			MapCardUI cardUI = cardInstance.GetComponent<MapCardUI>();

			if (cardUI != null)
			{
				cardUI.Initialize(mapData, sceneLoader);
				instantiatedCards.Add(cardUI);
				
				// Position the card horizontally
				RectTransform rectTransform = cardInstance.GetComponent<RectTransform>();
				if (rectTransform != null)
				{
					float xPos = firstCardPosition.x + (cardIndex * horizontalSpacing);
					rectTransform.anchoredPosition = new Vector2(xPos, firstCardPosition.y);
					
					Debug.Log($"Card {cardIndex} positioned at: ({xPos}, {firstCardPosition.y})");
				}
				
				cardIndex++;
			}
			else
			{
				Debug.LogWarning($"MapCardManager: MapCardUI component not found on prefab '{mapCardPrefab.name}'");
			}
		}
	}

	public void AddMap(MapData mapData)
	{
		if (mapData == null) return;

		mapsData.Add(mapData);

		if (mapCardPrefab != null && cardContainer != null)
		{
			GameObject cardInstance = Instantiate(mapCardPrefab, cardContainer);
			MapCardUI cardUI = cardInstance.GetComponent<MapCardUI>();

			if (cardUI != null && sceneLoader != null)
			{
				cardUI.Initialize(mapData, sceneLoader);
				instantiatedCards.Add(cardUI);
				
				// Position the card horizontally
				RectTransform rectTransform = cardInstance.GetComponent<RectTransform>();
				if (rectTransform != null)
				{
					int cardIndex = instantiatedCards.Count - 1;
					float xPos = firstCardPosition.x + (cardIndex * horizontalSpacing);
					rectTransform.anchoredPosition = new Vector2(xPos, firstCardPosition.y);
				}
			}
		}
	}

	public void ClearCards()
	{
		foreach (MapCardUI card in instantiatedCards)
		{
			if (card != null)
			{
				Destroy(card.gameObject);
			}
		}
		instantiatedCards.Clear();
	}
	

	public void RefreshCards()
	{
		GenerateMapCards();
	}

	private void OnValidate()
	{
		if (mapsData.Count == 0)
		{
			mapsData.Add(new MapData("Test Map", "Map_1"));
		}
	}
}
