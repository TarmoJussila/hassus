using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour
    where T : MonoSingleton<T>
{
    public static T Instance = null;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this);
        }
        else
        {
            Instance = (T)this;
        }
    }
}
