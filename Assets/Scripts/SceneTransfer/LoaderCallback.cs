using UnityEngine;

public class LoaderCallback : MonoBehaviour
{// Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool isFirstUpdate =  true;

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            Loader.LoaderCallback();
        }
    }
}
