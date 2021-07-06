using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceData", menuName = "FarmingSystem/ResourceData", order = 1)]
public class ResourceData : ScriptableObject
{
    [SerializeField] string data_name;
    
    public string DataName => data_name;
    public override bool Equals(object other) { return data_name == ((ResourceData)other).DataName; }
    public override int GetHashCode() => base.GetHashCode();
    public override string ToString() => data_name;
    [SerializeField] ResourceType type;
    public ResourceType Type => type;
    [SerializeField] Sprite image;
    public Sprite GetSprite() => image;
}
