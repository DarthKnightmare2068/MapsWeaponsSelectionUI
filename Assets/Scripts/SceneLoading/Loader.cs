using System;
using UnityEngine;

// TODO: This class appears to be incomplete or unused.
// The onLoaderCallback is never set anywhere in the codebase.
// Either implement SetLoaderCallback method or remove this class if not needed.
// This pattern is typically used for async scene loading with a loading screen.
public static class Loader
{
	private static Action onLoaderCallback;

	// Sets the callback to be executed after the loading scene initializes
	// Call this before loading a transition/loading scene
	public static void SetLoaderCallback(Action callback)
	{
		onLoaderCallback = callback;
	}

	// Called after first frame update to refresh scene (typically from LoaderCallback MonoBehaviour)
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
