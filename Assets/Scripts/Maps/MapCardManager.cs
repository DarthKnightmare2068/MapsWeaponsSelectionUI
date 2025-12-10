using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCardManager : SpawnCardLogic<MapData, MapCardUI>
{
	[SerializeField] private GameObject mapCardPrefab;
	[SerializeField] private SceneLoader sceneLoader;
	[SerializeField] private List<MapData> mapsData = new List<MapData>();
	[SerializeField] private ChooseLevelWarning warning; // Reference to warning object for dependency injection

	[Header("Lock Settings")]
	public float lockedCardAlpha = 0.3f; // Alpha value for locked cards (0.3 = 30% opacity)
	
	[Header("Testing")]
	[SerializeField] private int numberOfMaps = 3;

	private void Start()
	{
		SetupCardContainer();
		
		// Find warning object once (for dependency injection to all cards)
		// This is a single search instead of each card searching individually
		if (warning == null)
		{
			warning = FindFirstObjectByType<ChooseLevelWarning>();
			if (warning == null)
			{
				Debug.LogWarning("MapCardManager: ChooseLevelWarning not found in scene. Cards will not be able to show level selection warnings.");
			}
		}
		
		GenerateMapCards();
	}
	
	public void GenerateMapCards()
	{
		if (sceneLoader == null)
		{
			sceneLoader = FindFirstObjectByType<SceneLoader>();
			if (sceneLoader == null)
			{
				Debug.LogError("MapCardManager: SceneLoader not found in scene!");
				return;
			}
		}

		// Use shared spawn logic
		GenerateCards(mapsData, mapCardPrefab);
	}
	
	protected override void InitializeCard(MapCardUI card, MapData data, int index)
	{
		// Initialize card with map data and inject warning reference
		// Dependency injection: Pass the warning reference so each card doesn't need to search for it
		// This improves performance by eliminating repeated FindObjectsByType calls
		card.Initialize(data, sceneLoader, lockedCardAlpha, warning);
	}

	public void AddMap(MapData mapData)
	{
		// Dynamically add a new map card at runtime
		if (mapData == null) return;

		mapsData.Add(mapData);
		SpawnSingleCard(mapCardPrefab, mapData, mapsData.Count - 1);
		
		// Invalidate cache in ChooseLevelWarning since card count changed
		if (warning != null)
		{
			warning.InvalidateMapCardsCache();
		}
	}

	public void ClearCards()
	{
		ClearSpawnedCards();
		
		// Invalidate cache in ChooseLevelWarning since cards were cleared
		if (warning != null)
		{
			warning.InvalidateMapCardsCache();
		}
	}

	public void RefreshCards()
	{
		// Regenerate all cards from Maps Data
		GenerateMapCards();
		
		// Invalidate cache in ChooseLevelWarning since cards were regenerated
		if (warning != null)
		{
			warning.InvalidateMapCardsCache();
		}
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
