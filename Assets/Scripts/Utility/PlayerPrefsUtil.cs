using UnityEngine;

public partial class PlayerPrefsUtil
{
    public static void SaveVector(string name, Vector3 vector)
    {
        PlayerPrefs.SetFloat(name + "x", vector.x);
        PlayerPrefs.SetFloat(name + "y", vector.y);
        PlayerPrefs.SetFloat(name + "z", vector.z);
    }

    public static bool LoadVector(string name, out Vector3 vector)
    {
        string[] keys =
        {
            name + "x",
            name + "y",
            name + "z"
        };

        bool hasKey = PlayerPrefs.HasKey(keys[0]) && PlayerPrefs.HasKey(keys[1]) && PlayerPrefs.HasKey(keys[2]);

        vector = hasKey ? new Vector3(
            PlayerPrefs.GetFloat(keys[0]),
            PlayerPrefs.GetFloat(keys[1]),
            PlayerPrefs.GetFloat(keys[2])
            ) : Vector3.zero;

        return hasKey;
    }

    public static void DeleteVector(string name)
    {
        string[] keys =
        {
            name + "x",
            name + "y",
            name + "z"
        };

        foreach (string key in keys)
            PlayerPrefs.DeleteKey(key);
    }
}
