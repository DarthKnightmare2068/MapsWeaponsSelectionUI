using UnityEngine;

[System.Serializable]
public class MapData
{
	[SerializeField] private string mapName;
	[SerializeField] private string localizationKey; // Key for localization table (e.g., "Dungeon Explorer")
	[SerializeField] private string sceneName;
	[SerializeField] private Sprite mapImage;
	[SerializeField] private bool isLocked;

	public string MapName => mapName;
	public string LocalizationKey => string.IsNullOrEmpty(localizationKey) ? mapName : localizationKey;
	public string SceneName => sceneName;
	public Sprite MapImage => mapImage;
	public bool IsLocked => isLocked;

	public MapData(string mapName, string sceneName, Sprite mapImage = null, bool isLocked = false)
	{
		this.mapName = mapName;
		this.localizationKey = mapName; // Use mapName as default localization key
		this.sceneName = sceneName;
		this.mapImage = mapImage;
		this.isLocked = isLocked;
	}

	public MapData(string mapName, string localizationKey, string sceneName, Sprite mapImage = null, bool isLocked = false)
	{
		this.mapName = mapName;
		this.localizationKey = string.IsNullOrEmpty(localizationKey) ? mapName : localizationKey;
		this.sceneName = sceneName;
		this.mapImage = mapImage;
		this.isLocked = isLocked;
	}
}
