using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
	// Execute loader callback after first frame update
	private bool isFirstUpdate = true;

	private void Update()
	{
		if (isFirstUpdate)
		{
			isFirstUpdate = false;
			Loader.LoaderCallback();
		}
	}
}
