using System.Collections.Generic;
using UnityEngine;

public static class ResourcesDictionary
{
    private static Dictionary<string, Object> resourceCache = new ();

    public static T Load<T>(string path) where T : Object
    {
        if (resourceCache.ContainsKey(path))
        {
            // Return cached resource if available
            return resourceCache[path] as T;
        }
        else
        {
            // Load and cache the resource
            T loadedResource = Resources.Load<T>(path);

            if (loadedResource != null)
            {
                resourceCache.Add(path, loadedResource);
            }
            else
            {
                Debug.LogError($"Failed to load resource at path: {path}");
            }

            return loadedResource;
        }
    }

    public static void ClearCache()
    {
        resourceCache.Clear();
    }
}