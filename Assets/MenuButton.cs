using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System;

public class MenuButton : MonoBehaviour
{
    public float duration = 0.2f;
    public float selectedScale = 1.05f;
    public Color defaultColor = Color.white;
    public Color selectedColor = new Color(1f, 0.98f, 0.75f);

    TextMeshProUGUI textMesh;
    Vector3 originalScale;

    private void Awake()
    {
        EventTrigger eventTrigger = transform.GetOrAdd<EventTrigger>();
        eventTrigger.triggers.Clear();

        EventTrigger.Entry entry = new() { eventID = EventTriggerType.PointerEnter };
        entry.callback.AddListener((data) => { PointerEnter(); });
        eventTrigger.triggers.Add(entry);

        entry = new() { eventID = EventTriggerType.PointerExit };
        entry.callback.AddListener((data) => { PointerExit(); });
        eventTrigger.triggers.Add(entry);

        entry = new() { eventID = EventTriggerType.PointerDown };
        entry.callback.AddListener((data) => { PointerDown(); });
        eventTrigger.triggers.Add(entry);

        originalScale = transform.localScale;

        textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        transform.localScale = originalScale;
        textMesh.color = defaultColor;
    }

    public void PointerEnter()
    {
        StopAllCoroutines();
        StartCoroutine(BounceScaleRoutine(selectedScale, 2f / duration));
        textMesh.color = selectedColor;
        AudioManager.PlaySFX(AudioManager.instance.Clips.enterUI, Vector3.zero, 1f);
    }

    public void PointerExit()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleCoroutine(1f, duration));
        textMesh.color = defaultColor;
        AudioManager.PlaySFX(AudioManager.instance.Clips.exitUI, Vector3.zero, 1f);
    }

    private void PointerDown()
    {
        AudioManager.PlaySFX(AudioManager.instance.Clips.clickUI, Vector3.zero, 1f);
    }

    private IEnumerator ScaleCoroutine(float targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = originalScale * targetScale;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.localScale = endScale;
    }

    private IEnumerator BounceScaleRoutine(float targetScale, float frequency)
    {
        Vector3 startScale = originalScale;
        Vector3 endScale = originalScale * targetScale;
        float elapsedTime = 0f;
        while (true)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, Mathf.Sin(elapsedTime * frequency - 90) * 0.5f + 0.5f);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
