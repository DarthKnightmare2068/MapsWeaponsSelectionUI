using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCardManager : SpawnCardLogic<MapData, MapCardUI>
{
	[SerializeField] private GameObject mapCardPrefab;
	[SerializeField]
	[Tooltip("Assign via Inspector for best performance. Searches scene as fallback.")]
	private SceneLoader sceneLoader;
	[SerializeField] private List<MapData> mapsData = new List<MapData>();
	[SerializeField]
	[Tooltip("Assign via Inspector for best performance. Searches scene as fallback.")]
	private ChooseLevelWarning warning; // Reference to warning object for dependency injection

	[Header("Lock Settings")]
	public float lockedCardAlpha = 0.3f; // Alpha value for locked cards (0.3 = 30% opacity)

	[Header("Testing")]
	[SerializeField] private int numberOfMaps = 3;

	private void Start()
	{
		SetupCardContainer();

		// Find warning object once (for dependency injection to all cards)
		// This is a single search instead of each card searching individually
		// FIXED: Added warning log for expensive FindFirstObjectByType fallback
		if (warning == null)
		{
			Debug.LogWarning("MapCardManager: ChooseLevelWarning not assigned in Inspector. Using expensive FindFirstObjectByType as fallback. Please assign in Inspector for better performance.");
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
			Debug.LogWarning("MapCardManager: SceneLoader not assigned in Inspector. Using expensive FindFirstObjectByType as fallback. Please assign in Inspector for better performance.");
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
		MapCardUI newCard = SpawnSingleCard(mapCardPrefab, mapData, mapsData.Count - 1);
		if (newCard == null)
		{
			Debug.LogWarning($"MapCardManager: Failed to spawn card for map '{mapData.MapName}'. Rolling back data addition.");
			mapsData.RemoveAt(mapsData.Count - 1); // Rollback if spawn failed
			return;
		}

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

#if UNITY_EDITOR
	protected override void OnValidate()
	{
		// Call base class validation (handles auto-generation)
		base.OnValidate();
	}
	
	protected override int GetTargetDataCount()
	{
		return numberOfMaps;
	}
	
	protected override IList<MapData> GetDataList()
	{
		return mapsData;
	}
	
	protected override MapData CreateNewDataItem(int index)
	{
		// Create new MapData only for new indices
		// Default localization keys based on index (match Unity localization table keys)
		string[] localizationKeys = { "mapCard.DungeonExplorer", "mapCard.TrapCave", "mapCard.ShutterIsland" };
		string localizationKey = index < localizationKeys.Length ? localizationKeys[index] : $"Map_{index + 1}";
		return new MapData($"Map {index + 1}", localizationKey, $"Map_{index + 1}");
	}
#endif
}
