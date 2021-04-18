using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Sector : MonoBehaviour
{
    public bool execute = false;

    bool isVisited;
    public bool IsVisited { get; set; }

    public int space = 5;

    public Transform upleft;
    public Transform downright;

    Vector2 saveA;
    Vector2 saveB;

    bool recalculate;

    public void Initialize()
    {
        Transform[] ts = gameObject.GetComponentsInChildren<Transform>();

        while (ts.Length < 3)
        {
            GameObject go_new = new GameObject();
            go_new.transform.SetParent(this.transform);
            ts = gameObject.GetComponentsInChildren<Transform>();
        }

        upleft = transform.GetChild(0);
        upleft.gameObject.name = "A";
        downright = transform.GetChild(1);
        downright.gameObject.name = "B";
    }

    private void Update()
    {
        if (!execute) return;

        Vector3 A = upleft.position;
        Vector3 B = downright.position;

        //este algoritmo hace que por ejemplo A no sobrepase a B y viceversa, entonces...
        //por ejemplo A siempre va a estar en la esquina superior izquierda, 
        //por mas que muevas lo que muevas
        if (A.x > B.x - space) B.x = A.x + space;
        if (A.y < B.y + space) B.y = A.y - space;

        upleft.position = A;
        downright.position = B;
    }

    // para que cuando estemos jugando no se ejecute el
    // Update ya que este se ejecuta en modo edicion
    public void Optimize() { execute = false; }

    public Vector2 GetRandomPos()
    {
        return new Vector2(
            Random.Range(upleft.position.x, downright.position.x),
            Random.Range(downright.position.y, upleft.position.y));
    }

    private void OnDrawGizmos()
    {
        if (!execute) return;

        Vector3 A = upleft.position;
        Vector3 B = downright.position;

        Gizmos.DrawLine(new Vector2(A.x, A.y), new Vector2(B.x, A.y));
        Gizmos.DrawLine(new Vector2(A.x, B.y), new Vector2(B.x, B.y));

        Gizmos.DrawLine(new Vector2(A.x, A.y), new Vector2(A.x, B.y));
        Gizmos.DrawLine(new Vector2(B.x, A.y), new Vector2(B.x, B.y));
    }
}
