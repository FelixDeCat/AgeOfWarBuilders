using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    private void Awake() => instance = this;
    Dictionary<string, Action> events = new Dictionary<string, Action>();
    public static void ADD_EVENT_LISTENER(string eventName, Action action) => instance.AddEventListener(eventName, action);
    public static void REMOVE_EVENT_LISTENER(string eventName, Action action) => instance.RemoveEventListener(eventName, action);
    public static void TRIGGER_EVENT(string eventName) => instance.TriggerEvent(eventName);
    void AddEventListener(string eventName, Action action) { if (!events.ContainsKey(eventName)) events.Add(eventName, action); else events[eventName] += action; }
    void RemoveEventListener(string eventName, Action action) { if (events.ContainsKey(eventName)) events[eventName] -= action; }
    void TriggerEvent(string eventName) { if (events.ContainsKey(eventName)) events[eventName]?.Invoke(); }
}
