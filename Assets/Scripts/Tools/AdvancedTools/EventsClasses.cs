namespace Tools.EventClasses
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

    [System.Serializable] public class EventFloat : UnityEvent<float> { }
    [System.Serializable] public class EventInt : UnityEvent<int> { }
    [System.Serializable] public class EventIntIntString : UnityEvent<int, int, string> { }
    [System.Serializable] public class EventBool : UnityEvent<bool> { }
    [System.Serializable] public class EventTwoInt : UnityEvent<int, int> { }
    [System.Serializable] public class EventString : UnityEvent<string> { }
    [System.Serializable] public class EventVector2 : UnityEvent<Vector2> { }
    [System.Serializable] public class EventVector3 : UnityEvent<Vector3> { }
    [System.Serializable] public class EventObject : UnityEvent<object> { }
    [System.Serializable] public class EventAction : UnityEvent<Action> { }
    [System.Serializable] public class EventCounterPredicate : UnityEvent<Func<bool>> { }

}


