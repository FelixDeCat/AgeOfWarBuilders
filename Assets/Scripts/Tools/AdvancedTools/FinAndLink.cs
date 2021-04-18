namespace Tools.Extensions
{
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;
    using System;

    public static class FinAndLink
    {
        public static R FindAndLink<R>(this GameObject go)
        {
            R sample = default(R);

            try
            {
                var obj = go.GetComponentsInChildren<R>().First();
                return obj;
            }
            catch (System.InvalidOperationException ex)
            {
                var s = ex;
                Debug.LogError("Che bobo... me estas un pidiendo un \"" + sample.ToString() + "\" pero no hay ninguno entre los childrens");
                return sample;
            }
        }
        public static void FindAndLink<R>(this GameObject go, Action<List<R>> deleg)
        {
            R sample = default(R);

            try
            {
                var obj = go.GetComponentsInChildren<R>();
                deleg(obj.ToList());
            }
            catch (System.InvalidOperationException ex)
            {
                var s = ex;
                Debug.LogError("Che bobo... me estas un pidiendo un \"" + sample.ToString() + "\" pero no hay ninguno entre los childrens");
            }
        }
        public static void FindAndLink<R>(this GameObject go, Action<R[]> deleg)
        {
            R sample = default(R);

            try
            {
                var obj = go.GetComponentsInChildren<R>();
                deleg(obj);
            }
            catch (System.InvalidOperationException ex)
            {
                var s = ex;
                Debug.LogError("Che bobo... me estas un pidiendo un \"" + sample.ToString() + "\" pero no hay ninguno entre los childrens");
            }
        }
    }
}