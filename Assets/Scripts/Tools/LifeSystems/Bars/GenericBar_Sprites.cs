using UnityEngine;
using UnityEngine.UI;

public class GenericBar_Sprites : GenericBar
{
    public Image sp;
    public void SetImageOriginalColor() { sp.color = originalColor; }
    public void SetImageColor(Color val) { sp.color = val; }
    Color originalColor;
    public Color  OriginalColor { get { return originalColor; } }
    private void Start()
    {
        originalColor = sp.color;
    }

    protected override void OnSetValue(float value)
    {
        var percent = (value * 100) / maxValue;
        if (porcentaje) porcentaje.text = !realvalue ? ((int)percent).ToString() + "%" : value + " / " + maxValue;
        sp.fillAmount = percent * scaler;
    }
    public void OnSetAsyncOperation(string name, AsyncOperation operation)
    {
        porcentajePro.text = name;
        sp.fillAmount = operation.progress;
    }
}
