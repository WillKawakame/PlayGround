using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropTele : MonoBehaviour
{
    public GameObject player => GameObject.FindGameObjectWithTag("TelePoint");
    private Rigidbody rb => GetComponent<Rigidbody>();


    private Vector3 endPosition;
    private Vector3 startPosition;

    public TeleStates teleState = TeleStates.Default;


    private float desiredDuration = 0.1f;
    private float elapsedTime;
    private bool startLiftOff = true;
    public int pushForce = 50;


    [SerializeField]
    private AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));



    private void Start() {
        startPosition = transform.position;
        endPosition = transform.position + new Vector3(0, 1.5f, 0);
    } 
        
    private void Update(){
        State( teleState );
    }


    public void State(TeleStates teleState){
        switch(teleState){
            case TeleStates.Default:
                break;
            case TeleStates.Pull:
                if (startLiftOff) LiftOff();
                else ReachCharacter();
                break;
            case TeleStates.Push:
                Push();
                break;
            case TeleStates.Done:
                break;
        }
    }

    private void LiftOff(){
        // Animation Time to Complete
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / desiredDuration;
        //LiftOff
        transform.position = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(percentageComplete));
        Debug.Log("Loading");

        // Completede
        if (percentageComplete >= 1){ 
            startLiftOff = false;
        }

    }

    private void ReachCharacter() {
        // Reaching Character Every
        rb.useGravity = false;
        rb.drag = 8;

        Vector3 direction = player.transform.position - transform.position;
        direction = Vector3.ClampMagnitude(direction, 10000f);
        float force = Mathf.Clamp(rb.mass, 1, 10);

        rb.AddForce(direction * force, ForceMode.Impulse);
        RandomRotate();
    }

    private void Push(){
        rb.useGravity = true;
        rb.drag = 0;
        rb.AddForce(Camera.main.transform.forward * pushForce, ForceMode.Impulse);
        teleState = TeleStates.Done;
    }
    
    void RandomRotate() {
        rb.AddTorque(Vector2.up * Random.Range(-1, 1), ForceMode.Impulse);
        rb.AddTorque(Vector2.right * Random.Range(-1, 1), ForceMode.Impulse);

        rb.AddForce((Vector3.up * Random.Range(-1, 1))/12, ForceMode.Impulse);
        rb.AddForce((Vector3.right * Random.Range(-1, 1))/8, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other) {
        if(teleState == TeleStates.Done){
            StartCoroutine(DestroyGameObject());
        }
    }

    private void OnDestroy() {
        teleState = TeleStates.Default;
    }

    IEnumerator DestroyGameObject(){
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
