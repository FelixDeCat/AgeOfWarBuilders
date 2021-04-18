using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenFeedback : MonoBehaviour
{
    public static ScreenFeedback instancia;

    public float cuantoTarda;
    public Image imgFeedbackPierdeVida;

    public Color ColorLastimado;
    public Color ColorTrasnparente;

    void Awake() { instancia = this; }

    public void PerderVida()
    {
        imgFeedbackPierdeVida.color = ColorLastimado;
        StartCoroutine(Golpe());
    }

    IEnumerator Golpe()
    {
        float timer = 0.0f;
        while (timer <= 1.0f)
        {
            imgFeedbackPierdeVida.color = Color.Lerp(ColorLastimado, ColorTrasnparente, timer);
            timer += Time.deltaTime / cuantoTarda;
            yield return null;
        }
    }
}
