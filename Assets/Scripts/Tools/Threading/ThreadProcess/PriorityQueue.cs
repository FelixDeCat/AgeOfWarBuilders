using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Tools.UnityThread
{
    public enum Priority { error, high, low, med }

    public class PriorityQueue<T> where T : ThreadObject
    {
        Dictionary<string, T> uniques = new Dictionary<string, T>();
        Dictionary<Priority, Queue<T>> elements;

        bool hassomething;
        public bool HasSomething { get { return hassomething; } }

        bool inProcess;
        public bool InProcess { get { return inProcess; } }

        public PriorityQueue()
        {
            elements = new Dictionary<Priority, Queue<T>>();
            elements.Add(Priority.high, new Queue<T>());
            elements.Add(Priority.med, new Queue<T>());
            elements.Add(Priority.low, new Queue<T>());
            inProcess = false;
        }

        public void OpenProcess() { inProcess = true; }
        public void CloseProcess() { inProcess = false; }

        public void Add_ElementByPriority(T obj, Priority priority)
        {
            string unique_key = obj.Key_Unique_Process;

            if (unique_key != "null")
            {
                if (!uniques.ContainsKey(unique_key))
                {
                    uniques.Add(unique_key, obj);//agrego un nuevo objeto
                }
                else
                {
                    uniques[unique_key] = obj;//piso el objeto con el nuevo

                    #region Esto se me hace que es re imperformante, cuando pueda hay que cambiarlo a una custom collection QueueList exclusiva para hacer esto

                    var queueList = elements[priority].ToList();
                    for (int i = 0; i < queueList.Count; i++)
                    {
                        if (queueList[i].Key_Unique_Process == unique_key)
                        {
                            queueList.RemoveAt(i);
                            break;
                        }
                    }
                    elements[priority] = new Queue<T>(queueList);
                    #endregion
                }
            }

            elements[priority].Enqueue(obj);
            Check();
        }

        public T Pull()
        {
            Priority pr = Priority.error;
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[IndexToPriority(i)].Count > 0)
                {
                    pr = IndexToPriority(i);
                    break;
                }
            }
            if (pr == Priority.error) { throw new Exception("Esta vacio"); }

            var elem = elements[pr].Dequeue();

            string unique_key = elem.Key_Unique_Process;
            if (unique_key != "null")
            {
                try { uniques.Remove(unique_key); } catch { }
            }

            Check();
            return elem;
        }
        public T Peek()
        {
            Priority pr = Priority.error;
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[IndexToPriority(i)].Count > 0)
                {
                    pr = IndexToPriority(i);
                    break;
                }
            }
            if (pr == Priority.error) { throw new Exception("Esta vacio"); }
            var elem = elements[pr].Peek();
            Check();
            return elem;
        }

        void Check()
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[IndexToPriority(i)].Count > 0)
                {
                    hassomething = true;
                    return;
                }
            }
            hassomething = false;
        }

        private Priority IndexToPriority(int index)
        {
            if (index == 0) return Priority.high;
            if (index == 1) return Priority.med;
            if (index == 2) return Priority.low;
            return Priority.error;
        }
    }
}


