using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Etiqueta : MonoBehaviour
{
    public TextMeshProUGUI etiqueta;
    public void SetName(string s)
    {
        etiqueta.text = s;
    }
}
