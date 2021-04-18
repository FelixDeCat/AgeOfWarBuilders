using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class Localization
{
    

    #region Singleton
    private static Localization instance;
    public static Localization Instance
    {
        get
        {
            if (instance == null)
                instance = new Localization();
            return instance;
        }
    }
    private Localization()
    {
    }
    #endregion

    public event Action ChangeLanguaje = delegate { };

    public string currentLanguage;
    private Dictionary<string, string> texts;

    public void AddEventListener(Action listener) { ChangeLanguaje += listener; }

    public string TryGetText(string key)
    {
        if (texts == null) throw new System.Exception("Localization not initialized");
        return texts.ContainsKey(key) ? texts[key] : "[" + key + "]";
    }

    public void LoadFromJson(string path)
    {
        texts = new Dictionary<string, string>();

        if (File.Exists(path))
        {
            var dataAsJson = File.ReadAllText(path);
            var loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            foreach (var item in loadedData.items) texts.Add(item.key, item.value);

            Debug.Log("Load succes. " + texts.Count + " items generated.");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }

        ChangeLanguaje();
    }

    [System.Serializable]
    public class LocalizationData
    {
        public LocalizationItem[] items;
    }

    [System.Serializable]
    public class LocalizationItem
    {
        public string key;
        public string value;

        public LocalizationItem()
        {
        }

        public LocalizationItem(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }
}
