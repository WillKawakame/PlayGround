using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Telekinesis : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private PlayerInput playerInput => GetComponent<PlayerInput>();
    public LayerMask probsTelekinesis;
    private RaycastHit currentProp;
    private InputAction teleStart => playerInput.actions["TeleStart"]; 
    private InputAction teleEnd => playerInput.actions["TeleEnd"];
    private int priorityBoostAmount = 10;
    

    [HideInInspector]
    public bool canPull = true;

    [Header("Particles")]
    public GameObject holdParticle;

    void Update() => CastProps();
    

    private void CastProps() {
        RaycastHit hit;
        // Find the prop
        bool traceProps = Physics.SphereCast( transform.position, 0.5f, transform.forward, out hit, 50, probsTelekinesis );

        
        // If found the prop and !State.Pull
        if ( traceProps && canPull) {
            // Outline and set the prop 
            if( currentProp.transform == null ) OutlineProb( hit );
            else if( currentProp.transform.gameObject.GetComponent<Outline>() == null ) OutlineProb( hit );
            
            if( currentProp.transform.gameObject.GetComponent<PropTele>().teleState == TeleStates.Default ){
                if ( teleStart.triggered ) {
                    StartAim();
                    GameObject hParticle = Instantiate(holdParticle, Vector3.zero, Quaternion.identity);
                    hParticle.GetComponent<HoldParticle>().t = currentProp.transform;
                    currentProp.transform.gameObject.GetComponent<PropTele>().teleState = TeleStates.Pull;
                    canPull = false;
                }
            }
        }

        // Destroy outline
        if( currentProp.transform != hit.transform ) {
            if( currentProp.transform.gameObject.GetComponent<Outline>() != null ) Destroy( currentProp.transform.gameObject.GetComponent<Outline>() );
        }

        // If had pulled the prop you can push it
        if(currentProp.transform != null && currentProp.transform.gameObject.GetComponent<PropTele>().teleState == TeleStates.Pull){
            if( teleEnd.triggered ){
                CancelAim();
                Destroy(GameObject.FindGameObjectWithTag("t"));
                currentProp.transform.gameObject.GetComponent<PropTele>().teleState = TeleStates.Push;
                canPull = true;
            }
        }
    }

    private void OutlineProb( RaycastHit hit ){
        currentProp = hit;
        if( hit.transform.gameObject.GetComponent<Outline>() == null ){
            hit.transform.gameObject.AddComponent<Outline>();
        }
    }

    private void StartAim(){
        virtualCamera.Priority += priorityBoostAmount;
    }

    private void CancelAim(){
        virtualCamera.Priority -= priorityBoostAmount;
    }
}
