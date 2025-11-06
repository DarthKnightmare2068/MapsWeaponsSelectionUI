using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
	private static Action onLoaderCallback;

	public enum Scene
	{
		Menu,
		MapMenu
	}

	// Load scene with callback system (for loading screens)
	public static void Load(Scene scene)
	{
		// Set the Loader callback action to load the target scene
		onLoaderCallback = () =>
		{
			SceneManager.LoadScene(scene.ToString());
		};
	}

	// Called after first frame update to refresh scene
	public static void LoaderCallback()
	{
		// Execute loader callback action to load the target scene
		if (onLoaderCallback != null)
		{
			onLoaderCallback();
			onLoaderCallback = null;
		}
	}
}
