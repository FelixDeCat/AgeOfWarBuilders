using System.Collections;
using System.Collections.Generic;
using System;
using Tools.Screen;

namespace Tools.Extensions
{
    using UnityEngine;
    using RandomUnity = UnityEngine.Random;
    using RandomWin = System.Random;
    using System.Linq;

    namespace Tools.Extensions.Collections
    {

    }

    public static class Extensions
    {
        //public static GameObject FindObjectWithObjectFinder(ObjectFinder.type type)
        //{
        //    var objects = GameObject.FindObjectsOfType<ObjectFinder>();
        //    foreach (var v in objects) { if (v._type == type) return v.gameObject; }
        //    return null;
        //}

           

        private static System.Random rng = new System.Random();
        public static List<T> Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

        public static List<T> FindInRadius<T>(this T own, float radius) where T : Component
        {
            var col = new List<T>();

            foreach (var v in Physics.OverlapSphere(own.transform.position, radius))
                if (v.GetComponent<T>()) col.Add(v.GetComponent<T>());

            List<Collider> colliders = new List<Collider>();

            return col
                .Where(x => !own.Equals(x))
                .Where(n =>
                {
                    Vector3 dir = n.transform.position - own.transform.position;
                    if (!Physics.Raycast(own.transform.position, dir, out RaycastHit ray)) return false;
                    colliders.Add(ray.collider);
                    return colliders.Contains(n.GetComponent<Collider>());
                })
                .ToList();
        }

        public static string StringConcatCollection<T>(this IEnumerable<T> col) where T : Component
        {
            string aux = "Col>> " + col.ToString() + "";
            foreach (var c in col)
            {
                aux += " [" + c.ToString() + "] ";
            }
            return aux;
        }
        public static List<T> FindInRadius<T>(this T own, float radius, LayerMask layermask) where T : Component
        {
            var col = new List<T>();
            var finded_colliders = Physics.OverlapSphere(own.transform.position, radius);
            foreach (var v in finded_colliders)
            {
                if (v.GetComponent<T>()) col.Add(v.GetComponent<T>());
            }
            List<Collider> colliders = new List<Collider>();
            return col
                .Where(x => !own.Equals(x))
                .Where(n =>
                {
                    Vector3 dir = n.transform.position - own.transform.position;
                    if (!Physics.Raycast(own.transform.position, dir, out RaycastHit ray, 10, layermask)) return false;
                    colliders.Add(ray.collider);
                    return colliders.Contains(n.GetComponent<Collider>());
                })
                .ToList();
        }

        public static List<T> FindInRadius<T>(this Transform pos, float radius) where T : Component
        {
            var col = new List<T>();
            foreach (var v in Physics.OverlapSphere(pos.position, radius))
            {

                if (v.GetComponent<T>()) col.Add(v.GetComponent<T>());
            }
            return col.ToList();
        }

        public static List<T> FindInRadius<T>(this Vector3 pos, float radius) where T : Component
        {
            var col = new List<T>();
            foreach (var v in Physics.OverlapSphere(pos, radius))
            {
                if (v.GetComponent<T>()) col.Add(v.GetComponent<T>());
            }
            return col.ToList();
        }
        public static List<T> FindInRadiusByCondition<T>(this Vector3 pos, float radius, Func<T, bool> condition) where T : Component
        {
            var col = new List<T>();
            foreach (var v in Physics.OverlapSphere(pos, radius))
            {
                var comp = v.GetComponent<T>();
                if (comp != null)
                {
                    if (condition(comp))
                    {
                        col.Add(comp);
                    }
                }
            }
            return col.ToList();
        }
        public static List<T> FindInRadiusByConditionNoPhysics<T>(this Vector3 pos, float radius, List<T> objs, Func<T, bool> condition) where T : Component
        {
            var col = new List<T>();

            foreach (var o in objs)
            {
                if (Vector3.Distance(o.transform.position, pos) <= radius)
                {
                    var comp = o.GetComponent<T>();
                    if (comp != null)
                    {
                        if (condition(comp))
                        {
                            col.Add(comp);
                        }
                    }
                }
            }
            return col.ToList();
        }
        public static List<T> FindInRadiusNoPhysics<T>(this Vector3 pos, float radius, List<T> objs) where T : Component
        {
            var col = new List<T>();

            foreach (var o in objs)
            {
                if (Vector3.Distance(o.transform.position, pos) <= radius)
                {
                    var comp = o.GetComponent<T>();
                    if (comp != null)
                    {
                        col.Add(comp);
                    }
                }
            }
            return col.ToList();
        }

