using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.Extensions;
using System.Linq;

//este sistema de Node solo funciona con 4 conexiones
//es mas fácil para zafar ahora

namespace IA_Felix
{
    [ExecuteInEditMode]
    public class Node : MonoBehaviour
    {

        public HashSet<Node> vecinos;
        Action OnRecalculate;
        Rigidbody rig;
        public NodeCost costs;
        float original_external_cost;


        [SerializeField] bool connected = true;
        public bool IsConnected => connected;
        public void SetConnectionState(bool connected) => this.connected = connected;
        public void SetExternalCost(int weight) => costs.external_weight = weight;
        public void ResetExternalCost() => costs.external_weight = original_external_cost;

        public bool OnEditMode;

        [Header("to debug")]
        public List<Node> vecinosDebug;
        public Node parent;

        [Header("Config Node")]
        public float radius_to_find = 5;
        public float distance_to_delete = 0.2f;

        [Header("Gizmos")]
        public NodeRender render;

        [Header("Grid & Queries")]
        public GridComponent myGridComponent;
        public CircleQuery circleQuery;

        [Header("Mascaras")]
        public LayerMask mask_hit_floor;
        public LayerMask detectableLayers;

        public void RefreshReConnections()
        {
            vecinos.RemoveWhere(x => !x.CheckCompatibility(this) && !x.IsConnected);
            vecinosDebug = new List<Node>(vecinos);
        }
        public bool CheckCompatibility(Node suitor)
        {
            foreach (var n in vecinos)
            {
                if (n.Equals(suitor)) return true;
            }
            return false;
        }

        #region [GRID] TurnOn & TurnOff
        public void TurnOn()
        {
            ResetExternalCost();
            
            //SetConnectionState(true);
           // myGridComponent.Grid_Rise();
            OnRecalculate?.Invoke();
        }
        public void TurnOff()
        {
            SetExternalCost(int.MaxValue);
            //SetConnectionState(false);
            //myGridComponent.Grid_Death();
            OnRecalculate?.Invoke();
        }
        #endregion

        public void OnStart(Action OnRefreshCallBack)
        {
            render.Init(gameObject);
            OnRecalculate = OnRefreshCallBack;
            rig = this.GetComponent<Rigidbody>();
            original_external_cost = costs.external_weight;

            if (!OnEditMode)
            {
                myGridComponent = this.GetComponent<GridComponent>();
                myGridComponent.Grid_Initialize(this.gameObject);
                myGridComponent.Grid_Rise();

                circleQuery = this.GetComponent<CircleQuery>();
                circleQuery.Configure(rig.transform);

                Invoke("Execute", 1f);
            }
            else
            {
                Execute();
            }
        }

        public void ClampToFloor()
        {
            RaycastHit hit;

            if (Physics.Raycast(this.transform.position + this.transform.up * 10, this.transform.up * -1, out hit, 30, mask_hit_floor))
            {
                this.transform.position = hit.point;
            }
        }

        public void Execute()
        {
            if (!IsConnected)
            {
                foreach (var v in vecinos) v.Desconnect(this);
                vecinos.Clear();
                vecinosDebug.Clear();
                return;
            }

            if (!OnEditMode)
            {
                var v = FindVecinosByQuery(this);

                vecinos = new HashSet<Node>(v);
            }
            else
            {
                vecinos = new HashSet<Node>(FindVecinosByRadius(this));
            }

            vecinosDebug = new List<Node>(vecinos);
        }


        public void Desconnect(Node node)
        {
            vecinos.Remove(node);
            vecinosDebug = new List<Node>(vecinos);
        }

        private void OnDrawGizmos()
        {
            if (render != null) render.Draw(this.transform.position, vecinos, radius_to_find);
        }


        public void ShutDownRigidbody()
        {
            rig.isKinematic = true;
            rig.detectCollisions = false;
        }

        public HashSet<Node> FindVecinosByRadius(Node MyNode)
        {
            return new HashSet<Node>(MyNode.FindInRadius(radius_to_find, detectableLayers, x => x.IsConnected));
        }

        public IEnumerable<Node> FindVecinosByQuery(Node MyNode)
        {
            return circleQuery.Query() //IA2-P2 [SpatialGrid - Node Finder]
                 .OfType<GridComponent>()
                 .Select(x => x.Grid_Object.GetComponent<Node>()) //IA2-P3 [Select]
                 .Where(x => x != MyNode && x.IsConnected) //IA2-P3 [Where]
                 .Where(x => 
                 {
                     Vector3 dir = x.transform.position - transform.position;
                     if (!Physics.Raycast(transform.position, dir, out RaycastHit ray, 10, detectableLayers)) return false;
                     else return true;
                 }); 
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

    [System.Serializable]
    public struct NodeCost
    {
        public float cost;
        public float fitness;
        public float heuristic;
        public float external_weight;
    }


    #region GIZMOS
    [System.Serializable]
    public class NodeRender
    {
        public bool draw_gizmos = false;
        public bool draw_radius;
        public bool draw_neighbors;
        public Color connectionColor;
        public float multiplier_up_vector_gizmo_dist = 0.1f;

        public void Init(GameObject go)
        {
            render = go.GetComponent<Renderer>();
            //if(render) render.enabled = false;
        }
        Renderer render;
        public void PintarRojo() { if (render) render.material.color = Color.red; }
        public void PintarNegro() { if (render) render.material.color = Color.black; }
        public void PintarVerde() { if (render) render.material.color = Color.green; }
        public void PintarBlanco() { if (render) render.material.color = Color.white; }

        public void ShutDownRender()
        {
            if (render) render.enabled = false;
            draw_gizmos = false;
        }

        public void Draw(Vector3 myPos, List<Node> col, float radius)
        {
            if (!draw_gizmos) return;
            if (draw_radius) Gizmos.DrawWireSphere(myPos, radius);
            if (draw_neighbors) Gizmos.color = connectionColor;
            if (draw_neighbors) foreach (var n in col) Gizmos.DrawLine(myPos + Vector3.up * multiplier_up_vector_gizmo_dist, n.transform.position + Vector3.up * multiplier_up_vector_gizmo_dist);
        }
        public void Draw(Vector3 myPos, IEnumerable<Node> col, float radius)
        {
            if (!draw_gizmos) return;
            if (draw_radius) Gizmos.DrawWireSphere(myPos, radius);
            if (draw_neighbors)
            {
                Gizmos.color = connectionColor;

                if (col == null) return;
                foreach (var n in col)
                {
                    Vector3 HeightOffset = Vector3.up * multiplier_up_vector_gizmo_dist;

                    Vector3 IntitialPoint = myPos + HeightOffset;

                    var distance = Vector3.Distance(n.transform.position, myPos) / 2;
                    var dir = (n.transform.position - myPos).normalized;
                    var destinyPoint = myPos + dir * distance;

                    Vector3 FinalPoint = destinyPoint + HeightOffset;

                    if (render.gameObject.GetComponent<Node>().costs.external_weight == 2)
                    {
                        Gizmos.color = new Color(1.0f, 0.64f, 0.0f);
                    }
                    if (render.gameObject.GetComponent<Node>().costs.external_weight >= 3)
                    {
                        Gizmos.color = Color.red;
                    }
                    if (render.gameObject.GetComponent<Node>().costs.external_weight > 10)
                    {
                        Gizmos.color = Color.black;
                    }
                    Gizmos.DrawLine(IntitialPoint, destinyPoint);
                }
            }
        }
    }
    #endregion
}
