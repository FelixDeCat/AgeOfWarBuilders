using System;
using System.Collections;
using UnityEngine;

namespace Tools.UnityThread
{

    public class ThreadObject
    {
        IEnumerator process;
        Action local_finish;
        string name_process = "process";
        string keyUniqueProcess = "null";
        public string Key_Unique_Process { get { return keyUniqueProcess; } }
        public string Name_Process { get { return name_process; } }

        public ThreadObject(IEnumerator _process, string _name = "process", string _keyUniqueProcess = "null", Action _local_finish = null)
        {
            process = _process;
            local_finish = _local_finish;
            name_process = _name;
            keyUniqueProcess = _keyUniqueProcess;
        }

        public override bool Equals(object obj)
        {
            return ((ThreadObject)obj).Key_Unique_Process == this.Key_Unique_Process;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public IEnumerator Process()
        {
            yield return process;
            if (local_finish != null) local_finish.Invoke();
        }
    }
}