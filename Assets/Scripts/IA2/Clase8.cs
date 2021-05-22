using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Test
{
    string Name = "JigSaw";
    int years = 70;
    public int[] col = new int[] { 1, 2, 3, 4, 5, 6, 7 };
    public string[] names = new string[] { "pepe", "juana", "marta", "sofia", "tantatella", "apollo", "laika" };
}

public static class Clase8
{
    /*
        Select()
        Take()
        TakeWhile()
        SelectMany()
    */

    public static void Test()
    {
        //Test[] tests = new Test[3];

       // var selected = tests.SelectMany(x => x.col);
    }

    //public static IEnumerable<R> Select<T,R>(this IEnumerable<T> elem, Func<T,R> selector)
    //{
    //    foreach (var e in elem)
    //    {
    //        yield return selector(e);
    //    }
    //}
    //public static IEnumerable<T> Take<T>(this IEnumerable<T> elem, int cant = 1)
    //{
    //    int aux = 0;

    //    foreach (var e in elem)
    //    {
    //        aux++;

    //        if (aux > cant)
    //        {
    //            yield return e;
    //        }
    //        else
    //        {
    //            break;
    //        }
    //    }
    //}
    //public static IEnumerable<T> TakeWhile<T>(this IEnumerable<T> elem, Func<T,bool > pred)
    //{
    //    while (pred(elem.GetEnumerator().Current))
    //    {
    //        yield return elem.GetEnumerator().Current;
    //        elem.GetEnumerator().MoveNext();
    //    }
    //}
}
