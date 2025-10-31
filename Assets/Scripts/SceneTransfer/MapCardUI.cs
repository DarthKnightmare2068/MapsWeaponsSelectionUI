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
		if (mapButton == null)
		{
			mapButton = GetComponent<Button>();
		}
	}

	public void Initialize(MapData data, UIButtonSceneLoader loader)
	{
		mapData = data;
		sceneLoader = loader;

		if (mapNameText != null)
		{
			mapNameText.text = mapData.MapName;
		}

		if (mapImage != null && mapData.MapImage != null)
		{
			mapImage.sprite = mapData.MapImage;
			mapImage.gameObject.SetActive(true);
		}

		if (mapButton != null)
		{
			mapButton.interactable = !mapData.IsLocked;
			mapButton.onClick.RemoveAllListeners();
			mapButton.onClick.AddListener(OnMapCardClicked);
		}
	}

	private void OnMapCardClicked()
	{
		if (mapData != null && sceneLoader != null)
		{
			sceneLoader.LoadSceneByName(mapData.SceneName);
		}
	}

	public void UpdateCard(MapData data)
	{
		Initialize(data, sceneLoader);
	}
}
