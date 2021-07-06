using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class IA2Ejercicios1806 : MonoBehaviour
{
    /*
     Escriba el código de una función que genere números enteros a partir de 0.
    Escriba el código de una función que genere números pares.
    Escriba el código de una función que genere números a partir de un número inicial dado, y genere el siguiente número en base a una función pasada por parámetro.
    Escriba el código de una función que genere cosas a partir de un caso inicial dado, y genere la siguiente cosa en base a una función pasada por parámetro.
    Escriba el código de BFS a modo de Generator.
         */

    IEnumerable<int> Generate()
    {
        int i = 0;
        while (i < Int32.MaxValue)
        {
            yield return i;
            i++;
        }
    }

    IEnumerable<int> GeneratePares()
    {
        int i = 0;
        while (i <= Int32.MaxValue -1)
        {
            yield return i;
            i += 2; 
        }
    }

    IEnumerable<int> Generate(int seed, Func<int, int> gen)
    {
        int n = seed;
        while (true)
        {
            yield return n;
            n = gen(n);
        }
    }

    IEnumerable<T> Generate<T>(T seed, Func<T, T> gen)
    {
        var i = seed;
        while (true)
        {
            yield return i;
            i = gen(i);
        }
    }
}

public static class BFS
{

    /// <summary>
    /// Calculates a path using Breadth First Search. See more at https://github.com/kgazcurra/ProLibraryWiki/wiki/BFS
    /// </summary>
    /// <param name="start">The node where it starts calculating the path</param>
    /// <param name="isGoal">A function that, given a node, tells us whether we reached or goal or not</param>
    /// <param name="explode">A function that returns all the near neighbours of a given node</param>
    /// <typeparam name="T">Node type</typeparam>
    /// <returns>Returns a path from start node to goal</returns>

    public static IEnumerable<T> CalculatePathBFS<T>(T start, Func<T, bool> isGoal, Func<T, IEnumerable<T>> explode)
    {
        var queue = new Queue<T>();


        yield return start;

        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var dequeued = queue.Dequeue();

            if (isGoal(dequeued)) yield break;

            var toEnqueue = explode(dequeued);
            foreach (var n in toEnqueue)
            {
                yield return n;
                queue.Enqueue(n);
            }
        }
    }
}