using System;
using UnityEngine;

[Serializable]
public class WeightedNode<T> {

    [SerializeField] private T     _element;
    [SerializeField] private float _weight;

    public T     Element => _element;
    public float Weight  => _weight;


    public WeightedNode(T element, float weight) {
        _element = element;
        _weight  = weight;
    }

}