using System.Collections.Generic;
using FSM;
using UnityEngine;

public class ChaseState : MonoBaseState {

    public float speed = 2f;
    
    public float rangeDistance = 5;
    public float meleeDistance = 1.5f;
    
    private Player _player;
    
    
    private void Awake() {
        _player = FindObjectOfType<Player>();
    }

    public override void UpdateLoop() {
        var dir = (_player.transform.position - transform.position).normalized;

        transform.position += dir * (speed * Time.deltaTime);
    }

    public override IState ProcessInput() {
        var sqrDistance = (_player.transform.position - transform.position).sqrMagnitude;
        
        if (sqrDistance < meleeDistance * meleeDistance && Transitions.ContainsKey("OnMeleeAttackState")) {
            return Transitions["OnMeleeAttackState"];
        }

        return this;
    }
}