        public static T FindMostClose<T>(this Vector3 pos, float radius_to_find) where T : Component
        {
            List<T> col_in_radius = pos.FindInRadius<T>(radius_to_find);

            if (col_in_radius.Count > 0)
            {
                T most_close = default;
                float best_distance = float.MaxValue;
                foreach (var c in col_in_radius)
                {
                    float dist = Vector3.Distance(pos, c.transform.position);
                    if (best_distance > dist)
                    {
                        best_distance = dist;
                        most_close = c;
                    }
                }
                return most_close;
            }
            else
            {
                return default(T);
            }
        }

        public static Vector3 ClampV3ZeroDotFive(this Vector3 v3)
        {
            return new Vector3(v3.x.ClampZeroDotFive(), v3.y.ClampZeroDotFive(), v3.z.ClampZeroDotFive());
        }

        public static float ClampZeroDotFive(this float val)
        {
            float xaux = (int)val;

            if (val >= 0)
            {
                if (val <= xaux + 0.5f)
                {
                    if (val >= xaux + 0.25f) val = xaux + 0.5f;
                    else val = xaux;
                }
                else
                {
                    if (val <= xaux + 0.75) val = xaux + 0.5f;
                    else val = xaux + 1;
                }
            }
            else
            {
                if (val >= xaux - 0.5f)
                {
                    if (val <= xaux - 0.25f) val = xaux - 0.5f;
                    else val = xaux;
                }
                else
                {
                    if (val >= xaux - 0.75) val = xaux - 0.5f;
                    else val = xaux - 1;
                }
            }
            
            return val;
        }

        public static void AddListToList<T>(this List<T> col, List<T> toadd)
        {
            foreach (var c in toadd)
            {
                col.Add(c);
            }
        }


