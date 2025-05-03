using UnityEngine;

public static class TransformUtil
{
    public static T GetOrAdd<T>(this MonoBehaviour monoBehaviour) where T : Component
    {
        return monoBehaviour.transform.GetOrAdd<T>();
    }

    public static T GetOrAdd<T>(this Transform transform) where T : Component
    {
        T component = transform.GetComponent<T>();
        if (component == null)
        {
            component = transform.gameObject.AddComponent<T>();
        }

        return component;
    }
}
