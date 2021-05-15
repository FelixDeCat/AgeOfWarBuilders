﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpatialGrid : MonoBehaviour
{

    #region Variables

    public static SpatialGrid instance;

    //punto de inicio de la grilla en X
    public float x;

    //punto de inicio de la grilla en Z
    public float z;

    //ancho de las celdas
    public float cellWidth;

    //alto de las celdas
    public float cellHeight;

    //cantidad de columnas (el "ancho" de la grilla)
    public int width;

    //cantidad de filas (el "alto" de la grilla)
    public int height;

    //ultimas posiciones conocidas de los elementos, guardadas para comparación.
    private Dictionary<IGridEntity, Tuple<int, int>> lastPositions = new Dictionary<IGridEntity, Tuple<int, int>>();

    //los "contenedores"
    private HashSet<IGridEntity>[,] buckets;

    //el valor de posicion que tienen los elementos cuando no estan en la zona de la grilla.
    /*
     Const es implicitamente statica
     const tengo que ponerle el valor apenas la declaro, readonly puedo hacerlo en el constructor.
     Const solo sirve para tipos de dato primitivos.
     */
    readonly public Tuple<int, int> Outside = Tuple.Create(-1, -1);

    //Una colección vacía a devolver en las queries si no hay nada que devolver
    readonly public IGridEntity[] Empty = new IGridEntity[0];

    #endregion

    #region Funciones

    private void Awake()
    {
        instance = this;

        buckets = new HashSet<IGridEntity>[width, height];

        //creamos todos los hashsets
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                buckets[i, j] = new HashSet<IGridEntity>();
            }
        }

        var ents = RecursiveWalker(transform)
                  .Where(x => x.gameObject.activeSelf)//esto es para que no me seleccione los que no estan activos
                  .Select(n => n.GetComponent<IGridEntity>())
                  .Where(n => n != null);

        foreach (var e in ents)
        {
            e.OnMove += UpdateEntity;
            UpdateEntity(e);
        }
    }

    public void AddEntityToGrid(IGridEntity entity)
    {
        entity.OnMove += UpdateEntity;
        UpdateEntity(entity);
    }
    public void RemoveEntityToGrid(IGridEntity entity)
    {
        entity.OnMove -= UpdateEntity;
        entity.Position = new Vector3(int.MinValue, int.MinValue, int.MinValue);
        UpdateEntity(entity);
    }

    public void UpdateEntity(IGridEntity entity)
    {
        var lastPos = lastPositions.ContainsKey(entity) ? lastPositions[entity] : Outside;
        var currentPos = ConvertGlobalPosToGridNormalized(entity.Position);

        //Misma posición, no necesito hacer nada
        if (lastPos.Equals(currentPos))
            return;

        //Lo "sacamos" de la posición anterior
        if (IsInsideGrid(lastPos))
        {
            buckets[lastPos.Item1, lastPos.Item2].Remove(entity);
        }

        //Lo "metemos" a la celda nueva, o lo sacamos si salio de la grilla
        if (IsInsideGrid(currentPos) && entity.IsAlive)
        {
            buckets[currentPos.Item1, currentPos.Item2].Add(entity);
            lastPositions[entity] = currentPos;
        }
        else
            lastPositions.Remove(entity);
    }

    public IEnumerable<IGridEntity> Query(Vector3 aabbFrom, Vector3 aabbTo, Func<Vector3, bool> filterByPosition)
    {
        //establezco cual de los dos vectores es el maximo y el minimo, por si llega a estar al revez
        var from = new Vector3(Mathf.Min(aabbFrom.x, aabbTo.x), 0, Mathf.Min(aabbFrom.z, aabbTo.z));
        var to = new Vector3(Mathf.Max(aabbFrom.x, aabbTo.x), 0, Mathf.Max(aabbFrom.z, aabbTo.z));

        //normalizo a posicion de Grilla
        var fromCoord = ConvertGlobalPosToGridNormalized(from);
        var toCoord = ConvertGlobalPosToGridNormalized(to);

        //clampeo las dos coordenadas para que no se vayan fuera de la grilla
        fromCoord = Tuple.Create(Util.Clamp(fromCoord.Item1, 0, width), Util.Clamp(fromCoord.Item2, 0, height));
        toCoord = Tuple.Create(Util.Clamp(toCoord.Item1, 0, width), Util.Clamp(toCoord.Item2, 0, height));

        //un chekeo mas por las dudas
        if (!IsInsideGrid(fromCoord) && !IsInsideGrid(toCoord))
            return Empty;

        // Creamos tuplas de cada celda
        var cols = Util.Generate(fromCoord.Item1, x => x + 1)
                       .TakeWhile(n => n < width && n <= toCoord.Item1);

        var rows = Util.Generate(fromCoord.Item2, y => y + 1)
                       .TakeWhile(y => y < height && y <= toCoord.Item2);

        var cells = cols.SelectMany(col => rows.Select(row => Tuple.Create(col, row) ) );

        // Iteramos las que queden dentro del criterio
        return cells
              .SelectMany(cell => buckets[cell.Item1, cell.Item2])
              .Where(e =>
                         from.x <= e.Position.x && e.Position.x <= to.x &&
                         from.z <= e.Position.z && e.Position.z <= to.z
                    )
              .Where(n => filterByPosition(n.Position));
    }
    public void RemoveFromBucket(IGridEntity entity)
    {
        var pos = ConvertGlobalPosToGridNormalized(entity.Position);
        buckets[pos.Item1, pos.Item2].Remove(entity);
    }

    /// <summary>
    /// a esta funcion vos le pasas una posicion global, y te la convierte a una que puede manejar la grilla
    /// no importa a que posicion este la grilla ni el tamaño de sus celdas, Normaliza todo como si la grilla
    /// estuviera en pos (0,0) y tamaño (1,1)
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Tuple<int, int> ConvertGlobalPosToGridNormalized(Vector3 pos)
    {
        //quita la diferencia, divide segun las celdas y floorea
        return Tuple.Create(Mathf.FloorToInt((pos.x - x) / cellWidth),
                            Mathf.FloorToInt((pos.z - z) / cellHeight));
    }

    public bool IsInsideGrid(Tuple<int, int> position)
    {
        //si es menor a 0 o mayor a width o height, no esta dentro de la grilla
        return 0 <= position.Item1 && position.Item1 < width &&
               0 <= position.Item2 && position.Item2 < height;
    }

    void OnDestroy()
    {
        var ents = RecursiveWalker(transform).Select(n => n.GetComponent<IGridEntity>())
                                             .Where(n => n != null);

        foreach (var e in ents) e.OnMove -= UpdateEntity;
    }

    #region GENERATORS

    private static IEnumerable<Transform> RecursiveWalker(Transform parent)
    {
        foreach (Transform child in parent)
        {
            foreach (Transform grandchild in RecursiveWalker(child))
                yield return grandchild;
            yield return child;
        }
    }

    #endregion

    #endregion

    #region GRAPHIC REPRESENTATION

    public bool areGizmosShutDown;
    public bool activatedGrid;
    public bool showLogs = true;

    private void OnDrawGizmos()
    {
        var rows = Util.Generate(z, curr => curr + cellHeight)
                       .Select(row => Tuple.Create(new Vector3(x, 0, row),
                                                   new Vector3(x + cellWidth * width, 0, row)));

        //equivalente de rows
        /*for (int i = 0; i <= height; i++)
        {
            Gizmos.DrawLine(new Vector3(x, 0, z + cellHeight * i), new Vector3(x + cellWidth * width,0, z + cellHeight * i));
        }*/

        var cols = Util.Generate(x, curr => curr + cellWidth)
                       .Select(col => Tuple.Create(new Vector3(col, 0, z),
                                                   new Vector3(col, 0, z + cellHeight * height)));

        var allLines = rows.Take(width + 1).Concat(cols.Take(height + 1));

        foreach (var elem in allLines)
        {
            Gizmos.DrawLine(elem.Item1, elem.Item2);
        }

        if (buckets == null || areGizmosShutDown) return;

        var originalCol = GUI.color;
        GUI.color = Color.red;
        if (!activatedGrid)
        {
            var allElems = new List<IGridEntity>();
            foreach (var elem in buckets)
                allElems = allElems.Concat(elem).ToList();

            int connections = 0;
            foreach (var entity in allElems)
            {
                foreach (var neighbour in allElems.Where(x => x != entity))
                {
                    Gizmos.DrawLine(entity.Position, neighbour.Position);
                    connections++;
                }

                if (showLogs)
                    Debug.Log("tengo " + connections + " conexiones por individuo");
                connections = 0;
            }
        }
        else
        {
            int connections = 0;
            foreach (var elem in buckets)
            {
                foreach (var ent in elem)
                {
                    foreach (var n in elem.Where(x => x != ent))
                    {
                        Gizmos.DrawLine(ent.Position, n.Position);
                        connections++;
                    }

                    if (showLogs)
                        Debug.Log("tengo " + connections + " conexiones por individuo");
                    connections = 0;
                }
            }
        }

        GUI.color = originalCol;
        showLogs = false;
    }

    #endregion
}
