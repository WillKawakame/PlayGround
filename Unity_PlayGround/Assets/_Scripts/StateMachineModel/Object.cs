using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour {

    Base _currentState;
    StateFactory _state => new StateFactory(this);


    void Start() {
        _currentState = _state.State_();
        _currentState.EnterState(this);
    }


    void Update() {
        _currentState.UpdateState(this);
    }


    public void SwitchStates(Base state) {
        _currentState = state;
        _currentState.EnterState(this);
    }
    
}
