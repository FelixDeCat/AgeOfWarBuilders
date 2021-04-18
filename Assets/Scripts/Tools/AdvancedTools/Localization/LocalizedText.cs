using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour
{
    public string key;
    public bool show = false;

    private Text textComponent;

    private void Start()
    {
        if (textComponent == null) textComponent = GetComponent<Text>();
        textComponent.text = Localization.Instance.TryGetText(key);
        Localization.Instance.AddEventListener(OnLanguageChanged);
    }

    public void OnLanguageChanged()
    {
        textComponent.text = Localization.Instance.TryGetText(key);
    }

    

    private void OnValidate()
    {
        if (!show) return;
        if (textComponent == null) textComponent = GetComponent<Text>();
        textComponent.text = "[" + key + "]";
    }
}