        public static bool LineOfSight(Transform myTransform, Transform myTarget, float viewdistance, float viewangle, LayerMask layermask)
        {
            var dist = Vector3.Distance(myTransform.position, myTarget.transform.position);
            if (dist > viewdistance) return false;
            var dirtotarget = (myTarget.transform.position - myTransform.position).normalized;
            var angletotarget = Vector3.Angle(myTransform.forward, dirtotarget);
            if (angletotarget <= viewangle)
            {
                bool obstaclesBetween = false;
                if (Physics.Raycast(myTransform.position, dirtotarget, out RaycastHit raycastInfo, dist, layermask))
                {
                    obstaclesBetween = true;
                }
                return !obstaclesBetween ? true : false;
            }
            else return false;
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        {
            RandomWin rnd = new RandomWin();
            return source.OrderBy<T, int>((item) => rnd.Next());
        }

        //para strechear un componente UI dentro de otro. si es que no se va a mover en ejecución
        public static void Stretch(this RectTransform tr)
        {
            tr.anchorMin = new Vector2(0, 0);
            tr.anchorMax = new Vector2(1, 1);
            tr.offsetMax = new Vector2(0, 0);
            tr.offsetMin = new Vector2(0, 0);
        }

        public static int NextIndex(this int current, int count)
        {
            var aux = current;
            if (current >= count - 1) aux = 0;
            else aux++;

            return aux;
        }
        public static int BackIndex(this int current, int count)
        {
            var aux = current;
            if (current <= 0) aux = count - 1;
            else aux--;

            return aux;
        }
        public static int NextPingPongIndex(this int current, int count, ref bool go)
        {
            var aux = current;
            if (go) { if (current < count-1) aux++; else { aux = count-1; go = false; } }
            else { if (current > 0) aux--; else { aux = 0; go = true; } }
            return aux;
        }

        public static void Stretch<T>(this T obj) where T : UnityEngine.UI.Graphic
        {
            obj.rectTransform.anchorMin = new Vector2(0, 0);
            obj.rectTransform.anchorMax = new Vector2(1, 1);
            obj.rectTransform.offsetMax = new Vector2(0, 0);
            obj.rectTransform.offsetMin = new Vector2(0, 0);
        }

        public static T ReturnMostClose<T>(this IEnumerable<T> col, Vector3 comparer, Func<T, bool> predicate = null) where T : Component
        {
            float min_dist = float.MaxValue;
            T mostClose = default(T);

            foreach (var c in col)
            {
                if (predicate(c))
                {
                    var dist = Vector3.Distance(c.transform.position, comparer);

                    if (min_dist > dist)
                    {
                        min_dist = dist;
                        mostClose = c;
                    }
                }
            }
            return mostClose;
        }

        public static T CreateDefaultSubObject<T>(this GameObject owner, string name) where T : Component
        {
            GameObject go = new GameObject();
            go.name = name;
            T back = go.AddComponent<T>();
            go.transform.SetParent(owner.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = new Vector3(1, 1, 1);
            return back;
        }

        public static T CreateDefaultSubObject<T>(this GameObject owner, string name, GameObject model) where T : Component
        {
            GameObject go = GameObject.Instantiate(model);
            go.name = name;
            T back = go.GetComponent<T>();
            go.transform.SetParent(owner.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = new Vector3(1, 1, 1);
            return back;
        }
        public static T CreateDefaultSubObject<T>(this Transform owner, string name) where T : Component
        {
            GameObject go = new GameObject();
            go.name = name;
            T back = go.AddComponent<T>();
            go.transform.SetParent(owner.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = new Vector3(1, 1, 1);
            return back;
        }

        public static T CreateDefaultSubObject<T>(this Transform owner, string name, GameObject model) where T : Component
        {
            GameObject go = GameObject.Instantiate(model);
            go.name = name;
            T back = go.GetComponent<T>();
            go.transform.SetParent(owner.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = new Vector3(1, 1, 1);
            return back;
        }

        public static Vector2 RandomVectorDir()
        {
            var pos = RandomPosition() - RandomPosition();
            pos.Normalize();
            return pos;
        }

        public static Vector2 RandomPosition()
        {
            return new Vector2(
                RandomUnity.Range(ScreenLimits.Left_Inferior.x, ScreenLimits.Right_Superior.x),
                RandomUnity.Range(ScreenLimits.Left_Inferior.y, ScreenLimits.Right_Superior.y));
        }
        public static Vector3 RandomPositionByPoint(this Vector3 t, int radio, int _maxHeight)
        {
            Vector3 min = new Vector3(t.x - radio, 0, t.z - radio);
            Vector3 max = new Vector3(t.x + radio, t.y + _maxHeight, t.z + radio);
            return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
        }


        /// <summary>
        /// Dale una lista de tuplas con <peso, objetoRandom> y te va a devolver uno elegido al azar
        /// </summary>
        /// <param name="candidatos"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T WheelSelection<T>(List<Tuple<int, T>> candidatos)
        {
            int maxValue = 0;
            
            for (int i = 0; i < candidatos.Count; i++)
            {
                maxValue += candidatos[i].Item1;
            }

            var shuffleCandidates = candidatos.Shuffle().ToList();
            
            int rgn = Random.Range(0, maxValue);

            int sum = 0;
            
            for (int i = 0; i < shuffleCandidates.Count; i++)
            {
                sum += shuffleCandidates[i].Item1;

                if (sum > rgn)
                    return shuffleCandidates[i].Item2;
            }

            return default;
        }
        
        public static int GetAngleFromVector(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            int angle = Mathf.RoundToInt(n);

            return angle;
        }

        //public static Vector3 GetPointOnBezierCurve(BezierPoint ini, BezierPoint final, float value)
        //{
        //    Vector3 a = Vector3.Lerp(ini.transform.position, ini.son.transform.position, value);
        //    Vector3 b = Vector3.Lerp(ini.son.transform.position, final.son.transform.position, value);
        //    Vector3 c = Vector3.Lerp(final.son.transform.position, final.transform.position, value);

        //    Vector3 d = Vector3.Lerp(a, b, value);
        //    Vector3 e = Vector3.Lerp(b, c, value);
        //    Vector3 pointOnCurve = Vector3.Lerp(d, e, value);

        //    return pointOnCurve;
        //}
        //public static Quaternion GetRotatioOnBezierCurve(BezierPoint ini, BezierPoint final, float value)
        //{
        //    Quaternion a = Quaternion.Lerp(ini.transform.rotation, ini.son.transform.rotation, value);
        //    Quaternion b = Quaternion.Lerp(ini.son.transform.rotation, final.son.transform.rotation, value);
        //    Quaternion c = Quaternion.Lerp(final.son.transform.rotation, final.transform.rotation, value);

        //    Quaternion d = Quaternion.Lerp(a, b, value);
        //    Quaternion e = Quaternion.Lerp(b, c, value);
        //    Quaternion pointOnCurve = Quaternion.Lerp(d, e, value);

        //    return pointOnCurve;
        //}
    }
}

