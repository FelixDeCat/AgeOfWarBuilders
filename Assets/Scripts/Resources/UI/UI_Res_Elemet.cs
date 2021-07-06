using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Res_Elemet : MonoBehaviour
{
    public Image img;
    public TextMeshProUGUI quant;
    public Animator myAnim;

    public void Configurate(Sprite _img)
    {
        img.sprite = _img;
        quant.text = "0";
    }

    public void SetQuant(string val)
    {
        quant.text = val;
    }

    public void Animate_Add() => myAnim.Play("Add");
    public void Animate_Remove() => myAnim.Play("Remove");
    public void Animate_CanNotRemove() => myAnim.Play("CanNotRemove");
    public void Animate_CanNotAdd() => myAnim.Play("CanNotAdd");
}
