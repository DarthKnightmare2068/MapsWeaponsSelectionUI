using UnityEngine;

[System.Serializable]
public class MapData
{
	[SerializeField] private string mapName;
	[SerializeField] private string sceneName;
	[SerializeField] private Sprite mapImage;
	[SerializeField] private bool isLocked;

	public string MapName => mapName;
	public string SceneName => sceneName;
	public Sprite MapImage => mapImage;
	public bool IsLocked => isLocked;

	public MapData(string mapName, string sceneName, Sprite mapImage = null, bool isLocked = false)
	{
		this.mapName = mapName;
		this.sceneName = sceneName;
		this.mapImage = mapImage;
		this.isLocked = isLocked;
	}
}
