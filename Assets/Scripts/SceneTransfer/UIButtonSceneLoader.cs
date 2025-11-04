using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonSceneLoader : MonoBehaviour
{
	// Load specific scenes (for button OnClick events)
	public void LoadMenu()
	{
		SceneManager.LoadScene("Menu");
	}

	public void LoadMapMenu()
	{
		SceneManager.LoadScene("MapMenu");
	}

	public void LoadMap1()
	{
		SceneManager.LoadScene("Map_1");
	}

	public void LoadMap2()
	{
		SceneManager.LoadScene("Map_2");
	}

	public void LoadMap3()
	{
		SceneManager.LoadScene("Map_3");
	}

	// Generic method to load any scene by name
	public void LoadSceneByName(string sceneName)
	{
		if (!string.IsNullOrEmpty(sceneName))
		{
			SceneManager.LoadScene(sceneName);
		}
	}
}
