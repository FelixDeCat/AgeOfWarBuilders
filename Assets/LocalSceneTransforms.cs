using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSceneTransforms : MonoBehaviour
{
    public static LocalSceneTransforms instance;
    private void Awake()
    {
        instance = this;
    }

   [SerializeField] Transform parent_MyBuilings;

    public static Transform parent_MyBuildings => instance.parent_MyBuilings;
}
