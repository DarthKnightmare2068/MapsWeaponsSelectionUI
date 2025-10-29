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


    public static void Load(Scene scene)
    {   
        // Set the Loader callback action to load the target scene
       onLoaderCallback = () =>
       {
        SceneManager.LoadScene(scene.ToString());
       };
    }

    public static void LoaderCallback()
    {
        // Triggered after the 1st update lets to refresh scene
        // Excececute loader callback action to load the target scene
       if (onLoaderCallback != null)
       {
            onLoaderCallback();
            onLoaderCallback = null;
       }
    }
}
