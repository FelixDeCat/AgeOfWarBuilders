using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceToCombat : MonoBehaviour
{
    [SerializeField] GridComponent gridComponent;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        gridComponent.Grid_Initialize(this.gameObject);
        gridComponent.Grid_Rise();
    }
    public void Deinitialize()
    {
        gridComponent.Grid_Deinitialize();
        gridComponent.Grid_Death();
    }
}
