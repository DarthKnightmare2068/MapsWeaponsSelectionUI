using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapCardUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI mapNameText;
	[SerializeField] private Button mapButton;
	[SerializeField] private Image mapImage;

	private MapData mapData;
	private UIButtonSceneLoader sceneLoader;

	private void Awake()
	{
		// Auto-find Button component if not assigned
		if (mapButton == null)
		{
			mapButton = GetComponent<Button>();
		}
	}

	public void Initialize(MapData data, UIButtonSceneLoader loader)
	{
		mapData = data;
		sceneLoader = loader;

		// Set map name text
		if (mapNameText != null)
		{
			mapNameText.text = mapData.MapName;
		}

		// Set map image if available
		if (mapImage != null && mapData.MapImage != null)
		{
			mapImage.sprite = mapData.MapImage;
			mapImage.gameObject.SetActive(true);
		}

		// Setup button (locked cards are non-interactable)
		if (mapButton != null)
		{
			mapButton.interactable = !mapData.IsLocked;
			mapButton.onClick.RemoveAllListeners();
			mapButton.onClick.AddListener(OnMapCardClicked);
		}
	}

	private void OnMapCardClicked()
	{
		// Load the scene associated with this map card
		if (mapData != null && sceneLoader != null)
		{
			sceneLoader.LoadSceneByName(mapData.SceneName);
		}
	}

	public void UpdateCard(MapData data)
	{
		// Update card with new map data
		Initialize(data, sceneLoader);
	}
}
