using System;
using FSM;

public class PatrolState : MonoBaseState {

    private Player _player;
    
    
    private void Awake() {
        _player = FindObjectOfType<Player>();
    }
    
    public override void UpdateLoop()
    {
        //TODO: patrullo
    }

    public override IState ProcessInput() {
        var sqrDistance = (_player.transform.position - transform.position).sqrMagnitude;

        if (sqrDistance < 100f) {
            return Transitions["OnChaseState"];
        }

        return this;
    }
}
