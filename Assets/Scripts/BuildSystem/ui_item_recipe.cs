using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ui_item_recipe : MonoBehaviour
{
    public Image img_recipe;
    public TextMeshProUGUI text_recipe;

    public void SetValues(Sprite img, int quant)
    {
        img_recipe.sprite = img;
        text_recipe.text = quant.ToString();
    }

    public void IHave(bool val)
    {
        text_recipe.color = val ? Color.green : Color.red;
    }
}
