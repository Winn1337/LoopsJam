using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSlider : MonoBehaviour
{
    public Image fill;
    public Color defaultColor = Color.white;
    public Color selectedColor = new Color(1f, 0.98f, 0.75f);

    bool down;
    bool entered;

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

        entry = new() { eventID = EventTriggerType.PointerUp };
        entry.callback.AddListener((data) => { PointerUp(); });
        eventTrigger.triggers.Add(entry);
    }

    private void OnEnable()
    {
        fill.color = defaultColor;
    }

    public void PointerEnter()
    {
        entered = true;
        fill.color = selectedColor;
        AudioManager.PlaySFX(AudioManager.instance.Clips.enterUI, Vector3.zero, 1f);
    }

    public void PointerExit()
    {
        entered = false;
        if (down) return;

        fill.color = defaultColor;
        AudioManager.PlaySFX(AudioManager.instance.Clips.exitUI, Vector3.zero, 1f);
    }

    public void PointerDown()
    {
        down = true;
        fill.color = selectedColor;
        AudioManager.PlaySFX(AudioManager.instance.Clips.clickUI, Vector3.zero, 1f);
    }

    public void PointerUp()
    {
        down = false;
        if (entered) return;
        fill.color = defaultColor;
    }
}
