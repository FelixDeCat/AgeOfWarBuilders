using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UVMove : MonoBehaviour
{
    RawImage image;
    Rect aux;
    Rect aucs;
    float timer;
    void Start()
    {
        image = GetComponent<RawImage>();
        aux = image.uvRect;
        aucs = new Rect();
    }

    void Update()
    {
        if (timer < 1f)
        {
            timer = timer + 1f * Time.deltaTime;
        }
        else
        {
            timer = 0;
        }

        aucs.Set(timer, aux.y, aux.width, aux.height);
        image.uvRect = aucs;
    }
}
