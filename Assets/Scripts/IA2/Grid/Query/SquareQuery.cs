using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquareQuery : MonoBehaviour, IQuery {

    public SpatialGrid             targetGrid;
    public float                   width    = 15f;
    public float                   height   = 30f;
    public IEnumerable<IGridEntity> selected = new List<IGridEntity>();

    public IEnumerable<IGridEntity> Query() {
        var h = height * 0.5f;
        var w = width  * 0.5f;
        //posicion inicial --> esquina superior izquierda de la "caja"
        //posición final --> esquina inferior derecha de la "caja"
        //como funcion para filtrar le damos una que siempre devuelve true, para que no filtre nada.
        return targetGrid.Query(
                                transform.position + new Vector3(-w, 0, -h),
                                transform.position + new Vector3(w,  0, h),
                                x => true);
    }

    void OnDrawGizmos() {
        if (targetGrid == null) return;

        //Flatten the sphere we're going to draw
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 0, height));
    }
}