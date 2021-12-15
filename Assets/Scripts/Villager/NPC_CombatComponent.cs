using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;
using Tools.GameObjectTools;

public class NPC_CombatComponent : MonoBehaviour
{
    public float time_to_pulse = 1f;

    public float distance_to_begin_combat = 5f;
    public float distance_to_follow = 8f;

    Transform myTransform;
    Action OnEnterCombat;
    Action OnExitCombat;
    Villager own;

    public AnimEvent events_by_animations;
    public Sensor sensor;

    Pulse pulse_detection;

    public CircleQuery query;

    Action<Vector3> goTo;

    Enemy current;
    public Enemy Current => current;
    Transform target_position;
    bool IHaveEnemy => current != null;
    Vector3 EnemyPosition => current.transform.position;
    Vector3 EnemyDirection => (EnemyPosition - myTransform.position).normalized;
    bool beginCombat;
    bool combat_has_started;
    bool attackorder;

    public void Configure(Transform _myTransform, Action _OnEnterCombat, Action _OnExitCombat, Action<Vector3> _GoTo)
    {
        query.Configure(_myTransform);
        myTransform = _myTransform;
        OnEnterCombat = _OnEnterCombat;
        OnExitCombat = _OnExitCombat;
        pulse_detection = new Pulse(PulseTick, time_to_pulse);
        goTo = _GoTo;
        own = myTransform.GetComponent<Villager>();

        sensor.AddCallback_OnTriggerEnter(ReceiveAttackedEnemy);
        events_by_animations.ADD_ANIM_EVENT_LISTENER("OnAttack", EV_ATTACK);
        events_by_animations.ADD_ANIM_EVENT_LISTENER("OnENDAttack", EV_END_ATTACK);

        
    }

    public void EV_ATTACK()
    {
        sensor.On();
    }
    public void EV_END_ATTACK()
    {
        sensor.Off();
    }

    public void ReceiveAttackedEnemy(GameObject go)
    {
        Debug.Log("GAMEOBJECT " + go.name);
        
        var enemy = go.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.ReceiveDamage(10);
        }
    }

    public void PulseTick() //cada 1 seg
    {
        var enem = query.Query() //IA2-P2 [SpatialGrid - NPC_Combat_Component]
            .OfType<GridComponent>()
            .Where(x => x.Grid_Object.GetComponent<Enemy>()) //IA2-P3 [Where]
            .Select(x => x.Grid_Object.GetComponent<Enemy>()) //IA2-P3 [Select]
            .OrderBy(x => (transform.position - x.transform.position).sqrMagnitude) //IA2-P3 [OrderBy]
            .FirstOrDefault();

        if (enem == null)
        {
            //Debug.Log("No Hay enemigos cerca");
            beginCombat = false;
            current = null;
            if (combat_has_started)
            {
                combat_has_started = false;
                OnExitCombat.Invoke();
            }
        }
        else
        {
           // Debug.Log("Encontre un enemigo");
            if (enem != current)
            {
                current = enem;
                beginCombat = true;
                OnEnterCombat.Invoke();
                combat_has_started = true;
            }
        }
    }

    public void Tick()
    {
        
    }

    public void GoToAttack()
    {
        attackorder = true;
    }

    private void Update()
    {
        pulse_detection?.Tick(Time.deltaTime);

        if (!IHaveEnemy || !attackorder) return;

        //var finder = own.targetFinder;
        var lookat = own.smoothLookAt;

        //own.StopPathFinder();

        if (own.inDanger)
        {
            //si esta en rango pero no es taaanto peligro, no hago nada
            if (Vector3.Distance(myTransform.position, EnemyPosition) > distance_to_follow)
            {
                own.PlayPathFinder();
                own.GoToPositionWithPathFinder(EnemyPosition);
                own.view.PLAY_ANIM_Idle();
            }
            else
            {
                Debug.Log("Esta cerca");
                own.StopPathFinder();

                lookat.SetDirection(EnemyDirection);
                lookat.Look(Time.deltaTime);

                if (Vector3.Distance(myTransform.position, EnemyPosition) < distance_to_begin_combat)
                {
                    Debug.Log("Distancia para atacar");

                    own.view.PLAY_ANIM_Attack();
                    //si lo tengo en rango y lo suficientemente cerca coomo para pegarle
                }
                else
                {
                    Debug.Log("Me tengo que acercar");
                    //si esta en rango y esta lo suficientemente cerca, pero no tanto como para pegarle, lo voy a buscar
                    own.view.PLAY_ANIM_Walk();
                    own.StopPathFinder();
                    own.follow_component.Tick_Follow(Time.deltaTime, EnemyDirection);
                }
            }
        }
    }

    #region PulseThings
    public void BeginSensor() => pulse_detection.Begin();
    public void StopSensor() => pulse_detection.End();
    
    #endregion

}
