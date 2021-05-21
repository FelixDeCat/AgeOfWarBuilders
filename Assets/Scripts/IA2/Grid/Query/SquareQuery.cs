using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquareQuery : QueryComponent, IQuery {

    public float                   width    = 15f;
    public float                   height   = 30f;
    public IEnumerable<IGridEntity> selected = new List<IGridEntity>();

    protected override void OnConfigure(Transform target)
    {

    }


    public override IEnumerable<IGridEntity> Query() {
        var h = height * 0.5f;
        var w = width  * 0.5f;
        //posicion inicial --> esquina superior izquierda de la "caja"
        //posición final --> esquina inferior derecha de la "caja"
        //como funcion para filtrar le damos una que siempre devuelve true, para que no filtre nada.
        return myGrid.Query(
                                target.position + new Vector3(-w, 0, -h),
                                target.position + new Vector3(w,  0, h),
                                x => true);
    }

    void OnDrawGizmos() {
        if (target == null) return;

        //Flatten the sphere we're going to draw
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(target.position, new Vector3(width, 0, height));
    }
}