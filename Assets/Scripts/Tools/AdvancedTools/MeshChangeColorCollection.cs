using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshChangeColorCollection : MonoBehaviour
{
    [SerializeField] Renderer[] rendersToChange;

    [SerializeField] string RENDER_COLOR_PARAM = "_Color";

    public void Change(Color color)
    {
        for (int i = 0; i < rendersToChange.Length; i++)
        {
            rendersToChange[i].material.SetColor(RENDER_COLOR_PARAM, color);
        }
    }

}
