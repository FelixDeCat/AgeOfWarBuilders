using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Interfaces;

public enum UpdatePriority { High, Med, Low }

public class UpdateManager : MonoBehaviour
{

    Dictionary<UpdatePriority, List<IUpdateable>> updateables = new Dictionary<UpdatePriority, List<IUpdateable>>();

    public void StartGameUpdate()
    {

    }

    public void SubscribeUpdateable(IUpdateable updateable, UpdatePriority updatePriority = UpdatePriority.High)
    {

    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
}
