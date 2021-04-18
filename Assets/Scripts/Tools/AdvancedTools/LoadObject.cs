using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadObject : LoadComponent
{
    Action endload = delegate { };
    [SerializeField] string prefabname = "asd";

    GameObject go;

    public void LoadScene()
    {
        StartCoroutine(LoadMe());
    }

    protected override IEnumerator LoadMe()
    {
        go = Instantiate(Resources.Load<GameObject>(prefabname), this.transform);
        endload.Invoke();
        yield return null;
    }

    public void LoadObjects(Action callbackendload)
    {
        endload = callbackendload;
        StartCoroutine(LoadMe());
    }

    public void Clean()
    {
        Destroy(go);
    }

    
}
