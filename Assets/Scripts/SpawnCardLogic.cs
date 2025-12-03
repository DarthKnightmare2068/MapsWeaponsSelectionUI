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

	protected Transform cardContainer;
	protected readonly List<TCard> instantiatedCards = new List<TCard>();

	protected void SetupCardContainer()
	{
		// Find the scroll view Content under the parent Canvas and cache it as cardContainer.
		// Call this once in Start() of the derived manager.
		Canvas canvas = GetComponentInParent<Canvas>();
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

		InitialiseCard(card, data, index);
		instantiatedCards.Add(card);
		return card;
	}

	protected void ClearSpawnedCards()
	{
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

	protected abstract void InitialiseCard(TCard card, TData data, int index);
}

