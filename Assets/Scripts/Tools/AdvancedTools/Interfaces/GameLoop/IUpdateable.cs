using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.Interfaces
{
    public interface IUpdateable : IPlayable
    {
        void Tick(float DeltaTime);
    }
}


