using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GenericEnergyComponent : MonoBehaviour
{

    [SerializeField] string name_energy_type;
    [SerializeField] int initial_Energy = 100;
    [SerializeField] protected LifeSystemBase life;
    [SerializeField] FrontendStatBase ui_life;

    public float Energy { get => life.Life; }

    [SerializeField] UnityEvent IAmSpendingEnergy;
    [SerializeField] UnityEvent IAmResting;
    [SerializeField] UnityEvent IAmTired;

    private void Start()
    {
        life = new LifeSystemBase(
            initial_Energy,
            Feedback_IAmSpendingEnergy,
            Feedback_IAmResting,
            FeedbackIAmTired,
            ui_life,
            initial_Energy);
    }

    public bool EnergyIsFull() => life.IsFull();
    public bool EnergyEmpty() => life.IsEmpty();
    public void AddEnergy(int val) => life.Heal(1); 
    public void SpendEnergy(int spended = 1) => life.Hit(spended);
    public void FillAllEnergy() => life.ResetLife();

    protected virtual void Feedback_IAmSpendingEnergy() { IAmSpendingEnergy.Invoke(); }
    protected virtual void Feedback_IAmResting() { IAmResting.Invoke(); }
    protected virtual void FeedbackIAmTired() { IAmTired.Invoke(); }
}
