using UnityEngine;
using UnityEngine.UI;
using TMPro;
public abstract class GenericBar : MonoBehaviour
{
    protected float maxValue;
    protected float scaler;

    [Header("para visualizar la barra")]
    [SerializeField] protected Text porcentaje;
    [SerializeField] protected TextMeshProUGUI porcentajePro;
    [SerializeField] protected bool realvalue;

    public void Configure(int maxValue, float scaler)
    {
        this.maxValue = maxValue;
        this.scaler = scaler;
        
    }
    public void Configure(int maxValue, float scaler, float val)
    {
        this.maxValue = maxValue;
        this.scaler = scaler;

        var percent = (val * 100) / maxValue;
        if (porcentaje) porcentaje.text = !realvalue ? ((int)percent).ToString() + "%" : val + " / " + maxValue;
        if (porcentajePro) porcentajePro.text = !realvalue ? ((int)percent).ToString() + "%" : val + " / " + maxValue;
    }
    public void Configure(float maxValue, float scaler)
    {
        this.maxValue = maxValue;
        this.scaler = scaler;
    }

    public void SetValue(float val) => OnSetValue(val);
    protected abstract void OnSetValue(float val);
    
}
