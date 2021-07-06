using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectCoordinates : MonoBehaviour
{
    public float radius = 5;
    public Vector3 centerpoint;
    public Vector3[] coordinates;
    public string[] names;

    public Etiqueta model;
    public Transform parent;
    public Etiqueta[] etiquetas;

    private void Start()
    {
        for (int i = 0; i < coordinates.Length; i++)
        {
            var e = GameObject.Instantiate(model, parent);
            e.transform.position = coordinates[i];
            e.SetName(names[i]);
        }
    }

    private void OnDrawGizmos()
    {
        centerpoint = Vector3.zero;

        Gizmos.color = Color.red;
        for (int i = 0; i < coordinates.Length; i++)
        {
            centerpoint += coordinates[i];
            Gizmos.DrawSphere(coordinates[i], radius);
        }

        centerpoint = centerpoint / coordinates.Length;

        Gizmos.color = Color.blue;
        for (int i = 0; i < coordinates.Length; i++)
        {
            Gizmos.DrawLine(centerpoint, coordinates[i]);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(4000,0, 4000));
    }
}
