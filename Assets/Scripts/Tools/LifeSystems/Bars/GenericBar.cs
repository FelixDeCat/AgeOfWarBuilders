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
    public void Configure(int maxValue, int scaler, float val)
    {
        this.maxValue = maxValue;
        this.scaler = scaler;
    }
    public void Configure(float maxValue, float scaler)
    {
        this.maxValue = maxValue;
        this.scaler = scaler;
    }

    public void SetValue(float val) => OnSetValue(val);
    protected abstract void OnSetValue(float val);
    
}
