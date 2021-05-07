using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

public class Map {
    public bool                  isOnDangerZone;
    public int                   gold;
    public int                   distance;
    public IEnumerable<Action>   steps;
    public IEnumerable<Warnings> stepWarn;
}

public enum Warnings { Safe, Dangerous, Jewels, Gold, MagicObject }