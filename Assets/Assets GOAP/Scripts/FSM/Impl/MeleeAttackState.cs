using System;
using FSM;
using UnityEngine;

public class MeleeAttackState : MonoBaseState {
    
    public override event Action OnNeedsReplan;

    public float attackRate = .5f;
    
    public float maxDistance = 1.5f;
    
    private Player _player;

    private float _lastAttackTime;


    private void Awake() {
        _player = FindObjectOfType<Player>();
    }

    public override void UpdateLoop() {
        if (Time.time >= _lastAttackTime + attackRate) {
            _lastAttackTime = Time.time;
            Debug.Log("Ataco");
        }
        
        var sqrDistance = (_player.transform.position - transform.position).sqrMagnitude;
        
        if (sqrDistance > maxDistance * maxDistance) {
            OnNeedsReplan?.Invoke();
        }
    }

    public override IState ProcessInput() {
        return this;
    }
}