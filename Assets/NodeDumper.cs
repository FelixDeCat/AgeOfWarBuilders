using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;
using IA_Felix;
using System.Linq;

public class NodeDumper : MonoBehaviour
{
    public float radius = 0.8f;
    float timer = 0;
    public float time_to_check = 0.1f;

    [SerializeField] LayerMask mask;

    [SerializeField] Node[] nodes = new Node[0];
    [SerializeField] Vector3 offset;

    [SerializeField] bool drawGizmos;

    public bool isAutomatic = true;

    //optimizar esto, ponerlo en el Rise y en el Death de las constriucciones

    #region Rise & Death
    public void Rise()
    {
        StartCoroutine(WaitEndFrame());
    }

    IEnumerator WaitEndFrame()
    {
        yield return new WaitForEndOfFrame();
        nodes = Physics.OverlapSphere(this.transform.position + offset, radius, mask)
                .Select(x => x.GetComponent<Node>())
                .ToArray();

        for (int i = 0; i < nodes.Length; i++)
        {
            Debug.Log("Apagando...  " + nodes[i].gameObject.name);
            nodes[i].TurnOff();
        }

        LocalNodeHandler.instance.Refresh();
        yield return null;
    }
    public void Death()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            Debug.Log("Prendiendo...  " + nodes[i].gameObject.name);
            nodes[i].TurnOn();
        }
    }
    #endregion

    #region Automatico Menos Performante
    private void Update()
    {
        if (!isAutomatic) return;

        if (timer < time_to_check)
        {
            timer = timer + 1 * Time.deltaTime;
        }
        else
        {
            Debug.Log("Entra aca");
            timer = 0;
            nodes = Physics.OverlapSphere(this.transform.position + offset, radius, mask)
                .Select(x => x.GetComponent<Node>())
                .ToArray();

            for (int i = 0; i < nodes.Length; i++)
            {
                Debug.Log("Apagando...  " + nodes[i].gameObject.name);
                nodes[i].TurnOff();
            }
        }
    }
    private void OnDestroy()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].TurnOn();
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].TurnOn();
        }
    }
    #endregion

    #region GIZMOS
    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position + offset, radius);
        }
    }
    #endregion
}
