using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldParticle : MonoBehaviour
{
    public Transform t;
    public void Update()
    {
        transform.position = t.position;
    }
}
