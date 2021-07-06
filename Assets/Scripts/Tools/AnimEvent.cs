using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimEvent : MonoBehaviour
{
    Dictionary<string, Action> events = new Dictionary<string, Action>();

    public void ADD_ANIM_EVENT_LISTENER(string parameter, Action callback)
    {
        if (!events.ContainsKey(parameter))
        {
            events.Add(parameter, null);
            events[parameter] += callback;
        }
        else
        {
            events[parameter] += callback;
        }
    }
    public void REMOVE_ANIM_EVENT_LISTENER(string parameter, Action callback)
    {
        if (events.ContainsKey(parameter))
        {
            events[parameter] -= callback;
        }
    }

    public void TRIGGER_EVENT(string parameter)
    {
        if (events.ContainsKey(parameter))
            events[parameter]?.Invoke();
    }
}
