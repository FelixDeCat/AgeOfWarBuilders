using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using System;

namespace Tools.UnityThread
{
    public class ThreadHandler : MonoBehaviour
    {
        static ThreadHandler instance;
        private void Awake() => instance = this;

        Queue<ThreadObject> process_queue = new Queue<ThreadObject>();
        Dictionary<string, ThreadObject> uniques = new Dictionary<string, ThreadObject>();

        PriorityQueue<ThreadObject> processes = new PriorityQueue<ThreadObject>();

        [SerializeField] float allowedTime = 0.01f;

        public Action EndProcess = delegate { };
        float time_delay_for_connected_callbacks = 0.01f;
        bool cooldown = false;
        float timer;


        bool useLoadScreen;
        [SerializeField] TextMeshProUGUI CurrentProcess = null;
        public static void AuxiliarDebug(string val) { instance?.AuxDebug(val); }

        public static void EnqueueProcess(ThreadObject obj, Action callbackEndProcess = null, Priority priority = Priority.error)
        {
            instance?.AddThreadObject(obj, callbackEndProcess, priority);
        }

        void AuxDebug(string s) { CurrentProcess.text = "Processing... " + s; }

        ThreadObject hot_obj;
        Priority hot_priority;

        void AddThreadObject(ThreadObject obj, Action callbackEndProcess = null, Priority priority = Priority.error)
        {
            // ¿HOT? ¿por que me guardo esto en una variable local?
            // esto se debe a que probablemente hayan Threads con Callback que esten seguidos uno de otro
            // Si mi proximo Proceso tiene un callback y yo tambien tengo uno... primero voy a esperar a 
            // que termine el CD para continuar con la ejecucion
            hot_obj = obj;
            hot_priority = priority;


            // si este proceso viene acompañado de un Callback con algo adentro, piso el anterior
            // El if sirve para que los procesos sin callback no Nulleen el callback principal
            if (callbackEndProcess != null)
            {
                EndProcess = callbackEndProcess;

                // ¿que pasaria si mi proceso anterior hace que vuelva a entrar acá?
                // ¿Y si Justo el proceso anterior tiene un Callback y yo tambien?
                // eso generaria muchos problemas...
                // por lo que pausamos la ejecucion con un cooldown
                // termina el cooldown y seguimos fritando papas fritas
                if (!cooldown)
                {
                    BeginToProcess();
                }
            }
            else
            {
                //esto es porque a veces a veces tengo el cd activo y entro
                cooldown = false;
                timer = 0;
                BeginToProcess();
            }
        }

        private void Update()
        {
            if (cooldown)
            {
                if (timer < time_delay_for_connected_callbacks)
                {
                    timer = timer + 1 * Time.deltaTime;
                }
                else
                {
                    timer = 0;
                    BeginToProcess();
                    cooldown = false;
                }
            }
        }

        void BeginToProcess()
        {
            if (hot_obj == null) return;
            processes.Add_ElementByPriority(hot_obj, hot_priority);

            hot_obj = null;
            hot_priority = Priority.error;

            if (!processes.InProcess)
            {
                processes.OpenProcess();
                StartCoroutine(Process());
            }
        }

        float startTime = 0;
        IEnumerator Process()
        {
            while (processes.HasSomething)
            {
                var aux = processes.Peek();
                CurrentProcess.text = "Processing... " + aux.Name_Process;
                yield return aux.Process();


                if (Time.realtimeSinceStartup - startTime > allowedTime)
                {
                    yield return null;
                }

                processes.Pull();
            }

            Debug.Log("Ya no tengo nada para procesar");

            cooldown = true;
            EndProcess?.Invoke();
            EndProcess = delegate { };


            Debug.LogWarning("Close Process");
            processes.CloseProcess();

            yield return null;
        }
    }
}

