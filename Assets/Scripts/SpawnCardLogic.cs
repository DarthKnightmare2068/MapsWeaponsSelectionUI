using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnCardLogic<TData, TCard> : MonoBehaviour where TCard : MonoBehaviour
{
	// Shared spawn logic for scroll views that display a list of cards.
	// This base class:
	// - Finds the scroll view Content transform
	// - Spawns card prefabs under that Content
	// - Tracks and clears spawned card instances
	// Concrete managers (maps, weapons, etc.) provide:
	// - The data list
	// - The prefab
	// - How to initialise a card from a piece of data
	[Header("Spawn Settings")]
	[SerializeField] private string contentPath = "Scroll View/Viewport/Content";

	protected static bool isQuitting = false;

	protected Transform cardContainer;
	protected readonly List<TCard> instantiatedCards = new List<TCard>();

	protected void SetupCardContainer(Canvas providedCanvas = null)
	{
		// Find the scroll view Content under the parent Canvas and cache it as cardContainer.
		// Call this once in Start() of the derived manager.
		// If a Canvas is provided, use it to avoid duplicate lookup.
		Canvas canvas;
		if (providedCanvas != null)
		{
			canvas = providedCanvas;
		}
		else
		{
			canvas = GetComponentInParent<Canvas>();
		}
		
		if (canvas == null)
		{
			Debug.LogError($"{GetType().Name}: Must be a child of Canvas!");
			return;
		}

		Transform content = canvas.transform.Find(contentPath);
		cardContainer = content != null ? content : canvas.transform;
	}

	protected void GenerateCards(IList<TData> dataList, GameObject cardPrefab)
	{
		// Spawn cards for all entries in the provided data list using the given prefab.
		// The derived manager can call this whenever its data list changes.
		ClearSpawnedCards();

		if (cardPrefab == null)
		{
			Debug.LogError($"{GetType().Name}: Card prefab is not assigned!");
			return;
		}

		if (cardContainer == null)
		{
			Debug.LogError($"{GetType().Name}: Card container is not set. Call SetupCardContainer() first.");
			return;
		}

		if (dataList == null) return;

		for (int i = 0; i < dataList.Count; i++)
		{
			TData data = dataList[i];
			if (!IsDataValid(data)) continue;

			SpawnSingleCard(cardPrefab, data, i);
		}
	}

	protected TCard SpawnSingleCard(GameObject cardPrefab, TData data, int index)
	{
		// Spawn a single card instance and initialise it.
		// Used by GenerateCards() and by derived managers when adding items at runtime.
		if (cardPrefab == null || cardContainer == null) return null;

		GameObject cardInstance = Instantiate(cardPrefab, cardContainer);
		TCard card = cardInstance.GetComponent<TCard>();

		if (card == null)
		{
			Debug.LogWarning($"{GetType().Name}: {typeof(TCard).Name} component not found on prefab '{cardPrefab.name}'");
			Destroy(cardInstance);
			return null;
		}

		InitializeCard(card, data, index);
		instantiatedCards.Add(card);
		return card;
	}

	protected void ClearSpawnedCards()
	{
		// Skip cleanup during application quit to prevent errors
		if (isQuitting) return;
		
		// Clear all spawned card instances.
		foreach (TCard card in instantiatedCards)
		{
			if (card != null)
			{
				Destroy(card.gameObject);
			}
		}
		instantiatedCards.Clear();
	}

	protected virtual bool IsDataValid(TData data)
	{
		// Override to specify what counts as "valid" data. Default: non-null reference.
		return !(data is null);
	}

	protected abstract void InitializeCard(TCard card, TData data, int index);

#if UNITY_EDITOR
	// OnValidate runs in editor to auto-generate test data when counts don't match
	protected virtual void OnValidate()
	{
		// Only run in editor, not during play mode
		if (Application.isPlaying) return;
		
		// Get the target count and current data list from derived class
		int targetCount = GetTargetDataCount();
		IList<TData> dataList = GetDataList();
		
		// Only auto-generate if count doesn't match
		if (dataList.Count != targetCount)
		{
			ValidateAndResizeDataList(dataList, targetCount);
		}
	}
	
	// Validates and resizes the data list to match target count, preserving existing data.
	// Common pattern: preserve existing data, clear list, loop and reuse/create items.
	protected void ValidateAndResizeDataList(IList<TData> dataList, int targetCount)
	{
		// Preserve existing data (with images) when possible
		List<TData> preservedData = new List<TData>(dataList);
		dataList.Clear();
		
		for (int i = 0; i < targetCount; i++)
		{
			// If we have preserved data for this index, reuse it (preserves images)
			if (i < preservedData.Count && preservedData[i] != null)
			{
				dataList.Add(preservedData[i]);
			}
			else
			{
				// Create new data only for new indices - derived class provides creation logic
				TData newData = CreateNewDataItem(i);
				dataList.Add(newData);
			}
		}
		
		// Allow derived class to perform post-generation operations
		OnDataListResized(dataList);
	}
	
	// Derived classes must provide the target count for auto-generation.
	protected abstract int GetTargetDataCount();
	
	// Derived classes must provide access to their data list.
	protected abstract IList<TData> GetDataList();
	
	// Derived classes implement this to create new data items when needed.
	protected abstract TData CreateNewDataItem(int index);
	
	// Called after data list is resized. Override to perform additional operations.
	protected virtual void OnDataListResized(IList<TData> dataList) { }
#endif

	private void OnApplicationQuit()
	{
		isQuitting = true;
	}
}

