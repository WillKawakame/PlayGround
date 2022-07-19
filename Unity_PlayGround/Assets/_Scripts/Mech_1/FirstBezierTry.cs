using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FirstBezierTry : MonoBehaviour
{
    public Vector3 startPos; 
    public float time;
    public Transform endPos;
    public GameObject sphere;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime / 1.5f;

        if(time < 1){
            transform.position = BezierFunction(startPos, endPos.position, time);
        }else{
            startPos = new Vector3(endPos.position.x, endPos.position.y, endPos.position.z);
            endPos.position = new Vector3(endPos.position.x, endPos.position.y, endPos.position.z + 10);
            time = 0;
        }
    }

    public Vector3 BezierFunction(Vector3 initialPoint, Vector3 hitPoint, float time){
        float dif = hitPoint.z - initialPoint.z;
        float diff = (2/dif) * dif;
        float time1 = time;

        //Central Point of Circular Movement
        Vector3 p2Mid = new Vector3(initialPoint.x, initialPoint.y, initialPoint.z + 1);
        Vector3 p3Mid = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z - 1);

        //Circular Movement
        float angle = 0;
        angle = time * 6;
        float x = Mathf.Cos(angle) * 3f;
        float y = Mathf.Sin(angle) * 3f;

        Vector3 p2 = new Vector3(p2Mid.x - x, p2Mid.y - y, p2Mid.z);
        Vector3 p3 = new Vector3(p3Mid.x + x, p3Mid.y + y, p3Mid.z);

        float timeSub = (1 - time);

        Vector3 part1 = initialPoint * Mathf.Pow(timeSub, 3);
        Vector3 part2 = 3 * Mathf.Pow(timeSub, 2) * time * p2;
        Vector3 part3 = 3 * timeSub * Mathf.Pow(time, 2) * p3;
        Vector3 part4 = Mathf.Pow(time, 3) * hitPoint;

        Vector3 finalPart = part1 + part2 + part3 + part4;
        return finalPart;
    }
}
