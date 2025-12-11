using UnityEngine;

// Utility extension methods for finding and adding components.
// Eliminates repeated component-finding patterns throughout the codebase.
public static class ComponentExtensions
{
    // Gets a component from this GameObject, or from its parent if not found.
    // This is a common pattern used to find components in the hierarchy.
    public static T GetComponentOrInParent<T>(this Component component) where T : Component
    {
        if (component == null) return null;

        // Try to get the component from this GameObject first
        T result = component.GetComponent<T>();

        // If not found, try to get from parent
        if (result == null)
        {
            result = component.GetComponentInParent<T>();
        }

        return result;
    }

    // Gets a component from the GameObject, or adds it if it doesn't exist.
    // Useful for ensuring a component exists without manually checking.
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject == null) return null;

        T component = gameObject.GetComponent<T>();

        // Add the component if it doesn't exist
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }
}
