using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class SectorManager : MonoBehaviour
{
    public List<Sector> sectors;

    private void Awake() {
        Recalculate();
        sectors.ForEach(x => {
            x.Initialize();
            x.Optimize();
        });

    }

    public Vector2 FindRandomPos()
    {
        for (int i = 0; i < sectors.Count; i++)
        {
            if (!sectors[i].IsVisited)
            {
                sectors[i].IsVisited = true;
                return sectors[i].GetRandomPos();
            }
        }
        Recalculate();
        sectors[0].IsVisited = true;
        return sectors[0].GetRandomPos();
    }

    void Recalculate()
    {
        sectors.ForEach(x => x.IsVisited = false);
        sectors.Randomize();
    }
}
