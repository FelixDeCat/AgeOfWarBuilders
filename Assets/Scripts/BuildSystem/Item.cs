using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Structs;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
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
    public Item(Item_255 item)
    {
        this.item = item;
    }
    #endregion
    public override bool Equals(object other) => ID == ((Item)other).ID;
    public override string ToString() => "[" + ID + ":" + Name + "]";
    public override int GetHashCode()
    {
        var hashCode = 648655266;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<Item_255>.Default.GetHashCode(item);
        hashCode = hashCode * -1521134295 + ID.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
        return hashCode;
    }
    #endregion
}
