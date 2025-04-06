using UnityEngine;

public class MenuCanvas : MonoBehaviour
{
    public GameObject[] startWith;

    private void OnEnable()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        foreach (GameObject obj in startWith)
            obj.SetActive(true);
    }
}
