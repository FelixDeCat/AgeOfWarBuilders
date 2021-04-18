using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_SimpleTwo : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image img;
    public void Set(string value, Sprite sp) {
        text.text = value;
        img.sprite = sp;
    }
}
