using System;
using UnityEngine;

public static class Loader
{
	private static Action onLoaderCallback;

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
