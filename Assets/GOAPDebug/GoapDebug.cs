using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GoapDebug : MonoBehaviour
{
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtGoapActions;
    public GoapDebugElement model;
    public Transform parent_elements;
    Dictionary<string, GoapDebugElement> col_Elements = new Dictionary<string, GoapDebugElement>();


    //public GameObject villager;
    //public CanvasGroup group;
    //bool active_villager = true;

    public static GoapDebug instance;
    private void Awake()
    {
        instance = this;
    }

    //private void Start()
    //{
    //    Invoke("Shutdown", 0.1f);
    //}
    //void Shutdown()
    //{
    //    villager.SetActive(false);
    //    active_villager = false;
    //    group.alpha = 0;
    //}

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Y))
    //    {
    //        active_villager = !active_villager;

    //        if (active_villager)
    //        {
    //            villager.SetActive(true);
    //            active_villager = true;
    //            group.alpha = 1;
    //            group.interactable = true;
    //        }
    //        else
    //        {
    //            villager.SetActive(false);
    //            active_villager = false;
    //            group.alpha = 0;
    //            group.interactable = false;
    //        }
    //    }
    //}

    public static void Message(string msg) { if(instance) instance.txtGoapActions.text = msg; }

    public static void RefreshState(GOAPState state) {  if(instance) instance.Refresh_State(state); }
    void Refresh_State(GOAPState state)
    {
        foreach (var s in state.values)
        {
            if (!col_Elements.ContainsKey(s.Key))
            {
                var elem = GameObject.Instantiate(model, parent_elements);
                elem.Configure(OnValueChange, s.Key);
                elem.SetValue(s.Value);
                col_Elements.Add(s.Key, elem);
            }
            else
            {
                var elem = col_Elements[s.Key];
                elem.SetValue(s.Value);
            }
        }
    }
    void OnValueChange(string s)
    {
        var villager = FindObjectOfType<Villager>();
        villager.DebugChangeAState(s);
    }

    public static void RefresPlan(IEnumerable<GOAPAction> actions) { if(instance) instance.Refresh_Plan(actions); }
    void Refresh_Plan(IEnumerable<GOAPAction> actions)
    {
        txtGoapActions.text = "\n";
        foreach (var e in actions)
        {
            txtGoapActions.text += e.name + "\n";
        }
    }
}
