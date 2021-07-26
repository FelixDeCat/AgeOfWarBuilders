using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindTarget : MonoBehaviour
{
    bool find;
    [SerializeField] ThreatReceptor threatReceptor;

    
    [SerializeField] PathFinder pathfinder;

    float timer_recalculate;
    float time_to_recalculate = 0.5f;

    public void BeginFinding()
    {
        if (!find)
        {
            timer_recalculate = 0;
        }
        find = true;
    }
    public void StopFinding()
    {
        find = false;
        timer_recalculate = 0;
    }

    LivingEntity currentarget;
    public LivingEntity Target =>  currentarget;
    public float DistanceToTarget => Vector3.Distance(currentarget.transform.position, this.transform.position);
    public Vector3 DirectionToTarget => (currentarget.transform.position - this.transform.position).normalized;

    bool havetarget = false;
    public bool IHaveATarget => havetarget;

    public void TickFinding()
    {
        if (find)
        {
            if (timer_recalculate < time_to_recalculate)
            {
                timer_recalculate = timer_recalculate + 1 * Time.deltaTime;
            }
            else
            {
                timer_recalculate = 0;

                var threat = threatReceptor
                    .Threats
                    .FirstOrDefault();

                if (threat != null)
                {
                    currentarget = threat.GetComponent<LivingEntity>();
                    if (currentarget == null)
                    {
                        //pequeño fix cabeza, si no encuentro el living, lo debe tener mi parent
                        currentarget = threat.transform.parent.GetComponent<LivingEntity>();
                    }
                }

                if (currentarget == null)
                {
                    if (!pathfinder.CanExecute(SceneReferences.Player.transform.position))//si veo que no puedo llegar al player, voy a la base
                    {
                        currentarget = SceneReferences.myCenterBase.GetComponent<LivingEntity>();
                    }
                    else
                    {
                        currentarget = SceneReferences.Player.GetComponent<LivingEntity>();
                    }
                }

                havetarget = currentarget != null; //si fuera null no tengo target
            }
        }
    }
}
