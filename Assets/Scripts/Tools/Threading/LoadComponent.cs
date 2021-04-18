namespace Tools.UnityThread
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Tools.Interfaces;
    using System;

    public abstract class LoadComponent : MonoBehaviour, ISceneLoadable
    {
        public IEnumerator Load() { yield return LoadMe(); }
        protected abstract IEnumerator LoadMe();
        public virtual void OnSceneLoaded() { }
    }
}
