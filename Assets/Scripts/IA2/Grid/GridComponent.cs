using UnityEngine;
using System;
public class GridComponent : MonoBehaviour, IGridEntity
{
    #region Variables
    [SerializeField] Grids type_grid;
    #region Interface IGrid
    public event Action<IGridEntity> OnMove;
    public Vector3 Position
    {
        get => myObject != null ? myObject.transform.position : new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
        set => myObject.transform.position = value;
    }
    bool isAlive;
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    #endregion
    GameObject myObject;
    public GameObject Grid_Object => myObject;

    public Grids gridType { get => type_grid; }
    #endregion

    #region Rise & Death
    public void Grid_Rise()
    {
        isAlive = true;
        OnMove?.Invoke(this);
    }
    public void Grid_Death()
    {
        isAlive = false;
        OnMove?.Invoke(this);
    }
    #endregion

    #region Initialize & Deinitialize
    public void Grid_Initialize(GameObject myObject)
    {
        this.myObject = myObject;
        if (GridManager.instance)
        {
            GridManager.GetGrid(type_grid).AddEntityToGrid(this);
            isAlive = true;
        }
        else Invoke("RetardedInitialization", 0.1f);
    }
    void RetardedInitialization()
    {
        GridManager.GetGrid(type_grid).AddEntityToGrid(this);
        isAlive = true;
    }
    public void Grid_Deinitialize()
    {
        GridManager.GetGrid(type_grid).RemoveEntityToGrid(this);
        isAlive = false;
    }
    #endregion

    #region Refresh
    public void Grid_RefreshComponent()
    {
        OnMove?.Invoke(this);
    }
    #endregion
}
