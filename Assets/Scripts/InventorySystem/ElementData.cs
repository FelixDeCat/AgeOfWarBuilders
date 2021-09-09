using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "ScriptableObjects/Element", order = 1)]
public class ElementData : ScriptableObject
{
    #region Editor variables
    [SerializeField] string element_name = "default_element";
    [SerializeField] int element_ID = -1;
    [SerializeField] int max_stack = 10;
    [SerializeField] string description = "this is a element description";
    [SerializeField] float weight = 1.0f;
    [SerializeField] Sprite image;
    #endregion

    #region Getters
    public int Element_ID => element_ID;
    public string Element_Name => element_name;
    public int MaxStack => max_stack;
    public string Description => description;
    public float Weight => weight;
    public Sprite Element_Image => image;
    #endregion

    #region Object override
    public override bool Equals(object other)
    {
        var elem = (ElementData)other;
        return elem.element_name == element_name &&
            elem.max_stack == max_stack &&
            elem.description == description &&
            elem.weight == weight;
    }
    public override int GetHashCode()
    {
        var hashCode = -268434545;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + element_ID.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(element_name);
        hashCode = hashCode * -1521134295 + max_stack.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(description);
        hashCode = hashCode * -1521134295 + weight.GetHashCode();
        hashCode = hashCode * -1521134295 + Element_ID.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Element_Name);
        hashCode = hashCode * -1521134295 + MaxStack.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
        hashCode = hashCode * -1521134295 + Weight.GetHashCode();
        return hashCode;
    }
    #endregion

}
