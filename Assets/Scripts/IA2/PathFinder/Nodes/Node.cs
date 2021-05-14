using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

//este sistema de Node solo funciona con 4 conexiones
//es mas fácil para zafar ahora

namespace IA_Felix
{
    [ExecuteInEditMode]
    public class Node : MonoBehaviour
    {

        public List<Node> vecinos;

        public List<Node> toEliminate;
        public Node parent;

        public NodeCost costs;
        public NodeRender render;
        public NodeFinder finder;

        public LayerMask mask_hit_floor;

        public bool execute;

        public void CheckIfNeighborsCanSeeMe()
        {
            var toRemove = new List<Node>();
            foreach (var n in vecinos) if (!n.CheckCompatibility(this)) toRemove.Add(n);
            toEliminate = toRemove;
            foreach (var n in toRemove) vecinos.Remove(n);
        }
        public bool CheckCompatibility(Node suitor)
        {
            foreach (var n in vecinos)
            {
                if (n.Equals(suitor)) return true;
            }
            return false;
        }

        public void Awake()
        {
            finder.Init(GetComponent<Rigidbody>());
            render.Init(gameObject);

            Execute();
        }

        public void OnStart()
        {
            //si o si esto no tiene que ejecutarse luego del awake, xq sino hay algunos nodos que no los detecta el overlapshere
            vecinos = finder.FindVecinos(this);


        }

        public void ClampToFloor()
        {
            RaycastHit hit;

            if (Physics.Raycast(this.transform.position + this.transform.up * 10 , this.transform.up * -1, out hit, 30, mask_hit_floor))
            {
                this.transform.position = hit.point;
            }
        }

        public void Execute()
        {
            vecinos = finder.FindVecinos(this);
        }

        public void ShutDown()
        {
            //esto si o si... al terminar todo... no meterlo dentro del mismo start xq sino apaga los rigidbodys y en el proximo nodo no lo detecta
            //finder.ShutDownRigidbody();
        }

        private void Update()
        {
            if (execute)
            {
                execute = false;
                Execute();
            }
        }

        private void OnDrawGizmos()
        {
            if (render != null) render.Draw(this.transform.position, vecinos, finder.radius);
        }
    }

    [System.Serializable]
    public struct NodeCost
    {
        public float cost;
        public float fitness;
        public float heuristic;
    }

    [System.Serializable]
    public class NodeRender
    {
        public bool gizmos = false;
        public bool draw_radius;
        public bool draw_neighbors;
        public Color connectionColor;
        public float multiplier_up_vector_gizmo_dist = 0.1f;

        public void Init(GameObject go) {
            render = go.GetComponent<Renderer>();
            //if(render) render.enabled = false;
        }
        Renderer render;
        public void PintarRojo() { if(render) render.material.color = Color.red; }
        public void PintarNegro() { if (render) render.material.color = Color.black; }
        public void PintarVerde() { if (render) render.material.color = Color.green; }
        public void PintarBlanco() { if (render) render.material.color = Color.white; }

        public void ShutDownRender()
        {
            if (render) render.enabled = false;
            gizmos = false;
        }

        public void Draw(Vector3 myPos, List<Node> col, float radius)
        {
            if (!gizmos) return;
            if (draw_radius) Gizmos.DrawWireSphere(myPos, radius);
            if (draw_neighbors) Gizmos.color = connectionColor;
            if (draw_neighbors) foreach (var n in col) Gizmos.DrawLine(myPos+Vector3.up* multiplier_up_vector_gizmo_dist, n.transform.position + Vector3.up* multiplier_up_vector_gizmo_dist);
        }
    }

    [System.Serializable]
    public class NodeFinder
    {
        Rigidbody rig;
        public float radius = 5;
        public float distance_to_delete = 0.2f;

        public LayerMask detectableLayers;

        public List<Node> encontrados = new List<Node>();

        public Collider[] colliders;

        public void ShutDownRigidbody()
        {
            rig.isKinematic = true;
            rig.detectCollisions = false;

        }

        public void Init(Rigidbody _rig) { rig = _rig; }

        public List<Node> FindVecinos(Node MyNode)
        {
            return MyNode.FindInRadius(radius, detectableLayers);
        }

        public void EliminateMostClose(Node MyNode)
        {
            var to_eliminate = MyNode.FindInRadius(distance_to_delete, detectableLayers);
            for (int i = 0; i < to_eliminate.Count; i++)
            {
                MonoBehaviour.DestroyImmediate(to_eliminate[i].gameObject);
            }
            to_eliminate.Clear();
        }
    }
}
