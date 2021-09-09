using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Structs;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item256 : ScriptableObject
{
    [SerializeField] Item_255 item;

    public int ID { get => item.Id; }
    public string Name { get => item.Name; }

    #region Modificadores
    #region Contructor
    /// <summary>
    /// [NO USAR] solo en el caso de que tenga que crearlo por codigo
    /// </summary>
    /// <param name="item"></param>
    public Item256(Item_255 item)
    {
        this.item = item;
    }
    #endregion
    public override bool Equals(object other) => ID == ((Item256)other).ID;
    public override string ToString() => "[" + ID + ":" + Name + "]";
    #endregion
}
